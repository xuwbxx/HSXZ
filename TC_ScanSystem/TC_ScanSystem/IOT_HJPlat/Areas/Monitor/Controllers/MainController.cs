using Business.Monitor;
using ExternalService.IOT;
using Model.Common;
using Model.Monitor.Request;
using Model.Monitor.Response;
using Model.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tool;
using ZJFramework10.Extension;

namespace IOT_HJPlat.Areas.Monitor.Controllers
{
    public class MainController : Controller
    {
        MonitorService service = new MonitorService();
        // GET: Monitor/Main
        public ActionResult Index()
        {
            var ret = service.GetMonitorType();
            //var Area = service.getMonitorArea(1);
            var MonitorTypes = ret.Data as List<Model.Monitor.Response.MonitorType>;

            var MonitorTypeIDRequest = HttpContext.Request.QueryString["MonitorTypeID"];
            if (!string.IsNullOrEmpty(MonitorTypeIDRequest))
            {
                ViewData["MonitorTypeID"] = MonitorTypeIDRequest;
            }
            else
            {
                ViewData["MonitorTypeID"] = MonitorTypes.FirstOrDefault() == null ? "" : MonitorTypes.FirstOrDefault().ID.ToString();
            }


            var layerlist = service.GetMonitorAlarm().Data as List<MonitorLayers>;
            ViewData["7PHC"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 21).Level;
            ViewData["47PHC"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 11).Level;
            ViewData["7ZL"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 27).Level;
            ViewData["47ZL"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 26).Level;
            ViewData["7HL"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 22).Level;
            ViewData["47HL"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 14).Level;
            ViewData["7PHC_Img"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 21).IconClass;
            ViewData["47PHC_Img"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 11).IconClass;
            ViewData["7ZL_Img"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 27).IconClass;
            ViewData["47ZL_Img"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 26).IconClass;
            ViewData["7HL_Img"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 22).IconClass;
            ViewData["47HL_Img"] = layerlist.FirstOrDefault(u => u.MonitorAreaID == 14).IconClass;

            return View(ret.Data);
            
        }

        [HttpPost]
        /// <summary>
        /// 获取天气
        /// </summary>
        /// <returns></returns>
        public JsonResult GetWeather()
        {
            WeatherResponse response = new WeatherResponse();
            try
            {
                string CityName = "横沙";
                string CacheKeyTemp = CacheKey.天气服务数据缓存 + "-" + CityName;
                var WeatherCache = CacheUtils.Get(CacheKeyTemp);
                if (WeatherCache == null)
                {
                    WeatherService weatherService = new WeatherService();
                    response = weatherService.GetWeatherInfo(CityName);
                    CacheUtils.Set(CacheKey.天气服务数据缓存, response, 120);
                }
                else
                {
                    response = WeatherCache as WeatherResponse;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(response, JsonRequestBehavior.DenyGet);
            
        }

        [HttpPost]
        public JsonResult getMonitorType()
        {
            var ret = service.GetMonitorType();
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult getMonitorArea(MonitorAreaRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request == null || request.MonitorTypeID == null || request.MonitorTypeID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            ret = service.getMonitorArea(request.MonitorTypeID);
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult getMonitorLayers(Model.Monitor.Request.MonitorAreaRequest request)
        {
            BaseReturn ret = new BaseReturn();
            
            ret = service.GetMonitorLayers(request.MonitorTypeID);
            return Json(ret, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult getMonitorAreaAlarm()
        {
            BaseReturn ret = new BaseReturn();

            ret = service.getMonitorArea();
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 监测详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult MonitorDetail()
        {
            var MonitorAreaID = HttpContext.Request.QueryString["MonitorAreaID"];
            //var MonitorAreaID = HttpContext.Request.QueryString["MonitorAreaID"];
            //if (string.IsNullOrEmpty(MonitorAreaName))
            //{
            //    return RedirectToAction("MonitorError");
            //}

            ////var types = service.GetMonitorType(MonitorAreaName);
           //var areaTypes = service.GetMonitorTypes(MonitorAreaName);

            ////var MonitorAreaID = FOr types.Data

            ////获取此检测区域信息
            ////var Detail = service.getMonitorAreaDetail(Convert.ToInt32(MonitorAreaID));

            //int MonitorAreaID = 0;

            //if (areaTypes.Count > 0)
            //{
            //    MonitorAreaID = areaTypes.FirstOrDefault().ID;
            //}
            //else
            //{
            //    return View();
            //}
            if (string.IsNullOrEmpty(MonitorAreaID))
            {
                return RedirectToAction("MonitorError");
            }

            //获取此检测区域信息
            var Detail = service.getMonitorAreaDetail(Convert.ToInt32(MonitorAreaID));
            if (Detail == null)
            {
                return RedirectToAction("MonitorError");
            }
            
          //  var areaTypes = service.GetMonitorType().Data as ;
            //ViewData["MonitorTypes"] = areaTypes;
            ViewData["MonitorAreaName"] = Detail.MonitorAreaName;

            //var Detail = service.getMonitorAreaDetail(MonitorAreaID);

            //var Detail = service.getMonitorAreaDetail(MonitorAreaName);

            //if (Detail == null)
            //{
            //    return RedirectToAction("MonitorError");
            //}

            ViewData["MonitorTypeID"] = Detail.MonitorTypeID.AsStr();
            ViewData["BackGroundPic"] = Detail.BackGroundPic.AsStr();
            ViewData["MonitorAreaPic"] = Detail.MonitorAreaPic.AsStr();
            //ViewData["MonitorAreaID"] = Detail.MonitorTypeID;
            ViewData["MonitorAreaID"] = Detail.ID;
            ViewData["MonitorTypeName"] = Detail.MonitorTypeName;


           // 监测类型
            var MonitorTypes = service.GetMonitorType();
            ViewData["MonitorTypes"] = MonitorTypes.Data;
            
            //ViewData["MonitorTypes"] = areaTypes;

            var ret = service.getMonitorArea(Detail.MonitorTypeID);

            //return View(ret.Data);
            return View(ret.Data);
            //return View();

        }


        /// <summary>
        /// 监测详情页
        /// </summary>
        /// <returns></returns>
        //public ActionResult MonitorDetail()
        //{
        //    var MonitorAreaID = HttpContext.Request.QueryString["MonitorAreaID"];
        //    if (string.IsNullOrEmpty(MonitorAreaID))
        //    {
        //        return RedirectToAction("MonitorError");
        //    }

        //    //获取此检测区域信息
        //    var Detail = service.getMonitorAreaDetail(Convert.ToInt32(MonitorAreaID));
        //    if (Detail == null)
        //    {
        //        return RedirectToAction("MonitorError");
        //    }

        //    ViewData["BackGroundPic"] = Detail.BackGroundPic;
        //    ViewData["MonitorAreaPic"] = Detail.MonitorAreaPic;
        //    ViewData["MonitorAreaID"] = MonitorAreaID;
        //    ViewData["MonitorTypeName"] = Detail.MonitorTypeName;

        //    //监测类型
        //    var MonitorTypes = service.GetMonitorType();
        //    ViewData["MonitorTypes"] = MonitorTypes.Data;

        //    var ret = service.getMonitorArea(Detail.MonitorTypeID);

        //    return View(ret.Data);
        //}

        public ActionResult MonitorError()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getMonitorPosition(MonitorPositionRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request == null || request.MonitorAreaID == null || request.MonitorAreaID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            if(request.EndTime==null)
            {
                request.EndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            else
            request.EndTime = request.EndTime.Value.AddDays(1);
            ret = service.getMonitorPosition(request.MonitorAreaID, request.BeginTime, request.EndTime);
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        //[HttpPost]
        //public JsonResult getMonitorPosition(MonitorPositionRequest request)
        //{
        //    BaseReturn ret = new BaseReturn();
        //    if (request == null || request.AreaName == null || request.ScanTypeID == 0)
        //    {
        //        return Json(ret, JsonRequestBehavior.DenyGet);
        //    }

        //    //ret = service.getMonitorPosition(request.MonitorAreaID);
        //    ret = service.getMonitorPosition(request.AreaName, request.ScanTypeID);
        //    return Json(ret, JsonRequestBehavior.DenyGet);
        //}

        [HttpPost]
        public JsonResult getMonitorPoint(MonitorPointRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request == null || request.MonitorPositionID == null || request.MonitorPositionID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            ret = service.getMonitorPoint(request.MonitorPositionID);
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult getMonitorPointDetail(MonitorPointRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request == null)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            ret = service.getMonitorPointInfo(request.MonitorPointID,request.MonitorPositionID);
            return Json(ret, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult getMonitorPointData(MonitorPointRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request == null || request.MonitorPointID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            //request.EndTime = request.EndTime.Value.AddDays(1);
            ret = service.getMonitorPointData(request);


            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 监测总览图（新版，设计稿 design/index.html）
        /// </summary>
        /// <returns></returns>
        public ActionResult NewIndex()
        {
            return View();
        }
    }
}
