using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage.Response
{
    public class ProjectResponse
    {
        public int ID { set; get; }

        public string ProjectName { set; get; }

        public string ShortName { set; get; }

        public string ProjectCode { set; get; }

        public List<MenuTree> trees { set; get; }
    }
}
