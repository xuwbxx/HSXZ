using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.DAO;
using FDScanSystem.Helper;
using Interactive.Utility;

namespace FDScanSystem.Server.Job
{
    public class DataAddJob : MoniterBase
    {
        protected DataAddJob() : base(300000) { }
        static MoniterBase _myInstance;
        static object _lock = new object();
        //private string now = "2013-10-8 00:00:00";
        protected override void PostData()
        {
            AppContext.Log("执行自动定时生成模块数据....");
            // LogWriter.Info("执行自动定时读取模块数据....");

            var db = DataContextFactory.Create();
            Random ran = new Random();
            var pointlist = db.MK_Point.Where(u => u.NewID == -1).ToList();
            foreach(var point in pointlist)
            {
                var dataValue = new MK_DataValue();
                if (point.PointName.Equals("3号钢板计1") || point.PointName.Equals("3号钢板计5") || point.PointName.Equals("3号钢板计6") || point.PointName.Equals("3号钢板计7") || point.PointName.Equals("3号钢板计8") || point.PointName.Equals("3号钢板计9") || point.PointName.Equals("3号钢板计10") || point.PointName.Equals("3号钢板计12") || point.PointName.Equals("3号钢板计14") || point.PointName.Equals("3号钢板计18"))
                {
                    double tempvalue = ran.Next(0, 71);
                    dataValue.ScanValue = point.Para1 + tempvalue;
                    dataValue.ScanValue2 = 0;
                    dataValue.ScanValue3 = point.Para1 + tempvalue;
                    dataValue.ScanValue4 = 0;
                    dataValue.ScanValue5 = point.Para1 + tempvalue;
                    dataValue.ScanValue6 = 0;
                    dataValue.CreateDate = DateTime.Now;
                    dataValue.PointId = point.ID;
                    dataValue.Status = 0;
                    db.MK_DataValue.InsertOnSubmit(dataValue);
                    db.SubmitChanges();
                }
            }


            Singleton<ServicesStatuesMachine>.Instance.Increment();
        }

        //private bool IsPass(MK_AutoReadConfig config)
        //{
        //    TimeSpan subTime = DateTime.Now.Subtract(config.BeginDate.Value);
        //    return (((long)subTime.TotalMinutes) % 5) == 1;
        //}

        /// <summary>
        /// 获取单例实例
        /// </summary>
        /// <returns></returns>
        public static MoniterBase GetInstance()
        {
            if (_myInstance == null)
            {
                lock (_lock)
                {
                    if (_myInstance == null)
                    {
                        _myInstance = new DataAddJob();
                    }
                }
            }
            return _myInstance;
        }
    }
}
