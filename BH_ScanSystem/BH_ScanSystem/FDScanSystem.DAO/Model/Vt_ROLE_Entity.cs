/*
* Copyrigth 2008 Company All rigths reserved
* File Name: T_ROLE_Entity.cs
* Created By:    Wenbin.Xu
* Created Date:   2012/11/13 10:50:21
* Description:    T_ROLE的实体类
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
	/// 表示T_ROLE的类型
	/// </summary>
    public class FD_Role_Entity
	{
		/// <summary>
		/// 
		/// </summary>
        [DataMapping("ID", System.Data.DbType.Int32)]
        public System.Int32 ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [DataMapping("ROLENAME", System.Data.DbType.String)]
		public System.String ROLENAME { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [DataMapping("DESCRIPTION", System.Data.DbType.String)]
		public System.String DESCRIPTION { get; set; }

		/// <summary>
		/// 
		/// </summary>
        [DataMapping("CREATEDATE", System.Data.DbType.DateTime)]
		public System.DateTime CREATEDATE { get; set; }

	}
}