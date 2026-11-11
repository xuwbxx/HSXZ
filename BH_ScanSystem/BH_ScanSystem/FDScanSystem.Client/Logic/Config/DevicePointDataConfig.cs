using FDScanSystem.Protocol.ScanConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FDScanSystem.Client.Logic.Config
{
    public class DevicePointDataConfig
    {
        readonly string CONFIG_LOCATION = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/device_point_data.txt");

        public List<DeviceInfo> Devices { get; set; }

        static bool IsLoaded = false;

        public void Load()
        {
            using (FileStream fs = new FileStream(CONFIG_LOCATION, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader fsr = new StreamReader(fs, Encoding.UTF8))
                {
                    string content = fsr.ReadToEnd();
                    this.Devices = ZJFramework10.Serialize.Helper.DeserializeString<List<DeviceInfo>>(content);
                }
            }
        }

        public void Update(List<DeviceInfo> newConfig)
        {
            using (FileStream fs = new FileStream(CONFIG_LOCATION, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter fsw = new StreamWriter(fs, Encoding.UTF8))
                {
                    fsw.WriteLine(ZJFramework10.Serialize.Helper.SerializeString(newConfig));

                    IsLoaded = false;
                }
            }
        }

        static DevicePointDataConfig config = new DevicePointDataConfig();

        public static DevicePointDataConfig GetInstance()
        {
            if (!IsLoaded)
            {
                config.Load();
            }
            //DeviceInfo deviceInfo = new DeviceInfo();
            //deviceInfo.DeviceId = 1;
            //deviceInfo.CommunicationDeviceId = 1;
            //deviceInfo.DeviceIdentity = "USB-4716,BID#0";
            //deviceInfo.HowLong = 60 * 1000;
            //deviceInfo.Interval = 200;
            //deviceInfo.Points = new Dictionary<int, PointInfo>();

            //for (int i = 0; i < 8; i++)
            //{
            //    deviceInfo.Points.Add(i, new PointInfo()
            //    {
            //        AlertMax = 100,
            //        AlertMin = -100,
            //        Channel1 = i.ToString(),
            //        Channel2 = "0",
            //        PointId = 1,
            //        DeviceId = 1,
            //        StandardValue = 0.5
            //    });
            //}
            //config = new DevicePointDataConfig();
            //config.Devices = new List<DeviceInfo>(2);
            //config.Devices.Add(deviceInfo);
            return config;
        }
    }



}
