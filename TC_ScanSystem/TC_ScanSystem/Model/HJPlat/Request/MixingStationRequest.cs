using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Common;

namespace Model.HJPlat.Request
{
    public class MixingStationRequest:BaseRequest
    {
        public string MixMachineNo { set; get; }

        public string Begin { set; get; }

        public string End { set; get; }
    }
}
