using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Protocol;

namespace FDScanSystem.Client.Logic
{
    public abstract class MessageProviderBase
    {


        public abstract bool Provide(ClientMsgEntity msg);
    }
}
