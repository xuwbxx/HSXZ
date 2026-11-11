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
    public class TrackPositionController : Controller
    {
        WorkService service = new WorkService();
        public ActionResult Index()
        {
            //安全箱人员
            var Persons = service.Repository_Safety_PersonInfo.Query().ToList();

            return View(Persons);
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public JsonResult PositionHistoryQuery(PositionHistoryRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.UserID == 0)
                {
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                request.UserID = 2078;
                List<PositionHistoryResponse> list = new List<PositionHistoryResponse>();
                service.Repository_View_PositionHistory.Query(a => a.UserId == request.UserID).OrderByDescending(a => a.PositionTime).Take(2).OrderBy(a => a.PositionTime).ToList().ForEach(a =>
                {
                    PositionHistoryResponse se = new PositionHistoryResponse();
                    se.UserID = Convert.ToInt32(a.UserId);
                    se.PersonName = a.PersonName;
                    se.UnitName = a.UnitName;
                    se.TelPhone = a.TelPhone;
                    se.IdentifyID = a.IdentifyID;
                    se.Station = a.Station;
                    se.PositionTime = Convert.ToDateTime(a.PositionTime).ToString("yyyy-MM-dd HH:mm:ss");
                    list.Add(se);
                });
                ret.Data = list;
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