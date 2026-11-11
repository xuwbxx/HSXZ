using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class SafetyHatResponse
    {
        public string AlarmName { set; get; }

        public string AlarmTime { set; get; }

        public string PicPath { set; get; }

        public string MoviePath { set; get; }
    }
}
