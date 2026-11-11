using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Client.Logic.ThreadManager
{
    public class ScanThreadContext
    {
        public ScanThreadContext(ScanDataTaskConfig config)
        {
            this.DeviceId = config.DeviceId;
            this.DeviceIdentity = config.DeviceIdentity;
            this.StartDateTime = DateTime.Now.ToFileTime();
            this.IsAlert = config.IsAlert;
            this.AlertPointId = config.PointId;
            this.AlertValue = config.AlertValue;
            this.TDJZPL = config.TDJZPL;
            this.ZDJZPL = config.ZDJZPL;
            this.Interval = config.Interval;
            
        }

        /// <summary>
        /// 是否是阀值采集
        /// </summary>
        public bool IsAlert { get; set; }

        /// <summary>
        /// 阀值报警对应的通道号
        /// </summary>
        public int AlertPointId { get; set; }

        /// <summary>
        /// 报警值
        /// </summary>
        public double AlertValue { get; set; }
        /// <summary>
        /// 采集设备ID
        /// </summary>
        public int DeviceId { get; set; }

        /// <summary>
        /// 通断截止频率
        /// </summary>
        public int TDJZPL { set; get; }

        /// <summary>
        /// 阻断截止频率
        /// </summary>
        public int ZDJZPL { set; get; }

        /// <summary>
        /// 采样间隔
        /// </summary>
        public int Interval { set; get; }

        ///// <summary>
        ///// 采样频率
        ///// </summary>
        //public int Perpiod { set; get; }

        /// <summary>
        /// 链接标识
        /// </summary>
        public string DeviceIdentity { get; set; }

        /// <summary>
        /// 启动时间
        /// </summary>
        public long StartDateTime { get; set; }
    }
}
