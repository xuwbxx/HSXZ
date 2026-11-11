using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZJFramework10.Extension;
namespace OfficeShop.WebModel.Storage {
    public class UserCookieFacade {

        const string USERINFO_COOKIE = "USERINFO_COOKIE";
        const string USERINFO_LOGIN_COOKIE = "USERINFO_LOGIN_COOKIE";


        const string USER_LEFTTREE_STATE = "USER_LEFTTREE_STATE";


        public static int GetLeftTreeState() {
            if (HttpContext.Current.Session[USER_LEFTTREE_STATE] == null) {
                return 1;
            }

            else {
                return HttpContext.Current.Session[USER_LEFTTREE_STATE].AsInt();
            }
        }


        public static void GetPasswordAndUsernameForCookie(out string userName, out string password, out int userType) {
            userName = string.Empty;
            password = string.Empty;
            userType = 0;
            string value = CookieHelper.GetCookieValue(USERINFO_COOKIE);
            if (!string.IsNullOrEmpty(value)) {
                string[] values = value.Split('#');
                if (values.Length >= 3) {
                    userType = values[0].AsInt();
                    userName = values[1];
                    password = values[2];
                }
            }
        }


        public static void SetLoginCookie(string userName, string password, int userType) {
            CookieHelper.SetCookie2(USERINFO_LOGIN_COOKIE, string.Concat(userType, "#", userName, "#", System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5")));
        }


        public static void ClearLoginCookie() {
            CookieHelper.ClearCookie(USERINFO_LOGIN_COOKIE);
        }

        public static void GetLoginCookie(out string userName, out string password, out int userType) {
            userName = string.Empty;
            password = string.Empty;
            userType = 0;
            string value = CookieHelper.GetCookieValue(USERINFO_LOGIN_COOKIE);
            if (!string.IsNullOrEmpty(value)) {
                string[] values = value.Split('#');
                if (values.Length >= 3) {
                    userType = values[0].AsInt(0);
                    userName = values[1];
                    password = values[2];
                }
            }
        }
    }
}
