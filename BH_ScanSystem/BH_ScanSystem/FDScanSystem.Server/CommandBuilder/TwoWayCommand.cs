using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.DAO;
using Interactive.Utility;
using FDScanSystem.Server.Models;

namespace FDScanSystem.Server.Job
{
    /// <summary>
    /// 两次循环的命令
    /// </summary>
    public class TwoWayCommand : CommandBase
    {
        public override int Type
        {
            get { return 3; }
        }

        /// <summary>
        /// 发送次数
        /// </summary>
        public int SendCount { get; set; }

        public override bool Sending()
        {

            if (this.SendCount > 5)
            {

                var db = DataContextFactory.Create();
                if (this.ID > 0)
                {
                    AppContext.Log(string.Format("{0}--{1}--{2}", ID, Identity, this.SendCount));
                    // AppContext.Error(string.Format("{0}--{1}--{2}", ID, Identity, this.SendCount));
                    var command = db.MK_Scan_Command.FirstOrDefault(u => u.ID == ID);
                    if (command != null)
                    {
                        command.Status = (int)CommandStatus.SendOutTimes;
                        db.SubmitChanges();

                        var device = db.MK_Device.FirstOrDefault(u => u.DeviceIden == Identity);
                        if (device != null)
                        {

                            if (device.Status == 1)
                            {
                                var projectName = db.Base_Project.FirstOrDefault().ProjectName;
                                string str = string.Format("{0}项目的设备编号{1}已经断线", projectName, device.DeviceName);
                                //var SMSlist = db.SMS_User.ToList();
                                //foreach(var sms in SMSlist)
                                //{
                                //    Send.SendSMS(sms.Mobile, str);
                                //}
                            }

                            device.Status = 0;
                            db.SubmitChanges();
                            //var point = db.MK_Point.FirstOrDefault(u => u.DeviceId == device.ID);
                            //{
                            //    if (point != null)
                            //    {
                            //        var position = db.MK_Position.FirstOrDefault(u => u.ID == point.PositionId);
                            //        if (position != null)
                            //        {
                            //            var layer = db.MK_Layer.FirstOrDefault(u => u.ID == position.LayerId);
                            //            if (layer != null && layer.LayerType!=1)
                            //            {
                            //                layer.LayerType = 2;
                            //                db.SubmitChanges();
                            //            }
                            //        }
                            //    }
                            //}
                        }

                        

                        Singleton<CommandQueueManager>.Instance.CommandIsDone(this.ConnectionKey);
                        return false;
                    }

                }
            }
            return true;

        }

        public override void Sended()
        {
            base.Sended();
            this.SendCount++;
        }

        public void Offline()
        {
            //

            var db = DataContextFactory.Create();
            if (this.ID > 0)
            {
                var command = db.MK_Scan_Command.FirstOrDefault(u => u.ID == ID);
                if (command != null)
                {
                    command.Status = (int)CommandStatus.SendOutTimes;
                    db.SubmitChanges();

                    Singleton<CommandQueueManager>.Instance.CommandIsDone(this.ConnectionKey);
                }
            }
        }
    }
}
