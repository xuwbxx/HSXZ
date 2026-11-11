using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage.Response
{
    public class RoleResponse
    {
        public RoleResponse()
        {
            this.trees = new List<MenuTree>();
        }
        public int ID { set; get; }
        public string RoleName { set; get; }
        public string RoleCode { set; get; }

        public List<MenuTree> trees { set; get; }
    }

    public class RoleInfoResponse
    {
        public int ID { set; get; }
        public string RoleName { set; get; }
        public string RoleCode { set; get; }
        public DateTime? CreateTime { set; get; }
        public bool? IsDelete { set; get; }
        public int[] MenuIDs { get; set; }
    }
}
