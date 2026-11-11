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
    public partial class FD_ScanType_Edit : AdminPageBase
    {

        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("cid");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var db = DataContextFactory.Create();

                var model = db.FD_Dict.SingleOrDefault(u => u.ID == Id);
                if (model != null)
                {
                    txtName.Text = model.DictValue;
                    //txtDes.Text = model.RoleCode;
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            FD_Dict model = new FD_Dict();
            if (Id > 0)
            {
                model = db.FD_Dict.SingleOrDefault(u => u.ID == Id);
            }
            model.DictValue = txtName.Text;
            model.DictType = DictTypeEnum.SCANTYPE;
            //model.RoleCode = txtDes.Text;
            if (Id == 0)
            {
                db.FD_Dict.InsertOnSubmit(model);
                db.SubmitChanges();
                model.DictID = model.ID;
            }
            //db.SubmitChanges();
            //model.DictID = model.ID;
            db.SubmitChanges();
            MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_ScanType; }
        }
    }
}