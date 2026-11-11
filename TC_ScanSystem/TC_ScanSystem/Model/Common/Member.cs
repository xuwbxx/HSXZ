using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class Member
    {
        public Member()
        {
            RoleIDs = new List<int>();
            Menus = new List<int>();
            MenuPaths = new List<string>();
        }
        public int ID { set; get; }
        public string UserName { set; get; }
        public int? DepartID { set; get; }
        public string DepartName { set; get; }
        public string RealName { set; get; }

        public int? PositionID { set; get; }

        public string PositionName { set; get; }

        public string Mobile { set; get; }
        
        public List<int> RoleIDs { set; get; }

        public List<int> Menus { set; get; }

        public List<string> MenuPaths { set; get; }

        public string ProjectCode { set; get; }

        public int ProjectID { set; get; }
    }
}
