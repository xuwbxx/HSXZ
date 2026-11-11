/*
* Copyrigth 2008 Company All rigths reserved
* File Name: T_FUNCTION_Entity.cs
* Created By:    Wenbin.Xu
* Created Date:   2012/11/13 10:45:21
* Description:    T_FUNCTION的实体类
* Change History: 
* 
* */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WisDragon.Framework.EntityAttribute;

namespace OfficeShop.Model {
    /// <summary>
    /// 表示T_FUNCTION的类型
    /// </summary>
    public class FD_Function_Entity {
        /// <summary>
        /// 
        /// </summary>
        [DataMapping("ID", System.Data.DbType.Int32)]
        public System.Int32 ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("FunctionName", System.Data.DbType.String)]
        public System.String FunctionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("Description", System.Data.DbType.String)]
        public System.String DESCRIPTION { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("FunctionUrl", System.Data.DbType.AnsiString)]
        public System.String FunctionUrl { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMapping("FunctionCode", System.Data.DbType.AnsiString)]
        public System.String FunctionCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("ParentId", System.Data.DbType.Int32)]
        public System.Int32 ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("CreateDate", System.Data.DbType.DateTime)]
        public System.DateTime CreateDate{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("Sort", System.Data.DbType.Decimal)]
        public System.Decimal Sort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("ParentName", System.Data.DbType.String)]
        public System.String ParentName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("IsMenu", System.Data.DbType.Boolean)]
        public bool IsMenu { get; set; }


        public string IsMenuName {
            get {
                return IsMenu ? "是" : "否";
            }
        }
    }
}