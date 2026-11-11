using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FD_ScanSystem.WebModel;
using FD_ScanSystem.WebModel.Enums;
using FDScanSystem.DAO;
using FDScanSystem.DAO.Model;
using OfficeShop.DAL;
using OfficeShop.Model;
using OfficeShop.WebModel;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.DataInfo
{
    public partial class FileDownload : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //UserContext user = PassPort.CurrentUser;
            //var adminContext = user as AdminUserContext;
            //if (adminContext != null)
            //{
            //    var url = adminContext.FunctionRights.Where(u => !string.IsNullOrEmpty(u.FunctionUrl)).FirstOrDefault();
            //    if (url != null)
            //    {
            //        ltUrl.Text = "<a href=\"" + url.FunctionUrl + "\">首页</a>";
            //    }
            //    else
            //    {
            //        Response.Redirect("/login.aspx");
            //    }
            //}
            if (!IsPostBack)
            {

            }
            BindData();
        }

        private void BindData()
        {
            var db = DataContextFactory.Create();
            var list = db.FD_FileUpload.Where(u => u.FileType == drpScanType.Value.AsInt()); ;
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                list = list.Where(u => u.FileName == txtName.Text);
            }
            aspGrid.DataSource = list;
            aspGrid.DataBind();
        }

        protected void btnRefresh_CLick(object sender, EventArgs e)
        { }

        protected void btn_Choose_Click(object sender, EventArgs e)
        { }

        public override string PermissionCode
        {
            get { return PermissionDict.F_FileDownload; }
        }



        protected void aspGrid_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            e.ExceptionHandled = true;
            BindData();
        }
    }
}