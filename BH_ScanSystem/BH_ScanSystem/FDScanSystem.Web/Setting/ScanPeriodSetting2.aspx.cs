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
using FD_ScanSystem.WebModel.Enums;
using FDScanSystem.Protocol.ScanConfig;
using FDScanSystem.Protocol;

namespace FDScanSystem.Web.Setting
{
    public partial class ScanPeriodSetting2 : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            var scanSetting = db.FD_Scan_Setting.FirstOrDefault(u => u.ScanType == (int)ScanTypeEnum.Dynamic);
            if (!IsPostBack)
            {
                var result = (from a in db.FD_Device
                              where a.DeviceProtocol == (int)DeviceProtocolEnum.JIASU
                              select new
                              {
                                  a.ID,
                                  a.DeviceName,
                                  a.Perpiod,
                                  a.HowLong,
                                  a.TDJZPL,
                                  a.ZDJZPL,
                                  NodeTextPath = db.FD_ScanNode.Where(h => h.ID == a.NodeId).Select(h => h.NodeName).FirstOrDefault()
                              }).ToList();

                this.rptDataSource.DataSource = result;
                this.rptDataSource.DataBind();

                if (scanSetting != null)
                {
                    this.txtBeginDate.SelectedDate = scanSetting.BeginDate;
                    this.txtPeriod.Text = ((int)(scanSetting.Interval * 0.001)).ToString();
                    
                }
            }

        }


        public override string PermissionCode
        {
            get { return PermissionDict.Setting_Period; }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DAO.FD_Scan_Setting setting = new DAO.FD_Scan_Setting();
            var db = DataContextFactory.Create();
            bool isUpdate = true;
            var scanSetting = db.FD_Scan_Setting.FirstOrDefault(u => u.ScanType == (int)ScanTypeEnum.Dynamic);

            if (scanSetting == null)
            {
                scanSetting = new FD_Scan_Setting();
                isUpdate = false;
            }
            scanSetting.ScanType = (int)ScanTypeEnum.Dynamic;
            scanSetting.BeginDate = this.txtBeginDate.SelectedDate;
            scanSetting.Interval = this.txtPeriod.Text.AsInt() * 1000;
            if (!isUpdate)
            {
                db.FD_Scan_Setting.InsertOnSubmit(scanSetting);
            }

            var result = db.FD_Device.Where(u => u.DeviceProtocol == (int)DeviceProtocolEnum.JIASU).ToList();
            foreach (var item in result)
            {
                int period = ZJFramework10.Utility.FormManager.GetValue<int>("txt_period_" + item.ID);
                int howlong = ZJFramework10.Utility.FormManager.GetValue<int>("txt_howlong_" + item.ID);
                int TDZJPL = ZJFramework10.Utility.FormManager.GetValue<int>("txt_TDJZPL_" + item.ID);
                int ZDZJPL = ZJFramework10.Utility.FormManager.GetValue<int>("txt_ZDJZPL_" + item.ID);

                item.Perpiod = period <= 0 ? ClientConstSet.Default_Dynamic_Period : period;
                item.HowLong = howlong <= 0 ? ClientConstSet.Default_Dynamic_HowLong : howlong;
                item.TDJZPL = TDZJPL <= 0 ? ClientConstSet.Default_Dynamic_TDZJPL : TDZJPL;
                item.ZDJZPL = ZDZJPL <= 0 ? ClientConstSet.Default_Dynamic_ZDZJPL : ZDZJPL;
            }
            db.SubmitChanges();

            var source = (from a in db.FD_Device
                          where a.DeviceProtocol == (int)DeviceProtocolEnum.JIASU
                          select new
                          {
                              a.ID,
                              a.DeviceName,
                              a.Perpiod,
                              a.HowLong,
                              a.TDJZPL,
                              a.ZDJZPL,
                              NodeTextPath = db.FD_ScanNode.Where(h => h.ID == a.NodeId).Select(h => h.NodeName).FirstOrDefault()
                          }).ToList();

            this.rptDataSource.DataSource = source;
            this.rptDataSource.DataBind();

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
                    deviceConfig.TDJZPL = deviceInfo.TDJZPL ?? 1;
                    deviceConfig.ZDJZPL = deviceInfo.ZDJZPL ?? 1;
                 
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

            db.SubmitChanges();


            this.Successs("保存成功");
        }


        protected string BindPerpiod(object val)
        {
            if (val != null)
            {
                int period = (int)val;
                if (period <= 0) return ClientConstSet.Default_Dynamic_Period.ToString();
                return period.ToString();
            }
            return ClientConstSet.Default_Dynamic_Period.ToString();
        }

        protected string BindHowLong(object val)
        {
            if (val != null)
            {
                int howlong = (int)val;
                if (howlong <= 0) return ClientConstSet.Default_Dynamic_HowLong.ToString();
                return howlong.ToString();
            }
            return ClientConstSet.Default_Dynamic_HowLong.ToString();
        }

        protected string BindTDJZPL(object val)
        {
            if (val != null)
            {
                int TDJZPL = (int)val;
                if (TDJZPL <= 0) return ClientConstSet.Default_Dynamic_TDZJPL.ToString();
                return TDJZPL.ToString();
            }
            return ClientConstSet.Default_Dynamic_TDZJPL.ToString();
        }

        protected string BindZDJZPL(object val)
        {
            if (val != null)
            {
                int ZDJZPL = (int)val;
                if (ZDJZPL <= 0) return ClientConstSet.Default_Dynamic_ZDZJPL.ToString();
                return ZDJZPL.ToString();
            }
            return ClientConstSet.Default_Dynamic_ZDZJPL.ToString();
        }
    }
}