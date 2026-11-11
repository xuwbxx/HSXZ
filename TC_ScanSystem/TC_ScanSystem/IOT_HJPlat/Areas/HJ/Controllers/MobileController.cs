using Business.Machinery;
using Business.Training;
using Business.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOT_HJPlat.Areas.HJ.Controllers
{
    public class MobileController : Controller
    {
        // GET: Mobile/Data
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CraneInfo()
        {
            ActionResult ret = null;

            string paraStr = HttpContext.Request.QueryString["id"];

            if (!string.IsNullOrEmpty(paraStr))
            {
                CraneBusiness craBiz = new CraneBusiness();
                var response = craBiz.GetGantryCraneItemInfo(paraStr);
                ret = View(response);
            }

            return ret;
        }

        public ActionResult CultivateStatistic()
        {
            TrainingBusiness traBiz = new TrainingBusiness();
            var response = traBiz.GetTrainingStatistic();

            return View(response);
        }

        public ActionResult PersonDetail()
        {
            ActionResult ret = null;

            string paraStr = HttpContext.Request.QueryString["id"];

            if (!string.IsNullOrEmpty(paraStr))
            {
                TrainingBusiness traBiz = new TrainingBusiness();
                var response = traBiz.GetPersonDetail(paraStr);
                ret = View(response);
            }

            return ret;
        }

        public FileResult PersonPortrait()
        {
            FileResult ret = null;

            string paraStr = HttpContext.Request.QueryString["id"];

            if (!string.IsNullOrEmpty(paraStr))
            {
                TrainingBusiness traBiz = new TrainingBusiness();
                var response = traBiz.GetPortrait(paraStr);
                ret = File(response, "impage/jpeg");
            }

            return ret;
        }

        public FileResult PersonQRCode()
        {
            FileResult ret = null;

            string paraStr = HttpContext.Request.QueryString["id"];

            if (!string.IsNullOrEmpty(paraStr))
            {
                TrainingBusiness traBiz = new TrainingBusiness();
                var response = traBiz.GetQRCode(paraStr);
                ret = File(response, "impage/jpeg");
            }

            return ret;
        }

        public ActionResult VehiclePresent()
        {
            ActionResult ret = null;

            string paraStr = HttpContext.Request.QueryString["area"];

            if (!string.IsNullOrEmpty(paraStr))
            {
                VehicleBusiness vehBiz = new VehicleBusiness();
                var response = vehBiz.GetVehiclePresent(paraStr);
                ret = View(response);
            }

            return ret;
        }
    }
}