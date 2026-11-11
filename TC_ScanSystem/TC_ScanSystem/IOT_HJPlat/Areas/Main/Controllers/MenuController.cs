using Business.Manage;
using DataEntity.Services;
using IOT_HJPlat.App_Start;
using Model.HJPlat.Response;
using Model.Manage.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataEntity;

namespace IOT_HJPlat.Areas.Main.Controllers
{
    public class MenuController : Controller
    {
        DBDataContext service = DataContextFactory.Create();
        // GET: Main/Menu
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult TopMenu()
        {
            try
            {
               
                    ViewData["RealName"] = AppUser.Current.RealName;
            }
            catch
            {
                Response.Redirect("/Home/LogOut");
            }
            return PartialView();
        }

        public PartialViewResult LeftMenu()
        {
            MenuHelper helper = new MenuHelper();
            var AllMenus = helper.GetMenus();

            //第一级菜单
            var FirstMenus = AllMenus.Where(a => a.ParentID == 0).OrderBy(a => a.Order);

            List<MenuResponse> list = new List<MenuResponse>();
            foreach (var menu1 in FirstMenus)
            {
                MenuResponse model = new MenuResponse();
                model.ID = menu1.ID;
                model.Class = menu1.Class;
                model.MenuName = menu1.MenuName;
                model.Order = menu1.Order;
                model.Path = menu1.Path;

                //取出耳机菜单
                var SecondMenu = AllMenus.Where(a => a.ParentID == menu1.ID).OrderBy(a => a.Order);
                foreach (var menu2 in SecondMenu)
                {
                    MenuResponse child = new MenuResponse();
                    child.ID = menu2.ID;
                    child.Class = menu2.Class;
                    child.MenuName = menu2.MenuName;
                    child.Order = menu2.Order;
                    child.ParentID = menu2.ParentID;
                    child.Path = menu2.Path;
                    model.Child.Add(child);
                }
                list.Add(model);
            }

            //activeMenu
            var ActivedMenu = HttpContext.Request.Cookies["IOTMenuActived"];
            if (ActivedMenu != null)
            {
                ViewData["ActivedMenu"] = ActivedMenu.Value;
            }

            return PartialView(list);
        }
    }
}