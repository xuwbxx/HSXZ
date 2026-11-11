using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Video
{
    public enum HikResourceType
    {
        [Description("组织机构")]
        Org = 1000,

        [Description("用户")]
        User = 2000,

        [Description("编码设备")]
        Device = 30000,

        [Description("监控点")]
        Point = 10000,

        [Description("IO报警")]
        IO = 40000,

        [Description("对讲通道")]
        Channel = 90000,

        [Description("环境量")]
        Environment = 8803000,

        [Description("门禁")]
        EG = 501000
    }
}
