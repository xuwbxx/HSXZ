using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class CommnicationRequest : BaseRequest
    {
        public int ProjectId { set; get; }
        public int Protocol { set; get; }

        public int ID { set; get; }
    }
}
