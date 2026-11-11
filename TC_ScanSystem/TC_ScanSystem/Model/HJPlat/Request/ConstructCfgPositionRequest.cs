using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class ConstructCfgPositionRequest : BaseRequest
    {
        public int ID { set; get; }

        public int ProjectID { set; get; }

        public int LayerID { set; get; }

        public int ScanTypeID { set; get; }
    }
}
