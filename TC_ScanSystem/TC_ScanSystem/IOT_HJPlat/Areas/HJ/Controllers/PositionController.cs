using DataEntity.DataContext;
using DataEntity.Services;
using Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tool;

namespace IOT_HJPlat.Areas.HJ.Controllers
{
    public class PositionController : Controller
    {
        WorkService service = new WorkService();
        // GET: Scan/Position
        public ActionResult Index()
        {
            return View();
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
                        //text = "[{\"ID\":0,\"UserId\":null,\"DeviceCode\":\"861658046377134\",\"RealName\":null,\"xvalue\":32.052327,\"yvalue\":118.657213,\"zvalue\":0.0,\"BD09Lon\":118.668751068763,\"BD09Lat\":32.0563889008537,\"StructorId\":null,\"CaveId\":null,\"StructorName\":null,\"CaveName\":null,\"PositionTime\":\"2019-03-22T16:46:40.838+08:00\",\"UpdateTime\":null},{\"ID\":0,\"UserId\":null,\"DeviceCode\":\"862632030359316\",\"RealName\":null,\"xvalue\":32.0524531,\"yvalue\":118.6572717,\"zvalue\":0.0,\"BD09Lon\":118.668809536291,\"BD09Lat\":32.0565159423116,\"StructorId\":null,\"CaveId\":null,\"StructorName\":null,\"CaveName\":null,\"PositionTime\":\"2019-03-22T16:47:14.092+08:00\",\"UpdateTime\":null},{\"ID\":0,\"UserId\":null,\"DeviceCode\":\"864821036337875\",\"RealName\":null,\"xvalue\":32.05231894,\"yvalue\":118.6572024,\"zvalue\":0.0,\"BD09Lon\":118.668740254455,\"BD09Lat\":32.0563803277895,\"StructorId\":null,\"CaveId\":null,\"StructorName\":null,\"CaveName\":null,\"PositionTime\":\"2019-03-21T17:42:18.885+08:00\",\"UpdateTime\":null},{\"ID\":0,\"UserId\":null,\"DeviceCode\":\"868382030476470\",\"RealName\":null,\"xvalue\":32.052257,\"yvalue\":118.657159,\"zvalue\":0.0,\"BD09Lon\":118.668696702654,\"BD09Lat\":32.0563178123306,\"StructorId\":null,\"CaveId\":null,\"StructorName\":null,\"CaveName\":null,\"PositionTime\":\"2019-03-22T15:45:25.132+08:00\",\"UpdateTime\":null}]";
                        //text = "[{"ID":0,"UserId":null,"DeviceCode":"861658046377134","RealName":null,"xvalue":32.052327,"yvalue":118.657213,"zvalue":0.0,"BD09Lon":118.668751068763,"BD09Lat":32.0563889008537,"StructorId":null,"CaveId":null,"StructorName":null,"CaveName":null,"PositionTime":"2019 - 03 - 22T16: 46:40.838 + 08:00","UpdateTime":null},{"ID":0,"UserId":null,"DeviceCode":"862632030359316","RealName":null,"xvalue":32.0524531,"yvalue":118.6572717,"zvalue":0.0,"BD09Lon":118.668809536291,"BD09Lat":32.0565159423116,"StructorId":null,"CaveId":null,"StructorName":null,"CaveName":null,"PositionTime":"2019 - 03 - 22T16: 47:14.092 + 08:00","UpdateTime":null},{"ID":0,"UserId":null,"DeviceCode":"864821036337875","RealName":null,"xvalue":32.05231894,"yvalue":118.6572024,"zvalue":0.0,"BD09Lon":118.668740254455,"BD09Lat":32.0563803277895,"StructorId":null,"CaveId":null,"StructorName":null,"CaveName":null,"PositionTime":"2019 - 03 - 21T17: 42:18.885 + 08:00","UpdateTime":null},{"ID":0,"UserId":null,"DeviceCode":"868382030476470","RealName":null,"xvalue":32.052257,"yvalue":118.657159,"zvalue":0.0,"BD09Lon":118.668696702654,"BD09Lat":32.0563178123306,"StructorId":null,"CaveId":null,"StructorName":null,"CaveName":null,"PositionTime":"2019 - 03 - 22T15: 45:25.132 + 08:00","UpdateTime":null}]";

                        //反序列化
                        List<Scan_Position_Current> List = text.Deserialize<List<Scan_Position_Current>>();
                        DateTime now = DateTime.Now;
                        if (List != null && List.Count > 0)
                        {
                            //获取工具箱数据
                            var SafetyData = new List<Safety_PersonInfo>();

                            var PersonInfoCache = CacheUtils.Get(CacheKey.工具箱人员信息数据缓存);
                            if (PersonInfoCache == null)
                            {
                                SafetyData = service.Repository_Safety_PersonInfo.Query().ToList();
                                CacheUtils.Set(CacheKey.工具箱人员信息数据缓存, SafetyData, 60);
                            }
                            else
                            {
                                SafetyData = PersonInfoCache as List<Safety_PersonInfo>;
                            }

                            List<Scan_Position_Current> CurrentList = new List<Scan_Position_Current>();
                            List<Scan_Position_History> HistoryList = new List<Scan_Position_History>();
                            foreach (var data in List)
                            {
                                //查询工具箱人员ID
                                var User = SafetyData.FirstOrDefault(a => a.ReadCardId != null && a.ReadCardId.Equals(data.DeviceCode));
                                if (User == null || User.AutoID == null)
                                {
                                    continue;
                                }
                                int UserID = Convert.ToInt32(User.AutoID);

                                Scan_Position_Current currentSe = new Scan_Position_Current();
                                currentSe.BD09Lat = data.BD09Lat;
                                currentSe.BD09Lon = data.BD09Lon;
                                currentSe.CaveId = data.CaveId;
                                currentSe.CaveName = data.CaveName;
                                currentSe.DeviceCode = data.DeviceCode;
                                currentSe.PositionTime = data.PositionTime;
                                currentSe.RealName = User.PersonName;
                                currentSe.StructorId = data.StructorId;
                                currentSe.StructorName = data.StructorName;
                                currentSe.UserId = UserID;
                                currentSe.xvalue = data.xvalue;
                                currentSe.yvalue = data.yvalue;
                                currentSe.zvalue = data.zvalue;
                                currentSe.UpdateTime = now;
                                CurrentList.Add(currentSe);

                                Scan_Position_History historySe = new Scan_Position_History();
                                historySe.BD09Lat = data.BD09Lat;
                                historySe.BD09Lon = data.BD09Lon;
                                historySe.CaveId = data.CaveId;
                                historySe.CaveName = data.CaveName;
                                historySe.DeviceCode = data.DeviceCode;
                                historySe.PositionTime = data.PositionTime;
                                historySe.RealName = User.PersonName;
                                historySe.StructorId = data.StructorId;
                                historySe.StructorName = data.StructorName;
                                historySe.UserId = UserID;
                                historySe.xvalue = data.xvalue;
                                historySe.yvalue = data.yvalue;
                                historySe.zvalue = data.zvalue;
                                historySe.UpdateTime = now;
                                HistoryList.Add(historySe);
                            }
                            //实时定位表
                            //先删除实时表
                            var Scan_Position_CurrentList = service.Repository_Scan_Position_Current.Query();
                            service.Repository_Scan_Position_Current.DeleteList(Scan_Position_CurrentList);
                            //再插入数据
                            service.Repository_Scan_Position_Current.AddList(CurrentList);

                            //插入历史定位表
                            service.Repository_Scan_Position_History.AddList(HistoryList);

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