using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class ConstructCfgScanTypeResponse
    {
        #region Model
        private int _id;
        private string _typename;
        private string _description;
        private string _imagepath;
        private string _protocol;
        private int? _projectid;
        private int? _iscommand;
        private string _projectname;
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
        public string TypeName
        {
            set { _typename = value; }
            get { return _typename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImagePath
        {
            set { _imagepath = value; }
            get { return _imagepath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Protocol
        {
            set { _protocol = value; }
            get { return _protocol; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProjectId
        {
            set { _projectid = value; }
            get { return _projectid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsCommand
        {
            set { _iscommand = value; }
            get { return _iscommand; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }
        #endregion Model

        public string CreateTime { set; get; }

        public string Type { set; get; }
    }
}
