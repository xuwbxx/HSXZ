using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Video
{
    public class VideoInfo
    {
        public List<TreeView> Data { set; get; }

        public VideoPosition Position { set; get; }
        
        public bool Success { set; get; }
    }

    public class VideoPosition
    {
        public VideoPosition()
        {
            Positions = new List<PositionInfo>();

            PositionInfo P1 = new PositionInfo();
            P1.Code = "001500";
            P1.Name = "天浦路洗车台枪机1#";
            P1.Lon = "118.675907";
            P1.Lat = "32.068054";
            Positions.Add(P1);

            PositionInfo P2 = new PositionInfo();
            P2.Code = "001501";
            P2.Name = "天浦路洗车台枪机2#";
            P2.Lon = "118.676697";
            P2.Lat = "32.067197";
            Positions.Add(P2);

            PositionInfo P3 = new PositionInfo();
            P3.Code = "001502";
            P3.Name = "二标制高点球机2#";
            P3.Lon = "118.664696";
            P3.Lat = "32.066478";
            Positions.Add(P3);
        }
        
        public List<PositionInfo> Positions { set; get; }
    }

    public class PositionInfo
    {
        public string Code { set; get; }

        public string Name { set; get; }
        public string Lon { set; get; }

        public string Lat { set; get; }
    }
}
