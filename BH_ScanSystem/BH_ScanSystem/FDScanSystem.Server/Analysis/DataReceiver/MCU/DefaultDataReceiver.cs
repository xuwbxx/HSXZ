using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Server.Analysis.DataReceiver.MCU;

namespace FDScanSystem.Server.Job.DataReceiver {
    public class DefaultDataReceiver : MCUDataReceiverBase {

        protected override DataReceiveArgs SubHandle(Device.DeviceItem device, DataReceiveArgs args, string msg) {
            args.IsHandle = true;//是否被处理
            return args;
        }
    }
}
