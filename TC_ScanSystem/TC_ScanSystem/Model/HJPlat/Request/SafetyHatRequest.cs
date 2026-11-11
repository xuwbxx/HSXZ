using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class SafetyHatRequest : BaseRequest
    {
        public DateTime? Begin { set; get; }

        public DateTime? End { set; get; }
    }
}
