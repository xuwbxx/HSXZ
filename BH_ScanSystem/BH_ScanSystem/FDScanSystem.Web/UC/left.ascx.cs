using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using ZJFramework10.Extension;
using FD_ScanSystem.WebModel.Auth;
using FD_ScanSystem.WebModel;
using OfficeShop.WebModel;
using OfficeShop.Model;
using FDScanSystem.DAO;

namespace FDScanSystem.Web.UC
{
    public partial class left : System.Web.UI.UserControl
    {

        private List<FD_ScanSystem.WebModel.MenuItem> MenuList = null;
        private int UserId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            UserContext user = PassPort.CurrentUser;

            var adminContext = user as AdminUserContext;
            var url = adminContext.FunctionRights.Where(u => !string.IsNullOrEmpty(u.FunctionUrl)).FirstOrDefault();
            if (url != null)
            {
                ltUrl.Text = "<a href=\"" + url.FunctionUrl + "\">主页</a>";
            }

            UserId = user.UserID;



            MenuList = (from a in db.FD_User.Where(u => u.ID == UserId)
                        join b in db.FD_UserRole
                        on a.ID equals b.UserId
                        join c in db.FD_RoleFunction
                        on b.RoleId equals c.RoleId
                        join d in db.FD_Function
                        on c.FunctionId equals d.ID
                        where d.ParentId == 0 && d.IsMenu == true
                        orderby d.Sort
                        select new FD_ScanSystem.WebModel.MenuItem
                        {
                            Description = d.FunctionName,
                            FunctionId = d.ID,
                            Code = d.FunctionCode,
                            Link = d.FunctionUrl
                        }).ToList();
            menuRpt.DataSource = MenuList;
            menuRpt.DataBind();
        }

        protected void rpMenu_Bind(object sender, RepeaterItemEventArgs e)
        {

            var db = DataContextFactory.Create();
            int ParentId = MenuList[e.Item.ItemIndex].FunctionId;

            Repeater rp = e.Item.Controls[0].FindControl("menuSubRpt") as Repeater;
            if (rp != null)
            {
                rp.DataSource = (from a in db.FD_User.Where(u => u.ID == UserId)
                                 join b in db.FD_UserRole
                                 on a.ID equals b.UserId
                                 join c in db.FD_RoleFunction
                                 on b.RoleId equals c.RoleId
                                 join d in db.FD_Function
                                 on c.FunctionId equals d.ID
                                 where d.ParentId == ParentId && d.IsMenu == true
                                 orderby d.Sort
                                 select new FD_ScanSystem.WebModel.MenuItem
                                 {
                                     Description = d.FunctionName,
                                     FunctionId = d.ID,
                                     Code = d.FunctionCode,
                                     Link = d.FunctionUrl
                                 }).ToList();
                rp.DataBind();
            }

        }


        public string BindCheckClass(object o)
        {
            if (this.Page is AdminPageBase)
            {
                return (this.Page as AdminPageBase).PermissionCode.Equals(o.ToString(), StringComparison.CurrentCultureIgnoreCase) ? "active" : string.Empty;
            }
            return string.Empty;
        }

        //public int UserId()
        //{
        //    if (this.Page is AdminPageBase)
        //    {
        //        return (this.Page as AdminPageBase).CurrentUser.UserID;
        //    }
        //    return 0;
        //}
    }
}