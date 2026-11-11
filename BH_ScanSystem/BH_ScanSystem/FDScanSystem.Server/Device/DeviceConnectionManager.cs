using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using FDScanSystem.DAO;
using FDScanSystem.DAO.Scan;
using FDScanSystem.Server.Analysis;
using FDScanSystem.Server.Job;



namespace FDScanSystem.Server.Device
{
    public class DeviceConnectionManager
    {
        /// <summary>
        /// 锁定对象
        /// </summary>
        private static object _lockObj = new object();
        /// <summary>
        /// 链接管理对象
        /// </summary>
        private static DeviceConnectionManager _instance;
        /// <summary>
        /// 实例化
        /// </summary>
        /// <returns></returns>
        public static DeviceConnectionManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new DeviceConnectionManager();
                    }
                }
            }
            return _instance;

        }

        /// <summary>
        /// 连接池
        /// </summary>
        //  Dictionary<string, DeviceItem> _dict = new Dictionary<string, DeviceItem>();
        Dictionary<string, DeviceItem> _dict2 = new Dictionary<string, DeviceItem>();

        object _lockObject = new object();

        protected void Initialize()
        {
            var db = DataContextFactory.Create();
            var command1 = db.MK_Scan_Command.Where(u => u.CreateDate < DateTime.Now.AddDays(-3)).ToList();
            db.MK_Scan_Command.DeleteAllOnSubmit(command1);
            db.SubmitChanges();
            var result = ServerDAL.ListComDevice();
            foreach (var comdeviceItem in result)
            {
                var device = db.MK_Device.Where(u => u.CommunicationDeviceId == comdeviceItem.ID).ToList();
                foreach (var d in device)
                {
                    d.Status = 0;
                    db.SubmitChanges();
                }
                var dEntity = new DeviceItem
                {
                    Buffer = new List<byte>(),
                    DeviceId = comdeviceItem.ID,
                    ScanDevices = comdeviceItem.Devices,
                    Type = Analysis.AnalysisProtocolType.MCU,
                    Protocol = comdeviceItem.DataProtocol,
                    Status = ConnectionStatus.Open,
                    ConnectionDate = DateTime.Now,
                    COMPort = new System.IO.Ports.SerialPort(comdeviceItem.CommunicationCode, 9600)

                };
                dEntity.COMPort.NewLine = "\r\n";
                dEntity.COMPort.RtsEnable = true;
                dEntity.COMPort.DataReceived += COMPort_DataReceived;
                try
                {
                    if (dEntity.COMPort != null)
                    {
                        if (!dEntity.COMPort.IsOpen)
                            dEntity.COMPort.Open();
                    }
                }
                catch
                {
                    AppContext.Log(string.Format("{0}的COM端口初始化失败", comdeviceItem.CommunicationCode));
                    //   AppContext.Error(string.Format("{0}的COM端口初始化失败", comdeviceItem.CommunicationCode));
                    dEntity = null;
                    continue;
                }
                dEntity.Status = ConnectionStatus.Active;
                if (dEntity.Protocol == 0)
                    dEntity.COMPort.WriteLine("MCU32 Info?");
                else if (dEntity.Protocol == 3)
                {
                    string val = "";
                    //string str = "01 03 00 64 00 40 05 E5";
                    //var temp = str.Split(' ');
                    //foreach (var s in temp)
                    //{

                    //    val = val + System.Convert.ToInt32(s, 16).ToString();
                    //}
                    //dEntity.COMPort.WriteLine(val);

                }
                else
                {
                    string val = "";
                    string str = "68 04 00 04 08";
                    var temp = str.Split(' ');
                    foreach (var s in temp)
                    {

                        val = val + System.Convert.ToInt32(s, 16).ToString();
                    }
                    dEntity.COMPort.WriteLine(val);
                }
                _dict2.Add(comdeviceItem.CommunicationCode, dEntity);
            }

        }

        void COMPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            var port = sender as SerialPort;
            if (port == null) return;

            var deviceItem = DeviceConnectionManager.GetInstance().GetWithoutStatus(port.PortName);
            if (deviceItem != null)
                COMAnalysisProtocolFactory.Create(deviceItem).OnReceived(deviceItem);
        }

        protected DeviceConnectionManager()
        {

            Initialize();
        }

        /// <summary>
        /// 锁定
        /// </summary>
        /// <returns></returns>
        public bool ActiveLock()
        {
            return System.Threading.Monitor.TryEnter(_lockObject, 1000);
        }

        /// <summary>
        /// 激活链接
        /// </summary>
        /// <param name="item"></param>
        public void Active(DeviceItem item)
        {
            item.Status = ConnectionStatus.Active;

            //  lock (_lockObject) {
            if (_dict2.ContainsKey(item.Key))
            {
                _dict2[item.Key] = item;
            }
            // }
        }

        /// <summary>
        /// 解锁
        /// </summary>
        public void ActiveUnlock()
        {

            System.Threading.Monitor.Exit(_lockObject);
        }

        /// <summary>
        /// 添加一个链接
        /// </summary>
        /// <param name="item"></param>
        public void Add(DeviceItem item)
        {
            lock (_lockObject)
            {
                if (!_dict2.ContainsKey(item.Key))
                {
                    _dict2.Add(item.Key, item);
                }
            }
        }



        /// <summary>
        /// 获取没有状态的链接
        /// </summary>
        /// <param name="iden"></param>
        /// <returns></returns>
        public DeviceItem GetWithoutStatus(string hashCode)
        {
            DeviceItem outVal = null;
            if (_dict2 != null)
            {
                if (_dict2.TryGetValue(hashCode, out outVal))
                {
                    return outVal;
                }
            }
            return null;
        }



        /// <summary>
        /// 获取激活状态下的链接
        /// </summary>
        /// <param name="iden"></param>
        /// <returns></returns>
        public DeviceItem Get(string hash)
        {
            DeviceItem outVal = null;
            if (_dict2.TryGetValue(hash, out outVal))
            {
                if (outVal.Status == ConnectionStatus.Active)
                    return outVal;
                else
                    return null;
            }
            return null;
        }




        public void Update(string hash, byte[] data)
        {
            var deviceItem = Get(hash);
            if (deviceItem != null)
            {
                if (deviceItem.Buffer == null)
                {
                    deviceItem.Buffer = new List<byte>();
                }
                deviceItem.Buffer.AddRange(data);
                lock (_lockObject)
                {
                    _dict2[hash] = deviceItem;
                    // _dict2[deviceItem.ConnectionId] = deviceItem;
                }
            }
        }

        public byte[] FlushBuffer(string hash)
        {
            var deviceItem = Get(hash);
            byte[] remainBuffer = null;
            if (deviceItem != null)
            {
                if (deviceItem.Buffer == null)
                {

                    return remainBuffer;
                }
                remainBuffer = deviceItem.Buffer.ToArray();

                deviceItem.Buffer.Clear();
                deviceItem.Buffer = new List<byte>();
                lock (_lockObject)
                {
                    _dict2[hash] = deviceItem;
                    // _dict2[deviceItem.ConnectionId] = deviceItem;
                }
            }
            return remainBuffer;
        }

        public byte[] FlushBuffer1(string hash)
        {
            var deviceItem = Get(hash);
            byte[] remainBuffer = null;
            if (deviceItem != null)
            {
                if (deviceItem.Buffer1 == null)
                {

                    return remainBuffer;
                }
                remainBuffer = deviceItem.Buffer1.ToArray();

                deviceItem.Buffer1.Clear();
                deviceItem.Buffer1 = new List<byte>();
                lock (_lockObject)
                {
                    _dict2[hash] = deviceItem;
                    // _dict2[deviceItem.ConnectionId] = deviceItem;
                }
            }
            return remainBuffer;
        }

        public void ReConnect()
        {
            lock (_lockObject)
            {
                if (_dict2 != null)
                {
                    if (_dict2.Values != null)
                    {
                        foreach (var deviceItem in _dict2.Values)
                        {
                            try
                            {
                                if (deviceItem.COMPort != null)
                                {
                                    if (deviceItem.COMPort.IsOpen)
                                        deviceItem.COMPort.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                AppContext.Log(ex.Message);
                                //    AppContext.Error(ex);
                            }
                            Thread.Sleep(100);
                        }
                        _dict2.Clear();
                        _dict2 = null;
                    }
                }
                Thread.Sleep(200);
                _dict2 = new Dictionary<string, DeviceItem>();
                Initialize();

            }


        }

        public void WriteStatus(Action<string> writeMsg)
        {
            foreach (var deviceItem in _dict2.Values)
            {
                writeMsg(string.Format("COM端口号:{0};连接时间:{1};Status:{2}", deviceItem.COMPort.PortName, deviceItem.ConnectionDate.ToString("yyyy-MM-dd HH:mm:ss"), deviceItem.Status == (int)ConnectionStatus.Open ? "打开" : "激活"));
            }

        }


        ~DeviceConnectionManager()
        {
            if (_dict2 != null)
            {
                if (_dict2.Values != null)
                {
                    foreach (var deviceItem in _dict2.Values)
                    {
                        try
                        {
                            if (deviceItem.COMPort != null)
                            {
                                deviceItem.COMPort.Close();
                            }
                        }
                        catch
                        {

                        }
                    }
                    _dict2.Clear();
                    _dict2 = null;
                }
            }

        }
    }
}
