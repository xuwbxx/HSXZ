using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class GantryCraneItemInfoResponse
    {

        public int Idx { get; set; }
        public string Name { get; set; }
        public GantryCraneItemInfoResponse()
        {
            this.statusInfo = new GantryCraneStatusResponse();
        }
        private GantryCraneStatusResponse statusInfo;

        public GantryCraneStatusResponse StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; }
        }
    }
}
