using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Monitor.Response
{
    public class MonitorDetailType
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public int? AreaID { get; set; }
        public string ImgName { get; set; }
    }
}
