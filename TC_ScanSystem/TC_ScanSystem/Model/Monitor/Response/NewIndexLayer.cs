using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Monitor.Response
{
    /// <summary>
    /// 监测总览图新版页面（/Monitor/Main/NewIndex）排架列表项
    /// </summary>
    public class NewIndexLayer
    {
        /// <summary>
        /// 图层 ID（MK_Layer.ID）
        /// </summary>
        public int LayerID { set; get; }

        /// <summary>
        /// 图层名称
        /// </summary>
        public string LayerName { set; get; }

        /// <summary>
        /// 监测/采集类型 ID（MK_ScanType.ID），用于详情页参数
        /// </summary>
        public int ScanTypeID { set; get; }

        /// <summary>
        /// 监测/采集类型名称
        /// </summary>
        public string ScanTypeName { set; get; }

        /// <summary>
        /// 报警次数（口径与 GetMonitorAlarm 一致）
        /// </summary>
        public int Level { set; get; }

        /// <summary>
        /// 是否告警（Level &gt; 0）
        /// </summary>
        public bool IsAlarm { set; get; }
    }
}

