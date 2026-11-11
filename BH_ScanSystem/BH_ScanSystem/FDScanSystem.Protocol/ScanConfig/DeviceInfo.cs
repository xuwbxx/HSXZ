using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Protocol.ScanConfig
{
    public class DeviceInfo
    {
        public int DeviceId { get; set; }

        public int CommunicationDeviceId { get; set; }

        public string DeviceIdentity { get; set; }

        public int HowLong { get; set; }

        public int Interval { get; set; }

        public int TDJZPL { set; get; }

        public int ZDJZPL { set; get; }


        public Dictionary<int, PointInfo> Points { get; set; }
    }

}
