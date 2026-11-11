using FDScanSystem.Client.Logic.Config;
using FDScanSystem.Protocol.ScanConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Client.Logic.MsgProvider
{
    public class MsgProvider_10002 : MessageProviderBase
    {
        public override bool Provide(Protocol.ClientMsgEntity msg)
        {
            //业务逻辑处理
            //  throw new NotImplementedException();
            object config = string.Empty;
            if (msg.Paramters.TryGetValue("Content", out config))
            {
                ScanSetting settingInfo = ZJFramework10.Serialize.Helper.DeserializeString<ScanSetting>(config.ToString());
                if (settingInfo != null)
                {

                    ScanSettingConfig.GetInstance().Update(settingInfo);
                }
            }
            AppContext.Success(string.Format("收到一条来自服务器的消息:{0}", msg.Identity));
            return false;
        }
    }
}
