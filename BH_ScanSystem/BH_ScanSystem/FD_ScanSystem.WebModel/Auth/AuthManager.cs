using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeShop.Model;

namespace FD_ScanSystem.WebModel.Auth
{
    public class AuthManager
    {
        public static IAuth CreateAuth(AdminUserContext userContext)
        {
            return new DefaultAuth(userContext);
        }
    }
}
