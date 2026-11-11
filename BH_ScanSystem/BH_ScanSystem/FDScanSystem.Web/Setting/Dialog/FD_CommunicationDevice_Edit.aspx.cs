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
using ZJFramework10;

namespace FDScanSystem.Web.Setting.Dialog
{
    public partial class FD_CommunicationDevice_Edit : AdminPageBase
    {
        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("cid");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var db = DataContextFactory.Create();
                var model = db.FD_CommunicationDevice.SingleOrDefault(u => u.ID == Id);
                if (model != null)
                {
                    txtName.Text = model.DeviceName;
                    drpType.SelectedValue = model.CommunicationType.ToString();
                    drpProtocol.SelectedValue = model.DataProtocol.ToString();
                    txtIP.Text = model.IP;
                    txtCode.Text = model.CommunicationCode;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            FD_CommunicationDevice model = new FD_CommunicationDevice();
            if (Id > 0)
            {
                model = db.FD_CommunicationDevice.SingleOrDefault(u => u.ID == Id);
            }
            model.DeviceName = txtName.Text;
            model.CommunicationType = drpType.SelectedValue.AsInt();
            model.DataProtocol = drpProtocol.SelectedValue.AsInt();
            model.CommunicationCode = txtCode.Text;
            model.IP = txtIP.Text;
            model.CreateDate = DateTime.Now;
            //model.RoleCode = txtDes.Text;
            if (Id == 0)
            {
                db.FD_CommunicationDevice.InsertOnSubmit(model);
                //db.SubmitChanges();
                //model.DictID = model.ID;
            }
            //db.SubmitChanges();
            //model.DictID = model.ID;
            db.SubmitChanges();
            MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_CommunicationDevice; }
        }
    }
}