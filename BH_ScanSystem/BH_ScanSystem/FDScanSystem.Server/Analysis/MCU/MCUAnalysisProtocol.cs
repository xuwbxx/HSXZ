using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using FDScanSystem.DAO;
using FDScanSystem.Helper;
using FDScanSystem.Server;
using FDScanSystem.Server.Analysis.DataReceiver;
using FDScanSystem.Server.CommandBuilder.MCU;
using FDScanSystem.Server.Device;
using FDScanSystem.Server.Job;
using FDScanSystem.Server.Job.DataReceiver;
using Interactive.Utility;
using ZJFramework10.Extension;

namespace FDScanSystem.Server.Analysis.MCU
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
        private static string tempstr = "";
        private static byte[] tempbyte = new byte[0];


        private List<IDataReceive> _dataReceives;

        public override Job.ICommandBuilder CommandBuilder
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


        public override void OnReceived(DeviceItem sender, byte[] receiveData)
        {
            // base.OnReceived(sender, receiveData);
            var device = sender;
            if (device == null)
            {
                //ERROR
                return;
            }
            if (device.Buffer1 == null)
            {
                device.Buffer1 = new List<byte>();
            }

            if (device.Buffer == null)
            {
                device.Buffer = new List<byte>();
            }
            var db = DataContextFactory.Create();
            //var dModel = db.MK_Device.FirstOrDefault(u => u.ID == device.DeviceId);
            //if (dModel == null)
            //{
            //    return;
            //}

            if (receiveData.Length > 0 && receiveData[receiveData.Length - 1] == 0x0A)
            {
                var buffer = DeviceConnectionManager.GetInstance().FlushBuffer(device.Key);
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

                // LogWriter.Info("收到一条来自MSCU的完整消息:" + fullmsg);
                AppContext.Log("收到一条来自MSCU的完整消息:" + fullmsg);

                // var db = DataContextFactory.Create();
                var d = db.MK_Device.FirstOrDefault(u => u.ID == sender.DeviceId);
                if (d != null)
                {
                    d.Status = 1;
                    db.SubmitChanges();

                    //var point = db.MK_Point.FirstOrDefault(u => u.DeviceId == d.ID);
                    //{
                    //    if (point != null)
                    //    {
                    //        var position = db.MK_Position.FirstOrDefault(u => u.ID == point.PositionId);
                    //        if (position != null)
                    //        {
                    //            var layer = db.MK_Layer.FirstOrDefault(u => u.ID == position.LayerId);
                    //            if (layer != null && layer.LayerType!=1)
                    //            {
                    //                layer.LayerType = 0;
                    //                db.SubmitChanges();
                    //            }
                    //        }
                    //    }
                    //}

                }

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
            else if (receiveData.Length > 0 && device.Buffer.Count >= 16 && device.Buffer.Count < 69)
            {
                if (device.Buffer[0] != 104)
                {
                    DeviceConnectionManager.GetInstance().Update(device.Key, receiveData);
                }
                else
                {

                    var buffer = DeviceConnectionManager.GetInstance().FlushBuffer(device.Key);

                    string result = "";
                    //   byte[] allData = null;
                    if (buffer != null)
                    {
                        if (buffer[0] == 104)
                        {
                            for (int i = 0; i < 16; i++)
                            {
                                int temp = Convert.ToString(buffer[i], 16).AsInt();
                                if (temp < 10)
                                {
                                    result += "0" + temp + " ";
                                }
                                else
                                    result += temp + " ";
                            }
                            //allData = new byte[buffer.Length + receiveData.Length];

                            //Buffer.BlockCopy(buffer, 0, allData, 0, buffer.Length);
                            //bool flag = true;
                            //Buffer.BlockCopy(receiveData, 0, allData, buffer.Length, receiveData.Length);
                            AppContext.Log("收到一条完整消息:" + result);
                            //LogWriter.Info("收到一条完整消息:" + result);
                            DataReceiveArgs args = null;
                            foreach (var dataReceiver in Receiver)
                            {
                                if ((args = dataReceiver.Handle1(device, result)) != null && args.IsHandle)
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
                    }
                    else
                    {
                        //allData = receiveData;
                    }
                }
                ////    bool flag = false;
                //DataReceiveArgs args = null;
                //foreach (var dataReceiver in Receiver)
                //{
                //    if ((args = dataReceiver.Handle(device, allData)) != null && args.IsHandle)
                //    {
                //        //  flag = true;
                //        break;
                //    }
                //}
                ////if (!flag) {
                ////    args = DefaultReceiver.Handle(device, allData);
                ////}
                //if (!args.IsClearState)
                //{
                //    Singleton<CommandQueueManager>.Instance.CommandIsDone(device.Key);
                //}
            }
            else if ((device.Buffer.Count + receiveData.Length + device.Buffer1.Count) >= 69 && device.Protocol == 3)
            {

                string str = "";
             //   for (int i = 0; i < receiveData.Length; i++)
             //   {
             //       string temp = Convert.ToString(receiveData[i], 16);
             //       //string t = Convert.ToInt32(temp, 16).AsStr();
             //       if (temp.Length < 2)
             //       {
             //           temp = "0" + temp;
             //       }
             //       str += temp + " ";
             //   }
             ////   AppContext.Log("收到报文22:" + str);
             //   str = "";


                //if (device.Buffer1.Count > 0)
                //{
                //    for (int i = 0; i < device.Buffer1.Count; i++)
                //    {
                //        string temp = Convert.ToString(device.Buffer1[i], 16);
                //        if (temp.Length < 2)
                //        {
                //            temp = "0" + temp;
                //        }
                //        if (temp.Equals("00") || temp.ToLowerInvariant().Equals("ff"))
                //        {
                //            device.Buffer1.RemoveAt(0);
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //}

                var buffer1 = DeviceConnectionManager.GetInstance().FlushBuffer1(device.Key);
                if (buffer1.Length == 0)
                {
                    List<string> ss = new List<string>();
                    foreach (var b in device.Buffer)
                    {
                        string temp = Convert.ToString(b, 16);
                        if (temp.Length < 2)
                        {
                            temp = "0" + temp;
                        }
                        ss.Add(temp);
                    }
                    int count = device.Buffer.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (ss.Count > 0)
                        {
                            if (ss[0].AsStr().Equals("00") || ss[0].AsStr().ToLowerInvariant().Equals("ff"))
                            {
                                device.Buffer.RemoveAt(0);
                                ss.RemoveAt(0);
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (device.Buffer.Count >= 3)
                        {
                            if (ss[1].Equals("03") && ss[2].Equals("40"))
                            {
                                break;
                            }
                            else
                            {
                                device.Buffer.RemoveAt(1);
                                ss.RemoveAt(1);
                            }
                        }

                    }
                    foreach (var i in ss)
                    {
                        str += i + " ";
                    }
                    //  AppContext.Log("Buffer报文:" + str);
                    str = "";
                }
                var buffer = DeviceConnectionManager.GetInstance().FlushBuffer(device.Key);


                //if (buffer.Length == 0)
                //{
                //    for (int i = 0; i < receiveData.Length; i++)
                //    {
                //        string temp = Convert.ToString(receiveData[i], 16);
                //        if (temp.Length < 2)
                //        {
                //            temp = "0" + temp;
                //        }
                //        if (temp.Equals("00") || temp.ToLowerInvariant().Equals("ff"))
                //        {

                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //}

                string result = "";
                byte[] allData = null;

                //if (buffer != null && buffer.Length > 0)
                //{

                allData = new byte[buffer.Length + receiveData.Length + buffer1.Length];
                //AppContext.Log("Buffer1的长度:" + buffer1.Length.ToString());
                //AppContext.Log("Buffer的长度:" + buffer.Length.ToString());
                //AppContext.Log("recieverData的长度:" + receiveData.Length.ToString());
                Buffer.BlockCopy(buffer1, 0, allData, 0, buffer1.Length);
                Buffer.BlockCopy(buffer, 0, allData, buffer1.Length, buffer.Length);
                Buffer.BlockCopy(receiveData, 0, allData, buffer.Length + buffer1.Length, receiveData.Length);
                if (allData.Length >= 69)
                {
                    // device.Buffer1 = new List<byte> { };
                    tempstr = "";

                    // int j = 0;
                    // 
                    //if (buffer[0] == 104)
                    //{
                    int sum = allData.Length / 69;

                    int cc = 0;
                    List<string> crc = new List<string>();
                    for (int i = 0; i < allData.Length; i++)
                    {
                        cc = i / 69;
                        string temp = Convert.ToString(allData[i], 16);
                        //string t = Convert.ToInt32(temp, 16).AsStr();
                        if (temp.Length < 2)
                        {
                            temp = "0" + temp;
                        }
                        if (cc < sum)
                        {
                            if ((i + 1) % 69 == 0)
                            {
                                result += temp + " ";
                                cc++;

                                if (cc == 1)
                                {
                                    AppContext.Log("收到一条完整消息:" + result);


                                    DataReceiveArgs args = null;
                                    foreach (var dataReceiver in Receiver)
                                    {
                                        if ((args = dataReceiver.Handle1(device, result)) != null && args.IsHandle)
                                        {
                                            //  flag = true;
                                            //   AppContext.Log("收到一条完整消息:" + result);
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

                                result = "";
                                continue;
                            }
                            result += temp + " ";

                        }
                        else
                        {
                            device.Buffer1.Add(allData[i]);
                            crc.Add(temp);
                           // tempstr += temp + " ";
                        }

                        // device.Length = device.Buffer1.Count();

                        //allData = new byte[buffer.Length + receiveData.Length];

                        //Buffer.BlockCopy(buffer, 0, allData, 0, buffer.Length);
                        //bool flag = true;
                        //Buffer.BlockCopy(receiveData, 0, allData, buffer.Length, receiveData.Length);

                        receiveData = null; ;
                        //LogWriter.Info("收到一条完整消息:" + result);

                    }

               //     AppContext.Log("收到多余报文:" + tempstr);
                    int count = device.Buffer1.Count;
                    tempstr = "";
                    for (int i = 0; i < count; i++)
                    {
                        if (crc.Count > 0)
                        {
                            if (crc[0] == "00" || crc[0].ToLowerInvariant() == "ff")
                            {
                                crc.RemoveAt(0);
                                device.Buffer1.RemoveAt(0);
                            }
                        }
                        if (device.Buffer1.Count >= 3)
                        {
                            if (crc[1].Equals("03") && crc[2].Equals("40"))
                            {
                                break;
                            }
                            else
                            {
                                crc.RemoveAt(1);
                                device.Buffer1.RemoveAt(1);
                            }
                        }

                    }

                    //foreach (var i in crc)
                    //{
                    //    tempstr += i + " ";
                    //}
                    //AppContext.Log("Buffer1的报文:" + tempstr);
                }
                else
                {
                    DeviceConnectionManager.GetInstance().Update(device.Key, allData);
                }
                //}
                //else
                //{
                //    DeviceConnectionManager.GetInstance().Update(device.Key, receiveData);
                //}

            }
            else
            {
                //string str = "";
                //for (int i = 0; i < receiveData.Length; i++)
                //{
                //    string temp = Convert.ToString(receiveData[i], 16);
                //    //string t = Convert.ToInt32(temp, 16).AsStr();
                //    if (temp.Length < 2)
                //    {
                //        temp = "0" + temp;
                //    }
                //    str += temp + " ";
                //}
                //AppContext.Log("收到报文11:" + str);
                //AppContext.Log("RecieverData的长度11:" + receiveData.Length.ToString());
                DeviceConnectionManager.GetInstance().Update(device.Key, receiveData);
                //Singleton<CommandQueueManager>.Instance.CommandIsDone(device.Key);
            }
        }

        public override DataReceiver.IDataReceive DefaultReceiver
        {
            get { return _dataReceives.LastOrDefault(); }
        }
    }



}
