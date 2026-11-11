using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Board.Response
{
    public class TargetResponse
    {
        public TargetResponse()
        {
            this.Item = new List<TargetItem>();
        }
        public List<TargetItem> Item { get; set; }
    }

    public class TargetItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
