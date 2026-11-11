using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeShop.WebModel;
using FD_ScanSystem.WebModel;
using FDScanSystem.DAO;
using ZJFramework10.Extension;
using TrainingsApp.Helper;
using FD_ScanSystem.WebModel.Enums;
using FDScanSystem.Protocol.ScanConfig;
using FDScanSystem.Protocol;

namespace FDScanSystem.Web.Setting.Dialog
{
    public partial class DeviceEdit : AdminPageBase
    {
        protected int DeviceId
        {
            get
            {
                return ZJFramework10.Utility.QueryStringManager.GetValue<int>("device", 0);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();
            if (!IsPostBack)
            {
                this.cbxCommunication.DataSource = db.FD_CommunicationDevice.ToList();
                this.cbxCommunication.ValueField = "ID";
                this.cbxCommunication.TextField = "DeviceName";
                this.cbxCommunication.ValueType = typeof(int);
                this.cbxCommunication.DataBind();


                this.cbxDeviceProtocol.DataSource = EnumHelper.GetEnumItems(typeof(DeviceProtocolEnum));
                this.cbxDeviceProtocol.ValueField = "Key";
                this.cbxDeviceProtocol.TextField = "Value";
                this.cbxDeviceProtocol.ValueType = typeof(int);
                this.cbxDeviceProtocol.DataBind();

                this.cbxNode.DataSource = db.FD_ScanNode.ToList();
                this.cbxNode.ValueField = "ID";
                this.cbxNode.TextField = "NodeName";
                this.cbxNode.ValueType = typeof(int);
                this.cbxNode.DataBind();

                var deviceInfo = db.FD_Device.FirstOrDefault(u => u.ID == DeviceId);
                if (deviceInfo != null)
                {
                    this.txtDeviceIdentity.Text = deviceInfo.DeviceIdentity;
                    this.txtDeviceName.Text = deviceInfo.DeviceName;
                    this.txtHowLong.Text = deviceInfo.HowLong.ToString();
                    this.txtPeriod.Text = deviceInfo.Perpiod.ToString();
                    this.cbxCommunication.Value = deviceInfo.CommunicationDeviceId;
                    this.cbxDeviceProtocol.Value = deviceInfo.DeviceProtocol;
                    this.cbxNode.Value = deviceInfo.NodeId;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            FD_Device deviceInfo = new FD_Device();
            deviceInfo.CreateDate = DateTime.Now;
            bool isUpdate = false;
            var db = DataContextFactory.Create();
            if (DeviceId > 0)
            {
                deviceInfo = db.FD_Device.FirstOrDefault(u => u.ID == DeviceId);
                isUpdate = deviceInfo != null;

            }
            deviceInfo.NodeId = cbxNode.Value.AsInt();
            deviceInfo.Perpiod = this.txtPeriod.Text.AsInt();
            deviceInfo.ProjectId = 0;
            deviceInfo.CommunicationDeviceId = cbxCommunication.Value.AsInt();
            deviceInfo.DeviceIdentity = this.txtDeviceIdentity.Text;
            deviceInfo.DeviceName = this.txtDeviceName.Text;
            deviceInfo.DeviceProtocol = cbxDeviceProtocol.Value.AsInt();
            deviceInfo.HowLong = this.txtHowLong.Text.AsInt();
            deviceInfo.LastestDate = DateTime.Now;
            deviceInfo.Interval = "0-0-0";
            DateTime dt = DateTime.Now;
            deviceInfo.BeginTime = new DateTime(dt.Year, dt.Month, dt.Day, 12, dt.Minute, dt.Second);

            deviceInfo.DeviceTime = DateTime.Now;
            if (!isUpdate) db.FD_Device.InsertOnSubmit(deviceInfo);

            db.SubmitChanges();


            var deviceGroupList = db.FD_Device.Where(u => u.DeviceProtocol == (int)DeviceProtocolEnum.JIASU).GroupBy(u => u.CommunicationDeviceId).ToList();
            foreach (var deviceGroupInfo in deviceGroupList)
            {

                var communication = db.FD_CommunicationDevice.FirstOrDefault(u => u.ID == deviceGroupInfo.Key);
                if (communication == null) continue;
                List<DeviceInfo> msgContent = new List<DeviceInfo>(3);
                int deviceId = 0;
                string deviceIdentity = string.Empty;
                foreach (var deviceInfo1 in deviceGroupInfo)
                {
                    DeviceInfo deviceConfig = new DeviceInfo();
                    deviceId = deviceInfo1.ID;
                    deviceIdentity = deviceInfo1.DeviceIdentity;
                    deviceConfig.DeviceId = deviceInfo1.ID;
                    deviceConfig.DeviceIdentity = deviceInfo1.DeviceIdentity;
                    deviceConfig.CommunicationDeviceId = deviceInfo1.CommunicationDeviceId;
                    deviceConfig.HowLong = deviceInfo1.HowLong;
                    deviceConfig.Interval = deviceInfo1.Perpiod;
                    deviceConfig.ZDJZPL = deviceInfo1.ZDJZPL ?? 1;
                    deviceConfig.TDJZPL = deviceInfo1.TDJZPL ?? 1;
                    deviceConfig.Points = new Dictionary<int, PointInfo>();

                    var points = db.FD_ScanPoint.Where(u => u.DeviceId == deviceInfo.ID);
                    foreach (var point in points)
                    {
                        deviceConfig.Points.Add(point.ID, new PointInfo
                        {
                            AlertMax = point.AlertMax ?? 0,
                            AlertMin = point.AlertMin ?? 0,
                            Channel1 = point.Channel1,
                            Channel2 = point.Channel2,
                            DeviceId = point.DeviceId,
                            PointId = point.ID,
                            StandardValue = point.Other1.AsDouble(),
                            lingmingValue = point.Other2.AsDouble(),
                            Perpiod = point.Perpiod ?? 0
                        });
                    }
                    msgContent.Add(deviceConfig);
                }

                ClientMsgEntity msg = new ClientMsgEntity();
                msg.Command = ClientMsgCode.ServerToClient_SetPeriod2;
                msg.AddParameter("Content", ZJFramework10.Serialize.Helper.SerializeString(msgContent));
                db.FD_Command.InsertOnSubmit(new FD_Command
                {
                    Command = msg.ToString(),
                    CommunicationDeviceId = communication.ID,
                    CommunicationIden = communication.CommunicationCode,
                    CreateDate = DateTime.Now,
                    DeviceId = deviceId,
                    DeviceIden = deviceIdentity,
                    // deviceInfo.DeviceIdentity,
                    IsSystem = 0,
                    LastPointId = 0,
                    Status = 0,
                    Result = string.Empty
                });
            }


            //db.SubmitChanges();
            //model.DictID = model.ID;
            db.SubmitChanges();



            MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");



        }

        public override string PermissionCode
        {
            get { return PermissionDict.Setting_Device_Manager; }
        }
    }
}