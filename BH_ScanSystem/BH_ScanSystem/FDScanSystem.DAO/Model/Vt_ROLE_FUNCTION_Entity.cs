/*
* Copyrigth 2008 Company All rigths reserved
* File Name: T_ROLE_FUNCTION_Entity.cs
* Created By:    Wenbin.Xu
* Created Date:   2012/11/13 10:50:45
* Description:    T_ROLE_FUNCTION的实体类
* Change History: 
* 
* */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WisDragon.Framework.EntityAttribute;

namespace OfficeShop.Model
{
	/// <summary>
	/// 表示T_ROLE_FUNCTION的类型
	/// </summary>
    public class FD_Role_FUNCTION_Entity
	{
		/// <summary>
		/// 
		/// </summary>
        [DataMapping("ID", System.Data.DbType.Int32)]
        public System.Int32 ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [DataMapping("RoleId", System.Data.DbType.Int32)]
        public System.Int32 RoleId { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [DataMapping("FunctionId", System.Data.DbType.Int32)]
        public System.Int32 FunctionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("FunctionName", System.Data.DbType.String)]
        public System.String FunctionName { get; set; }
	}
}