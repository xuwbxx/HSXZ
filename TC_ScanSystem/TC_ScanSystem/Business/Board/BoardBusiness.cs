using DataEntity.DataContext;
using DataEntity.Services;
using Model.Board.Response;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Board
{
    public class BoardBusiness
    {
        //WorkService service = new WorkService();

        //#region 环境监测
        //public List<MonitorEnvirPointResponse> GetEnvironmentParam()
        //{
        //    List<MonitorEnvirPointResponse> ret = new List<MonitorEnvirPointResponse>();
        //    MonitorEnvirPointResponse tempPoint = new MonitorEnvirPointResponse();
        //    MonitorEnvirPointItem tempPointItem = new MonitorEnvirPointItem();

        //    tempPoint.Item = new List<MonitorEnvirPointItem>();

        //    var latestData = service.Repository_View_EnvironmentData.Query(a => a.ID > 0).OrderByDescending(a => a.CreateTime).FirstOrDefault();
        //    var rangeData = service.Repository_Scan_Monitor_EnvItem.Query(a => a.Id > 0).ToList();

        //    if (latestData != null)
        //    {
        //        tempPoint.Name = latestData.PointName;
        //        tempPoint.ID = latestData.ID;

        //        foreach (var item in rangeData)
        //        {
        //            var retItem = SetEnvirData(item.TypeName, latestData, rangeData);
        //            if (retItem != null)
        //            {
        //                tempPoint.Item.Add(retItem);
        //            }
        //        }
        //        ret.Add(tempPoint);
        //    }

        //    return ret;
        //}

        //private MonitorEnvirPointItem SetEnvirData(string itemStr, View_EnvironmentData valObj, List<Scan_Monitor_EnvItem> range)
        //{
        //    MonitorEnvirPointItem itemTemp = new MonitorEnvirPointItem();

        //    itemTemp.Name = itemStr;

        //    switch (itemStr)
        //    {
        //        case "温度":
        //            itemTemp.Value = valObj.Temperature.HasValue? valObj.Temperature.Value.ToString() : "";
        //            break;
        //        case "湿度":
        //            itemTemp.Value = valObj.Humidity.HasValue ? valObj.Humidity.Value.ToString() : "";
        //            break;
        //        case "PM2.5":
        //            itemTemp.Value = valObj.PM25.HasValue ? valObj.PM25.Value.ToString() : "";
        //            break;
        //        case "PM10":
        //            itemTemp.Value = valObj.PM10.HasValue ? valObj.PM10.Value.ToString() : "";
        //            break;
        //        case "噪音":
        //            itemTemp.Value = valObj.Noise.HasValue ? valObj.Noise.Value.ToString() : "";
        //            break;
        //        case "风速":
        //            itemTemp.Value = valObj.WindSpeed.HasValue ? valObj.WindSpeed.Value.ToString() : "";
        //            break;
        //        case "风力":
        //            itemTemp.Value = valObj.WindForce.HasValue ? valObj.WindForce.Value.ToString() : "";
        //            break;
        //        default:
        //            itemTemp = null;
        //            break;
        //    }

        //    if (itemTemp != null)
        //    {
        //        var item = range.Where(a => a.TypeName.Equals(itemStr, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        //        if (item != null)
        //        {
        //            itemTemp.Max = item.MaxValue.HasValue ? item.MaxValue.Value.ToString() : "";
        //            itemTemp.Min = item.MinValue.HasValue ? item.MinValue.Value.ToString() : "";
        //            itemTemp.ValueR = item.RValue.HasValue ? item.RValue.Value.ToString() : "";
        //            itemTemp.ValueY = item.YValue.HasValue ? item.YValue.Value.ToString() : "";
        //            itemTemp.Unit = item.Unit;
        //        }
        //    }

        //    return itemTemp;
        //}

        //#endregion

        //#region 工程进度
        //public ProgressResponse GetProgressData()
        //{
        //    ProgressResponse ret = new ProgressResponse();

        //    ret.Item = new List<ProgressItem>();

        //    //ret.Name = "工程进度";
        //    //ret.Unit = "月";

        //    var data = service.Repository_Board_Progress.Query(a => a.ID > 0).OrderByDescending(a => a.FillinMonth).Take(6).ToList();

        //    foreach (var item in data)
        //    {
        //        var temp = new ProgressItem();

        //        //var date = item.FillinMonth.HasValue ? item.FillinMonth.Value.ToString("yy.MM") : "";
        //        var date = item.FillinMonth.HasValue ? item.FillinMonth.Value.ToString(): "";

        //        temp.Name = date;
        //        temp.ValueExpect = item.PlanValue.HasValue? item.PlanValue.Value.ToString():"";
        //        temp.ValueActual= item.ActualValue.HasValue? item.ActualValue.Value.ToString():"";

        //        ret.Item.Add(temp);
        //    }

        //    return ret;
        //}
        //#endregion

        //#region 概况统计
        //public SummaryResponse GetSummaryData()
        //{
        //    SummaryResponse ret = new SummaryResponse();
        //    ret.Item = new List<SummaryItem>();

        //    var cost = new SummaryItem();
        //    var people = new SummaryItem();
        //    var machine = new SummaryItem();

        //    cost.Name = "累计产值";
        //    var valCost = service.Repository_Board_Progress.Query(a => a.ID > 0).OrderByDescending(a => a.FillinMonth).Sum(a => a.ActualValue.Value);
        //    cost.Value = valCost.ToString();
        //    cost.Unit = "万元";

        //    people.Name = "进场人员";
        //    //var valuePeo = service.Repository_View_Scan_Loc_UserData.Query(a => !string.IsNullOrEmpty(a.UserId)).Count();
        //    var valuePeo = service.Repository_Scan_Position_Current.Query().Count();
        //    people.Value = valuePeo.ToString();
        //    people.Unit = "人";

        //    machine.Name = "进场机械";
        //    var valueMachine = service.Repository_View_Base_Equipment.Query(a => a.Idx > 0).Count();
        //    machine.Value = valueMachine.ToString();
        //    machine.Unit = "台";

        //    ret.Item.Add(cost);
        //    ret.Item.Add(people);
        //    ret.Item.Add(machine);

        //    return ret;
        //}
        //#endregion

        //#region 指标完成
        //public TargetResponse GetTargetData()
        //{
        //    var ret = new TargetResponse();

        //    var temp = service.Repository_Board_Target.Query(a => a.ID > 0).FirstOrDefault();
        //    if (temp != null)
        //    {
        //        var security = new TargetItem();
        //        var quality = new TargetItem();
        //        var progress = new TargetItem();
        //        var cost = new TargetItem();

        //        security.Name = "安全检查";
        //        security.Value = temp.Security.HasValue ? temp.Security.Value.ToString():"";

        //        quality.Name = "质量检查";
        //        quality.Value = temp.Quality.HasValue ? temp.Quality.Value.ToString():"";

        //        progress.Name = "进度计划";
        //        progress.Value = temp.Progress.HasValue ? temp.Progress.Value.ToString(): "";

        //        cost.Name = "成本计划";
        //        cost.Value = temp.Cost.HasValue ? temp.Cost.Value.ToString(): "";

        //        ret.Item.Add(security);
        //        ret.Item.Add(quality);
        //        ret.Item.Add(progress);
        //        ret.Item.Add(cost);
        //    }

        //    return ret;
        //}
        //#endregion
    }
}
