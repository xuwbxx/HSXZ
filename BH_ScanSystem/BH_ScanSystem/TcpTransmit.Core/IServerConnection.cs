namespace TcpTransmit.Core {
    /// <summary>
    /// 服务器端Socket特有的特性
    /// </summary>
    public interface IServerConnection  {
        /// <summary>
        /// 向所有人发送信息
        /// </summary>
        /// <param name="buffer">要发送的信息</param>
        /// <param name="includeMe">是否包含自己</param>
        void BeginSendToAll(byte[] buffer, bool includeMe);

        /// <summary>
        /// 向一个连接发送信息
        /// </summary>
        /// <param name="connection">连接</param>
        /// <param name="buffer">要发送的信息</param>
        void BeginSendTo(BaseSocketConnection connection, byte[] buffer);
    }
}