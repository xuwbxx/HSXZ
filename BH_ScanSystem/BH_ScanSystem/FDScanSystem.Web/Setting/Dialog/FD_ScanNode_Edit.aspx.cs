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
using ZJFramework10;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.Setting.Dialog
{
    public partial class FD_ScanNode_Edit : AdminPageBase
    {
        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("cid");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var db = DataContextFactory.Create();

                var model = db.FD_ScanNode.SingleOrDefault(u => u.ID == Id);
                if (model != null)
                {
                    txtName.Text = model.NodeName;
                    txtX.Text = model.MapX;
                    txtY.Text = model.MapY;
                    //txtDes.Text = model.RoleCode;
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            FD_ScanNode model = new FD_ScanNode();
            if (Id > 0)
            {
                model = db.FD_ScanNode.SingleOrDefault(u => u.ID == Id);
            }
            model.NodeName = txtName.Text;
            model.MapX = txtX.Text;
            model.MapY = txtY.Text;
            model.NodeIdPath = 0;
            model.NodeImage = "";
            model.NodeLevel = 0;
            model.NodeParentId = 0;
            model.NodeTextPath = "";
            model.NodeTypeId = 0;
            model.ProjectId = 0;

            //model.RoleCode = txtDes.Text;
            if (Id == 0)
            {
                db.FD_ScanNode.InsertOnSubmit(model);

            }
            //db.SubmitChanges();
            //model.DictID = model.ID;
            db.SubmitChanges();
            MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_ScanNode; }
        }
    }
}