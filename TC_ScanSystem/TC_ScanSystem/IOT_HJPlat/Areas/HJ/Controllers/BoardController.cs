using Business.Board;
using ExternalService.IOT;
using Model.Common;
using Model.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tool;

namespace IOT_HJPlat.Areas.HJ.Controllers
{
    public class BoardController : Controller
    {
        [FilterHelper]
        public ActionResult Index()
        {
            //var mm = AppUser.Current.RealName;
            return View();
        }

        public PartialViewResult Nav()
        {
            return PartialView();
        }

        public PartialViewResult Target()
        {
            return PartialView();
        }

        public PartialViewResult Alarm()
        {
            return PartialView();
        }

        public PartialViewResult Head()
        {
            WeatherResponse response = new WeatherResponse();

            var WeatherCache = CacheUtils.Get(CacheKey.天气服务数据缓存);
            if (WeatherCache == null)
            {
                WeatherService weatherService = new WeatherService();
                response = weatherService.GetWeatherInfo();
                CacheUtils.Set(CacheKey.天气服务数据缓存, response, 120);
            }
            else
            {
                response = WeatherCache as WeatherResponse;
            }

            return PartialView(response);
        }

        public PartialViewResult Cumulative()
        {
            return PartialView();
        }

        public PartialViewResult Map()
        {
            return PartialView();
        }

        public PartialViewResult Map2()
        {
            return PartialView();
        }

        public PartialViewResult Prewarning()
        {
            return PartialView();
        }

        public PartialViewResult Speed()
        {
            return PartialView();
        }

        public PartialViewResult Monitor()
        {
            return PartialView();
        }

        public PartialViewResult Environment()
        {
            //区域
            //var area = service.Repository_SQCP_CSMonitor_EnvSpot.Query(a => !a.IsDelete.Value).ToList();


            return PartialView();
        }

        public PartialViewResult Equipment()
        {
            return PartialView();
        }

        public PartialViewResult Resource()
        {
            return PartialView();
        }

        public PartialViewResult People()
        {
            return PartialView();
        }

        public ActionResult Test()
        {
            return View();
        }

        public JsonResult GetEnvirMonitorData()
        {
            BaseReturn ret = new BaseReturn();

            try
            {
                BoardBusiness biz = new BoardBusiness();

                var response = biz.GetEnvironmentParam();
                ret.Data = response;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogText(ex.Message);
                ret.Success = false;
            }

            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        public ActionResult Earth()
        {
            return View();
        }

        public ActionResult Earth3D()
        {
            return View();
        }
    }
}