using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZJFramework10.Extension;

namespace Model.HJPlat.Response
{
    public class ProgressResponse
    {
        public int ID { set; get; }
        public double? PlanValue { set; get; }
        public double? ActualValue { set; get; }
        public DateTime? FillinMonth { set; get; }
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
        public string ShowDate { get { return FillinMonth.Value.ToString("yyyy-MM"); } }
    }
}
