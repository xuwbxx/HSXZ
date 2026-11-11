using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using FDScanSystem.DAO;
using FDScanSystem.Helper;
using FDScanSystem.Server;
using FDScanSystem.Server.Analysis.DataReceiver.MCU;
using FDScanSystem.Server.Device;
using Interactive.Utility;

namespace FDScanSystem.Server.Job.DataReceiver
{
    public class OKDataReceiver : MCUDataReceiverBase
    {
        //public DataReceiveArgs Handle(int code, string msg)
        //{
        //    DataReceiveArgs args = new DataReceiveArgs();

        //    if (msg.Trim().ToUpperInvariant().Contains("OK"))
        //    {
        //        args.IsHandle = true;//是否被处理
        //        args.IsClearState = true;//是否被消化

        //        var comm = DeviceConnectionManager.GetInstance().Get(code);
        //        var command = Singleton<CommandQueueManager>.Instance.GetCurrent(code);
        //        Singleton<CommandQueueManager>.Instance.CommandIsDone(code);
        //        if (command is TwoWayCommand)
        //        {


        //            CommandDispatcher.GetInstance().Dispatch(new XPD_Scan_Command
        //            {
        //                Command = "MCU32 {0} Read",
        //                CreateDate = DateTime.Now,
        //                CommunicationIden = command.Identity,
        //                DeviceIden = command.DeviceIdentity,
        //                Status = 0,
        //                ID = command.ID
        //            });
        //        }

        //        AppContext.Log("成功处理了OK信息 ");
        //    }

        //    return args;
        //}

        protected override DataReceiveArgs SubHandle(DeviceItem device, DataReceiveArgs args, string msg)
        {

            if (msg.Trim().ToUpperInvariant().Contains("OK"))
            {
                args.IsHandle = true;//是否被处理

                var command = Singleton<CommandQueueManager>.Instance.GetCurrent(device.Key);
                if (command is TwoWayCommand)
                {

                    Singleton<CommandQueueManager>.Instance.CommandIsDone(device.Key);
                    args.IsClearState = true;//是否被消化

                    var provider = Analysis.COMAnalysisProtocolFactory.Create(Analysis.AnalysisProtocolType.MCU);
                    var newCommand = provider.CommandBuilder.Build(command.ID, command.Identity, "MCU32 {0} Read");
                    newCommand.ConnectionKey = device.Key;
                    newCommand.CommunicationDeviceId = command.CommunicationDeviceId;
                    newCommand.DeviceId = command.DeviceId;

                    CommandDispatcher.GetInstance().Dispatch(newCommand);
                }
                var db = DataContextFactory.Create();
                var d = db.MK_Device.FirstOrDefault(u => u.ID == command.DeviceId);
                if (d != null)
                {
                    d.Status = 1;
                    db.SubmitChanges();
                }
                // LogWriter.Info("成功处理了OK信息 ");
                AppContext.Log("成功处理了OK信息 ");
            }

            return args;
        }
    }
}
