using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tool
{
    public static class ConvertUtils
    {
        #region 将字符转换为Unicode
        /// <summary>
        /// 将汉字转换为Unicode
        /// </summary>
        /// <param name="str">要转码的字符</param>
        /// <returns>转码ToUnicode后的字符串</returns>
        public static string ToUnicode(this string str)
        {
            if (string.IsNullOrWhiteSpace(str) || !RegexUtils.IsChineseCharacter(str))
            {
                return str;
            }

            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                sb.AppendFormat(@"\u{0}", Convert.ToString(c, 16));
            }
            return sb.ToString();
        }
        #endregion

        #region 将Unicode转换字符
        /// <summary>
        /// 将Unicode编码转换汉字
        /// </summary>
        /// <param name="str">Unicode编码的汉字</param>
        /// <returns>转码后的GB2312字符串</returns>
        public static string FromUnicode(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            MatchCollection mc = Regex.Matches(str, @"\\u([\w]{2})([\w]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (mc.Count == 0)
            {
                return str;
            }

            StringBuilder sb = new StringBuilder();
            byte[] bts = new byte[2];
            foreach (Match m in mc)
            {
                bts[0] = (byte)int.Parse(m.Groups[2].Value, NumberStyles.HexNumber);
                bts[1] = (byte)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber);
                sb.Append(Encoding.Unicode.GetString(bts));
            }
            return sb.ToString();
        }
        #endregion

        #region 将字符转换为十六进制
        /// <summary>
        /// 转换为十六进制字符串
        /// </summary>
        /// <param name="bytes">要转换的字符</param>
        /// <returns>转换后的字符串</returns>
        public static string ToHex(this byte[] bytes)
        {
            return Regex.Replace(BitConverter.ToString(bytes), "-", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion

        #region 将十六进制转换byte数组
        /// <summary>
        /// 将十六进制转换byte数组
        /// </summary>
        /// <param name="hex">十六进制字符串</param>
        /// <returns>byte数组</returns>
        public static byte[] FromHex(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        #endregion

        #region 将字典数据转换成url链接
        /// <summary>
        /// 将字典数据转换成url链接
        /// </summary>
        /// <param name="iDict">集合类型的变量</param>
        /// <returns>返回生成的url链接</returns>
        public static string ToLink(this IEnumerable<KeyValuePair<string, string>> iDict)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> dict in iDict)
            {
                prestr.Append(string.Format("{0}={1}&", dict.Key, dict.Value));
            }
            //去掉最後一個&字符
            prestr.Remove(prestr.Length - 1, 1);
            return prestr.ToString();
        }
        #endregion

        #region 过滤字典中的数据
        /// <summary>
        /// 过滤字典中的数据
        /// </summary>
        /// <param name="dict">要过滤参数的字典</param>
        /// <param name="ignoreKeys">要忽略的键值</param>
        /// <returns>过滤后的字典</returns>
        public static IEnumerable<KeyValuePair<string, string>> ToFliter<T>(this T dict, IEnumerable<string> ignoreKeys) where T : IDictionary<string, string>, new()
        {
            var sArray = new T();
            IEnumerable<string> keys = dict.Keys.Where(key => !ignoreKeys.Contains(key) && !string.IsNullOrWhiteSpace(dict[key]));
            foreach (string key in keys)
            {
                sArray.Add(key, dict[key]);
            }
            return sArray;
        }
        #endregion

        #region 将字符中的部分用*号代替
        /// <summary>
        /// 将字符中的部分用*号代替(低于6位字符的不做替换)
        /// </summary>
        /// <param name="str">要混淆的原字符串</param>
        /// <returns>返回替换后的字符</returns>
        public static string ToHide(this string str)
        {
            //空字符直接返回
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;

            string suffix = string.Empty;
            if (RegexUtils.IsEmail(str))
            {
                // @符号所在的位置
                int atIdx = str.IndexOf("@", StringComparison.Ordinal);
                // 邮箱后缀
                suffix = str.Substring(atIdx);
                // Email地址
                str = str.Substring(0, atIdx);
            }
            int iLen = str.Length / 2;
            int starIndex = (int)Math.Ceiling((double)iLen / 2);
            string hideString = str.Substring(0, starIndex) + "".PadRight(iLen, '*') + str.Substring(iLen + starIndex);
            return hideString + suffix;
        }
        #endregion

        #region 将T转换为Dictionary
        /// <summary>
        /// 将T转换为Dictionary
        /// </summary>
        /// <typeparam name="T">T的类型</typeparam>
        /// <param name="obj">T数据</param>
        /// <returns>转换的字典集合</returns>
        public static Dictionary<string, string> ToDictionary<T>(this T obj) where T : class
        {
            var dict = new Dictionary<string, string>();
            PropertyInfo[] fields = obj.GetType().GetProperties();
            if (fields.Length <= 0) return dict;
            foreach (PropertyInfo field in fields)
            {
                object value = field.GetValue(obj, null);
                if (field.PropertyType.IsEnum)
                {
                    value = (int)value;
                }
                dict.Add(field.Name, value.ToString());
            }
            return dict;
        }
        #endregion
    }
}
