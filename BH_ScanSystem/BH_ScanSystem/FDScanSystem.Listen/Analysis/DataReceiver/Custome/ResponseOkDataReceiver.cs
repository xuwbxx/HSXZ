using FDScanSystem.DAO;
using FDScanSystem.Listen.Logic;
using FDScanSystem.Protocol;
using Interactive.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJFramework10.Extension;
namespace FDScanSystem.Listen.Analysis.DataReceiver.Custome
{
    public class ResponseOkDataReceiver : CusomerDataReceiverBase
    {
        protected override Logic.DataReceiveArgs SubHandle(Device.DeviceItem device, Logic.DataReceiveArgs args, string msg)
        {


            ClientMsgEntity msgEntity = ClientMsgEntity.DeSerialize(msg);

            if (msgEntity != null && msgEntity.Command.Equals(ClientMsgCode.Response_OK))
            {
                args.IsHandle = true;
                var command = Singleton<CommandQueueManager>.Instance.GetCurrent(device.Key);
                if (command != null && command.Identity.Equals(msgEntity.Identity))
                {
                    var db = DataContextFactory.Create();
                    var fdCommand = db.FD_Command.FirstOrDefault(u => u.ID == command.ID);
                    if (fdCommand != null)
                    {
                        fdCommand.Result = "Response Ok";
                        db.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
                    }
                    Singleton<CommandQueueManager>.Instance.CommandIsDone(device.Key);
                }
            }

            //int deviceId = msgEntity.Paramters["DEVICEID"].AsInt();
            //long startTime = msgEntity.Paramters["STARTTIME"].AsInt64();
            //string content = msgEntity.Paramters["Content"].ToString();
            //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Callback), msgEntity);
            ////   FileManager.SaveToFile(deviceId, startTime, content);
            //ClientMsgEntity response = new ClientMsgEntity();
            //response.Command = ClientMsgCode.Response_OK;
            //response.Identity = msgEntity.Identity;
            //args.ResponseMsg = response;
            AppContext.Success(string.Format("收到一条返回成功数据信息 : {0}", msgEntity.Command));
            return args;
        }




    }
}
