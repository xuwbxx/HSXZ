using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Request
{
    public class Base_LED_SyncInfoRequest
    {
        #region Model
        private int _id;
        private int _structorid;
        private int _caveid;
        private int _locendpointid;
        private string _title;
        private string _helloword;
        private string _processword;
        private string _content1;
        private string _content2;
        private DateTime _syncdata;
        private DateTime _updatedate;
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
        public int StructorId
        {
            set { _structorid = value; }
            get { return _structorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CaveId
        {
            set { _caveid = value; }
            get { return _caveid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int LocEndPointId
        {
            set { _locendpointid = value; }
            get { return _locendpointid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HelloWord
        {
            set { _helloword = value; }
            get { return _helloword; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProcessWord
        {
            set { _processword = value; }
            get { return _processword; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Content1
        {
            set { _content1 = value; }
            get { return _content1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Content2
        {
            set { _content2 = value; }
            get { return _content2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime SyncData
        {
            set { _syncdata = value; }
            get { return _syncdata; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            set { _updatedate = value; }
            get { return _updatedate; }
        }
        #endregion Model

        public string Type { set; get; }
    }
}
