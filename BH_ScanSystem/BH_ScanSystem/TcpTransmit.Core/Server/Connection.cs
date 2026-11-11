using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace TcpTransmit.Core.Server {
    /// <summary>
    /// 服务器端Socket连接
    /// </summary>
    public class Connection : BaseSocketConnection, IServerConnection {
        private static readonly byte[] KeepAliveInOptionValues;

        static Connection() {
            const uint dummy = 0;
            KeepAliveInOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(KeepAliveInOptionValues, 0);
            BitConverter.GetBytes((uint)5000).CopyTo(KeepAliveInOptionValues, Marshal.SizeOf(dummy));
            BitConverter.GetBytes((uint)100).CopyTo(KeepAliveInOptionValues, Marshal.SizeOf(dummy) * 2);
        }

        public Connection(BaseConnectionHost host, Socket socket)
            : base(host, socket) {
            socket.IOControl(IOControlCode.KeepAliveValues, KeepAliveInOptionValues, null);
        }

        #region Overrides of BaseSocketConnection

        /// <summary>
        /// 把当前连接转换为客户端连接
        /// </summary>
        /// <returns></returns>
        public override IClientConnection AsClientConnection() {
            return null;
        }

        /// <summary>
        /// 把当前连接转换为服务器端连接
        /// </summary>
        /// <returns></returns>
        public override IServerConnection AsServerConnection() {
            return this;
        }

        #endregion

        #region Implementation of IServerConnection

        /// <summary>
        /// 向所有人发送信息
        /// </summary>
        /// <param name="buffer">要发送的信息</param>
        /// <param name="includeMe">是否包含自己</param>
        public void BeginSendToAll(byte[] buffer, bool includeMe) {
            Host.BeginSendToAll(this, buffer, includeMe);
        }

        /// <summary>
        /// 向一个连接发送信息
        /// </summary>
        /// <param name="connection">连接</param>
        /// <param name="buffer">要发送的信息</param>
        public void BeginSendTo(BaseSocketConnection connection, byte[] buffer) {
            Host.BeginSendTo(connection, buffer);
        }

        #endregion
    }
}