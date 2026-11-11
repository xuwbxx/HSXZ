using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Interactive.Utility.Collections;
using Interactive.Utility.Tracer;
using log4net;
using TcpTransmit.Core.System;
using TcpTransmit.Core.Utility;

namespace TcpTransmit.Core
{
    /// <summary>
    /// 所有Socket的宿主
    /// </summary>
    public abstract class BaseConnectionHost : BaseDisposable
    {
        #region 字段

        /// <summary>
        /// 最大的传输大小,这里设为64KB
        /// </summary>
        private const int MaxBufferSize = 1024 * 1024;

        private static readonly int[] BufferArray = new[] { 8 * 1024, 16 * 1024, 32 * 1024, 64 * 1024, 128 * 1024, 256 * 1024, 512 * 1024, 1024 * 1024 };

        protected static readonly ILog MyLog = LogManager.GetLogger("ConnectionHost");

        internal static DateTime TimeOfNow = DateTime.Now;
        //定时更新时间的Timer,为性能考虑,不要每次都DateTime.Now
        private static readonly Timer RefreshTimer = new Timer(delegate { TimeOfNow = DateTime.Now; }, null, 1000,
                                                               1000);

        /// <summary>
        /// 是否是服务端
        /// </summary>
        protected static bool SentByServer = true;

        /// <summary>
        /// SocketAsyncEventArgs池
        /// </summary>
        private readonly ObjectPool<SocketAsyncEventArgs> _argsPool;

        /// <summary>
        /// 宿主类型,客户端或服务器端
        /// </summary>
        private readonly HostType _hostType;

        /// <summary>
        /// 所有的连接Creator
        /// </summary>
        private readonly List<BaseConnectionCreator> _socketCreators;

        /// <summary>
        /// 用来同步Active
        /// </summary>
        private readonly object _syncActive;

        /// <summary>
        /// 是否活动
        /// </summary>
        private bool _active;

        /// <summary>
        /// 用来生成客户端连接Id
        /// </summary>
        private int _connectionId;

        /// <summary>
        /// 事件处理者
        /// </summary>
        private IHandler _handler;


        /// <summary>
        /// 所有的连接
        /// </summary>
        private Dictionary<int, BaseSocketConnection> _socketConnections;

        /// <summary>
        /// 连接锁
        /// </summary>
        private ReaderWriterLockSlim _socketConnectionsSync;

        /// <summary>
        /// 等待所有连接释放完毕的信号量
        /// </summary>
        private ManualResetEvent _waitConnectionsDisposing;

        /// <summary>
        /// 等待所有连接创造者释放完毕的信号量
        /// </summary>
        private ManualResetEvent _waitCreatorsDisposing;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hostType">宿主类型</param>
        /// <param name="handler">事件处理者</param>
        /// <param name="socketBufferSize">socket接收发送缓存大小</param>
        /// <param name="messageBufferSize">处理缓存大小</param>       
        /// <param name="minPoolsAsyncEventArgsNum">池中AsyncEventArgs最小数量</param>
        /// <param name="maxPoolsAsyncEventArgsNum">池中AsyncEventArgs最大数量</param>
        protected BaseConnectionHost(HostType hostType, IHandler handler, int socketBufferSize, int messageBufferSize,
                                     int minPoolsAsyncEventArgsNum,
                                     int maxPoolsAsyncEventArgsNum)
        {
            _hostType = hostType;
            _connectionId = 1000;

            _socketConnectionsSync = new ReaderWriterLockSlim();

            _socketConnections = new Dictionary<int, BaseSocketConnection>();
            _socketCreators = new List<BaseConnectionCreator>();


            _handler = handler;

            _waitCreatorsDisposing = new ManualResetEvent(false);
            _waitConnectionsDisposing = new ManualResetEvent(false);


            MessageBufferSize = messageBufferSize;
            SocketBufferSize = socketBufferSize;

            _active = false;
            _syncActive = new Object();


            _argsPool = new ObjectPool<SocketAsyncEventArgs>(minPoolsAsyncEventArgsNum, maxPoolsAsyncEventArgsNum,
                                                             () =>
                                                             {
                                                                 var arg = new SocketAsyncEventArgs();
                                                                 arg.SetBuffer(new byte[messageBufferSize], 0,
                                                                               messageBufferSize);
                                                                 arg.Completed += (OnSendOvOnCompleted);
                                                                 return arg;
                                                             });
        }

        #endregion

        #region 属性

        /// <summary>
        /// 是否活动
        /// </summary>
        public bool Active
        {
            get
            {
                if (disposed)
                    return false;

                lock (_syncActive)
                {
                    return _active;
                }
            }

            internal set
            {
                lock (_syncActive)
                {
                    _active = value;
                }
            }
        }


        /// <summary>
        /// socket接收发送缓存大小
        /// </summary>
        public int SocketBufferSize { get; set; }

        /// <summary>
        /// 处理缓存大小
        /// </summary>
        public int MessageBufferSize { get; set; }

        /// <summary>
        /// 宿主类型
        /// </summary>
        public HostType HostType
        {
            get { return _hostType; }
        }

        #endregion

        #region 开始,结束,释放资源

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="canAccessFinalizable">是否由GC来执行,true代表不是,false代表是</param>
        protected override void Free(bool canAccessFinalizable)
        {
            if (_waitCreatorsDisposing != null)
            {
                _waitCreatorsDisposing.Set();
                _waitCreatorsDisposing.Close();
                _waitCreatorsDisposing = null;
            }

            if (_waitConnectionsDisposing != null)
            {
                _waitConnectionsDisposing.Set();
                _waitConnectionsDisposing.Close();
                _waitConnectionsDisposing = null;
            }


            if (_socketConnections != null)
            {
                _socketConnections.Clear();
                _socketConnections = null;
            }
            if (RefreshTimer != null)
                RefreshTimer.Dispose();


            _socketConnectionsSync = null;
            _handler = null;


            base.Free(canAccessFinalizable);
        }


        /// <summary>
        /// 开始运行,启动宿主
        /// </summary>
        public virtual void Start()
        {
            if (disposed) return;

            //启动Socket创造,如服务器端的TcpLinster
            int loopSleep = 0;

            foreach (BaseConnectionCreator creator in _socketCreators)
            {
                creator.Start();
                ThreadEx.LoopSleep(ref loopSleep);
            }


            Active = true;
        }


        /// <summary>
        /// 停止连接和链接者
        /// </summary>
        public virtual void Stop()
        {
            if (disposed) return;

            Active = false;

            StopConnections();
            StopCreators();
            _argsPool.Dispose();
        }


        /// <summary>
        /// 停掉所有的SocketCreator
        /// </summary>
        protected void StopCreators()
        {
            if (disposed) return;


            //----- Stop Creators!
            BaseConnectionCreator[] creators = GetSocketCreators();

            if (creators != null)
            {
                _waitCreatorsDisposing.Reset();

                int loopCount = 0;

                foreach (BaseConnectionCreator creator in creators)
                    try
                    {
                        creator.Stop();
                    }
                    finally
                    {
                        RemoveCreator(creator);
                        creator.Dispose();

                        ThreadEx.LoopSleep(ref loopCount);
                    }

                if (creators.Length > 0)
                    _waitCreatorsDisposing.WaitOne(Timeout.Infinite, false);
            }
        }


        /// <summary>
        /// 停掉所有连接
        /// </summary>
        protected void StopConnections()
        {
            if (disposed) return;


            //----- Stop Connections!
            BaseSocketConnection[] connections = GetSocketConnections();

            if (connections != null)
            {
                _waitConnectionsDisposing.Reset();

                int loopSleep = 0;

                foreach (BaseSocketConnection connection in connections)
                {
                    connection.BeginDisconnect();
                    ThreadEx.LoopSleep(ref loopSleep);
                }

                if (connections.Length > 0)
                    _waitConnectionsDisposing.WaitOne(Timeout.Infinite, false);
            }
        }

        #region DisposeAndNullConnection

        /// <summary>
        /// 释放一个连接的资源
        /// </summary>
        /// <param name="connection">连接</param>
        internal void DisposeConnection(BaseSocketConnection connection)
        {
            if (disposed) return;

            if (connection != null)
            {
                if (connection.ReadOV != null)
                    if (connection.ReadOV.Buffer != null)
                    {
                        connection.ReadOV.SetBuffer(null, 0, 0);
                    }

                connection.Dispose();
            }
        }

        #endregion

        #endregion

        #region 特殊功能函数

        /// <summary>
        /// 得到连接ID
        /// </summary>
        /// <returns></returns>
        public int GetConnectionId()
        {
            return Interlocked.Increment(ref _connectionId);
        }


        /// <summary>
        /// 将一个连接从集合中移除
        /// </summary>
        /// <param name="socketConnection">要移除的连接</param>
        internal void RemoveSocketConnection(BaseSocketConnection socketConnection)
        {
            if (disposed) return;


            if (socketConnection != null)
            {
                _socketConnectionsSync.EnterWriteLock();

                try
                {
                    _socketConnections.Remove(socketConnection.ConnectionId);
                }
                finally
                {
                    if (_socketConnections.Count <= 0)
                        _waitConnectionsDisposing.Set();

                    _socketConnectionsSync.ExitWriteLock();
                }
            }
        }


        internal void AddSocketConnection(BaseSocketConnection socketConnection)
        {
            if (disposed) return;


            _socketConnectionsSync.EnterWriteLock();

            try
            {
                _socketConnections.Add(socketConnection.ConnectionId, socketConnection);
            }
            finally
            {
                _socketConnectionsSync.ExitWriteLock();
            }
            socketConnection.ReadOV.Completed += (OnReadOvOnCompleted);
        }

        internal void OnReadOvOnCompleted(object sender, SocketAsyncEventArgs e)
        {
            BeginReadCallbackAsync(e);
        }

        /// <summary>
        /// 得到所有的连接
        /// </summary>
        /// <returns>连接集合</returns>
        public BaseSocketConnection[] GetSocketConnections()
        {
            if (disposed) return null;
            BaseSocketConnection[] items;


            _socketConnectionsSync.EnterReadLock();

            try
            {
                items = new BaseSocketConnection[_socketConnections.Count];
                _socketConnections.Values.CopyTo(items, 0);
            }
            finally
            {
                _socketConnectionsSync.ExitReadLock();
            }


            return items;
        }

        /// <summary>
        /// 通过Id得到连接
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>连接</returns>
        public BaseSocketConnection GetSocketConnectionById(int id)
        {
            if (disposed) return null;
            BaseSocketConnection item;

            _socketConnectionsSync.EnterReadLock();

            try
            {
                item = _socketConnections[id];
            }
            finally
            {
                _socketConnectionsSync.ExitReadLock();
            }


            return item;
        }

        /// <summary>
        /// 得到连接创造者
        /// </summary>
        /// <returns>连接创造者数组</returns>
        protected BaseConnectionCreator[] GetSocketCreators()
        {
            if (disposed) return null;
            BaseConnectionCreator[] items;
            lock (_socketCreators)
            {
                items = new BaseConnectionCreator[_socketCreators.Count];
                _socketCreators.CopyTo(items, 0);
            }


            return items;
        }

        /// <summary>
        /// 去掉一个连接创造者
        /// </summary>
        /// <param name="creator">要去掉的连接创造者</param>
        protected void RemoveCreator(BaseConnectionCreator creator)
        {
            if (disposed) return;
            lock (_socketCreators)
            {
                _socketCreators.Remove(creator);

                if (_socketCreators.Count <= 0)
                    _waitCreatorsDisposing.Set();
            }
        }

        /// <summary>
        /// 添加一个连接创造者
        /// </summary>
        /// <param name="creator">链接创造者</param>
        protected void AddCreator(BaseConnectionCreator creator)
        {
            if (disposed) return;

            lock (_socketCreators)
            {
                _socketCreators.Add(creator);
            }
        }

        #endregion

        #region 主要几个处理函数BeginSend,BeginReceive,BeginDisconnect

        /// <summary>
        /// 开始发送数据
        /// </summary>
        /// <param name="connection">连接</param>
        /// <param name="buffer">内容</param>
        internal void BeginSend(BaseSocketConnection connection, byte[] buffer)
        {
            if (disposed) return;
            if ((buffer.Length > MaxBufferSize))
                throw new ArgumentException("包大于512k,拒绝处理!数据包大小：" + buffer.Length);
            try
            {
                if (connection.Active)
                {
                    SocketAsyncEventArgs socketAsyncEventArgs = _argsPool.Get();
                    BufferUtils.GetPacketBuffer(buffer, MessageBufferSize, socketAsyncEventArgs);
                    socketAsyncEventArgs.UserToken = connection;

                    if (connection.Socket != null && !connection.Socket.SendAsync(socketAsyncEventArgs))
                        BeginSendCallbackAsync(socketAsyncEventArgs);
                }
            }
            catch (SocketException soex)
            {
                if (connection == null)
                    MyLog.Error("在发送数据时有问题产生,因为该连接为null!" + "  错误代码:" +
                                soex.ErrorCode, soex);
                else
                {
                    MyLog.Debug("在发送数据时有问题产生,发送不成功!该连接有问题!该连接的信息如下,ip地址" + connection.RemoteEndPoint + "  错误代码:" +
                                soex.ErrorCode, soex);
                    connection.BeginDisconnect();
                }
            }
            catch (Exception ex)
            {
                if (connection == null) MyLog.Error("在发送数据时有问题产生,因为该连接为null!");
                else MyLog.Error("在发送数据时有问题产生,原因未知.ip地址" + connection.RemoteEndPoint, ex);
                FireOnException(connection, ex);
            }
        }

        /// <summary>
        /// 当发送完触发的函数
        /// </summary>
        private void OnSendOvOnCompleted(object sender, SocketAsyncEventArgs e)
        {
            BeginSendCallbackAsync(e);
        }

        /// <summary>
        /// 发送的回调函数
        /// </summary>
        /// <param name="e">异步套接字</param>
        private void BeginSendCallbackAsync(SocketAsyncEventArgs e)
        {
            if (disposed) return;


            BaseSocketConnection connection = null;

            try
            {
                connection = (BaseSocketConnection)e.UserToken;
                if (connection.Active)
                    if (e.SocketError == SocketError.Success)
                    {
                        connection.SetConnectionData(0, e.BytesTransferred);

                        if ((e.Offset + e.BytesTransferred) < e.Count)
                        {
                            //----- Continue to send until all bytes are sent!
                            e.SetBuffer(e.Offset + e.BytesTransferred, e.Count - e.BytesTransferred - e.Offset);
                            if (!connection.Socket.SendAsync(e))
                                BeginSendCallbackAsync(e);
                        }
                        else
                        {
                            e.SetBuffer(0, 0);
                            e.UserToken = null;
                            if (!_argsPool.Set(e))
                            {
                                e.SetBuffer(null, 0, 0);
                                e.Dispose();
                            }

                            FireOnSent(connection, SentByServer);
                        }
                    }
                    else
                    {
                        MyLog.Debug("在发送数据时有问题产生,发送不成功!该连接有问题!该连接的信息如下,ip地址" + connection.RemoteEndPoint + "  错误代码:" +
                                    e.SocketError);
                        connection.BeginDisconnect();
                    }
            }
            catch (SocketException soex)
            {
                if (e != null)
                {
                    try
                    {
                        e.SetBuffer(null, 0, 0);
                        e.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MyLog.Error("在释放SocketAsyncEventArgs时发生问题!", ex);
                    }
                }
                if (connection == null)
                {
                    MyLog.Error("在发送数据时有问题产生,因为该连接为null!" + "  错误代码:" +
                                soex.ErrorCode, soex);
                }
                else
                {
                    MyLog.Debug("在发送数据时有问题产生,该连接有问题!该连接的信息如下,ip地址" + connection.RemoteEndPoint, soex);
                    connection.BeginDisconnect();
                }
            }
            catch (Exception ex)
            {
                if (connection == null)
                    MyLog.Error("在发送数据时有问题产生,因为该连接为null!", ex);
                else
                    MyLog.Error("在发送数据时有问题产生,原因未知.ip地址" + connection.RemoteEndPoint, ex);
                FireOnException(connection, ex);
            }
        }


        /// <summary>
        /// 开始断掉连接
        /// </summary>
        /// <param name="connection">连接</param>
        internal void BeginDisconnect(BaseSocketConnection connection)
        {
            if (disposed) return;
            try
            {
                connection.StopConnection();
            }
            finally
            {
                RemoveSocketConnection(connection);
                DisposeConnection(connection);
            }
        }

        /// <summary>
        /// 从一个连接接收数据
        /// </summary>
        /// <param name="connection">连接</param>
        internal void BeginReceive(BaseSocketConnection connection)
        {
            if (disposed) return;


            byte[] readMessage;

            try
            {
                if (connection.Active)
                {
                    bool completedAsync = true;


                    if (connection.ReadPending)
                        return;

                    lock (connection.SyncReadPending)
                    {
                        if (!connection.ReadPending)
                        {
                            readMessage = new byte[MessageBufferSize];
                            connection.ReadOV.SetBuffer(readMessage, 0, readMessage.Length);
                            connection.ReadOV.UserToken = connection;
                            completedAsync = connection.Socket.ReceiveAsync(connection.ReadOV);
                            connection.ReadPending = true;
                        }
                    }

                    if (!completedAsync)
                        BeginReadCallbackAsync(connection.ReadOV);
                }
            }
            catch (SocketException soex)
            {
                if (connection == null)
                {
                    MyLog.Error("在接收数据时有问题产生,因为该连接为null!" + "  错误代码:" +
                                soex.ErrorCode, soex);
                }
                else
                {
                    MyLog.Debug("在接收数据时有问题产生,该连接有问题!该连接的信息如下,ip地址" + connection.RemoteEndPoint, soex);
                    connection.BeginDisconnect();
                }
            }
            catch (Exception ex)
            {
                if (connection == null)
                    MyLog.Error("在接收数据时有问题产生,因为该连接为null!", ex);
                else
                    MyLog.Error("在接收数据时有问题产生,原因未知.ip地址" + connection.RemoteEndPoint, ex);
                FireOnException(connection, ex);
            }
        }

        #region BeginReadCallbackAsync

        /// <summary>
        /// 异步读取数据(回调函数)
        /// </summary>
        /// <param name="e">异步套接字</param>
        private void BeginReadCallbackAsync(SocketAsyncEventArgs e)
        {
            if (disposed) return;

            BaseSocketConnection connection = null;

            try
            {
                connection = (BaseSocketConnection)e.UserToken;

                if (connection.Active)
                    if (e.SocketError == SocketError.Success)
                    {
                        if (e.BytesTransferred > 0)
                        {
                            ReadFromConnection(connection, e.BytesTransferred);
                            connection.SetConnectionData(e.BytesTransferred, 0);
                        }
                        else connection.BeginDisconnect();
                    }
                    else
                    {
                        MyLog.Debug("在接收数据回调时有问题产生,接收时,该连接有问题!该连接的信息如下,ip地址" + connection.RemoteEndPoint + "  错误代码:" +
                                    e.SocketError);
                        connection.BeginDisconnect();
                    }
            }
            catch (SocketException soex)
            {
                if (connection == null)
                {
                    MyLog.Error("在接收数据时有问题产生,因为该连接为null!" + "  错误代码:" +
                                soex.ErrorCode, soex);
                }
                else
                {
                    MyLog.Debug("在接收数据时有问题产生,该连接有问题!该连接的信息如下,ip地址" + connection.RemoteEndPoint, soex);
                    connection.BeginDisconnect();
                }
            }
            catch (Exception ex)
            {
                if (connection == null)
                    MyLog.Error("在接收数据时有问题产生,因为该连接为null!");
                else
                    MyLog.Error("在接收数据时有问题产生,原因未知.ip地址" + connection.RemoteEndPoint, ex);
                FireOnException(connection, ex);
            }
        }

        private static int GetNewSize(int remain)
        {
            return BufferArray.FirstOrDefault(bufferSize => bufferSize > remain);
        }


        /// <summary>
        /// 链接是否断掉
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static bool IsSocketConnectionClose(SocketAsyncEventArgs e)
        {
            return ((e.SocketError == SocketError.ConnectionReset)
                    || (e.SocketError == SocketError.NotConnected)
                    || (e.SocketError == SocketError.Shutdown)
                    || (e.SocketError == SocketError.ConnectionAborted)
                    || (e.SocketError == SocketError.Disconnecting) || (e.SocketError == SocketError.TimedOut) ||
                    (e.SocketError == SocketError.OperationAborted));
        }

        #endregion

        #region ReadFromConnection

        /// <summary>
        /// 从连接中获取数据
        /// </summary>
        /// <param name="connection">连接</param>
        /// <param name="readBytes">要读多少</param>
        private void ReadFromConnection(BaseSocketConnection connection, int readBytes)
        {
            bool onePacketFound = false;
            SocketAsyncEventArgs e = connection.ReadOV;

            //剩下的字符串
            int remainingBytes = ReadMessageWithOutTail(connection, e, readBytes, ref onePacketFound);

            if (remainingBytes == 0)
            {
                e.SetBuffer(0, e.Buffer.Length);
            }
            else if (!onePacketFound) //如果剩余的大小够100字节,继续让他去读
                if ((e.Buffer.Length - remainingBytes - e.Offset) > 500)
                    e.SetBuffer(e.Offset + remainingBytes, e.Buffer.Length - remainingBytes - e.Offset);
                else
                {
                    int newBufferLength = e.Buffer.Length * 2;
                    if (newBufferLength > MaxBufferSize)
                    {
                        e.SetBuffer(null, 0, 0);
                        connection.BeginDisconnect();
                        throw new Exception("大于64Kb,俺处理不了,何必了,何苦了!");
                    }
                    var readMessage = new byte[newBufferLength];
                    Buffer.BlockCopy(e.Buffer, 0, readMessage, 0, remainingBytes + e.Offset);
                    e.SetBuffer(readMessage, e.Offset + remainingBytes,
                                readMessage.Length - remainingBytes - e.Offset);
                }
            else
            {
                if (remainingBytes <= 0)
                    MyLog.Error("真的有负数吗?" + remainingBytes);
                var readMessage = new byte[GetNewSize(remainingBytes)];
                Buffer.BlockCopy(e.Buffer, e.Offset, readMessage, 0, remainingBytes);

                e.SetBuffer(readMessage, remainingBytes, readMessage.Length - remainingBytes);
            }

            if (connection.Active)
            {
                //----- Read!
                bool completedAsync = connection.Socket.ReceiveAsync(e);


                if (!completedAsync)
                    BeginReadCallbackAsync(e);
            }
        }

        #endregion

        #region ReadMessageWithOutTail

        /// <summary>
        /// 读取信息,如果并不能读完,则返回有多少个是独立的片段
        /// </summary>
        /// <param name="connection">连接</param>
        /// <param name="e">异步参数</param>
        /// <param name="readBytes">总共读了多少</param>
        /// <param name="onePacketFound">是否有一个完整Packet</param>
        /// <returns>还剩多少个字节是独立的片段</returns>
        private int ReadMessageWithOutTail(BaseSocketConnection connection, SocketAsyncEventArgs e, int readBytes,
                                           ref bool onePacketFound)
        {
            byte[] delimiter = BufferUtils.Delimiter;
            int delimiterSize = BufferUtils.DelimiterLength;

            bool readPacket;

            //还剩下多少字节
            int remainingBytes = readBytes;

            int bufferLength = e.Buffer.Length;
            byte[] buffer = e.Buffer;

            //找包的起始地址
            int offsetToFind;
            //要找的数量
            int findCount;
            //偏移量
            int offsetNum;
            if (e.Offset > 0)
            {
                if (e.Offset < 3)
                {
                    offsetToFind = 0;
                    findCount = remainingBytes + e.Offset;
                    offsetNum = 0;
                }
                else
                {
                    offsetToFind = e.Offset - delimiterSize + 1;
                    findCount = remainingBytes + delimiterSize - 1;
                    offsetNum = offsetToFind;
                }
            }
            else
            {
                offsetToFind = 0;
                findCount = remainingBytes;
                offsetNum = 0;
            }

            int offsetBuffer = 0;
            int totalToFind = findCount;


            do
            {

                bool packetFound = false;
                if (delimiter.Length > 0)
                {
                    if (totalToFind - offsetToFind + offsetNum < 3) break;
                    //杨军2010年3月19日做优化,原因是采用BufferManager后,数据其实是共享的,可能会存在问题
                    // 在这里优化了查找的范围,查找的总范围为totalToFind,每一次要查找的起始地址offsetToFind
                    // 数量为remainingBytes,但是有一点要注意,第一次的remainingBytes可能会多减一个3
                    while (offsetToFind < totalToFind + offsetNum)
                    {
                        offsetToFind = Array.IndexOf(buffer, delimiter[0], offsetToFind, findCount);
                        if (totalToFind - offsetToFind + offsetNum < 3) break;

                        if (offsetToFind == -1) break;

                        #region 如果以后的分割数组由用户自定义的话这里要打开

                        /* if (delimiterSize == 1){
                        offsetToFind++;
                        packetFound = true;
                        break;
                    }*/

                        #endregion

                        packetFound = true;
                        for (int i = 1; i < delimiterSize; i++)
                        {
                            offsetToFind++;

                            if (buffer[offsetToFind] == delimiter[i])
                                continue;
                            packetFound = false;
                            break;
                        }

                        if (packetFound)
                            break;
                    }
                }
                else
                {
                    offsetToFind = readBytes;
                    packetFound = true;
                }

                if (packetFound)
                {
                    onePacketFound = true;

                    byte[] rawBuffer = BufferUtils.GetRawBufferWithTail(connection, e, offsetToFind,
                                                                        offsetBuffer);



                    remainingBytes -= (offsetToFind - e.Offset);
                    offsetToFind += 1;
                    if (remainingBytes == 1)
                        remainingBytes = 0;
                    readPacket = remainingBytes > 0;
                    if (readPacket)
                    {
                        findCount = remainingBytes;
                        e.SetBuffer(offsetToFind, bufferLength - offsetToFind);
                        offsetBuffer = offsetToFind;
                    }
                    FireOnReceived(connection, rawBuffer);
                }
                else
                    readPacket = false;
            } while (readPacket);

            return remainingBytes;
        }

        #endregion

        #endregion

        #region 事件,提供给用户使用

        #region FireOnSent

        private void FireOnSent(BaseSocketConnection connection, bool sentByServer)
        {
            if (disposed) return;


            if (connection.Active)
            {
#if DEBUG
                ITracer trace = TraceManager.GetTracer("FireOnSent");

#endif
                try
                {
                    _handler.OnSent(connection, sentByServer);
                }
                catch (Exception exception)
                {
                    MyLog.Error("在处理发送事件时出错", exception);
                }
#if DEBUG
                finally
                {
                    trace.Check(() => "发送超时", 100);
                }

#endif
            }
        }

        #endregion

        #region FireOnReceived

        private void FireOnReceived(BaseSocketConnection connection, byte[] buffer)
        {
            if (disposed) return;

            if (connection.Active)
            {
#if DEBUG
                ITracer trace = TraceManager.GetTracer("FireOnReceived");

#endif
                try
                {
                    _handler.OnReceived(connection, buffer);
                }
                catch (Exception exception)
                {
                    MyLog.Error("在处理接收事件时出错", exception);
                }
#if DEBUG
                finally
                {
                    trace.Check(() => "接收超时", 200);
                }
#endif
            }
        }

        #endregion

        #region FireOnDisconnected

        internal void FireOnDisconnected(BaseSocketConnection connection)
        {
            if (disposed) return;
#if DEBUG
            ITracer trace = TraceManager.GetTracer("FireOnDisconnected");

#endif

            try
            {
                _handler.OnDisconnected(connection);
            }
            catch (Exception exception)
            {
                MyLog.Error("在处理断开事件时出错", exception);
            }
#if DEBUG
            finally
            {
                trace.Check(() => "断开超时", 100);
            }
#endif
        }


        internal void FireOnException(BaseSocketConnection connection, Exception ex)
        {
            if (disposed) return;

#if DEBUG
            ITracer trace = TraceManager.GetTracer("FireOnException");

#endif
            try
            {
                var exceptionEventArgs = new ExceptionEventArgs(connection, ex);
                _handler.OnException(ref exceptionEventArgs);
                if (connection.Active)
                {
                    if (!exceptionEventArgs.IsCloseConnection)
                    {
                        connection.BeginReceive();
                    }
                    else
                    {
                        connection.BeginDisconnect();
                    }
                }
            }
            catch (Exception exception)
            {
                MyLog.Error("在处理异常事件时出错", exception);
            }
#if DEBUG
            finally
            {
                trace.Check(() => "异常超时", 50);
            }
#endif
        }


        /// <summary>
        /// 连接上执行的时间
        /// </summary>
        /// <param name="connection"></param>
        internal void FireOnConnected(BaseSocketConnection connection)
        {
            if (disposed) return;


            if (connection.Active)
            {
#if DEBUG
                ITracer trace = TraceManager.GetTracer("FireOnConnected");

#endif
                try
                {
                    _handler.OnConnected(connection);
                }
                catch (Exception exception)
                {
                    MyLog.Error("在处理连接事件时出错", exception);
                }
#if DEBUG
                finally
                {
                    trace.Check(() => "连接超时", 100);
                }
#endif
            }
        }

        #endregion

        #endregion

        #region Abstract Methods

        internal abstract void BeginReconnect(IClientConnection connection);
        internal abstract void BeginSendToAll(IServerConnection connection, byte[] buffer, bool includeMe);
        internal abstract void BeginSendTo(BaseSocketConnection connectionTo, byte[] buffer);

        #endregion
    }
}