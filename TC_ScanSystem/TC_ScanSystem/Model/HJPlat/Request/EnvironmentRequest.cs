using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class EnvironmentRequest : BaseRequest
    {
        public int PointID { set; get; }

        public string Begin { set; get; }

        public string End { set; get; }
    }
}
