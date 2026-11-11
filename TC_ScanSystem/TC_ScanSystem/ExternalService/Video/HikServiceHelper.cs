using ExternalService.HikVideoService;
using Model.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace ExternalService.Video
{
    public class HikServiceHelper
    {
        private VmsSdkWebServicePortTypeClient _ClientService { get; set; }
        private string _Token { get; set; }
        private string _HikIP { get; set; }
        private string _HikPort { get; set; }
        private string _HikUser { get; set; }
        private string _HikPwd { get; set; }

        public HikServiceHelper(string HikIP, string HikPort, string HikUser, string HikPwd)
        {
            _HikIP = HikIP;
            _HikPort = HikPort;
            _HikUser = HikUser;
            _HikPwd = HikPwd;
            _ClientService = GetService();
        }

        private VmsSdkWebServicePortTypeClient GetService()
        {
            VmsSdkWebServicePortTypeClient _clientService = null;
            try
            {
                BasicHttpBinding bhb = new BasicHttpBinding()
                {
                    MaxBufferSize = 2147483647,
                    MaxReceivedMessageSize = 2147483647,
                    ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas() { MaxStringContentLength = 2147483647 }
                };
                string url = string.Format("http://{0}:{1}/vms/services/VmsSdkWebService.VmsSdkWebServiceHttpSoap11Endpoint/", _HikIP, _HikPort);
                _clientService = new VmsSdkWebServicePortTypeClient(bhb, new EndpointAddress(url));
            }
            catch (Exception ex)
            {
                //Logger.Log(ex.ToString());
            }
            return _clientService;
        }

        public string GetToken()
        {
            string _token = string.Empty;
            try
            {
                SingletonLogin loginObj = SingletonLogin.Instance;
                //string tgtXML = string.Empty;
                if (string.IsNullOrEmpty(loginObj.LoginStr) || !loginObj.LoginStr.Contains("成功"))
                {
                    loginObj.LoginStr = _ClientService.sdkLogin(_HikUser, EncryptHelper.SHA256Encrypt(_HikPwd), _HikIP, "", "");
                }

                ServiceResult login = ServiceResult.Parse(loginObj.LoginStr);
                if (login.ResultCode != 0 || login.Rows[0]["tgt"].Length == 0)
                {
                    return _Token;
                }
                string tgt = _ClientService.applyToken(login.Rows[0]["tgt"]);
                ServiceResult sr = ServiceResult.Parse(tgt);
                _token = sr.Rows[0]["st"];
            }
            catch (Exception ex)
            {
                //Logger.Log(ex.ToString());
            }
            return _token;
        }

        public string GetResourceByOrgCode(string orgCode, HikResourceType type)
        {
            string resource = string.Empty;
            try
            {
                _Token = GetToken();
                resource = _ClientService.getResourceByOrgCode(_Token, (int)type, orgCode, "");
            }
            catch (Exception ex)
            {
                //Logger.Log(ex.ToString());
            }
            return resource;
        }

        public string GetResourceByPage(int pageIndex, int pageSize, int sort, HikResourceType type)
        {
            string resource = string.Empty;
            try
            {
                _Token = GetToken();
                resource = _ClientService.getResourceByPage(_Token, (int)type, pageIndex, pageSize, "", "", sort);
            }
            catch (Exception ex)
            {
                //Logger.Log(ex.ToString());
            }
            return resource;
        }

        public string GetPreviewOcxOptions(string IndexCode, string token, string WebIP)
        {
            string resource = string.Empty;
            try
            {
                resource = _ClientService.getPreviewOcxOptions(token, IndexCode, WebIP);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogText(ex.ToString());
            }
            return resource;
        }
    }

    public sealed class SingletonLogin
    {
        private SingletonLogin() { }
        /// <summary>
        /// 登录后，海康服务器返回的tgc字符串。
        /// </summary>
        public string LoginStr { get; set; }

        public static readonly SingletonLogin Instance = new SingletonLogin();
    }
}
