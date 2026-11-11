using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class BaseRequest
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageSize { set; get; }

        public int GetSkipCount()
        {
            if (PageIndex == 0)
            {
                PageIndex = 1;
            }
            return (PageIndex - 1) * PageSize;
        }
    }
}
