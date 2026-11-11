using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FD_ScanSystem.WebModel;
using FDScanSystem.DAO;
using ZJFramework10;
using OfficeShop.WebModel;
using OfficeShop.DAL;
using OfficeShop.WebModel.Storage;
using OfficeShop.Model;
using ZJFramework10.Extension;

namespace FDScanSystem.Web
{
    public partial class Login : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            string userName = txt_User.Text;
            string password = txt_Password.Text;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                UserContext user = null;

                try
                {

                    user = PassPort.AdminUserLogin(userName, password);

                }
                catch (Exception ex) { Error(ex.Message); return; }

                if (user == null)
                {
                    Error("登录不成功,请检查用户名或密码是否输入正确");
                    return;
                }
                else
                {

                    var db = DataContextFactory.Create();
                    Session["CurContext"] = user;
                    UserCookieFacade.SetLoginCookie(user.UserName, password, 0);
                    var adminContext = user as AdminUserContext;
                    var url = adminContext.FunctionRights.Where(u => !string.IsNullOrEmpty(u.FunctionUrl)).FirstOrDefault();
                    if (url != null)
                    {
                        Response.Redirect(url.FunctionUrl);
                    }
                }
            }
        }
    }
}