namespace TcpTransmit.Core
{
    /// <summary>
    /// 事件处理
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// 自身初始化时用到
        /// </summary>
        /// <param name="argument">初始化的参数</param>
        void Init(string argument);

        /// <summary>
        /// 当连接时触发
        /// </summary>
        /// <param name="e">
        /// Sock连接
        /// </param>
        void OnConnected(BaseSocketConnection e);

        /// <summary>
        /// 当接收数据时触发
        /// </summary>
        /// <param name="e">Sock连接</param>
        /// <param name="receiveData">接到到的数据</param>
        void OnReceived(BaseSocketConnection e,byte[] receiveData);

        /// <summary>
        /// 当发送完数据时触发
        /// </summary>
        /// <param name="e">Sock连接</param>
        /// <param name="sendServer">是否被服务器端的BeginSendTo和BeginSendToAll触发</param>
        void OnSent(BaseSocketConnection e,bool sendServer);

        /// <summary>
        /// 当断开连接时触发
        /// </summary>
        /// <param name="e">Sock连接</param>
        void OnDisconnected(BaseSocketConnection e);

        /// <summary>
        /// 当出现错误时触发
        /// </summary>
        /// <param name="e">
        /// 错误信息
        /// </param>
        void OnException(ref  ExceptionEventArgs e);
    }
}