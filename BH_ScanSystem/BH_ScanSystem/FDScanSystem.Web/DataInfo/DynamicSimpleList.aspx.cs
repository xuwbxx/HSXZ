using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using FD_ScanSystem.WebModel;
using FD_ScanSystem.WebModel.Enums;
using FDScanSystem.DAO;
using OfficeShop.Model;
using OfficeShop.WebModel;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.DataInfo
{
    public partial class DynamicSimpleList : AdminPageBase
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

                this.cbxProtocol.DataSource = db.FD_Dict.Where(u => u.DictType == DictTypeEnum.SCANTYPE && u.DictValue.Contains("加速度")).ToList();
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
            string DBName = "FD_DynamicValue2" + txtBeginDate.Date.ToString("yyyyMM");

            var Exist = db.ExecuteQuery<FD_Dynamic>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
            if (Exist.ID == 0)
            {
                return;
            }
            //IQueryable<TempQueryEntity> querySource = (from a in db.FD_DynamicValue2
            //                                           join b in db.FD_ScanPoint
            //                                           on a.PointId equals b.ID
            //                                           select new TempQueryEntity
            //                                           {
            //                                               Dynamic = a,
            //                                               Point = b

            //                                           });
            DateTime dd = this.txtEndDate.Date.AddDays(1);
            string sql = string.Format("select a.ID,PointId,a.DeviceId,a.CreateDate,MAXValue,YXValue,NodeId,ScanTypeId,b.PointName from {0} a inner join FD_ScanPoint b on a.PointId=b.ID where a.CreateDate>'{1}' and a.CreateDate<'{2}'", DBName, txtBeginDate.Date, dd);
            IEnumerable<FD_Dynamic> querySource = db.ExecuteQuery<FD_Dynamic>(sql);


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

                querySource = querySource.Where(u => u.PointId == pointId);
            }

            if (this.cbxNode.SelectedIndex > 0)
            {
                if (rem == 0)
                {
                    int nodeId = cbxNode.Value.AsInt();

                    querySource = querySource.Where(u => u.NodeId == nodeId);
                }
            }

            if (this.cbxProtocol.SelectedIndex > 0)
            {
                if (rem == 0)
                {
                    int protocolId = this.cbxProtocol.Value.AsInt();

                    querySource = querySource.Where(u => u.ScanTypeId == protocolId);
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

            //if (!string.IsNullOrEmpty(this.txtBeginDate.Text))
            //{
            //    querySource = querySource.Where(u => u.Dynamic.CreateDate >= this.txtBeginDate.Date);
            //}

            //if (!string.IsNullOrEmpty(this.txtEndDate.Text))
            //{
            //    DateTime dd = this.txtEndDate.Date.AddDays(1);
            //    querySource = querySource.Where(u => u.Dynamic.CreateDate < dd);
            //}


            if (this.cbxPoint.SelectedIndex > 0)
            {

                this.aspGrid.DataSource = querySource.OrderByDescending(u => u.CreateDate).Select(u => new
                {
                    u.ID,
                    u.PointId,
                    NodeName = db.FD_ScanNode.Where(y => y.ID == u.NodeId).Select(y => y.NodeName).FirstOrDefault(),
                    DeviceName = db.FD_Device.Where(y => y.ID == u.DeviceId).Select(y => y.DeviceName).FirstOrDefault(),
                    // ProtocolName = u.Point.PointName,
                    PointName = u.PointName,
                    MAXValue = u.MAXValue,
                    YXValue = u.YXValue,
                    u.CreateDate
                }).OrderBy(u => u.PointId).ThenByDescending(u => u.CreateDate).ToList();

            }
            else
            {
                this.aspGrid.DataSource = querySource.OrderByDescending(u => u.CreateDate).Select(u => new
                {
                    u.ID,
                    u.PointId,
                    NodeName = db.FD_ScanNode.Where(y => y.ID == u.NodeId).Select(y => y.NodeName).FirstOrDefault(),
                    DeviceName = db.FD_Device.Where(y => y.ID == u.DeviceId).Select(y => y.DeviceName).FirstOrDefault(),
                    PointName = u.PointName,
                    //PointName = db.FD_ScanPoint.Where(y => y.ID == cbxPoint.Value.AsInt()).Select(y => y.PointName).FirstOrDefault(),
                    MAXValue = u.MAXValue,
                    YXValue = u.YXValue,
                    u.CreateDate
                }).OrderBy(u => u.PointId).ThenByDescending(u => u.CreateDate).ToList();
            }
            this.aspGrid.DataBind();
        }

        public override string PermissionCode
        {
            get { return PermissionDict.DataInfo_Dynamic_List; }
        }

        protected void btnXlsxExport1_Click(object sender, EventArgs e)
        {
            gridExport.WriteXlsToResponse();
        }

        protected void btnRefresh_CLick(object sender, EventArgs e)
        {

        }

        public class FD_Dynamic
        {
            public int ID { set; get; }
            public int PointId { set; get; }
            public int DeviceId { set; get; }
            public string PointName { set; get; }
            public string DeviceName { set; get; }
            public double MAXValue { set; get; }
            public double YXValue { set; get; }
            public DateTime CreateDate { set; get; }
            public int NodeId { set; get; }
            public int ScanTypeId { set; get; }
            public string NodeName { set; get; }
         
        }
    }
}