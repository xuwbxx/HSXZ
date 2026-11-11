using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FDScanSystem.DAO.Model {
    public enum DepartConst {
        [Description("工程结构研究室")]
        Depart_1 = 1,

        [Description("岩土工程研究室")]
        Depart_2 = 2,

        [Description("工程材料研究室")]
        Depart_3 = 3,
        
        [Description("工艺自动化研究所")]
        Depart_4 = 4,


        [Description("生产技术部")]
        Depart_12 = 12,

        [Description("财务部")]
        Depart_15 = 15,
    }
}
