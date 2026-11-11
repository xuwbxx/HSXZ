using FDScanSystem.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Listen.Logic
{

    public class DataReceiveArgs
    {

        public bool IsHandle { get; set; }

        public bool IsClearState { get; set; }

        public ClientMsgEntity ResponseMsg { get; set; }
    }
}
