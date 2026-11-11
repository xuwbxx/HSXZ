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
using System.IO;
using Model.Monitor.Request;
using Business.Monitor;

namespace IOT_HJPlat.Areas.Scan.Controllers
{
    public class testController:Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ContentResult UpLoadFile(HttpPostedFileBase fileData)
        {
            if (fileData != null && fileData.ContentLength > 0)
            {
                string fileSave = Server.MapPath("/UpLoad/");
                //获取文件的扩展名
                string extName = Path.GetExtension(fileData.FileName);
                //得到一个新的文件名称
                string newName = Guid.NewGuid().ToString() + extName;
                fileData.SaveAs(Path.Combine(fileSave, newName));
            }
            return Content("");
        }


        public ActionResult Test2()
        {
            BaseReturn ret = new BaseReturn();
            BaseReturn ret1 = new BaseReturn();
            //BaseReturn ret2 = new BaseReturn();
            MonitorService mon = new MonitorService();
            MonitorPointRequest request = new MonitorPointRequest();
            request.MonitorPointID = 1;
            request.StartTime = null;
            request.EndTime = null;
            // ret = mon.getMonitorArea();
            ret1 = mon.getMonitorPointData(request);
            return View();
        }
    }
}