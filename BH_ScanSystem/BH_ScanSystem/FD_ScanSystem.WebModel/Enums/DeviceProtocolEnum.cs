using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FD_ScanSystem.WebModel.Enums
{
    public enum DeviceProtocolEnum
    {

        /// <summary>
        /// 加速采集
        /// </summary>
        [Description("动态采集设备")]
        JIASU = 0,

        /// <summary>
        /// MCU采集
        /// </summary>
        [Description("静态采集设备")]
        MCU = 1,
    }
}
