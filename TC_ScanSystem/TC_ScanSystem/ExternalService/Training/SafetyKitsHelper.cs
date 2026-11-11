using ExternalService.SafetyKitsService;
using Model.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace ExternalService.Training
{
    public class SafetyKitsHelper
    {
        public byte[] GetPortrait(string safetyKey, string personID)
        {
            byte[] retImg = new byte[] { };

            if (!string.IsNullOrEmpty(personID))
            {
                try
                {
                    DataServiceSoapClient svi = new DataServiceSoapClient();

                    retImg = svi.GetPersonPicture(new CheckHeader() { Key = safetyKey }, personID);
                }
                catch (Exception ex)
                {
                    LogUtils.Instance.LogText("SafetyKitsHelper.GetPortrait=============>" + ex.Message);
                }
            }

            return retImg;
        }
    }
}
