using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Interactive.Utility;
using System.Data;
using System.Data.SqlClient;
using ZJFramework10.Extension;
using FDScanSystem.Listen.Device;
using FDScanSystem.Listen.Analysis.DataReceiver.MCU;


namespace FDScanSystem.Listen.Logic.DataReceiver
{
    public class DetailDataReceiver : MCUDataReceiverBase
    {

        public DataReceiveArgs Handle(int code, string msg)
        {
            DataReceiveArgs args = new DataReceiveArgs();
            return args;
        }

        protected override DataReceiveArgs SubHandle(DeviceItem device, DataReceiveArgs args, string msg)
        {

            if (msg.Trim().Length > 20)
            {
                args.IsHandle = true;

                getString(msg, device.Key);

                AppContext.Log("成功处理了Detail信息 ");
                //Insert to DB
            }
            return args;
        }

        private void getString(string str, string connectionKey)
        {
         


        }






    }
}
