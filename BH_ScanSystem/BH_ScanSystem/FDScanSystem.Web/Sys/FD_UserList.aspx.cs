using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDScanSystem.DAO;
using FD_ScanSystem.WebModel;
using FDScanSystem.DAO.Model;
using OfficeShop.Model;
using OfficeShop.DAL;
using OfficeShop.WebModel;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.Sys
{
    public partial class FD_UserList : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserContext user = PassPort.CurrentUser;
            var adminContext = user as AdminUserContext;
            if (adminContext != null)
            {
                var url = adminContext.FunctionRights.Where(u => !string.IsNullOrEmpty(u.FunctionUrl)).FirstOrDefault();
                if (url != null)
                {
                    ltUrl.Text = "<a href=\"" + url.FunctionUrl + "\">首页</a>";
                }
            }
            else
            {
                Response.Redirect("/login.aspx");
            }
            BindData();
        }

        private void BindData()
        {
            var db = DataContextFactory.Create();
            var list = db.FD_User.ToList();
            aspGrid.DataSource = list;
            aspGrid.DataBind();
        }

        protected void aspGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int key = e.Keys[0].AsInt();
            if (key > 0)
            {
                var db = DataContextFactory.Create();
                var cinfo = db.FD_User.FirstOrDefault(u => u.ID == key);
                if (cinfo != null)
                {
                    db.FD_User.DeleteOnSubmit(cinfo);
                    db.SubmitChanges();
                }
            }
        }

        protected void aspGrid_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            e.ExceptionHandled = true;
            BindData();
        }

        protected void btnRefresh_CLick(object sender, EventArgs e)
        { }

        public override string PermissionCode
        {
            get { return PermissionDict.F_System_User; }
        }
    }
}