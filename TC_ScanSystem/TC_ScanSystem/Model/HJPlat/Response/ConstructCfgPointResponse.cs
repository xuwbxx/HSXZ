using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class ConstructCfgPointResponse
    {
        #region Model
        private int _id;
        private string _pointname;
        private int? _positionid;
        private int? _deviceid;
        private string _mapx;
        private string _mapy;
        private double? _maxnum;
        private double? _maxnum2;
        private double? _maxnum3;
        private double? _minnum;
        private double? _minnum2;
        private double? _minnum3;
        private DateTime? _createdate;
        private string _unit;
        private string _unit2;
        private string _mapdataname;
        private string _mapdataname2;
        private string _pname;
        private double? _height;
        private double? _testheight;
        private int? _scantypeid;
        private double? _para1;
        private double? _para2;
        private double? _para3;
        private double? _para4;
        private double? _para5;
        private double? _para6;
        private double? _para7;
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
        public string PointName
        {
            set { _pointname = value; }
            get { return _pointname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PositionId
        {
            set { _positionid = value; }
            get { return _positionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DeviceId
        {
            set { _deviceid = value; }
            get { return _deviceid; }
        }

        public int? ScanTypeId
        {
            set { _scantypeid = value; }
            get { return _scantypeid; }
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
        public double? MaxNum
        {
            set { _maxnum = value; }
            get { return _maxnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double? MaxNum2
        {
            set { _maxnum2 = value; }
            get { return _maxnum2; }
        }
        public double? MaxNum3
        {
            set { _maxnum3 = value; }
            get { return _maxnum3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double? MinNum
        {
            set { _minnum = value; }
            get { return _minnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double? MinNum2
        {
            set { _minnum2 = value; }
            get { return _minnum2; }
        }
        public double? MinNum3
        {
            set { _minnum3 = value; }
            get { return _minnum3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Unit
        {
            set { _unit = value; }
            get { return _unit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Unit2
        {
            set { _unit2 = value; }
            get { return _unit2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MapDataName
        {
            set { _mapdataname = value; }
            get { return _mapdataname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MapDataName2
        {
            set { _mapdataname2 = value; }
            get { return _mapdataname2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string pName
        {
            set { _pname = value; }
            get { return _pname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double? Height
        {
            set { _height = value; }
            get { return _height; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double? testHeight
        {
            set { _testheight = value; }
            get { return _testheight; }
        }
        #endregion Model

        public string Type { set; get; }

        public string MapXs { set; get; }
        public string MapYs { set; get; }

        public string TypeName { set; get; }

        public string ImagePath { set; get; }

        public int? IsCommand { set; get; }

        public string Url { set; get; }

        public string SensorNumber { set; get; }
        public int? SensorType { set; get; }

        public int? DeviceProtocol { set; get; }

        public double? Para1 { set; get; }
        public double? Para3 { set; get; }
        public double? Para2 { set; get; }
        public double? Para4 { set; get; }
        public double? Para5 { set; get; }
        public double? Para6 { set; get; }
        public double? Para7 { set; get; }
    }
}
