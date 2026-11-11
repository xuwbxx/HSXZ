using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class TrainPersonResponse
    {
        public int Idx { set; get; }
        public string PersonName { set; get; }
        public int PersonId { set; get; }
        public string Sex { set; get; }
        public string IdentifyID { set; get; }
        public string Station { set; get; }
        public string UnitName { set; get; }
        public string TelPhone { set; get; }
    }
}
