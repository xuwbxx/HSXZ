using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class ConstructCfgViewPositionResponse
    {
        #region Model
        private int _id;
        private string _positionname;
        private int _layerid;
        private string _layername;
        private int _scantypeid;
        private string _typename;
        private int _projecyid;
        private string _projectname;
        private DateTime? _createdate;
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
        public string PositionName
        {
            set { _positionname = value; }
            get { return _positionname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int LayerID
        {
            set { _layerid = value; }
            get { return _layerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LayerName
        {
            set { _layername = value; }
            get { return _layername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ScanTypeID
        {
            set { _scantypeid = value; }
            get { return _scantypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TypeName
        {
            set { _typename = value; }
            get { return _typename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ProjecyID
        {
            set { _projecyid = value; }
            get { return _projecyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
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

        public string CreateTime { set; get; }
    }
}
