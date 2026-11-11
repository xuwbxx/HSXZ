using Model.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace ExternalService.IOT
{
    public class WeatherService
    {
        public WeatherResponse GetWeatherInfo(string CityName)
        {

            //WeatherWebService.WeatherWebServiceSoapClient client = new WeatherWebService.WeatherWebServiceSoapClient("WeatherWebServiceSoap12");
            //var data = client.getWeatherbyCityName("上海");

            WeatherResponse response = new WeatherResponse();

            try
            {
                //获取天气预报API
                string WeatherUrl = @"https://free-api.heweather.com/s6/weather/forecast?location=" + CityName + "&key=76f7879bc5dc4f32be8bcb32ce2cdf18";

                var list = NetUtils.HttpPost(WeatherUrl, "").Deserialize<Weaher>();

                var se = list.HeWeather6.FirstOrDefault();

                if (se != null)
                {
                    response.Point = se.basic.location;
                    //response.Weather
                    foreach (var item in se.daily_forecast)
                    {
                        //找出今天
                        DateTime dt = Convert.ToDateTime(item.date);
                        if (dt.Year == DateTime.Today.Year && dt.Month == DateTime.Today.Month && dt.Day == DateTime.Today.Day)
                        {
                            response.Weather = item.cond_txt_d;
                            response.MaxTemp = item.tmp_max;
                            response.MinTemp = item.tmp_min;
                            response.Success = true;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                response = new WeatherResponse();
            }

            return response;

        }


    }
}
