using System.Net;

namespace TcpTransmit.Core.Client
{
    /// <summary>
    /// 客户端宿主
    /// </summary>
    public class Host : BaseConnectionHost
    {
        internal Host(IHandler handler, int socketBufferSize, int messageBufferSize, int minPoolsAsyncEventArgsNum, int maxPoolsAsyncEventArgsNum)
            : base(HostType.Client, handler, socketBufferSize, messageBufferSize,minPoolsAsyncEventArgsNum,maxPoolsAsyncEventArgsNum) { }

        internal Host(IHandler handler)
            : this(handler, 1024 * 2, 1024 * 8,2,5)
        {
            SentByServer = false;
        }

        #region Overrides of BaseConnectionHost

        internal override void BeginReconnect(IClientConnection connection)
        {
            var clientConnection = connection as Connection;
            if (clientConnection != null && clientConnection.Connector != null)
            {
                //clientConnection.BeginDisconnect();
                clientConnection.Connector.ReconnectConnection(true, null);
            }
        }

        /// <summary>
        /// 当链接断开时执行此方法重新连接
        /// </summary>
        /// <param name="baseSocketConnection">断开的连接</param>
        public void ReconnectAfterDisconnected(BaseSocketConnection baseSocketConnection)
        {
            BeginReconnect(baseSocketConnection as IClientConnection);
        }

        internal override void BeginSendToAll(IServerConnection connection, byte[] buffer, bool includeMe) { }

        internal override void BeginSendTo(BaseSocketConnection connectionTo, byte[] buffer) { }

        #endregion

        #region AddConnector

        /// <summary>
        /// 添加一个连接创造者
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="reconnectAttempts">最大重连次数</param>
        /// <param name="reconnectAttemptInterval">重连间隔</param>
        /// <param name="localEndPoint">要连接的Ip地址</param>
        /// <returns>连接创造者</returns>
        public SocketConnector AddConnector(string name, int reconnectAttempts, int reconnectAttemptInterval,
                                            IPEndPoint localEndPoint)
        {
            if (disposed) return null;
            var result = new SocketConnector(this, name, localEndPoint, reconnectAttempts, reconnectAttemptInterval);
            AddCreator(result);


            return result;
        }

        /// <summary>
        /// 由默认宿主放送信息
        /// </summary>
        /// <param name="message">要发送的信息</param>
        public void BeginSend(byte[] message)
        {
            if (disposed) return;
            BaseSocketConnection[] connections = GetSocketConnections();
            foreach (BaseSocketConnection connection in connections) BeginSend(connection, message);
        }

        #endregion
    }
}