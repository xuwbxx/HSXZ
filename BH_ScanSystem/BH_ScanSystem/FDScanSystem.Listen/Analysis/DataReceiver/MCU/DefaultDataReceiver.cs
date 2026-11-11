using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Listen.Analysis.DataReceiver.MCU;

namespace FDScanSystem.Listen.Logic.DataReceiver {
    public class DefaultDataReceiver : MCUDataReceiverBase {

        protected override DataReceiveArgs SubHandle(Device.DeviceItem device, DataReceiveArgs args, string msg) {
            args.IsHandle = true;//是否被处理
            return args;
        }
    }
}
