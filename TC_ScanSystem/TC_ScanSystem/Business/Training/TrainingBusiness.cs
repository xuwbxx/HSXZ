using DataEntity.Services;
using ExternalService.SafetyKitsService;
using ExternalService.Training;
using Model.HJPlat.Response;
using Model.Training;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace Business.Training
{
    public class TrainingBusiness
    {
        WorkService service = new WorkService();

        private readonly string safetyKitsAPIRoute = ConfigurationManager.AppSettings["SafetyKitsHost"] + "api/sqcp/SafetyKits/";
        private readonly string safetyKitsCode = ConfigurationManager.AppSettings["SafetyKitsCode"];

        public CultivateStatisticResponse GetTrainingStatistic()
        {
            CultivateStatisticResponse ret = new CultivateStatisticResponse();

            string response = NetUtils.HttpGet(safetyKitsAPIRoute + "GetTotal?Id=");

            if (!string.IsNullOrEmpty(response))
            {
                try
                {
                    ret = JsonUtility.Deserialize<CultivateStatisticResponse>(response);
                }
                catch (Exception ex)
                {
                    LogUtils.Instance.LogText("GetTrainingStatistic=============>" + ex.Message);
                    ret = new CultivateStatisticResponse();
                }
            }

            return ret;
        }

        public SafetyKitsResponse GetPersonDetail(string idStr)
        {
            SafetyKitsResponse ret = new SafetyKitsResponse();

            string response = NetUtils.HttpGet(safetyKitsAPIRoute + "GetPersonInfoByIdentifyID?IdentifyID=" + idStr);

            if (!string.IsNullOrEmpty(response))
            {
                try
                {
                    ret = JsonUtility.Deserialize<SafetyKitsResponse>(response);
                }
                catch (Exception ex)
                {
                    LogUtils.Instance.LogText("GetPersonDetail=============>" + ex.Message);
                    ret = new SafetyKitsResponse();
                }
            }

            return ret;
        }

        public byte[] GetPortrait(string personID)
        {
            byte[] retImg = new byte[] { };

            if (!string.IsNullOrEmpty(personID))
            {
                try
                {
                    SafetyKitsHelper helper = new SafetyKitsHelper();
                    retImg = helper.GetPortrait(safetyKitsCode, personID);
                }
                catch (Exception ex)
                {
                    LogUtils.Instance.LogText("TrainingBusiness.GetPortrait=============>" + ex.Message);
                    retImg = new byte[] { };
                }

            }

            return retImg;
        }

        public byte[] GetQRCode(string qrCodeStr)
        {
            byte[] retImg = new byte[] { };

            try
            {
                retImg = QRCodeUtils.CreateQRCodeToByte(qrCodeStr);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogText("GetQRCode=============>" + ex.Message);
                retImg = new byte[] { };
            }

            return retImg;
        }

    }
}
