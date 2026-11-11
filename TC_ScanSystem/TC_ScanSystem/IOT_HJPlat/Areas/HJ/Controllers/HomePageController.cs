using Business.Video;
using DataEntity.DataContext;
using DataEntity.Services;
using IOT_HJPlat.App_Start;
using Model.Common;
using Model.HJPlat.Request;
using Model.HJPlat.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tool;

namespace IOT_HJPlat.Areas.HJ.Controllers
{
    public class HomePageController : Controller
    {

        WorkService service = new WorkService();

        #region new
        [FilterHelper]
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult UserInfo()
        {
            var User = AppUser.Current;
            return PartialView(User);
        }

        public PartialViewResult Remind()
        {
            return PartialView();
        }

        public PartialViewResult Monitor()
        {

            //只取前9个
            return PartialView();
        }

        [HttpPost]
        public JsonResult VideoQuery()
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                VideoBusiness video = new VideoBusiness();

                List<CameraTreeView> Videos = new List<CameraTreeView>();
                var VideoCache = CacheUtils.Get(CacheKey.首页视频缓存);
                if (VideoCache == null)
                {
                    Videos = video.VideoInit();
                    CacheUtils.Set(CacheKey.首页视频缓存, Videos, 60);
                }
                else
                {
                    Videos = VideoCache as List<CameraTreeView>;
                }

                //CameraTreeView m = new CameraTreeView();
                //m.IndexCode = "12345";
                //m.NodeValue = "12";
                //Videos.Add(m);

                ret.Data = Videos.OrderByDescending(a => a.NodeValue).Take(9).ToList();

                ret.Success = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        public PartialViewResult Weather()
        {
            return PartialView();
        }

        public PartialViewResult Environment()
        {
            var data = service.Repository_View_EnvironmentData.Query().OrderByDescending(a => a.CreateTime).Take(5).ToList();

            return PartialView();
        }

        public PartialViewResult Map()
        {

            return PartialView();
        }

        [HttpPost]
        public JsonResult EnvironmentQuery(EnvironmentRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                List<View_EnvironmentData> list = new List<View_EnvironmentData>();
                var EnvironmentCache = CacheUtils.Get(CacheKey.首页环境监测缓存);
                if (EnvironmentCache == null)
                {
                    list = service.Repository_View_EnvironmentData.Query().OrderByDescending(a => a.CreateTime).Take(5).ToList();
                    CacheUtils.Set(CacheKey.首页环境监测缓存, list, 30);
                }
                else
                {
                    list = EnvironmentCache as List<View_EnvironmentData>;
                }
                ret.Data = list;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
                //log
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        public PartialViewResult Download()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult DownloadList()
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                List<DownloadResponse> list = new List<DownloadResponse>();
                var DownloadCache = CacheUtils.Get(CacheKey.首页下载文件缓存);
                if (DownloadCache == null)
                {
                    service.Repository_DownloadCenter.Query(a => !a.IsDelete.Value).Take(5).ToList()
                        .ForEach(a =>
                        {
                            var se = new DownloadResponse();
                            se.ID = a.ID;
                            se.FileName = a.FileName;
                            se.CreateTime = a.CreateTime == null ? "" : Convert.ToDateTime(a.CreateTime).ToShortDateString();
                            list.Add(se);
                        });
                    CacheUtils.Set(CacheKey.首页下载文件缓存, list, 120);
                }
                else
                {
                    list = DownloadCache as List<DownloadResponse>;
                }
                ret.Data = list;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
                //log
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        public FileResult FileDownload()
        {
            try
            {
                var FileID = HttpContext.Request["FileID"];
                if (string.IsNullOrEmpty(FileID))
                    return null;
                int id = Convert.ToInt32(FileID);
                var FileInfo = service.Repository_DownloadCenter.Query(a => !a.IsDelete.Value && a.ID == id).FirstOrDefault();
                if (FileInfo == null)
                    return null;

                string filename = FileInfo.FileName;
                string filepath = AppDomain.CurrentDomain.BaseDirectory + FileInfo.FilePath;

                LogUtils.Instance.LogText("FileDownload------>" + filepath);

                return File(new FileStream(filepath, FileMode.Open), "application/octet-stream", Server.UrlEncode(filename));
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogText("FileDownload------>" + ex.ToString());
                return null;
            }

        }


        public ActionResult DownloadCenter()
        {
            return View();
        }
        #endregion



    }
}