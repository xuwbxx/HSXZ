using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Monitor.Request
{
    public class MonitorPositionRequest : BaseRequest
    {
        public int MonitorAreaID { set; get; }
        public DateTime? BeginTime { set; get; }
        public DateTime? EndTime { set; get; }

        //public string AreaName { get; set; }
        //public int ScanTypeID { get; set; }
    }
}
