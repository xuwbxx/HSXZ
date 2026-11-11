using System;
using System.Net;
using System.Threading;
using Interactive.Utility.Reflect;
using TcpTransmit.Core.Configuration.Server;
using TcpTransmit.Core.System;

namespace TcpTransmit.Core.Server{
    /// <summary>
    /// 服务器端宿主
    /// </summary>
    public class Host : BaseConnectionHost{
        private static readonly object defaultHostLock = new object();
        private static Host _defaulHost;

        /// <summary>
        /// 用来定期关闭Socket
        /// </summary>
        private Timer _idleTimer;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handler">事件处理者</param>
        /// <param name="socketBufferSize">socket接收发送缓存大小</param>
        /// <param name="messageBufferSize">处理缓存大小</param>
        /// <param name="idleCheckInterval">检查的时间间隔(毫秒)</param>
        /// <param name="idleTimeOutValue">每个连接的最大休息时间(分钟)</param>
        /// <param name="minPoolsAsyncEventArgsNum">池中AsyncEventArgs最小数量</param>
        /// <param name="maxPoolsAsyncEventArgsNum">池中AsyncEventArgs最大数量</param>
        public Host(IHandler handler, int socketBufferSize, int messageBufferSize, int idleCheckInterval,
                    int idleTimeOutValue, int minPoolsAsyncEventArgsNum, int maxPoolsAsyncEventArgsNum)
            : base(HostType.Server, handler, socketBufferSize, messageBufferSize,minPoolsAsyncEventArgsNum,maxPoolsAsyncEventArgsNum){
            IdleCheckInterval = idleCheckInterval;
            IdleTimeOutValue = idleTimeOutValue;
        }

        public Host(IHandler handler) : this(handler, 1024*2, 1024*4, 0, 0,50,100){
        }

        public Host(IHandler handler, int idleCheckInterval, int idleTimeOutValue, int minPoolsAsyncEventArgsNum, int maxPoolsAsyncEventArgsNum)
            : this(handler, 1024*2, 1024*4, idleCheckInterval, idleTimeOutValue,minPoolsAsyncEventArgsNum,maxPoolsAsyncEventArgsNum){
        }

        #region Overrides of BaseConnectionHost

        internal override void BeginReconnect(IClientConnection connection){
        }

        internal override void BeginSendToAll(IServerConnection connection, byte[] buffer, bool includeMe){
            if (disposed) return;

            BaseSocketConnection[] items = GetSocketConnections();

            if (items != null){
                int loopSleep = 0;

                foreach (BaseSocketConnection cnn in items){
                    if (disposed)
                        break;

                    try{
                        if (includeMe || connection != cnn){
                            var localBuffer = new byte[buffer.Length];
                            Buffer.BlockCopy(buffer, 0, localBuffer, 0, buffer.Length);

                            BeginSend(cnn, localBuffer);
                        }
                    }
                    finally{
                        ThreadEx.LoopSleep(ref loopSleep);
                    }
                }
            }
        }

        internal override void BeginSendTo(BaseSocketConnection connectionTo, byte[] buffer){
            if (disposed) return;
            BeginSend(connectionTo, buffer);
        }

        #endregion

        #region AddListener

        /// <summary>
        /// Add the server connector (SocketListener).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="localEndPoint"></param>
        public SocketListener AddListener(string name, IPEndPoint localEndPoint){
            return AddListener(name, localEndPoint, 5, 2);
        }


        public SocketListener AddListener(string name, IPEndPoint localEndPoint, int backLog,
                                          byte acceptThreads){
            if (disposed) return null;
            var listener = new SocketListener(this, name, localEndPoint, backLog, acceptThreads);
            AddCreator(listener);
            return listener;
        }

        #endregion

        /// <summary>
        /// 默认服务器端宿主
        /// </summary>
        public static Host DefaultHost{
            get{
                if (_defaulHost == null){
                    lock (defaultHostLock){
                        if (_defaulHost == null){
                            TcpTransmitSection configuraSection = TcpTransmitSection.Instance;
                            var handler = Helper.GetObject(configuraSection.Handler.AssemblyName,
                                                           configuraSection.Handler.ClassName) as IHandler;
                            if (handler == null)
                                throw new Exception("配置文件有问题,Ihandle没有或者配置失败!");

                            handler.Init(configuraSection.Handler.Argument);

                            _defaulHost = new Host(handler, configuraSection.SocketBufferSize,
                                                  configuraSection.MessageBufferSize,
                                                  configuraSection.IdleCheckInterval, configuraSection.IdleTimeOutValue,configuraSection.MinPoolsAsyncEventArgsNum,configuraSection.MaxPoolsAsyncEventArgsNum);

                            if (configuraSection.Listeners == null || configuraSection.Listeners.Count < 1)
                                throw new Exception("配置文件有问题,Listener配置失败!");


                            for (int i = 0; i < configuraSection.Listeners.Count; i++){
                                ListenerElement listener = configuraSection.Listeners[i];
                                _defaulHost.AddListener(listener.Name, listener.IpEndPoint, listener.BackLog,
                                                       (byte) listener.AcceptThreads);
                            }
                        }
                    }
                }
                return _defaulHost;
            }
        }

        public int IdleCheckInterval { get; set; }

        public int IdleTimeOutValue { get; set; }

        /// <summary>
        /// 检查连接,目前的算法是判断最后一次使用的时间
        /// </summary>
        /// <param name="stateInfo"></param>
        private void CheckSocketConnections(Object stateInfo){
            if (disposed) return;

            //----- Disable timer event!
            _idleTimer.Change(Timeout.Infinite, Timeout.Infinite);

            try{
                //----- Get connections!
                BaseSocketConnection[] items = GetSocketConnections();

                if (items != null){
                    int loopSleep = 0;

                    foreach (BaseSocketConnection cnn in items){
                        if (disposed)
                            break;

                        try{
                            if (cnn != null){
                                if (DateTime.Now > (cnn.LastAction.AddMinutes(IdleTimeOutValue)))
                                    cnn.BeginDisconnect();
                            }
                        }
                        finally{
                            ThreadEx.LoopSleep(ref loopSleep);
                        }
                    }
                }
            }
            finally{
                if (!disposed){
                    //----- Restart the timer event!
                    _idleTimer.Change(IdleCheckInterval, IdleCheckInterval);
                }
            }
        }

        public override void Start(){
            if ((IdleCheckInterval > 0) && (IdleTimeOutValue > 0))
                _idleTimer = new Timer(CheckSocketConnections);

            if (_idleTimer != null)
                _idleTimer.Change(IdleCheckInterval, IdleCheckInterval);

            base.Start();
        }

        protected override void Free(bool canAccessFinalizable){
            if (_idleTimer != null){
                _idleTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _idleTimer.Dispose();
                _idleTimer = null;
            }

            base.Free(canAccessFinalizable);
        }
    }
}