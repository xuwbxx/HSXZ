using DataEntity.DataContext;
using DataEntity.Services;
using Model.Common;
using Model.HJPlat.Request;
using Model.HJPlat.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tool;

namespace IOT_HJPlat.Areas.HJ.Controllers
{
    public class PersonInAndOutController : Controller
    {
        WorkService service = new WorkService();

        [FilterHelper]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DataQuery(PersonInAndOutRequest request)
        {

            BaseReturn ret = new BaseReturn();
            try
            {
                //组装视图
                var query = service.Repository_Scan_PersonInAndOut.Query().AsQueryable();

                //筛选
                if (!string.IsNullOrEmpty(request.Name))
                {
                    query = query.Where(a => a.EmployeeName.Equals(request.Name)).AsQueryable();
                }
                if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                {
                    DateTime begin = Convert.ToDateTime(request.Begin);
                    DateTime end = Convert.ToDateTime(request.End);
                    query = query.Where(a => a.EventTime >= begin && a.EventTime <= end).AsQueryable();
                }

                int Count = query.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                //如果不是第一页，并且没有数据，则查询第一页
                var CurrentQuery = query.OrderByDescending(a => a.EventTime).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = query.OrderByDescending(a => a.EventTime).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                List<PersonInAndOutResponse> list = new List<PersonInAndOutResponse>();
                //数据组装和处理
                CurrentQuery.ForEach(a =>
                {
                    PersonInAndOutResponse se = new PersonInAndOutResponse();
                    se.AreaName = a.AreaName;
                    se.Card = a.Card;
                    se.EmployeeName = a.EmployeeName;
                    se.EventTime = Convert.ToDateTime(a.EventTime).ToString("yyyy-MM-dd HH:mm:ss");
                    se.JobName = a.JobName;
                    se.TypeName = a.TypeName;
                    list.Add(se);
                });



                ret.Data = list;
                ret.Success = true;
                ret.PageIndex = request.PageIndex;

            }
            catch (Exception ex)
            {
                ret.Success = false;
                //记录日志

            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        public void DataReceive()
        {
            try
            {
                var Stream = HttpContext.Request.InputStream;

                if (Stream.Length > 0)
                {
                    MemoryStream memstream = new MemoryStream();
                    Request.InputStream.CopyTo(memstream);
                    memstream.Position = 0;
                    using (StreamReader reader = new StreamReader(memstream))
                    {
                        string text = reader.ReadToEnd();

                        LogUtils.Instance.LogText(text);

                        List<Scan_PersonInAndOut> list = text.Deserialize<List<Scan_PersonInAndOut>>();

                        if (list != null && list.Count > 0)
                        {
                            service.Repository_Scan_PersonInAndOut.AddList(list);
                            service.SaveChange();
                        }

                    }
                }



            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogText(ex.ToString());
            }
        }
    }
}