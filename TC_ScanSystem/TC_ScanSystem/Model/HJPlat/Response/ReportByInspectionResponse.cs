using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class ReportByInspectionResponse
    {
        public int Idx { get; set; }
        public double CNT { get; set; }

        public double ZXJC { get; set; }

        public double RCJC { get; set; }

        public double ZHJC { get; set; }

        public double JJRJC { get; set; }

        public double QTJC { get; set; }

        public double ZJC { get; set; }

        public double YJC { get; set; }

        public string structName { get; set; }
    }
}
