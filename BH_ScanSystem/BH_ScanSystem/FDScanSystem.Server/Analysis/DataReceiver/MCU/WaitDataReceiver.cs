using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Server.Analysis.DataReceiver.MCU;
using FDScanSystem.Server.Device;
using Interactive.Utility;

namespace FDScanSystem.Server.Job.DataReceiver
{
    public class WaitDataReceiver : MCUDataReceiverBase
    {
        public DataReceiveArgs Handle(int code, string msg)
        {
            DataReceiveArgs args = new DataReceiveArgs();



            return args;
        }

        protected override DataReceiveArgs SubHandle(DeviceItem device, DataReceiveArgs args, string msg)
        {
            //      args.IsHandle = true;

            if (msg.Trim().ToUpperInvariant().Contains("WAIT"))
            {

                args.IsHandle = true;
                var command = Singleton<CommandQueueManager>.Instance.GetCurrent(device.Key);
                if (command is TwoWayCommand)
                {
                    args.IsClearState = true;
                    TwoWayCommand s = (command as TwoWayCommand);
                    s.SendCount++;
                    if (s.SendCount > 10)
                    {
                        s.Offline();
                    }
                }
            }

            return args;
        }
    }
}
