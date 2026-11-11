using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class ConstructCommandResponse
    {
        public string Date { set; get; }
        public string Hour { set; get; }
        public string Min { set; get; }
        public string DeviceDate { set; get; }

        public string BeginTime { set; get; }
    }
}
