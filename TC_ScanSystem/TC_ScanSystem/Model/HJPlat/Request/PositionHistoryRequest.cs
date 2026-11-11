using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class PositionHistoryRequest : BaseRequest
    {
        public string UserID { set; get; }

        public string Begin { set; get; }

        public string End { set; get; }
    }
}
