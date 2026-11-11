using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat
{
    public class VideoRequest : BaseRequest
    {
        public string ca { set; get; }

        public string code { set; get; }
    }
}
