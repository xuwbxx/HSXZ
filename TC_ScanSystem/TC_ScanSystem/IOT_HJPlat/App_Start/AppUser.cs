using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tool;

namespace IOT_HJPlat.App_Start
{
    public class AppUser
    {
        public static Member Current
        {
            get
            {
                try
                {
                    Member user = CookieUtils.GetCookiesInfo();
                    return user;
                }
                catch (Exception)
                {
                    return new Member();
                }
            }
        }
    }
}