using DataEntity;
using DataEntity.Services;
//using GWY_SourceCode;
using Model.Common;
using Model.HJPlat.Request;
using Model.HJPlat.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.Manage;
using Tool;
using Model.Manage.Response;
using Model.Manage.Request;

namespace IOT_HJPlat.Areas.Manage.Controllers
{
    public class MenuController:Controller
    {
        DBDataContext service = DataContextFactory.Create();

        [FilterHelper]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult DataQuery(MenuRequest request)
        {
            BaseReturn ret = new BaseReturn();
            
            try
            {
                MenuConfig ser = new MenuConfig();
                var Menus = ser.GetMenuTree();

                ret.Data = Menus;
                ret.Success = true;
            }
            catch (Exception e)
            {
                ret.Message = "错误";
                ret.Success = false;
            }

            return Json(ret, JsonRequestBehavior.DenyGet);
        }
        
        [HttpPost]
        public JsonResult Add(MenuRequest request)
        {
            BaseReturn ret = new BaseReturn();

            try
            {
                if (string.IsNullOrEmpty(request.MenuName))
                {
                    ret.Message = "菜单名不能是空";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                if (request.ParentID != 0)
                {
                    var ParentMenu = service.Base_Menu.Where(a => !a.IsDelete.Value && a.ID == request.ParentID).FirstOrDefault();
                    if (ParentMenu == null)
                    {
                        ret.Message = "此父节点菜单不存在";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                }
                Base_Menu menu = new Base_Menu();
                menu.Class = request.Class;
                menu.CreateTime = DateTime.Now;
                menu.FunctionCode = request.FunctionCode;
                menu.IsDelete = false;
                menu.MenuName = request.MenuName;
                menu.Order = request.Order;
                menu.ParentID = request.ParentID;
                menu.Path = request.Path;
                menu.Type = 1;  //1: PC端  2：手机端
                service.Base_Menu.InsertOnSubmit(menu);
                service.SubmitChanges();

                ret.Success = true;
                CacheUtils.Remove(CacheKey.菜单数据缓存);
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult Modify(MenuRequest request)
        {
            BaseReturn ret = new BaseReturn();

            try
            {
                if (request.ID == 0)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                var Menus = service.Base_Menu.Where(a => !a.IsDelete.Value && a.Type == 1).ToList();

                var Menu = Menus.FirstOrDefault(a => a.ID == request.ID);
                if (Menu == null)
                {
                    ret.Message = "数据出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (Menu.ParentID == 0 && request.ParentID != 0)
                {
                    ret.Message = "1级菜单不能修改父节点";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                //判断父节点层级
                var FatherMenu = Menus.FirstOrDefault(a => a.ID == request.ParentID);
                if (request.ParentID != 0)
                {
                    if (FatherMenu == null)
                    {
                        ret.Message = "不存在此父节点";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                    if (FatherMenu != null && FatherMenu.ParentID != 0)
                    {
                        ret.Message = "层级不能超过2级";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                }
                
                Menu.Class = request.Class;
                Menu.CreateTime = DateTime.Now;
                Menu.FunctionCode = request.FunctionCode;
                Menu.MenuName = request.MenuName;
                Menu.Order = request.Order;
                Menu.ParentID = request.ParentID;
                Menu.Path = request.Path;

             //   serviceBase_Menu.Update(Menu);
                service.SubmitChanges();
                CacheUtils.Remove(CacheKey.菜单数据缓存);
                ret.Success = true;

            }
            catch (Exception)
            {
                ret.Success = false;
            }
            
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult Delete(MenuRequest request)
        {
            BaseReturn ret = new BaseReturn();

            try
            {
                if (request == null || request.ID <= 0)
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                var configInfo = service.Base_Menu.Where(a => a.ID == request.ID).FirstOrDefault();

                if (configInfo == null)
                {
                    ret.Message = "配置信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (!configInfo.IsDelete.HasValue || !configInfo.IsDelete.Value)
                {
                    configInfo.IsDelete = true;
                    // service.Base_Menu.Update(configInfo);
                    service.SubmitChanges();
                }

                //3.保存
               

                ret.Success = true;

                CacheUtils.Remove(CacheKey.菜单数据缓存);
            }
            catch (Exception ex)
            {
                ret.Message = "错误";
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

    }
}