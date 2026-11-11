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
using Model.Manage.Request;
using Model.Manage.Response;

namespace IOT_HJPlat.Areas.Manage.Controllers
{
    public class RoleController:Controller
    {
        DBDataContext service =DataContextFactory.Create();

        [FilterHelper]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult DataQuery(RoleRequest request)
        {

            BaseReturn ret = new BaseReturn();
            try
            {
                //组装视图
                // var query = service.Repository_View_BaseUser.Query().AsQueryable();
                var query = service.Base_Role.Where(u => !u.IsDelete.Value).AsQueryable();

                //筛选
                if (!string.IsNullOrEmpty(request.RoleName))
                {
                    query = query.Where(a => a.RoleName.Contains(request.RoleName)).AsQueryable();
                }
                if (!string.IsNullOrEmpty(request.RoleCode))
                {
                    query = query.Where(a => a.RoleCode.Contains(request.RoleCode)).AsQueryable();
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
        public JsonResult DataDelete(RoleRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.ID == 0)
                {
                    ret.Success = false;
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                var Role = service.Base_Role.Where(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                if (Role == null)
                {
                    ret.Success = false;
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                Role.IsDelete = true;
                Role.CreateTime = DateTime.Now;
                service.SubmitChanges();

                ret.Success = true;

            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult RoleQuery(RoleRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.ID == 0)
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                var Role = service.Base_Role.Where(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                if (Role == null)
                {
                    ret.Success = false;
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                RoleResponse se = new RoleResponse();
                se.ID = Role.ID;
                se.RoleName = Role.RoleName;
                se.RoleCode = Role.RoleCode;

                MenuConfig ser = new MenuConfig();
                var Menus = ser.GetRoleMenuTree(request.ID);

                se.trees = Menus;


                ret.Data = se;
                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult MenuQuery(RoleRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {

                RoleResponse se = new RoleResponse();

                MenuConfig ser = new MenuConfig();
                var Menus = ser.GetAllMenuTree();

                se.trees = Menus;


                ret.Data = se;
                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DataSave(RoleRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (string.IsNullOrEmpty(request.Type))
                {
                    ret.Message = "出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                if (string.IsNullOrEmpty(request.RoleName))
                {
                    ret.Message = "角色名称不能为空";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                DateTime now = DateTime.Now;
                if (request.Type.Equals("Modify"))
                {
                    if (request.ID == 0)
                    {
                        ret.Message = "出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                    var Role = service.Base_Role.Where(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                    if (Role == null)
                    {
                        ret.Message = "出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                    //修改用户表
                    Role.RoleName = request.RoleName;
                    Role.RoleCode = request.RoleCode;
                    Role.CreateTime = now;

                    //修改角色菜单
                    var RoleMenus = service.Base_RoleMenu.Where(a => !a.IsDelete.Value && a.RoleID == request.ID).ToList();
                    service.Base_RoleMenu.DeleteAllOnSubmit(RoleMenus);

                    if (request.MenuIDs != null && request.MenuIDs.Count > 0)
                    {
                        foreach (int MenuID in request.MenuIDs)
                        {
                            Base_RoleMenu se = new Base_RoleMenu();
                            se.IsDelete = false;
                            se.MenuID = MenuID;
                            se.RoleID = request.ID;
                            se.CreateTime = now;
                            service.Base_RoleMenu.InsertOnSubmit(se);
                        }
                    }
                    
                    service.SubmitChanges();
                    ret.Success = true;
                }
                else if (request.Type.Equals("Add"))
                {
                    Base_Role Role = new Base_Role();
                    Role.RoleName = request.RoleName;
                    Role.RoleCode = request.RoleCode;
                    Role.CreateTime = now;
                    Role.IsDelete = false;
                    service.Base_Role.InsertOnSubmit(Role);
                    service.SubmitChanges();

                    //获取新增的RoleID
                    int NewRoleID = service.Base_Role.Where(a => !a.IsDelete.Value).OrderByDescending(a => a.ID).FirstOrDefault().ID;

                    //修改角色菜单
                    if (request.MenuIDs != null && request.MenuIDs.Count > 0)
                    {
                        foreach (int MenuID in request.MenuIDs)
                        {
                            Base_RoleMenu se = new Base_RoleMenu();
                            se.IsDelete = false;
                            se.MenuID = MenuID;
                            se.RoleID = NewRoleID;
                            se.CreateTime = now;
                            service.Base_RoleMenu.InsertOnSubmit(se);
                        }
                    }
                    
                    service.SubmitChanges();
                    
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