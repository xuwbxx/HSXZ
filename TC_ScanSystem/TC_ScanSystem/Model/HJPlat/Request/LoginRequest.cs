using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class LoginRequest : BaseRequest
    {
        public string UserName { set; get; }

        public string Password { set; get; }
    }
}
