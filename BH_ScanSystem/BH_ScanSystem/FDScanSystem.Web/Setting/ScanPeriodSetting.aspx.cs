using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeShop.WebModel;
using FD_ScanSystem.WebModel;
using FDScanSystem.DAO;
using ZJFramework10.Extension;
using FD_ScanSystem.WebModel.Enums;

namespace FDScanSystem.Web.Setting
{
    public partial class ScanPeriodSetting : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            var scanSetting = db.FD_Scan_Setting.FirstOrDefault(u => u.ScanType == (int)ScanTypeEnum.Static);
            if (!Page.IsPostBack && scanSetting != null)
            {
                this.txtBeginDate.SelectedDate = scanSetting.BeginDate;
                this.txtPeriod.Text = ((int)(scanSetting.Interval * 0.001)).ToString();
            }
        }


        public override string PermissionCode
        {
            get { return PermissionDict.Setting_Period; }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DAO.FD_Scan_Setting setting = new DAO.FD_Scan_Setting();
            var db = DataContextFactory.Create();
            bool isUpdate = true;
            var scanSetting = db.FD_Scan_Setting.FirstOrDefault(u => u.ScanType == (int)ScanTypeEnum.Static);

            if (scanSetting == null)
            {
                scanSetting = new FD_Scan_Setting();
                isUpdate = false;
            }
            scanSetting.ScanType = (int)ScanTypeEnum.Static;
            scanSetting.BeginDate = this.txtBeginDate.SelectedDate;
            scanSetting.Interval = this.txtPeriod.Text.AsInt();
            if (!isUpdate)
            {
                db.FD_Scan_Setting.InsertOnSubmit(scanSetting);
            }

            db.SubmitChanges();

            this.Successs("保存成功");
        }
    }
}