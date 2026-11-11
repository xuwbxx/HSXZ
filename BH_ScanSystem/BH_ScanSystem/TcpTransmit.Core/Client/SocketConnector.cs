using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpTransmit.Core.Client {
    /// <summary>
    /// 客户端连接者(简单实现),客户端仅仅有一个socket
    /// 欢迎大家扩展
    /// </summary>
    public class SocketConnector : BaseConnectionCreator {
        private static readonly IPEndPoint internalLocalEndPoint = new IPEndPoint(IPAddress.Any, 0);
        private int reconnectAttempted;
        private Timer reconnectTimer;

        public SocketConnector(BaseConnectionHost host, string name, IPEndPoint localEndPoint, int reconnectAttempts,
                               int reconnectAttemptInterval)
            : base(host, name, localEndPoint) {
            ReconnectAttempts = reconnectAttempts;
            ReconnectAttemptInterval = reconnectAttemptInterval;
            reconnectTimer = new Timer(ReconnectConnectionTimerCallBack);
            reconnectAttempted = 0;
        }

        #region Overrides of BaseConnectionCreator

        public override void Start() {
            if (disposed) return;
            BeginConnect();
        }

        #endregion

        #region Stop

        public override void Stop() {
            Dispose();
        }

        #endregion

        protected override void Free(bool canAccessFinalizable) {
            if (reconnectTimer != null) {
                reconnectTimer.Dispose();
                reconnectTimer = null;
            }

            if (Socket != null) {
                Socket.Close();
                Socket = null;
            }

            base.Free(canAccessFinalizable);
        }

        #region BeginConnect

        /// <summary>
        /// Begin the connection with host.
        /// </summary>
        internal void BeginConnect() {
            if (disposed)
                return;


            //----- Create Socket!
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(internalLocalEndPoint);
            Socket.ReceiveBufferSize = Host.SocketBufferSize;
            Socket.SendBufferSize = Host.SocketBufferSize;


            reconnectTimer.Change(Timeout.Infinite, Timeout.Infinite);

            var e = new SocketAsyncEventArgs();
            e.RemoteEndPoint = LocalEndPoint;
            e.Completed += BeginConnectCallbackAsync;
            e.UserToken = this;


            if (!Socket.ConnectAsync(e))
                BeginConnectCallbackAsync(this, e);
        }

        #endregion

        #region BeginConnectCallbackAsync

        /// <summary>
        /// 连接的回调函数
        /// </summary>
        internal void BeginConnectCallbackAsync(object sender, SocketAsyncEventArgs e) {
            if (disposed) return;

            BaseSocketConnection connection = null;
            SocketConnector connector;
            Exception exception = null;

            if (e.SocketError == SocketError.Success) {
                try {
                    connector = (SocketConnector)e.UserToken;

                    connection = new Connection(Host, connector, connector.Socket);

                    //----- Adjust buffer size!
                    connector.Socket.ReceiveBufferSize = Host.SocketBufferSize;
                    connector.Socket.SendBufferSize = Host.SocketBufferSize;

                    //----- Initialize!
                    Host.AddSocketConnection(connection);
                    connection.Active = true;

                    Host.FireOnConnected(connection);
                }
                catch (Exception ex) {
                    exception = ex;

                    if (connection != null) {
                        Host.DisposeConnection(connection);
                        Host.RemoveSocketConnection(connection);

                        connection = null;
                    }
                }
            }
            else
                exception = new SocketException((int)e.SocketError);

            if (exception != null) {
                reconnectAttempted++;
                ReconnectConnection(false, exception);
            }


            e.UserToken = null;
            e.Dispose();
            e = null;
        }

        #endregion

        #region ReconnectConnection

        /// <summary>
        /// 从新连接
        /// </summary>
        /// <param name="resetAttempts">是否从新开始</param>
        /// <param name="ex">上一次连接的错误</param>
        internal void ReconnectConnection(bool resetAttempts, Exception ex) {
            if (disposed) return;


            if (resetAttempts) {
                //----- Reset counter and start new connect!
                reconnectAttempted = 0;
                reconnectTimer.Change(ReconnectAttemptInterval, Timeout.Infinite);
            }
            else {
                //----- Check attempt count!
                if (ReconnectAttempts > 0) {
                    if (reconnectAttempted < ReconnectAttempts) {
                        Host.FireOnException(null,
                                             new ReconnectAttemptException("重新连接", this, ex,
                                                                           reconnectAttempted, false));
                        reconnectTimer.Change(ReconnectAttemptInterval, Timeout.Infinite);
                    }
                    else {
                        Host.FireOnException(null,
                                             new ReconnectAttemptException("重新连接", this, ex,
                                                                           reconnectAttempted, true));
                    }
                }
                else {
                    reconnectTimer.Change(ReconnectAttemptInterval, Timeout.Infinite);
                }
                //else{
                //    Host.FireOnException(null,
                //                         new ReconnectAttemptException("重新连接,最大次数小于0!", this, ex,
                //                                                       reconnectAttempted, true));
                //}
            }
        }

        #endregion

        #region ReconnectConnectionTimerCallBack

        /// <summary>
        /// 连接回传
        /// </summary>
        /// <param name="stateInfo"></param>
        private void ReconnectConnectionTimerCallBack(Object stateInfo) {
            if (disposed) return;
            reconnectTimer.Change(Timeout.Infinite, Timeout.Infinite);
            BeginConnect();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 最大尝试连接次数
        /// </summary>
        public int ReconnectAttempts { get; set; }

        /// <summary>
        /// 连接间隔
        /// </summary>
        public int ReconnectAttemptInterval { get; set; }

        /// <summary>
        /// Socket
        /// </summary>
        internal Socket Socket { get; private set; }

        #endregion
    }
}