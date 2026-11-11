using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FD_ScanSystem.WebModel;
using OfficeShop.Model;

namespace OfficeShop.WebModel
{
    public abstract class AdminPageBase : PageBase
    {


        protected override void OnInit(EventArgs e)
        {
            //return;
            AdminUserContext user = CurrentUser as AdminUserContext;
            if (IsAuthenticated)
            {
                if (user == null)
                {
                    Response.Redirect("/Login.aspx", true);
                    return;
                }
                else
                {
                    if ((user.FunctionRights == null || user.FunctionRights.Count == 0) && IsAuthenticated)
                    {
                        //跳转到无使用权限页面
                        Response.Redirect("/Login.aspx", true);
                        return;
                    }


                    if (!user.FunctionRights.Where(a => !string.Equals(a.FunctionUrl, null)).ToList().Exists(a => a.FunctionUrl.ToUpper().Contains(Request.FilePath.ToUpper())) && IsAuthenticated)
                    {
                        //跳转到无使用权限页面
                        Response.Redirect("/Error.aspx", true);
                        return;
                    }
                    if (user.FunctionRights.FirstOrDefault(u => u.FunctionCode.Equals(this.PermissionCode, StringComparison.CurrentCultureIgnoreCase)) == null)
                    {
                        Response.Redirect("/Error.aspx", true);
                        return;
                    }
                }
            }


            base.OnInit(e);
        }

        protected virtual void btnRefresh_CLick(object sender, EventArgs e)
        {

        }

        protected bool HasFunction(string code)
        {

            return AdminContext.FunctionRights.FirstOrDefault(u => u.FunctionCode.Equals(code, StringComparison.CurrentCultureIgnoreCase)) != null;
        }

        protected AdminUserContext AdminContext
        {
            get
            {
                return this.CurrentUser as AdminUserContext;
            }
        }

        public abstract string PermissionCode { get; }
    }


    public abstract class DialogPageBase : AdminPageBase
    {
        protected override void OnInit(EventArgs e)
        {


            base.OnInit(e);
        }


    }

    public static class PassPortExtenstion
    {

        public static bool HasFunction(this AdminUserContext context, string code)
        {
            if (PassPort.IsAuthenticated && context != null && context.FunctionRights.Count > 0)
            {
                return context.FunctionRights.Where(u => u.FunctionCode.Equals(code, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault() != null;
            }
            else
            {
                return false;
            }
        }
    }
}
