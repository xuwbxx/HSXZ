using System;
using System.Net;
using System.Net.Sockets;
using log4net;
using TcpTransmit.Core.System;

namespace TcpTransmit.Core.Server {
    /// <summary>
    /// 服务器端Socket监听者
    /// </summary>
    public class SocketListener : BaseConnectionCreator {
        private static readonly ILog Log = LogManager.GetLogger("SocketListener");

        #region Overrides of BaseConnectionCreator

        public override void Start() {
            if (disposed) return;
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(LocalEndPoint);
            Socket.Listen(BackLog * AcceptThreads);

            //----- Begin accept new connections!
            int loopCount = 0;
            SocketAsyncEventArgs e;

            for (int i = 1; i <= AcceptThreads; i++) {
                e = new SocketAsyncEventArgs { UserToken = this };
                e.Completed += BeginAcceptCallbackAsync;

                if (!Socket.AcceptAsync(e))
                    BeginAcceptCallbackAsync(this, e);

                ThreadEx.LoopSleep(ref loopCount);
            }
        }

        public override void Stop() {
            Dispose();
        }

        #endregion

        public SocketListener(BaseConnectionHost host, string name, IPEndPoint localEndPoint, int backLog,
                              byte acceptThreads)
            : base(host, name, localEndPoint) {
            BackLog = backLog;
            AcceptThreads = acceptThreads;
        }

        protected override void Free(bool canAccessFinalizable) {
            if (Socket != null) {
                Socket.Close();
                Socket = null;
            }

            base.Free(canAccessFinalizable);
        }

        //internal void BeginAcceptCallbackAsync(object sender, SocketAsyncEventArgs e){
        //    ThreadPool.QueueUserWorkItem(BeginAcceptCallback, e);
        //}

        /// <summary>
        /// 回调函数,增加连接
        /// </summary>
        internal void BeginAcceptCallbackAsync(object sender, SocketAsyncEventArgs e) {
            if (disposed) return;

            Socket acceptedSocket;
            BaseSocketConnection connection = null;

            var listener = (SocketListener)e.UserToken;

            if (e.SocketError == SocketError.Success) {
                try {
                    //----- Get accepted socket!
                    acceptedSocket = e.AcceptSocket;

                    //----- Adjust buffer size!
                    acceptedSocket.ReceiveBufferSize = Host.SocketBufferSize;
                    acceptedSocket.SendBufferSize = Host.SocketBufferSize;
                    acceptedSocket.SendTimeout = 1000;
                    acceptedSocket.ReceiveTimeout = 1000;

                    connection = new Connection(Host, acceptedSocket);

                    //----- Initialize!
                    Host.AddSocketConnection(connection);
                    connection.Active = true;

                    Host.FireOnConnected(connection);
                }
                catch (Exception ex) {
                    if (connection != null) {
                        if (Host != null) {
                            Host.DisposeConnection(connection);
                            Host.RemoveSocketConnection(connection);
                        }
                    }
                    Log.Error("在接收socket的时候产生问题!", ex);
                }
            }
            else {
                Log.Fatal("在接收socket的时候产生问题,为false!");
            }


            e.AcceptSocket = null;
            e.RemoteEndPoint = null;

            if (!listener.disposed && listener.Socket != null && !listener.Socket.AcceptAsync(e))
                BeginAcceptCallbackAsync(this, e);

        }

        #region Properties

        public int BackLog { get; set; }

        public byte AcceptThreads { get; set; }

        internal Socket Socket { get; private set; }

        #endregion
    }
}