using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOT_THPlat.Areas.Common.Controllers
{
    public class PopUpController : Controller
    {
        // GET: Common/PopUp
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult EChartPop()
        {
            return PartialView();
        }

        public PartialViewResult QRCodePop()
        {
            return PartialView();
        }
    }
}