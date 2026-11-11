using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Common;
using ZJFramework10.Extension;

namespace Model.HJPlat.Request
{
    public class ProgressRequest:BaseRequest
    {
        public int ID { set; get; }
        public double? PlanValue { set; get; }
        public double? ActualValue { set; get; }
        public DateTime? FillinMonth { set; get; }
        public string Type { set; get; }

        public int tempYear { set; get; }

        public int tempMonth { set; get; }

        public int Year
        {
            get
            {
                if (!string.IsNullOrEmpty(FillinMonth.AsStr()))
                {
                    return FillinMonth.Value.Year;
                }
                return 0;
            }
        }

        public int Month
        {
            get
            {
                if (!string.IsNullOrEmpty(FillinMonth.AsStr()))
                {
                    return FillinMonth.Value.Month;
                }
                return 0;
            }
        }
    }
}
