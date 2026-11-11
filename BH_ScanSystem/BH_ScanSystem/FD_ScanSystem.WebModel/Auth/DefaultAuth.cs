using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeShop.Model;

namespace FD_ScanSystem.WebModel.Auth
{
    public class DefaultAuth : IAuth
    {
        private AdminUserContext _user = null;
        public DefaultAuth(AdminUserContext userContext)
        {
            _user = userContext;
        }


        public bool Auth(string code)
        {
          // return true;
            if (_user == null) return false;
            if (_user.FunctionRights == null) return false;

            return _user.FunctionRights.FirstOrDefault(u => u.FunctionCode.Equals(code, StringComparison.CurrentCultureIgnoreCase)) != null;
        }
    }
}
