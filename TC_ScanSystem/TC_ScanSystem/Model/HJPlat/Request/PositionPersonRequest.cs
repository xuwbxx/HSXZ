using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class PositionPersonRequest : BaseRequest
    {
        public string PersonName { set; get; }

        public int UserID { set; get; }
    }
}
