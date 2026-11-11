using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Common;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class RecordMachineRequest : BaseRequest
    {
        /// <summary>
        /// 机械设备代码列表，逗号作分隔符
        /// </summary>
        public string MachineCodes { set; get; }

        public string MachineName { set; get; }

        public string MachineCode { set; get; }
    }
}
