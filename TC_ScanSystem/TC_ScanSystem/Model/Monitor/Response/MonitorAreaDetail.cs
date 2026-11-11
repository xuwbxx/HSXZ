using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Monitor.Response
{
    public class MonitorAreaDetail
    {
        public int ID { set; get; }

        public string MonitorAreaName { set; get; }

        /// <summary>
        /// 监测类型和名称
        /// </summary>
        public int MonitorTypeID { set; get; }
        public string MonitorTypeName { set; get; }

        public string BackGroundPic { set; get; }

        public string MonitorAreaPic { set; get; }
    }
}
