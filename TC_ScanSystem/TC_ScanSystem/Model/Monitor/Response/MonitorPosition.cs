using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Monitor.Response
{
    /// <summary>
    /// 监测位置
    /// </summary>
    public class MonitorPosition
    {
        public MonitorPosition()
        {
            MonitorPoints = new List<MonitorPoint>();
            MonitorValues = new List<MonitorSensorValue>();
        }
        public int ID { set; get; }
        public int PositionId { set; get; }
        public string BackGroundPath { set; get; }

        public string MonitorPositionName { set; get; }

        public int Level { set; get; }
        public string IconClass { set; get; }

        public string X { set; get; }
    

        public string Y { set; get; }

        /// <summary>
        /// 是否报警(监测位置-监测点，取监测位置中所有监测点最近10条数据，只要有一条超标，就是true)
        /// </summary>
        public bool IsAlarm { set; get; }
        public string PointName { set; get; }
        public double? MaxValue { set; get; }
        public double? MinValue { set; get; }
        public int? DeviceId { set; get; }
        public int? DataCount { set; get; }

        public List<MonitorPoint> MonitorPoints { set; get; }
        public List<MonitorSensorValue> MonitorValues { set; get; }
    }

    /// <summary>
    /// 监测点
    /// </summary>
    public class MonitorPoint
    {
        public MonitorPoint()
        {
            MonitorValues = new List<MonitorSensorValue>();
        }
        public int ID { set; get; }

        public string MonitorPointName { set; get; }

        public string X { set; get; }
        public string Y { set; get; }
        public double? MaxValue { set; get; } 
        public double? MinValue { set; get; }

        public int? DeviceProtocol { set; get; }
        public List<MonitorSensorValue> MonitorValues { set; get; }
        // public MonitorSensor MonitorSensor { set; get; }
    }

    ///// <summary>
    ///// 传感器
    ///// </summary>
    //public class MonitorSensor
    //{
    //    public MonitorSensor()
    //    {
    //        MonitorValues = new List<MonitorSensorValue>();
    //    }

    //    //传感器属性按照设计图里的字段给

    //    public List<MonitorSensorValue> MonitorValues { set; get; }
    //}

    public class MonitorSensorValue
    {
        public string Value1 { set; get; }
        public string Value2 { set; get; }
        public string  CreateTime { set; get; }

        /// <summary>
        /// 本条数据超标，就是true
        /// </summary>
        public bool IsAlarm { set; get; }
        public int Level { set; get; }
    }
}
