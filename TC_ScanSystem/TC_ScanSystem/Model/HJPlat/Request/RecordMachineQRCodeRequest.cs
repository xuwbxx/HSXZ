using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Model.Common;

namespace Model.HJPlat.Request
{
    public class RecordMachineQRCodeRequest:BaseRequest
    {
        public string Idxs { get; set; }
        public string MachineCodes { get; set; }
    }
}
