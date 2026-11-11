using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeShop.WebModel;
using FD_ScanSystem.WebModel;
using FDScanSystem.DAO;
using OfficeShop.Model;
using ZJFramework10.Extension;
namespace FDScanSystem.Web.DataInfo
{
    public partial class DynamicDataAlertQuery : AdminPageBase
    {
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
                else
                {
                    Response.Redirect("/login.aspx");
                }
            }
            if (!IsPostBack)
            {
                var db = DataContextFactory.Create();
                this.cbxNode.DataSource = db.FD_ScanNode.Where(u => u.NodeParentId == 0).ToList();
                this.cbxNode.ValueField = "ID";
                this.cbxNode.TextField = "NodeName";
                this.cbxNode.ValueType = typeof(int);
                this.cbxNode.DataBind();

                this.cbxNode.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("-选择风机-", -1));



                this.cbxDevice.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("-选择采集设备-", -1));
            }
            DataCreated();
        }


        protected void DataCreated()
        {
            var db = DataContextFactory.Create();

            IQueryable<TempQueryEntity3> querySource = (from a in db.FD_DataValue3
                                                        join b in db.FD_Device
                                                        on a.DeviceId equals b.ID
                                                        join c in db.FD_ScanPoint
                                                        on a.PointId equals c.ID
                                                        select new TempQueryEntity3
                                                        {
                                                            DataValue = a,
                                                            Device = b,
                                                            Point = c
                                                        });


            if (this.cbxNode.SelectedIndex > 0)
            {
                int nodeId = cbxNode.Value.AsInt();

                querySource = querySource.Where(u => u.Device.NodeId == nodeId);
            }

            if (this.cbxDevice.SelectedIndex > 0)
            {
                int protocolId = this.cbxDevice.Value.AsInt();

                querySource = querySource.Where(u => u.Device.ID == protocolId);
            }
            if (!string.IsNullOrEmpty(this.txtBeginDate.Text))
            {
                querySource = querySource.Where(u => u.DataValue.CreateDate >= this.txtBeginDate.Date || u.DataValue.ScanTime >= this.txtBeginDate.Date);
            }

            if (!string.IsNullOrEmpty(this.txtEndDate.Text))
            {
                querySource = querySource.Where(u => u.DataValue.CreateDate < this.txtEndDate.Date.AddDays(1) || u.DataValue.ScanTime < this.txtEndDate.Date.AddDays(1));
            }

            this.aspGrid.DataSource = querySource.OrderByDescending(u => u.DataValue.ScanTime).Select(u => new
            {
                u.DataValue.ID,
                NodeName = db.FD_ScanNode.Where(y => y.ID == u.Device.NodeId).Select(y => y.NodeName).FirstOrDefault(),
                DeviceName = u.Device.DeviceName,
                ScanLocation = u.DataValue.ScanLocation,
                ScanTime = u.DataValue.ScanTime,
                ScanEvent = u.DataValue.ScanEvent,
                u.DataValue.CreateDate,
                DeviceId = u.Device.ID,
                u.DataValue.ScanValue,
                u.Point.PointName
            }).ToList();
            this.aspGrid.DataBind();

        }

        protected void select_Changed(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();
            this.cbxDevice.DataSource = db.FD_Device.Where(u => u.NodeId == cbxNode.Value.AsInt()).ToList();
            this.cbxDevice.ValueField = "ID";
            this.cbxDevice.TextField = "DeviceName";
            this.cbxDevice.DataBind();
            this.cbxDevice.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("-选择采集设备-", -1));
            this.cbxDevice.SelectedIndex = 0;
        }


        public override string PermissionCode
        {
            get { return PermissionDict.DataInfo_Dynamic_Alert; }
        }
    }

    internal class TempQueryEntity3
    {
        internal FD_DataValue3 DataValue { get; set; }

        internal FD_Device Device { get; set; }

        internal FD_ScanPoint Point { get; set; }
    }
}