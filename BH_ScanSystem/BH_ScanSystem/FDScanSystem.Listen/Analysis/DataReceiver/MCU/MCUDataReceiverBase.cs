using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Listen.Device;
using FDScanSystem.Listen.Logic;
using Interactive.Utility;


namespace FDScanSystem.Listen.Analysis.DataReceiver.MCU
{
    public abstract class MCUDataReceiverBase : IDataReceive
    {
        public DataReceiveArgs Handle(Device.DeviceItem device, byte[] receiverData)
        {
            DataReceiveArgs args = new DataReceiveArgs();
            args.IsClearState = false;
            args.IsHandle = false;

            string fullmsg = Encoding.ASCII.GetString(receiverData);

            //AppContext.Log("收到一条来自MSCU的完整消息:" + fullmsg);
            args = SubHandle(device, args, fullmsg);
            if (args.IsHandle && !args.IsClearState)
            {
                var command = Singleton<CommandQueueManager>.Instance.GetCurrent(device.Key);
                if (command != null && command.ID > 0)
                {
                    var db = FDScanSystem.DAO.DataContextFactory.Create();
                    var dataCommand = db.FD_Command.FirstOrDefault(u => u.ID == command.ID);
                    if (dataCommand != null)
                    {
                        dataCommand.Result = fullmsg;
                        db.SubmitChanges();
                    }

                }
            }
            return args;
        }

        protected abstract DataReceiveArgs SubHandle(Device.DeviceItem device, DataReceiveArgs args, string msg);
        //{
        //    args.IsHandle = true;
        //    args.IsClearState = false;
        //    return args;
        //}

    }
}
