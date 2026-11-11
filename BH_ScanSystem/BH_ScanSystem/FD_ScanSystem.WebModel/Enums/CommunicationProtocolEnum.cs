using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FD_ScanSystem.WebModel.Enums
{
    public enum CommunicationProtocolEnum
    {

        /// <summary>
        /// 默认协议
        /// </summary>
        [Description("默认协议")]
        DEF = 0,

        /// <summary>
        /// 自定义协议
        /// </summary>
        [Description("自定义协议")]
        CUM = 1,

    }
}
