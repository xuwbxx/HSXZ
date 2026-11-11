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

namespace FDScanSystem.Web.Setting
{
    public partial class FD_ScanLayer_List : AdminPageBase
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
                else
                {
                    Response.Redirect("/login.aspx");
                }
            }
            //if (!IsPostBack)
            //{

            //}
            BindData();
        }

        protected void btnRefresh_CLick(object sender, EventArgs e)
        { }

        protected void btn_Choose_Click(object sender, EventArgs e)
        { }

        private void BindData()
        {
            var db = DataContextFactory.Create();



            var list = (from a in db.FD_ScanLayer
                        join b in db.FD_ScanNode
                        on a.NodeId equals b.ID
                        select new PointView
                        {
                            LayerName = a.LayerName,
                            PointName = b.NodeName,
                            ID = a.ID

                        }).ToList();

            aspGrid.DataSource = list.OrderBy(u => u.PointName);
            aspGrid.DataBind();
        }

        protected void aspGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int key = e.Keys[0].AsInt();
            if (key > 0)
            {
                var db = DataContextFactory.Create();
                var cinfo = db.FD_ScanLayer.FirstOrDefault(u => u.ID == key);
                if (cinfo != null)
                {
                    db.FD_ScanLayer.DeleteOnSubmit(cinfo);
                    db.SubmitChanges();
                }
            }
        }

        protected void aspGrid_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            e.ExceptionHandled = true;
            BindData();
        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_ScanLayer; }
        }
    }
}