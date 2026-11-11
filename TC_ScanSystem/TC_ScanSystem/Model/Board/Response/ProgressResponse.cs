using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Board.Response
{
    public class ProgressResponse
    {
        ////public int ID { get; set; }
        //public string Name { get; set; }
        ///// <summary>
        ///// 年，月，季度
        ///// </summary>
        //public string Unit { get; set; }
        public List<ProgressItem> Item { get; set; }
    }

    public class ProgressItem
    {
        public string Name{ get; set; }
        public string ValueExpect { get; set; }
        public string ValueActual{ get; set; }
    }
}
