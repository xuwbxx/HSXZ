using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage.Response
{
    public class MenuTree
    {
        public MenuTree()
        {
            selected = false;
            partsel = false;
            children = new List<MenuTree>();
        }
        public string key { get; set; }
        public string title { get; set; }
        public bool selected { get; set; }
        public bool partsel { get; set; }
        public object data { get; set; }
        public int parentID { get; set; }

        public string Path { set; get; }

        public string FunctionCode { set; get; }
        public int Order { set; get; }

        public string Class { set; get; }
        public List<MenuTree> children { get; set; }
    }
}
