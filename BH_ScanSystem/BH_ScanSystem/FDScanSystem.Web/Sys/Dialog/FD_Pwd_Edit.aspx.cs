using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeShop.Model;
using OfficeShop.WebModel;
using FD_ScanSystem.WebModel;
using FDScanSystem.DAO;

namespace FDScanSystem.Web.Sys.Dialog
{
    public partial class FD_Pwd_Edit  : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserContext user = Session["CurContext"] as UserContext;

            var db = DataContextFactory.Create();
            int userId = user.UserID;
            var userInfo = db.FD_User.FirstOrDefault(u => u.ID == userId);
            if (userInfo != null)
            {
                if (userInfo.Password != this.txtPwd.Text)
                {

                    this.txtPwd.ErrorText = "原始密码输入错误";
                    this.txtPwd.IsValid = false;
                    return;
                }
                if (this.txtNewPwd.Text.Trim().Length < 6)
                {
                    this.txtNewPwd.ErrorText = "密码长度必须大于5位";
                    this.txtNewPwd.IsValid = false;
                    return;
                }
                if (this.txtNewPwd.Text != this.txtNewPwd2.Text)
                {
                    this.txtNewPwd2.ErrorText = "两次密码输入不一致";
                    this.txtNewPwd2.IsValid = false;
                    return;
                }

                userInfo.Password = this.txtNewPwd.Text;
                db.SubmitChanges();
                // MessageBox.Show("用户密码保存成功", MessageBoxIcon.Sucess);
                this.txtNewPwd.Text = string.Empty;
                this.txtNewPwd2.Text = string.Empty;
                this.txtPwd.Text = string.Empty;
                MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");
            }
            else
            {
                MessageBox.Show("用户不存在,不可修改密码", MessageBoxIcon.Error);
                return;
            }
        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_System_User; }
        }
    }
}