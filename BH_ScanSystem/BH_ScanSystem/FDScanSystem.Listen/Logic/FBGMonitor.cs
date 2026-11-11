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
    public class FBGMonitor : WindMoniterBase
    {
        protected FBGMonitor() : base() { }
        static WindMoniterBase _myInstance;
        static object _lock = new object();
        protected override void PostData()
        {
            var db = DataContextFactory.Create();

            FD_DynamicValue2 dyna = new FD_DynamicValue2();
            dyna.CreateDate = DateTime.Now;
            string name = dyna.CreateDate.Value.ToString("yyyyMM");
            string DBName = "FD_DynamicValue2" + name;
            var Exist = db.ExecuteQuery<FD_DynamicValue2>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
            if (Exist.ID == 0)
            {
                string sql1 = string.Format(@"CREATE TABLE [dbo].[{0}](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [int] NULL,
	[PointId] [int] NULL,
	[MAXValue] [float] NULL,
	[YXValue] [float] NULL,
	[AverageValue] [float] NULL,
	[CreateDate] [datetime] NULL,
	[DynamicId] [int] NULL,
 CONSTRAINT [PK_{1}] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]", DBName, DBName);
                db.ExecuteCommand(sql1);
            }

            var device = db.MK_Device.Where(u => u.DeviceProtocol == 2).ToList();
            var db2 = DataContextFactory2.Create();
            foreach (var dev in device)
            {
                int i = 1;
                var pointlist = db.MK_Point.Where(u => u.DeviceId == dev.ID).ToList();
                int PositionId = 0;
                string str1 = "";
                for (int j = 0; j < pointlist.Count; j++)
                {
                    if (string.IsNullOrEmpty(pointlist[j].MapDataName))
                        continue;
                    if (j < pointlist.Count - 1)
                    {
                       
                        str1 += pointlist[j].ID + ",";
                    }
                    else
                    {
                        str1 += pointlist[j].ID;
                        PositionId = pointlist[i].PositionId ?? 0;
                    }
                }

                string sql = string.Format("select Max(CreateDate) as CreateDate from {0} where PointId in ({1})", DBName, str1);
                var fdvalue = db.ExecuteQuery<DateTime?>(sql).ToList().FirstOrDefault();
                if (fdvalue == null)
                {
                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                }

                var fbg_list = db2.FBG_Data.Where(u => u.MyDateTime > fdvalue).ToList();
                if (fbg_list.Count > 0)
                {
                    dev.Status = 1;
                    db.SubmitChanges();
                }
                else
                {
                    if (dev.Status == 1)
                    {
                        var projectName = db.Base_Project.FirstOrDefault().ProjectName;
                        string str = string.Format("{0}项目的设备编号{1}已经断线", projectName, dev.DeviceName);
                        var SMSlist = db.SMS_User.ToList();
                        foreach (var sms in SMSlist)
                        {
                            Send.SendSMS(sms.Mobile, str);
                        }
                    }
                    dev.Status = 0;
                    db.SubmitChanges();

                    //var view = db.MK_Position.FirstOrDefault(u => u.ID == PositionId);
                    //if (view != null)
                    //{
                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == view.LayerId);
                    //    if (layer != null && layer.LayerType != 1)
                    //    {
                    //        layer.LayerType = 2;
                    //        i = 0;
                    //        db.SubmitChanges();
                    //    }
                    //}
                }
                foreach (var fbg in fbg_list)
                {
                    foreach (var point in pointlist)
                    {
                        FD_DynamicValue2 model = new FD_DynamicValue2();
                        if (point.MapDataName.ToLowerInvariant().Equals("v001"))
                        {
                            model.MAXValue = fbg.V001.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v002"))
                        {
                            model.MAXValue = fbg.V002.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v003"))
                        {
                            model.MAXValue = fbg.V003.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v004"))
                        {
                            model.MAXValue = fbg.V004.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v005"))
                        {
                            model.MAXValue = fbg.V005.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v006"))
                        {
                            model.MAXValue = fbg.V006.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v007"))
                        {
                            model.MAXValue = fbg.V007.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v008"))
                        {
                            model.MAXValue = fbg.V008.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v009"))
                        {
                            model.MAXValue = fbg.V009.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v010"))
                        {
                            model.MAXValue = fbg.V010.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v011"))
                        {
                            model.MAXValue = fbg.V011.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v012"))
                        {
                            model.MAXValue = fbg.V012.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v013"))
                        {
                            model.MAXValue = fbg.V013.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v014"))
                        {
                            model.MAXValue = fbg.V014.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v015"))
                        {
                            model.MAXValue = fbg.V015.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v016"))
                        {
                            model.MAXValue = fbg.V016.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v017"))
                        {
                            model.MAXValue = fbg.V017.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v018"))
                        {
                            model.MAXValue = fbg.V018.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v019"))
                        {
                            model.MAXValue = fbg.V019.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v020"))
                        {
                            model.MAXValue = fbg.V020.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v021"))
                        {
                            model.MAXValue = fbg.V021.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v022"))
                        {
                            model.MAXValue = fbg.V022.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v023"))
                        {
                            model.MAXValue = fbg.V023.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v024"))
                        {
                            model.MAXValue = fbg.V024.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v025"))
                        {
                            model.MAXValue = fbg.V025.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v026"))
                        {
                            model.MAXValue = fbg.V026.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v027"))
                        {
                            model.MAXValue = fbg.V027.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v028"))
                        {
                            model.MAXValue = fbg.V028.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v029"))
                        {
                            model.MAXValue = fbg.V029.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v030"))
                        {
                            model.MAXValue = fbg.V030.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v031"))
                        {
                            model.MAXValue = fbg.V031.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v032"))
                        {
                            model.MAXValue = fbg.V032.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v033"))
                        {
                            model.MAXValue = fbg.V033.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v034"))
                        {
                            model.MAXValue = fbg.V034.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v035"))
                        {
                            model.MAXValue = fbg.V035.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v036"))
                        {
                            model.MAXValue = fbg.V036.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v037"))
                        {
                            model.MAXValue = fbg.V037.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v038"))
                        {
                            model.MAXValue = fbg.V038.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v039"))
                        {
                            model.MAXValue = fbg.V039.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v040"))
                        {
                            model.MAXValue = fbg.V040.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v041"))
                        {
                            model.MAXValue = fbg.V041.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v042"))
                        {
                            model.MAXValue = fbg.V042.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v043"))
                        {
                            model.MAXValue = fbg.V043.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v044"))
                        {
                            model.MAXValue = fbg.V044.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v045"))
                        {
                            model.MAXValue = fbg.V045.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v046"))
                        {
                            model.MAXValue = fbg.V046.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v047"))
                        {
                            model.MAXValue = fbg.V047.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v048"))
                        {
                            model.MAXValue = fbg.V048.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v049"))
                        {
                            model.MAXValue = fbg.V049.AsDouble(0.0);
                        }
                        else if (point.MapDataName.ToLowerInvariant().Equals("v050"))
                        {
                            model.MAXValue = fbg.V050.AsDouble(0.0);
                        }
                        model.PointId = point.ID;

                        model.YXValue = 0;
                        model.AverageValue = 0;
                        model.RateValue = 0;
                        model.DeviceId = dev.ID;
                        model.DynamicId = 0;
                        model.CreateDate = fbg.MyDateTime.AsDatetime(DateTime.Now);
                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.RateValue, model.CreateDate, model.DynamicId));
                        AppContext.Log("传感器：" + point.MapDataName + "数据导入完成......");
                    }
                }
                AppContext.Log("设备号：" + dev.DeviceName + "数据导入完成......");
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
                        _myInstance = new FBGMonitor();
                    }
                }
            }
            return _myInstance;
        }
    }
    
    

}
