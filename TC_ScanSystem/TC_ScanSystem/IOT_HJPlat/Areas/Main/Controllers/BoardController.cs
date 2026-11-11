//using Business.Board;
//using Business.Video;
using DataEntity.Services;
using ExternalService.IOT;
using IOT_HJPlat.App_Start;
using Model.Board.Request;
using Model.Common;
using Model.HJPlat.Response;
using Model.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tool;

namespace IOT_HJPlat.Areas.Main.Controllers
{
    public class BoardController : Controller
    {
        //[FilterHelper]
        //public ActionResult Index()
        //{
        //    //var mm = AppUser.Current.RealName;
        //    return View("Index2");
        //}

        //public ActionResult Index2()
        //{
        //    return View();
        //}

        //public PartialViewResult Nav()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Target()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Alarm()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Head()
        //{
        //    WeatherResponse response = new WeatherResponse();

        //    var WeatherCache = CacheUtils.Get(CacheKey.天气服务数据缓存);
        //    if (WeatherCache == null)
        //    {
        //        WeatherService weatherService = new WeatherService();
        //        response = weatherService.GetWeatherInfo();
        //        CacheUtils.Set(CacheKey.天气服务数据缓存, response, 120);
        //    }
        //    else
        //    {
        //        response = WeatherCache as WeatherResponse;
        //    }

        //    return PartialView(response);
        //}

        //public PartialViewResult Cumulative()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Cumulative2()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Map()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Map2()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Prewarning()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Prewarning2()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Speed()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Monitor()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Environment()
        //{
        //    //区域
        //    //var area = service.Repository_SQCP_CSMonitor_EnvSpot.Query(a => !a.IsDelete.Value).ToList();


        //    return PartialView();
        //}

        //public PartialViewResult EnvironmentGauge()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Equipment()
        //{
        //    return PartialView();
        //}
        //public PartialViewResult Equipment2()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult Position()
        //{
        //    return PartialView();
        //}        

        //public PartialViewResult Resource()
        //{
        //    return PartialView();
        //}

        //public PartialViewResult People()
        //{
        //    return PartialView();
        //}

        //public ActionResult Test()
        //{
        //    return View();
        //}

        //public ActionResult Earth()
        //{
        //    return View();
        //}

        //public ActionResult Earth3D()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public JsonResult GetEnvirMonitorData()
        //{
        //    BaseReturn ret = new BaseReturn();

        //    try
        //    {
        //        BoardBusiness biz = new BoardBusiness();

        //        var response = biz.GetEnvironmentParam();
        //        ret.Data = response;
        //        ret.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtils.Instance.LogText(ex.Message);
        //        ret.Success = false;
        //    }

        //    return Json(ret, JsonRequestBehavior.DenyGet);
        //}

        //[HttpPost]
        //public JsonResult VideoQuery()
        //{
        //    BaseReturn ret = new BaseReturn();
        //    try
        //    {
        //        VideoBusiness videoBiz = new VideoBusiness();

        //        List<TreeView> videos = new List<TreeView>();
        //        List<TreeView> vidTree = new List<TreeView>();

        //        var VideoCache = CacheUtils.Get(CacheKey.视频监控列表缓存);
        //        if (VideoCache == null)
        //        {
        //            vidTree = videoBiz.VideoTree();
        //            CacheUtils.Set(CacheKey.视频监控列表缓存, vidTree, 60);
        //        }
        //        else
        //        {
        //            vidTree = VideoCache as List<TreeView>;
        //        }

        //        videos = videoBiz.GetAllNodes(vidTree);
        //        ret.Data = videos;

        //        ret.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtils.Instance.LogText(ex.Message);
        //        ret.Success = false;
        //    }

        //    return Json(ret, JsonRequestBehavior.DenyGet);
        //}

        //[HttpPost]
        //public JsonResult GetTarget()
        //{
        //    BaseReturn ret = new BaseReturn();
        //    try
        //    {
        //        BoardBusiness biz = new BoardBusiness();
        //        var response = biz.GetTargetData();

        //        ret.Data = response;
        //        ret.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtils.Instance.LogText(ex.Message);
        //        ret.Success = false;
        //    }

        //    return Json(ret, JsonRequestBehavior.DenyGet);
        //}

        //[HttpPost]
        //public JsonResult GetSummary()
        //{
        //    BaseReturn ret = new BaseReturn();
        //    try
        //    {
        //        BoardBusiness biz = new BoardBusiness();
        //        var response = biz.GetSummaryData();

        //        ret.Data = response;
        //        ret.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtils.Instance.LogText(ex.Message);
        //        ret.Success = false;
        //    }

        //    return Json(ret, JsonRequestBehavior.DenyGet);
        //}

        //[HttpPost]
        //public JsonResult GetProgress()
        //{
        //    BaseReturn ret = new BaseReturn();
        //    try
        //    {
        //        BoardBusiness biz = new BoardBusiness();
        //        var response = biz.GetProgressData();

        //        ret.Data = response;
        //        ret.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtils.Instance.LogText(ex.Message);
        //        ret.Success = false;
        //    }

        //    return Json(ret, JsonRequestBehavior.DenyGet);
        //}

        //public PartialViewResult PersonPosition()
        //{
        //    List<Base_Cave_CoordinateResponse> list = new List<Base_Cave_CoordinateResponse>();

        //    WorkService service = new WorkService();
        //    var Coordinates = service.Repository_Base_Cave_Coordinate.Query().OrderBy(a => a.OrderBy).ToList();

        //    service.Repository_Base_Cave.Query(a => !a.IsDelete && a.IsDisplay.Value).ToList().ForEach(a =>
        //    {
        //        Base_Cave_CoordinateResponse se = new Base_Cave_CoordinateResponse();
        //        se.ID = a.ID;
        //        se.CaveName = a.CaveName;
        //        se.CenterLon = a.CenterBD09Lon;
        //        se.CenterLat = a.CenterBD09Lat;
        //        foreach (var item in Coordinates.Where(b => b.CaveId == a.ID).OrderBy(b => b.OrderBy))
        //        {
        //            Cave_Coordinate cood = new Cave_Coordinate();
        //            cood.ID = item.ID;
        //            cood.Lon = item.BD09Lon;
        //            cood.Lat = item.BD09Lat;
        //            cood.PointName = item.PointName;
        //            cood.Order = item.OrderBy;
        //            se.Coordinates.Add(cood);
        //        }
        //        list.Add(se);
        //    });

        //    ViewData["WorkCaves"] = list.Serialize();
        //    return PartialView();
        //}

    }
}