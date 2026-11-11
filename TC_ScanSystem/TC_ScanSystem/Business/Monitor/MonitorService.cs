using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEntity;
using System.Data.Linq;
using System.Collections;
using Model.Monitor.Response;
using Model.Common;
using Model.Monitor.Request;
using ZJFramework10.Extension;

namespace Business.Monitor
{
    public class MonitorService
    {
        private DBDataContext service = DataContextFactory.Create();

        public BaseReturn GetMonitorType()
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var list = service.MK_ScanType.Where(u => u.ProjectId == 1).Select(u => new MonitorType
                {
                    MonitorName = u.TypeName,
                    ID = u.ID,
                    IconClass = u.Description
                }).ToList();
                ret.Data = list;
                //ret.PageIndex=
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorType>();
                ret.Message = ex.Message;
                ret.Success = false;
            }
            return ret;

        }


        public BaseReturn GetMonitorAlarm()
        {
            BaseReturn ret = new BaseReturn();
            try
            {

                var Info = service.MK_Layer.Select(a => new MonitorLayers
                {
                    MonitorAreaID = a.ID,
                    MonitorAreaName = a.LayerName,
                    IconClass = a.Description,
                    NewMapX = a.NewMapX,
                    NewMapY = a.NewMapY,
                    NewMapZ = a.NewMapZ,
                    X = a.MapX,
                    Y = a.MapY
                }).OrderBy(u => u.MonitorAreaID).ToList();
                foreach (var layer in Info)
                {
                    var pointlist = (from b in service.MK_Position.Where(u => u.LayerId == layer.MonitorAreaID)
                                     join c in service.MK_Point on b.ID equals c.PositionId
                                     join d in service.MK_Device on c.DeviceId equals d.ID
                                     select new { c.ID, c.MaxNum, c.MinNum, d.DeviceProtocol }).FirstOrDefault();
                    // var point = pointlist.FirstOrDefault();
                    if (pointlist != null)
                    {
                        var valuelist = new List<MonitorSensorValue>();
                        if (pointlist.DeviceProtocol == 0)
                        {
                            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            valuelist = service.MK_Alert.Where(u => u.PointId == pointlist.ID && u.CreateDate == date && u.Level > 1).OrderByDescending(u => u.CreateDate).ToList().Select(u =>
                                        new MonitorSensorValue
                                        {
                                            Value1 = (u.ScanValue ?? 0).ToString("F3"),
                                            Value2 = (u.AlertValue ?? 0).ToString("F3"),
                                            CreateTime = u.CreateDate.Value.ToString(),
                                            Level = u.Level ?? 0
                                        }
                            ).ToList();
                            // ret.TotalCount = service.MK_DataValue.Where(u => u.PointId == pointlist.ID && u.CreateDate == date).Count();
                            if (valuelist.Count() == 0)
                            {
                                layer.IconClass = "icon_zc.png";
                            }
                            else
                            {
                                layer.IconClass = "icon_bj.png";

                            }
                            layer.Level = valuelist.Count();
                        }
                        else
                        {
                            //ret.title1 = "最大值";
                            //ret.title2 = "有效值";
                            // ret.title1 = "监测值(MPa)";

                            DateTime begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            DateTime end = DateTime.Now;

                            //ret.title1 = "监测值";
                            //ret.title2 = "初始值";
                            valuelist = service.MK_Alert.Where(u => u.PointId == pointlist.ID && u.CreateDate >= begin && u.CreateDate < end).ToList().Select(u =>
                                         new MonitorSensorValue
                                         {
                                             Value1 = (u.ScanValue ?? 0).ToString("F3"),
                                             Value2 = (u.AlertValue ?? 0).ToString("F3"),
                                             Level = u.Level ?? 0,
                                             CreateTime = u.CreateDate.Value.ToString()
                                         }
                            ).ToList();

                            
                            layer.Level = valuelist.Where(u => Convert.ToDateTime(u.CreateTime) >= DateTime.Now.AddMinutes(-30)).Count();
                            if (layer.Level == 0)
                            {
                                layer.IconClass = "icon_zc.png";
                            }
                            else
                            {
                                layer.IconClass = "icon_bj.png";

                            }
                        }


                        //if (ret.TotalCount == 0)
                        //{
                        //    layer.IconClass = "specialgray";
                        //}
                        //else if (valuelist.Count == 0)
                        //{
                        //    layer.IconClass = "";
                        //}
                        //else
                        //{
                        //    foreach (var value in valuelist)
                        //    {
                        //        if (value.Level == 1)
                        //        {
                        //            layer.IconClass = "specialyellow";
                        //        }
                        //        if (value.Level == 2)
                        //        {
                        //            layer.IconClass = "specialorange";
                        //        }
                        //        if (value.Level == 3)
                        //        {
                        //            layer.IconClass = "specialred";
                        //        }
                        //    }
                        //}


                        //if (layer.IsAlarm == true)
                        //{
                        //    break;
                        //}

                    }

                }
                //var list = service.MK_ScanType.Select(u => new MonitorType
                //{
                //    MonitorName = u.TypeName,
                //    ID = u.ID,
                //    IconClass = u.Description
                //}).ToList();
                ret.Data = Info;
                //ret.Data = list;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorType>();
                ret.Message = ex.Message;
                ret.Success = false;
            }
            return ret;
        }

        public BaseReturn GetMonitorTypeInfo(int TypeId)
        {
            BaseReturn ret = new BaseReturn();
            try
            {

                var Info = service.MK_ScanType.Where(u => u.ID == TypeId).FirstOrDefault();
                //var list = service.MK_ScanType.Select(u => new MonitorType
                //{
                //    MonitorName = u.TypeName,
                //    ID = u.ID,
                //    IconClass = u.Description
                //}).ToList();
                ret.Data = Info;
                //ret.Data = list;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorType>();
                ret.Message = ex.Message;
                ret.Success = false;
            }
            return ret;
        }

        public BaseReturn GetMonitorType(string name)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var tempLst = service.MK_Layer.Where(a => a.LayerName == name).Select(u => new
                {
                    ScanTypeId = u.ScanTypeId,
                    Name = service.MK_ScanType.Where(a => a.ID == u.ScanTypeId).Select(a => a.TypeName)
                }).ToList();

                //var list = service.MK_ScanType.Select(u => new MonitorType
                //{
                //    MonitorName = u.TypeName,
                //    ID = u.ID,
                //    IconClass = u.Description
                //}).ToList();
                ret.Data = tempLst;
                //ret.Data = list;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorType>();
                ret.Message = ex.Message;
                ret.Success = false;
            }
            return ret;

        }


        //public BaseReturn GetMonitorLayers(int MonitorTypeID)
        //{
        //    BaseReturn ret = new BaseReturn();
        //    try
        //    {

        //        //var list = service.MK_Layer.GroupBy(a=>a.LayerName,b=>b.Description);
        //        var gplist = service.MK_Layer.Where(u => u.ScanTypeId == MonitorTypeID).OrderBy(u => u.LayerName).Select(a => new MonitorLayers
        //        {
        //            MonitorAreaID = a.ID,
        //            MonitorAreaName = a.LayerName,
        //            IconClass = a.Description,
        //            NewMapX = a.NewMapX,
        //            NewMapY = a.NewMapY,
        //            NewMapZ = a.NewMapZ,
        //            X = a.MapX,
        //            Y = a.MapY
        //        }).ToList();

        //        // var list = new List<MonitorLayers>();

        //        //foreach (var item in gplist)
        //        //{
        //        //    var temp = new MonitorLayers();

        //        //    temp.MonitorAreaName = item.LayerName;
        //        //    temp.IconClass = item.Description;
        //        //    temp.X = item.MapX;
        //        //    temp.Y = item.MapY;

        //        //    list.Add(temp);
        //        //}

        //        ret.Data = gplist;
        //        ret.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ret.Data = new List<MonitorLayers>();
        //        ret.Message = ex.Message;
        //        ret.Success = false;
        //    }
        //    return ret;
        //}


        public BaseReturn GetMonitorLayers(int MonitorTypeID)
        {
            BaseReturn ret = new BaseReturn();
            try
            {

                //var list = service.MK_Layer.GroupBy(a=>a.LayerName,b=>b.Description);
                var gplist = service.MK_Layer.Where(u => u.ScanTypeId == MonitorTypeID).OrderBy(u => u.LayerName).Select(a => new MonitorLayers
                {
                    MonitorAreaID = a.ID,
                    MonitorAreaName = a.LayerName,
                    IconClass = a.Description,
                    NewMapX = a.NewMapX,
                    NewMapY = a.NewMapY,
                    NewMapZ = a.NewMapZ,
                    X = a.MapX,
                    Y = a.MapY
                }).ToList();


                foreach (var layer in gplist)
                {
                    layer.IconClass = "point1";
                    layer.Level = 0;
                    var list = (from a in service.MK_Position.Where(u => u.LayerId == layer.MonitorAreaID)
                                join b in service.MK_Point on a.ID equals b.PositionId
                                select new MonitorPosition
                                {
                                    MonitorPositionName = a.PositionName,
                                    PositionId = a.ID,
                                    // BackGroundPath = a.BackGroundPath,
                                    PointName = b.PointName,
                                    MaxValue = b.MaxNum,
                                    MinValue = uint.MinValue,
                                    ID = b.ID,
                                    X = b.MapX,
                                    Y = b.MapY,
                                    DeviceId = b.DeviceId,
                                    DataCount = 0,
                                    Level = 0,
                                    IconClass = "",
                                    IsAlarm = false
                                }).ToList();


                    foreach (var point in list)
                    {
                        var device = service.MK_Device.FirstOrDefault(u => u.ID == point.DeviceId);
                        if (device == null) continue;
                        if (device.DeviceProtocol == 0)
                        {
                            //ret.title1 = "监测值";
                            //  ret.title2 = "原始值";
                            //var temp = service.MK_DataValue.Where(u => u.PointId == point.ID && u.CreateDate >= BeginTime && u.CreateDate < EndTime).OrderByDescending(u => u.CreateDate).Take(10).OrderBy(a => a.CreateDate).ToList();
                            //point.MonitorValues = temp.Select(u =>
                            //             new MonitorSensorValue
                            //             {
                            //                 Value1 = (u.ScanValue5 ?? 0).ToString("F3"),
                            //                 Value2 = (u.ScanValue ?? 0).ToString("F3"),
                            //                 IsAlarm = false,
                            //                 CreateTime = u.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                            //             }
                            //).ToList();


                            ret.title1 = "监测值(mm)";

                            DateTime begin = DateTime.Now.AddMinutes(-30);
                            DateTime end = DateTime.Now;

                            //ret.title1 = "监测值";
                            //ret.title2 = "初始值";
                            point.MonitorValues = service.MK_Alert.Where(u => u.PointId == point.ID && u.CreateDate >= begin && u.CreateDate < end).ToList().Select(u =>
                                         new MonitorSensorValue
                                         {
                                             Value1 = (u.ScanValue ?? 0).ToString("F3"),
                                             Value2 = (u.AlertValue ?? 0).ToString("F3"),
                                             Level = u.Level ?? 0
                                         }
                            ).ToList();

                            layer.Level += point.MonitorValues.Count();

                        }
                        else
                        {
                            //ret.title1 = "最大值";
                            //ret.title2 = "有效值";
                            ret.title1 = "监测值(MPa)";
                            //  ret.title2 = "初始值";
                            //string DBName = "FD_DynamicValue3" + DateTime.Now.ToString("yyyyMM");

                            //var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                            //if (Exist.ID == 0)
                            //{
                            //    point.MonitorValues = new List<MonitorSensorValue>();
                            //    continue;
                            //}

                            //string sql = string.Format("select top(10) ID,CreateDate,ScanValue,Result from {0} where PointId={1} and CreateDate>='{2}' and CreateDate<'{3}' order By CreateDate desc", DBName, point.ID, BeginTime, EndTime);
                            //point.MonitorValues = service.ExecuteQuery<FD_Dynamic>(sql).OrderBy(a => a.CreateDate).Select(u => new MonitorSensorValue
                            //{
                            //    Value1 = u.ScanValue.ToString("F3"),
                            //   // Value2 = u.YXValue,
                            //    IsAlarm = false,
                            //    CreateTime = u.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")
                            //}).ToList();
                            //valuelist = service.ExecuteQuery<FD_Dynamic>(sql).Select(u => new MonitorSensorValue
                            //{
                            //    Value1 = u.ScanValue.ToString("F3"),
                            //    // Value2 = u.YXValue
                            //}).ToList();


                            ret.title1 = "监测值(MPa)";

                            DateTime begin = DateTime.Now.AddMinutes(-30);
                            DateTime end = DateTime.Now;

                            //ret.title1 = "监测值";
                            //ret.title2 = "初始值";
                            point.MonitorValues = service.MK_Alert.Where(u => u.PointId == point.ID && u.CreateDate >= begin && u.CreateDate < end).ToList().Select(u =>
                                         new MonitorSensorValue
                                         {
                                             Value1 = (u.ScanValue ?? 0).ToString("F3"),
                                             Value2 = (u.ALertValue ?? 0).ToString("F3"),
                                             Level = u.Level ?? 0
                                         }
                            ).ToList();

                            //  ret.title2 = "初始值";
                            //string DBName = "FD_DynamicValue3" + DateTime.Now.ToString("yyyyMM");

                            //var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                            //if (Exist.ID == 0)
                            //{
                            //    //  ret.Success = false;
                            //    //model.IsAlarm = false;
                            //    continue;
                            //}
                            //else
                            //{

                            //    string sql = string.Format("select top(10) ID,CreateDate,ScanValue,Result from {0} where PointId={1} and CreateDate>='{2}' and CreateDate<='{3}' order By CreateDate desc", DBName, point.ID, begin, end);
                            //    //valuelist = service.ExecuteQuery<FD_Dynamic>(sql).Select(u => new MonitorSensorValue
                            //    //{
                            //    //    Value1 = u.ScanValue.ToString("F3"),
                            //    //    // Value2 = u.YXValue
                            //    //}).ToList();
                            //    ret.TotalCount = service.ExecuteQuery<FD_Dynamic>(sql).Count();
                            //}
                            layer.Level += point.MonitorValues.Count();


                        }
                        int count = 0;

                        //if (ret.TotalCount == 0)
                        //{
                        //    point.IconClass = "specialgray";
                        //}
                        //else if (point.MonitorValues.Count == 0)
                        //{
                        //    point.IconClass = "";
                        //}
                        //else
                        //{
                        //foreach (var value in point.MonitorValues)
                        //{
                        //    layer.IconClass = "point2";
                        //    break;
                        //    //if (value.Level == 1)
                        //    //{
                        //    //    point.IconClass = "specialyellow";
                        //    //}
                        //    //if (value.Level == 2)
                        //    //{
                        //    //    point.IconClass = "specialorange";
                        //    //}
                        //    //if (value.Level == 3)
                        //    //{
                        //    //    point.IconClass = "specialred";
                        //    //}
                        //}
                        ////}
                        //if (layer.IconClass.Equals("point2"))
                        //{
                        //    break;
                        //}
                    }
                    if(layer.Level>0)
                    {
                        layer.IconClass = "point2";
                    }
                }
                // var list = new List<MonitorLayers>();

                //foreach (var item in gplist)
                //{
                //    var temp = new MonitorLayers();

                //    temp.MonitorAreaName = item.LayerName;
                //    temp.IconClass = item.Description;
                //    temp.X = item.MapX;
                //    temp.Y = item.MapY;

                //    list.Add(temp);
                //}

                ret.Data = gplist;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorLayers>();
                ret.Message = ex.Message;
                ret.Success = false;
            }
            return ret;
        }

        public BaseReturn getMonitorArea(int MonitorTypeID)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var list = (from a in service.MK_Layer.Where(u => u.ScanTypeId == MonitorTypeID)
                            join b in service.MK_ScanType on a.ScanTypeId equals b.ID
                            select new MonitorArea
                            {
                                ImgName = b.ImagePath,
                                MonitorAreaName = a.LayerName,
                                MonitorTypeID = b.ID,
                                MonitorTypeName = b.TypeName,
                                ID = a.ID,
                                IsCommand = b.IsCommand,
                                NewMapX = a.NewMapX,
                                NewMapY = a.NewMapY,
                                NewMapZ = a.NewMapZ,

                                IsAlarm = false,
                                X = a.MapX,
                                Y = a.MapY,
                                IconClass = a.Description
                            }).ToList();
                foreach (var layer in list)
                {
                    var pointlist = (from b in service.MK_Position.Where(u => u.LayerId == layer.ID)
                                     join c in service.MK_Point on b.ID equals c.PositionId
                                     join d in service.MK_Device on c.DeviceId equals d.ID
                                     select new { c.ID, c.MaxNum, c.MinNum, d.DeviceProtocol }).FirstOrDefault();
                    // var point = pointlist.FirstOrDefault();
                    if (pointlist != null)
                    {
                        List<MonitorSensorValue> valuelist = new List<MonitorSensorValue>();
                        if (pointlist.DeviceProtocol == 0)
                        {
                            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            ret.title1 = "监测值(mm)";
                           // ret.title2 = "初始值";
                            valuelist = service.MK_DataValue.Where(u => u.PointId == pointlist.ID && u.CreateDate==date).OrderByDescending(u => u.CreateDate).Take(10).ToList().Select(u =>
                                    new MonitorSensorValue
                                    {
                                        CreateTime = u.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                        Value1 = (u.ScanValue5 ?? 0).ToString("F3"),
                                        Value2 = (u.ScanValue ?? 0).ToString("F3")
                                    }
                            ).ToList();
                        }
                        else
                        {
                            //ret.title1 = "最大值";
                            //ret.title2 = "有效值";
                            ret.title1 = "监测值(MPa)";
                            //  ret.title2 = "初始值";
                            string DBName = "FD_DynamicValue3" + DateTime.Now.ToString("yyyyMM");

                            var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                            if (Exist.ID == 0)
                            {
                                //  ret.Success = false;
                                //model.IsAlarm = false;
                                continue;
                            }
                            else
                            {

                                string sql = string.Format("select top(10) ID,CreateDate,ScanValue,Result from {0} where PointId={1} order By CreateDate desc", DBName, pointlist.ID);
                                valuelist = service.ExecuteQuery<FD_Dynamic>(sql).Select(u => new MonitorSensorValue
                                {
                                    Value1 = u.ScanValue.ToString("F3"),
                                    // Value2 = u.YXValue
                                }).ToList();
                            }
                        }
                        foreach (var value in valuelist)
                        {
                            if (value.Value1.AsDouble(0) > pointlist.MaxNum || value.Value1.AsDouble(0) < pointlist.MinNum)
                            {
                                layer.IsAlarm = true;
                                break;
                                //model.IsAlarm = true;
                            }
                        }
                        if (layer.IsAlarm == true)
                        {
                            break;
                        }

                    }

                }
                ret.Data = list;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorArea>();
                ret.Message = ex.Message;
                ret.Success = false;
            }
            return ret;
        }

        public BaseReturn getMonitorArea()
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var layerlist = service.MK_Layer.ToList();
                //var layerlist = service.MK_Layer.Where(u => u.LayerName.Contains("PHC")).ToList();
                List<MonitorArea> list = new List<MonitorArea>();
                foreach (var layer in layerlist)
                {
                    var scantype = service.MK_ScanType.FirstOrDefault(u => u.ID == layer.ScanTypeId);
                    if (scantype != null)
                    {
                        MonitorArea model = new MonitorArea();
                        model.ID = layer.ID;
                        model.MonitorAreaName = layer.LayerName;
                        model.X = layer.MapX;
                        model.Y = layer.MapY;
                        model.IsAlarm = false;
                        var point = (from a in layerlist.Where(u => u.ID == layer.ID)
                                     join b in service.MK_Position
                                     on a.ID equals b.LayerId
                                     join c in service.MK_Point on b.ID equals c.PositionId
                                     join d in service.MK_Device on c.DeviceId equals d.ID
                                     select new { c.ID, c.MaxNum, c.MinNum, d.DeviceProtocol }).FirstOrDefault();
                        if (point != null)
                        {
                            List<MonitorSensorValue> valuelist = new List<MonitorSensorValue>();
                            if (point.DeviceProtocol == 0)
                            {
                                ret.title1 = "监测值(mm)";
                                // ret.title2 = "原始值";
                                valuelist = service.MK_DataValue.Where(u => u.PointId == point.ID).OrderByDescending(u => u.CreateDate).Take(10).Select(u =>
                                        new MonitorSensorValue
                                        {
                                            Value1 = (u.ScanValue5 ?? 0).ToString("F3"),
                                            //    Value2 = u.ScanValue ?? 0
                                        }
                                ).ToList();
                            }
                            else
                            {
                                //ret.title1 = "最大值";
                                //ret.title2 = "有效值";
                                ret.title1 = "监测值(MPa)";
                                // ret.title2 = "初始值";
                                string DBName = "FD_DynamicValue3" + DateTime.Now.ToString("yyyyMM");

                                var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                                if (Exist.ID == 0)
                                {
                                    //  ret.Success = false;
                                    //model.IsAlarm = false;
                                    continue;
                                }
                                else
                                {
                                    string sql = string.Format("select top(10) ID,CreateDate,ScanValue,Result from {0} where PointId={1} order By CreateDate desc", DBName, point.ID);
                                    valuelist = service.ExecuteQuery<FD_Dynamic>(sql).Select(u => new MonitorSensorValue
                                    {
                                        Value1 = u.ScanValue.ToString("F3"),
                                        //  Value2 = u.YXValue
                                    }).ToList();
                                }
                            }
                            foreach (var value in valuelist)
                            {
                                if (value.Value1.AsDouble(0) > point.MaxNum || value.Value1.AsDouble(0) < point.MinNum)
                                {
                                    model.IsAlarm = true;
                                    break;
                                    //model.IsAlarm = true;
                                }
                            }
                            if (model.IsAlarm == true)
                            {
                                break;
                            }

                        }
                        list.Add(model);
                    }
                }

                ret.Data = list;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorArea>();
                ret.Message = ex.Message;
                ret.Success = false;
            }
            return ret;
        }

        public BaseReturn getMonitorPoint(int MonitorPositionID)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var list = service.MK_Point.Where(u => u.PositionId == MonitorPositionID).Select(u => new MonitorPoint
                {
                    MonitorPointName = u.PointName,
                    ID = u.ID
                }).ToList();
                ret.Data = list;
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorPoint>();
                ret.Message = ex.Message;
                ret.Success = false;
            }
            return ret;
        }

        public BaseReturn getMonitorValue(int MonitorPointID)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                int IsCommand = 1;
                var model = (from a in service.MK_Point.Where(u => u.ID == MonitorPointID)
                             join b in service.MK_Position on a.PositionId equals b.ID
                             join c in service.MK_Layer on b.LayerId equals c.ID
                             join d in service.MK_ScanType on c.ScanTypeId equals d.ID
                             join e in service.MK_Device on a.DeviceId equals e.ID
                             select new
                             {
                                 MaxValue = a.MaxNum,
                                 MinValue = a.MinNum,
                                 e.DeviceProtocol,
                                 IsCommand = d.IsCommand
                             }).FirstOrDefault();
                if (model != null)
                {
                    List<MonitorSensorValue> list = new List<MonitorSensorValue>();
                    if (model.DeviceProtocol == 0)
                    {
                        ret.title1 = "监测值(mm)";
                        ret.title2 = "原始值";
                        list = service.MK_DataValue.Where(u => u.PointId == MonitorPointID).OrderByDescending(u => u.CreateDate).Take(10).Select(u =>
                                new MonitorSensorValue
                                {
                                    Value1 = (u.ScanValue5 ?? 0).ToString("F3"),
                                    Value2 = (u.ScanValue ?? 0).ToString("F3"),
                                    IsAlarm = false,
                                    CreateTime = u.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                }
                        ).ToList();
                    }
                    else
                    {
                        //ret.title1 = "最大值";
                        //ret.title2 = "有效值";
                        ret.title1 = "监测值(MPa)";
                        ret.title2 = "初始值";
                        string DBName = "FD_DynamicValue3" + DateTime.Now.ToString("yyyyMM");

                        var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                        if (Exist.ID == 0)
                        {
                            ret.Success = false;
                            ret.Data = new List<MonitorSensorValue>();
                        }
                        else
                        {
                            string sql = string.Format("select top(10) ID,CreateDate,ScanValue,Result from {0} where PointId={1} order By CreateDate desc", DBName, MonitorPointID);
                            list = service.ExecuteQuery<FD_Dynamic>(sql).Select(u => new MonitorSensorValue
                            {
                                Value1 = u.ScanValue.ToString("F3"),
                                // Value2 = u.YXValue,
                                IsAlarm = false,
                                CreateTime = u.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")
                            }).ToList();
                        }
                    }

                    foreach (var data in list)
                    {
                        if (data.Value1.AsDouble(0) > model.MaxValue || data.Value1.AsDouble(0) < model.MinValue)
                        {
                            data.IsAlarm = true;
                            //model.IsAlarm = true;
                        }

                    }
                    if (list.Count > 0)
                    {
                        ret.Data = list;
                        ret.Success = true;
                    }
                }
                else
                {
                    ret.Data = new List<MonitorSensorValue>();

                    ret.Success = false;
                }
                return ret;

            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorSensorValue>();
                ret.Success = false;
            }
            return ret;
        }

        //public BaseReturn getMonitorPosition(int MonitorAreaID, DateTime? BeginTime, DateTime? EndTime)
        //{
        //    BaseReturn ret = new BaseReturn();
        //    try
        //    {

        //        var Layer = service.MK_Layer.FirstOrDefault(u => u.ID == MonitorAreaID);
        //        if (Layer != null)
        //        {
        //            int IsCommand = 1;
        //            ret.Url = Layer.BackgroundPath;
        //            var ScanType = service.MK_ScanType.FirstOrDefault(u => u.ID == Layer.ScanTypeId);
        //            if (ScanType != null)
        //            {
        //                IsCommand = ScanType.IsCommand ?? 0;
        //            }
        //            var list = (from a in service.MK_Position.Where(u => u.LayerId == MonitorAreaID)
        //                        join b in service.MK_Point on a.ID equals b.PositionId
        //                        select new MonitorPosition
        //                        {
        //                            MonitorPositionName = a.PositionName,
        //                            PositionId = a.ID,
        //                            // BackGroundPath = a.BackGroundPath,
        //                            PointName = b.PointName,
        //                            MaxValue = b.MaxNum,
        //                            MinValue = uint.MinValue,
        //                            ID = b.ID,
        //                            X = b.MapX,
        //                            Y = b.MapY,
        //                            DeviceId = b.DeviceId,
        //                            DataCount = 0,
        //                            IsAlarm = false
        //                        }).ToList();
        //            //foreach (var model in list)
        //            //{
        //            //model.MonitorPoints = service.MK_Point.Where(u => u.PositionId == model.ID).Select(u => new MonitorPoint
        //            //{
        //            //    ID = u.ID,
        //            //    MaxValue = u.MaxNum,
        //            //    X = u.MapX,
        //            //    Y = u.MapY,
        //            //    //MaxValue2 = u.MaxNum2 ?? 0,
        //            //    MinValue = u.MinNum,

        //            //    // MinValue2 = u.MinNum2 ?? 0,
        //            //    MonitorPointName = u.PointName
        //            //}).ToList();
        //            foreach (var point in list)
        //            {
        //                var device = service.MK_Device.FirstOrDefault(u => u.ID == point.DeviceId);
        //                if (device == null) continue;
        //                if (device.DeviceProtocol == 0)
        //                {
        //                    ret.title1 = "监测值(mm)";
        //                    //  ret.title2 = "原始值";
        //                    var temp = service.MK_DataValue.Where(u => u.PointId == point.ID && u.CreateDate >= BeginTime && u.CreateDate < EndTime).OrderByDescending(u => u.CreateDate).Take(10).OrderBy(a => a.CreateDate).ToList();
        //                    point.MonitorValues = temp.Select(u =>
        //                                 new MonitorSensorValue
        //                                 {
        //                                     Value1 = (u.ScanValue5 ?? 0).ToString("F3"),
        //                                     Value2 = (u.ScanValue ?? 0).ToString("F3"),
        //                                     IsAlarm = false,
        //                                     CreateTime = u.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
        //                                 }
        //                    ).ToList();
        //                }
        //                else
        //                {
        //                    //ret.title1 = "最大值";
        //                    //ret.title2 = "有效值";
        //                    ret.title1 = "监测值(MPa)";
        //                    //  ret.title2 = "初始值";
        //                    string DBName = "FD_DynamicValue3" + DateTime.Now.ToString("yyyyMM");

        //                    var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
        //                    if (Exist.ID == 0)
        //                    {
        //                        point.MonitorValues = new List<MonitorSensorValue>();
        //                        continue;
        //                    }

        //                    string sql = string.Format("select top(10) ID,CreateDate,ScanValue,Result from {0} where PointId={1} and CreateDate>='{2}' and CreateDate<'{3}' order By CreateDate desc", DBName, point.ID, BeginTime, EndTime);
        //                    point.MonitorValues = service.ExecuteQuery<FD_Dynamic>(sql).OrderBy(a => a.CreateDate).Select(u => new MonitorSensorValue
        //                    {
        //                        Value1 = u.ScanValue.ToString("F3"),
        //                        // Value2 = u.YXValue,
        //                        IsAlarm = false,
        //                        CreateTime = u.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")
        //                    }).ToList();

        //                }
        //                int count = 0;
        //                point.DataCount = point.MonitorValues.Count;
        //                foreach (var data in point.MonitorValues)
        //                {
        //                    if (point.MaxValue.AsDouble(0.0) != 0.0 || point.MinValue.AsDouble(0.0) != 0)
        //                    {
        //                        if (data.Value1.AsDouble(0) > point.MaxValue || data.Value1.AsDouble(0) < point.MinValue)
        //                        {
        //                            if (count == 0)
        //                            {
        //                                point.IsAlarm = true;
        //                            }
        //                            data.IsAlarm = true;
        //                            // model.IsAlarm = true;
        //                        }
        //                    }
        //                    count++;

        //                }
        //            }
        //            //   }
        //            ret.Success = true;
        //            ret.Data = list;
        //        }
        //        else
        //        {
        //            ret.Data = new List<MonitorPosition>();
        //            ret.Success = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ret.Data = new List<MonitorPosition>();
        //        ret.Message = ex.Message;
        //        ret.Success = false;
        //    }
        //    return ret;
        //}




        public BaseReturn getMonitorPosition(int MonitorAreaID, DateTime? BeginTime, DateTime? EndTime)
        {
            BaseReturn ret = new BaseReturn();
            try
            {

                var Layer = service.MK_Layer.FirstOrDefault(u => u.ID == MonitorAreaID);
                if (Layer != null)
                {
                    int IsCommand = 1;
                    ret.Url = Layer.BackgroundPath;
                    var ScanType = service.MK_ScanType.FirstOrDefault(u => u.ID == Layer.ScanTypeId);
                    if (ScanType != null)
                    {
                        IsCommand = ScanType.IsCommand ?? 0;
                    }
                    var list = (from a in service.MK_Position.Where(u => u.LayerId == MonitorAreaID)
                                join b in service.MK_Point on a.ID equals b.PositionId
                                select new MonitorPosition
                                {
                                    MonitorPositionName = a.PositionName,
                                    PositionId = a.ID,
                                    // BackGroundPath = a.BackGroundPath,
                                    PointName = b.PointName,
                                    MaxValue = b.MaxNum,
                                    MinValue = uint.MinValue,
                                    ID = b.ID,
                                    X = b.MapX,
                                    Y = b.MapY,
                                    DeviceId = b.DeviceId,
                                    DataCount = 0,
                                    Level = 0,
                                    IconClass = "",
                                    IsAlarm = false
                                }).ToList();
                    //foreach (var model in list)
                    //{
                    //model.MonitorPoints = service.MK_Point.Where(u => u.PositionId == model.ID).Select(u => new MonitorPoint
                    //{
                    //    ID = u.ID,
                    //    MaxValue = u.MaxNum,
                    //    X = u.MapX,
                    //    Y = u.MapY,
                    //    //MaxValue2 = u.MaxNum2 ?? 0,
                    //    MinValue = u.MinNum,

                    //    // MinValue2 = u.MinNum2 ?? 0,
                    //    MonitorPointName = u.PointName
                    //}).ToList();
                    foreach (var point in list)
                    {
                        var device = service.MK_Device.FirstOrDefault(u => u.ID == point.DeviceId);
                        if (device == null) continue;
                        if (device.DeviceProtocol == 0)
                        {
                            //ret.title1 = "监测值";
                            //  ret.title2 = "原始值";
                            //var temp = service.MK_DataValue.Where(u => u.PointId == point.ID && u.CreateDate >= BeginTime && u.CreateDate < EndTime).OrderByDescending(u => u.CreateDate).Take(10).OrderBy(a => a.CreateDate).ToList();
                            //point.MonitorValues = temp.Select(u =>
                            //             new MonitorSensorValue
                            //             {
                            //                 Value1 = (u.ScanValue5 ?? 0).ToString("F3"),
                            //                 Value2 = (u.ScanValue ?? 0).ToString("F3"),
                            //                 IsAlarm = false,
                            //                 CreateTime = u.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                            //             }
                            //).ToList();


                            ret.title1 = "监测值(MM)";

                            DateTime begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            DateTime end = DateTime.Now;

                            //ret.title1 = "监测值";
                            //ret.title2 = "初始值";
                            point.MonitorValues = service.MK_Alert.Where(u => u.PointId == point.ID && u.CreateDate >= begin && u.CreateDate < end).ToList().Select(u =>
                                         new MonitorSensorValue
                                         {
                                             Value1 = (u.ScanValue ?? 0).ToString("F3"),
                                             Value2 = (u.AlertValue ?? 0).ToString("F3"),
                                             Level = u.Level ?? 0
                                         }
                            ).ToList();

                            ret.TotalCount = service.MK_DataValue.Where(u => u.PointId == point.ID && u.CreateDate >= begin && u.CreateDate < end).Count();

                        }
                        else
                        {
                            //ret.title1 = "最大值";
                            //ret.title2 = "有效值";
                            ret.title1 = "监测值(MPa)";
                            //  ret.title2 = "初始值";
                            //string DBName = "FD_DynamicValue3" + DateTime.Now.ToString("yyyyMM");

                            //var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                            //if (Exist.ID == 0)
                            //{
                            //    point.MonitorValues = new List<MonitorSensorValue>();
                            //    continue;
                            //}

                            //string sql = string.Format("select top(10) ID,CreateDate,ScanValue,Result from {0} where PointId={1} and CreateDate>='{2}' and CreateDate<'{3}' order By CreateDate desc", DBName, point.ID, BeginTime, EndTime);
                            //point.MonitorValues = service.ExecuteQuery<FD_Dynamic>(sql).OrderBy(a => a.CreateDate).Select(u => new MonitorSensorValue
                            //{
                            //    Value1 = u.ScanValue.ToString("F3"),
                            //   // Value2 = u.YXValue,
                            //    IsAlarm = false,
                            //    CreateTime = u.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")
                            //}).ToList();
                            //valuelist = service.ExecuteQuery<FD_Dynamic>(sql).Select(u => new MonitorSensorValue
                            //{
                            //    Value1 = u.ScanValue.ToString("F3"),
                            //    // Value2 = u.YXValue
                            //}).ToList();


                            ret.title1 = "监测值(MPa)";

                            DateTime begin = DateTime.Now.AddMinutes(-30);
                            DateTime end = DateTime.Now;

                            //ret.title1 = "监测值";
                            //ret.title2 = "初始值";
                            point.MonitorValues = service.MK_Alert.Where(u => u.PointId == point.ID && u.CreateDate >= begin && u.CreateDate < end).ToList().Select(u =>
                                         new MonitorSensorValue
                                         {
                                             Value1 = (u.ScanValue ?? 0).ToString("F3"),
                                             Value2 = (u.ALertValue ?? 0).ToString("F3"),
                                             Level = u.Level ?? 0
                                         }
                            ).ToList();

                            //  ret.title2 = "初始值";
                            string DBName = "FD_DynamicValue3" + DateTime.Now.ToString("yyyyMM");

                            var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                            if (Exist.ID == 0)
                            {
                                //  ret.Success = false;
                                //model.IsAlarm = false;
                                continue;
                            }
                            else
                            {

                                string sql = string.Format("select top(10) ID,CreateDate,ScanValue,Result from {0} where PointId={1} and CreateDate>='{2}' and CreateDate<='{3}' order By CreateDate desc", DBName, point.ID, begin, end);
                                //valuelist = service.ExecuteQuery<FD_Dynamic>(sql).Select(u => new MonitorSensorValue
                                //{
                                //    Value1 = u.ScanValue.ToString("F3"),
                                //    // Value2 = u.YXValue
                                //}).ToList();
                                ret.TotalCount = service.ExecuteQuery<FD_Dynamic>(sql).Count();
                            }



                        }
                        int count = 0;

                        if (ret.TotalCount == 0)
                        {
                            point.IconClass = "specialgray";
                        }
                        else if (point.MonitorValues.Count == 0)
                        {
                            point.IconClass = "";
                        }
                        else
                        {
                            foreach (var value in point.MonitorValues)
                            {
                                if (value.Level == 1)
                                {
                                    point.IconClass = "specialyellow";
                                }
                                if (value.Level == 2)
                                {
                                    point.IconClass = "specialorange";
                                }
                                if (value.Level == 3)
                                {
                                    point.IconClass = "specialred";
                                }
                            }
                        }
                        // point.DataCount = point.MonitorValues.Count;
                        //foreach (var data in point.MonitorValues)
                        //{
                        //    if (point.MaxValue.AsDouble(0.0) != 0.0 || point.MinValue.AsDouble(0.0) != 0)
                        //    {
                        //        if (data.Value1.AsDouble(0) > point.MaxValue || data.Value1.AsDouble(0) < point.MinValue)
                        //        {
                        //            if (count == 0)
                        //            {
                        //                point.IsAlarm = true;
                        //            }
                        //            data.IsAlarm = true;
                        //            // model.IsAlarm = true;
                        //        }
                        //    }
                        //    count++;

                        //}


                    }
                    //   }
                    ret.Success = true;
                    ret.Data = list;
                }
                else
                {
                    ret.Data = new List<MonitorPosition>();
                    ret.Success = false;
                }
            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorPosition>();
                ret.Message = ex.Message;
                ret.Success = false;
            }
            return ret;
        }




        //public BaseReturn getMonitorPosition(string areaName, int scanTypeID)
        //{
        //    int areaId = service.MK_Layer.Where(a => (a.LayerName == areaName && a.ScanTypeId.Value == scanTypeID)).FirstOrDefault().ID;

        //        return getMonitorPosition(areaId);
        //}

        public MonitorAreaDetail getMonitorAreaDetail(int MonitorAreaID)
        {
            MonitorAreaDetail ret = new MonitorAreaDetail();
            try
            {

                ret = (from a in service.MK_Layer.Where(u => u.ID == MonitorAreaID)
                       join b in service.MK_ScanType on a.ScanTypeId equals b.ID
                       select new MonitorAreaDetail
                       {
                           ID = a.ID,
                           MonitorAreaName = a.LayerName,
                           MonitorTypeID = b.ID,
                           MonitorTypeName = b.TypeName,
                           BackGroundPic = a.BackgroundPath2,
                           MonitorAreaPic = a.BackgroundPath
                       }).FirstOrDefault();
                return ret;
            }
            catch (Exception ex)
            {
                return new MonitorAreaDetail();
            }
        }


        public MonitorAreaDetail getMonitorAreaDetail(string MonitorAreaName)
        {
            MonitorAreaDetail ret = new MonitorAreaDetail();

            try
            {

                ret = (from a in service.MK_Layer.Where(u => u.LayerName == MonitorAreaName)
                       join b in service.MK_ScanType on a.ScanTypeId equals b.ID
                       select new MonitorAreaDetail
                       {
                           ID = a.ID,
                           MonitorAreaName = a.LayerName,
                           MonitorTypeID = b.ID,
                           MonitorTypeName = b.TypeName,
                           BackGroundPic = a.BackgroundPath2,
                           MonitorAreaPic = a.BackgroundPath
                       }).FirstOrDefault();
                return ret;
            }
            catch (Exception ex)
            {
                return new MonitorAreaDetail();
            }
        }



        public List<MonitorDetailType> GetMonitorTypes(string name)
        {
            //var list = service.MK_Layer.Where(a => a.LayerName == name).Select(u => new MonitorDetailType
            //{

            //    ID = u.ScanTypeId.Value,
            //    Name = service.MK_ScanType.Where(a => a.ID == u.ScanTypeId).Select(a => a.TypeName).ToList().FirstOrDefault(),
            //    AreaID = u.ID

            //}).ToList();

            var list = service.MK_ScanType.Where(a => a.ID > 0).Select(u => new MonitorDetailType
            {
                ID = u.ID,
                Name = u.TypeName,
                AreaID = service.MK_Layer.Where(a => a.ScanTypeId == u.ID && a.LayerName == name).Select(a => a.ID).FirstOrDefault(),
                ImgName = u.ImagePath
            }).ToList();


            //var test = service.MK_Layer.Where(a => a.ScanTypeId == 1 && a.LayerName == name).Select(a => a.ID).FirstOrDefault();

            //AreaID ? = service.MK_Layer.Where(a => a.ScanTypeId == u.ID && a.LayerName == name).Select(a => a.ID).FirstOrDefault()

            return list;
        }

        public BaseReturn getMonitorPointInfo(int MonitorPointID, int LayerId)
        {
            BaseReturn ret = new BaseReturn();
            try
            {
                var list = new MonitorPointInfo();

                var pointlist = (from a in service.MK_Point
                                 join b in service.MK_Position on a.PositionId equals b.ID
                                 join c in service.MK_Layer on b.LayerId equals c.ID
                                 select new
                                 {
                                     a.ID,
                                     a.pName,
                                     a.testHeight,
                                     a.Height,
                                     a.SensorNumber,
                                     a.SensorType,
                                     a.PointName,
                                     b.LayerId,
                                     a.PositionId
                                 });
                if (LayerId == 0 || LayerId == null)
                {
                    pointlist = pointlist.Where(u => u.ID == MonitorPointID);
                }
                else
                {
                    pointlist = pointlist.Where(u => u.LayerId == LayerId);
                }
                var point = pointlist.FirstOrDefault();
                if (point != null)
                {
                    list.ID = point.ID;
                    list.SensorName = point.pName;
                    list.PointName = point.PointName;
                    list.TheoryHeight = point.testHeight.ToString();
                    list.ActualHeight = point.Height.ToString();
                    list.PileNumber = point.SensorNumber;
                    if (point.SensorType == 1)
                    {
                        list.SensorType = "FBG-4911光纤光栅式传感器";
                    }
                    else
                    {
                        //list.SensorType = "振弦式传感器";
                        list.SensorType = "FBG-4911光纤光栅式传感器";
                    }
                    var position = service.MK_Position.FirstOrDefault(u => u.ID == point.PositionId);
                    if (position != null)
                    {
                        list.PileLength = position.PileLength;
                        list.PileDiameter = position.PileDiameter;
                        list.PileNumber = position.PileNumber;
                        list.PositionName = position.PositionName;
                    }
                    ret.Data = list;
                    ret.Success = true;
                }
                else
                {
                    ret.Data = new List<MonitorPointInfo>();
                    ret.Success = false;
                }
            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorPointInfo>();
                ret.Message = ex.Message;
                ret.Success = false;
            }
            return ret;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseReturn getMonitorPointData(MonitorPointRequest request)
        {
            DateTime date = DateTime.Now;
            if (request.StartTime != null && request.EndTime <= DateTime.Now)
            {
                date = Convert.ToDateTime(request.StartTime);
            }
            else
            {
                request.EndTime = DateTime.Now;
            }
            BaseReturn ret = new BaseReturn();
            try
            {
                List<MonitorSensorValue> list = new List<MonitorSensorValue>();

                // int IsCommand = 1;
                var model = (from a in service.MK_Point.Where(u => u.ID == request.MonitorPointID)
                             join b in service.MK_Position on a.PositionId equals b.ID
                             join c in service.MK_Layer on b.LayerId equals c.ID
                             join d in service.MK_ScanType on c.ScanTypeId equals d.ID
                             join e in service.MK_Device on a.DeviceId equals e.ID
                             select new
                             {
                                 PointName = a.PointName,
                                 MaxValue = a.MaxNum,
                                 MinValue = a.MinNum,
                                 e.DeviceProtocol,
                                 IsCommand = d.IsCommand
                             }).FirstOrDefault();
                if (model != null)
                {

                    if (model.DeviceProtocol == 0)
                    {
                        ret.title1 = "监测值(mm)";
                        ret.Url = model.PointName;
                        //   ret.title2 = "原始值";
                        var tempquery = service.MK_DataValue.Where(u => u.PointId == request.MonitorPointID).OrderByDescending(u => u.CreateDate);
                        if (request.StartTime == null)
                        {
                            list = tempquery.Take(10).ToList().Select(u =>
                             new MonitorSensorValue
                             {
                                 IsAlarm = false,
                                 CreateTime = u.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                 Value1 = (u.ScanValue5 ?? 0).ToString("F3"),
                                 Value2 = (u.ScanValue ?? 0).ToString("F3")
                             }
                            ).ToList();
                        }
                        else
                        {
                            var tempquery1 = tempquery.Where(u => u.CreateDate <= request.EndTime.Value);
                            int Count = tempquery1.Count();
                            ret.TotalCount = Count;
                            ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                            ////如果不是第一页，并且没有数据，则查询第一页
                            var Currentlist = tempquery1.OrderBy(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                            if (request.PageIndex != 1 && Currentlist.Count == 0)
                            {
                                request.PageIndex = 1;
                                Currentlist = tempquery1.OrderBy(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();

                                //list = tempquery1.OrderBy(a => a.CreateDate).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                            }
                            list = Currentlist.Select(u =>
                             new MonitorSensorValue
                             {
                                 IsAlarm = true,
                                 CreateTime = u.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                 Value1 = (u.ScanValue5 ?? 0).ToString("F3"),
                                 //  Value2 = u.ScanValue ?? 0
                             }
                            ).ToList();
                        }
                    }
                    else
                    {
                        //ret.title1 = "最大值";
                        //ret.title2 = "有效值";
                        ret.title1 = "监测值(MPa)";
                        ret.Url = model.PointName;
                        // ret.title2 = "原始值";
                        int begin = date.ToString("yyyyMM").AsInt();
                        int end = request.EndTime.Value.ToString("yyyyMM").AsInt();

                        for (int i = begin; i <= end; i++)
                        {
                            if (i.ToString().Contains("13"))
                            {
                                i = i + 100 - 12;
                            }

                            string DBName = "FD_DynamicValue3" + i;

                            var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                            if (Exist.ID == 0)
                            {
                                continue;
                                //  ret.Success = false;
                                //model.IsAlarm = false;
                            }
                            else
                            {
                                string sql = "";
                                if (request.StartTime == null)
                                {
                                    sql = string.Format("select top (10) ID,CreateDate,ScanValue,Result from {0} where PointId={1} order By CreateDate desc", DBName, request.MonitorPointID);
                                }
                                else
                                {
                                    sql = string.Format("select ID,CreateDate,ScanValue,Result from {0} where PointId={1} and CreateDate>='{2}' and CreateDate<='{3}' order By CreateDate", DBName, request.MonitorPointID, request.StartTime, request.EndTime);
                                }
                                var query = service.ExecuteQuery<FD_Dynamic>(sql).Select(u => new MonitorSensorValue
                                {
                                    CreateTime = u.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    Value1 = u.ScanValue.ToString("F3"),
                                    IsAlarm = false,
                                    // Value2 = u.YXValue
                                }).ToList();
                                query.ForEach(u =>
                                {
                                    MonitorSensorValue se = new MonitorSensorValue();
                                    se.CreateTime = u.CreateTime;
                                    se.Value1 = u.Value1;
                                    //    se.Value2 = u.Value2;
                                    se.IsAlarm = u.IsAlarm;
                                    list.Add(se);
                                });
                            }
                        }
                        if (request.PageSize.AsInt(0) == 0)
                        {
                            request.PageSize = 1;
                        }
                        ret.Echart = list;
                        int Count = list.Count();
                        ret.TotalCount = Count;
                        ret.PageIndex = request.PageIndex;
                        ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                        ////如果不是第一页，并且没有数据，则查询第一页
                        list = list.OrderBy(a => a.CreateTime).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                        if (request.PageIndex != 1 && list.Count == 0)
                        {
                            request.PageIndex = 1;
                            list = list.OrderBy(a => a.CreateTime).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                        }

                        foreach (var value in list)
                        {
                            if (value.Value1.AsDouble(0) > model.MaxValue || value.Value1.AsDouble(0) < model.MinValue)
                            {
                                value.IsAlarm = true;
                                //model.IsAlarm = true;
                            }
                        }
                    }

                    ret.Data = list;
                    ret.Success = true;
                }
                else
                {
                    ret.Data = new List<MonitorSensorValue>();
                    // ret.Message = ex.Message;
                    ret.Success = false;
                }

                //分页查询代码示例

                //int Count = query.Count();
                //ret.TotalCount = Count;
                //ret.TotalPage = Count % request.PageSize == 0 ? (Count / request.PageSize) : (Count / request.PageSize) + 1;

                ////如果不是第一页，并且没有数据，则查询第一页
                //var CurrentQuery = query.OrderByDescending(a => a.CreateTime).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                //if (request.PageIndex != 1 && CurrentQuery.Count == 0)
                //{
                //    request.PageIndex = 1;
                //    CurrentQuery = query.OrderByDescending(a => a.CreateTime).Skip(request.GetSkipCount()).Take(request.PageSize).ToList();
                //}



            }
            catch (Exception ex)
            {
                ret.Data = new List<MonitorSensorValue>();
                ret.Message = ex.Message;
                ret.Success = false;
            }
            return ret;
        }
    }




    public class FD_Dynamic
    {
        public int ID { set; get; }
        public int PointId { set; get; }
        public int DeviceId { set; get; }
        public int PositionId { set; get; }
        public string PointName { set; get; }
        public string DeviceName { set; get; }
        //public double MAXValue { set; get; }
        //public double YXValue { set; get; }
        public double ScanValue { set; get; }
        public double Result { set; get; }
        public double WaveLength { set; get; }
        public DateTime CreateDate { set; get; }
        public int LayerId { set; get; }
        public int ScanTypeId { set; get; }
        public string LayerName { set; get; }

    }
}
