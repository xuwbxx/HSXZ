using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Monitor.Request
{
    public class MonitorAreaRequest : BaseRequest
    {
        public int MonitorTypeID { set; get; }
    }
}
