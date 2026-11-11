using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class ConstructCfgScanTypeRequest : BaseRequest
    {
        public int ProjectID { set; get; }

        public int IsCommand { set; get; }

        public int ScanTypeID { set; get; }
    }
}
