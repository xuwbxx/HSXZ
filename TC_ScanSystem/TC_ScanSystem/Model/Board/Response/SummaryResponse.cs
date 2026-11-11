using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Board.Response
{
    public class SummaryResponse
    {
        //public string Name { get; set; }
        //public string Value { get; set; }
        //public string Unit { get; set; }
        public List<SummaryItem> Item { get; set; }
    }

    public class SummaryItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
    }
}
