using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage.Request
{
    public class ProjectRequest : BaseRequest
    {
        public int ID { set; get; }

        public string ProjectName { set; get; }

        public string ShortName { set; get; }

        public string ProjectCode { set; get; }

        public List<int> MenuIDs { set; get; }
    }
}
