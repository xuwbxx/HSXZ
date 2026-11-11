using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public class KeyValueHelper
    {
        static Dictionary<string, string> conents = new Dictionary<string, string>();

        private static string _matchKey = "=";
        public static string matchKey { get { return _matchKey; } set { _matchKey = value; } }

        private static string _matchValue = ";";
        public static string matchValue { get { return _matchValue; } set { _matchValue = value; } }

        /// <summary>
        /// 解析输入Bytes中的键值对
        /// </summary>
        /// <param name="data">输入字节数组</param>
        /// <returns>解析后的键值对字典</returns>
        public static Dictionary<string, string> GetConentByBytes(byte[] data)
        {
            conents.Clear();
            conents = GetConentByString(Encoding.Default.GetString(data));
            return conents;
        }

        /// <summary>
        /// 解析输入字符串中的键值对
        /// </summary>
        /// <param name="data">输入字符串</param>
        /// <returns>解析后的键值对字典</returns>
        public static Dictionary<string, string> GetConentByString(string data)
        {
            conents.Clear();

            if (data.Substring(data.Length - 1) != _matchValue)
            {
                data = data + _matchValue;
            }

            try
            {
                int pos = 0;
                int startIndex = 0;
                while (true)
                {
                    //Get Key
                    pos = data.IndexOf(_matchKey, startIndex);
                    string key = data.Substring(startIndex, pos - startIndex);
                    startIndex = pos + 1;
                    //Get Value
                    pos = data.IndexOf(_matchValue, startIndex);
                    string value = data.Substring(startIndex, pos - startIndex);
                    startIndex = pos + 1;
                    conents.Add(key, value);

                    if (startIndex >= data.Length)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Info: " + ex.ToString());
            }

            return conents;
        }
    }
}
