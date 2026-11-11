using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FDScanSystem.Server.Job;

using FDScanSystem.Server.Analysis;
using FDScanSystem.Server;


namespace FDScanSystem.Server.Device
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
                //var db = DataContextFactory.Create();
                //var queryEntity = db.XPD_Current_Communication.FirstOrDefault(u => u.IP == ip && u.Port == port);
                //if (queryEntity == null) {
                //    queryEntity = new XPD_Current_Communication();
                //    queryEntity.CommunicationIden = e.LocalEndPoint.Port.ToString();
                //    queryEntity.ConnectDate = DateTime.Now;
                //    queryEntity.IP = ip;
                //    queryEntity.Port = port;
                //    queryEntity.Status = 0;//初始化
                //    db.XPD_Current_Communication.InsertOnSubmit(queryEntity);
                //    db.SubmitChanges();
                //}

                //ConnectionInitManager.InitializeConnection(e);
            }
            catch (Exception ex)
            {
                AppContext.Error(ex);
            }
        }

        public void OnReceived(BaseSocketConnection e, byte[] receiveData)
        {
            AppContext.Log(string.Format("收到一条新的消息.链接信息:[远程地址:{0} ;  数据长度:{1}]", e.RemoteEndPoint.ToString(), receiveData.Length));
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

        }

        public void OnDisconnected(BaseSocketConnection e)
        {
            AppContext.Log(string.Format("一条链接断开.链接信息:[远程地址:{0} ; ]", e.RemoteEndPoint.ToString()));
            try
            {
                //TODO : provide offline logic
                //string ip = e.RemoteEndPoint.Address.ToString();
                //int port = e.RemoteEndPoint.Port;
                //var db = DataContextFactory.CreateServerDB();
                //var queryEntity = db.MK_Current_Communication.FirstOrDefault(u => u.IP == ip && u.Port == port);
                //if (queryEntity != null) {
                //    queryEntity.Status = -1;//链接断开
                //    queryEntity.LastAction = DateTime.Now;
                //    db.SubmitChanges();
                //}

                //var provider = AnalysisProtocolFactory.Create(e);
                //provider.OnDisconnected(e);

                //   ConnectionInitManager.Disconnect(e);
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
