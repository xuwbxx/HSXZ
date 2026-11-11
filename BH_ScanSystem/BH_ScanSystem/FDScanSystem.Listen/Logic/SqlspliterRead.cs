using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Data;
using System.Threading;
using FDScanSystem.DAO;
using FDScanSystem.Listen.Device;
using System.Data.SQLite;
using Interactive.Utility;
using ZJFramework10.Extension;


namespace FDScanSystem.Listen.Logic
{
    public class SqlspliterRead : WindMoniterBase
    {
        protected SqlspliterRead() : base() { }
        static WindMoniterBase _myInstance;
        static object _lock = new object();
        protected override void PostData()
        {
            var db = DataContextFactory.Create();
           // var device = db.MK_Device.Where(u => u.DeviceProtocol == 1).ToList();
           // List<model> list = new List<model>();

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
            foreach (var dev in device)
            {
                var pointlist = db.MK_Point.Where(u => u.DeviceId == dev.ID).ToList();
                foreach (var point in pointlist)
                {


                    if (string.IsNullOrEmpty(point.MapDataName))
                        continue;
                    string sql = string.Format("select Max(CreateDate) as CreateDate from {0} where PointId={1}", DBName, point.ID);
                    var fdvalue = db.ExecuteQuery<DateTime?>(sql).ToList().FirstOrDefault();
                    if (fdvalue == null)
                    {
                        DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                    }
                    string sqlstr = string.Format("select * from T_SensorVal where DevName='{0}' and SensorID='{1}' ", dev.DeviceIden, point.MapDataName);
                    List<model> list = new List<model>();
                    if (fdvalue == null)
                    {
                        fdvalue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

                        ///   list = db1.ZDXQ_1.Where(u => Convert.ToDateTime(u.时间) > date).ToList();
                        ///   
                    }
                    sqlstr = sqlstr + string.Format(" and SensorDate> '{0}' order by SensorDate", fdvalue.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    list = getTablie(sqlstr);

                    foreach (var value in list)
                    {
                        

                        FD_DynamicValue2 model = new FD_DynamicValue2();
                        model.PointId = point.ID;
                        model.MAXValue = value.SensorVal;
                        model.YXValue = 0;
                        model.AverageValue = 0;
                        model.DeviceId = dev.ID;
                        model.DynamicId = value.SensorStatus;

                        model.CreateDate = value.SensorDate.AsDatetime(DateTime.Now);

                        string sql1 = string.Format("select *  from {0} where PointId={1} and CreateDate='{2}'", DBName, point.ID, model.CreateDate);

                        var list1 = db.ExecuteQuery<FD_DataValue2>(sql1).ToList();
                        if(list1.Count>0)
                        {
                            continue;
                        }

                        db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.CreateDate, model.DynamicId));

                        
                        Delete(string.Format("delete from T_SensorVal where DevName='{0}' and SensorID='{1}' and SensorDate< '{2}'", dev.DeviceIden, point.MapDataName, value.SensorDate.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")));
                    }

                }
                AppContext.Log(dev.DeviceName + "数据导入完成......");
            }

        }

        public List<model> getTablie(string sqlstr)
        {
            List<model> list = new List<model>();
            string path = System.Configuration.ConfigurationManager.AppSettings["path"];
            //string path = "E:/HisData2.db";

            using (SQLiteConnection conn = new SQLiteConnection("data source = " + path + ";Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10"))
            {

                using (SQLiteDataAdapter ap = new SQLiteDataAdapter(sqlstr, conn))
                {
                    DataSet ds = new DataSet();
                    ap.Fill(ds, "ss");
                    if (ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        int count = dt.Rows.Count;
                        if (count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                model m = new model();
                                m.DevName = row[0].AsStr();
                                m.SensorID = row[1].AsStr();
                                m.SensorDate = row[2].AsDatetime(DateTime.Now);
                                m.SensorVal = row[3].AsDouble();
                                m.SensorStatus = row[4].AsInt();
                                list.Add(m);
                            }
                        }
                    }
                }
            }
            return list;
        }

        public void Delete(string sqlstr)
        {
            string path = System.Configuration.ConfigurationManager.AppSettings["path"];
            //string path = "E:/HisData2.db";

            using (SQLiteConnection conn = new SQLiteConnection("data source = " + path + ";Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10"))
            {
                conn.Open();
                using (SQLiteCommand ap = new SQLiteCommand(sqlstr, conn))
                {
                    ap.ExecuteNonQuery();
                }
                conn.Close();
            }
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
                        _myInstance = new SqlspliterRead();
                    }
                }
            }
            return _myInstance;
        }
    }

    public class model
    {
        public string DevName { set; get; }
        public string SensorID { set; get; }
        public DateTime SensorDate { set; get; }
        public double SensorVal { set; get; }
        public int SensorStatus { set; get; }
    }
}
