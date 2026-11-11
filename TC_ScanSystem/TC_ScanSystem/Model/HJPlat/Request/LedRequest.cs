using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class LedRequest : BaseRequest
    {
        public int ID { set; get; }
        public string CaveName { set; get; }

        public string StructorName { set; get; }
    }
}
