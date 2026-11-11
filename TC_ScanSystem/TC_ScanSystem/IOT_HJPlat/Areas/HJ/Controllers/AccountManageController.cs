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
    public class AccountManageController : Controller
    {
        WorkService service = new WorkService();

        [FilterHelper]
        public ActionResult Index()
        {
            ViewData["Departs"] = service.Repository_Base_Depart.Query(a => !a.IsDelete.Value).ToList();
            ViewData["Positions"] = service.Repository_Base_Position.Query(a => !a.IsDelete.Value).ToList();
            ViewData["Structors"] = service.Repository_Base_Structor.Query(a => !a.IsDelete).ToList();
            ViewData["Roles"] = service.Repository_Base_Role.Query(a => !a.IsDelete.Value).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult DataQuery(AccountRequest request)
        {

            BaseReturn ret = new BaseReturn();
            try
            {
                //组装视图
                var query = service.Repository_View_BaseUser.Query().AsQueryable();

                //筛选
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    query = query.Where(a => a.UserName.Equals(request.UserName)).AsQueryable();
                }
                if (!string.IsNullOrEmpty(request.RealName))
                {
                    query = query.Where(a => a.RealName.Equals(request.RealName)).AsQueryable();
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
        public JsonResult DataDelete(AccountRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.ID == 0)
                {
                    ret.Success = false;
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                var User = service.Repository_Base_User.Query(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                if (User == null)
                {
                    ret.Success = false;
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                User.IsDelete = true;
                User.CreateTime = DateTime.Now;
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
        public JsonResult AccountQuery(AccountRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.ID == 0)
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                var User = service.Repository_Base_User.Query(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                if (User == null)
                {
                    ret.Success = false;
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                AccountResponse se = new AccountResponse();
                se.ID = User.ID;
                se.DepartID = User.DepartID;
                se.Password = User.Password;
                se.PositionID = User.PositionID;
                se.StructorID = User.StructorId;
                se.UserName = User.UserName;
                se.RealName = User.RealName;
                se.Mobile = User.Mobile;

                string Roles = "";
                service.Repository_Base_UserRole.Query(a => !a.IsDelete.Value && a.UserID == User.ID).ToList().ForEach(a =>
                {
                    Roles += a.RoleID + ",";
                });
                se.Roles = Roles.Trim(',');

                ret.Data = se;
                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        public JsonResult AccountSave(AccountRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (string.IsNullOrEmpty(request.Type))
                {
                    ret.Message = "出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.RealName))
                {
                    ret.Message = "用户名、密码、姓名不能为空";
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
                    var User = service.Repository_Base_User.Query(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                    if (User == null)
                    {
                        ret.Message = "出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                    //修改用户表
                    User.Password = request.Password;
                    User.RealName = request.RealName;
                    User.Mobile = request.Mobile;
                    User.DepartID = request.DepartID;
                    User.PositionID = request.PositionID;
                    User.RoleID = 0;
                    User.StructorId = request.StructorID;
                    User.UserName = request.UserName;

                    //修改角色表
                    //先删除
                    var oldRoles = service.Repository_Base_UserRole.Query(a => !a.IsDelete.Value && a.UserID == request.ID).ToList();
                    service.Repository_Base_UserRole.DeleteList(oldRoles);
                    //再添加
                    if (!string.IsNullOrEmpty(request.Roles))
                    {
                        List<Base_UserRole> list = new List<Base_UserRole>();
                        string[] Roles = request.Roles.Trim(',').Split(',');
                        for (int i = 0; i < Roles.Length; i++)
                        {
                            Base_UserRole role = new Base_UserRole();
                            role.UserID = User.ID;
                            role.RoleID = Convert.ToInt32(Roles[i]);
                            role.IsDelete = false;
                            role.CreateTime = now;
                            list.Add(role);
                        }
                        service.Repository_Base_UserRole.AddList(list);
                    }
                    service.SaveChange();
                    ret.Success = true;
                }
                else if (request.Type.Equals("Add"))
                {
                    Base_User se = new Base_User();
                    se.DepartID = request.DepartID;
                    se.Mobile = request.Mobile;
                    se.Password = request.Password;
                    se.UserName = request.UserName;
                    se.PositionID = request.PositionID;
                    se.RealName = request.RealName;
                    se.RoleID = 0;
                    se.StructorId = request.StructorID;
                    se.UserName = request.UserName;
                    se.CreateTime = now;
                    se.IsDelete = false;

                    service.Repository_Base_User.Insert(se);
                    service.SaveChange();

                    //拿ID
                    var NewUser = service.Repository_Base_User.Query().OrderByDescending(a => a.ID).FirstOrDefault();

                    if (!string.IsNullOrEmpty(request.Roles))
                    {
                        List<Base_UserRole> list = new List<Base_UserRole>();
                        string[] Roles = request.Roles.Trim(',').Split(',');
                        for (int i = 0; i < Roles.Length; i++)
                        {
                            Base_UserRole role = new Base_UserRole();
                            role.UserID = NewUser.ID;
                            role.RoleID = Convert.ToInt32(Roles[i]);
                            role.IsDelete = false;
                            role.CreateTime = now;
                            list.Add(role);
                        }
                        service.Repository_Base_UserRole.AddList(list);
                        service.SaveChange();
                    }
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