using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Other
{
    public class Weaher
    {
        public List<WeatherInfo> HeWeather6 { get; set; }

        public Weaher()
        {
            HeWeather6 = new List<WeatherInfo>();
        }
    }

    public class WeatherInfo
    {
        /// <summary>
        /// 基础信息
        /// </summary>
        public BasicInfo basic { get; set; }
        /// <summary>
        /// 接口更新时间
        /// </summary>
        public UpdateInfo update { get; set; }
        /// <summary>
        /// 接口状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 天气预报
        /// </summary>
        public List<DailyForecast> daily_forecast { get; set; }
    }

    public class BasicInfo
    {
        /// <summary>
        /// 地区／城市ID
        /// </summary>
        public string cid { get; set; }
        /// <summary>
        /// 地区／城市名称
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 该地区／城市的上级城市
        /// </summary>
        public string parent_city { get; set; }
        /// <summary>
        /// 该地区／城市所属行政区域
        /// </summary>
        public string admin_area { get; set; }
        /// <summary>
        /// 该地区／城市所属国家名称
        /// </summary>
        public string cnty { get; set; }
        /// <summary>
        /// 地区／城市经度
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// 地区／城市纬度
        /// </summary>
        public string lon { get; set; }
        /// <summary>
        /// 	该地区／城市所在时区
        /// </summary>
        public string tz { get; set; }
    }

    public class DailyForecast
    {
        /// <summary>
        /// 白天天气状况代码
        /// </summary>
        public string cond_code_d { get; set; }
        /// <summary>
        /// 晚间天气状况代码
        /// </summary>
        public string cond_code_n { get; set; }
        /// <summary>
        /// 白天天气状况描述
        /// </summary>
        public string cond_txt_d { get; set; }
        /// <summary>
        /// 晚间天气状况描述
        /// </summary>
        public string cond_txt_n { get; set; }
        /// <summary>
        /// 预报日期
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 相对湿度
        /// </summary>
        public string hum { get; set; }
        /// <summary>
        /// 月升时间
        /// </summary>
        public string mr { get; set; }
        /// <summary>
        /// 月落时间
        /// </summary>
        public string ms { get; set; }
        /// <summary>
        /// 降水量
        /// </summary>
        public string pcpn { get; set; }
        /// <summary>
        /// 降水概率
        /// </summary>
        public string pop { get; set; }
        /// <summary>
        /// 大气压强
        /// </summary>
        public string pres { get; set; }
        /// <summary>
        /// 日出时间
        /// </summary>
        public string sr { get; set; }
        /// <summary>
        /// 日落时间
        /// </summary>
        public string ss { get; set; }
        /// <summary>
        /// 最高温度
        /// </summary>
        public string tmp_max { get; set; }
        /// <summary>
        /// 最低温度
        /// </summary>
        public string tmp_min { get; set; }
        /// <summary>
        /// 紫外线强度指数
        /// </summary>
        public string uv_index { get; set; }
        /// <summary>
        /// 能见度，单位：公里
        /// </summary>
        public string vis { get; set; }
        /// <summary>
        /// 风向360角度
        /// </summary>
        public string wind_deg { get; set; }
        /// <summary>
        /// 风向
        /// </summary>
        public string wind_dir { get; set; }
        /// <summary>
        /// 风力
        /// </summary>
        public string wind_sc { get; set; }
        /// <summary>
        /// 风速，公里/小时
        /// </summary>
        public string wind_spd { get; set; }
    }

    public class UpdateInfo
    {
        /// <summary>
        /// 当地时间
        /// </summary>
        public string loc { get; set; }
        /// <summary>
        /// UTC时间
        /// </summary>
        public string utc { get; set; }
    }

    public enum HeWeather6_Status
    {
        /// <summary>
        /// 数据正常
        /// </summary>
        ok,
        /// <summary>
        /// 错误的key
        /// </summary>
        invalidkey,
        /// <summary>
        /// 未知或错误城市/地区
        /// </summary>
        unknownlocation,
        /// <summary>
        /// 	该城市/地区没有你所请求的数据
        /// </summary>
        nodataforthislocation,
        /// <summary>
        /// 超过访问次数
        /// </summary>
        nomorerequests,
        /// <summary>
        /// 参数错误
        /// </summary>
        invalidparam,
        /// <summary>
        /// 超过限定的QPM
        /// </summary>
        toofast,
        /// <summary>
        /// 无响应或超时
        /// </summary>
        dead,
        /// <summary>
        /// 无访问权限
        /// </summary>
        permissiondenied
    }



    public class WeatherResponse
    {
        public WeatherResponse()
        {
            Success = false;
        }
        public bool Success { set; get; }
        public string Point { set; get; }

        public string Weather { set; get; }

        public string MinTemp { set; get; }

        public string MaxTemp { set; get; }
    }
}
