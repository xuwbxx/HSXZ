using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Interactive.Utility;
using System.Threading;
using FDScanSystem.Listen.Device;
using FDScanSystem.DAO;
using System.Configuration;
using System.Net.Sockets;
using System.Net;
using ZJFramework10.Extension;

namespace FDScanSystem.Listen.Logic
{
    public class OtherReadMonitor : WindMoniterBase
    {
        protected OtherReadMonitor() : base(90000) { }
        static WindMoniterBase _myInstance;
        static object _lock = new object();
        protected override  void PostData()
        {
            AppContext.Log("执行自动定时读取数据....");
            var db = DataContextFactory.Create();
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

            try
            {
                var pointslist = db.MK_Point.Where(u => u.NewID == 0 && u.MapDataName.Equals("0")).OrderBy(u => u.PointName).ToList();


                foreach (var p in pointslist)
                {


                    //if (!p.PointName.Equals("47PHC-2-6-B"))
                    //{
                    //    continue;
                    //}
                    string sql = string.Format("select Max(CreateDate) from {0} where PointId={1}", DBName, p.ID);
                    var fdvalue = db.ExecuteQuery<DateTime?>(sql).FirstOrDefault();
                    int PositionId = p.PositionId ?? 0;
                    DateTime mindate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                    DateTime maxdate = DateTime.Now;
                    if (fdvalue == null)
                    {
                        fdvalue = mindate;
                    }

                    if (p.PointName.Contains("7PHC") && !p.PointName.Contains("47PHC"))
                    {
                        var strlist = p.PointName.Split('-');
                        if (strlist[3].Equals("A"))
                        {
                            int tempPara = strlist[2].AsInt();
                            string para1_Name = string.Format("{0}-{1}-{2}-A", strlist[0], strlist[1], (tempPara - 1).AsStr());
                            string para2_Name = string.Format("{0}-{1}-{2}-A", strlist[0], strlist[1], (tempPara + 1).AsStr());
                            sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='{1}') and CreateDate<'{2}' and CreateDate>'{3}'", DBName, para1_Name, maxdate, fdvalue);
                            var PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                            foreach (var l in PHC_List)
                            {
                                double para2 = getPara(para2_Name, l.CreateDate, DBName);
                                if (para2 == -1)
                                {
                                    continue;
                                }
                                FD_DynamicValue3 model = new FD_DynamicValue3();
                                model.PointId = p.ID;
                                model.WaveLength = l.WaveLength;
                                model.Result = l.Result.AsDouble();
                                model.DeviceId = p.DeviceId;
                                model.CreateDate = l.CreateDate;
                                model.ScanValue = (l.ScanValue + para2) / 2;
                                db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                            }
                        }
                        else
                        {
                            sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='7PHC-{1}-2-B') and CreateDate<'{2}' and CreateDate>'{3}'", DBName, strlist[1], maxdate, fdvalue);
                            var PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                            foreach (var l in PHC_List)
                            {
                                double para1 = l.ScanValue.AsDouble();
                                // double para1 = getPara("PHC7_2_2_B", l.CreateDate, DBName);
                                double para2 = getPara(string.Format("7PHC-{0}-2-A", strlist[1]), l.CreateDate, DBName);

                                string para3_Name = string.Format("7PHC-{0}-{1}-A", strlist[1], strlist[2]);
                                double para3 = getPara(para3_Name, l.CreateDate, DBName);
                                if (para2 == -1 || para3 == -1)
                                {
                                    continue;
                                }
                                FD_DynamicValue3 model = new FD_DynamicValue3();
                                model.PointId = p.ID;
                                model.WaveLength = l.WaveLength;
                                model.Result = l.Result.AsDouble();
                                model.DeviceId = p.DeviceId;
                                model.CreateDate = l.CreateDate;

                                if (strlist[3].Equals("C"))
                                {
                                    double para4 = getPara(string.Format("7PHC-{0}-2-C", strlist[1]), l.CreateDate, DBName);
                                    if (para4 == -1)
                                    {
                                        continue;
                                    }
                                    model.ScanValue = get7PHC_C(para1, para2, para3, para4, p.Para3.AsDouble(), strlist[1].AsInt(0));
                                    //scanvalue = get7PHC_C(para1,para2,para3,para4,);
                                }
                                else
                                {
                                    model.ScanValue = get7PHC_B(para1, para2, para3, p.Para3);
                                }


                                db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                            }


                        }

                    }
                    else if (p.PointName.Contains("47PHC"))
                    {
                        var strlist = p.PointName.Split('-');
                        sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='47PHC-1-1-A') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                        var PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                        foreach (var l in PHC_List)
                        {
                            double para1 = l.ScanValue.AsDouble();
                            // double para1 = getPara("PHC7_2_2_B", l.CreateDate, DBName);
                            double para2 = getPara("47PHC-1-1-B", l.CreateDate, DBName);
                            double para3 = getPara("47PHC-1-2-A", l.CreateDate, DBName);
                            double para4 = getPara("47PHC-1-2-B", l.CreateDate, DBName);
                            if (para2 == -1 || para3 == -1 || para4 == -1)
                            {
                                continue;
                            }
                            FD_DynamicValue3 model = new FD_DynamicValue3();
                            model.PointId = p.ID;
                            model.WaveLength = l.WaveLength;
                            model.Result = l.Result.AsDouble();
                            model.DeviceId = p.DeviceId;
                            model.CreateDate = l.CreateDate;

                            model.ScanValue = get47PHC(para1, para2, para3, para4, p.Para3, p.Para4, p.Para5, p.Para6, strlist[3]);
                            if (p.PointName.Equals("47PHC-2-7-A")|| p.PointName.Equals("47PHC-2-8-A"))
                            {
                                model.ScanValue = model.ScanValue + 1;
                            }
                            else if(p.PointName.Equals("47PHC-2-8-B"))
                            {
                                model.ScanValue = model.ScanValue - 2;
                            }
                            else if (p.PointName.Equals("47PHC-2-7-B"))
                            {
                                model.ScanValue = model.ScanValue / 2 - 5;
                            }
                            else if (p.PointName.Equals("47PHC-2-6-B"))
                            {
                                model.ScanValue = model.ScanValue / 3 - 6;
                            }
                            else if (p.PointName.Equals("47PHC-2-6-A"))
                            {
                                model.ScanValue = model.ScanValue / 4 - 3.5;
                            }
                            else if (p.PointName.Equals("47PHC-2-5-B"))
                            {
                                model.ScanValue = model.ScanValue / 3 - 3.4;
                            }
                            else if (p.PointName.Equals("47PHC-2-5-A"))
                            {
                                model.ScanValue = model.ScanValue / 3 - 4;
                            }
                            else if (p.PointName.Equals("47PHC-2-4-B"))
                            {
                                model.ScanValue = model.ScanValue / 3 - 3.1;
                            }
                            else if (p.PointName.Equals("47PHC-2-4-A"))
                            {
                                model.ScanValue = model.ScanValue / 3 - 4;
                            }


                            db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                            Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                        }
                    }
                    else
                    {
                        switch (p.PointName)
                        {
                            case "7HL-X-13b":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='7HL-X-13') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                var PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = 0.863 * l.ScanValue;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            case "7HL-X-15b":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='7HL-X-15') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    //double para2 = getPara("7HL-X-10", l.CreateDate, DBName);
                                    //double para3 = getPara("7HL-X-15", l.CreateDate, DBName);
                                    //if (para2 == -1 || para3 == -1)
                                    //{
                                    //    continue;
                                    //}
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    // model.ScanValue = (para1 / para2) * para3;
                                    model.ScanValue = 0.955 * para1;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            case "7HL-X-16b":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='7HL-X-16') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    //double para2 = getPara("7HL-X-9", l.CreateDate, DBName);
                                    //double para3 = getPara("7HL-X-16", l.CreateDate, DBName);
                                    //if (para2 == -1 || para3 == -1)
                                    //{
                                    //    continue;
                                    //}
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = para1 * 1.58;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            case "7HL-S-3":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='7HL-X-6') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    //double para2 = getPara("7HL-X-4", l.CreateDate, DBName);
                                    //double para3 = getPara("7HL-X-6", l.CreateDate, DBName);
                                    //if (para2 == -1 || para3 == -1)
                                    //{
                                    //    continue;
                                    //}
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = 0.14 * para1;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            case "7HL-S-4":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='7HL-X-7') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    //double para2 = getPara("7HL-X-2", l.CreateDate, DBName);
                                    //double para3 = getPara("7HL-X-7", l.CreateDate, DBName);
                                    //if (para2 == -1 || para3 == -1)
                                    //{
                                    //    continue;
                                    //}
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = -0.177 * para1;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            case "7HL-S-8":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='7HL-X-15') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    //double para2 = getPara("7HL-X-10", l.CreateDate, DBName);
                                    //double para3 = getPara("7HL-X-15", l.CreateDate, DBName);
                                    //if (para2 == -1 || para3 == -1)
                                    //{
                                    //    continue;
                                    //}
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = -0.177 * para1;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            case "7HL-S-7":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='7HL-X-14') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    //double para2 = getPara("7HL-X-12", l.CreateDate, DBName);
                                    //double para3 = getPara("7HL-X-14", l.CreateDate, DBName);
                                    //if (para2 == -1 || para3 == -1)
                                    //{
                                    //    continue;
                                    //}
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = 0.14 * para1;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;
                            case "47HL-S-2":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='47HL-S-1') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    double para2 = getPara("47HL-S-5", l.CreateDate, DBName);
                                   // double para3 = getPara("47HL-X-4", l.CreateDate, DBName);
                                    if (para2 == -1)
                                    {
                                        continue;
                                    }
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = (para1 + para2) / 2;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            case "47HL-X-9b":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='47HL-X-9') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    double para2 = getPara("47HL-X-1", l.CreateDate, DBName);

                                    if (para2 == -1)
                                    {
                                        continue;
                                    }
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = (para1 + para2) / 2;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            case "47HL-X-10b":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='47HL-X-10') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    double para2 = getPara("47HL-X-2", l.CreateDate, DBName);

                                    if (para2 == -1)
                                    {
                                        continue;
                                    }
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = (para1 + para2) / 2;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            case "47HL-X-11b":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='47HL-X-11') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    double para2 = getPara("47HL-X-3", l.CreateDate, DBName);

                                    if (para2 == -1)
                                    {
                                        continue;
                                    }
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = (para1 + para2) / 2;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            case "47HL-X-15b":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='47HL-X-7') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    double para2 = getPara("47HL-X-15", l.CreateDate, DBName);

                                    if (para2 == -1)
                                    {
                                        continue;
                                    }
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = (para1 + para2) / 2;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            case "47HL-X-16b":
                                sql = string.Format("select * from {0} where PointId in (select ID from MK_Point where PointName='47HL-X-8') and CreateDate<'{1}' and CreateDate>'{2}'", DBName, maxdate, fdvalue);
                                PHC_List = db.ExecuteQuery<FD_DynamicValue3>(sql).ToList();
                                foreach (var l in PHC_List)
                                {
                                    double para1 = l.ScanValue.AsDouble();
                                    double para2 = getPara("47HL-X-16", l.CreateDate, DBName);

                                    if (para2 == -1)
                                    {
                                        continue;
                                    }
                                    FD_DynamicValue3 model = new FD_DynamicValue3();
                                    model.PointId = p.ID;
                                    model.WaveLength = l.WaveLength;
                                    model.Result = l.Result.AsDouble();
                                    model.DeviceId = p.DeviceId;
                                    model.CreateDate = l.CreateDate;
                                    model.ScanValue = (para1 + para2) / 2;
                                    db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.WaveLength, model.Result, model.CreateDate, model.ScanValue, 0));
                                    Alarm(model.PointId ?? 0, model.CreateDate.Value, p.MaxNum ?? 0, p.MaxNum2 ?? 0, p.MaxNum3 ?? 0, p.MinNum ?? 0, p.MinNum2 ?? 0, p.MinNum3 ?? 0, model.ScanValue ?? 0);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    AppContext.Log(p.PointName + "数据导入完成......");


                }
            }
            catch(Exception ex)
            { AppContext.Log(ex.Message); }


        }

        public double? get47PHC(double? para1, double? para2, double? para3, double? para4, double? para5, double? para6,double? para7,double? para8, string s)
        {
            double? step1 = ((para1 + para2) * 190.788 - 66.683) * 0.00262;
            double? step2 = (para1 - para2 + para3 + para4) * 52.198 - 28.305;
            double? step3 = (para1 - para2) * 52.198;
            double? step4 = (para1 - para2 - para3 + para4) * 52.198;
            if (s.Contains("A"))
            {
                return step1 + (step2 * para5) + (step3 * para6) + (step4 * para7) - para8;
            }
            return step1 - (step2 * para5) - (step3 * para6) - (step4 * para7) + para8;

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
            else if (MinNum.AsDouble(0) != 0 && ScanValue.AsDouble(0) <MinNum)
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



        public double? get7PHC_B(double? para1, double? para2, double? para3, double? para4)
        {
            return para1 - para3 + ((para1 + para2) / 2) - para4;
        }

        public double? get7PHC_C(double? para1, double? para2, double? para3, double? para4, double para5, int number)
        {
            if (number == 2)
            {
                return para4 - (para3 - ((para1 + para2) / 2) + para5) * 2.203;
            }
            return para4 - (para3 - ((para1 + para2) / 2) + para5) / 0.468;
        }

        public double getPara(string PointName, DateTime? CreateDate, string DBName)
        {
            var db = DataContextFactory.Create();
            string sql = string.Format("select ScanValue from {0} where PointId in (select ID from MK_Point where PointName='{1}') and CreateDate='{2}'", DBName, PointName, CreateDate);
            double? value = db.ExecuteQuery<double?>(sql).FirstOrDefault();
            if (value == null)
            {
                return -1;
            }
            return value.AsDouble(0);

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
                        _myInstance = new OtherReadMonitor();
                    }
                }
            }
            return _myInstance;
        }

    }
}
