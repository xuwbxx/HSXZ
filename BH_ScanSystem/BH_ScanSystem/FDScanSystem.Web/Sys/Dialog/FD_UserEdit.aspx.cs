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
    public partial class FD_UserEdit : AdminPageBase
    {

        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("cid");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var db = DataContextFactory.Create();

                var model = db.FD_User.SingleOrDefault(u => u.ID == Id);
                if (model != null)
                {
                    txtName.Text = model.UserName;
                    txtPwd.Text = model.Password;
                    txtRealName.Text = model.RealName;
                    txtPwd.Enabled = false;
                }
                else
                {
                    txtPwd.Enabled = true;
                }

            }
        }


        public override string PermissionCode
        {
            get { return PermissionDict.F_System_User; }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            FD_User model = new FD_User();
            if (Id > 0)
            {
                model = db.FD_User.SingleOrDefault(u => u.ID == Id);
            }
            model.UserName = txtName.Text;
            
            model.RealName = txtRealName.Text;
            model.CreateDate = DateTime.Now;
            model.LastLoginDate = DateTime.Now;

            if (Id == 0)
            {
                model.Password = txtPwd.Text;
                db.FD_User.InsertOnSubmit(model);
            }
            db.SubmitChanges();
            MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
        }
    }
}