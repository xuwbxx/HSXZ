using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDScanSystem.DAO;
using FD_ScanSystem.WebModel;
using FD_ScanSystem.WebModel.Enums;
using FDScanSystem.DAO.Model;
using OfficeShop.Model;
using OfficeShop.DAL;
using OfficeShop.WebModel;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.Setting
{
    public partial class FD_CommunicationDevice_List : AdminPageBase
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
            var list = (from e1 in db.FD_CommunicationDevice
                        select new CommunicationView
                        {
                            ID = e1.ID,
                            IP = e1.IP,
                            Port = e1.Port,
                            CommunicationName = e1.DeviceName,
                            communicate = e1.CommunicationType,
                            protocol = e1.DataProtocol,
                        }).ToList();
            aspGrid.DataSource = list;
            aspGrid.DataBind();
        }

        protected void aspGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int key = e.Keys[0].AsInt();
            if (key > 0)
            {
                var db = DataContextFactory.Create();
                var cinfo = db.FD_CommunicationDevice.FirstOrDefault(u => u.ID == key);
                if (cinfo != null)
                {
                    db.FD_CommunicationDevice.DeleteOnSubmit(cinfo);
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
            get { return PermissionDict.F_CommunicationDevice; }
        }
    }

    public  class CommunicationView
    {
        public int ID { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public string CommunicationName { get; set; }
        public string CommunicationType
        {
            get
            {
                if (communicate == (int)CommunicationTypeEnum.TCP) { return "TCP"; }
                else { return "COM"; }
            }
        }
        public int communicate { get; set; }
        public string DataProtocol
        {
            get
            {
                if (protocol == (int)CommunicationProtocolEnum.DEF) { return "默认协议"; }
                else { return "自定义协议"; }
            }
        }
        public int protocol { get; set; }
       
        public int device { get; set; }
    }
}