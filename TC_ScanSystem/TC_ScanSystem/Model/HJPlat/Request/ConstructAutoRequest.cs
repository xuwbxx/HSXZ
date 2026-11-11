using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class ConstructAutoRequest : BaseRequest
    {
        public string Begin { set; get; }

        public string End { set; get; }

        public string PointID { set; get; }

        public int PositionID { set; get; }

        public int DeviceID { set; get; }

        public int LayerID { set; get; }

        public int ScanTypeID { set; get; }

        public int? DeviceType { set; get; }
    }
}
