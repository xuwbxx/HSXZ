using DataEntity;
using DataEntity.Services;
using Model.Common;
using Model.Echarts;
using Model.HJPlat.Request;
using Model.HJPlat.Response;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ZJFramework10.Extension;
using Tool;
using System.Configuration;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Diagnostics;

namespace IOT_HJPlat.Areas.Scan.Controllers
{
    public class ConstructionController : Controller
    {
        DBDataContext service = DataContextFactory.Create();
        private static string filename = "";
        private static string filename2 = "";
        [FilterHelper]
        // GET: Scan/Construction
        public ActionResult Index()
        {
            ViewData["TypeId"] = "";
            ViewData["PositionId"] = "";
            ViewData["LayerId"] = "";
            int TypeId = Request.QueryString["TypeId"].AsInt(0);
            int PositionId = Request.QueryString["PositionId"].AsInt(0);
            int LayerId = Request.QueryString["LayerId"].AsInt(0);
            var LayerList = service.MK_Layer.ToList();
            var ScanTypeList = service.MK_ScanType.Where(u=>u.IsCommand==1).ToList();
            var pointId = Request.QueryString["ID"];
            int p = 0;
            if(!string.IsNullOrEmpty(pointId))
            {
                var pp = pointId.Split(',');
                if(pp.Length>0)
                {
                    p = pp[0].AsInt();
                }
            }
            var point = service.MK_Point.FirstOrDefault(u => u.ID == p);
            if (point != null)
            {
                TypeId = point.ScanTypeId ?? 0;
                PositionId = point.PositionId ?? 0;
                ViewData["TypeId"] = TypeId;
                ViewData["PositionId"] = PositionId;
                var position = service.MK_Position.FirstOrDefault(u => u.ID == PositionId);
                if (position != null)
                {
                    LayerId = position.LayerId ?? 0;
                    ViewData["LayerId"] = LayerId;
                }

                var scantype = service.MK_ScanType.FirstOrDefault(u => u.ID == TypeId);
                if (scantype != null)
                {
                    if (scantype.IsCommand == 3)
                    {
                        Response.Redirect("/Scan/Construction/DynamicIndex?ID=" + point.ID.ToString());
                    }
                }

            }
            else
            {
                ViewData["TypeId"] = TypeId;
                ViewData["PositionId"] = PositionId;
                ViewData["LayerId"] = LayerId;
                var scantype = service.MK_ScanType.FirstOrDefault(u => u.ID == TypeId);
                if (scantype != null)
                {
                    if (scantype.IsCommand == 3)
                    {
                        Response.Redirect(string.Format("/Scan/Construction/DynamicIndex?TypeId={0}&LayerId={1}&PositionId={2}",TypeId,LayerId,PositionId));
                    }
                }

            }
            ViewData["LayerList"] = LayerList;
            ViewData["ScanTypeList"] = ScanTypeList;
            var PositionList = service.MK_Position.Where(u => u.LayerId == LayerId).ToList();
            var PointList = service.View_MK_Point.Where(u => u.PositionId == PositionId && u.DeviceProtocol == 0).ToList();
            ViewData["PositionList"] = PositionList;
            ViewData["PointList"] = PointList;
            ViewData["Point"] = Request.QueryString["ID"].AsStr("");
            return View();
        }


        [FilterHelper]
        // GET: Scan/Construction
        public ActionResult CalIndex()
        {
           // Response.Redirect("/Scan/Construction/DynamicIndex");
            ViewData["TypeId"] = "";
            ViewData["PositionId"] = "";
            ViewData["LayerId"] = "";
            ViewData["begin"] = "";
            ViewData["end"] = "";
            int TypeId = Request.QueryString["TypeId"].AsInt(0);
            int PositionId = Request.QueryString["PositionId"].AsInt(0);
            int LayerId = Request.QueryString["LayerId"].AsInt(0);
            var LayerList = service.MK_Layer.ToList();
            var ScanTypeList = service.MK_ScanType.Where(u=>u.IsCommand==1).ToList();
            var pointId = Request.QueryString["ID"];
            DateTime begin = Request.QueryString["begin"].AsDatetime(DateTime.Now);
            DateTime end = Request.QueryString["end"].AsDatetime(DateTime.Now);
           // var pointId = Request.QueryString["ID"];
            int p = 0;
            if (!string.IsNullOrEmpty(pointId))
            {
                var pp = pointId.Split(',');
                if (pp.Length > 0)
                {
                    p = pp[0].AsInt();
                }
            }
            var point = service.MK_Point.FirstOrDefault(u => u.ID == p);
         //   var point = service.MK_Point.FirstOrDefault(u => u.ID == pointId);
            if (point != null)
            {
                TypeId = point.ScanTypeId ?? 0;
                PositionId = point.PositionId ?? 0;
                ViewData["TypeId"] = TypeId;
                ViewData["PositionId"] = PositionId;
                var position = service.MK_Position.FirstOrDefault(u => u.ID == PositionId);
                if (position != null)
                {
                    LayerId = position.LayerId ?? 0;
                    ViewData["LayerId"] = LayerId;
                }

                //var scantype = service.MK_ScanType.FirstOrDefault(u => u.ID == TypeId);
                //if (scantype != null)
                //{
                //    if (scantype.IsCommand == 3)
                //    {
                //        Response.Redirect(string.Format("/Scan/Construction/DynamicIndex?ID={0}&begin={1}&end={2}", point.ID.ToString(),begin.ToString("yyyy-MM-dd"),end.ToString("yyyy-MM-dd")));
                //    }
                //}
            }
            else
            {
                ViewData["TypeId"] = TypeId;
                ViewData["PositionId"] = PositionId;
                ViewData["LayerId"] = LayerId;
              //  var scantype = service.MK_ScanType.FirstOrDefault(u => u.ID == TypeId);
                //if (scantype != null)
                //{
                //    if (scantype.IsCommand == 3)
                //    {
                //        Response.Redirect(string.Format("/Scan/Construction/DynamicIndex?TypeId={0}&LayerId={1}&PositionId={2}&begin={3}&end={4}", TypeId, LayerId, PositionId,begin.ToString("yyyy-MM-dd"),end.ToString("yyyy-MM-dd")));
                //    }
                //}

            }
            ViewData["begin"] = begin.ToString("yyyy-MM-dd");
            ViewData["end"] = end.ToString("yyyy-MM-dd");
            ViewData["LayerList"] = LayerList;
            ViewData["ScanTypeList"] = ScanTypeList;
            var PositionList = service.MK_Position.Where(u => u.LayerId == LayerId).ToList();
            var PointList = service.View_MK_Point.Where(u => u.PositionId == PositionId  && u.DeviceProtocol == 0).ToList();
            ViewData["PositionList"] = PositionList;
            ViewData["PointList"] = PointList;
            ViewData["Point"] = Request.QueryString["ID"].AsStr("");
            return View();
        }


        [FilterHelper]
        // GET: Scan/Construction
        public ActionResult AlarmCenter()
        {
            // Response.Redirect("/Scan/Construction/DynamicIndex");
            ViewData["TypeId"] = "";
            ViewData["PositionId"] = "";
            ViewData["LayerId"] = "";
            ViewData["begin"] = "";
            ViewData["end"] = "";
            int TypeId = Request.QueryString["TypeId"].AsInt(0);
            int PositionId = Request.QueryString["PositionId"].AsInt(0);
            int LayerId = Request.QueryString["LayerId"].AsInt(0);
            var LayerList = service.MK_Layer.ToList();
            var ScanTypeList = service.MK_ScanType.ToList();
            var pointId = Request.QueryString["ID"];
            DateTime begin = Request.QueryString["begin"].AsDatetime(DateTime.Now);
            DateTime end = Request.QueryString["end"].AsDatetime(DateTime.Now);
            // var pointId = Request.QueryString["ID"];
            int p = 0;
            if (!string.IsNullOrEmpty(pointId))
            {
                var pp = pointId.Split(',');
                if (pp.Length > 0)
                {
                    p = pp[0].AsInt();
                }
            }
            var point = service.MK_Point.FirstOrDefault(u => u.ID == p);
            //   var point = service.MK_Point.FirstOrDefault(u => u.ID == pointId);
            if (point != null)
            {
                TypeId = point.ScanTypeId ?? 0;
                PositionId = point.PositionId ?? 0;
                ViewData["TypeId"] = TypeId;
                ViewData["PositionId"] = PositionId;
                var position = service.MK_Position.FirstOrDefault(u => u.ID == PositionId);
                if (position != null)
                {
                    LayerId = position.LayerId ?? 0;
                    ViewData["LayerId"] = LayerId;
                }

                //var scantype = service.MK_ScanType.FirstOrDefault(u => u.ID == TypeId);
                //if (scantype != null)
                //{
                //    if (scantype.IsCommand == 3)
                //    {
                //        Response.Redirect(string.Format("/Scan/Construction/DynamicIndex?ID={0}&begin={1}&end={2}", point.ID.ToString(), begin.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd")));
                //    }
                //}
            }
            else
            {
                ViewData["TypeId"] = TypeId;
                ViewData["PositionId"] = PositionId;
                ViewData["LayerId"] = LayerId;
                //var scantype = service.MK_ScanType.FirstOrDefault(u => u.ID == TypeId);
                //if (scantype != null)
                //{
                //    if (scantype.IsCommand == 3)
                //    {
                //        Response.Redirect(string.Format("/Scan/Construction/DynamicIndex?TypeId={0}&LayerId={1}&PositionId={2}&begin={3}&end={4}", TypeId, LayerId, PositionId, begin.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd")));
                //    }
                //}

            }
            ViewData["begin"] = begin.ToString("yyyy-MM-dd");
            ViewData["end"] = end.ToString("yyyy-MM-dd");
            ViewData["LayerList"] = LayerList;
            ViewData["ScanTypeList"] = ScanTypeList;
            var PositionList = service.MK_Position.Where(u => u.LayerId == LayerId).ToList();
            var PointList = service.View_MK_Point.Where(u => u.PositionId == PositionId && u.ScanTypeId == TypeId).ToList();
            ViewData["PositionList"] = PositionList;
            ViewData["PointList"] = PointList;
            ViewData["Point"] = Request.QueryString["ID"].AsStr("");
            return View();
        }


        [FilterHelper]
        public ActionResult DynamicIndex()
        {
            ViewData["TypeId"] = "";
            ViewData["PositionId"] = "";
            ViewData["LayerId"] = "";
            ViewData["begin"] = "";
            ViewData["end"] = "";
            int TypeId = Request.QueryString["TypeId"].AsInt(0);
            int PositionId = Request.QueryString["PositionId"].AsInt(0);
            int LayerId = Request.QueryString["LayerId"].AsInt(0);
            DateTime begin = Request.QueryString["begin"].AsDatetime(DateTime.Now);
            DateTime end = Request.QueryString["end"].AsDatetime(DateTime.Now);
            
            var LayerList = service.MK_Layer.ToList();
            var ScanTypeList = service.MK_ScanType.ToList();
            var pointId = Request.QueryString["ID"];
            int p = 0;
            if (!string.IsNullOrEmpty(pointId))
            {
                var pp = pointId.Split(',');
                if (pp.Length > 0)
                {
                    p = pp[0].AsInt();
                }
            }
            var point = service.MK_Point.FirstOrDefault(u => u.ID == p);
            if (point != null)
            {
                TypeId = point.ScanTypeId ?? 0;
                PositionId = point.PositionId ?? 0;
                ViewData["TypeId"] = TypeId;
                ViewData["PositionId"] = PositionId;
                var position = service.MK_Position.FirstOrDefault(u => u.ID == PositionId);
                if (position != null)
                {
                    LayerId = position.LayerId ?? 0;
                    ViewData["LayerId"] = LayerId;
                }

                //var scantype = service.MK_ScanType.FirstOrDefault(u => u.ID == TypeId);
                //if (scantype != null)
                //{
                //    if (scantype.IsCommand != 3)
                //    {
                //        Response.Redirect(string.Format("/Scan/Construction/CalIndex?ID={0}&begin={1}&end={2}", point.ID.ToString(),begin.ToString("yyyy-MM0dd"),end.ToString("yyyy-MM-dd")));
                //    }
                //}
            }
            else
            {
                ViewData["TypeId"] = TypeId;
                ViewData["PositionId"] = PositionId;
                ViewData["LayerId"] = LayerId;

                var scantype = service.MK_ScanType.FirstOrDefault(u => u.ID == TypeId);
                //if (scantype != null)
                //{
                //    if (scantype.IsCommand != 3)
                //    {
                //        Response.Redirect(string.Format("/Scan/Construction/CalIndex?TypeId={0}&LayerId={1}&PositionId={2}&begin={3}&end={4}", TypeId, LayerId, PositionId,begin.ToString("yyyy-MM-dd"),end.ToString("yyyy-MM-dd")));
                //    }
                //}

            }
            ViewData["LayerList"] = LayerList;
            ViewData["ScanTypeList"] = ScanTypeList;
            ViewData["begin"] = begin.ToString("yyyy-MM-dd");
            ViewData["end"] = end.ToString("yyyy-MM-dd");
            ViewData["Point"] = Request.QueryString["ID"].AsStr("");
            var PositionList = service.MK_Position.Where(u => u.LayerId == LayerId).ToList();
            var PointList = service.View_MK_Point.Where(u => u.PositionId == PositionId && u.ScanTypeId == TypeId && u.DeviceProtocol == 1).ToList();
            ViewData["PositionList"] = PositionList;
            ViewData["PointList"] = PointList;
            return View();
        }

        [FilterHelper]
        public ActionResult PointSet()
        {
            var Devices = service.MK_Device.ToList();
            ViewData["Devices"] = Devices;
           

            var ScanTypes = service.MK_ScanType.ToList();
            foreach (var s in ScanTypes)
            {
                s.ImagePath = s.ImagePath + "-4.png";
            }
            ViewData["ScanTypes"] = ScanTypes;
            var Layers = service.MK_Layer.ToList();
            ViewData["Layers"] = Layers;
            var Postions = service.MK_Position.Where(a => a.LayerId != null).ToList();
            if (Postions.Count > 0)
            {
                ViewData["PositionImg"] = Postions[0].BackGroundPath;

                var PointList = service.View_MK_PointList.Where(u => u.PositionId == Postions[0].ID).Select(u => new ConstructCfgPointResponse
                {
                    ID = u.ID,
                    DeviceProtocol = u.DeviceProtocol,
                    MapXs = u.MapX.ToString() + "%",
                    MapYs = u.MapY.ToString() + "%",
                    TypeName = u.TypeName,
                    ImagePath=u.ImagePath + "-4.png",
                //ImagePath = u.ImagePath,
                PointName = u.PointName
                }).ToList();
                //foreach (var point in PointList)
                //{
                //    int count = 0;
                //    if (point.DeviceProtocol == 0)
                //    {
                //        count = service.MK_DataValue.Where(u => u.PointId == point.ID).Count();
                //        if (count > 0)
                //        {
                //            point.ImagePath = point.ImagePath + "-4.png";
                //        }
                //        else
                //        {
                //            point.ImagePath = point.ImagePath + "-5.png";
                //        }
                //    }
                //    else if(point.DeviceProtocol==1)
                //    {
                //        string DBName = "FD_DynamicValue2" + DateTime.Now.ToString("yyyyMM");
                //        var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                //        if (Exist.ID == 0)
                //        {
                //            point.ImagePath = point.ImagePath + "-5.png";
                //            continue;
                //            //return Json(ret, JsonRequestBehavior.DenyGet);
                //        }

                //        //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //        //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //        //if (CacheData != null)
                //        //{
                //        //    ret = CacheData as BaseReturn;

                //        //}
                //        //else
                //        //{
                //        string sql = string.Format("select count(*) as PointId from {0}  where PointId={1}", DBName, point.ID);
                //        IEnumerable<FD_Dynamic> query = service.ExecuteQuery<FD_Dynamic>(sql);
                //        count = query.FirstOrDefault().PointId;
                //        if (count > 0)
                //        {
                //            point.ImagePath = point.ImagePath + "-4.png";
                //        }
                //        else
                //        {
                //            point.ImagePath = point.ImagePath + "-5.png";
                //        }
                //    }
                //}
                ViewData["PointList"] = PointList;

            }
            else
            {
                ViewData["PointList"] = new List<ConstructCfgPointResponse>();
            }
            ViewData["Postions"] = Postions;

            

            return View();
        }

        [FilterHelper]
        // GET: Scan/Construction
        public ActionResult AlertIndex()
        {
            // Response.Redirect("/Scan/Construction/DynamicIndex");
            ViewData["TypeId"] = "";
            ViewData["PositionId"] = "";
            ViewData["LayerId"] = "";
            ViewData["begin"] = "";
            ViewData["end"] = "";
            int TypeId = Request.QueryString["TypeId"].AsInt(0);
            int PositionId = Request.QueryString["PositionId"].AsInt(0);
            int LayerId = Request.QueryString["LayerId"].AsInt(0);
            var LayerList = service.MK_Layer.ToList();
            var ScanTypeList = service.MK_ScanType.ToList();
            var pointId = Request.QueryString["ID"];
            DateTime begin = Request.QueryString["begin"].AsDatetime(DateTime.Now);
            DateTime end = Request.QueryString["end"].AsDatetime(DateTime.Now);
            // var pointId = Request.QueryString["ID"];

            //   var point = service.MK_Point.FirstOrDefault(u => u.ID == pointId);

            ViewData["TypeId"] = TypeId;
            ViewData["PositionId"] = PositionId;
            ViewData["LayerId"] = LayerId;
            ViewData["PointId"] = pointId;
            //  var scantype = service.MK_ScanType.FirstOrDefault(u => u.ID == TypeId);
            //if (scantype != null)
            //{
            //    if (scantype.IsCommand == 3)
            //    {
            //        Response.Redirect(string.Format("/Scan/Construction/DynamicIndex?TypeId={0}&LayerId={1}&PositionId={2}&begin={3}&end={4}", TypeId, LayerId, PositionId,begin.ToString("yyyy-MM-dd"),end.ToString("yyyy-MM-dd")));
            //    }
            //}

            ViewData["begin"] = begin.ToString("yyyy-MM-dd");
            ViewData["end"] = end.ToString("yyyy-MM-dd");
            ViewData["LayerList"] = LayerList;
            ViewData["ScanTypeList"] = ScanTypeList;
            var PositionList = service.MK_Position.Where(u => u.LayerId == LayerId).ToList();
            var PointList = service.View_MK_Point.Where(u => u.PositionId == PositionId && u.DeviceProtocol == 0).ToList();
            ViewData["PositionList"] = PositionList;
            ViewData["PointList"] = PointList;
            ViewData["Point"] = Request.QueryString["ID"].AsStr("");
            return View();
        }


        [FilterHelper]
        public ActionResult LayerSet()
        {
            //var Devices = service.MK_Device.ToList();
            //ViewData["Devices"] = Devices;
            //var Layers = service.MK_Layer.ToList();
            //ViewData["Layers"] = Layers;

            //var ScanTypes = service.MK_ScanType.ToList();
            //foreach (var s in ScanTypes)
            //{
            //    s.ImagePath = s.ImagePath + "-4.png";
            //}
            //ViewData["ScanTypes"] = ScanTypes;

            //var Postions = service.MK_Position.Where(a => a.LayerId != null).ToList();
            //if (Postions.Count > 0)
            //{
            //    ViewData["PositionImg"] = Postions[0].BackGroundPath;

            //    var PointList = service.View_MK_PointList.Where(u => u.PositionId == Postions[0].ID).Select(u => new ConstructCfgPointResponse
            //    {
            //        ID = u.ID,
            //        MapXs = u.MapX.ToString() + "%",
            //        MapYs = u.MapY.ToString() + "%",
            //        TypeName = u.TypeName,
            //        ImagePath = u.ImagePath,
            //        PointName = u.PointName
            //    }).ToList();
            //    foreach (var point in PointList)
            //    {
            //        int count = 0;
            //        if (point.IsCommand == 1)
            //        {
            //            count = service.MK_DataValue.Where(u => u.PointId == point.ID).Count();
            //            if (count > 0)
            //            {
            //                point.ImagePath = point.ImagePath + "-4.png";
            //            }
            //            else
            //            {
            //                point.ImagePath = point.ImagePath + "-5.png";
            //            }
            //        }
            //        else if (point.IsCommand == 3)
            //        {
            //            string DBName = "FD_DynamicValue2" + DateTime.Now.ToString("yyyyMM");
            //            var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
            //            if (Exist.ID == 0)
            //            {
            //                point.ImagePath = point.ImagePath + "-5.png";
            //                continue;
            //                //return Json(ret, JsonRequestBehavior.DenyGet);
            //            }

            //            //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

            //            //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

            //            //if (CacheData != null)
            //            //{
            //            //    ret = CacheData as BaseReturn;

            //            //}
            //            //else
            //            //{
            //            string sql = string.Format("select count(*) as PointId from {0}  where PointId={1}", DBName, point.ID);
            //            IEnumerable<FD_Dynamic> query = service.ExecuteQuery<FD_Dynamic>(sql);
            //            count = query.FirstOrDefault().PointId;
            //            if (count > 0)
            //            {
            //                point.ImagePath = point.ImagePath + "-4.png";
            //            }
            //            else
            //            {
            //                point.ImagePath = point.ImagePath + "-5.png";
            //            }
            //        }
            //    }
            //    ViewData["PointList"] = PointList;

            //}
            //else
            //{
            //    ViewData["PointList"] = new List<ConstructCfgPointResponse>();
            //}
            //ViewData["Postions"] = Postions;
            var LayerList = service.MK_Layer.Select(u => new ConstructCfgLayerResponse
            {
                ID = u.ID,
                MapXs = u.MapX.ToString() + "%",
                MapYs = u.MapY.ToString() + "%",
                LayerName=u.LayerName,
                //  TypeName = u.TypeName,
                BackgroundPath = u.BackgroundPath,
               // PointName = u.PointName
            }).ToList();
            ViewData["Layers"] = LayerList;
            var ScanTypes = service.MK_ScanType.ToList();
            ViewData["ScanTypes"] = ScanTypes;
            var Projects = service.Base_Project.Where(a => !a.IsDelete.Value).ToList();
            ViewData["Projects"] = Projects;
            return View();
        }


        [FilterHelper]
        public ActionResult PositionSet()
        {
            
            var ScanTypes = service.MK_ScanType.ToList();
            ViewData["ScanTypes"] = ScanTypes;
            if(ScanTypes.Count>0)
            {
                var LayerList = service.MK_Layer.Where(u => u.ScanTypeId == ScanTypes[0].ID).ToList();
                ViewData["Layers"] = LayerList;
                if(LayerList.Count>0)
                {
                    ViewData["LayerImg"] = LayerList[0].BackgroundPath.AsStr();
                    var PositionList = service.MK_Position.Where(u => u.LayerId == LayerList[0].ID).Select(u => new ConstructCfgPositionResponse
                    {
                        ID = u.ID,
                        MapXs = u.MapX.ToString() + "%",
                        MapYs = u.MapY.ToString() + "%",
                        //Widths = u.width.ToString() + "%",
                        //Heights = u.height.ToString() + "%",
                        LayerId = u.LayerId,
                        PositionName = u.PositionName
                    }).ToList();
                    ViewData["Position"] = PositionList;
                }
                else
                {
                    ViewData["Position"] = new ConstructCfgPositionResponse();
                    ViewData["LayerImg"] = "";
                }
            }
            else
            {
                ViewData["Layers"] = new MK_Layer();
                ViewData["LayerImg"] = "";
                ViewData["Position"] = new ConstructCfgPositionResponse();
            }
           
            return View();
        }

        [HttpPost]
        public JsonResult PositionSetQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            //if (request.PositionID == 0)
            //{
            //    return Json(ret, JsonRequestBehavior.DenyGet);
            //}
            try
            {
                var LayerList = service.MK_Position.AsQueryable();
                //if (request.PositionID.AsInt(0) > 0)
                //{
                //    PointList = PointList.Where(a => a.PositionId == request.PositionID);
                //}
                if (request.LayerID.AsInt() > 0)
                {
                    LayerList = LayerList.Where(a => a.LayerId == request.LayerID);
                }
                var Layer = LayerList.Select(u => new ConstructCfgPositionResponse
                {
                    ID = u.ID,
                    MapXs = u.MapX.ToString() + "%",
                    MapYs = u.MapY.ToString() + "%",
                    PositionName = u.PositionName,
                    //  TypeName = u.TypeName,
                    //BackGroundPath = u.BackGroundPath
                    // PointName = u.PointName
                }).ToList();
                //foreach (var point in Layer)
                //{
                //    var count = service.MK_DataValue.Where(u => u.PointId == point.ID).Count();
                //    if (count > 0)
                //    {
                //        point.ImagePath = point.ImagePath + "-4.png";
                //    }
                //    else
                //    {
                //        point.ImagePath = point.ImagePath + "-5.png";
                //    }
                //}
                ret.Data = Layer;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult LayerSetQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            //if (request.PositionID == 0)
            //{
            //    return Json(ret, JsonRequestBehavior.DenyGet);
            //}
            try
            {
                var LayerList = service.MK_Layer.AsQueryable();
                //if (request.PositionID.AsInt(0) > 0)
                //{
                //    PointList = PointList.Where(a => a.PositionId == request.PositionID);
                //}
                if (request.ScanTypeID.AsInt() > 0)
                {
                    LayerList = LayerList.Where(a => a.ScanTypeId == request.ScanTypeID);
                }
                var Layer = LayerList.Select(u => new ConstructCfgLayerResponse
                {
                    ID = u.ID,
                    MapXs = u.MapX.ToString() + "%",
                    MapYs = u.MapY.ToString() + "%",
                    LayerName = u.LayerName,
                    //  TypeName = u.TypeName,
                    BackgroundPath = u.BackgroundPath,
                    // PointName = u.PointName
                }).ToList();
                //foreach (var point in Layer)
                //{
                //    var count = service.MK_DataValue.Where(u => u.PointId == point.ID).Count();
                //    if (count > 0)
                //    {
                //        point.ImagePath = point.ImagePath + "-4.png";
                //    }
                //    else
                //    {
                //        point.ImagePath = point.ImagePath + "-5.png";
                //    }
                //}
                ret.Data = Layer;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

    


        [HttpPost]
        public JsonResult PointSetQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.PositionID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var PointList = service.View_MK_PointList.AsQueryable();
                if (request.PositionID.AsInt(0) > 0)
                {
                    PointList = PointList.Where(a => a.PositionId == request.PositionID);
                }
                //if (request.ScanTypeID.AsInt() > 0)
                //{
                //    PointList = PointList.Where(a => a.ScanTypeId == request.ScanTypeID);
                //}
                var Point = PointList.Select(u => new
                ConstructCfgPositionResponse
                {
                    ID = u.ID,
                    MapXs = u.MapX + "%",
                    MapYs = u.MapY + "%",
                    //ImagePath = u.ImagePath,
                    ImagePath = u.ImagePath + "-4.png",
                    TypeName = u.TypeName,
                    PositionName = u.PointName
                }).ToList();
                //foreach (var point in Point)
                //{
                //    var count = service.MK_DataValue.Where(u => u.PointId == point.ID).Count();
                //    if (count > 0)
                //    {
                //        point.ImagePath = point.ImagePath + "-4.png";
                //    }
                //    else
                //    {
                //        point.ImagePath = point.ImagePath + "-5.png";
                //    }
                //}
                ret.Data = Point;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }


        [FilterHelper]
        public ActionResult Config()
        {
            
            var Projects = service.Base_Project.Where(a => !a.IsDelete.Value).ToList();
            ViewData["Projects"] = Projects;

            var Layers = service.MK_Layer.ToList();
            ViewData["Layers"] = Layers;

            var ScanTypes = service.MK_ScanType.ToList();
            ViewData["ScanTypes"] = ScanTypes;

            var Protocols = service.MK_Protocol.Where(a => !a.IsDelete.Value).ToList();
            ViewData["Protocols"] = Protocols;

            var Postions = service.MK_Position.Where(a => a.LayerId != null).ToList();
            ViewData["Postions"] = Postions;

            var Devices = service.MK_Device.ToList();
            ViewData["Devices"] = Devices;

            var Config = service.MK_AutoReadConfig.FirstOrDefault();
            ViewData["Config"] = Config;

            return View();
        }
        public PartialViewResult Layer()
        {
            return PartialView();
        }
        public PartialViewResult Point()
        {
            return PartialView();
        }
        public PartialViewResult Position()
        {
            return PartialView();
        }
        public PartialViewResult Protocol()
        {
            return PartialView();
        }
        public PartialViewResult ScanType()
        {
            return PartialView();
        }
        public PartialViewResult Acquisition()
        {
            return PartialView();
        }

        #region AutoScan


        [HttpPost]
        public JsonResult AutoChart(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{
                var query = service.View_MK_AutoScan.Where(a => a.ID > 0);

                string url = "/Scan/Construction/DynamicIndex?TypeId=0&LayerId=0&PositionId=0&ID=0&begin=" + request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + "&end=" + request.End.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd");
                //var query = service.View_MK_AutoScan.Where(a => a.ID > 0);

                if (!string.IsNullOrEmpty(request.PointID))
                {
                    var pointId = request.PointID.Split(',').ToList();
                    List<int> Point = new List<int>();
                    foreach (var p in pointId)
                    {
                        if(p.AsInt(0)>0)
                        Point.Add(p.AsInt());
                    }
                    query = query.Where(a => Point.Contains(a.PointId));
                    url = url.Replace("ID=0", "ID=" + request.PointID);
                }
                if (request.PositionID != 0)
                {
                    query = query.Where(a => a.PositionID == request.PositionID);
                    url = url.Replace("PositionId=0", "PositionId=" + request.PositionID.ToString());
                }
                //if (request.DeviceID != 0)
                //{
                //    query = query.Where(a => a.DeviceID == request.DeviceID);
                //}
                if (request.LayerID != 0)
                {
                    query = query.Where(a => a.LayerID == request.LayerID);
                    url = url.Replace("LayerId=0", "LayerId=" + request.LayerID.ToString());
                }
                //if (request.ScanTypeID != 0)
                //{
                //    query = query.Where(a => a.ScanTypeID == request.ScanTypeID);

                //    url = url.Replace("TypeId=0", "TypeId=" + request.ScanTypeID.ToString());
                //    var type = service.MK_ScanType.FirstOrDefault(u => u.ID == request.ScanTypeID);
                //    if (type != null)
                //    {

                //        if (type.IsCommand == 3)
                //        {
                //            ret.value = 1;
                //            ret.Url = url;
                //            ret.Success = true;
                //            return Json(ret, JsonRequestBehavior.DenyGet);
                //        }
                //    }
                //}
                if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                {
                    if (!request.Begin.Contains(" "))
                    {
                        request.Begin += " 00:00:00";
                    }
                    if (!request.End.Contains(" "))
                    {
                        request.End += " 23:59:59";
                    }
                    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                    DateTime EndTime = Convert.ToDateTime(request.End.AsDatetime(DateTime.Now));
                    query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).OrderBy(u=>u.CreateDate).AsQueryable();
                }
                //var echart = GetEchartData(request.DeviceID, request.PointID, request.Begin, request.End);
                var echart = GetEchartData(query.ToList(), 2);
               // ret.Data = list;
                ret.Echart = echart;
                ret.PageIndex = request.PageIndex;
                ret.Success = true;
                //    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
                //}


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult AutoLoadChart(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{
                //var point = service.MK_Point.FirstOrDefault(u => u.ID == request.PointID);
                //if (point != null)
                //{
                //    request.DeviceID = point.DeviceId ?? 0;
                //}
                var query = service.View_MK_AutoScan.Where(a => a.ID > 0);

                //if (request.PointID != 0)
                //{
                //    query = query.Where(a => a.PointId == request.PointID);
                //}
                if (request.PositionID != 0)
                {
                    query = query.Where(a => a.PositionID == request.PositionID);
                }
                //if (request.DeviceID != 0)
                //{
                //    query = query.Where(a => a.DeviceID == request.DeviceID);
                //}
                var pointId = request.PointID.Split(',').ToList();
                List<int> point = new List<int>();
                foreach (var p in pointId)
                {
                    if (p.AsInt(0) > 0)
                    {
                        point.Add(p.AsInt());
                    }
                }
                if (point.Count > 0)
                {
                    query = query.Where(a => point.Contains(a.PointId));
                }
                if (request.LayerID != 0)
                {
                    query = query.Where(a => a.LayerID == request.LayerID);
                }
                if (request.ScanTypeID != 0)
                {
                    query = query.Where(a => a.ScanTypeID == request.ScanTypeID);
                }
                if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                {
                    if (!request.Begin.Contains(" "))
                    {
                        request.Begin += " 00:00:00";
                    }
                    if (!request.End.Contains(" "))
                    {
                        request.End += " 23:59:59";
                    }
                    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                    DateTime EndTime = Convert.ToDateTime(request.End);
                    query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                }
                var echart = GetEchartData(query.OrderBy(u=>u.CreateDate).ToList(),2);

              //  ret.Data = list;
                ret.Echart = echart;
                ret.PageIndex = request.PageIndex;
                ret.Success = true;
                //    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
                //}


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult AlertLoadQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{

                if (!request.Begin.AsStr().Contains(" "))
                {
                    request.Begin += " 00:00:00";
                }
                else
                {
                    request.Begin = request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 00:00:00";
                }
                if (!request.End.AsStr().Contains(" "))
                {
                    request.End += " 23:59:59";
                }
                else
                {
                    request.End = request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 23:59:59";
                }
                //DateTime BeginTime = Convert.ToDateTime(request.Begin);
                //DateTime EndTime = Convert.ToDateTime(request.End.AsDatetime(DateTime.Now));
                //if (request.ScanTypeID == 8)
                //{
                //    BeginTime = new DateTime(BeginTime.Year, BeginTime.Month, 1);
                //    EndTime = new DateTime(EndTime.Year, EndTime.Month, 1);
                //}

                var query = service.View_MK_AlertList.Where(a => a.ID > 0 && a.CreateDate<=DateTime.Now);
                if (request.LayerID.AsInt(0) > 0)
                {
                    query = query.Where(u => u.LayerId == request.LayerID);
                }

                //var echart = GetEchartData(request.DeviceID, request.PointID, request.Begin, request.End);
                int Count = query.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                //如果不是第一页，并且没有数据，则查询第一页
                var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();

                CurrentQuery.ForEach(a =>
                {
                    ConstructAutoResponse se = new ConstructAutoResponse();
                    se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                    // se.DeviceName = a.DeviceName;
                    se.LayerName = a.LayerName;
                    se.PointName = a.PointName;
                    se.PositionName = a.PositionName;
                    se.ScanTypeName = a.TypeName;
                    se.ScanValue = a.ScanValue.HasValue ? a.ScanValue.Value.ToString("#0.00") : "";
                    se.ScanValue2 = a.AlertValue.HasValue ? a.AlertValue.Value.ToString("#0.00") : "";
                    se.level = a.Level ?? 0;
                    list.Add(se);
                });





                ret.Data = list;
                //  ret.Echart = echart;
                ret.PageIndex = request.PageIndex;
                ret.Success = true;
                //    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
                //}


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult AlertQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{
                var query = service.View_MK_AlertList.Where(a => a.ID > 0);
                if (request.PointID.AsInt() != 0)
                {
                    query = query.Where(a => a.PointId == request.PointID.AsInt());
                }

                //var query = service.View_MK_AutoScan.Where(a => a.ID > 0);


                if (request.PositionID != 0)
                {
                    query = query.Where(a => a.PositionId == request.PositionID);

                }
                //if (request.DeviceID != 0)
                //{
                //    query = query.Where(a => a.DeviceID == request.DeviceID);
                //}
                if (request.LayerID != 0)
                {
                    query = query.Where(a => a.LayerId == request.LayerID);
                }
                if (request.ScanTypeID != 0)
                {
                    query = query.Where(a => a.ScanTypeId == request.ScanTypeID);

                }
                if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                {
                    if (!request.Begin.Contains(" "))
                    {
                        request.Begin += " 00:00:00";
                    }
                    else
                    {
                        request.Begin = request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 00:00:00";
                    }
                    if (!request.End.Contains(" "))
                    {
                        request.End += " 23:59:59";
                    }
                    else
                    {
                        request.End = request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 23:59:59";
                    }
                    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                    DateTime EndTime = Convert.ToDateTime(request.End.AsDatetime(DateTime.Now));
                    if(request.ScanTypeID==8)
                    {
                        BeginTime = new DateTime(BeginTime.Year, BeginTime.Month, 1);
                        EndTime = new DateTime(EndTime.Year, EndTime.Month, 1);
                    }
                   
                    query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                }
                //var echart = GetEchartData(request.DeviceID, request.PointID, request.Begin, request.End);
                int Count = query.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                //如果不是第一页，并且没有数据，则查询第一页
                var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();

                CurrentQuery.ForEach(a =>
                {
                    ConstructAutoResponse se = new ConstructAutoResponse();
                    se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                    // se.DeviceName = a.DeviceName;
                    se.LayerName = a.LayerName;
                    se.PointName = a.PointName;
                    se.PositionName = a.PositionName;
                    se.ScanTypeName = a.TypeName;
                    se.ScanValue = a.ScanValue.HasValue ? a.ScanValue.Value.ToString("#0.00") : "";
                    se.ScanValue2 = a.AlertValue.HasValue ? a.AlertValue.Value.ToString("#0.00") : "";
                    se.level = a.Level ?? 0;
                    list.Add(se);
                });





                ret.Data = list;
                //  ret.Echart = echart;
                ret.PageIndex = request.PageIndex;
                ret.Success = true;
                //    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
                //}


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }




        [HttpPost]
        public JsonResult AutoLoadQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{
                //var point = service.MK_Point.FirstOrDefault(u => u.ID == request.PointID);
                //if (point != null)
                //{
                //    request.DeviceID = point.DeviceId ?? 0;
                //}
                var query = service.View_MK_AutoScan.Where(a => a.ID > 0);
                var pointId = request.PointID.Split(',').ToList();
                List<int> point = new List<int>();
                foreach(var p in pointId)
                {
                    if(p.AsInt(0)>0)
                    {
                        point.Add(p.AsInt());
                    }
                }
                if (point.Count > 0)
                {
                    query = query.Where(a => point.Contains(a.PointId));
                }
                if (request.PositionID != 0)
                {
                    query = query.Where(a => a.PositionID == request.PositionID);
                }
                //if (request.DeviceID != 0)
                //{
                //    query = query.Where(a => a.DeviceID == request.DeviceID);
                //}
                if (request.LayerID != 0)
                {
                    query = query.Where(a => a.LayerID == request.LayerID);
                }
                if (request.ScanTypeID != 0)
                {
                    query = query.Where(a => a.ScanTypeID == request.ScanTypeID);
                }
                if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                {
                    if (!request.Begin.Contains(" "))
                    {
                        request.Begin += " 00:00:00";
                    }
                    if (!request.End.Contains(" "))
                    {
                        request.End += " 23:59:59";
                    }
                    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                    DateTime EndTime = Convert.ToDateTime(request.End);
                    query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                }
                var echart = GetEchartData1(query.OrderBy(u=>u.CreateDate).ToList());
                int Count = query.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                //如果不是第一页，并且没有数据，则查询第一页
                var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();

                CurrentQuery.ForEach(a =>
                {
                    ConstructAutoResponse se = new ConstructAutoResponse();
                    se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                    se.DeviceName = a.DeviceName;
                    se.LayerName = a.LayerName;
                    se.PointName = a.PointName;
                    se.PositionName = a.PositionName;
                    se.ScanTypeName = a.TypeName;
                    se.ScanValue = a.ScanValue.HasValue ? a.ScanValue.Value.ToString("#0.0000") : "";
                    se.ScanValue2 = a.ScanValue2.HasValue ? a.ScanValue2.Value.ToString("#0.0000") : "";
                    se.ScanValue3 = a.ScanValue3.HasValue ? a.ScanValue3.Value.ToString("#0.0000") : "";
                    se.ScanValue4 = a.ScanValue4.HasValue ? a.ScanValue4.Value.ToString("#0.0000") : "";
                    se.ScanValue5 = a.ScanValue5.HasValue ? a.ScanValue5.Value.ToString("#0.0000") : "";
                    se.ScanValue6 = a.ScanValue6.HasValue ? a.ScanValue6.Value.ToString("#0.0000") : "";
                    list.Add(se);
                });





                ret.Data = list;
                ret.Echart = echart;
                ret.PageIndex = request.PageIndex;
                ret.Success = true;
                //    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
                //}


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }




        [HttpPost]
        public JsonResult AutoQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{
                    var query = service.View_MK_AutoScan.Where(a=> a.ID > 0);

                    string url = "/Scan/Construction/DynamicIndex?TypeId=0&LayerId=0&PositionId=0&ID=0&begin=" + request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + "&end=" + request.End.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd");
                //var query = service.View_MK_AutoScan.Where(a => a.ID > 0);

                if (!string.IsNullOrEmpty(request.PointID))
                {
                    var PointId = request.PointID.Split(',').ToList();
                    List<int> point = new List<int>();
                    string s = "";
                    int count = 0;
                    foreach (var p in PointId)
                    {
                        if (p.AsInt(0) > 0)
                        {
                            if (count == 0)
                            {
                                s += p;
                            }
                            else
                            {
                                s += "," + p;
                            }
                            point.Add(p.AsInt());
                            count++;
                        }
                    }
                    if (count > 0)
                        query = query.Where(a => point.Contains(a.PointId));
                    url = url.Replace("ID=0", "ID in(" + s + ")");
                }
                    if (request.PositionID != 0)
                    {
                        query = query.Where(a => a.PositionID == request.PositionID);
                        url = url.Replace("PositionId=0", "PositionId=" + request.PositionID.ToString());
                    }
                    //if (request.DeviceID != 0)
                    //{
                    //    query = query.Where(a => a.DeviceID == request.DeviceID);
                    //}
                    if (request.LayerID != 0)
                    {
                        query = query.Where(a => a.LayerID == request.LayerID);
                        url = url.Replace("LayerId=0", "LayerId=" + request.LayerID.ToString());
                    }
                    if (request.ScanTypeID != 0)
                    {
                        query = query.Where(a => a.ScanTypeID == request.ScanTypeID);

                        url = url.Replace("TypeId=0", "TypeId=" + request.ScanTypeID.ToString());
                        var type = service.MK_ScanType.FirstOrDefault(u => u.ID == request.ScanTypeID);
                        if (type != null)
                        {
                        
                                if (type.IsCommand == 3)
                                {
                                    ret.value = 1;
                                    ret.Url = url;
                                    ret.Success = true;
                                    return Json(ret, JsonRequestBehavior.DenyGet);
                                }
                        }
                    }
                    if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                    {
                    if (!request.Begin.Contains(" "))
                    {
                        request.Begin += " 00:00:00";
                    }
                    else
                    {
                        request.Begin = request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 00:00:00";
                    }
                    if (!request.End.Contains(" "))
                    {
                        request.End += " 23:59:59";
                    }
                    else
                    {
                        request.End = request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 23:59:59";
                    }
                    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                    DateTime EndTime = Convert.ToDateTime(request.End.AsDatetime(DateTime.Now));
                    query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                    }
                //var echart = GetEchartData(request.DeviceID, request.PointID, request.Begin, request.End);
                var echart = GetEchartData(query.OrderBy(u=>u.CreateDate).ToList(),1);
                int Count = query.Count();
                    ret.TotalCount = Count;
                    ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                    //如果不是第一页，并且没有数据，则查询第一页
                    var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                    {
                        request.PageIndex = 1;
                        CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    }

                    List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();

                    CurrentQuery.ForEach(a =>
                    {
                        ConstructAutoResponse se = new ConstructAutoResponse();
                        se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                        se.DeviceName = a.DeviceName;
                        se.LayerName = a.LayerName;
                        se.PointName = a.PointName;
                        se.PositionName = a.PositionName;
                        se.ScanTypeName = a.TypeName;
                        se.ScanValue = a.ScanValue.HasValue ? a.ScanValue.Value.ToString("#0.0000") : "";
                        se.ScanValue2 = a.ScanValue2.HasValue ? a.ScanValue2.Value.ToString("#0.0000") : "";
                        se.ScanValue4 = a.ScanValue3.HasValue ? a.ScanValue3.Value.ToString("#0.0000") : "";
                        se.ScanValue5 = a.ScanValue4.HasValue ? a.ScanValue4.Value.ToString("#0.0000") : "";
                        se.ScanValue5 = a.ScanValue5.HasValue ? a.ScanValue5.Value.ToString("#0.0000") : "";
                        se.ScanValue6 = a.ScanValue6.HasValue ? a.ScanValue6.Value.ToString("#0.0000") : "";
                        list.Add(se);
                    });



                    

                    ret.Data = list;
                    ret.Echart = echart;
                    ret.PageIndex = request.PageIndex;
                    ret.Success = true;
            //    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
            //}


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult AutoCalQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{
                    string url = "/Scan/Construction/DynamicIndex?TypeId=0&LayerId=0&PositionId=0&ID=0&begin=" + request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + "&end=" + request.End.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd");
                    var query = service.View_MK_AutoScan.Where(a => a.ID > 0);
                    ret.value = 0;
                List<int> Point = new List<int>();
                if (!string.IsNullOrEmpty(request.PointID))
                {
                    var pointId = request.PointID.Split(',').ToList();
                   
                    int count = 0;
                    string s = "0";
                    foreach(var p in pointId)
                    {
                        if(p.AsInt(0)>0)
                        {
                            s += "," + p;
                            Point.Add(p.AsInt());
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        query = query.Where(a => Point.Contains(a.PointId));
                    }
                    url = url.Replace("ID=0", "ID=" + request.PointID);
                }
                    if (request.PositionID != 0)
                    {
                        query = query.Where(a => a.PositionID == request.PositionID);
                        url = url.Replace("PositionId=0", "PositionId=" + request.PositionID.ToString());
                    }
                    //if (request.DeviceID != 0)
                    //{
                    //    query = query.Where(a => a.DeviceID == request.DeviceID);
                    //}
                    if (request.LayerID != 0)
                    {
                        query = query.Where(a => a.LayerID == request.LayerID);
                        url = url.Replace("LayerId=0", "LayerId=" + request.LayerID.ToString());
                    }
                    //if (request.ScanTypeID != 0)
                    //{
                    //    query = query.Where(a => a.ScanTypeID == request.ScanTypeID);
                       
                    //    url = url.Replace("TypeId=0", "TypeId=" + request.ScanTypeID.ToString());
                    //    var type = service.MK_ScanType.FirstOrDefault(u => u.ID == request.ScanTypeID);
                    //    if (type != null)
                    //    {
                    //        if (type.IsCommand == 3)
                    //        {
                    //            ret.value = 1;
                    //            ret.Url = url;
                    //            ret.Success = true;
                    //            return Json(ret, JsonRequestBehavior.DenyGet);
                    //        }
                    //        //Response.Redirect(url);
                            
                    //    }
                    //}
                    if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                    {
                    if (!request.Begin.Contains(" "))
                    {
                        request.Begin += " 00:00:00";
                    }
                    else
                    {
                        request.Begin = request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 00:00:00";
                    }
                    if (!request.End.Contains(" "))
                    {
                        request.End += " 23:59:59";
                    }
                    else
                    {
                        request.End = request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 23:59:59";
                    }
                    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                        DateTime EndTime = Convert.ToDateTime(request.End);
                        query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                    }

                    int Count = query.Count();
                    ret.TotalCount = Count;
                    ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                    //如果不是第一页，并且没有数据，则查询第一页
                    var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                    {
                        request.PageIndex = 1;
                        CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    }

                    List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();

                    CurrentQuery.ForEach(a =>
                    {
                        ConstructAutoResponse se = new ConstructAutoResponse();
                        se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                        se.DeviceName = a.DeviceName;
                        se.LayerName = a.LayerName;
                        se.PointName = a.PointName;
                        se.PositionName = a.PositionName;
                        se.ScanTypeName = a.TypeName;
                        se.ScanValue = a.ScanValue.HasValue ? a.ScanValue.Value.ToString("#0.0000") : "";
                        se.ScanValue2 = a.ScanValue2.HasValue ? a.ScanValue2.Value.ToString("#0.0000") : "";
                        se.ScanValue4 = a.ScanValue3.HasValue ? a.ScanValue3.Value.ToString("#0.0000") : "";
                        se.ScanValue5 = a.ScanValue4.HasValue ? a.ScanValue4.Value.ToString("#0.0000") : "";
                        se.ScanValue5 = a.ScanValue5.HasValue ? a.ScanValue5.Value.ToString("#0.0000") : "";
                        se.ScanValue6 = a.ScanValue6.HasValue ? a.ScanValue6.Value.ToString("#0.0000") : "";
                        list.Add(se);
                    });



                    var echart = GetEchartData2(request.DeviceID, Point, request.Begin, request.End);

                    ret.Data = list;
                    ret.Echart = echart;
                    ret.PageIndex = request.PageIndex;
                    ret.Success = true;
                   // CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
             //   }


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
            
        }


        public JsonResult AutoCalChartQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                // string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{
                //var query = service.View_MK_AutoScan.Where(a => a.ID > 0);

                //if (request.PointID != 0)
                //{
                //    query = query.Where(a => a.PointId == request.PointID);
                //}
                //if (request.PositionID != 0)
                //{
                //    query = query.Where(a => a.PositionID == request.PositionID);
                //}
                //if (request.DeviceID != 0)
                //{
                //    query = query.Where(a => a.DeviceID == request.DeviceID);
                //}
                //if (request.LayerID != 0)
                //{
                //    query = query.Where(a => a.LayerID == request.LayerID);
                //}
                //if (request.ScanTypeID != 0)
                //{
                //    query = query.Where(a => a.ScanTypeID == request.ScanTypeID);
                //}
                //if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                //{
                //    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                //    DateTime EndTime = Convert.ToDateTime(request.End);
                //    query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                //}

                //int Count = query.Count();
                //ret.TotalCount = Count;
                //ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                ////如果不是第一页，并且没有数据，则查询第一页
                //var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                //if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                //{
                //    request.PageIndex = 1;
                //    CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                //}

                //List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();

                //CurrentQuery.ForEach(a =>
                //{
                //    ConstructAutoResponse se = new ConstructAutoResponse();
                //    se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                //    se.DeviceName = a.DeviceName;
                //    se.LayerName = a.LayerName;
                //    se.PointName = a.PointName;
                //    se.PositionName = a.PositionName;
                //    se.ScanTypeName = a.TypeName;
                //    se.ScanValue = a.ScanValue.HasValue ? a.ScanValue.Value.ToString("#0.00") : "";
                //    se.ScanValue2 = a.ScanValue2.HasValue ? a.ScanValue2.Value.ToString("#0.00") : "";
                //    se.ScanValue4 = a.ScanValue3.HasValue ? a.ScanValue3.Value.ToString("#0.00") : "";
                //    se.ScanValue5 = a.ScanValue4.HasValue ? a.ScanValue4.Value.ToString("#0.00") : "";
                //    se.ScanValue5 = a.ScanValue5.HasValue ? a.ScanValue5.Value.ToString("#0.00") : "";
                //    se.ScanValue6 = a.ScanValue6.HasValue ? a.ScanValue6.Value.ToString("#0.00") : "";
                //    list.Add(se);
                //});

                var pointId = request.PointID.Split(',').ToList();
                List<int> Point = new List<int>();
                foreach(var p in pointId)
                {
                    if(p.AsInt(0)>0)
                    {
                        Point.Add(pointId.AsInt(0));
                    }
                }

                var echart = GetEchartData3(request.DeviceID, Point, request.Begin, request.End);

                  //  ret.Data = list;
                    ret.Echart = echart;
                    ret.PageIndex = request.PageIndex;
                    ret.Success = true;
                //    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
                //}


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }


        private DateTime getEndDate(DateTime date)
        {
          
                string temp = date.ToString("yyyy-MM-dd") + " " + "23:59:59";
                return temp.AsDatetime(DateTime.Now);
        }

        private DateTime getBeginDate(DateTime date)
        {
          
                string temp = date.ToString("yyyy-MM-dd") + " " + "00:00:00";
                return temp.AsDatetime(DateTime.Now);
        }




        [HttpPost]
        public JsonResult DynamicQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            //ret.Success = true;
            //return Json(ret, JsonRequestBehavior.DenyGet);
            try
            {


                //string url = "/Scan/Construction/CalIndex?TypeId=0&LayerId=0&PositionId=0&ID=0&begin=" + request.Begin.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + "&end=" + request.End.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd");
                //if (!string.IsNullOrEmpty(request.PointID))
                //{
                //   // query = query.Where(a => a.PointId == request.PointID);
                //    url = url.Replace("ID=0", "ID=" + request.PointID);
                //}
                ////if (request.PositionID != 0)
                ////{
                ////    query = query.Where(a => a.PositionID == request.PositionID);
                ////}
              
                //if (request.LayerID != 0)
                //{
                //    //query = query.Where(a => a.LayerId == request.LayerID);
                //    url = url.Replace("LayerId=0", "LayerId=" + request.LayerID.ToString());
                //}
                //if (request.ScanTypeID != 0)
                //{
                //   // query = query.Where(a => a.ScanTypeId == request.ScanTypeID);
                //    //url = url.Replace("TypeId=0", "TypeId=" + request.ScanTypeID.ToString());
                //    //var type = service.MK_ScanType.FirstOrDefault(u => u.ID == request.ScanTypeID);
                //    //if (type != null)
                //    //{
                //    //    if (type.IsCommand != 3)
                //    //    {
                //    //        ret.value = 1;
                //    //        ret.Url = url;
                //    //        ret.Success = true;
                //    //        return Json(ret, JsonRequestBehavior.DenyGet);
                //    //    }
                //    //    //Response.Redirect(url);

                //    //}
                //}


                string DBName = "FD_DynamicValue3" + getBeginDate(request.Begin.AsDatetime(DateTime.Now)).ToString("yyyyMM");
                var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                if (Exist.ID == 0)
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                
                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{
                    string sql = string.Format("select a.ID,PointId,b.PositionId,a.CreateDate,ScanValue,Result,b.PointName,c.LayerId,e.ScanTypeId from {0} a inner join MK_Point b on a.PointId=b.ID inner join MK_Position c on b.PositionId=c.ID  inner join MK_Layer e on c.LayerId=e.ID where 1=1 ", DBName);

                //var query = service.View_MK_AutoScan.Where(a => a.ID > 0);
                //string url = "/Scan/Construction/CalIndex?TypeId=0&LayerId=0&PositionId=0&ID=0";
                if (!string.IsNullOrEmpty(request.PointID))
                {
                    var PointId = request.PointID.Split(',').ToList();
                    List<int> point = new List<int>();
                    string s = "";
                    int count = 0;
                    foreach (var p in PointId)
                    {
                        if (p.AsInt(0) > 0)
                        {
                            if (count == 0)
                            {
                                s += p;
                            }
                            else
                            {
                                s += "," + p;
                            }
                            point.Add(p.AsInt());
                            count++;
                        }
                    }
                    if (count > 0)
                        sql += " and PointId in(" + s + ") ";
                    // query = query.Where(a => a.PointId == request.PointID);
                    //  url = url.Replace("ID=0", "ID=" + request.PointID.ToString());
                }
                if (request.PositionID != 0)
                {
                    sql += " and b.PositionId=" + request.PositionID.ToString();
                    //query = query.Where(a => a.PositionID == request.PositionID);
                }
                //if (request.DeviceID != 0)
                //    {
                //    sql += " and b.DeviceId="+request.DeviceID.ToString();
                //       // query = query.Where(a => a.DeviceId == request.DeviceID);
                      
                //    }
                    if (request.LayerID != 0)
                    {
                    sql += " and c.LayerId="+request.LayerID.ToString();
                        //query = query.Where(a => a.LayerId == request.LayerID);
                       // url = url.Replace("LayerId=0", "LayerId=" + request.LayerID.ToString());
                    }
                   
                    if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                    {
                    if (!request.Begin.Contains(" "))
                    {
                        request.Begin += " 00:00:00";
                    }
                    if (!request.End.Contains(" "))
                    {
                        request.End += " 23:59:59";
                    }
                    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                        DateTime EndTime = Convert.ToDateTime(request.End);
                    sql += " and a.CreateDate>'"+request.Begin+ "' and a.CreateDate<'"+ request.End+"'";
                       // query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                    }
                IEnumerable<FD_Dynamic> query = service.ExecuteQuery<FD_Dynamic>(sql).ToList();
                //var list1 = query.ToList();
                    int Count = query.Count();
                    ret.TotalCount = Count;
                    ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                    //如果不是第一页，并且没有数据，则查询第一页
                    var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                    {
                        request.PageIndex = 1;
                        CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    }

                    List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();

                    CurrentQuery.ForEach(a =>
                    {
                        ConstructAutoResponse se = new ConstructAutoResponse();
                        se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                        //se.DeviceName = a.DeviceName;
                        se.LayerName = service.MK_Layer.FirstOrDefault(u=>u.ID==a.LayerId).LayerName;
                        se.PointName = a.PointName;
                        se.PositionName = service.MK_Position.FirstOrDefault(u=>u.ID==a.PositionId).PositionName;
                        se.ScanTypeName = service.MK_ScanType.FirstOrDefault(u=>u.ID==a.ScanTypeId).TypeName;
                        se.ScanValue = a.ScanValue.ToString("F4");
                        se.ScanValue2 = a.Result.ToString("F4");

                        list.Add(se);
                    });



                    var echart = GetEchartDynamic(query.ToList());

                    ret.Data = list;
                    ret.Echart = echart;
                    ret.PageIndex = request.PageIndex;
                    ret.Success = true;
                  //  CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
             //   }


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DynamicChart(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                string DBName = "FD_DynamicValue2" + getBeginDate(request.Begin.AsDatetime(DateTime.Now)).ToString("yyyyMM");
                var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                if (Exist.ID == 0)
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{
                string sql = string.Format("select a.ID,PointId,b.PositionId,b.DeviceId,d.DeviceName,a.CreateDate,Result,ScanValue,b.PointName,c.LayerId from {0} a inner join MK_Point b on a.PointId=b.ID inner join MK_Position c on b.PositionId=c.ID inner join MK_Device d on b.DeviceId=d.ID where 1=1 ", DBName);
               
                //var query = service.View_MK_AutoScan.Where(a => a.ID > 0);

                //if (request.PointID != 0)
                //{
                //    query = query.Where(a => a.PointId == request.PointID);
                //}
                ////if (request.PositionID != 0)
                ////{
                ////    query = query.Where(a => a.PositionID == request.PositionID);
                ////}
                //if (request.DeviceID != 0)
                //{
                //    query = query.Where(a => a.DeviceId == request.DeviceID);
                //}
                //if (request.LayerID != 0)
                //{
                //    query = query.Where(a => a.LayerId == request.LayerID);
                //}
                //if (request.ScanTypeID != 0)
                //{
                //    query = query.Where(a => a.ScanTypeId == request.ScanTypeID);
                //}
                if (!string.IsNullOrEmpty(request.PointID))
                {
                    var pointId = request.PointID.Split(',').ToList();
                    List<int> Point = new List<int>();
                    int count = 0;
                    string s = "";
                    foreach(var p in pointId)
                    {
                        if(p.AsInt(0)>0)
                        {
                            if (count ==0)
                            {
                                s += p;
                            }
                            else
                            {
                                s += ',' + p;
                            }
                            Point.Add(p.AsInt());
                            count++;
                        }
                    }
                    if (count > 0)
                        sql += " and PointId in (" + s + ")";
                    // query = query.Where(a => a.PointId == request.PointID);
                    //  url = url.Replace("ID=0", "ID=" + request.PointID.ToString());
                }
                if (request.PositionID != 0)
                {
                    sql += " and b.PositionId=" + request.PositionID.ToString();
                    //query = query.Where(a => a.PositionID == request.PositionID);
                }
                //if (request.DeviceID != 0)
                //{
                //    sql += " and b.DeviceId=" + request.DeviceID.ToString();
                //    // query = query.Where(a => a.DeviceId == request.DeviceID);

                //}
                if (request.LayerID != 0)
                {
                    sql += " and c.LayerId=" + request.LayerID.ToString();
                    //query = query.Where(a => a.LayerId == request.LayerID);
                    // url = url.Replace("LayerId=0", "LayerId=" + request.LayerID.ToString());
                }
                //if (request.ScanTypeID != 0)
                //{
                //    sql += " and b.ScanTypeId=" + request.ScanTypeID.ToString();
                //    // query = query.Where(a => a.ScanTypeId == request.ScanTypeID);
                //    //url = url.Replace("TypeId=0", "TypeId=" + request.ScanTypeID.ToString());
                //    //var type = service.MK_ScanType.FirstOrDefault(u => u.ID == request.ScanTypeID);
                //    //if (type != null)
                //    //{
                //    //    if (type.IsCommand != 3)
                //    //    {
                //    //        ret.value = 1;
                //    //        ret.Url = url;
                //    //        ret.Success = true;
                //    //        return Json(ret, JsonRequestBehavior.DenyGet);
                //    //    }
                //    //    //Response.Redirect(url);

                //    //}
                //}
                if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                    {
                    if (!request.Begin.Contains(" "))
                    {
                        request.Begin += " 00:00:00";
                    }
                    if (!request.End.Contains(" "))
                    {
                        request.End += " 23:59:59";
                    }
                    //DateTime BeginTime = Convert.ToDateTime(request.Begin);
                    //    DateTime EndTime = Convert.ToDateTime(request.End);
                    sql += " and a.CreateDate>'" + request.Begin + "' and a.CreateDate<'" + request.End+"'";
                    //query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                }

                    //int Count = query.Count();
                    //ret.TotalCount = Count;
                    //ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                    ////如果不是第一页，并且没有数据，则查询第一页
                    //var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    //if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                    //{
                    //    request.PageIndex = 1;
                    //    CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    //}

                    List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();
                IEnumerable<FD_Dynamic> query = service.ExecuteQuery<FD_Dynamic>(sql).ToList();
                var currentquery = query.ToList();
                    currentquery.ForEach(a =>
                    {
                        ConstructAutoResponse se = new ConstructAutoResponse();
                        se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                       // se.DeviceName = a.DeviceName;
                        se.LayerName = service.MK_Layer.FirstOrDefault(u => u.ID == a.LayerId).LayerName;
                        se.PointName = a.PointName;
                        se.PositionName = service.MK_Position.FirstOrDefault(u => u.ID == a.PositionId).PositionName;
                        se.ScanTypeName = service.MK_ScanType.FirstOrDefault(u => u.ID == a.ScanTypeId).TypeName;
                        se.ScanValue = a.ScanValue.ToString("F4");
                       // se.ScanValue2 = a.YXValue.ToString("F4");

                        list.Add(se);
                    });



                    var echart = GetEchartDynamic2(currentquery);

                    ret.Data = list;
                    ret.Echart = echart;
                    ret.PageIndex = request.PageIndex;
                    ret.Success = true;
                //    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
                //}


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DynamicLoadQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{
                    //var point = service.MK_Point.FirstOrDefault(u => u.ID == request.PointID);
                    //if (point != null)
                    //{
                    //    request.DeviceID = point.DeviceId ?? 0;
                    //}
                      DateTime beginDate = getBeginDate(request.Begin.AsDatetime(DateTime.Now));
                      DateTime endDate = getEndDate(request.End.AsDatetime(DateTime.Now));
                    string DBName = "FD_DynamicValue2" + beginDate.ToString("yyyyMM");
                    var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                    if (Exist.ID == 0)
                    {
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                if (!request.Begin.Contains(" "))
                {
                    request.Begin += " 00:00:00";
                }
                if (!request.End.Contains(" "))
                {
                    request.End += " 23:59:59";
                }
                //DateTime BeginTime = Convert.ToDateTime(request.Begin);
                //DateTime EndTime = Convert.ToDateTime(request.End.AsDatetime(DateTime.Now).AddDays(1));
                string sql = string.Format("select a.ID,PointId,b.PositionId,b.DeviceId,d.DeviceName,a.CreateDate,ScanValue,Result,b.PointName,c.LayerId,e.ScanTypeId from {0} a inner join MK_Point b on a.PointId=b.ID inner join MK_Position c on b.PositionId=c.ID inner join MK_Device d on b.DeviceId=d.ID inner join MK_Layer e on c.LayerId=e.ID where a.CreateDate>'{1}' and a.CreateDate<'{2}'", DBName, request.Begin, request.End);

                //var query = service.View_MK_AutoScan.Where(a => a.ID > 0 && a.PointId == request.PointID);
                var PointId = request.PointID.Split(',').ToList();
                List<int> Point = new List<int>();
                string s = "";
                int count = 0;
                foreach (var str in PointId)
                {
                    if (PointId.AsInt(0) > 0)
                    {
                        Point.Add(PointId.AsInt());
                        if (count == 0)
                        {
                            s += str;
                        }
                        else
                        {
                            s += "," + str;
                        }
                        count++;
                    }
                    
                }

                if (Point.Count>0)
                {

                    // int Count = 0;

                    sql += " and PointId in (" + s + ")";
                    // query = query.Where(a => a.PointId == request.PointID);
                    //  url = url.Replace("ID=0", "ID=" + request.PointID.ToString());
                }
                //if (request.PositionID != 0)
                //{
                //    query = query.Where(a => a.PositionID == request.PositionID);
                //}
                if (request.DeviceID != 0)
                {
                    sql += " and b.DeviceId=" + request.DeviceID.ToString();
                    // query = query.Where(a => a.DeviceId == request.DeviceID);

                }
                if (request.LayerID != 0)
                {
                    sql += " and c.LayerId=" + request.LayerID.ToString();
                    //query = query.Where(a => a.LayerId == request.LayerID);
                    // url = url.Replace("LayerId=0", "LayerId=" + request.LayerID.ToString());
                }
                //if (request.ScanTypeID != 0)
                //{
                //    sql += " and b.ScanTypeId=" + request.ScanTypeID.ToString();
                //    // query = query.Where(a => a.ScanTypeId == request.ScanTypeID);
                //    //url = url.Replace("TypeId=0", "TypeId=" + request.ScanTypeID.ToString());
                //    //var type = service.MK_ScanType.FirstOrDefault(u => u.ID == request.ScanTypeID);
                //    //if (type != null)
                //    //{
                //    //    if (type.IsCommand != 3)
                //    //    {
                //    //        ret.value = 1;
                //    //        ret.Url = url;
                //    //        ret.Success = true;
                //    //        return Json(ret, JsonRequestBehavior.DenyGet);
                //    //    }
                //    //    //Response.Redirect(url);

                //    //}
                //}
                if (request.PositionID != 0)
                {
                    sql += " and b.PositionId=" + request.PositionID.ToString();
                    //query = query.Where(a => a.PositionId == request.PositionID);
                }
                //if (request.PointID != 0)
                //{
                //    query = query.Where(a => a.PointId == request.PointID);
                //}
                //if (request.PositionID != 0)
                //{
                //    query = query.Where(a => a.PositionId == request.PositionID);
                //}
                //if (request.DeviceID != 0)
                //{
                //    query = query.Where(a => a.DeviceId == request.DeviceID);
                //}
                //if (request.LayerID != 0)
                //{
                //    query = query.Where(a => a.LayerId == request.LayerID);
                //}
                //if (request.ScanTypeID != 0)
                //{
                //    query = query.Where(a => a.ScanTypeId == request.ScanTypeID);
                //}
                //if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                //{
                //    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                //    DateTime EndTime = Convert.ToDateTime(request.End);
                //    query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                //}
                var query = service.ExecuteQuery<FD_Dynamic>(sql).ToList();
                int Count = query.Count();
                    ret.TotalCount = Count;
                    ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                    //如果不是第一页，并且没有数据，则查询第一页
                    var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                    {
                        request.PageIndex = 1;
                        CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    }

                    List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();

                    CurrentQuery.ForEach(a =>
                    {
                        ConstructAutoResponse se = new ConstructAutoResponse();
                        se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                        //se.DeviceName = a.DeviceName;
                        se.LayerName = service.MK_Layer.FirstOrDefault(u=>u.ID==a.LayerId).LayerName;
                        se.PointName = a.PointName;
                        se.PositionName = service.MK_Position.FirstOrDefault(u=>u.ID==a.PositionId).PositionName;
                        se.ScanTypeName = service.MK_ScanType.FirstOrDefault(u=>u.ID==a.ScanTypeId).TypeName;
                        se.ScanValue = a.ScanValue.ToString("F4");
                        se.ScanValue2 = a.Result.ToString("F4");
                        list.Add(se);
                    });



                    var echart = GetEchartDynamic(query.ToList());

                    ret.Data = list;
                    ret.Echart = echart;
                    ret.PageIndex = request.PageIndex;
                    ret.Success = true;
                //    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
                //}


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }



        [HttpPost]
        public JsonResult DynamicLoadChart(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{

                    //var point = service.MK_Point.FirstOrDefault(u => u.ID == request.PointID);
                    //if (point != null)
                    //{
                    //    request.DeviceID = point.DeviceId ?? 0;
                    //}
                    DateTime beginDate = getBeginDate(request.Begin.AsDatetime(DateTime.Now));
                    DateTime endDate = getEndDate(request.End.AsDatetime(DateTime.Now));
                    string DBName = "FD_DynamicValue2" + beginDate.ToString("yyyyMM");

                    var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                    if (Exist.ID == 0)
                    {
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                if (!request.Begin.Contains(" "))
                {
                    request.Begin += " 00:00:00";
                }
                if (!request.End.Contains(" "))
                {
                    request.End += " 23:59:59";
                }
                string sql = string.Format("select a.ID,PointId,b.PositionId,b.DeviceId,d.DeviceName,a.CreateDate,ScanValue,Result,b.PointName,c.LayerId from {0} a inner join MK_Point b on a.PointId=b.ID inner join MK_Position c on b.PositionId=c.ID inner join MK_Device d on b.DeviceId=d.ID where a.CreateDate>'{1}' and a.CreateDate<'{2}'", DBName, request.Begin, request.End);
                    IEnumerable<FD_Dynamic> query = service.ExecuteQuery<FD_Dynamic>(sql);
                    //var query = service.View_MK_AutoScan.Where(a => a.ID > 0 && a.PointId == request.PointID);

                    //if (request.PointID != 0)
                    //{
                    //    query = query.Where(a => a.PointId == request.PointID);
                    //}
                    //if (request.PositionID != 0)
                    //{
                    //    query = query.Where(a => a.PositionID == request.PositionID);
                    //}
                    //if (request.DeviceID != 0)
                    //{
                    //    query = query.Where(a => a.DeviceID == request.DeviceID);
                    //}
                    //if (request.LayerID != 0)
                    //{
                    //    query = query.Where(a => a.LayerID == request.LayerID);
                    //}
                    //if (request.ScanTypeID != 0)
                    //{
                    //    query = query.Where(a => a.ScanTypeID == request.ScanTypeID);
                    //}
                    //if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                    //{
                    //    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                    //    DateTime EndTime = Convert.ToDateTime(request.End);
                    //    query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                    //}

                    int Count = query.Count();
                    ret.TotalCount = Count;
                    ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                    //如果不是第一页，并且没有数据，则查询第一页
                    var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                    {
                        request.PageIndex = 1;
                        CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    }

                    List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();

                    CurrentQuery.ForEach(a =>
                    {
                        ConstructAutoResponse se = new ConstructAutoResponse();
                        se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                       // se.DeviceName = a.DeviceName;
                        //  se.LayerName = a.LayerName;
                        se.PointName = a.PointName;
                        //se.PositionName = a.PositionName;
                        //se.ScanTypeName = a.TypeName;
                        se.ScanValue = a.ScanValue.ToString("F4");
                        //se.ScanValue2 = a.YXValue.ToString("F4");
                        list.Add(se);
                    });



                    var echart = GetEchartDynamic2(query.ToList());

                    ret.Data = list;
                    ret.Echart = echart;
                    ret.PageIndex = request.PageIndex;
                    ret.Success = true;
                //    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
                //}


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }




      


        [HttpPost]
        public JsonResult AutoCalLoadQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                //string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                //var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                //if (CacheData != null)
                //{
                //    ret = CacheData as BaseReturn;

                //}
                //else
                //{
                var PointId = request.PointID.Split(',').ToList();
                List<int> Point = new List<int>();
                foreach (var str in PointId)
                {
                    if (str.AsInt(0) > 0)
                        Point.Add(str.AsInt());
                }
                if(Point.Count>0)
                {
                    var point = service.MK_Point.FirstOrDefault(u => u.ID == Point[0]);
                    if (point != null)
                    {
                        request.DeviceID = point.DeviceId ?? 0;
                    }
                }
                //var point = service.MK_Point.FirstOrDefault(u => );
                //if (point != null)
                //{
                //    request.DeviceID = point.DeviceId ?? 0;
                //}
                var query = service.View_MK_AutoScan.Where(a => a.ID > 0);

                if (Point.Count > 0)
                {
                    query = query.Where(a => Point.Contains(a.PointId));
                }
                if (request.PositionID != 0)
                {
                    query = query.Where(a => a.PositionID == request.PositionID);
                }
                //if (request.DeviceID != 0)
                //{
                //    query = query.Where(a => a.DeviceID == request.DeviceID);
                //}
                if (request.LayerID != 0)
                {
                    query = query.Where(a => a.LayerID == request.LayerID);
                }
                if (request.ScanTypeID != 0)
                {
                    query = query.Where(a => a.ScanTypeID == request.ScanTypeID);
                }
                if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                {
                    if (!request.Begin.Contains(" "))
                    {
                        request.Begin += " 00:00:00";
                    }
                    if (!request.End.Contains(" "))
                    {
                        request.End += " 23:59:59";
                    }
                    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                    DateTime EndTime = Convert.ToDateTime(request.End);
                    query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                }

                int Count = query.Count();
                    ret.TotalCount = Count;
                    ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                    //如果不是第一页，并且没有数据，则查询第一页
                    var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                    {
                        request.PageIndex = 1;
                        CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    }

                    List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();

                    CurrentQuery.ForEach(a =>
                    {
                        ConstructAutoResponse se = new ConstructAutoResponse();
                        se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                        se.DeviceName = a.DeviceName;
                        se.LayerName = a.LayerName;
                        se.PointName = a.PointName;
                        se.PositionName = a.PositionName;
                        se.ScanTypeName = a.TypeName;
                        se.ScanValue = a.ScanValue.HasValue ? a.ScanValue.Value.ToString("#0.0000") : "";
                        se.ScanValue2 = a.ScanValue2.HasValue ? a.ScanValue2.Value.ToString("#0.0000") : "";
                        se.ScanValue3 = a.ScanValue3.HasValue ? a.ScanValue3.Value.ToString("#0.0000") : "";
                        se.ScanValue4 = a.ScanValue4.HasValue ? a.ScanValue4.Value.ToString("#0.0000") : "";
                        se.ScanValue5 = a.ScanValue5.HasValue ? a.ScanValue5.Value.ToString("#0.0000") : "";
                        se.ScanValue6 = a.ScanValue6.HasValue ? a.ScanValue6.Value.ToString("#0.0000") : "";
                        list.Add(se);
                    });



                    var echart = GetEchartData2(request.DeviceID, Point, request.Begin, request.End);

                    ret.Data = list;
                    ret.Echart = echart;
                    ret.PageIndex = request.PageIndex;
                    ret.Success = true;
                //    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
                //}


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }



        public LineEchart GetEchartDynamic(List<FD_Dynamic> TempQuery)
        {
            LineEchart info = new LineEchart();
            info.title.text = "最大值";

            //拿出所有数据
            //var TempQuery = service.View_MK_AutoScan.Where(a => a.ID > 0);
            //if (DeviceID != 0)
            //{
            //    TempQuery = TempQuery.Where(a => a.DeviceID == DeviceID);
            //}
            //if (PointID != 0)
            //{
            //    TempQuery = TempQuery.Where(a => a.PointId == PointID);
            //}
            //if (!string.IsNullOrEmpty(Begin) && !string.IsNullOrEmpty(End))
            //{
            //    DateTime dtBegin = Convert.ToDateTime(Begin);
            //    DateTime dtEnd = Convert.ToDateTime(End);
            //    TempQuery = TempQuery.Where(a => a.CreateDate >= dtBegin && a.CreateDate <= dtEnd);
            //}

            var FirstData = TempQuery.OrderByDescending(a => a.CreateDate).FirstOrDefault();

            //设备数据
            var DeviceData = TempQuery.Where(a => a.DeviceId == FirstData.DeviceId).OrderByDescending(a => a.CreateDate);

            var PointGroup = DeviceData.GroupBy(a => a.PointId).Select(group => new
            {
                PointID = group.FirstOrDefault().PointId,
                PointName = group.FirstOrDefault().PointName
            });

            int count = 1;
            if (PointGroup == null)
            {
                return info;
            }


            foreach (var Point in PointGroup)
            {
                var list = DeviceData.Where(a => a.PointId == Point.PointID).OrderBy(a => a.CreateDate).ToList();

                series Serie = new series()
                {
                    name = Point.PointName,
                    smooth=true,
                    type = "line"
                  
                };

                foreach (var item in list)
                {
                    if (count == 1)
                    {
                        info.xAxis.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm:ss"));
                    }

                    //info.xAxis.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm"));
                   // Serie.data.Add(Math.Round(Convert.ToDecimal(item.MAXValue), 4));

                    //Serie.data.Add(Math.Round(Convert.ToDecimal(100 + count), 2));
                }
                info.legend.data.Add(Point.PointName);
                info.series.Add(Serie);
                count++;
            }


            return info;
        }


        public LineEchart GetEchartDynamic2(List<FD_Dynamic> TempQuery)
        {
            LineEchart info = new LineEchart();

            info.title.text = "有效值";
            //拿出所有数据
            //var TempQuery = service.View_MK_AutoScan.Where(a => a.ID > 0);
            //if (DeviceID != 0)
            //{
            //    TempQuery = TempQuery.Where(a => a.DeviceID == DeviceID);
            //}
            //if (PointID != 0)
            //{
            //    TempQuery = TempQuery.Where(a => a.PointId == PointID);
            //}
            //if (!string.IsNullOrEmpty(Begin) && !string.IsNullOrEmpty(End))
            //{
            //    DateTime dtBegin = Convert.ToDateTime(Begin);
            //    DateTime dtEnd = Convert.ToDateTime(End);
            //    TempQuery = TempQuery.Where(a => a.CreateDate >= dtBegin && a.CreateDate <= dtEnd);
            //}

            var FirstData = TempQuery.OrderByDescending(a => a.CreateDate).FirstOrDefault();

            //设备数据
            var DeviceData = TempQuery.Where(a => a.DeviceId == FirstData.DeviceId).OrderByDescending(a => a.CreateDate);

            var PointGroup = DeviceData.GroupBy(a => a.PointId).Select(group => new
            {
                PointID = group.FirstOrDefault().PointId,
                PointName = group.FirstOrDefault().PointName
            });

            int count = 1;
            if (PointGroup == null)
            {
                return info;
            }
            foreach (var Point in PointGroup)
            {
                var list = DeviceData.Where(a => a.PointId == Point.PointID).OrderBy(a => a.CreateDate).ToList();

                series Serie = new series()
                {
                    name = Point.PointName,
                    smooth = true,
                    type = "line"
                };

                foreach (var item in list)
                {
                    if (count == 1)
                    {
                        info.xAxis.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm:ss"));
                    }

                    //info.xAxis.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm"));
                    Serie.data.Add(Math.Round(Convert.ToDecimal(item.ScanValue), 4));

                    //Serie.data.Add(Math.Round(Convert.ToDecimal(100 + count), 2));
                }
                info.legend.data.Add(Point.PointName);
                info.series.Add(Serie);
                count++;
            }


            return info;
        }


        [HttpPost]
        public JsonResult AutoCalChartLoadQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {

                string ConstrustAutoQueryCacheKey = CacheKey.车辆进出数据缓存 + "_param=" + request.PointID + "-" + request.PositionID + "-" + request.DeviceID + "-" + request.LayerID + "-" + request.ScanTypeID + "-" + request.Begin + "-" + request.End + "-" + request.PageIndex + "-" + request.PageSize;

                var CacheData = CacheUtils.Get(ConstrustAutoQueryCacheKey);

                if (CacheData != null)
                {
                    ret = CacheData as BaseReturn;

                }
                else
                {
                    //var point = service.MK_Point.FirstOrDefault(u => u.ID == request.PointID);
                    //if (point != null)
                    //{
                    //    request.DeviceID = point.DeviceId ?? 0;
                    //}
                    //var query = service.View_MK_AutoScan.Where(a => a.ID > 0);

                    //if (request.PointID != 0)
                    //{
                    //    query = query.Where(a => a.PointId == request.PointID);
                    //}
                    //if (request.PositionID != 0)
                    //{
                    //    query = query.Where(a => a.PositionID == request.PositionID);
                    //}
                    //if (request.DeviceID != 0)
                    //{
                    //    query = query.Where(a => a.DeviceID == request.DeviceID);
                    //}
                    //if (request.LayerID != 0)
                    //{
                    //    query = query.Where(a => a.LayerID == request.LayerID);
                    //}
                    //if (request.ScanTypeID != 0)
                    //{
                    //    query = query.Where(a => a.ScanTypeID == request.ScanTypeID);
                    //}
                    //if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                    //{
                    //    DateTime BeginTime = Convert.ToDateTime(request.Begin);
                    //    DateTime EndTime = Convert.ToDateTime(request.End);
                    //    query = query.Where(a => a.CreateDate >= BeginTime && a.CreateDate <= EndTime).AsQueryable();
                    //}

                    //int Count = query.Count();
                    //ret.TotalCount = Count;
                    //ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                    ////如果不是第一页，并且没有数据，则查询第一页
                    //var CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    //if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                    //{
                    //    request.PageIndex = 1;
                    //    CurrentQuery = query.OrderByDescending(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                    //}

                    //List<ConstructAutoResponse> list = new List<ConstructAutoResponse>();

                    //CurrentQuery.ForEach(a =>
                    //{
                    //    ConstructAutoResponse se = new ConstructAutoResponse();
                    //    se.CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                    //    se.DeviceName = a.DeviceName;
                    //    se.LayerName = a.LayerName;
                    //    se.PointName = a.PointName;
                    //    se.PositionName = a.PositionName;
                    //    se.ScanTypeName = a.TypeName;
                    //    se.ScanValue = a.ScanValue.HasValue ? a.ScanValue.Value.ToString("#0.00") : "";
                    //    se.ScanValue2 = a.ScanValue2.HasValue ? a.ScanValue2.Value.ToString("#0.00") : "";
                    //    se.ScanValue3 = a.ScanValue3.HasValue ? a.ScanValue3.Value.ToString("#0.00") : "";
                    //    se.ScanValue4 = a.ScanValue4.HasValue ? a.ScanValue4.Value.ToString("#0.00") : "";
                    //    se.ScanValue5 = a.ScanValue5.HasValue ? a.ScanValue5.Value.ToString("#0.00") : "";
                    //    se.ScanValue6 = a.ScanValue6.HasValue ? a.ScanValue6.Value.ToString("#0.00") : "";
                    //    list.Add(se);
                    //});

                    var PointId = request.PointID.Split(',').ToList();
                    List<int> Point = new List<int>();
                    foreach (var str in PointId)
                    {
                        if (str.AsInt(0) > 0)
                            Point.Add(PointId.AsInt());
                    }

                    var echart = GetEchartData3(request.DeviceID, Point, request.Begin, request.End);

                   // ret.Data = list;
                    ret.Echart = echart;
                 //   ret.PageIndex = request.PageIndex;
                    ret.Success = true;
                    CacheUtils.Set(ConstrustAutoQueryCacheKey, ret, 60);
                }


            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        public LineEchart GetEchartData(int DeviceID, int PointID, string Begin, string End)
        {
            LineEchart info = new LineEchart();
           // info.xAxis.axisLine.symbol = "arrow";

            //拿出所有数据
            var TempQuery = service.View_MK_AutoScan.Where(a => a.ID > 0);
            if (DeviceID != 0)
            {
                TempQuery = TempQuery.Where(a => a.DeviceID == DeviceID);
            }
            if (PointID != 0)
            {
                TempQuery = TempQuery.Where(a => a.PointId == PointID);
            }
            if (!string.IsNullOrEmpty(Begin) && !string.IsNullOrEmpty(End))
            {
                if (!Begin.Contains(" "))
                {
                    Begin += " 00:00:00";
                }
                if (!End.Contains(" "))
                {
                    End += " 23:59:59";
                }
                DateTime dtBegin = Convert.ToDateTime(Begin);
                DateTime dtEnd = Convert.ToDateTime(End);
                TempQuery = TempQuery.Where(a => a.CreateDate >= dtBegin && a.CreateDate <= dtEnd);
            }

            var FirstData = TempQuery.OrderByDescending(a => a.CreateDate).FirstOrDefault();

            //设备数据
            var DeviceData = TempQuery.Where(a => a.DeviceID == FirstData.DeviceID).OrderByDescending(a => a.CreateDate);

            var PointGroup = DeviceData.GroupBy(a => a.PointId).Select(group => new
            {
                PointID = group.FirstOrDefault().PointId,
                PointName = group.FirstOrDefault().PointName
            });

            int count = 1;
            foreach (var Point in PointGroup)
            {
                var list = DeviceData.Where(a => a.PointId == Point.PointID).OrderBy(a => a.CreateDate).ToList();

                series Serie = new series()
                {
                    name = Point.PointName,
                    type = "line"
                };

                foreach (var item in list)
                {
                    if (count == 1)
                    {
                        info.xAxis.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm"));
                    }
                    // info.xAxis.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm"));
                    Serie.data.Add(Math.Round(Convert.ToDecimal(item.ScanValue), 2));

                    //Serie.data.Add(Math.Round(Convert.ToDecimal(100 + count), 2));
                }
                info.legend.data.Add(Point.PointName);
                info.series.Add(Serie);
                count++;
            }


            return info;
        }

        public LineEchart GetEchartData(List<View_MK_AutoScan> query,int type)
        {
            LineEchart info = new LineEchart();
           // info.xAxis.axisLine

            //拿出所有数据
            //var TempQuery = service.View_MK_AutoScan.Where(a=> a.ID > 0);
            //if (DeviceID != 0)
            //{
            //    TempQuery = TempQuery.Where(a => a.DeviceID == DeviceID);
            //}
            //if (PointID != 0)
            //{
            //    TempQuery = TempQuery.Where(a => a.PointId == PointID);
            //}
            //if (!string.IsNullOrEmpty(Begin) && !string.IsNullOrEmpty(End))
            //{
            //    DateTime dtBegin = Convert.ToDateTime(Begin);
            //    DateTime dtEnd = Convert.ToDateTime(End);
            //    TempQuery = TempQuery.Where(a => a.CreateDate >= dtBegin && a.CreateDate <= dtEnd);
            //}

            var FirstData = query.OrderByDescending(a => a.CreateDate).FirstOrDefault();

            //设备数据
            var DeviceData = query.Where(a => a.DeviceID == FirstData.DeviceID).OrderByDescending(a => a.CreateDate);

            var PointGroup = DeviceData.GroupBy(a => a.PointId).Select(group => new
            {
                PointID = group.FirstOrDefault().PointId,
                PointName = group.FirstOrDefault().PointName
            });

            int count = 1;
            foreach (var Point in PointGroup)
            {
                //var list = DeviceData.Where(a => a.PointId == Point.PointID).Take(10).OrderBy(a => a.CreateDate).ToList();
                var list = DeviceData.Where(a => a.PointId == Point.PointID).OrderBy(a => a.CreateDate).ToList();

                series Serie = new series()
                {
                    name = Point.PointName,
                    smooth = true,
                    type = "line"
                };

                foreach (var item in list)
                {
                    if (count == 1)
                    {
                        info.xAxis.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm"));
                    }

                    //info.xAxis.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm"));
                    if (type == 1)
                    {
                        Serie.data.Add(Math.Round(Convert.ToDecimal(item.ScanValue), 2));
                    }
                    else
                    {
                        Serie.data.Add(Math.Round(Convert.ToDecimal(item.ScanValue2), 2));
                    }

                    //Serie.data.Add(Math.Round(Convert.ToDecimal(100 + count), 2));
                }
                info.legend.data.Add(Point.PointName);
                info.series.Add(Serie);
                count++;
            }


            return info;
        }


        public LineEchart GetEchartData1(List<View_MK_AutoScan> query)
        {
            LineEchart info = new LineEchart();

            //拿出所有数据
            //var TempQuery = service.View_MK_AutoScan.Where(a => a.ID > 0);
            //if (DeviceID != 0)
            //{
            //    TempQuery = TempQuery.Where(a => a.DeviceID == DeviceID);
            //}
            //if (PointID != 0)
            //{
            //    TempQuery = TempQuery.Where(a => a.PointId == PointID);
            //}
            //if (!string.IsNullOrEmpty(Begin) && !string.IsNullOrEmpty(End))
            //{
            //    DateTime dtBegin = Convert.ToDateTime(Begin);
            //    DateTime dtEnd = Convert.ToDateTime(End);
            //    TempQuery = TempQuery.Where(a => a.CreateDate >= dtBegin && a.CreateDate <= dtEnd);
            //}

            var FirstData = query.OrderByDescending(a => a.CreateDate).FirstOrDefault();
            if (FirstData == null)
            {
                return info;
            }
            //设备数据
            var DeviceData = query.Where(a => a.DeviceID == FirstData.DeviceID).OrderByDescending(a => a.CreateDate);

            var PointGroup = DeviceData.GroupBy(a => a.PointId).Select(group => new
            {
                PointID = group.FirstOrDefault().PointId,
                PointName = group.FirstOrDefault().PointName
            });

            int count = 1;
            if(PointGroup==null)
            {
                return info;
            }
            foreach (var Point in PointGroup)
            {
                var list = DeviceData.Where(a => a.PointId == Point.PointID).Take(10).OrderBy(a => a.CreateDate).ToList();

                series Serie = new series()
                {
                    name = Point.PointName,
                    smooth = true,
                    type = "line"
                };

                foreach (var item in list)
                {
                    //if (count == 1)
                    //{
                    //    X.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm:ss"));
                    //}
                    if(count==1)
                    info.xAxis.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm"));
                    Serie.data.Add(Math.Round(Convert.ToDecimal(item.ScanValue), 2));

                    //Serie.data.Add(Math.Round(Convert.ToDecimal(100 + count), 2));
                }
                info.legend.data.Add(Point.PointName);
                info.series.Add(Serie);
                count++;
            }


            return info;
        }


        public LineEchart GetEchartData2(int DeviceID, List<int> PointID, string Begin, string End)
        {
            LineEchart info = new LineEchart();

            //拿出所有数据
            var TempQuery = service.View_MK_AutoScan.Where(a => a.ID > 0);
            if (DeviceID != 0)
            {
                TempQuery = TempQuery.Where(a => a.DeviceID == DeviceID);
            }
            if (PointID.Count>0)
            {
                TempQuery = TempQuery.Where(a => PointID.Contains(a.PointId));
            }
            if (!string.IsNullOrEmpty(Begin) && !string.IsNullOrEmpty(End))
            {
                if (!Begin.Contains(" "))
                {
                    Begin += " 00:00:00";
                }
                if (!End.Contains(" "))
                {
                    End += " 23:59:59";
                }
                DateTime dtBegin = Convert.ToDateTime(Begin);
                DateTime dtEnd = Convert.ToDateTime(End);
                TempQuery = TempQuery.Where(a => a.CreateDate >= dtBegin && a.CreateDate <= dtEnd);
            }

            var FirstData = TempQuery.OrderByDescending(a => a.CreateDate).FirstOrDefault();
            if (FirstData == null)
            {
                return info;
            }
            //设备数据
            var DeviceData = TempQuery.Where(a => a.DeviceID == FirstData.DeviceID).OrderByDescending(a => a.CreateDate).ToList();

            var PointGroup = DeviceData.GroupBy(a => a.PointId).Select(group => new
            {
                PointID = group.FirstOrDefault().PointId,
                PointName = group.FirstOrDefault().PointName
            });

            int count = 1;
            if (PointGroup == null)
            {
                return info;
            }
            int i = 0;
            foreach (var Point in PointGroup)
            {
                //var list = DeviceData.Where(a => a.PointId == Point.PointID).Take(10).OrderBy(a => a.CreateDate).ToList();
                var list = DeviceData.Where(a => a.PointId == Point.PointID).OrderBy(a => a.CreateDate).ToList();

                series Serie = new series()
                {
                    name = Point.PointName,
                    smooth = true,
                    type = "line"
                };

                foreach (var item in list)
                {
                    //if (count == 1)
                    //{
                    //    X.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm:ss"));
                    //}
                    if(i==0)
                    info.xAxis.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm"));
                    Serie.data.Add(Math.Round(Convert.ToDecimal(item.ScanValue5), 2));

                    //Serie.data.Add(Math.Round(Convert.ToDecimal(100 + count), 2));
                }
                i++;
                info.legend.data.Add(Point.PointName);
                info.series.Add(Serie);
                count++;
            }


            return info;
        }


        public LineEchart GetEchartData3(int DeviceID, List<int> PointID, string Begin, string End)
        {
            LineEchart info = new LineEchart();

            //拿出所有数据
            var TempQuery = service.View_MK_AutoScan.Where(a => a.ID > 0);
            if (DeviceID.AsInt(0) != 0)
            {
                TempQuery = TempQuery.Where(a => a.DeviceID == DeviceID);
            }
            if (PointID.Count > 0)
            {
                TempQuery = TempQuery.Where(a => PointID.Contains(a.PointId));
            }
            if (!string.IsNullOrEmpty(Begin) && !string.IsNullOrEmpty(End))
            {
                if (!Begin.Contains(" "))
                {
                    Begin += " 00:00:00";
                }
                if (!End.Contains(" "))
                {
                    End += " 23:59:59";
                }
                DateTime dtBegin = Convert.ToDateTime(Begin);
                DateTime dtEnd = Convert.ToDateTime(End);
                TempQuery = TempQuery.Where(a => a.CreateDate >= dtBegin && a.CreateDate <= dtEnd);
            }

            var FirstData = TempQuery.OrderByDescending(a => a.CreateDate).FirstOrDefault();
            if (FirstData == null)
            {
                return info;
            }
            //设备数据
            var DeviceData = TempQuery.Where(a => a.DeviceID == FirstData.DeviceID).OrderByDescending(a => a.CreateDate);

            var PointGroup = DeviceData.GroupBy(a => a.PointId).Select(group => new
            {
                PointID = group.FirstOrDefault().PointId,
                PointName = group.FirstOrDefault().PointName
            });

            int count = 1;
            if (PointGroup == null)
            {
                return info;
            }
            foreach (var Point in PointGroup)
            {
                var list = DeviceData.Where(a => a.PointId == Point.PointID).Take(10).OrderBy(a => a.CreateDate).ToList();

                series Serie = new series()
                {
                    name = Point.PointName,
                    smooth = true,
                    type = "line"
                };

                foreach (var item in list)
                {
                    //if (count == 1)
                    //{
                    //    X.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm:ss"));
                    //}

                    info.xAxis.data.Add(Convert.ToDateTime(item.CreateDate).ToString("yyyy-MM-dd HH:mm"));
                    Serie.data.Add(Math.Round(Convert.ToDecimal(item.ScanValue6), 2));

                    //Serie.data.Add(Math.Round(Convert.ToDecimal(100 + count), 2));
                }
                info.legend.data.Add(Point.PointName);
                info.series.Add(Serie);
                count++;
            }


            return info;
        }


        public JsonResult DeviceQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.LayerID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var DeviceList = service.MK_Device.Where(a => a.LayerId == request.LayerID && a.DeviceProtocol==0).ToList();
                ret.Data = DeviceList;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        public JsonResult DeviceDynamicQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.LayerID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var DeviceList = service.MK_Device.Where(a => a.LayerId == request.LayerID && a.DeviceProtocol == 1).ToList();
                ret.Data = DeviceList;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult PositionIndexQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.LayerID.AsInt(0) == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var PositionList = service.MK_Position.AsQueryable();
                if (request.LayerID.AsInt(0) > 0)
                {
                    PositionList = PositionList.Where(a => a.LayerId == request.LayerID);
                }
                ret.Data = PositionList.ToList();
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        public JsonResult PointIndexQuery(ConstructAutoRequest request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.PositionID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var PointList = service.View_MK_Point.AsQueryable();
                if(request.PositionID.AsInt(0)>0)
                {
                    PointList = PointList.Where(u=>u.PositionId==request.PositionID);
                }
                //if (request.DeviceID.AsInt(0) > 0)
                //{
                //    PointList = PointList.Where(u=>u.DeviceId==request.DeviceID);
                //}
                if (request.ScanTypeID.AsInt(0) > 0)
                {
                    PointList = PointList.Where(u=>u.ScanTypeId==request.ScanTypeID);
                }
                //if(request.DeviceType!=null)
                //{
                //    PointList = PointList.Where(u => u.DeviceProtocol == request.DeviceType);
                //}
                //var PointList = service.MK_Point.Where(a => a.PositionId == request.PositionID && a.DeviceId == request.DeviceID).ToList();
                ret.Data = PointList.ToList();
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        #endregion


        #region Layer

        [HttpPost]
        public JsonResult LayerQuery(ConstructCfgLayerRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var tempQuery = service.View_MK_Layer.AsQueryable();

                if (request.ProjectID != 0)
                {
                    tempQuery = tempQuery.Where(a => a.ScanTypeId == request.ProjectID).AsQueryable();
                }

                int Count = tempQuery.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                var CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                //如果不是第一页，并且没有数据，则查询第一页
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                List<ConstructCfgLayerResponse> list = new List<ConstructCfgLayerResponse>();

                CurrentQuery.ForEach(a =>
                {
                    list.Add(new ConstructCfgLayerResponse()
                    {
                        ID = a.ID,
                        LayerName = a.LayerName,
                        ProjectName = a.ProjectName,
                        CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss")
                    });
                });

                ret.Data = list;
                ret.Success = true;
                ret.PageIndex = request.PageIndex;

            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult LayerInfoQuery(ConstructCfgLayerRequest request)
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
        public JsonResult SaveLayer(ConstructCfgLayerResponse request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (string.IsNullOrEmpty(request.Type))
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (string.IsNullOrEmpty(request.LayerName))
                {
                    ret.Message = "区域名称不能为空";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (request.ProjectID == 0)
                {
                    ret.Message = "请选择所属项目";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                //修改用户
                if (request.Type.Equals("Modify"))
                {
                    var Layer = service.MK_Layer.FirstOrDefault(a => a.ID == request.ID);
                    if (Layer == null)
                    {
                        ret.Message = "信息出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }

                    Layer.LayerName = request.LayerName.Trim();
                    Layer.ProjectId = request.ProjectID;
                    Layer.ScanTypeId = request.ScanTypeID;
                    Layer.MapX = request.MapX;
                    Layer.MapY = request.MapY;
                    Layer.Description = request.Description;
                    Layer.CreateDate = DateTime.Now;
                    if (string.IsNullOrEmpty(Layer.BackgroundPath))
                    {
                        Layer.BackgroundPath = filename;
                    }
                    else if (!string.IsNullOrEmpty(filename))
                    {
                        Layer.BackgroundPath = filename;
                    }
                    filename = "";

                    if (string.IsNullOrEmpty(Layer.BackgroundPath2))
                    {
                        Layer.BackgroundPath2 = filename2;
                    }
                    else if (!string.IsNullOrEmpty(filename2))
                    {
                        Layer.BackgroundPath2 = filename2;
                    }
                    filename = "";

                    service.SubmitChanges();
                    ret.Success = true;

                }
                else if (request.Type.Equals("Add"))
                {
                    MK_Layer se = new MK_Layer();
                    se.CreateDate = DateTime.Now;
                    se.Description = request.Description;
                    se.LayerName = request.LayerName.Trim();
                    se.MapY = request.MapY;
                    se.MapX = request.MapX;
                    se.ProjectId = request.ProjectID;
                    se.ScanTypeId = request.ScanTypeID;
                    se.BackgroundPath = filename;
                    se.BackgroundPath2 = filename2;
                    service.MK_Layer.InsertOnSubmit(se);
                    filename = "";
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

        [HttpPost]


        public ContentResult UpLoad_Layer(HttpPostedFileBase fileData)
        {
            if (fileData != null && fileData.ContentLength > 0)
            {
                string fileSave = Server.MapPath("/UpLoad/");
                //获取文件的扩展名
                string extName = Path.GetExtension(fileData.FileName);
                //得到一个新的文件名称
                string newName = Guid.NewGuid().ToString() + extName;
                fileData.SaveAs(Path.Combine(fileSave, newName));
                filename = newName;
            }
            return Content("");
        }

        [HttpPost]


        public ContentResult UpLoad_Layer2(HttpPostedFileBase fileData)
        {
            if (fileData != null && fileData.ContentLength > 0)
            {
                string fileSave = Server.MapPath("/UpLoad/");
                //获取文件的扩展名
                string extName = Path.GetExtension(fileData.FileName);
                //得到一个新的文件名称
                string newName = Guid.NewGuid().ToString() + extName;
                fileData.SaveAs(Path.Combine(fileSave, newName));
                filename2 = newName;
            }
            return Content("");
        }

        [HttpPost]
        public JsonResult DeleteLayer(ConstructCfgLayerResponse request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var se = service.MK_Layer.Where(a => a.ID == request.ID).FirstOrDefault();
                if (se == null)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                service.MK_Layer.DeleteOnSubmit(se);
                service.SubmitChanges();

                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region Position
        [HttpPost]
        public JsonResult PositionQuery(ConstructCfgPositionRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var tempQuery = service.View_MK_Position.AsQueryable();

                //if (request.ProjectID != 0)
                //{
                //    tempQuery = tempQuery.Where(a => a.ProjectID == request.ProjectID).AsQueryable();
                //}
                if(request.ScanTypeID!=0)
                {
                    tempQuery = tempQuery.Where(a => a.ScanTypeId == request.ProjectID).AsQueryable();
                }
                if (request.LayerID != 0)
                {
                    tempQuery = tempQuery.Where(a => a.LayerID == request.LayerID).AsQueryable();
                }
                //if (request.ScanTypeID != 0)
                //{
                //    tempQuery = tempQuery.Where(a => a.ScanTypeID == request.ScanTypeID).AsQueryable();
                //}

                int Count = tempQuery.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                var CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                //如果不是第一页，并且没有数据，则查询第一页
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                List<ConstructCfgViewPositionResponse> list = new List<ConstructCfgViewPositionResponse>();

                CurrentQuery.ForEach(a =>
                {
                    list.Add(new ConstructCfgViewPositionResponse()
                    {
                        ID = a.ID,
                        LayerName = a.LayerName,
                        ProjectName = a.ProjectName,
                        PositionName = a.PositionName,
                        CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss")
                    });
                });

                ret.Data = list;
                ret.Success = true;
                ret.PageIndex = request.PageIndex;

            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult PositionInfoQuery(ConstructCfgPositionRequest request)
        {
            BaseReturn ret = new BaseReturn();

            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            try
            {
                var PositionInfo = service.MK_Position.Where(a => a.ID == request.ID).FirstOrDefault();
                if (PositionInfo != null)
                {
                    ret.Data = PositionInfo;

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
        public JsonResult SavePosition(ConstructCfgPositionResponse request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (string.IsNullOrEmpty(request.Type))
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (string.IsNullOrEmpty(request.PositionName))
                {
                    ret.Message = "位置名称不能为空";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (request.LayerId == 0)
                {
                    ret.Message = "请选择监测区域";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                //if (request.ScanTypeId == 0)
                //{
                //    ret.Message = "请选择监测类型";
                //    return Json(ret, JsonRequestBehavior.DenyGet);
                //}

                //修改用户
                if (request.Type.Equals("Modify"))
                {
                    var Position = service.MK_Position.Where(a => a.ID == request.ID).FirstOrDefault();
                    if (Position == null)
                    {
                        ret.Message = "信息出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }

                    Position.PositionName = request.PositionName.Trim();
                    Position.LayerId = request.LayerId;
                  //  Position.ScanTypeId = request.ScanTypeId;
                    Position.MapX = request.MapX;
                    Position.MapY = request.MapY;
                    //Position.width = request.Width;
                    //Position.height = request.Height;
                    Position.PileDiameter = request.PileDiameter;
                    Position.PileLength = request.PileLength;
                    Position.PileNumber = request.PileNumber;
                    Position.ScanTypeId = request.ScanTypeId;
                    if(string.IsNullOrEmpty(Position.BackGroundPath))
                    Position.BackGroundPath = filename;
                    else if (!string.IsNullOrEmpty(filename))
                    {
                        Position.BackGroundPath = filename;
                    }
                    Position.CreateDate = DateTime.Now;

                    service.SubmitChanges();
                    ret.Success = true;

                }
                else if (request.Type.Equals("Add"))
                {
                    MK_Position se = new MK_Position();
                    se.CreateDate = DateTime.Now;
                    se.PositionName = request.PositionName.Trim();
                    se.LayerId = request.LayerId;
                    se.BackGroundPath = filename;
                   se.ScanTypeId = request.ScanTypeId;
                    se.MapX = request.MapX;
                    se.MapY = request.MapY;
                    se.PileDiameter = request.PileDiameter;
                    se.PileLength = request.PileLength;
                    se.PileNumber = request.PileNumber;
                    service.MK_Position.InsertOnSubmit(se);

                    service.SubmitChanges();
                    ret.Success = true;
                }
                

            }
            catch (Exception)
            {
                ret.Success = false;
            }
            filename = "";
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DeletePosition(ConstructCfgPositionResponse request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var se = service.MK_Position.Where(a => a.ID == request.ID).FirstOrDefault();
                if (se == null)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                service.MK_Position.DeleteOnSubmit(se);
                service.SubmitChanges();

                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region ScanType
        [HttpPost]
        public JsonResult ScanTypeQuery(ConstructCfgScanTypeRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var tempQuery = service.View_MK_ScanType.AsQueryable();

                if (request.ProjectID != 0)
                {
                    tempQuery = tempQuery.Where(a => a.ProjectId == request.ProjectID).AsQueryable();
                }

                if (request.IsCommand != 0)
                {
                    tempQuery = tempQuery.Where(a => a.IsCommand == request.IsCommand).AsQueryable();
                }

                int Count = tempQuery.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                var CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                //如果不是第一页，并且没有数据，则查询第一页
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                List<ConstructCfgScanTypeResponse> list = new List<ConstructCfgScanTypeResponse>();

                CurrentQuery.ForEach(a =>
                {
                    list.Add(new ConstructCfgScanTypeResponse()
                    {
                        ID = a.ID,
                        TypeName = a.TypeName,
                        ProjectName = a.ProjectName,
                        Protocol = a.Protocol,
                        CreateTime = a.CreateDate == null ? "" : Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss")
                    });
                });

                ret.Data = list;
                ret.Success = true;
                ret.PageIndex = request.PageIndex;

            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult ScanTypeInfoQuery(ConstructCfgScanTypeRequest request)
        {
            BaseReturn ret = new BaseReturn();

            if (request.ScanTypeID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            try
            {
                var ScanTypeInfo = service.MK_ScanType.Where(a => a.ID == request.ScanTypeID).FirstOrDefault();
                if (ScanTypeInfo != null)
                {
                    ret.Data = ScanTypeInfo;
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
        public JsonResult SaveScanType(ConstructCfgScanTypeResponse request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (string.IsNullOrEmpty(request.TypeName))
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }


                if (request.ProjectId == 0)
                {
                    ret.Message = "请选择所属项目";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (request.IsCommand == 0)
                {
                    ret.Message = "请选择协议";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                //修改用户
                if (request.Type.Equals("Modify"))
                {
                    var ScanType = service.MK_ScanType.Where(a => a.ID == request.ID).FirstOrDefault();
                    if (ScanType == null)
                    {
                        ret.Message = "信息出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }

                    ScanType.TypeName = request.TypeName.Trim();
                    ScanType.ProjectId = request.ProjectId;
                    ScanType.Description = request.Description;
                    ScanType.ImagePath = request.ImagePath;
                    ScanType.IsCommand = request.IsCommand;
                    ScanType.CreateDate = DateTime.Now;

                    service.SubmitChanges();
                    ret.Success = true;

                }
                else if (request.Type.Equals("Add"))
                {
                    MK_ScanType se = new MK_ScanType();
                    se.CreateDate = DateTime.Now;
                    se.Description = request.Description;
                    se.ImagePath = request.ImagePath;
                    se.TypeName = request.TypeName.Trim();
                    se.ProjectId = request.ProjectId;
                    se.IsCommand = request.IsCommand;

                    service.MK_ScanType.InsertOnSubmit(se);

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

        [HttpPost]
        public JsonResult DeleteScanType(ConstructCfgScanTypeResponse request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var se = service.MK_ScanType.Where(a => a.ID == request.ID).FirstOrDefault();
                if (se == null)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                service.MK_ScanType.DeleteOnSubmit(se);
                service.SubmitChanges();

                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region Protocol
        [HttpPost]
        public JsonResult ProtocolQuery(ConstructCfgProtocolRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var tempQuery = service.MK_Protocol.Where(a => !a.IsDelete.Value).AsQueryable();

                if (!string.IsNullOrEmpty(request.Protocol))
                {
                    tempQuery = tempQuery.Where(a => a.Protocol.Equals(request.Protocol.Trim())).AsQueryable();
                }


                int Count = tempQuery.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                var CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                //如果不是第一页，并且没有数据，则查询第一页
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                List<ConstructCfgProtocolResponse> list = new List<ConstructCfgProtocolResponse>();

                CurrentQuery.ForEach(a =>
                {
                    list.Add(new ConstructCfgProtocolResponse()
                    {
                        ID = a.ID,
                        Protocol = a.Protocol,
                        CreateTime = a.CreateTime == null ? "" : Convert.ToDateTime(a.CreateTime).ToString("yyyy-MM-dd HH:mm:ss")
                    });
                });

                ret.Data = list;
                ret.Success = true;
                ret.PageIndex = request.PageIndex;

            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult ProtocolInfoQuery(ConstructCfgProtocolRequest request)
        {
            BaseReturn ret = new BaseReturn();

            if (request.ProtocolID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            try
            {
                var ProtocolInfo = service.MK_Protocol.Where(a => !a.IsDelete.Value && a.ID == request.ProtocolID).FirstOrDefault();
                if (ProtocolInfo != null)
                {
                    ret.Data = ProtocolInfo;
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
        public JsonResult SaveProtocol(ConstructCfgProtocolResponse request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (string.IsNullOrEmpty(request.Protocol))
                {
                    ret.Message = "请输入协议名称";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                //修改用户
                if (request.Type.Equals("Modify"))
                {
                    var Protocol = service.MK_Protocol.Where(a => !a.IsDelete.Value && a.ID == request.ID).FirstOrDefault();
                    if (Protocol == null)
                    {
                        ret.Message = "信息出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }

                    Protocol.Protocol = request.Protocol.Trim();
                    Protocol.CreateTime = DateTime.Now;

                    service.SubmitChanges();
                    ret.Success = true;

                }
                else if (request.Type.Equals("Add"))
                {
                    MK_Protocol se = new MK_Protocol();
                    se.CreateTime = DateTime.Now;
                    se.IsDelete = false;
                    se.Protocol = request.Protocol;
                    service.MK_Protocol.InsertOnSubmit(se);

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

        [HttpPost]
        public JsonResult DeleteProtocol(ConstructCfgProtocolResponse request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var se = service.MK_Protocol.FirstOrDefault(a => !a.IsDelete.Value && a.ID == request.ID);
                if (se == null)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                se.IsDelete = true;

                service.SubmitChanges();

                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region Acquisition
        [HttpPost]
        public JsonResult ReadConfigSave(ConstructCfgAcquisitionRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var se = service.MK_AutoReadConfig.FirstOrDefault();
                if (se == null)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                se.BeginDate = Convert.ToDateTime(request.BeginDate);
                se.Interval = request.Interval;
                service.SubmitChanges();
                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region Point

        [HttpPost]
        public JsonResult PointQuery(ConstructCfgPointRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var tempQuery = service.View_MK_Point.AsQueryable();

                if (request.PositionID.AsInt(0) != 0)
                {
                    tempQuery = tempQuery.Where(a => a.PositionId == request.PositionID).AsQueryable();
                }
                if (request.DeviceID.AsInt(0) != 0)
                {
                    tempQuery = tempQuery.Where(a => a.DeviceId == request.DeviceID).AsQueryable();
                }
                if (request.ScanTypeID.AsInt(0)!=0)
                {
                    tempQuery = tempQuery.Where(u => u.ScanTypeId == request.ScanTypeID).AsQueryable();
                }

               if(request.DeviceType!=null)
                {
                    tempQuery = tempQuery.Where(u => u.DeviceProtocol == request.DeviceType).AsQueryable();
                }

                int Count = tempQuery.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                var CurrentQuery = tempQuery.OrderByDescending(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                //如果不是第一页，并且没有数据，则查询第一页
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = tempQuery.OrderByDescending(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }
                ret.Data = CurrentQuery;
                ret.Success = true;
                ret.PageIndex = request.PageIndex;

            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult PointInfoQuery(ConstructCfgPointRequest request)
        {
            BaseReturn ret = new BaseReturn();

            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            try
            {
                var PointInfo = service.MK_Point.FirstOrDefault(a => a.ID == request.ID);
                if (PointInfo != null)
                {
                    ret.Data = PointInfo;
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
        public JsonResult DeletePoint(ConstructCfgPointResponse request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var se = service.MK_Point.FirstOrDefault(a => a.ID == request.ID);
                if (se == null)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                service.MK_Point.DeleteOnSubmit(se);

                service.SubmitChanges();

                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult SavePoint(ConstructCfgPointResponse request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (string.IsNullOrEmpty(request.PointName))
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (request.PositionId == 0)
                {
                    ret.Message = "请选择测点位置";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (request.DeviceId == 0)
                {
                    ret.Message = "请选择监测设备";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                if (request.ScanTypeId == 0)
                {
                    ret.Message = "请选择监测设备";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                //修改用户
                if (request.Type.Equals("Modify"))
                {
                    var Point = service.MK_Point.FirstOrDefault(a => a.ID == request.ID);
                    if (Point == null)
                    {
                        ret.Message = "信息出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }

                    //反射封装
                    ObjectUtils.CopyObjectValue(request, Point, ObjectUtils.DefaultSkipList);
                    Point.CreateDate = DateTime.Now;
                    Point.MapX = request.MapX.AsStr("0");
                    Point.Para7 = request.Para7;
                    Point.Para6 = request.Para6;
                    Point.MapY = request.MapY.AsStr("0");
                    //Point.Para1=request.par

                    service.SubmitChanges();
                    ret.Success = true;

                }
                else if (request.Type.Equals("Add"))
                {
                    MK_Point se = new MK_Point();
                    ObjectUtils.CopyObjectValue(request, se, ObjectUtils.DefaultSkipList);
                    se.CreateDate = DateTime.Now;
                    se.MapX = request.MapX.AsStr("0");
                    se.MapY = request.MapY.AsStr("0");
                    
                    service.MK_Point.InsertOnSubmit(se);

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

        #endregion




        public FileResult Export()
        {
            try
            {
                int ExportDataCount = Convert.ToInt32(ConfigurationManager.AppSettings["ExportDataCount"]);

                string safeSql = @"select  PointName as '监测点',
                                    PositionName as '位置',
                                    DeviceName as '设备名',
                                    LayerName as '层级',
                                    TypeName as '类型',
                                    ScanValue as '检测值1',
                                    ScanValue2 as '监测值2',
                                    Convert(varchar(100),CreateDate,20) as '时间'
                                    from New_View_ConstructionAutoScan where 1 = 1 ";
                var begin = HttpContext.Request.QueryString["Begin"].Trim().AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 00:00:00";
                var end = HttpContext.Request.QueryString["End"].Trim().AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 23:59:59";
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["Begin"]) && !string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["End"]))
                {
                    safeSql += " and CreateDate > '" + begin + "' and CreateDate < '" + end + "' ";
                }
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["PointID"]) && !HttpContext.Request.QueryString["PointID"].Trim().Equals("0"))
                {
                    var pointId = HttpContext.Request.QueryString["PointID"].Split(',').ToList();
                    string point = "";
                    //List<int> Point = new List<int>();
                    int count = 0;
                    foreach (var p in pointId)
                    {
                        if (count > 0 && p.AsInt(0) > 0)
                        {
                            point += ",";
                        }
                        if (p.AsInt(0) > 0)
                        {
                            point += p;
                        }
                        count++;
                        // Point.Add(p.AsInt());
                    }
                    safeSql += " and PointId in (" + point + ") ";
                    //safeSql += " and PointId = " + HttpContext.Request.QueryString["PointID"].Trim().ToString();
                }
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["PositionID"]) && !HttpContext.Request.QueryString["PositionID"].Trim().Equals("0"))
                {
                    safeSql += " and PositionID = " + HttpContext.Request.QueryString["PositionID"].Trim().ToString();
                }
                //if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["DeviceID"]) && !HttpContext.Request.QueryString["DeviceID"].Trim().Equals("0"))
                //{
                //    safeSql += " and DeviceID = " + HttpContext.Request.QueryString["DeviceID"].Trim().ToString();
                //}
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["LayerID"]) && !HttpContext.Request.QueryString["LayerID"].Trim().Equals("0"))
                {
                    safeSql += " and LayerID = " + HttpContext.Request.QueryString["LayerID"].Trim().ToString();
                }
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["ScanTypeID"]) && !HttpContext.Request.QueryString["ScanTypeID"].Trim().Equals("0"))
                {
                    safeSql += " and ScanTypeID = " + HttpContext.Request.QueryString["ScanTypeID"].Trim().ToString();
                }
                safeSql += " order by CreateDate desc ";

                DataTable dt = SqlHelper.ExecuteDataTable(safeSql);

                var bytes = ExcelHelper.TableToBytes(dt, "施工监测数据");

                return File(bytes, "application/vnd.ms-excel", Server.UrlEncode("施工监测数据" + DateTime.Now.ToShortDateString() + ".xlsx"));
            }
            catch (Exception ex)
            {
                return null;
            }
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

        public FileResult Export1()
        {
            try
            {
                int ExportDataCount = Convert.ToInt32(ConfigurationManager.AppSettings["ExportDataCount"]);

                string safeSql = @"select  PointName as '监测点',
                                    PositionName as '位置',
                                    DeviceName as '设备名',
                                    LayerName as '层级',
                                    TypeName as '类型',
                                    ScanValue5 as '检测值1',
                                    ScanValue6 as '监测值2',
                                    Convert(varchar(100),CreateDate,20) as '时间'
                                    from New_View_ConstructionAutoScan where 1 = 1 ";
                var begin = HttpContext.Request.QueryString["Begin"].Trim().AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 00:00:00";
                var end= HttpContext.Request.QueryString["End"].Trim().AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 23:59:59";
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["Begin"]) && !string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["End"]))
                {
                    safeSql += " and CreateDate > '" + begin + "' and CreateDate < '" + end + "' ";
                }
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["PointID"]) && !HttpContext.Request.QueryString["PointID"].Trim().Equals("0"))
                {
                    var pointId = HttpContext.Request.QueryString["PointID"].Split(',').ToList();
                    string point = "";
                    //List<int> Point = new List<int>();
                    int count = 0;
                    foreach (var p in pointId)
                    {
                        if (count > 0 && p.AsInt(0) > 0)
                        {
                            point += ",";
                        }
                        if (p.AsInt(0) > 0)
                        {
                            point += p;
                        }
                        count++;
                        // Point.Add(p.AsInt());
                    }
                    safeSql += " and PointId in (" + point + ") ";
                   // safeSql += " and PointId = " + HttpContext.Request.QueryString["PointID"].Trim().ToString();
                }
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["PositionID"]) && !HttpContext.Request.QueryString["PositionID"].Trim().Equals("0"))
                {
                    safeSql += " and PositionID = " + HttpContext.Request.QueryString["PositionID"].Trim().ToString();
                }
                //if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["DeviceID"]) && !HttpContext.Request.QueryString["DeviceID"].Trim().Equals("0"))
                //{
                //    safeSql += " and DeviceID = " + HttpContext.Request.QueryString["DeviceID"].Trim().ToString();
                //}
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["LayerID"]) && !HttpContext.Request.QueryString["LayerID"].Trim().Equals("0"))
                {
                    safeSql += " and LayerID = " + HttpContext.Request.QueryString["LayerID"].Trim().ToString();
                }
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["ScanTypeID"]) && !HttpContext.Request.QueryString["ScanTypeID"].Trim().Equals("0"))
                {
                    safeSql += " and ScanTypeID = " + HttpContext.Request.QueryString["ScanTypeID"].Trim().ToString();
                }
                safeSql += " order by CreateDate desc ";

                DataTable dt = SqlHelper.ExecuteDataTable(safeSql);

                var bytes = ExcelHelper.TableToBytes(dt, "施工监测数据");

                return File(bytes, "application/vnd.ms-excel", Server.UrlEncode("施工监测数据" + DateTime.Now.ToShortDateString() + ".xlsx"));
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public FileResult Export_test()
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                //    int ExportDataCount = Convert.ToInt32(ConfigurationManager.AppSettings["ExportDataCount"]);

                //    string safeSql = @"select  PointName as '监测点',
                //                        PositionName as '位置',
                //                        DeviceName as '设备名',
                //                        LayerName as '层级',
                //                        TypeName as '类型',
                //                        ScanValue5 as '检测值1',
                //                        ScanValue6 as '监测值2',
                //                        Convert(varchar(100),CreateDate,20) as '时间'
                //                        from New_View_ConstructionAutoScan where 1 = 1 ";
                var begin = HttpContext.Request.QueryString["Begin"].Trim().AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 00:00:00";
                var end = HttpContext.Request.QueryString["End"].Trim().AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 23:59:59";
                //    if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["Begin"]) && !string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["End"]))
                //    {
                //        safeSql += " and CreateDate > '" + begin + "' and CreateDate < '" + end + "' ";
                //    }
                //    if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["PointID"]) && !HttpContext.Request.QueryString["PointID"].Trim().Equals("0"))
                //    {
                //        var pointId = HttpContext.Request.QueryString["PointID"].Split(',').ToList();
                //        string point = "";
                //        //List<int> Point = new List<int>();
                //        int count = 0;
                //        foreach (var p in pointId)
                //        {
                //            if (count > 0 && p.AsInt(0) > 0)
                //            {
                //                point += ",";
                //            }
                //            if (p.AsInt(0) > 0)
                //            {
                //                point += p;
                //            }
                //            count++;
                //            // Point.Add(p.AsInt());
                //        }
                //        safeSql += " and PointId in (" + point + ") ";
                //        // safeSql += " and PointId = " + HttpContext.Request.QueryString["PointID"].Trim().ToString();
                //    }

                //    safeSql += " order by CreateDate asc";

                //    DataTable dt = SqlHelper.ExecuteDataTable(safeSql);



                //    return File(bytes, "application/vnd.ms-excel", Server.UrlEncode("施工监测数据" + DateTime.Now.ToShortDateString() + ".xlsx"));
                //}
                //catch (Exception ex)
                //{
                //    return null;
                //}
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["PointID"]) && !HttpContext.Request.QueryString["PointID"].Trim().Equals("0"))
                {
                    var pointId = HttpContext.Request.QueryString["PointID"].Split(',').ToList();
                    var list = new List<MK_DataValue>();
                    var list1 = new List<MK_DataValue>()
;
                    // string point = "";
                    //List<int> Point = new List<int>();
                    if (pointId.Count > 1)
                    {
                        list = service.MK_DataValue.Where(u => u.PointId == pointId[0].AsInt(0) && u.CreateDate >= begin.AsDatetime(DateTime.Now) && u.CreateDate <= end.AsDatetime(DateTime.Now)).ToList();
                        list1 = service.MK_DataValue.Where(u => u.PointId == pointId[1].AsInt(0) && u.CreateDate >= begin.AsDatetime(DateTime.Now) && u.CreateDate <= end.AsDatetime(DateTime.Now)).ToList();
                    }
                    else
                    {
                        return null;
                    }



                    XSSFWorkbook xssfworkbook = new XSSFWorkbook();
                    ISheet sheet = xssfworkbook.CreateSheet("sheet1");

                    //数据
                    for (int i = 0; i < list.Count; i++)
                    {
                        IRow row1 = sheet.CreateRow(i);
                        ICell cell = row1.CreateCell(0);
                        cell.SetCellValue(list[i].CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        ICell cell2 = row1.CreateCell(1);
                        cell2.SetCellValue(list[i].ScanValue5.AsDouble(0).ToString("F4"));
                        //for (int j = 0; j < dt.Columns.Count; j++)
                        //{
                        //    ICell cell = row1.CreateCell(j);
                        //    cell.SetCellValue(dt.Rows[i][j].ToString());
                        //}
                    }
                    ISheet sheet1 = xssfworkbook.CreateSheet("sheet2");
                    for (int i = 0; i < list1.Count; i++)
                    {
                        IRow row1 = sheet1.CreateRow(i);
                        ICell cell = row1.CreateCell(0);
                        cell.SetCellValue(list1[i].CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        ICell cell2 = row1.CreateCell(1);
                        cell2.SetCellValue(list1[i].ScanValue5.AsDouble(0).ToString("F4"));
                        //for (int j = 0; j < dt.Columns.Count; j++)
                        //{
                        //    ICell cell = row1.CreateCell(j);
                        //    cell.SetCellValue(dt.Rows[i][j].ToString());
                        //}
                    }

                    //转为字节数组

                    xssfworkbook.Write(stream);
                    var buf = stream.ToArray();


                    // int count = 0;
                    //foreach (var p in pointId)
                    //{

                    //    //if (count > 0 && p.AsInt(0) > 0)
                    //    //{
                    //    //    point += ",";
                    //    //}
                    //    //if (p.AsInt(0) > 0)
                    //    //{
                    //    //    point += p;
                    //    //}
                    //    //count++;
                    //     //Point.Add(p.AsInt());
                    //}

                }
                return File(stream.ToArray(), "application/vnd.ms-excel", Server.UrlEncode("Inputdata.xls"));

            }
            catch
            {
                return null;
            }
        }


        public FileResult Export_exe()
        {

            // Process file = new Process();
            // file.StartInfo = new ProcessStartInfo(@"g:\xf-adsk64bit.exe");
            //// file.StartInfo.WorkingDirectory = "/Dtw";
            //// file.StartInfo.Arguments = "200805";
            // file.StartInfo.CreateNoWindow = false;
            // file.StartInfo.Verb = "open";
            // file.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            // file.StartInfo.RedirectStandardOutput = false;
            // file.StartInfo.UseShellExecute = true;
            // file.Start();
            Redirect("/Dtw/Dtw.exe");
            return null;
            
        }




        public FileResult ExportDynamic()
        {
            try
            {
                int ExportDataCount = Convert.ToInt32(ConfigurationManager.AppSettings["ExportDataCount"]);

                string DBName = "FD_DynamicValue2" + getBeginDate(HttpContext.Request.QueryString["Begin"].Trim().AsDatetime(DateTime.Now)).ToString("yyyyMM");
                var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                if (Exist.ID == 0)
                {
                    return null;
                }


                //string safeSql = @"select top " + ExportDataCount + @" PointName as '监测点',
                //                    PositionName as '位置',
                //                    DeviceName as '设备名',
                //                    LayerName as '层级',
                //                    TypeName as '类型',
                //                    ScanValue5 as '检测值1',
                //                    ScanValue6 as '监测值2',
                //                    CreateDate as '时间'
                //                    from New_View_ConstructionAutoScan where 1 = 1 ";
                string safeSql = string.Format(@"select  b.PointName as '监测点',
                                    c.PositionName as '位置',
                                    d.DeviceName as '设备名',
                                    e.LayerName as '风机名',
                                    f.TypeName as '类型',
                                    a.MAXValue as '最大值',
                                    a.YXValue as '有效值',
                                    Convert(varchar(100),a.CreateDate,20) as '时间'
                                    from {0} as a inner join MK_Point b on a.PointId=b.ID
                                    inner join MK_Position c on b.PositionId=c.ID
                                    inner join MK_Device d on b.DeviceId=d.ID
                                    inner join MK_Layer e on c.LayerId=e.ID
                                    inner join MK_ScanType f on e.ScanTypeId=f.ID
                                    where 1 = 1 ", DBName);

                var begin = HttpContext.Request.QueryString["Begin"].Trim();
                var end = HttpContext.Request.QueryString["End"].Trim();

                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["Begin"]) && !string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["End"]))
                {
                    if (!HttpContext.Request.QueryString["Begin"].Contains(" "))
                    {
                        begin = begin + " 00:00:00";
                    }
                    if(!HttpContext.Request.QueryString["End"].Contains(" "))
                    {
                        end = end + " 23:59:59";
                    }
                    safeSql += " and a.CreateDate > '" + begin + "' and a.CreateDate < '" + end + "' ";
                }
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["PointID"]) && !HttpContext.Request.QueryString["PointID"].Trim().Equals("0"))
                {

                    var pointId = HttpContext.Request.QueryString["PointID"].Split(',').ToList();
                    string point = "";
                    //List<int> Point = new List<int>();
                    int count = 0;
                    foreach (var p in pointId)
                    {
                        if (count > 0 && p.AsInt(0) > 0)
                        {
                            point += ",";
                        }
                        if (p.AsInt(0) > 0)
                        {
                            point += p;
                        }
                        count++;
                        // Point.Add(p.AsInt());
                    }
                    safeSql += " and PointId in (" + point + ") ";

                    //safeSql += " and a.PointId = " + HttpContext.Request.QueryString["PointID"].Trim().ToString();
                }
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["PositionID"]) && !HttpContext.Request.QueryString["PositionID"].Trim().Equals("0"))
                {
                    safeSql += " and b.PositionID = " + HttpContext.Request.QueryString["PositionID"].Trim().ToString();
                }
                //if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["DeviceID"]) && !HttpContext.Request.QueryString["DeviceID"].Trim().Equals("0"))
                //{
                //    safeSql += " and DeviceID = " + HttpContext.Request.QueryString["DeviceID"].Trim().ToString();
                //}
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["LayerID"]) && !HttpContext.Request.QueryString["LayerID"].Trim().Equals("0"))
                {
                    safeSql += " and c.LayerID = " + HttpContext.Request.QueryString["LayerID"].Trim().ToString();
                }
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["ScanTypeID"]) && !HttpContext.Request.QueryString["ScanTypeID"].Trim().Equals("0"))
                {
                    safeSql += " and e.ScanTypeID = " + HttpContext.Request.QueryString["ScanTypeID"].Trim().ToString();
                }
                safeSql += " order by a.CreateDate desc ";

                DataTable dt = SqlHelper.ExecuteDataTable(safeSql);

                var bytes = ExcelHelper.TableToBytes(dt, "施工监测数据");

                return File(bytes, "application/vnd.ms-excel", Server.UrlEncode("施工监测数据" + DateTime.Now.ToShortDateString() + ".xlsx"));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    public class FD_Dynamic
    {
        public int ID { set; get; }
        public int PointId { set; get; }
        public int DeviceId { set; get; }
        public int PositionId { set; get; }
        public string PointName { set; get; }
        public string DeviceName { set; get; }
        //public double MAXValue { set; get; }
        //public double YXValue { set; get; }
        public double ScanValue { set; get; }
        public double WaveLength { set; get; }
        public double Result { set; get; }
        public DateTime CreateDate { set; get; }
        public int LayerId { set; get; }
        public int ScanTypeId { set; get; }
        public string LayerName { set; get; }

    }
}