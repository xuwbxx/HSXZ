using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class TrainPersonRequest:BaseRequest
    {
        public int Idx { set; get; }
        public string PersonName { set; get; }
    }
}
