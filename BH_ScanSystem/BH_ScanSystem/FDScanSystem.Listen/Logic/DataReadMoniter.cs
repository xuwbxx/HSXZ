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
    public class DataReadMoniter : WindMoniterBase
    {
        protected DataReadMoniter() : base() { }
        static WindMoniterBase _myInstance;
        static object _lock = new object();
        /// <summary>
        /// 执行定时读取
        /// </summary>
        protected override void PostData()
        {
            int isupdate = 0;
            var db = DataContextFactory.Create();
            try
            {
                
                var device = db.MK_Device.Where(u => u.DeviceProtocol == 2).ToList();
                var db1 = DataContextFactory1.Create();
                List<DataExport> data = new List<DataExport>();
                FD_DynamicValue2 dyna = new FD_DynamicValue2();
                dyna.CreateDate = DateTime.Now;
                string name = dyna.CreateDate.Value.ToString("yyyyMM");
                string DBName = "FD_DynamicValue2" + name;
                var Exist = db.ExecuteQuery<FD_DynamicValue2>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                //                if (Exist.ID == 0)
                //                {
                //                    string sql1 = string.Format(@"CREATE TABLE [dbo].[{0}](
                //	[ID] [int] IDENTITY(1,1) NOT NULL,
                //	[DeviceId] [int] NULL,
                //	[PointId] [int] NULL,
                //	[MAXValue] [float] NULL,
                //	[YXValue] [float] NULL,
                //	[AverageValue] [float] NULL,
                //    [RateValue] [float] NULL,
                //	[CreateDate] [datetime] NULL,
                //	[DynamicId] [int] NULL,
                //    [Status] [int] NULL,
                // CONSTRAINT [PK_{1}] PRIMARY KEY CLUSTERED 
                //(
                //	[ID] ASC
                //)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                //) ON [PRIMARY]", DBName, DBName);
                //                    db.ExecuteCommand(sql1);
                //                }
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
                    var pointlist = db.MK_Point.Where(u => u.DeviceId == d.ID).OrderBy(u => u.MapDataName).ToList();
                    foreach (var p in pointlist)
                    {
                        string sql = string.Format("select Max(CreateDate) from {0} where PointId={1}", DBName, p.ID);
                        var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                        PositionId = p.PositionId ?? 0;
                        if (d.DeviceIden == "ChNo1_0")
                        {
                            var count = db1.ChNo1_0.Count();
                            if (count > 0)
                            {

                                List<ChNo1_0> list = new List<ChNo1_0>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ChNo1_0.Where(u => Convert.ToDateTime(u.CollectTime) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ChNo1_0.Where(u => Convert.ToDateTime(u.CollectTime) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    if(d.Status==1)
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
                                    AppContext.Log("ChNo1_0没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }


                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue3 model = new FD_DynamicValue3();
                                        model.PointId = p.ID;
                                        model.WaveLength = l.wave0;
                                        model.Result = l.power0;
                                        model.Status = 0;
                                        //model.AverageValue = l.通道1均值;
                                        //model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.ScanValue = 0;
                                       // model.DynamicId = 0;
                                        model.CreateDate = l.CollectTime.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue3 model = new FD_DynamicValue3();
                                        model.PointId = p.ID;
                                        model.WaveLength = l.wave1;
                                        model.Result = l.power1;
                                        model.Status = 0;
                                        //model.AverageValue = l.通道1均值;
                                        //model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.ScanValue = 0;
                                        // model.DynamicId = 0;
                                        model.CreateDate = l.CollectTime.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 3)
                                    {
                                        FD_DynamicValue3 model = new FD_DynamicValue3();
                                        model.PointId = p.ID;
                                        model.WaveLength = l.wave2;
                                        model.Result = l.power2;
                                        model.Status = 0;
                                        //model.AverageValue = l.通道1均值;
                                        //model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.ScanValue = 0;
                                        // model.DynamicId = 0;
                                        model.CreateDate = l.CollectTime.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 4)
                                    {
                                        FD_DynamicValue3 model = new FD_DynamicValue3();
                                        model.PointId = p.ID;
                                        model.WaveLength = l.wave3;
                                        model.Result = l.power3;
                                        model.Status = 0;
                                        //model.AverageValue = l.通道1均值;
                                        //model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.ScanValue = 0;
                                        // model.DynamicId = 0;
                                        model.CreateDate = l.CollectTime.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 5)
                                    {
                                        FD_DynamicValue3 model = new FD_DynamicValue3();
                                        model.PointId = p.ID;
                                        model.WaveLength = l.wave4;
                                        model.Result = l.power4;
                                        model.Status = 0;
                                        //model.AverageValue = l.通道1均值;
                                        //model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.ScanValue = 0;
                                        // model.DynamicId = 0;
                                        model.CreateDate = l.CollectTime.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 6)
                                    {
                                        FD_DynamicValue3 model = new FD_DynamicValue3();
                                        model.PointId = p.ID;
                                        model.WaveLength = l.wave5;
                                        model.Result = l.power5;
                                        model.Status = 0;
                                        //model.AverageValue = l.通道1均值;
                                        //model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.ScanValue = 0;
                                        // model.DynamicId = 0;
                                        model.CreateDate = l.CollectTime.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 7)
                                    {
                                        FD_DynamicValue3 model = new FD_DynamicValue3();
                                        model.PointId = p.ID;
                                        model.WaveLength = l.wave6;
                                        model.Result = l.power6;
                                        model.Status = 0;
                                        //model.AverageValue = l.通道1均值;
                                        //model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.ScanValue = 0;
                                        // model.DynamicId = 0;
                                        model.CreateDate = l.CollectTime.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 8)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道8峰峰值;
                                        model.YXValue = l.通道8有效值;
                                        model.AverageValue = l.通道8均值;
                                        model.RateValue = l.通道8主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 9)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道9峰峰值;
                                        model.YXValue = l.通道9有效值;
                                        model.AverageValue = l.通道9均值;
                                        model.RateValue = l.通道9主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 10)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道10峰峰值;
                                        model.YXValue = l.通道10有效值;
                                        model.AverageValue = l.通道10均值;
                                        model.RateValue = l.通道10主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 11)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道11峰峰值;
                                        model.YXValue = l.通道11有效值;
                                        model.AverageValue = l.通道11均值;
                                        model.RateValue = l.通道11主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 12)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道12峰峰值;
                                        model.YXValue = l.通道12有效值;
                                        model.AverageValue = l.通道12均值;
                                        model.RateValue = l.通道12主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 13)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道13峰峰值;
                                        model.YXValue = l.通道13有效值;
                                        model.AverageValue = l.通道13均值;
                                        model.RateValue = l.通道13主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 14)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道14峰峰值;
                                        model.YXValue = l.通道14有效值;
                                        model.AverageValue = l.通道14均值;
                                        model.RateValue = l.通道14主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 15)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道15峰峰值;
                                        model.YXValue = l.通道15有效值;
                                        model.AverageValue = l.通道15均值;
                                        model.RateValue = l.通道15主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 16)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道16峰峰值;
                                        model.YXValue = l.通道16有效值;
                                        model.AverageValue = l.通道16均值;
                                        model.RateValue = l.通道16主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }



                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_升压站一层振动数据导入完成......");
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
                                AppContext.Log("ZDXQ_升压站一层振动没有数据需要导入......");
                            }
                        }
                        if (d.DeviceIden == "ZDXQ_42号_升压站二层振动")
                        {
                            var count = db1.ZDXQ_42号_升压站二层振动.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_42号_升压站二层振动> list = new List<ZDXQ_42号_升压站二层振动>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_42号_升压站二层振动.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_42号_升压站二层振动.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
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
                                    i = 0;
                                    db.SubmitChanges();
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_升压站二层振动没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }

                                //var date = db1.ZDXQ_2.Max(u => u.时间).AsDatetime(DateTime.Now);
                                //var list = db1.ZDXQ_2.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 3)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道3峰峰值;
                                        model.YXValue = l.通道3有效值;
                                        model.AverageValue = l.通道3均值;
                                        model.RateValue = l.通道3主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 4)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道4峰峰值;
                                        model.YXValue = l.通道4有效值;
                                        model.AverageValue = l.通道4均值;
                                        model.RateValue = l.通道4主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 5)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道5峰峰值;
                                        model.YXValue = l.通道5有效值;
                                        model.RateValue = l.通道5主频率值;
                                        model.AverageValue = l.通道5均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 6)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道6峰峰值;
                                        model.YXValue = l.通道6有效值;
                                        model.RateValue = l.通道6主频率值;
                                        model.AverageValue = l.通道6均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 7)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道7峰峰值;
                                        model.YXValue = l.通道7有效值;
                                        model.AverageValue = l.通道7均值;
                                        model.RateValue = l.通道7主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 8)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道8峰峰值;
                                        model.YXValue = l.通道8有效值;
                                        model.AverageValue = l.通道8均值;
                                        model.RateValue = l.通道8主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 9)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道9峰峰值;
                                        model.YXValue = l.通道9有效值;
                                        model.AverageValue = l.通道9均值;
                                        model.RateValue = l.通道9主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 10)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道10峰峰值;
                                        model.YXValue = l.通道10有效值;
                                        model.AverageValue = l.通道10均值;
                                        model.RateValue = l.通道10主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 11)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道11峰峰值;
                                        model.YXValue = l.通道11有效值;
                                        model.AverageValue = l.通道11均值;
                                        model.RateValue = l.通道11主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 12)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道12峰峰值;
                                        model.YXValue = l.通道12有效值;
                                        model.AverageValue = l.通道12均值;
                                        model.RateValue = l.通道12主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    

                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_升压站二层振动数据导入完成......");
                            }
                            else
                            {
                                AppContext.Log("ZDXQ_升压站二层振动没有数据需要导入......");
                                d.Status = 0;
                                i = 0;
                                db.SubmitChanges();
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                            }
                        }
                        if (d.DeviceIden == "ZDXQ_43号_升压站三层振动")
                        {
                            var count = db1.ZDXQ_43号_升压站三层振动.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_43号_升压站三层振动> list = new List<ZDXQ_43号_升压站三层振动>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_43号_升压站三层振动.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_43号_升压站三层振动.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
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
                                   // i = 0;
                                    AppContext.Log("ZDXQ_升压站三层振动没有数据需要导入......");
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    db.SubmitChanges();
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 3)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道3峰峰值;
                                        model.YXValue = l.通道3有效值;
                                        model.AverageValue = l.通道3均值;
                                        model.RateValue = l.通道3主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 4)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道4峰峰值;
                                        model.YXValue = l.通道4有效值;
                                        model.AverageValue = l.通道4均值;
                                        model.RateValue = l.通道4主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 5)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道5峰峰值;
                                        model.YXValue = l.通道5有效值;
                                        model.RateValue = l.通道5主频率值;
                                        model.AverageValue = l.通道5均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 6)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道6峰峰值;
                                        model.YXValue = l.通道6有效值;
                                        model.RateValue = l.通道6主频率值;
                                        model.AverageValue = l.通道6均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 7)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道7峰峰值;
                                        model.YXValue = l.通道7有效值;
                                        model.AverageValue = l.通道7均值;
                                        model.RateValue = l.通道7主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 8)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道8峰峰值;
                                        model.YXValue = l.通道8有效值;
                                        model.AverageValue = l.通道8均值;
                                        model.RateValue = l.通道8主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 9)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道9峰峰值;
                                        model.YXValue = l.通道9有效值;
                                        model.AverageValue = l.通道9均值;
                                        model.RateValue = l.通道9主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 10)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道10峰峰值;
                                        model.YXValue = l.通道10有效值;
                                        model.AverageValue = l.通道10均值;
                                        model.RateValue = l.通道10主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 11)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道11峰峰值;
                                        model.YXValue = l.通道11有效值;
                                        model.AverageValue = l.通道11均值;
                                        model.RateValue = l.通道11主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 12)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道12峰峰值;
                                        model.YXValue = l.通道12有效值;
                                        model.AverageValue = l.通道12均值;
                                        model.RateValue = l.通道12主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }

                                    

                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_升压站三层振动数据导入完成......");
                            }
                            else
                            {
                                AppContext.Log("ZDXQ_升压站三层振动没有数据需要导入......");
                                i = 0;
                                d.Status = 0;
                                db.SubmitChanges();
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                            }
                        }
                        if (d.DeviceIden == "ZDXQ_44号_升压站1至4倾斜角")
                        {
                            var count = db1.ZDXQ_44号_升压站1至4倾斜角.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_44号_升压站1至4倾斜角> list = new List<ZDXQ_44号_升压站1至4倾斜角>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_44号_升压站1至4倾斜角.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_44号_升压站1至4倾斜角.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
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
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_升压站1至4倾角没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 3)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道3峰峰值;
                                        model.YXValue = l.通道3有效值;
                                        model.AverageValue = l.通道3均值;
                                        model.RateValue = l.通道3主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 4)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道4峰峰值;
                                        model.YXValue = l.通道4有效值;
                                        model.AverageValue = l.通道4均值;
                                        model.RateValue = l.通道4主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 5)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道5峰峰值;
                                        model.YXValue = l.通道5有效值;
                                        model.RateValue = l.通道5主频率值;
                                        model.AverageValue = l.通道5均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 6)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道6峰峰值;
                                        model.YXValue = l.通道6有效值;
                                        model.RateValue = l.通道6主频率值;
                                        model.AverageValue = l.通道6均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 7)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道7峰峰值;
                                        model.YXValue = l.通道7有效值;
                                        model.AverageValue = l.通道7均值;
                                        model.RateValue = l.通道7主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 8)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道8峰峰值;
                                        model.YXValue = l.通道8有效值;
                                        model.AverageValue = l.通道8均值;
                                        model.RateValue = l.通道8主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    

                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_升压站1至4倾角数据导入完成......");
                            }
                            else
                            {
                                AppContext.Log("ZDXQ_升压站1至4倾角没有数据需要导入......");
                                d.Status = 0;
                                i = 0;
                                db.SubmitChanges();
                              //  i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                            }
                        }
                        if (d.DeviceIden == "ZDXQ_45号_升压站5至8倾斜角")
                        {
                            var count = db1.ZDXQ_45号_升压站5至8倾斜角.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_45号_升压站5至8倾斜角> list = new List<ZDXQ_45号_升压站5至8倾斜角>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_45号_升压站5至8倾斜角.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_45号_升压站5至8倾斜角.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
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
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_升压站5至8倾角没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 3)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道3峰峰值;
                                        model.YXValue = l.通道3有效值;
                                        model.AverageValue = l.通道3均值;
                                        model.RateValue = l.通道3主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 4)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道4峰峰值;
                                        model.YXValue = l.通道4有效值;
                                        model.AverageValue = l.通道4均值;
                                        model.RateValue = l.通道4主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 5)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道5峰峰值;
                                        model.YXValue = l.通道5有效值;
                                        model.RateValue = l.通道5主频率值;
                                        model.AverageValue = l.通道5均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 6)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道6峰峰值;
                                        model.YXValue = l.通道6有效值;
                                        model.RateValue = l.通道6主频率值;
                                        model.AverageValue = l.通道6均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 7)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道7峰峰值;
                                        model.YXValue = l.通道7有效值;
                                        model.AverageValue = l.通道7均值;
                                        model.RateValue = l.通道7主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 8)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道8峰峰值;
                                        model.YXValue = l.通道8有效值;
                                        model.AverageValue = l.通道8均值;
                                        model.RateValue = l.通道8主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId,0));
                                    }
                                    

                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_升压站5至8倾角数据导入完成......");

                            }
                            else
                            {
                                AppContext.Log("ZDXQ_升压站5至8倾角没有数据需要导入......");
                                d.Status = 0;
                                i = 0;
                                db.SubmitChanges();

                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                            }
                        }
                        if (d.DeviceIden == "ZDXQ_15号_倾斜加速度监测")
                        {
                            var count = db1.ZDXQ_15号_倾斜加速度监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_15号_倾斜加速度监测> list = new List<ZDXQ_15号_倾斜加速度监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_15号_倾斜加速度监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_15号_倾斜加速度监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}

                                    AppContext.Log("ZDXQ_15号风机_倾斜加速度监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //    db.SubmitChanges();
                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 3)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道3峰峰值;
                                        model.YXValue = l.通道3有效值;
                                        model.AverageValue = l.通道3均值;
                                        model.RateValue = l.通道3主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 4)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道4峰峰值;
                                        model.YXValue = l.通道4有效值;
                                        model.AverageValue = l.通道4均值;
                                        model.RateValue = l.通道4主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 5)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道5峰峰值;
                                        model.YXValue = l.通道5有效值;
                                        model.RateValue = l.通道5主频率值;
                                        model.AverageValue = l.通道5均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 6)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道6峰峰值;
                                        model.YXValue = l.通道6有效值;
                                        model.RateValue = l.通道6主频率值;
                                        model.AverageValue = l.通道6均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 7)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道7峰峰值;
                                        model.YXValue = l.通道7有效值;
                                        model.AverageValue = l.通道7均值;
                                        model.RateValue = l.通道7主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 8)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道8峰峰值;
                                        model.YXValue = l.通道8有效值;
                                        model.AverageValue = l.通道8均值;
                                        model.RateValue = l.通道8主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 9)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道9峰峰值;
                                        model.YXValue = l.通道9有效值;
                                        model.AverageValue = l.通道9均值;
                                        model.RateValue = l.通道9主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 10)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道10峰峰值;
                                        model.YXValue = l.通道10有效值;
                                        model.AverageValue = l.通道10均值;
                                        model.RateValue = l.通道10主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 11)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道11峰峰值;
                                        model.YXValue = l.通道11有效值;
                                        model.AverageValue = l.通道11均值;
                                        model.RateValue = l.通道11主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 12)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道12峰峰值;
                                        model.YXValue = l.通道12有效值;
                                        model.AverageValue = l.通道12均值;
                                        model.RateValue = l.通道12主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 13)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道13峰峰值;
                                        model.YXValue = l.通道13有效值;
                                        model.AverageValue = l.通道13均值;
                                        model.RateValue = l.通道13主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 14)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道14峰峰值;
                                        model.YXValue = l.通道14有效值;
                                        model.AverageValue = l.通道14均值;
                                        model.RateValue = l.通道14主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 15)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道15峰峰值;
                                        model.YXValue = l.通道15有效值;
                                        model.AverageValue = l.通道15均值;
                                        model.RateValue = l.通道15主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 16)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道16峰峰值;
                                        model.YXValue = l.通道16有效值;
                                        model.AverageValue = l.通道16均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_15号风机_倾斜加速度监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_15号风机_倾斜加速度监测没有数据需要导入......");
                            }
                        }
                        if (d.DeviceIden == "ZDXQ_23号_倾斜加速度监测")
                        {
                            var count = db1.ZDXQ_23号_倾斜加速度监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_23号_倾斜加速度监测> list = new List<ZDXQ_23号_倾斜加速度监测> ();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_23号_倾斜加速度监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_23号_倾斜加速度监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    i = 0;
                                    db.SubmitChanges();
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_23号风机_倾斜加速度监测没有数据需要导入......");
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();
                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 3)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道3峰峰值;
                                        model.YXValue = l.通道3有效值;
                                        model.AverageValue = l.通道3均值;
                                        model.RateValue = l.通道3主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 4)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道4峰峰值;
                                        model.YXValue = l.通道4有效值;
                                        model.AverageValue = l.通道4均值;
                                        model.RateValue = l.通道4主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 5)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道5峰峰值;
                                        model.YXValue = l.通道5有效值;
                                        model.RateValue = l.通道5主频率值;
                                        model.AverageValue = l.通道5均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 6)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道6峰峰值;
                                        model.YXValue = l.通道6有效值;
                                        model.RateValue = l.通道6主频率值;
                                        model.AverageValue = l.通道6均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 7)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道7峰峰值;
                                        model.YXValue = l.通道7有效值;
                                        model.AverageValue = l.通道7均值;
                                        model.RateValue = l.通道7主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 8)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道8峰峰值;
                                        model.YXValue = l.通道8有效值;
                                        model.AverageValue = l.通道8均值;
                                        model.RateValue = l.通道8主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 9)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道9峰峰值;
                                        model.YXValue = l.通道9有效值;
                                        model.AverageValue = l.通道9均值;
                                        model.RateValue = l.通道9主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 10)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道10峰峰值;
                                        model.YXValue = l.通道10有效值;
                                        model.AverageValue = l.通道10均值;
                                        model.RateValue = l.通道10主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 11)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道11峰峰值;
                                        model.YXValue = l.通道11有效值;
                                        model.AverageValue = l.通道11均值;
                                        model.RateValue = l.通道11主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 12)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道12峰峰值;
                                        model.YXValue = l.通道12有效值;
                                        model.AverageValue = l.通道12均值;
                                        model.RateValue = l.通道12主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 13)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道13峰峰值;
                                        model.YXValue = l.通道13有效值;
                                        model.AverageValue = l.通道13均值;
                                        model.RateValue = l.通道13主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 14)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道14峰峰值;
                                        model.YXValue = l.通道14有效值;
                                        model.AverageValue = l.通道14均值;
                                        model.RateValue = l.通道14主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 15)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道15峰峰值;
                                        model.YXValue = l.通道15有效值;
                                        model.AverageValue = l.通道15均值;
                                        model.RateValue = l.通道15主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 16)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道16峰峰值;
                                        model.YXValue = l.通道16有效值;
                                        model.AverageValue = l.通道16均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_23号风机_倾斜加速度监测数据导入完成......");
                            }
                            else
                            {
                                AppContext.Log("ZDXQ_23号风机_倾斜加速度监测没有数据需要导入......");
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                            }
                        }

                        if (d.DeviceIden == "ZDXQ_3号_倾斜加速度监测")
                        {
                            var count = db1.ZDXQ_3号_倾斜加速度监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_3号_倾斜加速度监测> list = new List<ZDXQ_3号_倾斜加速度监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_3号_倾斜加速度监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_3号_倾斜加速度监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    i = 0;
                                    db.SubmitChanges();
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_3号风机_倾斜加速度监测没有数据需要导入......");
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();
                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 3)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道3峰峰值;
                                        model.YXValue = l.通道3有效值;
                                        model.AverageValue = l.通道3均值;
                                        model.RateValue = l.通道3主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 4)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道4峰峰值;
                                        model.YXValue = l.通道4有效值;
                                        model.AverageValue = l.通道4均值;
                                        model.RateValue = l.通道4主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 5)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道5峰峰值;
                                        model.YXValue = l.通道5有效值;
                                        model.RateValue = l.通道5主频率值;
                                        model.AverageValue = l.通道5均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 6)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道6峰峰值;
                                        model.YXValue = l.通道6有效值;
                                        model.RateValue = l.通道6主频率值;
                                        model.AverageValue = l.通道6均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 7)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道7峰峰值;
                                        model.YXValue = l.通道7有效值;
                                        model.AverageValue = l.通道7均值;
                                        model.RateValue = l.通道7主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 8)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道8峰峰值;
                                        model.YXValue = l.通道8有效值;
                                        model.AverageValue = l.通道8均值;
                                        model.RateValue = l.通道8主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 9)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道9峰峰值;
                                        model.YXValue = l.通道9有效值;
                                        model.AverageValue = l.通道9均值;
                                        model.RateValue = l.通道9主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 10)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道10峰峰值;
                                        model.YXValue = l.通道10有效值;
                                        model.AverageValue = l.通道10均值;
                                        model.RateValue = l.通道10主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 11)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道11峰峰值;
                                        model.YXValue = l.通道11有效值;
                                        model.AverageValue = l.通道11均值;
                                        model.RateValue = l.通道11主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 12)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道12峰峰值;
                                        model.YXValue = l.通道12有效值;
                                        model.AverageValue = l.通道12均值;
                                        model.RateValue = l.通道12主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 13)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道13峰峰值;
                                        model.YXValue = l.通道13有效值;
                                        model.AverageValue = l.通道13均值;
                                        model.RateValue = l.通道13主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 14)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道14峰峰值;
                                        model.YXValue = l.通道14有效值;
                                        model.AverageValue = l.通道14均值;
                                        model.RateValue = l.通道14主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 15)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道15峰峰值;
                                        model.YXValue = l.通道15有效值;
                                        model.AverageValue = l.通道15均值;
                                        model.RateValue = l.通道15主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 16)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道16峰峰值;
                                        model.YXValue = l.通道16有效值;
                                        model.AverageValue = l.通道16均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_3号风机_倾斜加速度监测数据导入完成......");
                            }
                            else
                            {
                                AppContext.Log("ZDXQ_3号风机_倾斜加速度监测没有数据需要导入......");
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                            }
                        }

                        if (d.DeviceIden == "ZDXQ_24号风机_倾斜加速度监测")
                        {
                            var count = db1.ZDXQ_24号_倾斜加速度监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_24号_倾斜加速度监测> list = new List<ZDXQ_24号_倾斜加速度监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_24号_倾斜加速度监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_24号_倾斜加速度监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_24号风机_倾斜加速度监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 3)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道3峰峰值;
                                        model.YXValue = l.通道3有效值;
                                        model.AverageValue = l.通道3均值;
                                        model.RateValue = l.通道3主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 4)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道4峰峰值;
                                        model.YXValue = l.通道4有效值;
                                        model.AverageValue = l.通道4均值;
                                        model.RateValue = l.通道4主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 5)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道5峰峰值;
                                        model.YXValue = l.通道5有效值;
                                        model.RateValue = l.通道5主频率值;
                                        model.AverageValue = l.通道5均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 6)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道6峰峰值;
                                        model.YXValue = l.通道6有效值;
                                        model.RateValue = l.通道6主频率值;
                                        model.AverageValue = l.通道6均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 7)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道7峰峰值;
                                        model.YXValue = l.通道7有效值;
                                        model.AverageValue = l.通道7均值;
                                        model.RateValue = l.通道7主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 8)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道8峰峰值;
                                        model.YXValue = l.通道8有效值;
                                        model.AverageValue = l.通道8均值;
                                        model.RateValue = l.通道8主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 9)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道9峰峰值;
                                        model.YXValue = l.通道9有效值;
                                        model.AverageValue = l.通道9均值;
                                        model.RateValue = l.通道9主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 10)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道10峰峰值;
                                        model.YXValue = l.通道10有效值;
                                        model.AverageValue = l.通道10均值;
                                        model.RateValue = l.通道10主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 11)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道11峰峰值;
                                        model.YXValue = l.通道11有效值;
                                        model.AverageValue = l.通道11均值;
                                        model.RateValue = l.通道11主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 12)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道12峰峰值;
                                        model.YXValue = l.通道12有效值;
                                        model.AverageValue = l.通道12均值;
                                        model.RateValue = l.通道12主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 13)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道13峰峰值;
                                        model.YXValue = l.通道13有效值;
                                        model.AverageValue = l.通道13均值;
                                        model.RateValue = l.通道13主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    
                                    if (p.MapDataName.AsInt(0) == 14)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道14峰峰值;
                                        model.YXValue = l.通道14有效值;
                                        model.AverageValue = l.通道14均值;
                                        model.RateValue = l.通道14主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 15)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道15峰峰值;
                                        model.YXValue = l.通道15有效值;
                                        model.AverageValue = l.通道15均值;
                                        model.RateValue = l.通道15主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                    if (p.MapDataName.AsInt(0) == 16)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道16峰峰值;
                                        model.YXValue = l.通道16有效值;
                                        model.AverageValue = l.通道16均值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }

                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_24号风机_倾斜加速度监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_24号风机_倾斜加速度监测没有数据需要导入......");
                            }

                        }


                        if (d.DeviceIden == "ZDXQ_1号_倾斜监测")
                        {
                            var count = db1.ZDXQ_1号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_1号_倾斜监测> list = new List<ZDXQ_1号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_1号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_1号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_1号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                   

                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_1号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_1号_倾斜监测没有数据需要导入......");
                            }

                        }


                        if (d.DeviceIden == "ZDXQ_2号_倾斜监测")
                        {
                            var count = db1.ZDXQ_2号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_2号_倾斜监测> list = new List<ZDXQ_2号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_2号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_2号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_2号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_2号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_1号_倾斜监测没有数据需要导入......");
                            }

                        }


                        if (d.DeviceIden == "ZDXQ_4号_倾斜监测")
                        {
                            var count = db1.ZDXQ_4号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_4号_倾斜监测> list = new List<ZDXQ_4号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_4号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_4号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_4号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_4号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_4号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_5号_倾斜监测")
                        {
                            var count = db1.ZDXQ_5号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_5号_倾斜监测> list = new List<ZDXQ_5号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_5号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_5号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_5号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_5号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_5号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_6号_倾斜监测")
                        {
                            var count = db1.ZDXQ_6号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_6号_倾斜监测> list = new List<ZDXQ_6号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_6号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_6号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_6号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_6号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_6号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_7号_倾斜监测")
                        {
                            var count = db1.ZDXQ_7号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_7号_倾斜监测> list = new List<ZDXQ_7号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_7号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_7号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_7号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_7号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_7号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_8号_倾斜监测")
                        {
                            var count = db1.ZDXQ_8号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_8号_倾斜监测> list = new List<ZDXQ_8号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_8号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_8号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_8号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_8号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_8号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_9号_倾斜监测")
                        {
                            var count = db1.ZDXQ_9号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_9号_倾斜监测> list = new List<ZDXQ_9号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_9号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_9号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_9号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_9号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_9号_倾斜监测没有数据需要导入......");
                            }

                        }


                        if (d.DeviceIden == "ZDXQ_10号_倾斜监测")
                        {
                            var count = db1.ZDXQ_10号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_10号_倾斜监测> list = new List<ZDXQ_10号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_10号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_10号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_10号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_10号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_10号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_11号_倾斜监测")
                        {
                            var count = db1.ZDXQ_11号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_11号_倾斜监测> list = new List<ZDXQ_11号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_11号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_11号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_11号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_11号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_11号_倾斜监测没有数据需要导入......");
                            }

                        }



                        if (d.DeviceIden == "ZDXQ_12号_倾斜监测")
                        {
                            var count = db1.ZDXQ_12号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_12号_倾斜监测> list = new List<ZDXQ_12号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_12号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_12号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_12号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_12号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_12号_倾斜监测没有数据需要导入......");
                            }

                        }


                        if (d.DeviceIden == "ZDXQ_14号_倾斜监测")
                        {
                            var count = db1.ZDXQ_14号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_14号_倾斜监测> list = new List<ZDXQ_14号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_14号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_14号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_14号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_14号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_14号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_16号_倾斜监测")
                        {
                            var count = db1.ZDXQ_16号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_16号_倾斜监测> list = new List<ZDXQ_16号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_16号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_16号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_16号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_16号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_16号_倾斜监测没有数据需要导入......");
                            }

                        }


                        if (d.DeviceIden == "ZDXQ_18号_倾斜监测")
                        {
                            var count = db1.ZDXQ_18号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_18号_倾斜监测> list = new List<ZDXQ_18号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_18号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_18号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_18号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }

                                isupdate = 1;
                                AppContext.Log("ZDXQ_18号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_18号_倾斜监测没有数据需要导入......");
                            }

                        }


                        if (d.DeviceIden == "ZDXQ_19号_倾斜监测")
                        {
                            var count = db1.ZDXQ_19号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_19号_倾斜监测> list = new List<ZDXQ_19号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_19号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_19号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_19号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_19号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_19号_倾斜监测没有数据需要导入......");
                            }

                        }


                        if (d.DeviceIden == "ZDXQ_20号_倾斜监测")
                        {
                            var count = db1.ZDXQ_20号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_20号_倾斜监测> list = new List<ZDXQ_20号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_20号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_20号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_20号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_20号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_20号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_21号_倾斜监测")
                        {
                            var count = db1.ZDXQ_21号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_21号_倾斜监测> list = new List<ZDXQ_21号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_21号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_21号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_21号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_21号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_21号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_22号_倾斜监测")
                        {
                            var count = db1.ZDXQ_22号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_22号_倾斜监测> list = new List<ZDXQ_22号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_22号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_22号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_22号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_22号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_22号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_25号_倾斜监测")
                        {
                            var count = db1.ZDXQ_25号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_25号_倾斜监测> list = new List<ZDXQ_25号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_25号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_25号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_25号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_25号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_25号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_26号_倾斜监测")
                        {
                            var count = db1.ZDXQ_26号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_26号_倾斜监测> list = new List<ZDXQ_26号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_26号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_26号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_26号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_26号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_26号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_28号_倾斜监测")
                        {
                            var count = db1.ZDXQ_28号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_28号_倾斜监测> list = new List<ZDXQ_28号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_28号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_28号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_28号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_28号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_28号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_29号_倾斜监测")
                        {
                            var count = db1.ZDXQ_29号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_29号_倾斜监测> list = new List<ZDXQ_29号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_29号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_29号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_29号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_29号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_29号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_30号_倾斜监测")
                        {
                            var count = db1.ZDXQ_30号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_30号_倾斜监测> list = new List<ZDXQ_30号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_30号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_30号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_30号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_30号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_30号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_31号_倾斜监测")
                        {
                            var count = db1.ZDXQ_31号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_31号_倾斜监测> list = new List<ZDXQ_31号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_31号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_31号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_31号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_31号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_31号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_32号_倾斜监测")
                        {
                            var count = db1.ZDXQ_32号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_32号_倾斜监测> list = new List<ZDXQ_32号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_32号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_32号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_32号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_32号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_32号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_33号_倾斜监测")
                        {
                            var count = db1.ZDXQ_33号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_33号_倾斜监测> list = new List<ZDXQ_33号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_33号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_33号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_33号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_33号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_33号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_34号_倾斜监测")
                        {
                            var count = db1.ZDXQ_34号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_34号_倾斜监测> list = new List<ZDXQ_34号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_34号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_34号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_34号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_34号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_34号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_35号_倾斜监测")
                        {
                            var count = db1.ZDXQ_35号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_35号_倾斜监测> list = new List<ZDXQ_35号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_35号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_35号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_35号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }


                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_35号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_35号_倾斜监测没有数据需要导入......");
                            }

                        }


                        if (d.DeviceIden == "ZDXQ_36号_倾斜监测")
                        {
                            var count = db1.ZDXQ_36号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_36号_倾斜监测> list = new List<ZDXQ_36号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_36号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_36号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_36号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }




                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_36号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_36号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_37号_倾斜监测")
                        {
                            var count = db1.ZDXQ_37号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_37号_倾斜监测> list = new List<ZDXQ_37号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_37号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_37号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_37号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }




                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_37号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_37号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_38号_倾斜监测")
                        {
                            var count = db1.ZDXQ_38号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_38号_倾斜监测> list = new List<ZDXQ_38号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_38号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_38号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_38号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }




                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_38号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_38号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_39号_倾斜监测")
                        {
                            var count = db1.ZDXQ_39号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_39号_倾斜监测> list = new List<ZDXQ_39号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_39号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_39号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_39号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }




                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_39号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_39号_倾斜监测没有数据需要导入......");
                            }

                        }

                        if (d.DeviceIden == "ZDXQ_40号_倾斜监测")
                        {
                            var count = db1.ZDXQ_40号_倾斜监测.Count();
                            if (count > 0)
                            {
                                //string sql = string.Format("select Max(CreateDate) from {0}", DBName);
                                //var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                                List<ZDXQ_40号_倾斜监测> list = new List<ZDXQ_40号_倾斜监测>();
                                if (fdvalue == null)
                                {
                                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                                    list = db1.ZDXQ_40号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                                }
                                else
                                {
                                    list = db1.ZDXQ_40号_倾斜监测.Where(u => Convert.ToDateTime(u.时间) > fdvalue).ToList();
                                }

                                if (list.Count == 0)
                                {
                                    d.Status = 0;
                                    db.SubmitChanges();
                                    i = 0;
                                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                    //if (view != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                    //    if (layer != null && layer.LayerType != 1)
                                    //    {
                                    //        layer.LayerType = 2;
                                    //        db.SubmitChanges();
                                    //    }
                                    //}
                                    AppContext.Log("ZDXQ_40号_倾斜监测没有数据需要导入......");
                                    continue;
                                }
                                else
                                {
                                    d.Status = 1;
                                    db.SubmitChanges();
                                }
                                //  db.SubmitChanges();

                                foreach (var l in list)
                                {

                                    if (p.MapDataName.AsInt(0) == 1)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道1峰峰值;
                                        model.YXValue = l.通道1有效值;
                                        model.AverageValue = l.通道1均值;
                                        model.RateValue = l.通道1主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }
                                    if (p.MapDataName.AsInt(0) == 2)
                                    {
                                        FD_DynamicValue2 model = new FD_DynamicValue2();
                                        model.PointId = p.ID;
                                        model.MAXValue = l.通道2峰峰值;
                                        model.YXValue = l.通道2有效值;
                                        model.AverageValue = l.通道2均值;
                                        model.RateValue = l.通道2主频率值;
                                        model.DeviceId = d.ID;
                                        model.DynamicId = 0;
                                        model.CreateDate = l.时间.AsDatetime(DateTime.Now);
                                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId, 0));
                                    }




                                }
                                isupdate = 1;
                                AppContext.Log("ZDXQ_40号_倾斜监测数据导入完成......");
                            }
                            else
                            {
                                d.Status = 0;
                                db.SubmitChanges();
                                i = 0;
                                //var view = db.MK_Position.FirstOrDefault(u => u.ID == p.PositionId);
                                //if (view != null)
                                //{
                                //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                                //    if (layer != null && layer.LayerType != 1)
                                //    {
                                //        layer.LayerType = 2;
                                //        db.SubmitChanges();
                                //    }
                                //}
                                AppContext.Log("ZDXQ_40号_倾斜监测没有数据需要导入......");
                            }

                        }

                    }
                    if (i == 1)
                    {
                        //var view = db.MK_Position.FirstOrDefault(u => u.ID == PositionId);
                        //if (view != null)
                        //{
                        //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                        //    if (layer != null && layer.LayerType != 1)
                        //    {
                        //        layer.LayerType = 0;
                        //        db.SubmitChanges();
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                AppContext.Error(ex.Message);
            }

            if(isupdate==1)
            {
                MK_DeviceTime devicedate = db.MK_DeviceTime.FirstOrDefault();
                if (devicedate != null)
                {
                    devicedate.UpdateDate = DateTime.Now;
                    db.SubmitChanges();
                }
            }
            AppContext.Log("读取数据完成......");

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
                        _myInstance = new DataReadMoniter();
                    }
                }
            }
            return _myInstance;
        }
    }

    public class DataExport
    {
        public double? MaxValue { set; get; }
        public double? effectValue { set; get; }
        public DateTime CreateDate { set; get; }
        public int ID { set; get; }
    }
}
