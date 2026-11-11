using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Monitor.Request
{
    public class MonitorDetailRequest : BaseRequest
    {
        public string AreaName { get; set; }
    }
}
