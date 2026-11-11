using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FD_ScanSystem.WebModel;
using FD_ScanSystem.WebModel.Enums;
using FDScanSystem.DAO;
using OfficeShop.Model;
using OfficeShop.WebModel;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.DataInfo.Dialog
{
    public partial class DataStaticEdit : AdminPageBase
    {

        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("cid");
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {
                var db = DataContextFactory.Create();

                FD_DataValue model = db.FD_DataValue.FirstOrDefault(u => u.ID == Id);
                if (model != null)
                {
                    txtValue5.Text = model.ScanValue5.ToString();
                    txtValue6.Text = model.ScanValue6.ToString();
                }
            }

        }   

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            FD_DataValue model = db.FD_DataValue.FirstOrDefault(u => u.ID == Id);
            if (model != null)
            {
                model.ScanValue5 = txtValue5.Text.AsDouble();
                model.ScanValue6 = txtValue6.Text.AsDouble();
                db.SubmitChanges();
                MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
            }
        }

        public override string PermissionCode
        {
            get { return PermissionDict.DataInfo_Static_CalQuery; }
        }
    }
}