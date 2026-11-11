using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Server.Device;
using FDScanSystem.Server.Job;
using FDScanSystem.Server.Job.DataReceiver;

namespace FDScanSystem.Server.Analysis.DataReceiver
{
    public interface IDataReceive
    {
        DataReceiveArgs Handle(DeviceItem device, byte[] receiverData);
        DataReceiveArgs Handle1(DeviceItem device, string receiverData);
    }
}
