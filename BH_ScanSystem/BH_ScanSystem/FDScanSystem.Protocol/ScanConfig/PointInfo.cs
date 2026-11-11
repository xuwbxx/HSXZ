using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Protocol.ScanConfig
{
    public class PointInfo
    {
        public int PointId { get; set; }

        public int DeviceId { get; set; }

        public string Channel1 { get; set; }

        public string Channel2 { get; set; }

        public double AlertMin { get; set; }

        public double AlertMax { get; set; }

        public double StandardValue { get; set; }

        public double lingmingValue { set; get; }

        public int Perpiod { set; get; }

    }
}
