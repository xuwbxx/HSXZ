using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class BaseReturn
    {
        public BaseReturn()
        {
            Success = false;
        }
        public bool Success { set; get; }
        public string Message { set; get; }

        public int TotalPage { set; get; }
        public int TotalCount { set; get; }

        public int PageIndex { set; get; }
        public object Data { set; get; }
       // public object Echart { set; get; }
        public object Foot { set; get; }

        public object Echart { set; get; }

        public int value { set; get; }

        public string Url { set; get; }
        public string title1 { set; get; }
        public string title2 { set; get; }
    }
}
