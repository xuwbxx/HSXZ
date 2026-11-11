using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OfficeShop.Helper {
    public sealed class Util {

        public static string MD5(string str) {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5");
        }

        public static string GetClientIP() { return "127.0.0.1"; }

        public static string FormatDateTime(DateTime? datetime) {
            if (datetime.HasValue) {
                return datetime.Value.ToString("yyyy-MM-dd hh:mm:ss");
            }
            else {
                return string.Empty;
            }
        }


        public static string CreateOrderNumber() {
            var epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return string.Format("{0:D10}{1:D3}", epoch, DateTime.Now.Millisecond);
        }


    }
}
