using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOT_HJPlat.Areas.HJ.Controllers
{
    public class VehicleInAndOutController : Controller
    {
        [FilterHelper]
        // GET: HJ/VehicleInAndOut
        public ActionResult Index()
        {
            return View();
        }
    }
}