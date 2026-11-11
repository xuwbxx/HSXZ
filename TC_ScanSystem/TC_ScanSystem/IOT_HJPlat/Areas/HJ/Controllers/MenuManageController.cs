using DataEntity.DataContext;
using DataEntity.Services;
using Model.Common;
using Model.HJPlat.Request;
using Model.HJPlat.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOT_HJPlat.Areas.HJ.Controllers
{
    public class MenuManageController : Controller
    {
        WorkService service = new WorkService();

        [FilterHelper]
        public ActionResult Index()
        {
            //ViewData["Departs"] = service.Repository_Base_Depart.Query(a => !a.IsDelete.Value).ToList();
            //ViewData["Positions"] = service.Repository_Base_Position.Query(a => !a.IsDelete.Value).ToList();
            //ViewData["Structors"] = service.Repository_Base_Structor.Query(a => !a.IsDelete).ToList();
            //ViewData["Roles"] = service.Repository_Base_Role.Query(a => !a.IsDelete.Value).ToList();
            return View();
        }


        [HttpPost]
        public JsonResult DataQuery(MenuRequest request)
        {

            BaseReturn ret = new BaseReturn();
            try
            {
                //组装视图
                // var query = service.Repository_View_BaseUser.Query().AsQueryable();
                var query = service.Repository_Base_Menu.Query().Where(u => u.IsDelete == false);

                //筛选
                if (!string.IsNullOrEmpty(request.MenuName))
                {
                    query = query.Where(a => a.MenuName.Contains(request.MenuName));
                }
                if (!string.IsNullOrEmpty(request.FunctionCode))
                {
                    query = query.Where(a => a.FunctionCode.Contains(request.FunctionCode));
                }

                int Count = query.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                //如果不是第一页，并且没有数据，则查询第一页
                var CurrentQuery = query.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = query.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                ret.Data = CurrentQuery;
                ret.Success = true;
                ret.PageIndex = request.PageIndex;

            }
            catch (Exception ex)
            {
                ret.Success = false;
                //记录日志

            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DataDelete(MenuRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.ID == 0)
                {
                    ret.Success = false;
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                var Menu = service.Repository_Base_Menu.Query(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                if (Menu == null)
                {
                    ret.Success = false;
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                Menu.IsDelete = true;
                Menu.CreateTime = DateTime.Now;
                service.SaveChange();

                ret.Success = true;

            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult MenuQuery(MenuRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.ID == 0)
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                var Menu = service.Repository_Base_Menu.Query(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                if (Menu == null)
                {
                    ret.Success = false;
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                MenuResponse se = new MenuResponse();
                se.ID = Menu.ID;
                se.Class = Menu.Class;
                se.MenuName = Menu.MenuName;
                se.Order = Menu.Order;
                se.ParentID = Menu.ParentID;
                se.Path = Menu.Path;
                se.Type = Menu.Type;
                se.IsDelete = Menu.IsDelete;
                se.Depth = Menu.Depth;
                se.CreateTime = Menu.CreateTime;
                se.FunctionCode = Menu.FunctionCode;
                //string Roles = "";
                //service.Repository_Base_UserRole.Query(a => !a.IsDelete.Value && a.UserID == User.ID).ToList().ForEach(a =>
                //{
                //    Roles += a.RoleID + ",";
                //});
                //se.Roles = Roles.Trim(',');

                ret.Data = se;
                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        public JsonResult MenuSave(MenuRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (string.IsNullOrEmpty(request.MenuType))
                {
                    ret.Message = "出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                if (string.IsNullOrEmpty(request.MenuName))
                {
                    ret.Message = "菜单名称不能为空";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                DateTime now = DateTime.Now;
                if (request.MenuType.Equals("Modify"))
                {
                    if (request.ID == 0)
                    {
                        ret.Message = "出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                    var Menu = service.Repository_Base_Menu.Query(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                    if (Menu == null)
                    {
                        ret.Message = "出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                    //修改用户表
                    Menu.MenuName = request.MenuName;
                    Menu.Class = request.Class;
                    Menu.Depth = request.Depth;
                    Menu.ParentID = request.ParentID;
                    Menu.Order = request.Order;
                    Menu.Path = request.Path;
                    //Menu.RoleID = 0;
                    Menu.Type = request.Type;
                    Menu.FunctionCode = request.FunctionCode;

                    //修改角色表
                    //先删除
                    //var oldRoles = service.Repository_Base_UserRole.Query(a => !a.IsDelete.Value && a.UserID == request.ID).ToList();
                    //service.Repository_Base_UserRole.DeleteList(oldRoles);
                    ////再添加
                    //if (!string.IsNullOrEmpty(request.Roles))
                    //{
                    //    List<Base_UserRole> list = new List<Base_UserRole>();
                    //    string[] Roles = request.Roles.Trim(',').Split(',');
                    //    for (int i = 0; i < Roles.Length; i++)
                    //    {
                    //        Base_UserRole role = new Base_UserRole();
                    //        role.UserID = User.ID;
                    //        role.RoleID = Convert.ToInt32(Roles[i]);
                    //        role.IsDelete = false;
                    //        role.CreateTime = now;
                    //        list.Add(role);
                    //    }
                    //    service.Repository_Base_UserRole.AddList(list);
                    //}
                    service.SaveChange();
                    ret.Success = true;
                }
                else if (request.MenuType.Equals("Add"))
                {
                    Base_Menu Menu = new Base_Menu();
                    Menu.MenuName = request.MenuName;
                    Menu.Class = request.Class;
                    Menu.Depth = request.Depth;
                    Menu.ParentID = request.ParentID;
                    Menu.Path = request.Path;
                    //Menu.RoleID = 0;
                    Menu.Type = request.Type;
                    Menu.Order = request.Order;
                    Menu.FunctionCode = request.FunctionCode;
                    Menu.CreateTime = now;
                    Menu.IsDelete = false;

                    service.Repository_Base_Menu.Insert(Menu);
                    service.SaveChange();

                    //拿ID
                    //var NewUser = service.Repository_Base_User.Query().OrderByDescending(a => a.ID).FirstOrDefault();

                    //if (!string.IsNullOrEmpty(request.Roles))
                    //{
                    //    List<Base_UserRole> list = new List<Base_UserRole>();
                    //    string[] Roles = request.Roles.Trim(',').Split(',');
                    //    for (int i = 0; i < Roles.Length; i++)
                    //    {
                    //        Base_UserRole role = new Base_UserRole();
                    //        role.UserID = NewUser.ID;
                    //        role.RoleID = Convert.ToInt32(Roles[i]);
                    //        role.IsDelete = false;
                    //        role.CreateTime = now;
                    //        list.Add(role);
                    //    }
                    //    service.Repository_Base_UserRole.AddList(list);
                    //    service.SaveChange();
                    //}
                    ret.Success = true;
                }


            }
            catch (Exception)
            {
                ret.Success = false;
            }

            return Json(ret, JsonRequestBehavior.DenyGet);
        }

    }
}