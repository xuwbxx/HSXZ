using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Monitor.Request
{
    public class MonitorPointRequest : BaseRequest
    {
        public int MonitorPositionID { set; get; }

        public int MonitorPointID { set; get; }

        public DateTime? StartTime { set; get; }

        public DateTime? EndTime { set; get; }
    }
}
