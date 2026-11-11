using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FDScanSystem.DAO.Model {
    public enum AuthLevels {
        [Description("普通员工")]
        MySelf = 1,

        [Description("项目负责人")]
        TeamLeader = 3,

        [Description("部门领导")]
        DepartLeader = 6,

        [Description("综合管理者")]
        Manager = 9,
    }
}
