using FDScanSystem.Client.Logic.MsgSender;
using FDScanSystem.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Client.Logic.MsgProvider
{
    public class MsgProvider_ResponseOK : MessageProviderBase
    {
        public override bool Provide(Protocol.ClientMsgEntity msg)
        {
            if (msg.Command.Equals(ClientMsgCode.Response_OK))
            {
                if (CommandDispatcher.GetInstance().CurrentCommand != null && CommandDispatcher.GetInstance().CurrentCommand.Identity.Equals(msg.Identity))
                {
                    CommandDispatcher.GetInstance().Done();
                }
                return true;
            }
            return false;
        }
    }
}

