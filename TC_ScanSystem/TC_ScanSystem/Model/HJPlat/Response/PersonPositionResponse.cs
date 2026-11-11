using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class PersonPositionResponse
    {
        public PersonPositionResponse()
        {
            this.Caves = new List<CavePosition>();
        }

        public List<CavePosition> Caves { set; get; }
    }

    public class CavePosition
    {
        public CavePosition()
        {
            this.Persons = new List<PersonPosition>();
        }
        public int ID { set; get; }

        public string CaveName { set; get; }

        public decimal LonMin { set; get; }

        public decimal LonMax { set; get; }

        public decimal LatMin { set; get; }

        public decimal LatMax { set; get; }

        public decimal CenterLon { set; get; }

        public decimal CenterLat { set; get; }

        public decimal radius { set; get; }

        public List<PersonPosition> Persons { set; get; }
    }

    public class PersonPosition
    {
        public int ID { set; get; }

        public int UserID { set; get; }

        public string PersonName { set; get; }

        public string UnitName { set; get; }

        public decimal Lon { set; get; }

        public decimal Lat { set; get; }
    }
}
