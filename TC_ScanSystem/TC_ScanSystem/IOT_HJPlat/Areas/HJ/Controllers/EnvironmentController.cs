using Business.AlertService;
using DataEntity.DataContext;
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
using Tool;

namespace IOT_HJPlat.Areas.HJ.Controllers
{
    public class EnvironmentController : Controller
    {
        WorkService service = new WorkService();

        [FilterHelper]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DataQuery(EnvironmentRequest request)
        {

            BaseReturn ret = new BaseReturn();
            try
            {
                //组装视图
                var query = service.Repository_View_EnvironmentData.Query().AsQueryable();

                //筛选
                if (request.PointID != 0)
                {
                    query = query.Where(a => a.PointID == request.PointID).AsQueryable();
                }
                if (!string.IsNullOrEmpty(request.Begin) && !string.IsNullOrEmpty(request.End))
                {
                    DateTime begin = Convert.ToDateTime(request.Begin);
                    DateTime end = Convert.ToDateTime(request.End);
                    query = query.Where(a => a.CreateTime >= begin && a.CreateTime <= end).AsQueryable();
                }

                int Count = query.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                //如果不是第一页，并且没有数据，则查询第一页
                var CurrentQuery = query.OrderByDescending(a => a.CreateTime).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = query.OrderByDescending(a => a.CreateTime).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                //数据组装和处理
                List<EnvironmentResponse> list = new List<EnvironmentResponse>();
                CurrentQuery.ForEach(a =>
                {
                    EnvironmentResponse se = new EnvironmentResponse();
                    se.PointName = a.PointName;
                    se.Temperature = a.Temperature == null ? "" : a.Temperature.ToString();
                    se.Humidity = a.Humidity == null ? "" : a.Humidity.ToString();
                    se.Noise = a.Noise == null ? "" : a.Noise.ToString();
                    se.PM25 = a.PM25 == null ? "" : a.PM25.ToString();
                    se.PM10 = a.PM10 == null ? "" : a.PM10.ToString();
                    se.WindForce = a.WindForce == null ? "" : a.WindForce.ToString();
                    se.WindSpeed = a.WindSpeed == null ? "" : a.WindSpeed.ToString();
                    se.WindDirectionText = a.WindDirectionText;
                    se.CreateTime = a.CreateTime == null ? "" : Convert.ToDateTime(a.CreateTime).ToString("yyyy-MM-dd HH:mm:ss");
                    list.Add(se);
                });

                //只取10条
                LineEchart echart = GetEchart(list.Take(10).ToList());

                ret.Data = list;
                ret.Echart = echart;
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

        public void EnvironmentDataReceive()
        {
            try
            {
                string str = Request.Params[0].ToString();

                LogUtils.Instance.LogText(str);

                //str = @"20190315183048375;ST=22;CN=2011;PW=123456;MN=190315004;Flag=5;CP=##DataTime=20190315183048;a34004-Rtd=63000,a34004-Flag=N;a34002-Rtd=75000,a34002-Flag=N;a01001-Rtd=12.2,a01001-Flag=N;a01002-Rtd=26.0,a01002-Flag=N;a01007-Rtd=0.0,a01007-Flag=N;a01008-Rtd=0,a01008-Flag=N##2A80";


                //20190318165900375; ST = 22; CN = 2011; PW = 123456; MN = 190315004; Flag = 5; CP =##DataTime=20190318165900;a34004-Rtd=44000,a34004-Flag=N;a34002-Rtd=58000,a34002-Flag=N;LA-Rtd=96.0,LA-Flag=N;a01008-Rtd=135,a01008-Flag=N;a01001-Rtd=12.3,a01001-Flag=N;a01002-Rtd=45.0,a01002-Flag=N;a01007-Rtd=2.7,a01007-Flag=N;a01009-Rtd=2,a01009-Flag=N##18C0

                string[] Params = str.Replace("##", ";").Replace(",", ";").Split(';');

                Scan_EnvironmentData se = new Scan_EnvironmentData();
                for (int i = 0; i < Params.Length; i++)
                {
                    string code = Params[i].ToString();

                    if (!code.Contains("="))
                        continue;

                    string key = code.Split('=')[0];
                    string value = code.Split('=')[1];



                    switch (key)
                    {
                        case "a01001-Rtd":
                            se.Temperature = Convert.ToDouble(value);
                            break;
                        case "a01002-Rtd":
                            se.Humidity = Convert.ToDouble(value);
                            break;
                        case "a34004-Rtd":
                            se.PM25 = Convert.ToDouble(Convert.ToDouble(value) / 1000);
                            break;
                        case "a34002-Rtd":
                            se.PM10 = Convert.ToDouble(Convert.ToDouble(value) / 1000);
                            break;
                        case "a01007-Rtd":
                            se.WindSpeed = Convert.ToDouble(value);
                            break;
                        case "a01008-Rtd":
                            se.WindDirection = Convert.ToDouble(value);
                            se.WindDirectionText = ConvertToText(Convert.ToInt32(value));
                            break;
                        case "LA-Rtd":
                            se.Noise = Convert.ToDouble(value);
                            break;
                        case "a01009-Rtd":
                            se.WindForce = Convert.ToDouble(value);
                            break;
                    }

                }
                se.CreateTime = DateTime.Now;
                se.PointID = 1;

                service.Repository_Scan_EnvironmentData.Insert(se);
                service.SaveChange();


                //短信报警处理
                //PM10
                if (se.PM10 != null)
                {
                    string Switch = System.Configuration.ConfigurationManager.AppSettings["SMS-Switch"];
                    if (Switch.Equals("0"))
                    {
                        ISMSService PM10Service = new PM10SMSService();
                        PM10Service.SMSSend(se.PM10);
                    }

                }



            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogText(ex.ToString());
            }
        }

        string ConvertToText(int Direction)
        {
            string text = "";
            switch (Direction)
            {
                case 0:
                    text = "北"; break;
                case 45:
                    text = "东北"; break;
                case 90:
                    text = "东"; break;
                case 135:
                    text = "东南"; break;
                case 180:
                    text = "南"; break;
                case 225:
                    text = "西南"; break;
                case 270:
                    text = "西"; break;
                case 315:
                    text = "西北"; break;
            }
            return text;
        }

        /// <summary>
        /// 组装echart模型
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private LineEchart GetEchart(List<EnvironmentResponse> list)
        {
            LineEchart echart = new LineEchart();
            echart.title.text = "环境趋势图";
            echart.title.subtext = "港湾院工艺所设计";

            echart.legend.data.AddRange(new List<string> { "温度", "湿度", "噪音", "PM2.5", "PM10" });

            //series
            series seriesTemp = new series()
            {
                name = "温度",
                type = "line"
            };
            series seriesHumi = new series()
            {
                name = "湿度",
                type = "line"
            };
            series seriesNoise = new series()
            {
                name = "噪音",
                type = "line"
            };
            series seriesPM25 = new series()
            {
                name = "PM2.5",
                type = "line"
            };
            series seriesPM10 = new series()
            {
                name = "PM10",
                type = "line"
            };

            list = list.OrderBy(a => a.CreateTime).ToList();
            foreach (var item in list)
            {
                echart.xAxis.data.Add(item.CreateTime);

                seriesTemp.data.Add(Convert.ToDecimal(item.Temperature));
                seriesHumi.data.Add(Convert.ToDecimal(item.Humidity));
                seriesNoise.data.Add(Convert.ToDecimal(item.Noise));
                seriesPM25.data.Add(Convert.ToDecimal(item.PM25));
                seriesPM10.data.Add(Convert.ToDecimal(item.PM10));
            }

            echart.series.Add(seriesTemp);
            echart.series.Add(seriesHumi);
            echart.series.Add(seriesNoise);
            echart.series.Add(seriesPM25);
            echart.series.Add(seriesPM10);

            return echart;
        }

    }
}