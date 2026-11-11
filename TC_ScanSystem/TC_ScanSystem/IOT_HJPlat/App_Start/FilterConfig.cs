using Model.Common;
using System.Web;
using System.Web.Mvc;
using Tool;

namespace IOT_HJPlat
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        
    }

    public class FilterHelper : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                Member UserInfo = CookieUtils.GetCookiesInfo();
                if (UserInfo == null)
                {
                    filterContext.Result = new RedirectResult("/Home/Login");
                    return;
                }

                var area = filterContext.RouteData.DataTokens;
                string AreaName = area["area"].ToString();
                var rd = filterContext.RouteData;
                var ControlName = rd.Values["Controller"].ToString();
                var ActionName = rd.Values["Action"].ToString();

                //看板页则跳过
                string Path = "/" + AreaName + "/" + ControlName + "/" + ActionName;
                if (Path.Equals("/Main/Board/Index"))
                {
                    return;
                }

                if (UserInfo.MenuPaths.Count == 0 || !UserInfo.MenuPaths.Contains(Path))
                {
                    CookieUtils.CleanUpCookie();
                    filterContext.Result = new RedirectResult("/Home/Login");
                    return;
                }

            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
