
using FDScanSystem.Server.Analysis.MCU;
using FDScanSystem.Server.Device;
using Interactive.Utility;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace FDScanSystem.Server.Analysis
{
    public class COMAnalysisProtocolFactory
    {

        public static AnalysisProtocolBase Create(DeviceItem deviceItem)
        {
     
            if (deviceItem != null)
            {
                return Create(deviceItem.Type);
            }

            return null;
        }

        public static AnalysisProtocolBase Create(AnalysisProtocolType type)
        {

            return Singleton<MCUAnalysisProtocol>.Instance;

        }

    }
}
