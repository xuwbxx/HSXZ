using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.DAO;
using FDScanSystem.Server.Device;
using FDScanSystem.Server.Job;
using Interactive.Utility;


namespace FDScanSystem.Server.Analysis.DataReceiver.MCU
{
    public abstract class MCUDataReceiverBase : IDataReceive
    {
        public Job.DataReceiveArgs Handle(Device.DeviceItem device, byte[] receiverData)
        {
            DataReceiveArgs args = new DataReceiveArgs();
            args.IsClearState = false;
            args.IsHandle = false;

            string fullmsg = Encoding.ASCII.GetString(receiverData);

            //AppContext.Log("收到一条来自MSCU的完整消息:" + fullmsg);
            args = SubHandle(device, args, fullmsg);
            if (args.IsHandle && !args.IsClearState)
            {
                //
                var command = Singleton<CommandQueueManager>.Instance.GetCurrent(device.Key);
                if (command != null && command.ID > 0)
                {
                    var db = DataContextFactory.Create();
                    var dataCommand = db.MK_Scan_Command.FirstOrDefault(u => u.ID == command.ID);
                    if (dataCommand != null)
                    {
                        dataCommand.Result = fullmsg;
                        db.SubmitChanges();
                    }
                    var d = db.MK_Device.FirstOrDefault(u => u.ID == command.DeviceId);
                    if (d != null)
                    {
                        d.Status = 1;
                        db.SubmitChanges();
                    }

                }
            }
            return args;
        }


        public Job.DataReceiveArgs Handle1(Device.DeviceItem device, string receiverData)
        {
            DataReceiveArgs args = new DataReceiveArgs();
            args.IsClearState = false;
            args.IsHandle = false;

            // string fullmsg = Encoding.ASCII.GetString(receiverData);

            //AppContext.Log("收到一条来自MSCU的完整消息:" + fullmsg);
            args = SubHandle(device, args, receiverData);
            if (args.IsHandle && !args.IsClearState)
            {
                //
                var command = Singleton<CommandQueueManager>.Instance.GetCurrent(device.Key);
                if (command != null && command.ID > 0)
                {
                    var db = DataContextFactory.Create();
                    var dataCommand = db.MK_Scan_Command.FirstOrDefault(u => u.ID == command.ID);
                    if (dataCommand != null)
                    {
                        dataCommand.Result = receiverData;
                        db.SubmitChanges();
                    }

                    var d = db.MK_Device.FirstOrDefault(u => u.ID == command.DeviceId);
                    if (d != null)
                    {
                        d.Status = 1;
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
