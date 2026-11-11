using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class ConstructCfgAcquisitionRequest : BaseRequest
    {
        public string BeginDate { set; get; }

        public int Interval { set; get; }
    }
}
