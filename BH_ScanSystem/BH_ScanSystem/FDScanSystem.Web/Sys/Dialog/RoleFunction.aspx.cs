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
using OfficeShop.Helper;
using OfficeShop.DAL;
using OfficeShop.WebModel;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.Sys
{
    public partial class RoleFunction : AdminPageBase
    {

        private static List<FD_Function_Entity> FuncList = null;
        private static List<FD_Role_FUNCTION_Entity> RoleFuncList = null;
        public Int32 RoleId
        {
            get { return int.Parse(Request.QueryString["BizCode"]); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // this.lbRole.Text = Server.UrlDecode(Request.QueryString["name"]);
                BindData();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<FD_Role_FUNCTION_Entity> list = new List<FD_Role_FUNCTION_Entity>();
            int val = 0;
            foreach (TreeNode node in this.TreeView.CheckedNodes)
            {
                if (!int.TryParse(node.Value, out val))
                {
                    continue;
                }
                list.Add(new FD_Role_FUNCTION_Entity() { RoleId  = RoleId, FunctionId = val });
            }

            try
            {
                Sys_Permission_DAL.FD_RoleFunction_Update(RoleId, list);

                //MessageBox.Show("操作成功！", MessageBoxIcon.Sucess);
                MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
            }
            catch (Exception ex)
            {
                // ProcessUnhandledException(ex);
            }
        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_System_Role; }
        }

        private void BindData()
        {
            try
            {
                this.TreeView.Nodes.Clear();

                FuncList = Sys_Permission_DAL.FD_Function_GetAllFunction();

                RoleFuncList = Sys_Permission_DAL.FD_Role_Function_ListByRoleId(RoleId);

                TreeViewMethod.LoadTree<FD_Function_Entity>(this.TreeView, "0", FuncList.OrderBy(u => u.Sort).ToList(), RoleFuncList.Select(a => a.FunctionId.ToString()).ToList(), "ID", "FunctionName", "ParentId");
            }
            catch (Exception ex)
            {
                // ProcessUnhandledException(ex);
            }
        }
    }
}