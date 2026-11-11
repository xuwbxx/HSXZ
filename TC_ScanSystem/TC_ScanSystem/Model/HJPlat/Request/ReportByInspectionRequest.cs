using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Common;

namespace Model.HJPlat.Request
{
    public class ReportByInspectionRequest:BaseRequest
    {
        public string StructId { set; get; }
        public DateTime? begin { set; get; }
        public DateTime? end { set; get; }

        public int? Type { set; get; }
    }
}
