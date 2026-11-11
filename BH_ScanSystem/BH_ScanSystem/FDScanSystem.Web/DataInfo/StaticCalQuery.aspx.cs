using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FD_ScanSystem.WebModel;
using FD_ScanSystem.WebModel.Enums;
using FDScanSystem.DAO;
using OfficeShop.Model;
using OfficeShop.WebModel;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.DataInfo
{
    public partial class StaticCalQuery : AdminPageBase
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
                txtBeginDate.Value = DateTime.Now.Date;
                txtEndDate.Value = DateTime.Now.Date;

                this.cbxProtocol.DataSource = db.FD_Dict.Where(u => u.DictType == DictTypeEnum.SCANTYPE && !u.DictValue.Contains("加速度")).ToList();
                this.cbxProtocol.ValueField = "DictID";
                this.cbxProtocol.TextField = "DictValue";
                this.cbxProtocol.ValueType = typeof(int);
                this.cbxProtocol.DataBind();
                this.cbxProtocol.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("-选择监测类型-", -1));

                this.cbxNode.SelectedIndex = 0;
                this.cbxProtocol.SelectedIndex = 0;
            }
            DataCreated();
        }

   

        protected void Type_Select(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();
            this.cbxPoint.DataSource = db.FD_ScanPoint.Where(u => u.NodeId == cbxNode.Value.AsInt() && u.ScanTypeId == cbxProtocol.Value.AsInt()).OrderBy(u => u.PointName).ToList();
            this.cbxPoint.ValueField = "ID";
            this.cbxPoint.TextField = "PointName";
            this.cbxPoint.ValueType = typeof(int);
            this.cbxPoint.DataBind();
            this.cbxPoint.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("-选择测点-", -1));
            this.cbxPoint.SelectedIndex = 0;
        }



        protected void Node_Select(object sender, EventArgs e)
        {
            var db = DataContextFactory.Create();
            this.cbxPoint.DataSource = db.FD_ScanPoint.Where(u => u.NodeId == cbxNode.Value.AsInt() && u.ScanTypeId == cbxProtocol.Value.AsInt()).OrderBy(u => u.PointName).ToList();
            this.cbxPoint.ValueField = "ID";
            this.cbxPoint.TextField = "PointName";
            this.cbxPoint.ValueType = typeof(int);
            this.cbxPoint.DataBind();
            this.cbxPoint.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("-选择测点-", -1));
            this.cbxPoint.SelectedIndex = 0;
        }


        protected void DataCreated()
        {
            var db = DataContextFactory.Create();

            IQueryable<TempQueryEntity> querySource = (from a in db.FD_DataValue
                                                       join b in db.FD_ScanPoint
                                                       on a.PointId equals b.ID
                                                       select new TempQueryEntity
                                                       {
                                                           DataValue = a,
                                                           Point = b

                                                       });


            int rem = 0;
            if (this.cbxPoint.SelectedIndex > 0)
            {
                int pointId = cbxPoint.Value.AsInt();

                var point = db.FD_ScanPoint.SingleOrDefault(u => u.ID == pointId);
                if (point != null)
                {
                    if (point.NewID != 0)
                    {
                        pointId = point.NewID ?? 0;
                        rem = 1;
                    }
                }

                querySource = querySource.Where(u => u.Point.ID == pointId);
            }

            if (this.cbxNode.SelectedIndex > 0)
            {
                if (rem == 0)
                {
                    int nodeId = cbxNode.Value.AsInt();

                    querySource = querySource.Where(u => u.Point.NodeId == nodeId);
                }
            }

            if (this.cbxProtocol.SelectedIndex > 0)
            {
                if (rem == 0)
                {
                    int protocolId = this.cbxProtocol.Value.AsInt();

                    querySource = querySource.Where(u => u.Point.ScanTypeId == protocolId);
                }
            }

            //if (this.cbxPoint.SelectedIndex > 0)
            //{
            //    int pointId = cbxPoint.Value.AsInt();

            //    var point = db.FD_ScanPoint.SingleOrDefault(u => u.ID == pointId);
            //    if (point != null)
            //    {
            //        if (point.NewID != 0)
            //        {
            //            pointId = point.NewID ?? 0;
            //        }
            //    }

            //    querySource = querySource.Where(u => u.Point.ID == pointId);
            //}

            //if (!string.IsNullOrEmpty(this.txtName.Text))
            //{
            //    querySource = querySource.Where(u => u.Point.PointName.Contains(this.txtName.Text));
            //}

            if (!string.IsNullOrEmpty(this.txtBeginDate.Text))
            {
                querySource = querySource.Where(u => u.DataValue.CreateDate >= this.txtBeginDate.Date);
            }

            if (!string.IsNullOrEmpty(this.txtEndDate.Text))
            {
                DateTime dd = this.txtEndDate.Date.AddDays(1);
                querySource = querySource.Where(u => u.DataValue.CreateDate < dd);
            }


            if (this.cbxPoint.SelectedIndex > 0)
            {

                this.aspGrid.DataSource = querySource.OrderByDescending(u => u.DataValue.CreateDate).Select(u => new
                {
                    u.DataValue.ID,
                    u.DataValue.PointId,
                    NodeName = db.FD_ScanNode.Where(y => y.ID == u.Point.NodeId).Select(y => y.NodeName).FirstOrDefault(),
                    DeviceName = db.FD_Device.Where(y => y.ID == u.Point.DeviceId).Select(y => y.DeviceName).FirstOrDefault(),
                    // ProtocolName = u.Point.PointName,
                    ProtocolName = db.FD_ScanPoint.Where(y => y.ID == cbxPoint.Value.AsInt()).Select(y => y.PointName).FirstOrDefault(),
                    DataValue1 = u.DataValue.ScanValue,
                    DataValue2 = u.DataValue.ScanValue2,
                    u.DataValue.ScanValue5,
                    u.DataValue.ScanValue6,
                    u.DataValue.CreateDate
                }).OrderBy(u => u.PointId).ThenByDescending(u => u.CreateDate).ToList();

            }
            else
            {
                this.aspGrid.DataSource = querySource.OrderByDescending(u => u.DataValue.CreateDate).Select(u => new
                {
                    u.DataValue.ID,
                    u.DataValue.PointId,
                    NodeName = db.FD_ScanNode.Where(y => y.ID == u.Point.NodeId).Select(y => y.NodeName).FirstOrDefault(),
                    DeviceName = db.FD_Device.Where(y => y.ID == u.Point.DeviceId).Select(y => y.DeviceName).FirstOrDefault(),
                    ProtocolName = u.Point.PointName,
                    //ProtocolName = db.FD_ScanPoint.Where(y => y.ID == cbxPoint.Value.AsInt()).Select(y => y.PointName).FirstOrDefault(),
                    DataValue1 = u.DataValue.ScanValue,
                    DataValue2 = u.DataValue.ScanValue2,
                    u.DataValue.ScanValue5,
                    u.DataValue.ScanValue6,
                    u.DataValue.CreateDate
                }).OrderBy(u => u.PointId).ThenByDescending(u => u.CreateDate).ToList();
            }
            this.aspGrid.DataBind();
        }

        public override string PermissionCode
        {
            get { return PermissionDict.DataInfo_Static_CalQuery; }
        }

        protected void btnXlsxExport1_Click(object sender, EventArgs e)
        {
            gridExport.WriteXlsToResponse();
        }

        protected void btnRefresh_CLick(object sender, EventArgs e)
        {
 
        }

        internal class TempQueryEntity
        {
            internal FD_DataValue DataValue { get; set; }

            internal FD_ScanPoint Point { get; set; }
        }
    }
}