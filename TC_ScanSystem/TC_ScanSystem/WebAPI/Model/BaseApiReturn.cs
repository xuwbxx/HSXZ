using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Model
{
    public class BaseApiReturn
    {
       
            public BaseApiReturn()
            {
                Success = false;
            }

            /// <summary>
            /// 是否成功
            /// </summary>
            public bool Success { set; get; }

            /// <summary>
            /// 如果是false,返回错误信息
            /// </summary>
            public string Message { set; get; }

            /// <summary>
            /// 返回行数
            /// </summary>
            public int TotalCount { set; get; }

            /// <summary>
            /// 主数据内容
            /// </summary>
            public object Data { set; get; }
    }
}