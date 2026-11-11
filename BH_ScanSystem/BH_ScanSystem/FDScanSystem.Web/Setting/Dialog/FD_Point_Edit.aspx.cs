using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FD_ScanSystem.WebModel;
using FD_ScanSystem.WebModel.Enums;
using FDScanSystem.DAO;
using FDScanSystem.DAO.Model;
using FDScanSystem.Protocol;
using FDScanSystem.Protocol.ScanConfig;
using OfficeShop.DAL;
using OfficeShop.Model;
using OfficeShop.WebModel;
using ZJFramework10;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.Setting.Dialog
{
    public partial class FD_Point_Edit : AdminPageBase
    {
        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("cid");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var db = DataContextFactory.Create();
                drpNode.DataSource = db.FD_ScanNode.Where(u => u.NodeParentId == 0).ToList();
                drpNode.DataTextField = "NodeName";
                drpNode.DataValueField = "ID";
                drpNode.DataBind();
                drpNode.Items.Insert(0, new ListItem("请选择", "0"));
                drpNode.SelectedIndex = 0;

                drpScanType.DataSource = db.FD_Dict.Where(u => u.DictType == DictTypeEnum.SCANTYPE).ToList();
                drpScanType.DataTextField = "DictValue";
                drpScanType.DataValueField = "DictID";
                drpScanType.DataBind();

                drpCom.DataSource = db.FD_CommunicationDevice.ToList();
                drpCom.DataTextField = "DeviceName";
                drpCom.DataValueField = "ID";
                drpCom.DataBind();
                if (drpCom.Items.Count > 0)
                {
                    drpCom.SelectedIndex = 0;
                }



                drpScanPoint.DataSource = db.FD_ScanPoint.OrderBy(u => u.Remark).ToList();
                drpScanPoint.DataTextField = "Remark";
                drpScanPoint.DataValueField = "ID";
                drpScanPoint.DataBind();
                drpScanPoint.Items.Insert(0, new ListItem("请选择", "0"));
                drpScanPoint.SelectedIndex = 0;
                if (Id > 0)
                {
                    var model = db.FD_ScanPoint.SingleOrDefault(u => u.ID == Id);
                    if (model != null)
                    {
                        txtChannel1.Text = model.Channel1;
                        txtChannel2.Text = model.Channel2;
                        txtMax1.Text = model.AlertMax.ToString();
                        txtMax2.Text = model.AlertMax2.ToString();
                        txtMin1.Text = model.AlertMin.ToString();
                        txtMin2.Text = model.AlertMin2.ToString();
                        txtName.Text = model.PointName;
                        txtRemark.Text = model.Remark;
                        txtUnit1.Text = model.Unit;
                        txtUnit2.Text = model.Unit2;
                        txtOther.Text = model.Other1;
                        txtOther2.Text = model.Other2;
                        txtX.Text = model.MapX;
                        txtY.Text = model.MapY;
                        drpScanPoint.SelectedValue = model.NewID.ToString();
                        txtpara1.Text = model.para1.ToString();
                        txtpara2.Text = model.para2.ToString();
                        txtpara3.Text = model.para3.ToString();
                        txtpara4.Text = model.para4.ToString();
                        txtpara5.Text = model.para5.ToString();
                        txtpara6.Text = model.para6.ToString();

                        drpNode.SelectedValue = model.NodeId.ToString();
                        drpScanType.SelectedValue = model.ScanTypeId.ToString();

                        var device = db.FD_Device.SingleOrDefault(u => u.ID == model.DeviceId);
                        if (device != null)
                        {
                            drpCom.SelectedValue = device.CommunicationDeviceId.ToString();
                            drpDevice.DataSource = db.FD_Device.Where(u => u.CommunicationDeviceId == device.CommunicationDeviceId).ToList();
                            drpDevice.DataTextField = "DeviceName";
                            drpDevice.DataValueField = "ID";
                            drpDevice.DataBind();
                            drpDevice.SelectedValue = model.DeviceId.ToString();

                            if (device.DeviceProtocol == (int)DeviceProtocolEnum.JIASU)
                            {
                                txtChannel2.Enabled = false;
                                txtMax2.Enabled = false;
                                txtMin2.Enabled = false;
                                txtUnit2.Enabled = false;
                                txtOther2.Enabled = true;
                            }
                            else
                            {
                                txtChannel2.Enabled = true;
                                txtMax2.Enabled = true;
                                txtMin2.Enabled = true;
                                txtUnit2.Enabled = true;
                                txtOther2.Enabled = false;
                            }

                        }

                        var layer = db.FD_ScanLayer.SingleOrDefault(u => u.ID == model.LayerId);
                        if (layer != null)
                        {
                            drpNode.SelectedValue = layer.NodeId.ToString();
                            drpLayer.DataSource = db.FD_ScanLayer.Where(u => u.NodeId == layer.NodeId).ToList();
                            drpLayer.DataTextField = "LayerName";
                            drpLayer.DataValueField = "ID";
                            drpLayer.DataBind();
                            drpLayer.SelectedValue = model.LayerId.ToString();
                        }
                        else
                        {
                            //drpLayer.SelectedValue = layer.NodeId.ToString();
                            drpLayer.DataSource = db.FD_ScanLayer.Where(u => u.NodeId == drpNode.SelectedValue.AsInt()).ToList();
                            drpLayer.DataTextField = "LayerName";
                            drpLayer.DataValueField = "ID";
                            drpLayer.DataBind();
                            drpLayer.SelectedValue = model.LayerId.ToString();
                        }


                    }
                }
                else
                {
                    if (drpCom.SelectedIndex >= 0)
                    {
                        //drpCom.SelectedValue = drpCom..ToString();
                        drpDevice.DataSource = db.FD_Device.Where(u => u.CommunicationDeviceId == drpCom.SelectedValue.AsInt()).ToList();
                        drpDevice.DataTextField = "DeviceName";
                        drpDevice.DataValueField = "ID";
                        drpDevice.DataBind();
                    }
                    if (drpNode.SelectedIndex > 0)
                    {
                        drpLayer.DataSource = db.FD_ScanLayer.Where(u => u.NodeId == drpNode.SelectedValue.AsInt()).ToList();
                        drpLayer.DataTextField = "LayerName";
                        drpLayer.DataValueField = "ID";
                        drpLayer.DataBind();
                    }

                }

            }
        }

        protected void drpDevice_Select(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();
            drpDevice.DataSource = db.FD_Device.Where(u => u.CommunicationDeviceId == drpCom.SelectedValue.AsInt()).ToList();
            drpDevice.DataTextField = "DeviceName";
            drpDevice.DataValueField = "ID";
            drpDevice.DataBind();

            if (drpDevice.Items.Count > 0)
            {
                var model = db.FD_Device.SingleOrDefault(u => u.ID == drpDevice.SelectedValue.AsInt());
                if (model != null)
                {
                    if (model.DeviceProtocol == (int)DeviceProtocolEnum.JIASU)
                    {
                        txtChannel2.Enabled = false;
                        txtMax2.Enabled = false;
                        txtMin2.Enabled = false;
                        txtUnit2.Enabled = false;
                        txtOther2.Enabled = true;

                    }
                    else
                    {
                        txtChannel2.Enabled = true;
                        txtMax2.Enabled = true;
                        txtMin2.Enabled = true;
                        txtUnit2.Enabled = true;
                        txtOther2.Enabled = false;
                    }
                }
            }
        }

        protected void drpLayer_Select(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();

            drpLayer.DataSource = db.FD_ScanLayer.Where(u => u.NodeId == drpNode.SelectedValue.AsInt()).ToList();
            drpLayer.DataTextField = "LayerName";
            drpLayer.DataValueField = "ID";
            drpLayer.DataBind();

        }

        protected void drpD_Select(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();
            if (drpDevice.Items.Count > 0)
            {
                var model = db.FD_Device.SingleOrDefault(u => u.ID == drpDevice.SelectedValue.AsInt());
                if (model != null)
                {
                    if (model.DeviceProtocol == (int)DeviceProtocolEnum.JIASU)
                    {
                        txtChannel2.Enabled = false;
                        txtMax2.Enabled = false;
                        txtMin2.Enabled = false;
                        txtUnit2.Enabled = false;
                        txtOther2.Enabled = true;
                    }
                    else
                    {
                        txtChannel2.Enabled = true;
                        txtMax2.Enabled = true;
                        txtMin2.Enabled = true;
                        txtUnit2.Enabled = true;
                        txtOther2.Enabled = false;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            var db = DataContextFactory.Create();

            FD_ScanPoint model = new FD_ScanPoint();
            if (Id > 0)
            {
                model = db.FD_ScanPoint.SingleOrDefault(u => u.ID == Id);
            }
            model.PointName = txtName.Text;
            model.CreateDate = DateTime.Now;
            model.Channel1 = txtChannel1.Text;
            model.Channel2 = txtChannel2.Text;
            model.AlertMax = txtMax1.Text.AsDouble();
            model.AlertMax2 = txtMax2.Text.AsDouble();
            model.AlertMin = txtMin1.Text.AsDouble();
            model.AlertMin2 = txtMin2.Text.AsDouble();
            model.Unit = txtUnit1.Text;
            model.Unit2 = txtUnit2.Text;
            model.Other1 = txtOther.Text;
            model.Remark = txtRemark.Text;
            model.NodeId = drpNode.SelectedValue.AsInt();
            model.LayerId = drpLayer.SelectedValue.AsInt();
            model.ScanTypeId = drpScanType.SelectedValue.AsInt();
            model.DeviceId = drpDevice.SelectedValue.AsInt();
            model.Other2 = txtOther2.Text;
            model.Other3 = "";
            model.Other4 = "";
            model.Perpiod = 0;
            //model.MapX = txtX.Text;
            //model.MapY = txtY.Text;
            model.para1 = txtpara1.Text.AsDouble();
            model.para2 = txtpara2.Text.AsDouble();
            model.para3 = txtpara3.Text.AsDouble();
            model.para4 = txtpara4.Text.AsDouble();
            model.para5 = txtpara5.Text.AsDouble();
            model.para6 = txtpara6.Text.AsDouble();
            model.NewID = drpScanPoint.SelectedValue.AsInt();
            //var device = db.FD_Device.SingleOrDefault(u => u.ID == model.DeviceId);
            //if (device != null)
            //{
            //    if (device.DeviceProtocol == (int)DeviceProtocolEnum.JIASU)
            //    {
            //        model.Perpiod = (1000 / device.Perpiod).AsInt(1);
            //    }
            //}

            model.MapX = txtX.Text;
            model.MapY = txtY.Text;
            model.ProjectId = 0;

            if (Id == 0)
            {
                db.FD_ScanPoint.InsertOnSubmit(model);
            }

            db.SubmitChanges();

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


            //db.SubmitChanges();
            //model.DictID = model.ID;
            db.SubmitChanges();




            MessageBox.Invoke("parent.Refresh('{0}')", "btnRefresh");

        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_ScanPoint; }
        }
    }
}