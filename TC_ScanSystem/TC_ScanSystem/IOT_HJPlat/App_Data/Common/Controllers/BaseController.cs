using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tool;

namespace IOT_THPlat.Areas.Common.Controllers
{
    public class BaseController : Controller
    {
        public string ProjectCode {
            get
            {
                Member user = CookieUtils.GetCookiesInfo();
                return user.ProjectCode;
            }
        }

        public int ProjectID
        {
            get
            {
                Member user = CookieUtils.GetCookiesInfo();
                return user.ProjectID;
            }
        }
    }
}