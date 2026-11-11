using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Monitor.Response
{
    public class MonitorPointInfo
    {
        /// <summary>
        /// 监测点ID
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 传感器编号
        /// </summary>
        public string SensorName { set; get; }
        /// <summary>
        /// 测斜仪自编号（出厂编号）
        /// </summary>
        public string SensorNumber { set; get; }
        /// <summary>
        /// 监测位置编号
        /// </summary>
        public string PositionName { set; get; }
        /// <summary>
        /// 监测桩长度
        /// </summary>
        public string PileLength { set; get; }
        /// <summary>
        /// 监测桩直径
        /// </summary>
        public string PileDiameter { set; get; }
        /// <summary>
        /// 监测桩桩号(测斜管编号)
        /// </summary>
        public string PileNumber { set; get; }
        /// <summary>
        /// 实际标高
        /// </summary>
        public string ActualHeight { set; get; }
        /// <summary>
        /// 理论标高
        /// </summary>
        public string TheoryHeight { set; get; }
        /// <summary>
        /// 传感器类型
        /// </summary>
        public string SensorType { set; get; }
        public string PointName { set; get; }

    }
}
