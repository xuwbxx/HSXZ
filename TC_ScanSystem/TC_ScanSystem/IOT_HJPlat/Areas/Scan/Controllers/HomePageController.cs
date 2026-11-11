using DataEntity;
using DataEntity.Services;
using Model.Common;
using Model.Echarts;
using Model.Enum;
using Model.HJPlat.Request;
using Model.HJPlat.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tool;
using ZJFramework10.Extension;

namespace IOT_HJPlat.Areas.Scan.Controllers
{
    public class HomePageController : Controller
    {
        DBDataContext service = DataContextFactory.Create();
        public ActionResult Index()
        {
            //Model.HJPlat.Response.ConstructCfgLayerResponse
            var LayerList = service.MK_Layer.Select(u => new ConstructCfgLayerResponse
            {
                ID = u.ID,
                MapXs = u.MapX.ToString() + "px",
                MapYs = u.MapY.ToString() + "px",
                LayerName = u.LayerName
            }).ToList();
            ViewData["LayerList"] = LayerList;
            return View();
        }

        public ActionResult Layer()
        {
           
            int LayerId = Request.QueryString["ID"].AsInt();
            //Model.HJPlat.Response.ConstructCfgLayerResponse
            var LayerList = service.MK_Layer.ToList();
            var PositionList = service.MK_Position.Where(u=>u.LayerId==LayerId).Select(u => new ConstructCfgPositionResponse
            {
                ID = u.ID,
                MapXs = u.MapX.ToString() + "%",
                MapYs = u.MapY.ToString() + "%",
                Widths = u.width.ToString() + "%",
                Heights = u.height.ToString() + "%",
                LayerId =u.LayerId,
                PositionName = u.PositionName
            }).ToList();
            ViewData["LayerList"] = LayerList;
            ViewData["PositionList"] = PositionList;
            if(LayerId>0)
            {
                var model = LayerList.FirstOrDefault(u=>u.ID==LayerId);
                if (model != null)
                {
                    ViewData["LayerImg"] = model.BackgroundPath;
                }
            }
            else if (LayerList.Count > 0)
            {
                ViewData["LayerImg"] = LayerList[0].BackgroundPath;
            }
            return View();
        }

        public ActionResult Position()
        {

            int PositionId = Request.QueryString["ID"].AsInt();
            int LayerId = Request.QueryString["LayerId"].AsInt();
            //Model.HJPlat.Response.ConstructCfgLayerResponse
            var PositionList = service.MK_Position.Where(u=>u.LayerId==LayerId).ToList();
            var PointList = service.View_MK_PointList.Where(u => u.PositionId==PositionId).Select(u => new ConstructCfgPointResponse
            {
                ID = u.ID,
                MapXs = u.MapX.ToString() + "%",
                MapYs = u.MapY.ToString() + "%",
                TypeName=u.TypeName,
                ImagePath = u.ImagePath,
                PointName = u.PointName
            }).ToList();
            DateTime begin = DateTime.Now.AddHours(-6);
            DateTime end = DateTime.Now;
            foreach (var point in PointList)
            {
                int count = 0;
                if (point.IsCommand == 3)
                {
                    point.Url = "/Scan/Construction/DynamicIndex?ID="+point.ID.ToString();
                    string DBName = "FD_DynamicValue2" + DateTime.Now.ToString("yyyyMM");
                    var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                    if (Exist.ID == 0)
                    {
                        count = 0;
                    }
                    else
                    {
                        string sql = string.Format("select * from {0} where PointId={1} and CreateDate>='{2}' and CreateDate<='{3}' ", DBName, point.ID, begin, end);
                        count = service.ExecuteQuery<FD_Dynamic>(sql).Count();
                    }
                }
                else
                {
                    point.Url = "/Scan/Construction/CalIndex?ID=" + point.ID.ToString();
                    count = service.MK_DataValue.Where(u => u.PointId == point.ID && u.CreateDate >= begin && u.CreateDate <= end).Count();

                }
                if (count > 0)
                {
                    point.ImagePath = point.ImagePath + "-4.png";
                }
                else
                {
                    point.ImagePath = point.ImagePath + "-5.png";
                }
                //var count = service.MK_DataValue.Where(u => u.PointId == point.ID).Count();
                //if (count > 0)
                //{
                //    point.ImagePath = point.ImagePath + "-4.png";
                //}
                //else
                //{
                //    point.ImagePath = point.ImagePath + "-5.png";
                //}
            }
            ViewData["PositionList"] = PositionList;
            ViewData["PointList"] = PointList;
            if (PositionId > 0)
            {
                var model = PositionList.FirstOrDefault(u => u.ID == PositionId);
                if (model != null)
                {
                    ViewData["PositionImg"] = model.BackGroundPath;            
                }
            }
            else if (PositionList.Count > 0)
            {
                ViewData["PositionImg"] = PositionList[0].BackGroundPath;
            }
            return View();
        }

        [HttpPost]
        public JsonResult PositionQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.LayerID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var PositionList = service.MK_Position.Where(a => a.LayerId == request.LayerID).Select(u=>new
                ConstructCfgPositionResponse
                {
                    ID = u.ID,
                    MapXs = u.MapX.ToString() + "%",
                    MapYs = u.MapY.ToString() + "%",
                    Widths=u.width.ToString()+"%",
                    Heights=u.height.ToString()+"%",
                    LayerId=u.LayerId,
                    PositionName = u.PositionName
                }).ToList();
                ret.Data = PositionList;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult LayerImgQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();

            if (request.LayerID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            try
            {
                var LayerInfo = service.MK_Layer.Where(a => a.ID == request.LayerID).FirstOrDefault();
                if (LayerInfo != null)
                {

                    ret.Data = LayerInfo;
                    ret.Success = true;
                }
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult PositionImgQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();

            if (request.PositionID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            try
            {
                var LayerInfo = service.MK_Position.Where(a => a.ID == request.PositionID).FirstOrDefault();
                if (LayerInfo != null)
                {

                    ret.Data = LayerInfo;
                    ret.Success = true;
                }
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult PointQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.PositionID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var PointList = service.View_MK_PointList.Where(a => a.PositionId == request.PositionID).Select(u => new
                ConstructCfgPositionResponse
                {
                    ID = u.ID,
                    MapXs = u.MapX.ToString() + "%",
                    MapYs = u.MapY.ToString() + "%",
                    ImagePath=u.ImagePath,
                    TypeName=u.TypeName,
                    IsCommand=u.IsCommand,
                    PositionName = u.PointName
                }).ToList();
                DateTime begin = DateTime.Now.AddHours(-6);
                DateTime end = DateTime.Now;
                foreach (var point in PointList)
                {

                    int count = 0;
                    if (point.IsCommand==3)
                    {
                        point.Url = "/Scan/Construction/DynamicIndex?ID=" + point.ID.ToString();
                        string DBName = "FD_DynamicValue2" + DateTime.Now.ToString("yyyyMM");
                        var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                        if (Exist.ID == 0)
                        {
                            count = 0;
                        }
                        else
                        {
                            string sql = string.Format("select * from {0} where PointId={1} and CreateDate>='{2}' and CreateDate<='{3}' ", DBName, point.ID, begin, end);
                            count = service.ExecuteQuery<FD_Dynamic>(sql).Count();
                        }
                    }
                    else
                    {
                        point.Url = "/Scan/Construction/CalIndex?ID=" + point.ID.ToString();
                        count = service.MK_DataValue.Where(u => u.PointId == point.ID && u.CreateDate >= begin && u.CreateDate <= end).Count();

                    }
                    if (count > 0)
                    {
                        point.ImagePath = point.ImagePath + "-4.png";
                    }
                    else
                    {
                        point.ImagePath = point.ImagePath + "-5.png";
                    }
                    //var count = service.MK_DataValue.Where(u => u.PointId == point.ID).Count();
                    //if (count > 0)
                    //{
                    //    point.ImagePath = point.ImagePath + "-4.png";
                    //}
                    //else
                    //{
                    //    point.ImagePath = point.ImagePath + "-5.png";
                    //}
                }
                ret.Data = PointList;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }
    }
}