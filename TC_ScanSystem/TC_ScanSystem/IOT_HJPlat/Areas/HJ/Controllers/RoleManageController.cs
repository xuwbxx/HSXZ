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
    public class RoleManageController : Controller
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
        public JsonResult DataQuery(RoleRequest request)
        {

            BaseReturn ret = new BaseReturn();
            try
            {
                //组装视图
                // var query = service.Repository_View_BaseUser.Query().AsQueryable();
                var query = service.Repository_Base_Role.Query().Where(u => u.IsDelete == false);

                //筛选
                if (!string.IsNullOrEmpty(request.RoleName))
                {
                    query = query.Where(a => a.RoleName.Contains(request.RoleName));
                }
                if (!string.IsNullOrEmpty(request.RoleCode))
                {
                    query = query.Where(a => a.RoleCode.Contains(request.RoleCode));
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

                var Menu = service.Repository_Base_Role.Query(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
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
        public JsonResult RoleQuery(RoleRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.ID == 0)
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                var Role = service.Repository_Base_Role.Query(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                if (Role == null)
                {
                    ret.Success = false;
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                RoleResponse se = new RoleResponse();
                se.ID = Role.ID;
                se.RoleName = Role.RoleName;
                se.RoleCode = Role.RoleCode;
                se.IsDelete = Role.IsDelete;
                se.CreateTime = Role.CreateTime;
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

        public JsonResult RoleSave(RoleRequest request)
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
                    ret.Message = "菜单名称不能为空";
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
                    var Role = service.Repository_Base_Role.Query(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                    if (Role == null)
                    {
                        ret.Message = "出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                    //修改用户表
                    Role.RoleName = request.RoleName;
                    Role.RoleCode = request.RoleCode;

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
                else if (request.Type.Equals("Add"))
                {
                    Base_Role Role = new Base_Role();
                    Role.RoleName = request.RoleName;
                    Role.RoleCode = request.RoleCode;
                    Role.CreateTime = now;
                    Role.IsDelete = false;

                    service.Repository_Base_Role.Insert(Role);
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