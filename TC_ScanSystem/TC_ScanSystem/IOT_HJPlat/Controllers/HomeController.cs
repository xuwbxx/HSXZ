using DataEntity;
using DataEntity.Services;
using GWY_CTool.Login;
using IOT_HJPlat.App_Start;
using Model.Common;
using Model.HJPlat;
using Model.HJPlat.Request;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tool;


namespace IOT_HJPlat.Controllers
{
    public class HomeController : Controller
    {
        DBDataContext service = DataContextFactory.Create();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            //return Redirect("/Monitor/Main/Index");
            //return RedirectToAction("Index", "Board", new System.Web.Routing.RouteValueDictionary { { "Area", "Main" } });
            return View();
        }

        /// <summary>
        /// 登录跳转
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult DoLogin(LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return RedirectToAction("Login");
            }
            var user = service.Base_User.Where(a => a.UserName.Equals(request.UserName) && a.Password.Equals(request.Password)).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            //处理登录
            Member userinfo = new Member();
            userinfo.ID = user.ID;
            userinfo.UserName = user.UserName;
           // userinfo.DepartID = user.DepartID;
           // userinfo.DepartName = user.DepartName;
            userinfo.RealName = user.RealName;
           // userinfo.Mobile = user.Mobile;
            //userinfo.PositionID = user.PositionID;
            //userinfo.PositionName = user.PositionName;

            //角色菜单(1对多)
            List<int> RoleMenus = new List<int>();
            service.Base_UserRole.Where(a => !a.IsDelete.Value && a.UserID == user.ID).ToList().ForEach(a =>
            {
                userinfo.RoleIDs.Add(Convert.ToInt32(a.RoleID));

                service.View_Base_RoleMenu.Where(b => b.Type == 1 && b.RoleID == a.RoleID).ToList().ForEach(c => {
                    int MenuID = Convert.ToInt32(c.MenuID);
                    if (!RoleMenus.Contains(MenuID))
                    {
                        RoleMenus.Add(MenuID);
                    }
                });

            });

            //保存项目ID
            userinfo.ProjectCode = "HJ";
            userinfo.ProjectID = 1;

            //项目菜单（1对1）
            List<int> ProjectMenus = new List<int>();
            service.View_Base_ProjectMenu.Where(a => a.ProjectCode.Equals(userinfo.ProjectCode)).ToList().ForEach(a =>
            {
                int MenuID = Convert.ToInt32(a.MenuID);
                if (!ProjectMenus.Contains(MenuID))
                {
                    ProjectMenus.Add(MenuID);
                }
            });



            service.Base_Menu.Where(a => !a.IsDelete.Value && a.Type == 1).ToList().ForEach(a =>
            {
                if (RoleMenus.Contains(a.ID) && ProjectMenus.Contains(a.ID))
                {
                    userinfo.Menus.Add(a.ID);
                    userinfo.MenuPaths.Add(a.Path);
                }
            });


            //service.Repository_Base_UserRole.Query(a => !a.IsDelete.Value && a.UserID == user.ID).ToList().ForEach(a =>
            //{
            //    service.Repository_View_Base_RoleMenu.Query(b => b.Type == 1 && b.RoleID == a.RoleID).ToList().ForEach(c => {
            //        int MenuID = Convert.ToInt32(c.MenuID);
            //        if (ProjectMenus.Exists(p => p.MenuID == MenuID) && !userinfo.Menus.Contains(MenuID))
            //        {
            //            userinfo.Menus.Add(MenuID);
            //            if (!string.IsNullOrEmpty(c.Path))
            //            {
            //                userinfo.MenuPaths.Add(c.Path);
            //            }
            //        }
            //    });

            //});





            //保存cookie
            CookieUtils.SaveCookies(userinfo);

            return Redirect("/Monitor/Main/Index");
        }

        /// <summary>
        /// 平台中心登录
        /// </summary>
        /// <returns></returns>
        public ActionResult PassPortLogin()
        {
            try
            {
                string code = HttpContext.Request.QueryString["Code"].ToString();
                if (string.IsNullOrEmpty(code))
                {
                    return RedirectToAction("Login");
                }

                var info = GWY_CTool.Login.LoginCenter.GetLoginInfo(code);
                
                if (info == null)
                {
                    return RedirectToAction("Login");
                }

                string UserName = info.UserName;
                string Password = info.PassWord;
                
                //复制平台登录方法
                var user = service.Base_User.Where(a => a.UserName.Equals(UserName.Trim()) && a.Password.Equals(Password.Trim())).FirstOrDefault();

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                //处理登录
                Member userinfo = new Member();
                userinfo.ID = user.ID;
                userinfo.UserName = user.UserName;
               // userinfo.DepartID = user.DepartID;
                //userinfo.DepartName = user.DepartName;
                userinfo.RealName = user.RealName;
                //userinfo.Mobile = user.Mobile;
                //userinfo.PositionID = user.PositionID;
                //userinfo.PositionName = user.PositionName;

                service.Base_UserRole.Where(a => !a.IsDelete.Value && a.UserID == user.ID).ToList().ForEach(a =>
                {
                    userinfo.RoleIDs.Add(Convert.ToInt32(a.RoleID));

                    service.View_Base_RoleMenu.Where(b => b.Type == 1 && b.RoleID == a.RoleID).ToList().ForEach(c => {
                        int MenuID = Convert.ToInt32(c.MenuID);
                        if (!userinfo.Menus.Contains(MenuID))
                        {
                            userinfo.Menus.Add(MenuID);
                            if (!string.IsNullOrEmpty(c.Path))
                            {
                                userinfo.MenuPaths.Add(c.Path);
                            }
                        }
                    });

                });

                //保存cookie
                CookieUtils.SaveCookies(userinfo);

                return RedirectToAction("Index", "Board", new System.Web.Routing.RouteValueDictionary { { "Area", "Main" } });

            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// 外部平台登录
        /// </summary>
        /// <returns></returns>
        public ActionResult ExternalLogin()
        {
            //oSatS0Ri2tu52iPpqrEvEg==
            try
            {
                string code = HttpContext.Request.QueryString["Code"].ToString();
                if (string.IsNullOrEmpty(code))
                {
                    return RedirectToAction("Login");
                }

                string LoginKey = ConfigurationManager.AppSettings["LoginKey"].ToString();
                string LoginIV = ConfigurationManager.AppSettings["LoginIV"].ToString();

                string DeCode = System.Web.HttpUtility.UrlDecode(code).Replace("%2B", "+");

                var info = CryptographyUtils.TripleDESDecrypt(DeCode, LoginKey, LoginIV);

                string UserName = info.Split('-')[0];
                string Password = info.Split('-')[1];

                //复制平台登录方法
                var user = service.Base_User.Where(a => a.UserName.Equals(UserName.Trim()) && a.Password.Equals(Password.Trim())).FirstOrDefault();

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                //处理登录
                Member userinfo = new Member();
                userinfo.ID = user.ID;
                userinfo.UserName = user.UserName;
               // userinfo.DepartID = user.DepartID;
               // userinfo.DepartName = user.DepartName;
                userinfo.RealName = user.RealName;
                //userinfo.Mobile = user.Mobile;
               // userinfo.PositionID = user.PositionID;
                //userinfo.PositionName = user.PositionName;

                service.Base_UserRole.Where(a => !a.IsDelete.Value && a.UserID == user.ID).ToList().ForEach(a =>
                {
                    userinfo.RoleIDs.Add(Convert.ToInt32(a.RoleID));

                    service.View_Base_RoleMenu.Where(b => b.Type == 1 && b.RoleID == a.RoleID).ToList().ForEach(c => {
                        int MenuID = Convert.ToInt32(c.MenuID);
                        if (!userinfo.Menus.Contains(MenuID))
                        {
                            userinfo.Menus.Add(MenuID);
                            if (!string.IsNullOrEmpty(c.Path))
                            {
                                userinfo.MenuPaths.Add(c.Path);
                            }
                        }
                    });

                });

                //保存cookie
                CookieUtils.SaveCookies(userinfo);

                return RedirectToAction("Index", "Board", new System.Web.Routing.RouteValueDictionary { { "Area", "Main" } });

            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult LogOut()
        {
            CookieUtils.CleanUpCookie();
            return RedirectToAction("Login");
        }
    }
}