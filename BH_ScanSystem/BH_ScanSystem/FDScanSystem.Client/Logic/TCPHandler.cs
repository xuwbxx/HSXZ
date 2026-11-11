using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcpTransmit.Core;
using System.Threading;
using TcpTransmit.Core.Client;
using FDScanSystem.Helper;

namespace FDScanSystem.Client.Logic
{
    public class TCPHandler : IHandler
    {

        public static readonly byte[] firstSendBytes;
        private static readonly int length;
        private static readonly TimeSpan ForeverTimeSpan = TimeSpan.FromMilliseconds(-1);
        private static readonly TimeSpan Period = new TimeSpan(0, 0, 5);
        private static readonly Random random = new Random();
        private readonly Timer timer;

        static TCPHandler()
        {

            byte[] bytes = new byte[] { 0x7B, 0x7B, 0x7B };
            firstSendBytes = bytes;
            //Buffer.BlockCopy(bytes, 0, firstSendBytes, 1, bytes.Length);
            //firstSendBytes[0] = 1;

        }

        public TCPHandler()
        {
            //timer = new Timer((ob => CheckConnection()), null, ForeverTimeSpan, ForeverTimeSpan);
        }

        private void CheckConnection()
        {
            try
            {
                timer.Change(ForeverTimeSpan, ForeverTimeSpan);
                var sendBytes = new byte[] { 0x7B, 0x7B, 0x7B };
                HostManager.Instance["Farm"].BeginSend(sendBytes);
            }
            catch (Exception)
            {
                LogWriter.Error("在检查Socket的连接状态时出现问题!");
            }
            finally
            {
                //timer.Change(new TimeSpan(0, 0, random.Next(0, 60)), ForeverTimeSpan);
            }
        }

        #region Implementation of IHandler

        /// <summary>
        /// 自身初始化时用到
        /// </summary>
        /// <param name="argument">初始化的参数</param>
        public void Init(string argument)
        {
        }


        /// <summary>
        /// 当连接时触发
        /// </summary>
        /// <param name="e">
        /// Sock连接
        /// </param>
        public void OnConnected(BaseSocketConnection e)
        {
            if (e != null)
            {
                AppContext.Log(string.Format("与服务器连接成功,服务器IP:{0}", e.RemoteEndPoint.ToString()));
                e.BeginSend(firstSendBytes);
                e.BeginReceive();
                //timer.Change(Period, ForeverTimeSpan);
            }
        }

        /// <summary>
        /// 当接收数据时触发
        /// </summary>
        /// <param name="e">Sock连接</param>
        /// <param name="receiveData">接到到的数据</param>
        public void OnReceived(BaseSocketConnection e, byte[] receiveData)
        {
            var responseMsg = MessageFacade.Provide(e, receiveData);
            if (responseMsg != null)
            {
                e.BeginSend(responseMsg.ToSerializeMsg());
            }
            AppContext.Log(string.Format("收到服务器端发送的消息,消息内容:{0}", Encoding.UTF8.GetString(receiveData)));
        }

        /// <summary>
        /// 当发送完数据时触发
        /// </summary>
        /// <param name="e">Sock连接</param>
        /// <param name="sendServer">是否被服务器端的BeginSendTo和BeginSendToAll触发</param>
        public void OnSent(BaseSocketConnection e, bool sendServer)
        {
            AppContext.Log("成功发送一条消息");
        }

        /// <summary>
        /// 当断开连接时触发
        /// </summary>
        /// <param name="e">Sock连接</param>
        public void OnDisconnected(BaseSocketConnection e)
        {
            if (e != null)
            {
                AppContext.Log("被踢线了,再见!");
                var clientConnection = e as IClientConnection;
                clientConnection.BeginReconnect();
            }
        }

        /// <summary>
        /// 当出现错误时触发
        /// </summary>
        /// <param name="e">
        /// 错误信息
        /// </param>
        public void OnException(ref ExceptionEventArgs e)
        {
            if (e != null)
            {
                AppContext.Log("客户端在通讯的过程中出错了!" + e.Exception.Message);
                if (e.Connection != null)
                {
                    //   e.Connection.BeginSend(new byte[255]);
                    e.Connection.BeginReceive();
                }
            }
        }

        #endregion
    }
}