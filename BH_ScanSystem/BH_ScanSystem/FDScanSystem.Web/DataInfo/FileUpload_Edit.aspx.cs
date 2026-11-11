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
using FDScanSystem.Protocol;
using FDScanSystem.Protocol.ScanConfig;
using OfficeShop.DAL;
using OfficeShop.Model;
using OfficeShop.WebModel;
using ZJFramework10;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.DataInfo
{
    public partial class FileUpload_Edit : AdminPageBase
    {
        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("cid");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var db = DataContextFactory.Create();
                var model = db.FD_FileUpload.SingleOrDefault(u => u.ID == Id);
                if (model != null)
                {
                    txtName.Text = model.FileName;
                    drpScanType.SelectedValue = model.FileType.ToString();
                }
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            FD_FileUpload model = new FD_FileUpload();
            if (Id > 0)
            {
                model = db.FD_FileUpload.SingleOrDefault(u => u.ID == Id);
            }
            model.FileName = txtName.Text;
            model.FileType = drpScanType.SelectedValue.AsInt();
            model.CreateDate = DateTime.Now;
            if (Id <= 0 && (this.upFile.PostedFile == null || this.upFile.PostedFile.ContentLength <= 0))
            {

                MessageBox.Alert("请上传【PDF】格式的文件,并且确认文件小于【2MB】");
                return;
            }

            if (this.upFile.PostedFile != null && this.upFile.PostedFile.ContentLength > 0)
            {
                if (!this.upFile.PostedFile.FileName.EndsWith("pdf", StringComparison.CurrentCultureIgnoreCase))
                {
                    MessageBox.Alert("请确认上传文件格式为【PDF】,并且确认文件小于【2MB】");
                    return;
                }
                var uploadLocation = FD_ScanSystem.WebModel.Uploader.UploadManager.Upload(this.upFile.PostedFile);
                if (uploadLocation != null)
                {
                    model.FileLocation = uploadLocation.FileLocation;
                    //fileInfo.SWFLocation = uploadLocation.SWFLocation;
                }
            }
            if (Id <= 0)
            {
                db.FD_FileUpload.InsertOnSubmit(model);
            }
            db.SubmitChanges();

            MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");

        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_FileUpload; }
        }
    }
}