using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class Base_Cave_CoordinateResponse
    {
        public Base_Cave_CoordinateResponse()
        {
            this.Coordinates = new List<Cave_Coordinate>();
        }
        public int ID { set; get; }
        
        public string CaveName { set; get; }

        public decimal? CenterLon { set; get; }

        public decimal? CenterLat { set; get; }
        
        public List<Cave_Coordinate> Coordinates { set; get; }
    }

    public class Cave_Coordinate
    {
        public int ID { set; get; }
        public decimal? Lon { set; get; }

        public decimal? Lat { set; get; }

        public string PointName { set; get; }

        public int? Order { set; get; }
    }
}
