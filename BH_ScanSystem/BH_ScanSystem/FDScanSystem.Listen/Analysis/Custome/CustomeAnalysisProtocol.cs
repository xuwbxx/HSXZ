using FDScanSystem.Listen.Analysis.DataReceiver;
using FDScanSystem.Listen.Analysis.DataReceiver.Custome;
using FDScanSystem.Listen.CommandBuilder.Custome;
using FDScanSystem.Listen.Device;
using FDScanSystem.Listen.Logic;
using Interactive.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Listen.Analysis.Custome
{
    public class CustomeAnalysisProtocol : AnalysisProtocolBase
    {
        public CustomeAnalysisProtocol()
        {
            _dataReceives = new List<IDataReceive>(6);
            _dataReceives.Add(new DataInfoDataReceiver());
            _dataReceives.Add(new AlertDataInfoDataReceiver());
            _dataReceives.Add(new ResponseOkDataReceiver());
        }

        private List<IDataReceive> _dataReceives;

        public override ICommandBuilder CommandBuilder
        {
            get
            {
                // return null;
                return Singleton<CustomeCommandBuilder>.Instance;
            }
        }

        public override List<DataReceiver.IDataReceive> Receiver
        {
            get { return _dataReceives; }
        }


        public override void OnReceived(TcpTransmit.Core.BaseSocketConnection e, byte[] receiveData)
        {
            base.OnReceived(e, receiveData);
            if (receiveData.Length > 2 && receiveData[receiveData.Length - 2] == 0x7B && receiveData[receiveData.Length - 1] == 0x7B)
            {
                //心跳包
                return;
            }

            var device = DeviceConnectionManager.GetInstance().GetWithoutStatus(e.RemoteEndPoint.ToString());
            if (device == null)
            {
                //ERROR
                return;
            }

            if (receiveData.Length > 1 && receiveData[receiveData.Length - 1] == 0x0A /*&& receiveData[receiveData.Length - 2] == 0x0D*/)
            {
                var buffer = DeviceConnectionManager.GetInstance().FlushBuffer(e.RemoteEndPoint.ToString());
                byte[] allData = null;
                if (buffer != null)
                {
                    allData = new byte[buffer.Length + receiveData.Length - 1];

                    Buffer.BlockCopy(buffer, 0, allData, 0, buffer.Length);

                    Buffer.BlockCopy(receiveData, 0, allData, buffer.Length, receiveData.Length - 1);

                }
                else
                {
                    allData = receiveData;
                }


                //string fullmsg = Encoding.ASCII.GetString(allData);


                //AppContext.Log(String.Format("收到一条来自MSCU的完整消息:{0}\r", fullmsg));
                bool flag = false;
                DataReceiveArgs args = null;
                foreach (var dataReceiver in Receiver)
                {
                    if ((args = dataReceiver.Handle(device, allData)) != null && args.IsHandle)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag && args.ResponseMsg != null)
                {
                    var bytes = Encoding.UTF8.GetBytes(args.ResponseMsg.ToString());
                    byte[] result = new byte[bytes.Length + 1];
                    Buffer.BlockCopy(bytes, 0, result, 0, bytes.Length);
                    result[bytes.Length] = 0x0A;
                    e.BeginSend(result);
                }
                //if (!flag) {
                //    args = DefaultReceiver.Handle(device, allData);
                //}
                //if (!args.IsClearState)
                //{
                //    Singleton<CommandQueueManager>.Instance.CommandIsDone(device.Key);
                //}
            }
            else
            {
                DeviceConnectionManager.GetInstance().Update(device.Key, receiveData);
            }

        }

        public override DataReceiver.IDataReceive DefaultReceiver
        {
            get { return _dataReceives.LastOrDefault(); }
        }
    }



}
