using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FD_ScanSystem.WebModel.Auth
{
    public interface IAuth
    {
        bool Auth(string code);
    }
}
