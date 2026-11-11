using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class CommunicationResponse
    {
        #region Model
        private string _communicationname;
        private int? _communicationtype;
        private int? _devicetype;
        private int? _dataprotocol;
        private string _ip;
        private int? _port;
        private string _communicationcode;
        private string _projectname;
        private int _id;
        private int? _projectid;
        /// <summary>
        /// 
        /// </summary>
        public string CommunicationName
        {
            set { _communicationname = value; }
            get { return _communicationname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CommunicationType
        {
            set { _communicationtype = value; }
            get { return _communicationtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DeviceType
        {
            set { _devicetype = value; }
            get { return _devicetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DataProtocol
        {
            set { _dataprotocol = value; }
            get { return _dataprotocol; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Port
        {
            set { _port = value; }
            get { return _port; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CommunicationCode
        {
            set { _communicationcode = value; }
            get { return _communicationcode; }
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
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProjectId
        {
            set { _projectid = value; }
            get { return _projectid; }
        }
        #endregion Model

        public string CommunicationTypeName { set; get; }
        public string DeviceTypeName { set; get; }
        public string DataProtocolName { set; get; }

        public string Type { set; get; }

    }
}
