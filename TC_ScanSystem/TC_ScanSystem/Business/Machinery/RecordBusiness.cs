using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tool;

namespace Business.Machinery
{
    public class RecordBusiness
    {
        #region 机械设备业务逻辑

        public Dictionary<string, string> GetQRCodeMachine(string idxs, string machineCodes)
        {
            Dictionary<string, string> codesDic = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(machineCodes) || string.IsNullOrEmpty(idxs))
            {
                return codesDic;
            }

            string[] codesArr = machineCodes.Split(',');
            string[] idArr = idxs.Split(',');
            int codeCount = codesArr.Length;
            //int idCount = idArr.Length;
            //int minCount = 0;
            //minCount = idCount > codeCount ? codeCount : idCount;

            if (codeCount > 0)
            {
                int i = 0;
                foreach (var itemCode in codesArr)
                {
                    string trimedStr = itemCode.Trim();
                    if (!string.IsNullOrEmpty(trimedStr))
                    {
                        string codeUrl = GenerateQRCode(trimedStr);
                        if (!string.IsNullOrEmpty(codeUrl))
                        {
                            codesDic.Add(idArr[i], codeUrl);
                        }
                        else
                        {
                            codesDic.Add(idArr[i], "");
                        }
                    }
                    i++;
                }
            }

            return codesDic;
        }


        private string GenerateQRCode(string machineCode)
        {
            string qrUrl = string.Empty;
            string qrCodeUrl = GetQRCodeUrl(machineCode);
            string qrFilePath = GetQRImgFile(machineCode);
            string qrCdContent = GetQRCodeContentUrl(machineCode);
            if (File.Exists(qrFilePath) || GenerateQRCodeImg(machineCode, qrFilePath))
            {
                qrUrl = qrCodeUrl;
            }
            return qrUrl;
        }

        private string GetQRCodeUrl(string machineCode)
        {
            string qrCodeUrl = string.Format("/File/QRImage/Machine/{0}.jpg", machineCode);
            return qrCodeUrl;
        }

        private string GetQRCodeContentUrl(string machineCode)
        {
            string rootUrlStr = ConfigurationManager.AppSettings["WebRoot"];
            string qrContent = string.Format("{0}/Machinery/Record/MachineInfo?Code={1}", rootUrlStr, machineCode);
            return qrContent;
        }

        private string GetQRImgDir()
        {
            //return ConfigurationManager.AppSettings["QRImgMachine"];
            return string.Format("{0}/File/QRImage/Machine/", AppDomain.CurrentDomain.BaseDirectory);
        }

        private string GetQRImgFile(string machineCode)
        {
            //return ConfigurationManager.AppSettings["QRImgMachine"];
            return string.Format("{0}/File/QRImage/Machine/{1}.jpg", AppDomain.CurrentDomain.BaseDirectory, machineCode);
        }

        private bool GenerateQRCodeImg(string machineCode, string machineQRDir)
        {
            string qrCodeContent = GetQRCodeContentUrl(machineCode);
            bool ret = true;
            try
            {
                Bitmap qrCodeImg = QRCodeUtils.CreateQRCode(qrCodeContent);
                Bitmap centerlogo = new Bitmap(Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"\File\CommonImage\login_logo.jpg"), 40, 40);
                Graphics g = Graphics.FromImage(qrCodeImg);
                g.DrawImage(centerlogo, new Point(qrCodeImg.Width / 2 - 20, qrCodeImg.Height / 2 - 20));

                string qrImgDir = GetQRImgDir();
                string qrImgFile = GetQRImgFile(machineCode);

                if (!Directory.Exists(GetQRImgDir()))
                {
                    Directory.CreateDirectory(GetQRImgDir());
                }

                if (!File.Exists(qrImgFile))
                {
                    qrCodeImg.Save(qrImgFile);
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }
        #endregion
    }
}
