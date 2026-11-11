using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcpTransmit.Core;
using FDScanSystem.Listen.Device;
using Interactive.Utility;

using FDScanSystem.Listen.Analysis.MCU;
using FDScanSystem.Listen.Analysis.Custome;

namespace FDScanSystem.Listen.Analysis
{
    public class AnalysisProtocolFactory
    {
        public static AnalysisProtocolBase Create(BaseSocketConnection connection)
        {
            var deviceItem = DeviceConnectionManager.GetInstance().GetWithoutStatus(connection.RemoteEndPoint.ToString());
            if (deviceItem != null)
            {
                return Create(deviceItem.Type);
            }

            return null;
        }

        public static AnalysisProtocolBase Create(AnalysisProtocolType type)
        {
            return Singleton<CustomeAnalysisProtocol>.Instance;
        }
    }
}
