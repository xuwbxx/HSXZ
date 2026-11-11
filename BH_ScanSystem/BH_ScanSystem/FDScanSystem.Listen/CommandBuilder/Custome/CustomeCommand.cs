using FDScanSystem.Listen.Logic;
using FDScanSystem.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Listen.CommandBuilder.Custome
{
    public class CustomeCommand : CommandBase
    {
        public CustomeCommand(string msg)
        {
            Msg = ClientMsgEntity.DeSerialize(msg);
            this.Identity = Msg.Identity;
        }

        public override byte[] GetCommandBytes()
        {
            byte[] msgs = Encoding.UTF8.GetBytes(Msg.ToString());
            byte[] results = new byte[msgs.Length + 1];

            Buffer.BlockCopy(msgs, 0, results, 0, msgs.Length);
            results[msgs.Length] = 0x0A;
            return results;
        }

        protected ClientMsgEntity Msg { get; set; }
        public override int Type
        {
            get { return 5; }
        }
    }
}
