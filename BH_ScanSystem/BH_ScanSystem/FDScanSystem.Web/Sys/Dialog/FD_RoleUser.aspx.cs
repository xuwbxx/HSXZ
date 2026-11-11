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

namespace FDScanSystem.Web.Sys.Dialog
{
    public partial class FD_RoleUser : AdminPageBase
    {

        private static List<FD_Role_Entity> RoleList = null;
        private static List<FD_USER_ROLE_Entity> UserRoleList = null;

        public int UserId
        {
            get { return int.Parse(Request.QueryString["BizCode"]); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lbUser.Text = Server.UrlDecode(Request.QueryString["name"]);
                BindData();
            }
        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_System_User; }
        }

        private void BindData()
        {
            try
            {
                this.cklRole.Items.Clear();

                RoleList = Sys_Permission_DAL.FD_Role_GetAllRole();
                this.cklRole.DataSource = RoleList;
                this.cklRole.DataBind();

                UserRoleList = Sys_Permission_DAL.FD_USER_Role_ListByUserId(UserId);
                for (int i = 0; i < UserRoleList.Count; i++)
                {
                    ListItem item = this.cklRole.Items.FindByValue(UserRoleList[i].RoleId.ToString());
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                // ProcessUnhandledException(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<FD_USER_ROLE_Entity> list = new List<FD_USER_ROLE_Entity>();
            decimal val = 0;
            for (int i = 0; i < this.cklRole.Items.Count; i++)
            {
                if (this.cklRole.Items[i].Selected)
                {
                    list.Add(new FD_USER_ROLE_Entity() { UserId = UserId, RoleId = int.Parse(this.cklRole.Items[i].Value) });
                }
            }

            try
            {
                Sys_Permission_DAL.FD_USER_Role_Update(UserId, list);

                // MessageBox.Show("操作成功！", MessageBoxIcon.Sucess);
                MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
            }
            catch (Exception ex)
            {
                // ProcessUnhandledException(ex);
            }
        }
    }
}