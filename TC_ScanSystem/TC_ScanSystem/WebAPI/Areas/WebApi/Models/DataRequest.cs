using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Areas.WebApi.Models
{
    public class DataRequest
    {
        public string BeginDate { set; get; }
        public string EndDate { set; get; }
        public int PointId { set; get; }
        public int PageSize { set; get; }
        public int PageIndex { set; get; }
      //  public string PointName { set; get; }
       // public string pName { set; get; } 
    }
}