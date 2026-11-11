using Model.Other;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace ExternalService.IOT
{
    public class IntelligentVisionService
    {
        string IntelligentVisionUrl = ConfigurationManager.AppSettings["IntelligentVisionUrl"].ToString();
        string IntelligentVisionUserName = ConfigurationManager.AppSettings["IntelligentVisionUserName"].ToString();
        string IntelligentVisionPassword = ConfigurationManager.AppSettings["IntelligentVisionPassword"].ToString();
        public List<IntelligentVisionModel> GetVisionAlarmData(DateTime? start = null, DateTime? end = null)
        {
            List<IntelligentVisionModel> list = new List<IntelligentVisionModel>();

            try
            {
                string GetUrl = IntelligentVisionUrl;

                GetUrl = string.Format(GetUrl + "?username={0}&pass={1}", IntelligentVisionUserName, IntelligentVisionPassword);

                if (start != null && end != null)
                {
                    string StartTime = Convert.ToDateTime(start).ToString("yyyy-MM-dd HH:mm:ss");
                    string EndTime = Convert.ToDateTime(end).ToString("yyyy-MM-dd HH:mm:ss");
                    GetUrl = string.Format(GetUrl + "&start={0}&end={1}", StartTime, EndTime);
                }
                string JsonData = NetUtils.HttpGet(GetUrl);
                list = JsonData.Deserialize<List<IntelligentVisionModel>>();
            }
            catch (Exception ex)
            {
                list = new List<IntelligentVisionModel>();
            }

            return list;
        }

        /// <summary>
        /// 获取指定时间段报警信息
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int GetVisionAlarmCount(DateTime? start = null, DateTime? end = null)
        {
            try
            {
                string GetUrl = IntelligentVisionUrl;
                if (start != null && end != null)
                {
                    string StartTime = Convert.ToDateTime(start).ToString("yyyy-MM-dd HH:mm:ss");
                    string EndTime = Convert.ToDateTime(end).ToString("yyyy-MM-dd HH:mm:ss");
                    GetUrl = string.Format(GetUrl + "&startTime =={ 0}&endTime={1}", StartTime, EndTime);
                }
                string JsonData = NetUtils.HttpGet(GetUrl);

                return Convert.ToInt32(JsonData);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public IntelligentVisionAlarmCount GetVisionAlarmCountingInfo(DateTime? start = null, DateTime? end = null)
        {
            IntelligentVisionAlarmCount count = new IntelligentVisionAlarmCount();
            try
            {
                string GetUrl = IntelligentVisionUrl;
                if (start != null && end != null)
                {
                    string StartTime = Convert.ToDateTime(start).ToString("yyyy-MM-dd HH:mm:ss");
                    string EndTime = Convert.ToDateTime(end).ToString("yyyy-MM-dd HH:mm:ss");
                    GetUrl = string.Format(GetUrl + "&startCountingTime =={ 0}&endCountingTime={1}", StartTime, EndTime);
                }
                string JsonData = NetUtils.HttpGet(GetUrl);

                count = JsonData.Deserialize<IntelligentVisionAlarmCount>();
            }
            catch (Exception ex)
            {
                count = new IntelligentVisionAlarmCount();
            }
            return count;
        }
    }
}
