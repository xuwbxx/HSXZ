using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FDScanSystem.Client.Logic.ThreadManager;
using FDScanSystem.Protocol;

namespace FDScanSystem.Client.Logic.MsgSender
{
    public class SendDataMsg : ClientToServerMsgBase
    {
        ScanThreadContext _dataInfo;
        string _fileLocation;
        public SendDataMsg(ScanThreadContext dataInfo, string file)
            : base()
        {
            _dataInfo = dataInfo;
            _fileLocation = file;
            if (File.Exists(_fileLocation))
            {
                this.Command = dataInfo.IsAlert ? ClientMsgCode.ClientToServer_DataAlertInfo : ClientMsgCode.ClientToServer_DataInfo;

                AddParameter("DEVICEID", _dataInfo.DeviceId);
                AddParameter("STARTTIME", _dataInfo.StartDateTime);
                AddParameter("TDJZPL", _dataInfo.TDJZPL);
                AddParameter("ZDJZPL", _dataInfo.ZDJZPL);
                AddParameter("Interval", _dataInfo.Interval);


                if (_dataInfo.AlertPointId > 0)
                {
                    AddParameter("POINTID", _dataInfo.AlertPointId);
                    AddParameter("ALERTVAL", _dataInfo.AlertValue);
                }
            }

        }

        public override byte[] ToSerializeMsg()
        {
            if (!this.Paramters.ContainsKey("Content"))
            {
                if (File.Exists(_fileLocation))
                {
                    using (FileStream fs = new FileStream(_fileLocation, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader fsr = new StreamReader(fs, Encoding.UTF8))
                        {
                            AddParameter("Content", fsr.ReadToEnd());
                        }
                    }
                }
            }

            return base.ToSerializeMsg();
        }

        public override void Callback()
        {
            base.Callback();
            if (File.Exists(_fileLocation))
            {
                try
                {
                    File.Delete(_fileLocation);
                }
                catch
                {
                    AppContext.Error(string.Format("删除文件出错:{0}", _fileLocation));
                }
            }
        }

    }
}
