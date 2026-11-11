using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Protocol.ScanConfig
{
    public class ScanSetting
    {
        public DateTime Begin { get; set; }

        public int Interval { get; set; }

        public int ScanTime { get; set; }
    }
}
