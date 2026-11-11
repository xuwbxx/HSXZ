namespace TcpTransmit.Core {
    /// <summary>
    /// 客户端Socket特有特性
    /// </summary>
    public interface IClientConnection {
        /// <summary>
        /// 开始重新连接
        /// </summary>
        void BeginReconnect();
    }
}