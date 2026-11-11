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
    public class PersonPositionController : Controller
    {
        WorkService service = new WorkService();

        [FilterHelper]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DataQuery(PositionPersonRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var Query = service.Repository_View_Scan_Position_Current_temp.Query().ToList();
                ret.Data = Query;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Success = false;
                //记录日志
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DataQuery2(PositionPersonRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var WorkCaves = service.Repository_Base_Cave.Query(a => !a.IsDelete && a.IsDisplay.Value).ToList();
                var Persons = service.Repository_View_Scan_Position_Current_temp.Query().ToList();

                PersonPositionResponse data = new PersonPositionResponse();


                foreach (var cave in WorkCaves)
                {
                    decimal minLon = Convert.ToDecimal(cave.BDlonMin);
                    decimal maxLon = Convert.ToDecimal(cave.BDlonMax);
                    decimal minLat = Convert.ToDecimal(cave.BDlatMin);
                    decimal maxLat = Convert.ToDecimal(cave.BDlatMax);

                    CavePosition CaveSe = new CavePosition();
                    CaveSe.LatMax = maxLat;
                    CaveSe.LatMin = minLat;
                    CaveSe.LonMax = maxLon;
                    CaveSe.LonMin = minLon;
                    CaveSe.CenterLon = (maxLon + minLon) / 2;
                    CaveSe.CenterLat = (maxLat + minLat) / 2;
                    CaveSe.radius = 500;
                    CaveSe.ID = cave.ID;
                    CaveSe.CaveName = cave.CaveName;

                    foreach (var person in Persons)
                    {
                        //如果坐标在范围内，则添加进去
                        if (person.BD09Lon >= minLon && person.BD09Lon <= maxLon && person.BD09Lat >= minLat && person.BD09Lat <= maxLat)
                        {
                            PersonPosition PersonSe = new PersonPosition();
                            PersonSe.ID = Convert.ToInt32(person.ID);
                            PersonSe.PersonName = person.PersonName;
                            PersonSe.UnitName = person.UnitName;
                            PersonSe.UserID = Convert.ToInt32(person.UserId);
                            PersonSe.Lon = Convert.ToDecimal(person.BD09Lon);
                            PersonSe.Lat = Convert.ToDecimal(person.BD09Lat);
                            CaveSe.Persons.Add(PersonSe);
                        }
                    }

                    data.Caves.Add(CaveSe);
                }

                ret.Data = data;
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