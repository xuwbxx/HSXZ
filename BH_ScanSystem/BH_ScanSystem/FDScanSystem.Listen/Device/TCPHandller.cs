using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcpTransmit.Core;
using FDScanSystem.Listen.Logic;

using FDScanSystem.Listen.Analysis;

using FDScanSystem.Listen;
using FDScanSystem.DAO;

namespace FDScanSystem.Listen.Device
{
    public class TCPHandller : IHandler
    {
        public TCPHandller() { }

        public void Init(string argument)
        {

        }

        public void OnConnected(BaseSocketConnection e)
        {
            e.BeginReceive();
            AppContext.Log(string.Format("创建一个新的连接.链接信息:[远程地址:{0} ;]", e.RemoteEndPoint.ToString()));
            try
            {

                string ip = e.RemoteEndPoint.Address.ToString();
                int port = e.RemoteEndPoint.Port;
                var db = DataContextFactory.Create();
                bool isInsert = false;
                var queryEntity = db.FD_Connection_Log.FirstOrDefault(u => u.RemoteEndPoint == ip);

                if (queryEntity == null)
                {
                    queryEntity = new FD_Connection_Log();
                    isInsert = true;
                }

                queryEntity.ConnectionDate = DateTime.Now;
                queryEntity.RemoteEndPoint = ip;// e.RemoteEndPoint.Address.ToString();
                queryEntity.Status = "连接";
                queryEntity.DisConnectDate = DateTime.Now;
                if (isInsert)
                    db.FD_Connection_Log.InsertOnSubmit(queryEntity);
                db.SubmitChanges();


                ConnectionInitManager.InitializeConnection(e);
            }
            catch (Exception ex)
            {
                AppContext.Error(ex);
            }
        }

        public void OnReceived(BaseSocketConnection e, byte[] receiveData)
        {
            AppContext.Log(string.Format("收到一条新的消息.链接信息:[远程地址:{0} ; ; 数据长度:{1}]", e.RemoteEndPoint.ToString(), receiveData.Length));
            try
            {
                var provider = AnalysisProtocolFactory.Create(e);
                if (provider != null)
                    provider.OnReceived(e, receiveData);
            }
            catch (Exception ex)
            {
                AppContext.Error(ex);
            }
        }

        public void OnSent(BaseSocketConnection e, bool sendServer)
        {
            AppContext.Log("成功发送一条数据");
        }

        public void OnDisconnected(BaseSocketConnection e)
        {
            AppContext.Log(string.Format("一条链接断开.链接信息:[远程地址:{0} ;  ]", e.RemoteEndPoint.ToString()));
            try
            {
                string ip = e.RemoteEndPoint.Address.ToString();
                int port = e.RemoteEndPoint.Port;
                var db = DataContextFactory.Create();
                var queryEntity = db.FD_Connection_Log.FirstOrDefault(u => u.RemoteEndPoint == ip);
                if (queryEntity != null)
                {
                    queryEntity.Status = "断开";//链接断开
                    queryEntity.DisConnectDate = DateTime.Now;
                    db.SubmitChanges();
                }

                var provider = AnalysisProtocolFactory.Create(e);
                if (provider != null)
                    provider.OnDisconnected(e);

            }
            catch (Exception ex)
            {
                AppContext.Error(ex);
            }
        }

        public void OnException(ref ExceptionEventArgs e)
        {

            AppContext.Error(e.Exception);
        }
    }
}
