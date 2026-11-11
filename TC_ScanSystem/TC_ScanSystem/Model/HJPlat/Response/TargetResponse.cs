using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class TargetResponse
    {
        public int ID { set; get; }

        public double? Security { set; get; }
        public double? Quality { set; get; }
        public double? Progress { set; get; }
        public double? Cost { set; get; }

        public DateTime? UpdateTime { set; get; }
    }
}
