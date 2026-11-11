using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class LedConfigResponse
    {
        public LedConfigResponse()
        {
            this.Caves = new List<LedCave>();
            this.Structors = new List<LedStructor>();
        }

        public List<LedCave> Caves { set; get; }

        public List<LedStructor> Structors { set; get; }
    }

    public class LedCave
    {
        public string CaveName { set; get; }

        public int ID { set; get; }
    }

    public class LedStructor
    {
        public string StructorName { set; get; }

        public int ID { set; get; }
    }
}
