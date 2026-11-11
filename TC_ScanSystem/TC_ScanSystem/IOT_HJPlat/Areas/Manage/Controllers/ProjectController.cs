using Business.Manage;
using DataEntity;
using DataEntity.Services;
using Model.Common;
using Model.Manage.Request;
using Model.Manage.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOT_HJPlat.Areas.Manage.Controllers
{
    public class ProjectController : Controller
    {
       DBDataContext service = DataContextFactory.Create();

        [FilterHelper]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DataQuery(ProjectRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                //组装视图
                var query = service.Base_Project.Where(u => !u.IsDelete.Value).AsQueryable();

                if (!string.IsNullOrEmpty(request.ProjectName))
                {
                    query = query.Where(a => a.ProjectName.Contains(request.ProjectName)).AsQueryable();
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
        public JsonResult ProjectQuery(ProjectRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.ID == 0)
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                var Project = service.Base_Project.Where(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                if (Project == null)
                {
                    ret.Success = false;
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                ProjectResponse se = new ProjectResponse();
                se.ID = Project.ID;
                se.ProjectName = Project.ProjectName;
                //se.ShortName = Project.ShortName;
                se.ProjectCode = Project.ProjectCode;
                
                MenuConfig ser = new MenuConfig();
                var Menus = ser.GetProjectMenuTree(request.ID);

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
        public JsonResult DataSave(ProjectRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.ID == 0)
                {
                    ret.Message = "出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                if (string.IsNullOrEmpty(request.ProjectName))
                {
                    ret.Message = "项目名称不能为空";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                DateTime now = DateTime.Now;

                var Project = service.Base_Project.Where(a => a.ID == request.ID).FirstOrDefault();
                if (Project == null)
                {
                    ret.Message = "出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                Project.ProjectName = request.ProjectName;

                var ProjectMenus = service.Base_ProjectMenu.Where(a => a.ProjectID == request.ID).ToList();
                service.Base_ProjectMenu.DeleteAllOnSubmit(ProjectMenus);

                if (request.MenuIDs != null && request.MenuIDs.Count > 0)
                {
                    foreach (int MenuID in request.MenuIDs)
                    {
                        Base_ProjectMenu se = new Base_ProjectMenu();
                        se.IsDelete = false;
                        se.MenuID = MenuID;
                        se.ProjectID = request.ID;
                        se.CreateTime = now;
                        service.Base_ProjectMenu.InsertOnSubmit(se);
                    }
                }

                service.SubmitChanges();

                ret.Success = true;


            }
            catch (Exception)
            {
                ret.Success = false;
            }

            return Json(ret, JsonRequestBehavior.DenyGet);
        }
    }
}