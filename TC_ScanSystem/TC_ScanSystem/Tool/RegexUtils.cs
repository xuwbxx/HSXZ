using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tool
{
    public static class RegexUtils
    {
        #region 判断是否为数字
        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="verifyString">要验证的字符串</param>
        /// <returns>true|false</returns>
        public static bool IsDigital(string verifyString)
        {
            return Regex.IsMatch(verifyString, @"^\d+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion

        #region 判断输入的字符串只包含汉字
        /// <summary>
        /// 判断输入的字符串只包含汉字
        /// </summary>
        /// <param name="verifyString">要验证的字符串</param>
        /// <returns>true|false</returns>
        public static bool IsChineseCharacter(string verifyString)
        {
            return Regex.IsMatch(verifyString, "^[\u4e00-\u9fa5]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion

        #region 邮箱验证
        /// <summary>
        /// 邮箱验证
        /// </summary>
        /// <param name="verifyString">要验证的邮箱地址</param>
        /// <returns>true|false</returns>
        public static bool IsEmail(string verifyString)
        {
            return Regex.IsMatch(verifyString, @"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion

        #region 日期验证（年月日）
        /// <summary>
        /// 日期验证（年月日）
        /// </summary>
        /// <param name="verifyString">需要验证的日期</param>
        /// <returns>true|false</returns>
        public static bool IsDate(string verifyString)
        {
            DateTime dt;
            return DateTime.TryParse(verifyString, out dt);
        }
        #endregion

        #region 校验(国内)邮政编码
        /// <summary>
        /// 校验(国内)邮政编码
        /// </summary>
        /// <param name="verifyString">要验证的字符串</param>
        /// <returns>true|false</returns>
        public static bool IsPostCode(string verifyString)
        {
            return Regex.IsMatch(verifyString, @"^[0-9]{6}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion

        #region 身份证校验
        /// <summary>
        /// 身份证校验（只验证18位身份证）
        /// </summary>
        /// <param name="verifyString">身份证号码</param>
        /// <returns>true|false</returns>
        public static bool IsIDCard(string verifyString)
        {
            //18位身份证的基本验证，包含生日
            // 11:"北京",12:"天津",13:"河北",14:"山西",15:"内蒙古",21:"辽宁",22:"吉林",23:"黑龙江",31:"上海",32:"江苏",33:"浙江",34:"安徽",35:"福建",36:"江西",37:"山东",41:"河南",42:"湖北",43:"湖南",44:"广东",45:"广西",46:"海南",50:"重庆",51:"四川",52:"贵州",53:"云南",54:"西藏",61:"陕西",62:"甘肃",63:"青海",64:"宁夏",65:"新疆",71:"台湾",81:"香港",82:"澳门",91:"国外"} 
            const string regex = @"^(?:1[1-5]|2[1-3]|3[1-7]|4[1-6]|5[0-4]|6[1-5]|71|8[1-2]|91)[0-9]{4}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|([1-2][0-9]|1[0-9]|2[0-8])))[0-9]{3}[0-9Xx]$";
            //正则判断是否正确
            bool bIsTrue = Regex.IsMatch(verifyString, regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //计算身份证校验码，根据国家标准GB 11643-1999
            if (!bIsTrue) return false;

            //加权因子
            int[] factor = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //校验码对应值
            string[] verifyNumber = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            //将身份证打散为字符串数组
            string[] idcardArray = Array.ConvertAll(verifyString.ToCharArray(), s => s.ToString());

            int checksum = 0;
            for (int i = 0; i < 17; i++)
            {
                checksum += Convert.ToInt32(idcardArray[i]) * factor[i];
            }
            checksum = checksum % 11;

            return idcardArray[17].Equals(verifyNumber[checksum]);
        }
        #endregion

        #region 校验是否为移动号码
        /// <summary>
        /// 校验是否为移动号码（包含虚拟段170）
        /// </summary>
        /// <param name="verifyString">移动手机号</param>
        /// <returns>true|false</returns>
        public static bool IsChinaMobile(string verifyString)
        {
            const string pattern = @"^(?:13[4-9]|14[7]|15[012789]|17[8]|18[23478])\d{8}$";
            return Regex.IsMatch(verifyString, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        #endregion

        #region 校验是否为联通号码
        /// <summary>
        /// 校验是否为联通号码
        /// </summary>
        /// <param name="verifyString">联通手机号</param>
        /// <returns>true|false</returns>
        public static bool IsChinaUnicom(string verifyString)
        {
            const string pattern = @"^(?:13[0-2]|14[5]|15[56]|17[6]|18[56])\d{8}$";
            return Regex.IsMatch(verifyString, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        #endregion

        #region 校验是否为电信号码
        /// <summary>
        /// 校验是否为电信号码
        /// </summary>
        /// <param name="verifyString">电信手机号</param>
        /// <returns>true|false</returns>
        public static bool IsChinaTelecom(string verifyString)
        {
            const string pattern = @"^(?:13[3]|15[3]|17[7]|18[019])\d{8}$";
            return Regex.IsMatch(verifyString, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion

        #region 检查是否为虚拟手机运营商号段
        /// <summary>
        /// 检查是否为虚拟手机运营商号段
        /// </summary>
        /// <param name="verifyString"></param>
        /// <returns></returns>
        public static bool IsVirtualOperators(string verifyString)
        {
            const string pattern = @"^(?:17[0])\d{8}$";
            return Regex.IsMatch(verifyString, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion

        #region 替换字符
        /// <summary>
        /// 在指定的输入字符串内，使用指定的替换字符串替换与指定正则表达式匹配的所有字符串。
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="replacement">替换字符串</param>
        /// <returns>返回替换后的字符串</returns>
        public static string Replace(string input, string pattern, string replacement)
        {
            return Regex.Replace(input ?? string.Empty, pattern, replacement ?? string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion

        #region 查找指定字符串
        /// <summary>
        /// 查找指定字符串
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>返回匹配到的第一个匹配项</returns>
        public static Match Match(string input, string pattern)
        {
            return Regex.Match(input, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 查找指定字符串
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>返回匹配到的所有集合</returns>
        public static MatchCollection Matchs(string input, string pattern)
        {
            return Regex.Matches(input, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
