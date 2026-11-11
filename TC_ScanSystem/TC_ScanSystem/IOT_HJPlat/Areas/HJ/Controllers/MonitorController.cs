using Business.Video;
using ExternalService.Video;
using Model.Common;
using Model.HJPlat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tool;

namespace IOT_HJPlat.Areas.HJ.Controllers
{
    public class MonitorController : Controller
    {
        // GET: Video/Monitor
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult VideoQuery()
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                VideoBusiness video = new VideoBusiness();

                List<TreeView> Videos = new List<TreeView>();
                var VideoCache = CacheUtils.Get(CacheKey.视频监控列表缓存);
                if (VideoCache == null)
                {
                    Videos = video.VideoTree();
                    CacheUtils.Set(CacheKey.视频监控列表缓存, Videos, 60);
                }
                else
                {
                    Videos = VideoCache as List<TreeView>;
                }

                ret.Data = Videos;


                ret.Success = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public string VideoTokenGet(VideoRequest collects)
        {
            string token = string.Empty;
            try
            {
                //NameValueCollection collects = HttpContext.Request.Params;

                string cameraId = collects.ca ?? "";
                string index_code = collects.code ?? "";

                string UserName = VideoConstant.HikServerUser;
                string Pwd = VideoConstant.HikServerPwd;
                string ip = VideoConstant.HikServerIP;
                string port = VideoConstant.HikServerPort;
                string web = VideoConstant.WebServerIP;

                HikServiceHelper service = new HikServiceHelper(ip, port, UserName, Pwd);
                token = service.GetToken();
                if (cameraId.Equals("1"))
                {

                }
                else
                {
                    token = service.GetPreviewOcxOptions(index_code, token, web);
                }

            }
            catch (Exception ex)
            {
                //Logger.Log(ex.ToString());
                token = "";
            }

            return token;
        }
    }
}