using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Model.Manage.Request
{
    public class RoleRequest : BaseRequest
    {
        public RoleRequest()
        {
            MenuIDs = new List<int>();
        }
        public int ID { set; get; }
        public string RoleName { set; get; }
        public string RoleCode { set; get; }
        public string Type { set; get; }

        public List<int> MenuIDs { set; get; }
    }
}
