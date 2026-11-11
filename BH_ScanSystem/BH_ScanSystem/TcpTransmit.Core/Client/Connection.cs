using System.Net.Sockets;

namespace TcpTransmit.Core.Client {
    public class Connection : BaseSocketConnection, IClientConnection {
        public Connection(BaseConnectionHost host, SocketConnector connector, Socket socket) : base(host, socket){
            Connector = connector;
        }

        #region Overrides of BaseSocketConnection

        /// <summary>
        /// 把当前连接转换为客户端连接
        /// </summary>
        /// <returns></returns>
        public override IClientConnection AsClientConnection(){
            return this;
        }

        /// <summary>
        /// 把当前连接转换为服务器端连接
        /// </summary>
        /// <returns></returns>
        public override IServerConnection AsServerConnection(){
            return null;
        }

        /// <summary>
        /// 开始重新连接
        /// </summary>
        public void BeginReconnect(){
            Host.BeginReconnect(this);
        }

        #endregion

        public SocketConnector Connector { get; private set; }
    }
}