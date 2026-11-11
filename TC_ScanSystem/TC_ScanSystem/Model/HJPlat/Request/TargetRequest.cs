using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Common;

namespace Model.HJPlat.Request
{
    public class TargetRequest:BaseRequest
    {
        public int ID { set; get; }

        public double? Security { set; get; }
        public double? Quality { set; get; }
        public double? Progress { set; get; }
        public double? Cost { set; get; }

        public DateTime? UpdateTime { set; get; }
    }
}
