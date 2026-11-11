using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class PositionHistoryResponse
    {
        public int UserID { set; get; }

        public string PersonName { set; get; }

        public string UnitName { set; get; }

        public string TelPhone { set; get; }

        public string IdentifyID { set; get; }

        public string Station { set; get; }

        public string PositionTime { set; get; }

        public decimal? Lon { set; get; }

        public decimal? Lat { set; get; }
    }
}
