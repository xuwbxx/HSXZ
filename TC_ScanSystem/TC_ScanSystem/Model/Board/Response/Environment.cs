using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Board.Response
{
    public class MonitorEnvirPointResponse : BaseReturn
    {
        //测点ID
        public int ID { get; set; }
        //测点名
        public string Name { get; set; }
        //测点参数
        public List<MonitorEnvirPointItem> Item { get; set; }
    }

    public class MonitorEnvirPointItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string ValueY { get; set; }
        public string ValueR { get; set; }
    }

    //public class EnvirRangeItem
    //{
    //    public string Name { get; set; }
    //    public string Unit { get; set; }
    //    public string Min { get; set; }
    //    public string Max { get; set; }
    //    public string ValueY { get; set; }
    //    public string ValueR { get; set; }
    //}
}
