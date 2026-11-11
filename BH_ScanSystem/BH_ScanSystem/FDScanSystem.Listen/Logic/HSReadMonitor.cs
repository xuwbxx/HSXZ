using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using FDScanSystem.DAO;
using FDScanSystem.Listen.Device;
using Interactive.Utility;
using ZJFramework10.Extension;
using FDScanSystem.Listen.Models;

namespace FDScanSystem.Listen.Logic
{
    public class HSReadMonitor : WindMoniterBase
    {
        protected HSReadMonitor() : base() { }
        static WindMoniterBase _myInstance;
        static object _lock = new object();
        protected override void PostData()
        {

            int isupdate = 0;
            var db = DataContextFactory.Create();
            try
            {
                AppContext.Log("执行自动定时采集数据....");
                var device = db.MK_Device.Where(u => u.DeviceProtocol == 2).ToList();
                // var db1 = DataContextFactory1.Create();
                List<DataExport> data = new List<DataExport>();
                FD_DynamicValue2 dyna = new FD_DynamicValue2();
                FD_DynamicValue3 dyna1 = new FD_DynamicValue3();
                dyna.CreateDate = DateTime.Now;
                string name = dyna.CreateDate.Value.ToString("yyyyMM");
                string DBName = "FD_DynamicValue3" + name;
                var Exist = db.ExecuteQuery<FD_DynamicValue3>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();

                if (Exist.ID == 0)
                {
                    string sql1 = string.Format(@"CREATE TABLE [dbo].[{0}](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [int] NULL,
	[PointId] [int] NULL,
	[WaveLength] [float] NULL,
	[Result] [float] NULL,
	[CreateDate] [datetime] NULL,
    [ScanValue] [float] NULL,
    [Status] [int] NULL,
 CONSTRAINT [PK_{1}] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]", DBName, DBName);
                    db.ExecuteCommand(sql1);
                }

                foreach (var d in device)
                {
                    int i = 1;
                    int PositionId = 0;
                    var pointlist = db.MK_Point.Where(u => u.DeviceId == d.ID && Convert.ToInt32(u.MapDataName) > 0 && u.MapDataName != null).OrderBy(u => u.MapDataName).ToList();
                    foreach (var p in pointlist)
                    {

                        //if (!p.PointName.Equals("7PHC-2-1-B"))
                        //{
                        //    continue;
                        //}
                        AppContext.Log(p.PointName + "正在采集");
                        // int isfinish = 0;
                        string sql = string.Format("select Max(CreateDate) from {0} where PointId={1}", DBName, p.ID);
                        var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                        PositionId = p.PositionId ?? 0;
                        DateTime? tempdate = null;
                        if (d.DeviceIden == "yingli115")
                        {
                            var count = db.yingli115.Count();
                            if (count > 0)
                            {

                                List<yingli115> list = new List<yingli115>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    tempdate = date;
                                    list = db.yingli115.Where(u => Convert.ToDateTime(u.SendTime) > date && u.SID.ToString().Equals(p.MapDataName)).ToList();
                                }
                                else
                                {

                                    list = db.yingli115.Where(u => Convert.ToDateTime(u.SendTime) > fdvalue && u.SID.ToString().Equals(p.MapDataName)).ToList();
                                    tempdate = fdvalue;
                                }

                                if (list.Count == 0)
                                {
                                    if (d.Status == 1)
                                    {
                                        var projectName = db.Base_Project.FirstOrDefault().ProjectName;
                                        string str = string.Format("{0}项目的设备编号{1}已经断线", projectName, d.DeviceName);
                                        //var SMSlist = db.SMS_User.ToList();
                                        //foreach (var sms in SMSlist)
                                        //{
                                        //    Send.SendSMS(sms.Mobile, str);
                                        //}
                                    }
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType!=1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        i = 0;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log(p.PointName + "没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }


                                foreach (var l in list)
                                {
                                    var wendu = db.yingli4.FirstOrDefault(u => u.SendTime.Equals(l.SendTime) && u.SID == 124);
                                    double W124 = 0;
                                    if (wendu != null)
                                    {
                                        W124 = wendu.WaveLength.AsDouble(0.0);
                                    }
                                    else
                                    {
                                        W124 = db.yingli4.Where(u=> u.SID == 124).OrderByDescending(u => Convert.ToDateTime(u.SendTime)).FirstOrDefault().WaveLength.AsDouble(0);
                                    }
                                    if (p.MapDataName.AsInt() == l.SID)
                                    {
                                        FD_DynamicValue3 model = new FD_DynamicValue3();
                                        model.PointId = p.ID;
                                        model.WaveLength = l.WaveLength;
                                        model.Result = l.Result.AsDouble();
                                        if (p.PointName.Contains("PHC"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) * 0.038 - (p.Para1 ?? 0);
                                        }
                                        else if (p.PointName.Equals("7HL-X-10"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 109.216) * (W124 - 1552.567)) / (3.14 * 16 * 16) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-X-11"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 92.496) * (W124 - 1552.567)) / (3.14 * 12.5 * 12.5) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-X-12"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 104.434) * (W124 - 1552.567)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-X-13"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 223.816) * (W124 - 1552.567)) / (3.14 * 16 * 16) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-X-14"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 103.879) * (W124 - 1552.567)) / (3.14 * 12.5 * 12.5) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-X-15"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 104.441) * (W124 - 1552.567)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-X-16"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 105.577) * (W124 - 1552.567)) / (3.14 * 16 * 16) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-X-1"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 133.533) * (W124 - 1552.567)) / (3.14 * 12.5 * 12.5) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-X-2"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 136.102) * (W124 - 1552.567)) / (3.14 * 12.5 * 12.5) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-X-3"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 105.484) * (W124 - 1552.567)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-X-4"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 133.15) * (W124 - 1552.567)) / (3.14 * 12.5 * 12.5) * 158;
                                        }
                                        
                                        else if (p.PointName.Equals("7HL-X-9"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 120.68) * (W124 - 1552.567)) / (3.14 * 14 * 14) * 158;
                                        }
                                        
                                        else if (p.PointName.Equals("7HL-S-1"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 220.867) * (W124 - 1552.567)) / (3.14 * 16 * 16) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-S-2"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 221.574) * (W124 - 1552.567)) / (3.14 * 16 * 16) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-S-5"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 90.674) * (W124 - 1552.567)) / (3.14 * 12.5 * 12.5) * 158;
                                        }
                                        else if (p.PointName.Equals("7HL-S-6"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 91.57) * (W124 - 1552.567)) / (3.14 * 12.5 * 12.5) * 158;
                                        }
                                        else if (p.PointName.Equals("7ZL-A-4b"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) / (3.14 * 16 * 16) * 158;
                                        }
                                        else if (p.PointName.Equals("7ZL-A-4"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) / (3.14 * 16 * 16) * 158;
                                        }
                                        else if (p.PointName.Equals("7ZL-A-5"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Equals("47ZL-A-1b"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) / 803.84 * 158;
                                        }
                                        else if (p.PointName.Equals("47ZL-A-1"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) / 314 * 158;
                                        }
                                        else
                                        {
                                            
                                            model.ScanValue = l.Result.AsDouble(0) * 0.0325 - (p.Para1 ?? 0);
                                            if (p.PointName.Equals("447HL-S-6"))
                                            {
                                                model.ScanValue = model.ScanValue * 0.5;
                                            }
                                        }
                                        
                                        model.DeviceId = d.ID;
                                        model.CreateDate = l.SendTime.AsDatetime(DateTime.Now);
                                        if (model.CreateDate > tempdate)
                                        {
                                            tempdate = model.CreateDate;
                                        }
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                        Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                        setData(p, model, DBName);
                                        //isfinish = 1;
                                    }


                                }
                                isupdate = 1;
                                //if (isfinish == 1)
                                AppContext.Log(p.PointName + "数据导入完成......");

                                db.ExecuteCommand(string.Format("delete from yingli115 where SID='{0}' and Convert(datetime,SendTime) <'{1}'", p.MapDataName, tempdate.ToString()));
                                    //list = db.yingli115.Where(u => Convert.ToDateTime(u.SendTime) > date && u.SID.ToString().Equals(p.MapDataName)).ToList();
  


                                //else
                                //    AppContext.Log(p.PointName + "没有数据需要导入......");
                            }
                            else
                            {
                                d.Status = 0;
                                i = 0;
                                db.SubmitChanges();
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType!=1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log(p.PointName + "没有数据需要导入......");
                            }
                        }


                        if (d.DeviceIden == "yingli4")
                        {
                            var count = db.yingli4.Count();
                            if (count > 0)
                            {

                                List<yingli4> list = new List<yingli4>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    tempdate = date;
                                    list = db.yingli4.Where(u => Convert.ToDateTime(u.SendTime) > date && u.SID.ToString().Equals(p.MapDataName)).ToList();
                                }
                                else
                                {
                                    tempdate = fdvalue;
                                    list = db.yingli4.Where(u => Convert.ToDateTime(u.SendTime) > fdvalue && u.SID.ToString().Equals(p.MapDataName)).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    if (d.Status == 1)
                                    {
                                        var projectName = db.Base_Project.FirstOrDefault().ProjectName;
                                        string str = string.Format("{0}项目的设备编号{1}已经断线", projectName, d.DeviceName);
                                        //var SMSlist = db.SMS_User.ToList();
                                        //foreach (var sms in SMSlist)
                                        //{
                                        //    Send.SendSMS(sms.Mobile, str);
                                        //}
                                    }
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType!=1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        i = 0;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log(p.PointName + "没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }


                                foreach (var l in list)
                                {
                                    var wendu = db.yingli115.FirstOrDefault(u => u.SendTime.Equals(l.SendTime) && u.SID == 144);
                                    double W144 = 0;
                                    if (wendu != null)
                                    {
                                        W144 = wendu.WaveLength.AsDouble(0.0);
                                    }
                                    else
                                    {
                                        W144 = db.yingli115.Where(u => u.SID == 144).OrderByDescending(u => Convert.ToDateTime(u.SendTime)).FirstOrDefault().WaveLength.AsDouble(0);
                                    }

                                    var wendu164 = db.yingli4.FirstOrDefault(u => u.SendTime.Equals(l.SendTime) && u.SID == 164);
                                    double W164 = 0;
                                    if (wendu164 != null)
                                    {
                                        W164 = wendu164.WaveLength.AsDouble(0.0);
                                    }
                                    else
                                    {
                                        W164= db.yingli4.Where(u => u.SID == 164).OrderByDescending(u => Convert.ToDateTime(u.SendTime)).FirstOrDefault().WaveLength.AsDouble(0);
                                    }
                                    var wendu124 = db.yingli4.FirstOrDefault(u => u.SendTime.Equals(l.SendTime) && u.SID == 124);
                                    double W124 = 0;
                                    if (wendu124 != null)
                                    {
                                        W124 = wendu124.WaveLength.AsDouble(0.0);
                                    }
                                    else
                                    {
                                        W124 = db.yingli4.Where(u => u.SID == 124).OrderByDescending(u => Convert.ToDateTime(u.SendTime)).FirstOrDefault().WaveLength.AsDouble(0);
                                    }

                                    if (p.MapDataName.AsInt() == l.SID)
                                    {
                                        FD_DynamicValue3 model = new FD_DynamicValue3();
                                        model.PointId = p.ID;
                                        model.WaveLength = l.WaveLength;
                                        model.Result = l.Result.AsDouble();
                                        model.DeviceId = d.ID;
                                        //if (p.PointName.Equals("7PHC-2-1-B"))
                                        //{
                                        //    string para1_Name = "7PHC-2-1-A";
                                        //    string para2_Name = "7PHC-2-1-C";

                                        //    double para1 = getPara(para1_Name, tempdate, DBName);
                                        //    double para2 = getPara(para2_Name, tempdate, DBName);
                                        //    if (para1 == -1 || para2 == -1)
                                        //    {
                                        //        continue;
                                        //    }
                                        //    model.ScanValue = (-1.228 * para1) + (2.228 * para2);

                                        //}
                                        //else 
                                        if (p.PointName.Contains("PHC"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) * 0.038 - (p.Para1 ?? 0);
                                        }
                                        else if (p.PointName.Contains("7ZL-A-2b"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 101.847) * (W144 - 1547.789)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-A-2"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 221.238) * (W144 - 1547.789)) / (3.14 * 16 * 16) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-A-3"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 106.111) * (W144 - 1547.789)) / (3.14 * 16 * 16) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-A-6b"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 106.268) * (W144 - 1547.789)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-A-6"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 107.047) * (W144 - 1547.789)) / (3.14 * 11 * 11) * 158;
                                        }

                                        else if (p.PointName.Contains("7ZL-A-7"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 106.404) * (W144 - 1547.789)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-A-8b"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 36.058) * (W144 - 1547.789)) / (3.14 * 64) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-A-8"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 105.972) * (W144 - 1547.789)) / (3.14 * 11 * 11) * 158;
                                        }

                                        else if (p.PointName.Contains("7ZL-A-9"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 104.78) * (W144 - 1547.789)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-A-12b"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 39.526) * (W144 - 1547.789)) / (3.14 * 100) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-A-12"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 40.034) * (W144 - 1547.789)) / (3.14 * 100) * 158;
                                        }

                                        else if (p.PointName.Contains("7ZL-B-2b"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 78.642) * (W164 - 1547.797)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-B-2"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 77.593) * (W164 - 1547.797)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-B-3"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 78.148) * (W164 - 1547.797)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-B-4b"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) / (3.14 * 100) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-B-4"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) / (3.14 * 16 * 16) * 158;
                                        }

                                        else if (p.PointName.Contains("7ZL-B-5"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) / (3.14 * 81) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-B-6b"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 105.198) * (W164 - 1547.797)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-B-6"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 105.138) * (W164 - 1547.797)) / (3.14 * 11 * 11) * 158;
                                        }

                                        else if (p.PointName.Contains("7ZL-B-7"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 104.356) * (W164 - 1547.797)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-B-8b"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 106.01) * (W164 - 1547.797)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-B-8"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 104.453) * (W164 - 1547.797)) / (3.14 * 11 * 11) * 158;
                                        }

                                        else if (p.PointName.Contains("7ZL-B-9"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 104.243) * (W164 - 1547.797)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-B-10b"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 34.014) * (W164 - 1547.797)) / (3.14 * 81) * 158;
                                        }
                                        else if (p.PointName.Contains("7ZL-B-10"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 34.035) * (W164 - 1547.797)) / (3.14 * 81) * 158;
                                        }

                                        else if (p.PointName.Contains("7ZL-B-11"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 42.938) * (W164 - 1547.797)) / (3.14 * 100) * 158;
                                        }
                                        else if (p.PointName.Contains("7HL-X-5"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 92.572) * (W124 - 1552.567)) / (3.14 * 12.5 * 12.5) * 158;
                                        }
                                        else if (p.PointName.Contains("7HL-X-6"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 221.629) * (W124 - 1552.567)) / (3.14 * 16 * 16) * 158;
                                        }
                                        else if (p.PointName.Contains("7HL-X-7"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 121.133) * (W124 - 1552.567)) / (3.14 * 14 * 14) * 158;
                                        }
                                        else if (p.PointName.Contains("7HL-X-8"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 104.412) * (W124 - 1552.567)) / (3.14 * 11 * 11) * 158;
                                        }
                                        else
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) * 0.0325 - (p.Para1 ?? 0);
                                        }
                                        model.CreateDate = l.SendTime.AsDatetime(DateTime.Now);
                                        if (model.CreateDate > tempdate)
                                        {
                                            tempdate = model.CreateDate;
                                        }
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                        Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                        setData(p, model, DBName);
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log(p.PointName + "数据导入完成......");

                              
                                db.ExecuteCommand(string.Format("delete from yingli4 where SID='{0}' and Convert(datetime,SendTime) <'{1}'", p.MapDataName, tempdate.ToString()));
                                    //list = db.yingli115.Where(u => Convert.ToDateTime(u.SendTime) > date && u.SID.ToString().Equals(p.MapDataName)).ToList();

                            }
                            else
                            {
                                d.Status = 0;
                                i = 0;
                                db.SubmitChanges();
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType!=1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log(p.PointName + "没有数据需要导入......");
                            }
                        }

                        if (d.DeviceIden == "yingli195")
                        {
                            var count = db.yingli195.Count();
                            if (count > 0)
                            {

                                List<yingli195> list = new List<yingli195>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    tempdate = date;
                                    list = db.yingli195.Where(u => Convert.ToDateTime(u.SendTime) > date && u.SID.ToString().Equals(p.MapDataName)).ToList();
                                }
                                else
                                {
                                    tempdate = fdvalue;
                                    list = db.yingli195.Where(u => Convert.ToDateTime(u.SendTime) > fdvalue && u.SID.ToString().Equals(p.MapDataName)).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    if (d.Status == 1)
                                    {
                                        var projectName = db.Base_Project.FirstOrDefault().ProjectName;
                                        string str = string.Format("{0}项目的设备编号{1}已经断线", projectName, d.DeviceName);
                                        //var SMSlist = db.SMS_User.ToList();
                                        //foreach (var sms in SMSlist)
                                        //{
                                        //    Send.SendSMS(sms.Mobile, str);
                                        //}
                                    }
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType!=1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        i = 0;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log(p.PointName + "没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }


                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt() == l.SID)
                                    {
                                        FD_DynamicValue3 model = new FD_DynamicValue3();
                                        var wendu = db.yingli195.FirstOrDefault(u => u.SendTime.Equals(l.SendTime) && u.SID == 234);
                                        double W234 = 0;
                                        if (wendu != null)
                                        {
                                            W234 = wendu.WaveLength.AsDouble(0.0);
                                        }
                                        else
                                        {
                                            W234 = db.yingli195.Where(u => u.SID == 234).OrderByDescending(u => Convert.ToDateTime(u.SendTime)).FirstOrDefault().WaveLength.AsDouble(0);
                                        }

                                        model.PointId = p.ID;
                                        model.WaveLength = l.WaveLength;
                                        model.Result = l.Result.AsDouble();
                                        if (p.PointName.Contains("PHC"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) * 0.038 - (p.Para1 ?? 0);
                                        }
                                        else if (p.PointName.Contains("47ZL-B-4b") || p.PointName.Contains("47ZL-B-4") || p.PointName.Contains("47ZL-B-5"))
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) / 314 * 158;
                                        }
                                        else if (p.PointName.Contains("47ZL-B-1b"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 41.997) * (W234 - 1541.738)) / (3.14 * 100) * 158;
                                        }
                                        else if (p.PointName.Contains("47ZL-B-1"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 38.411) * (W234 - 1541.738)) / (3.14 * 100) * 158;
                                        }
                                       
                                        else if (p.PointName.Contains("47ZL-B-6b"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 133.582) * (W234 - 1541.738)) / (3.14 * 16 * 16) * 158;
                                        }
                                        else if (p.PointName.Contains("47ZL-B-6"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 33.86) * (W234 - 1541.738)) / (3.14 * 16 * 16) * 158;
                                        }
                                       
                                        else if (p.PointName.Contains("47ZL-B-7"))
                                        {
                                            model.ScanValue = (l.Result.AsDouble(0) - (2.3 * 105.47) * (W234 - 1541.738)) / (3.14 * 16 * 16) * 158;
                                        }
                                        else
                                        {
                                            model.ScanValue = l.Result.AsDouble(0) * 0.0325 - (p.Para1 ?? 0);
                                        }
                                        //else
                                        //{
                                        //    model.ScanValue = l.Result.AsDouble(0) * 0.0325 - (p.Para1 ?? 0);
                                        //}
                                        model.DeviceId = d.ID;
                                        model.CreateDate = l.SendTime.AsDatetime(DateTime.Now);
                                        //  string str = string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6},{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                        Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                        setData(p, model, DBName);
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log(p.PointName + "数据导入完成......");

                                 db.ExecuteCommand(string.Format("delete from yingli195 where SID='{0}' and Convert(datetime,SendTime) <'{1}'", p.MapDataName, tempdate.ToString()));
                                    //list = db.yingli115.Where(u => Convert.ToDateTime(u.SendTime) > date && u.SID.ToString().Equals(p.MapDataName)).ToList();

                            }
                            else
                            {
                                d.Status = 0;
                                i = 0;
                                db.SubmitChanges();
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType!=1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log(p.PointName + "没有数据需要导入......");
                            }
                        }



                    }
                }


               
            
            

            }
            catch (Exception ex)
            {
                AppContext.Log(ex.Message);
            }

        }
        public void Alarm(int PointId, DateTime CreateDate, double MaxNum, double MaxNum2, double MaxNum3, double MinNum, double MinNum2, double MinNum3, double ScanValue)
        {

            var db = DataContextFactory.Create();
            MK_Alert model = new MK_Alert();
            model.PointId = PointId;
            model.CreateDate = CreateDate;
            model.Status = 0;
            model.ScanValue = ScanValue;
            if (MaxNum3.AsDouble(0) != 0 && ScanValue.AsDouble() > MaxNum3)
            {
                model.Level = 3;
                model.AlertValue = MaxNum3;
            }
            else if (MaxNum2.AsDouble(0) != 0 && ScanValue.AsDouble(0) > MaxNum2)
            {
                model.Level = 2;
                model.AlertValue = MaxNum2;
            }
            else if (MaxNum.AsDouble(0) != 0 && ScanValue.AsDouble(0) > MaxNum)
            {
                model.Level = 1;
                model.AlertValue = MaxNum;
            }
            else if (MinNum3.AsDouble(0) != 0 && ScanValue.AsDouble() < MinNum3)
            {
                model.Level = 3;
                model.AlertValue = MinNum3;
            }


            else if (MinNum2.AsDouble(0) != 0 && ScanValue.AsDouble(0) < MinNum2)
            {
                model.Level = 2;
                model.AlertValue = MinNum2;
            }
            else if (MinNum.AsDouble(0) != 0 && ScanValue.AsDouble(0) < MinNum)
            {

                model.Level = 1;
                model.AlertValue = MinNum;
            }
            else
            {
                return;
            }
            db.MK_Alert.InsertOnSubmit(model);
            db.SubmitChanges();

        }

        public double? get47PHC(double? para1, double? para2, double? para3, double? para4,double? para5,double? para6,string s)
        {
            double? step1 = ((para1 + para2) * 190.788 - 66.683) * 0.00262;
            double? step2 = ((para1 - para2 + para3 + para4) * 52.198 - 28.305) * para5;
            double? step3 = (para1 - para2) * 52.198;
            double? step4 = (para1 - para2 - para3 + para4) * 52.198 * 8;
            if(s.Contains("A"))
            {
                return (step1 + (step2 + (step3 + step4 - 113.211) * para6)) * 0.0096;
            }
            return (step1 - (step2 + (step3 + step4 - 113.211) * para6)) * 0.0096;

        }

    

        public double? get7PHC_B(double? para1,double? para2,double? para3,double? para4)
        {
            return para1 - para3 + ((para1 + para2) / 2) - para4;
        }

        public double? get7PHC_C(double? para1, double? para2, double? para3, double? para4,double para5,int number)
        {
            if (number == 2)
            {
                return para4 - (para3 - ((para1 + para2) / 2) + para5) * 2.203;
            }
            return para4 - (para3 - ((para1 + para2) / 2) + para5) / 0.468;
        }

        public double getPara(string PointName,DateTime? CreateDate,string DBName)
        {
            var db = DataContextFactory.Create();
            string sql = string.Format("select ScanValue from {0} where PointId in (select ID from MK_Point where PointName='{1}') and CreateDate='{2}'", DBName, PointName, CreateDate);
            double? value= db.ExecuteQuery<double?>(sql).FirstOrDefault();
            if(value==null)
            {
                return -1;
            }
            return value.AsDouble(0);

        }

        public void setData(MK_Point p,FD_DynamicValue3 l,string DBName)
        {
            var db = DataContextFactory.Create();
            var templist = db.MK_Point.Where(u => u.NewID == p.ID).ToList();
            foreach (var temp in templist)
            {
                FD_DynamicValue3 model = new FD_DynamicValue3();
                model.PointId = temp.ID;
                model.WaveLength = l.WaveLength;
                model.Result = l.Result.AsDouble();
                model.DeviceId = temp.DeviceId;
                model.CreateDate = l.CreateDate;
                model.ScanValue = l.ScanValue + (p.Para1 ?? 0);
                //if (temp.PointName.Contains("PHC"))
                //{
                //    model.ScanValue = l.Result.AsDouble(0) * 0.038 - (p.Para1 ?? 0);
                //}
                //else
                //{
                //    model.ScanValue = l.Result.AsDouble(0) * 0.0325 - (p.Para1 ?? 0);
                //}

                db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
            }
            //AppContext.Log(p.PointName + "数据导入完成......");

        }
        /// <summary>
        /// 获取单例实例
        /// </summary>
        /// <returns></returns>
        public static WindMoniterBase GetInstance()
        {
            if (_myInstance == null)
            {
                lock (_lock)
                {
                    if (_myInstance == null)
                    {
                        _myInstance = new HSReadMonitor();
                    }
                }
            }
            return _myInstance;
        }
    }

}
