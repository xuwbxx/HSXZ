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
    public partial class FD_ScanLayer_Edit : AdminPageBase
    {
        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("cid");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var db = DataContextFactory.Create();
                drpNode.DataSource = db.FD_ScanNode.Where(u => u.NodeParentId == 0).ToList();
                drpNode.DataTextField = "NodeName";
                drpNode.DataValueField = "ID";
                drpNode.DataBind();
                drpNode.Items.Insert(0, new ListItem("请选择", "0"));
                drpNode.SelectedIndex = 0;
                var model = db.FD_ScanLayer.SingleOrDefault(u => u.ID == Id);
                if (model != null)
                {
                    txtName.Text = model.LayerName;
                    drpNode.SelectedValue = model.NodeId.ToString();

                    if (!string.IsNullOrEmpty(model.ImageLocation) && model.ImageLocation.Contains("Upload"))
                    {
                        img.NavigateUrl = model.ImageLocation;
                        img.Visible = true;
                    }
                    else
                    {
                        img.Visible = false;
                    }

                    //txtDes.Text = model.RoleCode;
                }
                else
                {
                    img.Visible = false;
                }

            }
        }

        protected void btn_Upload_Click(object sender, EventArgs e)
        {
            if (UploadFile(upImage).Length > 0)
            {
                img.NavigateUrl = "/Upload/" + UploadFile(upImage);
                img.Visible = true;
            }
        }


        private string UploadFile(FileUpload upload)
        {
            if (upload.HasFile)
            {
                string filename = upload.PostedFile.FileName.ToString();
                string type = filename.Substring(filename.LastIndexOf(".") + 1);
                string str = System.DateTime.Now.ToString("yyyyMMddhhmmss");
                string pic_FullName = str + "File." + type;
                string filepath = HttpContext.Current.Server.MapPath(String.Format("/Upload/{0}", pic_FullName));
                //filepath += pic_FullName;
                upload.PostedFile.SaveAs(filepath);                       //将图片文件上传至服务器filepath路径
                return pic_FullName;

            }
            return "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            FD_ScanLayer model = new FD_ScanLayer();
            if (Id > 0)
            {
                model = db.FD_ScanLayer.SingleOrDefault(u => u.ID == Id);
            }
            model.LayerName = txtName.Text;
            model.NodeId = drpNode.SelectedValue.AsInt();
            if (img.Visible == true)
            {
                model.ImageLocation = img.NavigateUrl;
            }
            else
            {
                model.ImageLocation = "";
            }
            //model.MapX = "0";
            //model.MapY = "0";
            //model.NodeIdPath = 0;
            //model.NodeImage = "";
            //model.NodeLevel = 0;
            //model.NodeParentId = 0;
            //model.NodeTextPath = "";
            //model.NodeTypeId = 0;
            //model.ProjectId = 0;

            //model.RoleCode = txtDes.Text;
            if (Id == 0)
            {
                db.FD_ScanLayer.InsertOnSubmit(model);

            }
            //db.SubmitChanges();
            //model.DictID = model.ID;
            db.SubmitChanges();
            MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
        }



        public override string PermissionCode
        {
            get { return PermissionDict.F_ScanLayer; }
        }
    }
}