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

namespace FDScanSystem.Web
{
    public partial class FD_FunctionAdd : AdminPageBase
    {

        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("ID");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var db = DataContextFactory.Create();

                var group = db.FD_Function.Where(u => u.ParentId == 0).ToList();
                cbxGroup.DataSource = group;
                cbxGroup.DataTextField = "FunctionName";
                cbxGroup.DataValueField = "ID";  
                cbxGroup.DataBind();
                cbxGroup.Items.Insert(0, new ListItem("请选择", "0"));
                cbxGroup.SelectedIndex = 0;

                var model = db.FD_Function.SingleOrDefault(u => u.ID == Id);
                if (model != null)
                {
                    txtName.Text = model.FunctionName;
                    txtCode.Text = model.FunctionCode;
                    txtUrl.Text = model.FunctionUrl;
                    txtSort.Text = model.Sort.ToString();
                    cbxGroup.SelectedValue = model.ParentId.ToString();
                    chkIsMenu.Checked = model.IsMenu ?? false;
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            var db = DataContextFactory.Create();

            FD_Function model = new FD_Function();
            if (Id > 0)
            {
                model = db.FD_Function.SingleOrDefault(u => u.ID == Id);
            }
            model.FunctionCode = txtCode.Text;
            model.FunctionName = txtName.Text;
            model.FunctionUrl = txtUrl.Text;
            model.Sort = txtSort.Text.AsInt();
            model.ParentId = cbxGroup.SelectedValue.AsInt();
            model.CreateDate = DateTime.Now;
            model.IsMenu = chkIsMenu.Checked;
            if (Id == 0)
            {
                db.FD_Function.InsertOnSubmit(model);
            }
            db.SubmitChanges();

            MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
            //Context.Response.Redirect("/FD_FunctioinList.aspx");
        }

        //protected void btn_Back_Click(object sender, EventArgs e)
        //{
        //    Context.Response.Redirect("/FD_FunctioinList.aspx");
        //}

        public override string PermissionCode
        {
            get { return PermissionDict.F_System_Function; }
        }
    }
}