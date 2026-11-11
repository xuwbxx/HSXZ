using FDScanSystem.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcpTransmit.Core;

namespace FDScanSystem.Listen.Device
{
    public class ConnectionInitManager
    {
        public static void InitializeConnection(BaseSocketConnection e)
        {
            string ip = e.RemoteEndPoint.Address.ToString();

            int port = e.RemoteEndPoint.Port;

            string key = e.RemoteEndPoint.ToString();
            try
            {
                if (DeviceConnectionManager.GetInstance().ActiveLock())
                {
                    var db = DataContextFactory.Create();
                    var communicationInfo = db.FD_CommunicationDevice.Where(u => u.IP == ip).FirstOrDefault();
                    if (communicationInfo == null) return;
                    communicationInfo.Port = port;
                    db.SubmitChanges();
                    DeviceConnectionManager.GetInstance().Add(new DeviceItem()
                    {
                        ConnectionId = e.ConnectionId,
                        ConnectionDate = DateTime.Now,
                        DeviceId = communicationInfo.ID,
                        Status = ConnectionStatus.Active,
                        Type = Analysis.AnalysisProtocolType.Customer,
                        Buffer = new List<byte>(8092),
                        SIMId = string.Empty,
                        IP = ip,
                        Port = port
                    });
                }
            }
            catch { }
            finally
            {
                DeviceConnectionManager.GetInstance().ActiveUnlock();
            }
        }


        public static void Disconnect(BaseSocketConnection e)
        {
            string ip = e.RemoteEndPoint.Address.ToString();

            int port = e.RemoteEndPoint.Port;

            string key = e.RemoteEndPoint.ToString();
            try
            {
                if (DeviceConnectionManager.GetInstance().ActiveLock())
                {
                    var deviceItem = DeviceConnectionManager.GetInstance().Get(e.ConnectionId);
                    if (deviceItem != null)
                    {
                        DeviceConnectionManager.GetInstance().Disconnect(deviceItem);
                    }
                }
            }
            catch { }
            finally
            {
                DeviceConnectionManager.GetInstance().ActiveUnlock();
            }
        }
    }
}
