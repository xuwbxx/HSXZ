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
using ZJFramework10;
namespace FDScanSystem.Web.Sys
{
    public partial class FD_RoleEdit : AdminPageBase
    {
        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("cid");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var db = DataContextFactory.Create();

                var model = db.FD_Role.SingleOrDefault(u => u.ID == Id);
                if (model != null)
                {
                    txtName.Text = model.RoleName;
                    txtDes.Text = model.RoleCode;
                }

            }
        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_System_Role; }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            FD_Role model = new FD_Role();
            if (Id > 0)
            {
                model = db.FD_Role.SingleOrDefault(u => u.ID == Id);
            }
            model.RoleName = txtName.Text;
            model.RoleCode = txtDes.Text;
            if (Id == 0)
            {
                db.FD_Role.InsertOnSubmit(model);
            }
            db.SubmitChanges();
            MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
        }
    }
}