using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FD_ScanSystem.WebModel;
using FD_ScanSystem.WebModel.Enums;
using FD_ScanSystem.WebModel.View;
using FDScanSystem.DAO;
using FDScanSystem.Protocol;
using FDScanSystem.Protocol.ScanConfig;
using OfficeShop.Model;
using OfficeShop.WebModel;
using TrainingsApp.Helper;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.Setting
{
    public partial class FD_AutoScanConfig_List : AdminPageBase
    {

        private DBDataContext DB = DataContextFactory.Create();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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

                BindCom();


                BindDevice(cmb_Com.Value.AsInt());

                ControlByDevice();
            }
        }

        protected void cmb_Com_SelectedIndexChanged(object sender, EventArgs e)
        {

            BindDevice(cmb_Com.Value.AsInt());
            //ControlByDevice();

        }

        protected void btn_Choose_Click(object sender, EventArgs e)
        {
            ControlByDevice();

        }


        protected void btn_Time_Click(object sender, EventArgs e)
        {
            if (drpDay.Value != null && drpHour.Value != null && drpMin.Value != null)
            {
                string date = FormZero(drpDay.Value.AsInt());
                string hour = FormZero(drpHour.Value.AsInt());
                string min = FormZero(drpMin.Value.AsInt());

                if (drpDay.Value.AsInt() == 0 && drpHour.Value.AsInt() == 0)
                {
                    if (drpMin.Value.AsInt() < 5)
                    {
                        Page.RegisterStartupScript("", "<script>window.alert('采集频率不能少于5分钟');</script>");
                        return;
                    }
                }

                if (date.Length > 0 && hour.Length > 0 && min.Length > 0)
                {
                    Excute("MCU32 {0} WrSmpInt " + date + hour + min);
                }
                EditDevice(1);
                Page.RegisterStartupScript("", "<script>window.alert('设置成功！');</script>");
            }
            else
            {
                Page.RegisterStartupScript("", "<script>window.alert('有数据未配置');</script>");
            }
        }

        protected void aspxEdit_Click(object sender, EventArgs e)
        {
            if (aspxTimeRead.Value != null)
            {
                Excute("MCU32 {0} WrSmpStd " + aspxTimeRead.DateTime.ToString("HHmm"));
                EditDevice(3);
                Page.RegisterStartupScript("", "<script>window.alert('设置成功！');</script>");
            }
            else
            { Page.RegisterStartupScript("", "<script>window.alert('有数据未配置');</script>"); }

        }

        protected void btn_NowTime_Click(object sender, EventArgs e)
        {
            if (aspxNowDate.Value != null && aspxNowTime.Value != null)
            {
                string NowDate = aspxNowDate.Date.ToString("yy-MM-dd").Replace("-", "/");
                string NowTime = aspxNowTime.DateTime.ToString("HH:mm:ss");
                if (NowDate.Length > 0 && NowTime.Length > 0)
                {
                    Excute("MCU32 {0} SetTime " + NowDate + " " + NowTime);
                }
                EditDevice(2);
                Page.RegisterStartupScript("", "<script>window.alert('设置成功！');</script>");
            }
            else
            {
                Page.RegisterStartupScript("", "<script>window.alert('有数据未配置');</script>");
            }
        }

        private void BindCom()
        {

            cmb_Com.DataSource = DB.FD_CommunicationDevice.ToList();
            cmb_Com.TextField = "DeviceName";
            cmb_Com.ValueField = "ID";
            cmb_Com.DataBind();
            if (cmb_Com.Items.Count > 0)
            {
                cmb_Com.SelectedIndex = 0;
            }

        }

        private void BindDevice(int ComId)
        {
            if (ComId > 0)
            {
                cmb_Device.DataSource = DB.FD_Device.Where(d => d.CommunicationDeviceId == ComId).ToList();
                cmb_Device.TextField = "DeviceName";
                cmb_Device.ValueField = "ID";
                cmb_Device.DataBind();
                if (cmb_Device.Items.Count > 0)
                {
                    cmb_Device.SelectedIndex = 0;
                }
            }
        }

        private void ControlByDevice()
        {
            BindTime(cmb_Device.Value.AsInt());
        }

        private void BindTime(int DeviceId)
        {
            FD_Device temp = DB.FD_Device.SingleOrDefault(d => d.ID == DeviceId);
            if (temp != null)
            {
                if (!string.IsNullOrEmpty(temp.Interval))
                {
                    string[] interval = temp.Interval.Split('-');
                    if (interval[0].Length > 0)
                    {
                        drpDay.Value = interval[0];
                    }
                    if (interval[1].Length > 0)
                    {
                        drpHour.Value = interval[1];
                    }
                    if (interval[2].Length > 0)
                    {
                        drpMin.Value = interval[2];
                    }
                }
                if (temp.DeviceTime != null)
                {
                    string date = temp.DeviceTime.ToString().Split(' ')[0];
                    string time = temp.DeviceTime.ToString().Split(' ')[1];
                    if (date.Length > 0)
                    {
                        aspxNowDate.Value = temp.DeviceTime;
                    }
                    if (time.Length > 0)
                    {
                        aspxNowTime.Value = temp.DeviceTime;
                    }
                }
                if (temp.DeviceTime != null)
                {
                    string BeginTime = temp.BeginTime.ToString().Split(' ')[1];
                    if (BeginTime.Length > 0)
                    {
                        aspxTimeRead.Value = temp.BeginTime;
                    }
                }
            }
        }

        private void EditDevice(int flg)
        {
            FD_Device temp = DB.FD_Device.SingleOrDefault(d => d.ID == cmb_Device.Value.AsInt());
            if (temp != null)
            {
                switch (flg)
                {
                    case 1:
                        string interval = drpDay.Value.ToString() + '-' + drpHour.Value.ToString() + '-' + drpMin.Value.ToString();
                        temp.Interval = interval;
                        break;
                    case 2:
                        if (aspxNowDate.Value != null && aspxNowTime.Value != null)
                        {
                            string date = aspxNowDate.Value.ToString().Split(' ')[0] + ' ' + aspxNowTime.Value.ToString().Split(' ')[1];
                            DateTime devicetime = date.AsDatetime(DateTime.Now);
                            temp.DeviceTime = devicetime;
                        }
                        break;
                    case 3:
                        string begin = temp.BeginTime.ToString().Split(' ')[0] + ' ' + aspxTimeRead.Value.ToString().Split(' ')[1];
                        temp.BeginTime = begin.AsDatetime(DateTime.Now);
                        break;
                }
            }
            DB.SubmitChanges();
        }

        private void Excute(string str)
        {
            FD_Command temp_cmd = new FD_Command();
            temp_cmd.Command = str;
            temp_cmd.DeviceId = cmb_Device.Value.AsInt();
            if (cmb_Com.Value.AsInt() > 0)
            {
                temp_cmd.CommunicationDeviceId = cmb_Com.Value.AsInt();
                temp_cmd.CommunicationIden = DB.FD_CommunicationDevice.SingleOrDefault
                    (d => d.ID == temp_cmd.CommunicationDeviceId).CommunicationCode;
            }
            temp_cmd.Status = 0;
            temp_cmd.CreateDate = DateTime.Now;
            temp_cmd.IsSystem = 0;
            temp_cmd.Result = "";
            if (DB.FD_Device.SingleOrDefault(d => d.ID == temp_cmd.DeviceId) != null)
            {
                temp_cmd.DeviceIden = DB.FD_Device.SingleOrDefault(d => d.ID == temp_cmd.DeviceId).DeviceIdentity;
            }


            List<FD_ScanPoint> temp_point = new List<FD_ScanPoint>();
            temp_point = DB.FD_ScanPoint.Where(d => d.DeviceId == cmb_Device.Value.AsInt()).ToList();
            if (temp_point.Count > 0)
            {
                temp_cmd.LastPointId = temp_point[0].ID;
            }

            DB.FD_Command.InsertOnSubmit(temp_cmd);
            DB.SubmitChanges();
        }

        private static string FormZero(int value)
        {
            if (value < 10)
            {
                return "0" + value.ToString();
            }
            else
            {
                return value.ToString();
            }
        }


        public override string PermissionCode
        {
            get { return PermissionDict.F_AutoConfig; }
        }
    }
}