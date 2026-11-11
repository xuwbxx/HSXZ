using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage.Response
{
    public class AccountResponse
    {
        public int ID { set; get; }

        public string UserName { set; get; }

        public string Password { set; get; }

        public string Roles { set; get; }

        public int? DepartID { set; get; }

        public int? StructorID { set; get; }

        public int? PositionID { set; get; }

        public string RealName { set; get; }

        public string Mobile { set; get; }
    }
}
