using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataEntity;
using DataEntity.Services;
using Model.Common;
using Model.HJPlat.Request;
using Model.HJPlat.Response;
using WebAPI.Model;
using WebAPI.Areas.WebApi.Models;
using ZJFramework10.Extension;

namespace WebAPI.Areas.WebApi.Controllers
{
    public class APIController : ApiController
    {
        DBDataContext service = DataContextFactory.Create();
        //[HttpPost]
        //public BaseApiReturn test()
        //{
        //    BaseApiReturn ret = new BaseApiReturn();
        //    try
        //    {
        //        ret.Data = "test";
        //        ret.Message = "成功";
        //    }
        //    catch (Exception ex)
        //    {
        //        ret.Message = ex.Message;
        //    }
            
        //    return ret;
        //}

        [HttpPost]
        public BaseApiReturn getScanType()
        {
            BaseApiReturn ret = new BaseApiReturn();
            try
            {
                var list = service.MK_ScanType.ToList();
                ret.Data = list;
                ret.Message = "成功";
                ret.TotalCount = list.Count();
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Message = ex.Message;
                ret.Success = false;
            }

            return ret;
        }

        [HttpPost]
        public BaseApiReturn getPointList([FromBody]PointRequest request)
        {
            BaseApiReturn ret = new BaseApiReturn();
            try
            {
                //if (request == null)
                //{
                //    request = new ConstructCfgPointRequest();
                //}
                var list = service.View_MK_Point.AsQueryable();
                if(request.ScanTypeID.AsInt(0)>0)
                {
                    list = list.Where(u => u.ScanTypeId == request.ScanTypeID);
                }
                var se = list.Select(u => new
                {
                    u.ID,
                    u.PointName,
                    u.pName,
                    u.MapDataName,
                    u.ProjectName,
                    u.DeviceName,
                    u.TypeName,
                    u.Unit,
                    u.ScanTypeId
                });
                ret.Data = se.ToList();
                ret.TotalCount = se.Count();
                ret.Message = "成功";
                ret.Success = true;
            }
            catch (Exception ex)
            {
                ret.Message = ex.Message;
                ret.Success = false;
            }

            return ret;
        }

        [HttpPost]
        public BaseApiReturn getDataValueList([FromBody]DataRequest request)
        {
            BaseApiReturn ret = new BaseApiReturn();
            try
            {
                if (string.IsNullOrEmpty(request.BeginDate))
                {
                    request.BeginDate = (DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                }
                else
                {
                    request.BeginDate = request.BeginDate + " 00:00:00";
                }
                if (string.IsNullOrEmpty(request.EndDate))
                {
                    request.EndDate = (DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                }
                else
                {
                    request.EndDate = request.EndDate + " 23:59:59";
                }
                if (request.PointId.AsInt(0) > 0)
                {
                    var model = service.View_MK_PointList.FirstOrDefault(u => u.ID == request.PointId);
                    if (model != null)
                    {
                        if (model.IsCommand == 1)
                        {
                            var list = from a in service.MK_DataValue
                                       join b in service.View_MK_Point
                                       on a.PointId equals b.ID
                                       where a.PointId==request.PointId && a.CreateDate>=request.BeginDate.AsDatetime(DateTime.Now) && a.CreateDate<=request.EndDate.AsDatetime(DateTime.Now)
                                       select new
                                       {
                                           PointId=b.ID,
                                           b.PointName,
                                           b.pName,
                                           b.MapDataName,
                                           b.ProjectName,
                                           b.DeviceName,
                                           b.TypeName,
                                           b.Unit,
                                           a.ScanValue,
                                           ScanValue2=a.ScanValue5,
                                           a.CreateDate,
                                           b.ScanTypeId
                                       };
                            if (request.PageSize > 0 && request.PageIndex > 0)
                            {
                                ret.Data = list.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
                            }
                            else
                            {
                                ret.Data = list.ToList();
                            }
                            ret.Success = true;
                            ret.TotalCount = list.Count();
                            ret.Message = "成功";
                        }
                        else
                        {
                            //string DBName = "FD_DynamicValue2";
                            int begin = request.BeginDate.AsDatetime(DateTime.Now).ToString("yyyyMM").AsInt();
                            int end = request.EndDate.AsDatetime(DateTime.Now).ToString("yyyyMM").AsInt();
                            List<FD_Dynamic> list = new List<FD_Dynamic>();
                            for(int i=begin;i<=end;i++)
                            {
                                if (i.ToString().Contains("13"))
                                {
                                    i = i + 100 - 12;
                                }
                                string DBName = "FD_DynamicValue2" + i;
                                var Exist = service.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                                if (Exist.ID == 0)
                                {
                                    continue;
                                }
                                string sql = string.Format("select a.ID,PointId,b.TypeName,a.CreateDate,MAXValue as ScanValue,YXValue as ScanValue2,b.ScanTypeId,b.PointName,b.ProjectName,b.pName,b.Unit,b.MapDataName,b.DeviceName from {0} a inner join View_MK_Point b on a.PointId=b.ID  where 1=1 and a.PointId={1} and a.CreateDate>='{2}' and a.CreateDate<='{3}'", DBName, request.PointId, request.BeginDate, request.EndDate);
                                List<FD_Dynamic> query = service.ExecuteQuery<FD_Dynamic>(sql).ToList();
                                query.ForEach(u=>
                                {
                                    FD_Dynamic se = new FD_Dynamic();
                                    se.CreateDate = u.CreateDate;
                                    se.DeviceName = u.DeviceName;
                                    se.ID = u.ID;
                                    se.MapDataName = u.MapDataName;
                                    se.pName = u.MapDataName;
                                    se.PointId = u.PointId;
                                    se.PointName = u.PointName;
                                    se.ProjectName = u.ProjectName;
                                    se.ScanTypeId = u.ScanTypeId;
                                    se.ScanValue = u.ScanValue;
                                    se.ScanValue2 = u.ScanValue2;
                                    se.Unit = u.Unit;
                                    list.Add(se);
                                });
                            }
                            if (request.PageSize > 0 && request.PageIndex > 0)
                            {
                                ret.Data = list.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
                            }
                            else
                            {
                                ret.Data = list.ToList();
                            }
                            ret.Success = true;
                            ret.TotalCount = list.Count();
                            ret.Message = "成功";
                        }
                        
                    }
                    else
                    {
                        ret.Message = "测点不存在";
                        ret.TotalCount = 0;
                        ret.Success = false;
                    }
                }
                else
                {
                    ret.Message = "请选择测点";
                    ret.TotalCount = 0;
                    ret.Success = false;
                }
                //if (request == null)
                //{
                //    request = new ConstructCfgPointRequest();
                //}
                //var list = service.View_MK_Point.AsQueryable();
                //if (request.ScanTypeID.AsInt(0) > 0)
                //{
                //    list = list.Where(u => u.ScanTypeId == request.ScanTypeID);
                //}
                //var se = list.Select(u => new
                //{
                //    u.ID,
                //    u.PointName,
                //    u.pName,
                //    u.MapDataName,
                //    u.ProjectName,
                //    u.DeviceName,
                //    u.TypeName,
                //    u.Unit,
                //    u.ScanTypeId
                //});
                //ret.Data = se.ToList();
                //ret.Message = "成功";
                //ret.Success = true;
            }
            catch (Exception ex)
            {
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
        public string PointName { set; get; }
        public string DeviceName { set; get; }
        public string Unit { set; get; }
        public string ProjectName { set; get; }
        public string pName { set; get; }
        public string MapDataName { set; get; }
        public double ScanValue { set; get; }
        public double ScanValue2 { set; get; }
        public DateTime CreateDate { set; get; }
        public int ScanTypeId { set; get; }

    }
}
