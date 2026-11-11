using FDScanSystem.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Client.Logic.MsgSender
{
    public class ClientToServerMsgBase : ClientMsgEntity
    {


        int _Tick = 0;
        public virtual byte[] ToSerializeMsg()
        {
            byte[] bb = Encoding.UTF8.GetBytes(base.ToString());
            byte[] senderData = new byte[bb.Length + 1];
            Buffer.BlockCopy(bb, 0, senderData, 0, bb.Length);
            senderData[bb.Length] = 0x0A;
            return senderData;
        }

        public virtual void Callback() { 
        
            
        }

        public void Sended()
        {
            _Tick++;
        }

        public int GetTickCount() { return _Tick; }
    }
}
