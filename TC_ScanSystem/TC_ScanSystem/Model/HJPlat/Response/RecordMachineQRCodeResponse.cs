using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class RecordMachineQRCodeResponse
    {
        public string Idx { set; get; }
        public string MachineCode { set; get; }
        public string QRCodeUrl { set; get; }
    }
}
