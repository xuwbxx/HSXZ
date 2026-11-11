using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Listen.Logic;
using FDScanSystem.Listen.Device;
using FDScanSystem.Listen.Logic.DataReceiver;

namespace FDScanSystem.Listen.Analysis.DataReceiver {
    public interface IDataReceive {
        DataReceiveArgs Handle(DeviceItem device, byte[] receiverData);
    }
}
