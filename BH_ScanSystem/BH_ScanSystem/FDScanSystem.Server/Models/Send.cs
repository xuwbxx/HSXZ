using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Net;
using System.Xml;

namespace FDScanSystem.Server.Models
{
    public class Send
    {
        public static bool SendSMS(string mobile, string msg)
        {
            string SMSPostUrl = ConfigurationManager.AppSettings["SMSPostUrl"];
            string SMSUserID = ConfigurationManager.AppSettings["SMSUserID"];
            string SMSAccount = ConfigurationManager.AppSettings["SMSAccount"];
            string SMSPassword = ConfigurationManager.AppSettings["SMSPassword"];
            string SMSContentCode = ConfigurationManager.AppSettings["SMSContentCode"];
            try
            {

                string content = System.Web.HttpUtility.UrlEncode(string.Format(SMSContentCode + "{0}", msg), System.Text.Encoding.GetEncoding("gb2312"));
                string SMSparameters = String.Format("UserID=" + SMSUserID + "&Account=" + SMSAccount + "&Password=" + SMSPassword + "&SMSType=1&Content={0}&Phones={1}&SendDate=&Sendtime=", content, mobile);

                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(SMSPostUrl);
                string res = HttpSMSPost(hr, SMSPostUrl, SMSparameters);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(res);
                string errorNum = xml.SelectSingleNode("/LANZ_ROOT/ErrorNum").InnerText;  //获取是否登陆成功

                if (errorNum == "0")
                {
                    return true;
                }
                else
                {
                   // LogUtils.Instance.LogText(errorNum);
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static string HttpSMSPost(HttpWebRequest hr, string url, string parameters)
        {
            string strRet = null;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(parameters);

            try
            {
                hr.KeepAlive = false;
                hr.Method = "POST";
                hr.ContentType = "application/x-www-form-urlencoded";
                hr.ProtocolVersion = HttpVersion.Version10;
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                hr.ContentLength = data.Length;
                Stream newStream = hr.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();

                HttpWebResponse myResponse = (HttpWebResponse)hr.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
                strRet = reader.ReadToEnd();
                reader.Close();
                myResponse.Close();

                return strRet;

            }
            catch (Exception ex)
            {
                strRet = "-200";
            }
            return strRet;
        }
    }
}
