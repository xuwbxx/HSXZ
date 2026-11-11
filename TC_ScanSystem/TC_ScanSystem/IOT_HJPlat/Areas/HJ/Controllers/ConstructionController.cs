using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOT_HJPlat.Areas.HJ.Controllers
{
    public class ConstructionController : Controller
    {
        [FilterHelper]
        // GET: Scan/Construction
        public ActionResult Index()
        {
            return View();
        }

        [FilterHelper]
        public ActionResult Config()
        {
            return View();
        }
    }
}