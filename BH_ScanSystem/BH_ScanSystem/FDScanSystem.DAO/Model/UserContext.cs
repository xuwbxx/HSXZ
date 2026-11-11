using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using FDScanSystem.DAO.Model;


namespace OfficeShop.Model
{
    /// <summary>
    ///UserContext 的摘要说明
    /// </summary>
    public abstract class UserContext
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public Int32 UserID
        {
            get;
            set;
        }

        /// <summary>
        /// 用户登录帐号
        /// </summary>
        public string UserName
        {
            get;
            set;
        }


        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName
        {
            get;
            set;
        }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime
        {
            get;
            set;
        }

        /// <summary>
        /// 身份证
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile
        {
            get;
            set;
        }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName
        {
            get;
            set;
        }

        /// <summary>
        /// 所在部门
        /// </summary>
        public string DepartmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 职务
        /// </summary>
        public string Professional
        {
            get;
            set;
        }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex
        {
            get;
            set;
        }


        /// <summary>
        /// 性别
        /// </summary>
        public string SexName
        {
            get {
                if (Sex == 0)
                {
                    return "男";
                }
                else
                {
                    return "女";
                }
            }
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        public string Remark { get; set; }


       // public abstract UserType UserType { get; set; }

    }



    public class AdminUserContext : UserContext
    {

        public AdminUserContext() { }

        public List<FD_Function_Entity> FunctionRights { get; set; }

        //public override UserType UserType
        //{
        //    get;
        //    set;
        //}
    }
}