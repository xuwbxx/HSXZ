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


namespace FDScanSystem.Web
{
    public partial class FD_FunctionList : AdminPageBase
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
            List<FD_Function_Entity> list = Sys_Permission_DAL.FD_Function_List();
            ASPxGridView1.DataSource = list;
            ASPxGridView1.DataBind();
        }


        protected void btn_Add_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("/FD_FunctionAdd.aspx");
        }

        //protected void Del_Click(object sender, EventArgs e)
        //{

        //}

        protected void aspGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int key = e.Keys[0].AsInt();
            if (key > 0)
            {
                var db = DataContextFactory.Create();
                var cinfo = db.FD_Function.FirstOrDefault(u => u.ID == key);
                if (cinfo != null)
                {
                    db.FD_Function.DeleteOnSubmit(cinfo);
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
            get { return PermissionDict.F_System_Function; }
        }
    }
}