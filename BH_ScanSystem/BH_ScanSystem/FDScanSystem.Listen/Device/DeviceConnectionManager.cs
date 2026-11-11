using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Listen.Logic;
using TcpTransmit.Core.Server;
using FDScanSystem.DAO;


namespace FDScanSystem.Listen.Device
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
        Dictionary<string, DeviceItem> _dict = new Dictionary<string, DeviceItem>();
        Dictionary<int, DeviceItem> _dict2 = new Dictionary<int, DeviceItem>();

        object _lockObject = new object();

        protected void Initialize()
        {

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
            if (_dict.ContainsKey(item.Key))
            {
                _dict[item.Key] = item;
                _dict2[item.ConnectionId] = item;
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
                if (!_dict.ContainsKey(item.Key))
                {
                    _dict.Add(item.Key, item);
                    _dict2.Add(item.ConnectionId, item);
                }
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="item"></param>
        public void Disconnect(DeviceItem item)
        {
            lock (_lockObject)
            {
                var db = DataContextFactory.Create();
                var communication = db.FD_CommunicationDevice.Where(u => u.ID == item.DeviceId).FirstOrDefault();
                if (communication != null && communication.IP.Equals(item.IP) && communication.Port == item.Port)
                {
                  //  communication.IP = string.Empty;
                    communication.Port = 0;

                    db.SubmitChanges();
                }
                if (_dict.ContainsKey(item.Key))
                {
                    _dict.Remove(item.Key);
                    _dict2.Remove(item.ConnectionId);
                }
            }
        }

        /// <summary>
        /// 获取没有状态的链接
        /// </summary>
        /// <param name="iden"></param>
        /// <returns></returns>
        public DeviceItem GetWithoutStatus(string iden)
        {
            DeviceItem outVal = null;
            if (_dict.TryGetValue(iden, out outVal))
            {
                return outVal;
            }
            return null;
        }

        /// <summary>
        /// 获取没有状态的链接
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public DeviceItem GetWithoutStatus(int connectionId)
        {
            DeviceItem outVal = null;
            if (_dict2.TryGetValue(connectionId, out outVal))
            {
                return outVal;
            }
            return null;
        }

        /// <summary>
        /// 获取激活状态下的链接
        /// </summary>
        /// <param name="iden"></param>
        /// <returns></returns>
        public DeviceItem Get(string iden)
        {
            DeviceItem outVal = null;
            if (_dict.TryGetValue(iden, out outVal))
            {
                if (outVal.Status == ConnectionStatus.Active)
                    return outVal;
                else
                    return null;
            }
            return null;
        }

        public DeviceItem Get(int connectionId)
        {
            DeviceItem outVal = null;
            if (_dict2.TryGetValue(connectionId, out outVal))
            {
                // if (outVal.Status == ConnectionStatus.Active)
                return outVal;
                // else
                //  return null;
            }
            return null;
        }


        public void Update(string connectionKey, byte[] data)
        {
            var deviceItem = Get(connectionKey);
            if (deviceItem != null)
            {
                if (deviceItem.Buffer == null)
                {
                    deviceItem.Buffer = new List<byte>();
                }
                deviceItem.Buffer.AddRange(data);
                lock (_lockObject)
                {
                    _dict[connectionKey] = deviceItem;
                    _dict2[deviceItem.ConnectionId] = deviceItem;
                }
            }
        }

        public byte[] FlushBuffer(string connectionKey)
        {
            var deviceItem = Get(connectionKey);
            byte[] remainBuffer = null;
            if (deviceItem != null)
            {
                if (deviceItem.Buffer == null)
                {

                    return remainBuffer;
                }
                remainBuffer = deviceItem.Buffer.ToArray();

                deviceItem.Buffer.Clear();
                lock (_lockObject)
                {
                    _dict[connectionKey] = deviceItem;
                    _dict2[deviceItem.ConnectionId] = deviceItem;
                }
            }
            return remainBuffer;
        }


        ~DeviceConnectionManager()
        {
            if (_dict.Values != null)
            {
                _dict.Clear();
                _dict = null;
            }
            if (_dict2.Values != null)
            {
                _dict2.Clear();
                _dict2 = null;
            }
            try
            {
                if (Host.DefaultHost != null) Host.DefaultHost.Dispose();
            }
            catch { }

        }
    }
}
