//using DataEntity.DataContext;
using DataEntity.Services;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AlertService
{
    public class PM10SMSService : ISMSService
    {
        //WorkService service = new WorkService();
        //public void SMSSend(double? value)
        //{
        //    var PM10Type = service.Repository_Alarm_SMSType.Query(a => !a.IsDelete.Value && a.Type.Equals(AlarmEnvironmentEnum.PM10.ToString())).FirstOrDefault();
        //    if (PM10Type == null || PM10Type.AlarmValue1 == null)
        //        return;

        //    //判断是否超标(100，150)
        //    bool IsExceed = value > Convert.ToDouble(PM10Type.AlarmValue1) ? true : false;
            
        //    DateTime now = DateTime.Now;

        //    //绑定次预警服务的用户
        //    var UserList = service.Repository_View_Alarm_SMSUser.Query(a => !a.IsDelete.Value && a.TypeID == PM10Type.ID).ToList();
        //    foreach (var user in UserList)
        //    {
        //        if (string.IsNullOrEmpty(user.Mobile))
        //        {
        //            continue;
        //        }
        //        //上次超标，这次不超标；或者上次不超标，这次超标才发短信
        //        if ((IsExceed && !user.IsExceed.Value) || (!IsExceed && user.IsExceed.Value))
        //        {
        //            string msg = "";
        //            if (IsExceed)
        //            {
        //                //超标了发短信
        //                if (value > Convert.ToDouble(PM10Type.AlarmValue2))
        //                {
        //                    msg = "PM10超标警告: " + value.ToString() + " (采集时间：" + now.ToString("yyyy-MM-dd HH:mm:ss") + ")";
        //                }
        //                else
        //                {
        //                    msg = PM10Type.Content1 + ": " + value.ToString() + " (采集时间：" + now.ToString("yyyy-MM-dd HH:mm:ss") + ")";
        //                }
                        
        //            }
        //            else
        //            {
        //                //正常了发短信
        //                msg = PM10Type.Content2 + ": " + value.ToString() + " (采集时间：" + now.ToString("yyyy-MM-dd HH:mm:ss") + ")";
        //            }
        //            bool IfSuccess = Send.SendSMS(user.Mobile, msg);
        //            if (IfSuccess)
        //            {
        //                //更新数据
        //                Alarm_SMSUser se = service.Repository_Alarm_SMSUser.Query(a => a.ID == user.ID).FirstOrDefault();
        //                se.IsExceed = IsExceed;
        //                se.CreateTime = now;
                        
        //            }

        //            //发送成功和失败都记日志
        //            Alarm_SMSLog seLog = new Alarm_SMSLog();
        //            seLog.SMSUserID = user.ID;
        //            seLog.Message = msg;
        //            seLog.SendTime = now;
        //            seLog.IsSuccess = IfSuccess;
        //            service.Repository_Alarm_SMSLog.Insert(seLog);

        //        }
                
        //    }
        //    service.SaveChange();
        }
    }
}
