using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Listen.Logic;
using Interactive.Utility;
using XPDScanSystem.Services.CommandBuilder.MCU;
using FDScanSystem.Listen.Device;
using FDScanSystem.Listen.Analysis.DataReceiver;
using FDScanSystem.Listen.Logic.DataReceiver;
using FDScanSystem.DAO;


namespace FDScanSystem.Listen.Analysis.MCU
{
    public class MCUAnalysisProtocol : AnalysisProtocolBase
    {
        public MCUAnalysisProtocol()
        {
            _dataReceives = new List<IDataReceive>(6);

            _dataReceives.Add(new VersionDataReceiver());
            _dataReceives.Add(new OKDataReceiver());
            _dataReceives.Add(new WaitDataReceiver());
            _dataReceives.Add(new DetailDataReceiver());
            _dataReceives.Add(new DefaultDataReceiver());
            //_dataReceives.Add();
        }


        private List<IDataReceive> _dataReceives;

        public override ICommandBuilder CommandBuilder
        {
            get
            {
                return Singleton<MCUCommandBuilder>.Instance;
            }
        }

        public override List<DataReceiver.IDataReceive> Receiver
        {
            get { return _dataReceives; }
        }


        public override void OnReceived(TcpTransmit.Core.BaseSocketConnection e, byte[] receiveData)
        {
            base.OnReceived(e, receiveData);
            var device = DeviceConnectionManager.GetInstance().GetWithoutStatus(e.RemoteEndPoint.ToString());
            if (device == null)
            {
                //ERROR
                return;
            }
            if (receiveData.Length <= 23 && receiveData[0] == 0x7b && receiveData[receiveData.Length - 1] == 0x7b)
            {
                byte[] c = new byte[11];

                Buffer.BlockCopy(receiveData, 4, c, 0, 11);

                string identity = Encoding.ASCII.GetString(c);
                var deviceConnectionInstance = DeviceConnectionManager.GetInstance();
                //     AppContext.Log(string.Format("收到DTU的链接{0}", identity));
                if (deviceConnectionInstance.ActiveLock())
                {
                    //try
                    //{
                    //    var db = DataContextFactory.Create();
                    //    XPD_Scan_CommunicationDevice entity = null;
                    //    if ((entity = db.XPD_Scan_CommunicationDevice.Where(u => u.CommunicationCode.Equals(identity) && u.DeviceType == 1).FirstOrDefault()) != null)
                    //    {
                    //        device.SIMId = identity;
                    //        device.Buffer = new List<byte>(10);
                    //        device.DeviceId = entity.ID;
                    //        entity.IP = device.IP;
                    //        entity.Port = device.Port;
                    //        db.SubmitChanges();
                    //        deviceConnectionInstance.Active(device);
                    //        AppContext.Log(string.Format("MCUAnalysisProtocol更新关键字{0}", entity.DeviceType ?? 0));
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    AppContext.Error(ex);
                    //}
                    //finally
                    //{
                    //    deviceConnectionInstance.ActiveUnlock();
                    //}
                }
            }
            else
            {
                if (receiveData.Length > 1 && receiveData[receiveData.Length - 1] == 0x0A && receiveData[receiveData.Length - 2] == 0x0D)
                {
                    var buffer = DeviceConnectionManager.GetInstance().FlushBuffer(e.RemoteEndPoint.ToString());
                    byte[] allData = null;
                    if (buffer != null)
                    {
                        allData = new byte[buffer.Length + receiveData.Length];

                        Buffer.BlockCopy(buffer, 0, allData, 0, buffer.Length);

                        Buffer.BlockCopy(receiveData, 0, allData, buffer.Length, receiveData.Length);

                    }
                    else
                    {
                        allData = receiveData;
                    }


                    string fullmsg = Encoding.ASCII.GetString(allData);


                    AppContext.Log(String.Format("收到一条来自MSCU的完整消息:{0}\r", fullmsg));
                    //    bool flag = false;
                    DataReceiveArgs args = null;
                    foreach (var dataReceiver in Receiver)
                    {
                        if ((args = dataReceiver.Handle(device, allData)) != null && args.IsHandle)
                        {
                            //  flag = true;
                            break;
                        }
                    }
                    //if (!flag) {
                    //    args = DefaultReceiver.Handle(device, allData);
                    //}
                    if (!args.IsClearState)
                    {
                        Singleton<CommandQueueManager>.Instance.CommandIsDone(device.Key);
                    }

                }
                else
                {
                    DeviceConnectionManager.GetInstance().Update(device.Key, receiveData);
                }
            }
        }

        public override DataReceiver.IDataReceive DefaultReceiver
        {
            get { return _dataReceives.LastOrDefault(); }
        }
    }



}
