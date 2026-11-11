using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class EnvironmentResponse
    {
        public string PointName { set; get; }

        public string Temperature { set; get; }
        public string Humidity { set; get; }
        public string Noise { set; get; }
        public string PM25 { set; get; }
        public string PM10 { set; get; }
        public string WindSpeed { set; get; }
        public string WindDirectionText { set; get; }
        public string WindForce { set; get; }
        public string CreateTime { set; get; }
    }
}
