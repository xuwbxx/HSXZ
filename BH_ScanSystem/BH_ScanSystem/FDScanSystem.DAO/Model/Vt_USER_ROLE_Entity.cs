/*
* Copyrigth 2008 Company All rigths reserved
* File Name: TS_USER_ROLE_Entity.cs
* Created By:    Wenbin.Xu
* Created Date:   2012/11/13 10:50:40
* Description:    TS_USER_ROLE的实体类
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
    /// 表示TS_USER_ROLE的类型
    /// </summary>
    public class FD_USER_ROLE_Entity
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMapping("ID", System.Data.DbType.Int32)]
        public System.Int32 ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("UserId", System.Data.DbType.Int32)]
        public System.Int32 UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("RoleId", System.Data.DbType.Int32)]
        public System.Int32 RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMapping("RoleName", System.Data.DbType.String)]
        public System.String RoleName { get; set; }

    }
}