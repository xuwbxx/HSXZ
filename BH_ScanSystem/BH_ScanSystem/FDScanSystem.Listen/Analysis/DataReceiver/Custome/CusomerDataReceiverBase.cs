using FDScanSystem.Listen.Logic;
using Interactive.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Listen.Analysis.DataReceiver.Custome
{
    public abstract class CusomerDataReceiverBase : IDataReceive
    {
        public DataReceiveArgs Handle(Device.DeviceItem device, byte[] receiverData)
        {
            DataReceiveArgs args = new DataReceiveArgs();
            args.IsClearState = false;
            args.IsHandle = false;

            string fullmsg = Encoding.UTF8.GetString(receiverData);

            //AppContext.Log("收到一条来自MSCU的完整消息:" + fullmsg);
            args = SubHandle(device, args, fullmsg);
            if (args.IsHandle && !args.IsClearState)
            {


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
