using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage.Request
{
    public class MenuRequest
    {
        public int ID { get; set; }
        public string MenuName { get; set; }
        public int Depth { get; set; }
        public string Path { get; set; }
        public int Order { get; set; }
        public int ParentID { get; set; }
        public string Class { get; set; }

        public int Type { set; get; }
        public string FunctionCode { set; get; }
        public bool? IsDelete { set; get; }
    }
}
