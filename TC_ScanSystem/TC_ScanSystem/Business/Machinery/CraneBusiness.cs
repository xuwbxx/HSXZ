using DataEntity.Services;
using Model.HJPlat.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Machinery
{
    public class CraneBusiness
    {
        //WorkService service = new WorkService();

        ///// <summary>
        ///// 设备详细运行信息
        ///// </summary>
        ///// <param name="idStr">设备ID</param>
        ///// <returns>设备详细运行信息</returns>
        //public GantryCraneItemInfoResponse GetGantryCraneItemInfo(string idStr)
        //{
        //    GantryCraneItemInfoResponse response = new GantryCraneItemInfoResponse();
        //    int id = 0;
        //    if (string.IsNullOrWhiteSpace(idStr) || !int.TryParse(idStr, out id) || id == 0)
        //    {
        //        return response;
        //    }

        //    var item = service.Repository_Base_Equipment_Special.Query(a => a.Idx == id).ToList();

        //    if (item == null || item.Count == 0)
        //    {
        //        return response;
        //    }

        //    response.Idx = id;
        //    response.Name = item.First().EquipName;

        //    CraneBusiness craBiz = new CraneBusiness();

        //    response.StatusInfo = craBiz.GetLastestCraneData(id);

        //    return response;
        //}
        //public GantryCraneStatusResponse GetLastestCraneData(int EquipmentID)
        //{
        //    GantryCraneStatusResponse data = new GantryCraneStatusResponse();
        //    data.success = false;

        //    if (EquipmentID == 0)
        //    {
        //        return data;
        //    }

        //    var lastData = service.Repository_Scan_Special_Monitor.Query(a => a.EquipmentID == EquipmentID).OrderByDescending(a => a.CreateTime).FirstOrDefault();
        //    if (lastData == null)
        //    {
        //        return data;
        //    }

        //    string Code = lastData.MonitorMessage;
        //    if (string.IsNullOrEmpty(Code))
        //    {
        //        return data;
        //    }

        //    //GWYAS002183BS0-1984CS000000DS000000EL000018FS000000GS000000HS000000IS000000J1P
        //    //GWYAS001267BS000920CS000000DS000000ER000933FS000000GS000000HS000000IS000000J0K0L0M0O0Q0R0P
        //    if (!Code.StartsWith("GWY") || !Code.EndsWith("P"))
        //    {
        //        return data;
        //    }

        //    //设备MAC
        //    string MacStr = Code.Substring(89, 17);
        //    data.Mac = MacStr;

        //    //大车
        //    string CartStr = Code.Substring(3, 8);
        //    //data.Cart.action = CartStr.Substring(1, 1);
        //    data.Cart.action = ConvertStatusDirection(CartStr.Substring(1, 1));
        //    data.Cart.position = CraneCodeConvert(CartStr.Substring(2));
        //    data.Cart.unit = "米";
        //    //大车限位状态
        //    string CartLeftLimitStatusStr = Code.Substring(77, 2);
        //    data.Cart.LeftLimitStatus = ConvertStatusSwitch(CartLeftLimitStatusStr.Substring(1, 1));
        //    string CartRightLimitStatusStr = Code.Substring(79, 2);
        //    data.Cart.RightLimitStatus = ConvertStatusSwitch(CartRightLimitStatusStr.Substring(1, 1));

        //    //小车
        //    string DollyFrontLimitStatusStr = Code.Substring(81, 2);
        //    data.Dolly.FrontLimitStatus = ConvertStatusSwitch(DollyFrontLimitStatusStr.Substring(1, 1));
        //    string DollyBackLimitStatusStr = Code.Substring(83, 2);
        //    data.Dolly.BackLimitStatus = ConvertStatusSwitch(DollyBackLimitStatusStr.Substring(1, 1));


        //    //吊钩
        //    string HookActionStr = Code.Substring(11, 8);
        //    string HookSpeedStr = Code.Substring(51, 8);
        //    data.Hook.action = ConvertStatusDirection(HookActionStr.Substring(1, 1));
        //    data.Hook.position = CraneCodeConvert(HookActionStr.Substring(2));
        //    data.Hook.unit = "米";
        //    data.Hook.speed = CraneCodeConvert(HookSpeedStr.Substring(2));
        //    data.Hook.speedunit = "米/秒";

        //    //载重
        //    string WeightStr = Code.Substring(27, 8);
        //    data.Weight.unit = "吨";
        //    data.Weight.weight = CraneCodeConvert(WeightStr.Substring(2));

        //    //倾斜
        //    string TiltStr = Code.Substring(35, 8);
        //    data.Tilt.direction = ConvertStatusAngle(TiltStr.Substring(1, 1));
        //    data.Tilt.unit = "°";
        //    data.Tilt.Tilt = CraneCodeConvert(TiltStr.Substring(2));

        //    //风速
        //    string WindStr = Code.Substring(43, 8);
        //    data.Wind.unit = "米/秒";
        //    data.Wind.speed = CraneCodeConvert(WindStr.Substring(2));

        //    //累计工作时间
        //    string WorkDuringStr = Code.Substring(59, 8);
        //    data.Duration.unit = "分钟";
        //    data.Duration.time = CraneCodeConvert(WorkDuringStr.Substring(2));

        //    //夹轨器状态
        //    string ClampStr = Code.Substring(75, 2);
        //    data.Clamp.action = ConvertStatusSwitch(ClampStr.Substring(1, 1));

        //    //门限位状态
        //    string DoorStr = Code.Substring(85, 2);
        //    data.Door.action = ConvertStatusSwitch(ClampStr.Substring(1, 1));

        //    //支腿限位状态
        //    string LegStr = Code.Substring(87, 2);
        //    data.Leg.action = ConvertStatusSwitch(ClampStr.Substring(1, 1));



        //    data.CreateTime = lastData.CreateTime.HasValue ? lastData.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";

        //    data.success = true;
        //    return data;
        //}

        //private decimal CraneCodeConvert(string code)
        //{
        //    if (code.Contains("-"))
        //    {
        //        int poIndex = code.IndexOf('-');
        //        if (poIndex >= 4)
        //        {
        //            return 0;
        //        }
        //        string ValueStr = code.Substring(poIndex + 1);
        //        if (ValueStr.Length < 2)
        //        {
        //            return 0;
        //        }
        //        return decimal.Parse(ValueStr.Substring(0, ValueStr.Length - 2) + "." + ValueStr.Substring(ValueStr.Length - 2, 2));
        //    }
        //    else
        //    {
        //        return decimal.Parse(code.Substring(0, code.Length - 2) + "." + code.Substring(code.Length - 2, 2));
        //    }

        //}


        ///// <summary>
        ///// S静止, L向左, R向右，U向上, D向下
        ///// </summary>
        //private string ConvertStatusDirection(string codeStr)
        //{
        //    string ret = string.Empty;
        //    if (codeStr.Equals("S",StringComparison.OrdinalIgnoreCase))
        //    {
        //        ret = "静止";
        //    }
        //    else if (codeStr.Equals("L", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ret = "向左";
        //    }
        //    else if (codeStr.Equals("R", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ret = "向右";
        //    }
        //    else if (codeStr.Equals("U", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ret = "向上";
        //    }
        //    else if (codeStr.Equals("D", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ret = "向下";
        //    }
        //    return ret;
        //}


        ///// <summary>
        ///// 限位状态 0:锁定 1:解除
        ///// </summary>
        //private string ConvertStatusSwitch(string codeStr)
        //{
        //    string ret = string.Empty;
        //    if (codeStr.Equals("0", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ret = "锁定";
        //    }
        //    else if (codeStr.Equals("1", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ret = "解除";
        //    }
        //    return ret;
        //}

        ///// <summary>
        ///// 角度(S是0度,L度数负,R度数正)
        ///// </summary>
        //private string ConvertStatusAngle(string codeStr)
        //{
        //    string ret = string.Empty;
        //    if (codeStr.Equals("S", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ret = "";
        //    }
        //    else if (codeStr.Equals("L", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ret = "-";
        //    }
        //    else if (codeStr.Equals("R", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ret = "+";
        //    }
        //    return ret;
        //}
    }
}
