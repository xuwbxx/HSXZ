using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Monitor.Response
{
    /// <summary>
    /// 监测类型
    /// </summary>
    public class MonitorType
    {
        public int ID { set; get; }

        public string MonitorName { set; get; }

        /// <summary>
        /// 图标class
        /// </summary>
        public string IconClass { set; get; }
    }
}
