using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class ConstructCfgPositionResponse
    {
        #region Model
        private int _id;
        private int? _layerid;
        private int? _scantypeid;
        private string _positionname;
        private string _mapx;
        private string _mapy;
        private string _backgroundpath;
        private DateTime? _createdate;
        //private string _pilelength;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LayerId
        {
            set { _layerid = value; }
            get { return _layerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ScanTypeId
        {
            set { _scantypeid = value; }
            get { return _scantypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PositionName
        {
            set { _positionname = value; }
            get { return _positionname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MapX
        {
            set { _mapx = value; }
            get { return _mapx; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MapY
        {
            set { _mapy = value; }
            get { return _mapy; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BackGroundPath
        {
            set { _backgroundpath = value; }
            get { return _backgroundpath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        #endregion Model

        public string Type { set; get; }

        public string MapXs { set; get; }
        public string MapYs { set; get; }

        public int Width { set; get; }

        public int Height { set; get; }

        public string Widths { set; get; }

        public string Heights { set; get; }

        public string TypeName { set; get; }
        public string ImagePath { set; get; }

        public int? IsCommand { set; get; }
        public string Url { set; get; }
        public string PileLength { set; get; }
        public string PileDiameter { set; get; }
        public string PileNumber { set; get; }
    }
}
