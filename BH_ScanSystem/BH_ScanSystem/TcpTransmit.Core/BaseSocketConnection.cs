using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using TcpTransmit.Core.System;

namespace TcpTransmit.Core {
    /// <summary>
    /// Socket连接基类
    /// </summary>
    public abstract class BaseSocketConnection : BaseDisposable {
        private static readonly ILog Log = LogManager.GetLogger("BaseSocketConnection");

        #region 字段

        internal readonly object SyncReadPending;

        /// <summary>
        /// ID
        /// </summary>
        private readonly int _id;

        /// <summary>
        /// 是否活动读写锁
        /// </summary>
        private readonly ReaderWriterLockSlim _syncActive;

        /// <summary>
        /// 是否活动
        /// </summary>
        private bool _active;


        /// <summary>
        /// 宿主
        /// </summary>
        private BaseConnectionHost _host;

        /// <summary>
        /// 上次活动时间
        /// </summary>
        private DateTime _lastAction;

        /// <summary>
        /// 读Bytes
        /// </summary>
        private long _readBytes;

        private SocketAsyncEventArgs _readOv;

        /// <summary>
        /// socket
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// 写Bytes
        /// </summary>
        private long _writeBytes;

        #endregion

        #region Constructor

        internal BaseSocketConnection(BaseConnectionHost host, Socket socket) {
            //----- Connection Id!
            _id = host.GetConnectionId();

            _readBytes = 0;
            _writeBytes = 0;

            _host = host;
            _socket = socket;

            _syncActive = new ReaderWriterLockSlim();
            _active = false;


            _readOv = new SocketAsyncEventArgs();


            SyncReadPending = new object();
            ReadPending = false;


            _lastAction = DateTime.Now;

            socket.NoDelay = true;
            _remoteEndPoint = (IPEndPoint) _socket.RemoteEndPoint;
        }

        #endregion

        #region Destructor

        protected override void Free(bool canAccessFinalizable) {
            if (_readOv != null) {
                _readOv.SetBuffer(null, 0, 0);
                _readOv.Dispose();
                _readOv = null;
            }

            if (_socket != null) {
                _socket.Close();
                _socket = null;
            }

            _host = null;

            base.Free(canAccessFinalizable);
        }

        #endregion

        #region Properties

        internal bool ReadPending { get; set; }

        internal SocketAsyncEventArgs ReadOV {
            get { return _readOv; }
        }

        /// <summary>
        /// 连接是否是活动的
        /// </summary>
        internal bool Active {
            get {
                if (disposed)
                    return false;
                return _active;
                _syncActive.EnterReadLock();
                try {
                    return _active;
                }
                finally {
                    _syncActive.ExitReadLock();
                }
            }

            set {
                _syncActive.EnterWriteLock();
                try {
                    _active = value;
                }
                finally {
                    _syncActive.ExitWriteLock();
                }
            }
        }


        internal Socket Socket {
            get { return _socket; }
            set { _socket = value; }
        }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserDate { get; set; }

        internal HostType HostType {
            get { return _host.HostType; }
        }

        #endregion

        #region Methods

        internal void SetConnectionData(int fReadBytes, int fWriteBytes) {
            if (!disposed) {
                if (fReadBytes > 0)
                    Interlocked.Add(ref _readBytes, fReadBytes);


                if (fWriteBytes > 0)
                    Interlocked.Add(ref _writeBytes, fWriteBytes);


                _lastAction = BaseConnectionHost.TimeOfNow;
            }
        }

        #endregion

        #region Properties

        private readonly IPEndPoint _remoteEndPoint;

        /// <summary>
        /// 远程网络端点
        /// </summary>
        public IPEndPoint RemoteEndPoint {
            get { return _remoteEndPoint; }
        }

        /// <summary>
        /// 宿主
        /// </summary>
        public BaseConnectionHost Host {
            get { return _host; }
        }
      
        /// <summary>
        /// 连接Id
        /// </summary>
        public int ConnectionId {
            get { return _id; }
        }

        /// <summary>
        /// 上次动作的时间
        /// </summary>
        public DateTime LastAction {
            get { return _lastAction; }
        }

        /// <summary>
        /// 读的字节数
        /// </summary>
        public long ReadBytes {
            get { return _readBytes; }
        }

        /// <summary>
        /// 写的字节数
        /// </summary>
        public long WriteBytes {
            get { return _writeBytes; }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// 把当前连接转换为客户端连接
        /// </summary>
        /// <returns></returns>
        public abstract IClientConnection AsClientConnection();

        /// <summary>
        /// 把当前连接转换为服务器端连接
        /// </summary>
        /// <returns></returns>
        public abstract IServerConnection AsServerConnection();

        #endregion

        #region BeginSend

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buffer">要发送的数据</param>
        public void BeginSend(byte[] buffer) {
            if (!disposed)
                _host.BeginSend(this, buffer);
        }

        #endregion

        #region BeginReceive

        /// <summary>
        /// 接收数据
        /// </summary>
        public void BeginReceive() {
            if (!disposed)
                _host.BeginReceive(this);
        }

        #endregion

        #region BeginDisconnect

        /// <summary>
        /// 断开连接
        /// </summary>
        public void BeginDisconnect() {
            if (!disposed)
                _host.BeginDisconnect(this);
        }

        /// <summary>
        /// 停止连接活动
        /// </summary>
        public void StopConnection() {
            if (!disposed)
                if (_active) {
                    _syncActive.EnterWriteLock();
                    try {
                        if (_active) {
                            _active = false;

                            _socket.Shutdown(SocketShutdown.Both);
                            _readOv.Completed -= Host.OnReadOvOnCompleted;
                            _readOv.UserToken = null;
                            if (_readOv.Buffer != null) {
                                _readOv.SetBuffer(null,0,0);
                            }
                            _readOv.Dispose();
                            _readOv = null;

                            Host.FireOnDisconnected(this);
                        }
                    }
                    catch (Exception ex) {
                        Log.Error("在关掉连接时发生异常!", ex);
                    }
                    finally {
                        _syncActive.ExitWriteLock();
                    }
                }
        }

        #endregion

        #region GetConnections

        /// <summary>
        /// 得到所有的连接
        /// </summary>
        public BaseSocketConnection[] GetConnections() {
            return disposed ? null : _host.GetSocketConnections();
        }

        #endregion

        #region GetConnectionById

        /// <summary>
        /// 通过连接Id得到连接
        /// </summary>
        /// <param name="connectionId">连接Id</param>
        /// <returns>
        /// 返回一个连接
        /// </returns>
        public BaseSocketConnection GetConnectionById(int connectionId) {
            if (!disposed)
                return _host.GetSocketConnectionById(connectionId);

            return null;
        }

        #endregion
    }
}