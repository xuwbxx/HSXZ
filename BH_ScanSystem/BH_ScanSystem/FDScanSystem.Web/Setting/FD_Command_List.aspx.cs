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
    public partial class FD_Command_List : AdminPageBase
    {

        private DBDataContext DB = DataContextFactory.Create();
        protected void Page_Load(object sender, EventArgs e)
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

            if (!IsPostBack)
            {
                BindCom();
                //if (Session["Cmd_com"].AsInt() > 0)
                //{
                //    cmb_Com.Value = Session["Cmd_com"].ToString();
                //}

                BindDevice(cmb_Com.Value.AsInt());
                //if (Session["Cmd_dev"].AsInt() > 0)
                //{
                //    cmb_Device.Value = Session["Cmd_dev"].ToString();
                //}
                //ControlByDevice();
                FD_Device temp = DB.FD_Device.SingleOrDefault(d => d.ID == cmb_Device.Value.AsInt());
                //if (temp != null && temp.DeviceProtocol == 0)
                //{
                //    td2.Visible = true;
                //    btn_HuaHeng.Visible = false;
                //}
                //else
                //{
                //    td2.Visible = false;
                //    btn_HuaHeng.Visible = true;
                //}
                btn_HuaHeng.Visible = false;
                BindGrid();
                aspxBeginDay.Value = DateTime.Now;
                aspxEndDay.Value = DateTime.Now;
            }
        }

        private string FormZero(int value)
        {
            if (value < 0)
            {
                return "0" + value.ToString();
            }
            else
            {
                return value.ToString();
            }
        }



        protected void cmb_Com_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Session["Cmd_com"] = cmb_Com.SelectedItem.Value.AsInt();
            BindDevice(cmb_Com.Value.AsInt());
            //ControlByDevice();
            //if (cmb_Device.Items.Count > 0)
            //{
            //Session["Cmd_dev"] = cmb_Device.SelectedItem.Value.AsInt();
            //}
        }


        protected void btn_Choose_Click(object sender, EventArgs e)
        {
            ControlByDevice();
           
        }

        protected void btn_HuaHeng_Click(object sender, EventArgs e)
        {
            Excute("10");
            ControlByDevice();
        }

        protected void btn_DateRead_Click(object sender, EventArgs e)
        {
            string BeginDate = (aspxBeginDay.Value.ToString()).AsDatetime(DateTime.Now).ToString("yyMMdd") +
                aspxBeginTime.DateTime.ToString("HHmm");
            string EndDate = (aspxEndDay.Value.ToString()).AsDatetime(DateTime.Now).ToString("yyMMdd") +
                aspxEndTime.DateTime.ToString("HHmm");
            Excute("MCU32 {0} READ" + " " + BeginDate + " " + EndDate);
        }

        protected void btn_Send_Click(object sender, EventArgs e)
        {
            Excute("MCU32 {0} Begin");
        }

        protected void btn_Read_Click(object sender, EventArgs e)
        {
            Excute("MCU32 {0} Read");
        }

        protected void btn_Clear_Click(object sender, EventArgs e)
        {
            Excute("MCU32 {0} Clear");
        }

        protected void btn_SendCommand_Click(object sender, EventArgs e)
        {
            Excute(txt_Command.InnerText.Trim());
        }

        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            string[] del = new string[] { "ID" };
            List<Object> delist = ASPxGridView1.GetSelectedFieldValues(del);
            if (delist.Count > 0)
            {
                for (int i = 0; i < delist.Count; i++)
                {
                    FD_Command item = DB.FD_Command.SingleOrDefault(d => d.ID == delist[i].AsInt());
                    if (item != null)
                    {
                        DB.FD_Command.DeleteOnSubmit(item);
                    }
                }
                DB.SubmitChanges();
                BindGrid();
            }
            ControlByDevice();
        }

        private void BindCom()
        {
            
                cmb_Com.DataSource = DB.FD_CommunicationDevice.ToList();
                cmb_Com.TextField = "DeviceName";
                cmb_Com.ValueField = "ID";
                cmb_Com.DataBind();
                cmb_Com.SelectedIndex = 0;
          
                cmb_Com.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("未选择", 0));

        }

        private void BindDevice(int ComId)
        {
            if (ComId > 0 && DB.FD_Device.Where(d => d.CommunicationDeviceId == ComId).ToList().Count > 0)
            {
                cmb_Device.DataSource = DB.FD_Device.Where(d => d.CommunicationDeviceId == ComId).ToList();
                cmb_Device.TextField = "DeviceName";
                cmb_Device.ValueField = "ID";
                cmb_Device.DataBind();
                cmb_Device.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0; i < cmb_Device.Items.Count; i++)
                {
                    cmb_Device.Items.RemoveAt(i);
                }
                cmb_Device.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("未选择", 0));
                cmb_Device.SelectedIndex = 0;
            }
        }

        private void BindGrid()
        {
            if (cmb_Com.Value.AsInt() > 0)
            {
                List<CommandView> temp_view = (from e1 in DB.FD_Command
                                               join e2 in DB.FD_Device on e1.DeviceId equals e2.ID
                                               where (e2.ID == cmb_Device.Value.AsInt() && e1.IsSystem == 0)
                                               join e3 in DB.FD_CommunicationDevice
                                               on e1.CommunicationDeviceId equals e3.ID
                                               where (e3.ID == cmb_Com.Value.AsInt())
                                               orderby e1.CreateDate descending
                                               select new CommandView
                                               {
                                                   ID = e1.ID,
                                                   Command = e1.Command,
                                                   DeviceName = e2.DeviceName,
                                                   CommunicationName = e3.DeviceName,
                                                   Result = e1.Result,
                                                   CreateDate = e1.CreateDate
                                               }).ToList();
                ASPxGridView1.DataSource = temp_view;
                ASPxGridView1.DataBind();
            }
        }

        private void ControlByDevice()
        {
            if (cmb_Device.Value.AsInt() > 0)            //cmb_Device的选项不为空
            {
                FD_Device temp = DB.FD_Device.SingleOrDefault(d => d.ID == cmb_Device.Value.AsInt());
                if (temp.DeviceProtocol == 0)          //所选择的Device的握手协议为"MCU"
                {
                    //td1.Visible = false;
                   // td2.Visible = true;
                    btn_HuaHeng.Visible = false;
                }
                else                                   //所选择的Device的握手协议为"HuaHeng"
                {
                    //td1.Visible = true;
                  //  td2.Visible = false;
                    btn_HuaHeng.Visible = true;
                }
                BindGrid();
            }
            else                                        //cmb_Deviece的选项为空
            {
                //td1.Visible = false;
               // td2.Visible = false;
                btn_HuaHeng.Visible = true;
            }

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
            temp_cmd.DeviceIden = DB.FD_Device.SingleOrDefault(d => d.ID == temp_cmd.DeviceId).DeviceIdentity;


            List<FD_ScanPoint> temp_point = new List<FD_ScanPoint>();
            temp_point = DB.FD_ScanPoint.Where(d => d.DeviceId == cmb_Device.Value.AsInt()).ToList();
            if (temp_point.Count > 0)
            {
                temp_cmd.LastPointId = temp_point[0].ID;
            }

            DB.FD_Command.InsertOnSubmit(temp_cmd);
            DB.SubmitChanges();
            BindGrid();
        }


        public class CommandView
        {
            public int ID { get; set; }
            public string Command { get; set; }
            public string DeviceName { get; set; }
            public string CommunicationName { get; set; }
            public string Result { get; set; }
            public DateTime CreateDate { get; set; }

        }

        public override string PermissionCode
        {
            get { return PermissionDict.F_Command; }
        }
    }
}