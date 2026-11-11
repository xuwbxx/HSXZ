using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Client.Logic.MsgProvider
{
    public class MsgProvider_10001 : MessageProviderBase
    {
        public override bool Provide(Protocol.ClientMsgEntity msg)
        {
            //业务逻辑处理
            //  throw new NotImplementedException();

            AppContext.Success(string.Format("收到一条来自服务器的消息:{0}", msg.Identity));
            return false;
        }
    }
}
