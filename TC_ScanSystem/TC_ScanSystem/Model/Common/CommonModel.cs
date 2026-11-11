using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class CommonModel
    {
        public bool Success { set; get; }
        public string ErrorCode { set; get; }
        public string Error { set; get; }

        public int TotalCount { set; get; }

        public int PageCount { set; get; }
    }
}
