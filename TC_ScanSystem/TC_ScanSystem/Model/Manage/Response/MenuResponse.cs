using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage.Response
{
    public class MenuResponse
    {
        public MenuResponse()
        {
            this.Child = new List<MenuResponse>();
        }
        public int ID { set; get; }

        public string MenuName { set; get; }

        public string Class { set; get; }

        public string Path { set; get; }

        public int? Order { set; get; }

        public List<MenuResponse> Child { set; get; }

        public int? ParentID { set; get; }
        public bool? IsDelete { set; get; }
        public int? Depth { set; get; }
        public string FunctionCode { set; get; }
        public DateTime? CreateTime { set; get; }

        public int? Type { set; get; }
    }
}
