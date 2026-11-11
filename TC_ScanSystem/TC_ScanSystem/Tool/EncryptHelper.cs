using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public class EncryptHelper
    {
        public static string SHA256Encrypt(string str)
        {
            byte[] SHA256Data = System.Text.Encoding.UTF8.GetBytes(str);
            System.Security.Cryptography.SHA256 Sha256 = new System.Security.Cryptography.SHA256Managed();
            byte[] by = Sha256.ComputeHash(SHA256Data);
            return BitConverter.ToString(by).Replace("-", "").ToLower();
        }
    }
}
