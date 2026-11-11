using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FD_ScanSystem.WebModel.Enums
{
    public enum CommunicationTypeEnum
    {

        /// <summary>
        /// TCP
        /// </summary>
        [Description("TCP协议")]
        TCP = 0,

        /// <summary>
        /// MCU采集
        /// </summary>
        [Description("COM串口")]
        COM = 1,

    }
}
