using Model.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Tool
{
    public class CookieUtils
    {
        static string LoginCookieKey = ConfigurationManager.AppSettings["LoginCookieKey"].ToString();
        static string LoginKey = ConfigurationManager.AppSettings["LoginKey"].ToString();
        static string LoginIV = ConfigurationManager.AppSettings["LoginIV"].ToString();


        #region 存储 Cookie
        /// <summary>
        /// 存储cookie(有效时间2小时)
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="cookieDomain">cookie域</param>        
        public static void CreateCookie(string cookieName, string cookieValue, string cookieDomain)
        {
            HttpCookie cookie = new HttpCookie(cookieName)
            {
                //Domain = NetUtils.RootDomain(cookieDomain),
                //HttpOnly = false,
                Path = "/",
                Secure = false,
                Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(cookieValue)),
                Expires = DateTime.Now.AddHours(2)
            };
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region 清除Cookie

        /// <summary>
        /// 清除Cookie
        /// </summary>
        public static void CleanUpCookie()
        {
            //清除所有 Session 记录
            HttpContext.Current.Session.Abandon();
            //清除所有的跨域 Cookie
            var cookies = HttpContext.Current.Request.Cookies;

            var cookieKeys = cookies.AllKeys.Select(key => cookies[key]);
            foreach (var httpCookie in cookieKeys)
            {
                httpCookie.Expires = DateTime.UtcNow.AddDays(-10);
                //httpCookie.Domain = NetUtils.RootDomain(HttpContext.Current.Request.Url.ToString());
                HttpContext.Current.Response.Cookies.Set(httpCookie);
            }
        }
        #endregion

        /// <summary>
        /// 保存cookies
        /// </summary>
        /// <param name="userinfo"></param>
        public static void SaveCookies(Member userinfo)
        {
            //1.清除cookies
            CleanUpCookie();

            //2.保存cookies
            string CookieValue = CryptographyUtils.TripleDESEncrypt(userinfo.Serialize(), LoginKey, LoginIV, CipherMode.CBC);
            CreateCookie(LoginCookieKey, CookieValue, HttpContext.Current.Request.Url.ToString());
            
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static Member GetCookiesInfo()
        {
            Member userinfo = new Member();
            try
            {
                var cookies = HttpContext.Current.Request.Cookies[LoginCookieKey];
                if (cookies == null)
                {
                    return null;
                }
                string accountToken = Encoding.UTF8.GetString(Convert.FromBase64String(cookies.Value));
                string userStr = CryptographyUtils.TripleDESDecrypt(accountToken, LoginKey, LoginIV, CipherMode.CBC);

                if (string.IsNullOrWhiteSpace(userStr)) return null;
                userinfo = userStr.Deserialize<Member>();
            }
            catch (Exception)
            {
                return null;
            }
            return userinfo;
        }

    }
}
