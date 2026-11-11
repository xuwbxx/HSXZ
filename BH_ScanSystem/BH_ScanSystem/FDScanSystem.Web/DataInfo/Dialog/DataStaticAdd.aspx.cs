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
    public partial class DataStaticAdd : AdminPageBase
    {
        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("pid");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int i = 0; i < 24; i++)
                {
                    drpHour.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                for (int i = 0; i < 60; i++)
                {
                    drpMin.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
                drpHour.SelectedIndex = 0;
                drpMin.SelectedIndex = 0;
                drpDate.Value = DateTime.Now;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();
            FD_DataValue model = new FD_DataValue();
            model.ScanValue5 = txtValue5.Text.AsDouble();
            model.ScanValue6 = txtValue6.Text.AsDouble();
            model.DataId = 0;
            model.ScanValue = 0;
            model.ScanValue2 = 0;
            model.ScanValue3 = 0;
            model.ScanValue4 = 0;
            model.Status = 0;
            
            if (Id > 0)
            {
                model.PointId = Id;
                string tempdate = drpDate.Date.ToString("yyyy-MM-dd") + " " + drpHour.SelectedValue + ":" + drpMin.SelectedValue + ":00";
                model.CreateDate = (drpDate.Date.ToString("yyyy-MM-dd") + " " + drpHour.SelectedValue + ":" + drpMin.SelectedValue + ":00").AsDatetime(DateTime.Now);
                db.FD_DataValue.InsertOnSubmit(model);
                db.SubmitChanges();
            }
            MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
        }

        public override string PermissionCode
        {
            get { return PermissionDict.DataInfo_Static_CalQuery; }
        }
    }
}