using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Monitor.Response
{
    /// <summary>
    /// 监测层
    /// </summary>
    public class MonitorLayers
    {
        public int MonitorAreaID { set; get; }
        public string MonitorAreaName { set; get; }

        ///// <summary>
        ///// 监测类型和名称
        ///// </summary>
        //public int MonitorTypeID { set; get; }
        //public string MonitorTypeName { set; get; }

        public string X { set; get; }
        public int Level { set; get; }

        public string Y { set; get; }
        

        /// <summary>
        /// 图标Class
        /// </summary>
        public string IconClass { set; get; }
        public string NewMapX { set; get; }
        public string NewMapY { set; get; }
        public string NewMapZ { set; get; }
    }
    
    
}
