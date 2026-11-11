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
using ZJFramework10.Extension;


namespace IOT_HJPlat.Areas.Scan.Controllers
{
    public class DeviceController : Controller
    {
        DBDataContext service = DataContextFactory.Create();
        public ActionResult Index()
        {
            var Projects = service.Base_Project.ToList();
            var Protocols = service.MK_Protocol.ToList();
            var Layers = service.MK_Layer.ToList();
            
            var Communications = service.MK_CommunicationDevice.ToList();
            ViewData["Communications"] = Communications;
            ViewData["Projects"] = Projects;
            ViewData["Protocols"] = Protocols;
            ViewData["Layers"] = Layers;

            return View();
        }

        public ActionResult Command()
        {
            var Devices = service.MK_Device.ToList();
            ViewData["Devices"] = Devices;
            return View();
        }

        [HttpPost]
        public JsonResult SaveInterval(DeviceResponse request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                string Date = FormZero(request.Date.AsInt());
                string Hour = FormZero(request.Hour.AsInt());
                string Min = FormZero(request.Min.AsInt());
                    var Device = service.MK_Device.FirstOrDefault(a => a.ID == request.ID);
                    if (Device == null)
                    {
                        ret.Message = "信息出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                    //ObjectUtils.CopyObjectValue(request, Device, ObjectUtils.DefaultSkipList);
                    Device.CreateDate = DateTime.Now;
                Device.Interval = string.Format("{0}-{1}-{2}", Date, Hour, Min);
                    service.SubmitChanges();
                if (request.Date.Length > 0 && request.Hour.Length > 0 && request.Min.Length > 0)
                {
                    Excute("MCU32 {0} WrSmpInt " + Date + Hour + Min,Device);
                }
                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult SaveDeviceTime(DeviceResponse request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var Device = service.MK_Device.FirstOrDefault(a => a.ID == request.ID);
                if (Device == null)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                //ObjectUtils.CopyObjectValue(request, Device, ObjectUtils.DefaultSkipList);
                Device.CreateDate = DateTime.Now;
                Device.DeviceTime = request.DeviceTime;
                service.SubmitChanges();
                if (!string.IsNullOrEmpty(request.DeviceTime.AsStr()))
                {
                    Excute("MCU32 {0} SetTime "+request.DeviceTime.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss"), Device);
                }
                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult SaveBeginTime(DeviceResponse request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var Device = service.MK_Device.FirstOrDefault(a => a.ID == request.ID);
                if (Device == null)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                //ObjectUtils.CopyObjectValue(request, Device, ObjectUtils.DefaultSkipList);
                Device.CreateDate = DateTime.Now;
                string BeginTime = string.Format("{0}-{1}-{2}",DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
                if (!string.IsNullOrEmpty(request.BeginTime))
                {
                    BeginTime = BeginTime + " " + request.BeginTime;
                }
                Device.BeginTime = BeginTime.AsDatetime(DateTime.Now);
                service.SubmitChanges();
                if (!string.IsNullOrEmpty(request.DeviceTime.AsStr()))
                {
                    Excute("MCU32 {0} WrSmpStd " + BeginTime.AsDatetime(DateTime.Now).ToString("mmss"), Device);
                }
                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult Send(DeviceResponse request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var Device = service.MK_Device.FirstOrDefault(a => a.ID == request.ID);
                if (Device == null)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                //ObjectUtils.CopyObjectValue(request, Device, ObjectUtils.DefaultSkipList);
                Excute(request.Command, Device);
                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        private static string FormZero(int value)
        {
            if (value < 10)
            {
                return "0" + value.ToString();
            }
            else
            {
                return value.ToString();
            }
        }

        private void Excute(string str,MK_Device device)
        {
            MK_Scan_Command temp_cmd = new MK_Scan_Command();
            temp_cmd.Command = str;
            temp_cmd.DeviceID = device.ID;
            temp_cmd.CommunicationDeviceID = device.CommunicationDeviceId;
            temp_cmd.Status = 0;
            temp_cmd.CreateDate = DateTime.Now;
            temp_cmd.IsSystem = 0;
            temp_cmd.Result = "";
            temp_cmd.DeviceIden = device.DeviceIden;
            temp_cmd.CommunicationIden = service.MK_CommunicationDevice.SingleOrDefault
                    (d => d.ID == temp_cmd.CommunicationDeviceID).CommunicationCode;
            //if (DB.FD_Device.SingleOrDefault(d => d.ID == temp_cmd.DeviceId) != null)
            //{
            //    temp_cmd.DeviceIden = DB.FD_Device.SingleOrDefault(d => d.ID == temp_cmd.DeviceId).DeviceIdentity;
            //}


            List<MK_Point> temp_point = new List<MK_Point>();
            temp_point = service.MK_Point.Where(d => d.DeviceId == device.ID).ToList();
            if (temp_point.Count > 0)
            {
                temp_cmd.PointId = temp_point[0].ID;
            }

            service.MK_Scan_Command.InsertOnSubmit(temp_cmd);
            service.SubmitChanges();
        }

        [HttpPost]
        public JsonResult CommandQuery(DeviceRequest request)
        {
            BaseReturn ret = new BaseReturn();

            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            try
            {
                var DeviceInfo = service.MK_Device.FirstOrDefault(a => a.ID == request.ID);
                if (DeviceInfo != null)
                {
                    ConstructCommandResponse res = new ConstructCommandResponse();
                    if (!string.IsNullOrEmpty(DeviceInfo.Interval))
                    {
                        string[] interval = DeviceInfo.Interval.Split('-');
                        if (interval[0].Length > 0)
                        {
                            res.Date = interval[0];
                        }
                        if (interval[1].Length > 0)
                        {
                            res.Hour = interval[1];
                        }
                        if (interval[2].Length > 0)
                        {
                            res.Min = interval[2];
                        }
                    }
                    res.BeginTime = DeviceInfo.BeginTime.AsDatetime(DateTime.Now).ToString("hh:mm");
                    res.DeviceDate = DeviceInfo.DeviceTime.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss");
                    ret.Data = res;
                    ret.Success = true;
                }
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }


        #region Communication

        [HttpPost]
        public JsonResult ComQuery(CommnicationRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var tempQuery = service.View_MK_CommunicationDevice.AsQueryable();

                if (request.ProjectId != 0)
                {
                    tempQuery = tempQuery.Where(a => a.ProjectId == request.ProjectId).AsQueryable();
                }

                int Count = tempQuery.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                var CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                //如果不是第一页，并且没有数据，则查询第一页
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                List<CommunicationResponse> list = new List<CommunicationResponse>();

                CurrentQuery.ForEach(a =>
                {
                    list.Add(new CommunicationResponse()
                    {
                        ID = a.ID,
                        CommunicationName = a.CommunicationName,
                        CommunicationCode = a.CommunicationCode,
                        CommunicationTypeName = ((ConstructAutoCommunicationType)(Convert.ToInt32(a.CommunicationType))).ToString(),
                        DataProtocolName = ((ConstructAutoCommunicationProtocal)(Convert.ToInt32(a.DataProtocol))).ToString(),
                        DeviceTypeName = ((ConstructAutoCommunicationDevice)(Convert.ToInt32(a.DeviceType))).ToString(),
                        IP = a.IP,
                        Port = a.Port,
                        ProjectName = a.ProjectName
                    });
                });

                ret.Data = list;
                ret.Success = true;
                ret.PageIndex = request.PageIndex;

            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult ComInfoQuery(CommnicationRequest request)
        {
            BaseReturn ret = new BaseReturn();

            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            try
            {
                var CommunicationInfo = service.MK_CommunicationDevice.FirstOrDefault(a => a.ID == request.ID);
                if (CommunicationInfo != null)
                {
                    ret.Data = CommunicationInfo;
                    ret.Success = true;
                }
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult SaveCom(CommunicationResponse request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.ProjectId == null)
                {
                    ret.Message = "请选择所属项目";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (request.CommunicationType == null)
                {
                    ret.Message = "请选择通讯类型";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (request.DataProtocol == null)
                {
                    ret.Message = "请选择通讯协议";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (request.DeviceType == null)
                {
                    ret.Message = "请选择设备类型";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                //修改用户
                if (request.Type.Equals("Modify"))
                {
                    var Communication = service.MK_CommunicationDevice.FirstOrDefault(a => a.ID == request.ID);
                    if (Communication == null)
                    {
                        ret.Message = "信息出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                    ObjectUtils.CopyObjectValue(request, Communication, ObjectUtils.DefaultSkipList);
                    Communication.CreateDate = DateTime.Now;

                    service.SubmitChanges();
                    ret.Success = true;

                }
                else if (request.Type.Equals("Add"))
                {
                    MK_CommunicationDevice se = new MK_CommunicationDevice();
                    ObjectUtils.CopyObjectValue(request, se, ObjectUtils.DefaultSkipList);
                    se.CreateDate = DateTime.Now;

                    service.MK_CommunicationDevice.InsertOnSubmit(se);

                    service.SubmitChanges();
                    ret.Success = true;
                }

            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DeleteCom(CommunicationResponse request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var se = service.MK_CommunicationDevice.Where(a => a.ID == request.ID).FirstOrDefault();
                if (se == null)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                service.MK_CommunicationDevice.DeleteOnSubmit(se);
                service.SubmitChanges();

                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        #endregion

        [FilterHelper]
        public ActionResult ScanDevice()
        {
            var Projects = service.Base_Project.Where(a => !a.IsDelete.Value).ToList();
            ViewData["Projects"] = Projects;
            var Layers = service.MK_Layer.ToList();
            ViewData["Layers"] = Layers;
            var Communications = service.MK_CommunicationDevice.ToList();
            ViewData["Communications"] = Communications;
            return View();
        }

        [HttpPost]
        public JsonResult DeviceQuery(DeviceRequest request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var tempQuery = service.View_MK_Device.AsQueryable();

                if (request.ProjectID != 0)
                {
                    tempQuery = tempQuery.Where(a => a.ProjectId == request.ProjectID).AsQueryable();
                }

                if (request.LayerID != 0)
                {
                    tempQuery = tempQuery.Where(a => a.LayerId == request.LayerID).AsQueryable();
                }

                if (request.CommunicationDeviceID != 0)
                {
                    tempQuery = tempQuery.Where(a => a.CommunicationDeviceId == request.CommunicationDeviceID).AsQueryable();
                }

                int Count = tempQuery.Count();
                ret.TotalCount = Count;
                ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                var CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                //如果不是第一页，并且没有数据，则查询第一页
                if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                {
                    request.PageIndex = 1;
                    CurrentQuery = tempQuery.OrderBy(a => a.ID).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                }

                List<DeviceResponse> list = new List<DeviceResponse>();

                CurrentQuery.ForEach(a =>
                {
                    DeviceResponse se = new DeviceResponse();
                    se.ID = a.ID;
                    se.CommunicationName = a.CommunicationName;
                    se.DeviceIden = a.DeviceIden;
                    se.Status = a.Status;
                    se.DeviceName = a.DeviceName;
                    se.DeviceProtocolName = ((ConstructAutoCommunicationProtocal)(Convert.ToInt32(a.DeviceProtocol))).ToString();
                    se.CreateTime = Convert.ToDateTime(a.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");
                    se.LayerName = a.LayerName;
                    list.Add(se);
                });



                ret.Data = list;
                ret.Success = true;
                ret.PageIndex = request.PageIndex;

            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DeviceInfoQuery(DeviceRequest request)
        {
            BaseReturn ret = new BaseReturn();

            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }

            try
            {
                var DeviceInfo = service.MK_Device.FirstOrDefault(a => a.ID == request.ID);
                if (DeviceInfo != null)
                {
                    ret.Data = DeviceInfo;
                    ret.Success = true;
                }
            }
            catch (Exception ex)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult SaveDevice(DeviceResponse request)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                if (request.ProjectId == null)
                {
                    ret.Message = "请选择所属项目";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (request.LayerId == null)
                {
                    ret.Message = "请选择所属区域";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (request.DeviceProtocol == null)
                {
                    ret.Message = "请选择通讯协议";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                if (request.CommunicationDeviceId == null)
                {
                    ret.Message = "请选择通讯设备";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }

                //修改用户
                if (request.Type.Equals("Modify"))
                {
                    var Device = service.MK_Device.FirstOrDefault(a => a.ID == request.ID);
                    if (Device == null)
                    {
                        ret.Message = "信息出错";
                        return Json(ret, JsonRequestBehavior.DenyGet);
                    }
                    ObjectUtils.CopyObjectValue(request, Device, ObjectUtils.DefaultSkipList);
                    Device.CreateDate = DateTime.Now;

                    service.SubmitChanges();
                    ret.Success = true;

                }
                else if (request.Type.Equals("Add"))
                {
                    MK_Device se = new MK_Device();
                    ObjectUtils.CopyObjectValue(request, se, ObjectUtils.DefaultSkipList);
                    se.CreateDate = DateTime.Now;
                    se.BeginTime = DateTime.Now;
                    se.DeviceTime = DateTime.Now;
                    se.Status = 0;
                    service.MK_Device.InsertOnSubmit(se);

                    service.SubmitChanges();
                    ret.Success = true;
                }

            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult DeleteDevice(DeviceResponse request)
        {
            BaseReturn ret = new BaseReturn();
            if (request.ID == 0)
            {
                return Json(ret, JsonRequestBehavior.DenyGet);
            }
            try
            {
                var se = service.MK_Device.FirstOrDefault(a => a.ID == request.ID);
                if (se == null)
                {
                    ret.Message = "信息出错";
                    return Json(ret, JsonRequestBehavior.DenyGet);
                }
                service.MK_Device.DeleteOnSubmit(se);

                service.SubmitChanges();

                ret.Success = true;
            }
            catch (Exception)
            {
                ret.Success = false;
            }
            return Json(ret, JsonRequestBehavior.DenyGet);
        }

    }
}