using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class ConstructCfgLayerResponse
    {
        public int ID { set; get; }

        public string LayerName { set; get; }

        public string ProjectName { set; get; }

        public string CreateTime { set; get; }

        public string Description { set; get; }

        public int ProjectID { set; get; }
        public int ScanTypeID { set; get; }

        public string Type { set; get; }

        public string MapX { set; get; }
        public string MapY { set; get; }

        public string MapXs { set; get; }
        public string MapYs { set; get; }

        public string BackgroundPath { set; get; }
    }
}
