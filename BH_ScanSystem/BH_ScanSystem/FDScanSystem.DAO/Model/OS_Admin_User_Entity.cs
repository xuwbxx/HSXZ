using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WisDragon.Framework.EntityAttribute;

namespace FDScanSystem.DAO.Model {
    public class OS_Admin_User_Entity {

        [DataMapping("ID", System.Data.DbType.Int32)]
        public Int32 ID { get; set; }

        [DataMapping("UserName", System.Data.DbType.String)]
        public string UserName { get; set; }

        [DataMapping("Password", System.Data.DbType.String)]
        public string Pwd { get; set; }

        [DataMapping("UserType", System.Data.DbType.Byte)]
        public byte UserType { get; set; }

        [DataMapping("DepartId", System.Data.DbType.Int32)]
        public Int32 DepartId { get; set; }

        [DataMapping("RealName", System.Data.DbType.String)]
        public string RealName { get; set; }

        [DataMapping("Mobile", System.Data.DbType.String)]
        public string Mobile { get; set; }

        [DataMapping("IsLeader", System.Data.DbType.Boolean)]
        public Boolean IsLeader { get; set; }

        [DataMapping("IsProjectLeader", System.Data.DbType.Boolean)]
        public Boolean IsProjectLeader { get; set; }

        [DataMapping("Remark", System.Data.DbType.String)]
        public String Remark { get; set; }

        [DataMapping("DepartmentName", System.Data.DbType.String)]
        public string DepartmentName { get; set; }

        [DataMapping("Phone", System.Data.DbType.String)]
        public string Phone { get; set; }

        [DataMapping("Professional", System.Data.DbType.String)]
        public string Professional { get; set; }

        [DataMapping("CompanyId", System.Data.DbType.String)]
        public int CompanyId { get; set; }

        [DataMapping("Sex", System.Data.DbType.String)]
        public int Sex { get; set; }
    }
}
