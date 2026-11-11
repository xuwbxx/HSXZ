using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeShop.WebModel;
using FD_ScanSystem.WebModel;
using FDScanSystem.DAO;
using TrainingsApp.Helper;
using FD_ScanSystem.WebModel.Enums;
using ZJFramework10.Extension;
using OfficeShop.Model;
using FD_ScanSystem.WebModel.View;
using FDScanSystem.Protocol.ScanConfig;
using FDScanSystem.Protocol;

namespace FDScanSystem.Web.Setting
{
    public partial class DeviceList : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            UserContext user = PassPort.CurrentUser;
            var adminContext = user as AdminUserContext;
            if (adminContext != null)
            {
                var url = adminContext.FunctionRights.Where(u => !string.IsNullOrEmpty(u.FunctionUrl)).FirstOrDefault();
                if (url != null)
                {
                    ltUrl.Text = "<a href=\"" + url.FunctionUrl + "\">首页</a>";
                }
            }
            else
            {
                Response.Redirect("/login.aspx");
            }

            if (!IsPostBack)
            {
                this.cbxProtocol.DataSource = EnumHelper.GetEnumItems(typeof(DeviceProtocolEnum));
                this.cbxProtocol.ValueField = "Key";
                this.cbxProtocol.TextField = "Value";
                this.cbxProtocol.ValueType = typeof(int);
                this.cbxProtocol.DataBind();

                this.cbxNode.DataSource = db.FD_ScanNode.ToList();
                this.cbxNode.ValueField = "ID";
                this.cbxNode.TextField = "NodeName";
                this.cbxNode.ValueType = typeof(int);
                this.cbxNode.DataBind();

                cbxNode.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("-选择风机-", -1));
                cbxProtocol.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("-选择设备类型-", -1));
                this.cbxNode.SelectedIndex = 0;
                this.cbxProtocol.SelectedIndex = 0;
            }
            DataCreated();
        }


        protected void btnProcess_Click(object sender, EventArgs e)
        {
            DAO.FD_Scan_Setting setting = new DAO.FD_Scan_Setting();
            var db = DataContextFactory.Create();
            //bool isUpdate = true;
            //var scanSetting = db.FD_Scan_Setting.FirstOrDefault(u => u.ScanType == (int)ScanTypeEnum.Dynamic);

            //if (scanSetting == null)
            //{
            //    scanSetting = new FD_Scan_Setting();
            //    isUpdate = false;
            //}
            //scanSetting.ScanType = (int)ScanTypeEnum.Dynamic;
            //scanSetting.BeginDate = this.txtBeginDate.SelectedDate;
            //scanSetting.Interval = this.txtPeriod.Text.AsInt() * 1000;
            //if (!isUpdate)
            //{
            //    db.FD_Scan_Setting.InsertOnSubmit(scanSetting);
            //}

            //var result = db.FD_Device.Where(u => u.DeviceProtocol == (int)DeviceProtocolEnum.JIASU).ToList();
            //foreach (var item in result)
            //{
            //    int period = ZJFramework10.Utility.FormManager.GetValue<int>("txt_period_" + item.ID);
            //    int howlong = ZJFramework10.Utility.FormManager.GetValue<int>("txt_howlong_" + item.ID);

            //    item.Perpiod = period <= 0 ? ClientConstSet.Default_Dynamic_Period : period;
            //    item.HowLong = howlong <= 0 ? ClientConstSet.Default_Dynamic_HowLong : howlong;
            //}
            //db.SubmitChanges();

            //var source = (from a in db.FD_Device
            //              where a.DeviceProtocol == (int)DeviceProtocolEnum.JIASU
            //              select new
            //              {
            //                  a.ID,
            //                  a.DeviceName,
            //                  a.Perpiod,
            //                  a.HowLong,
            //                  NodeTextPath = db.FD_ScanNode.Where(h => h.ID == a.NodeId).Select(h => h.NodeName).FirstOrDefault()
            //              }).ToList();

            //this.rptDataSource.DataSource = source;
            //this.rptDataSource.DataBind();

            //this.txtBeginDate.SelectedDate = scanSetting.BeginDate;
            //this.txtPeriod.Text = ((int)(scanSetting.Interval * 0.001)).ToString();


            var deviceGroupList = db.FD_Device.Where(u => u.DeviceProtocol == (int)DeviceProtocolEnum.JIASU).GroupBy(u => u.CommunicationDeviceId).ToList();
            foreach (var deviceGroupInfo in deviceGroupList)
            {

                var communication = db.FD_CommunicationDevice.FirstOrDefault(u => u.ID == deviceGroupInfo.Key);
                if (communication == null) continue;
                List<DeviceInfo> msgContent = new List<DeviceInfo>(3);
                int deviceId = 0;
                string deviceIdentity = string.Empty;
                foreach (var deviceInfo in deviceGroupInfo)
                {
                    DeviceInfo deviceConfig = new DeviceInfo();
                    deviceId = deviceInfo.ID;
                    deviceIdentity = deviceInfo.DeviceIdentity;
                    deviceConfig.DeviceId = deviceInfo.ID;
                    deviceConfig.DeviceIdentity = deviceInfo.DeviceIdentity;
                    deviceConfig.CommunicationDeviceId = deviceInfo.CommunicationDeviceId;
                    deviceConfig.HowLong = deviceInfo.HowLong;
                    deviceConfig.Interval = deviceInfo.Perpiod;
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
                            lingmingValue = point.Other2.AsDouble()
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

            db.SubmitChanges();


            this.Successs("发送成功");
        }


        protected void DataCreated()
        {

            var db = DataContextFactory.Create();
            IQueryable<FD_Device> querySource = db.FD_Device;
            if (cbxNode.SelectedIndex > 0)
            {
                int nodeId = cbxNode.Value.AsInt();
                querySource = querySource.Where(u => u.NodeId == nodeId);
            }
            if (cbxProtocol.SelectedIndex > 0)
            {
                int protocol = cbxProtocol.Value.AsInt();
                querySource = querySource.Where(u => u.DeviceProtocol == protocol);
            }
            if (!string.IsNullOrEmpty(this.txtName.Text))
            {

                querySource = querySource.Where(u => u.DeviceName.Contains(this.txtName.Text));
            }

            this.aspGrid.DataSource = querySource.Select(u => new DeviceView
            {
                CreateDate = u.CreateDate,
                DeviceIdentity = u.DeviceIdentity,
                DeviceName = u.DeviceName,
                DeviceProtocol = u.DeviceProtocol,
                NodeId = u.NodeId,
                ID = u.ID,
                CommunicationName = db.FD_CommunicationDevice.Where(h => h.ID == u.CommunicationDeviceId).Select(h => h.DeviceName).FirstOrDefault(),
                NodeName = db.FD_ScanNode.Where(h => h.ID == u.NodeId).Select(t => t.NodeName).FirstOrDefault()
            }).ToList();
            this.aspGrid.DataBind();


        }

        protected void aspGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int key = e.Keys[0].AsInt();
            if (key > 0)
            {
                var db = DataContextFactory.Create();
                var cinfo = db.FD_Device.FirstOrDefault(u => u.ID == key);
                if (cinfo != null)
                {
                    db.FD_Device.DeleteOnSubmit(cinfo);
                    db.SubmitChanges();
                }
            }
        }

        protected void aspGrid_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            e.ExceptionHandled = true;
            DataCreated();
        }


        public override string PermissionCode
        {
            get { return PermissionDict.Setting_Device_Manager; }
        }
    }
}