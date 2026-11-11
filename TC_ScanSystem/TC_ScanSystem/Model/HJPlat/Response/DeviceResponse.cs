using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class DeviceResponse
    {
        #region Model
        private int _id;
        private string _devicename;
        private string _deviceiden;
        private int? _layerid;
        private string _layername;
        private int? _communicationdeviceid;
        private string _commnunicationname;
        private int? _deviceprotocol;
        private DateTime? _devicetime;
        private int? _projectid;
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
        public string DeviceName
        {
            set { _devicename = value; }
            get { return _devicename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DeviceIden
        {
            set { _deviceiden = value; }
            get { return _deviceiden; }
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
        public string LayerName
        {
            set { _layername = value; }
            get { return _layername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CommunicationDeviceId
        {
            set { _communicationdeviceid = value; }
            get { return _communicationdeviceid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CommunicationName
        {
            set { _commnunicationname = value; }
            get { return _commnunicationname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DeviceProtocol
        {
            set { _deviceprotocol = value; }
            get { return _deviceprotocol; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DeviceTime
        {
            set { _devicetime = value; }
            get { return _devicetime; }
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
        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }
        #endregion Model

        public string Type { set; get; }

        public string DeviceProtocolName { set; get; }

        public string CreateTime { set; get; }
        public string Date { set; get; }
        public string Hour { set; get; }
        public string Min { set; get; }

        public int? Status { set; get; }

        public string CRC { set; get; }

        public string StatusName
        {
            get
            {
                if(Status==0)
                {
                    return "未连接";
                }
                return "已连接";
            }
        }

        public string BeginTime { set; get; }

        public string Command { set; get; }
    }
}
