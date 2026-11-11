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
    public partial class StaticDataChart : AdminPageBase
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


                this.cbxProtocol.DataSource = db.FD_Dict.Where(u => u.DictType == DictTypeEnum.SCANTYPE && !u.DictValue.Contains("加速度")).ToList();
                this.cbxProtocol.ValueField = "DictID";
                this.cbxProtocol.TextField = "DictValue";
                this.cbxProtocol.ValueType = typeof(int);
                this.cbxProtocol.DataBind();
                this.cbxProtocol.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("-选择监测类型-", -1));

                this.cbxNode.SelectedIndex = 0;
                this.cbxProtocol.SelectedIndex = 0;
                txtBeginDate.Value = DateTime.Now.Date;
                txtEndDate.Value = DateTime.Now.Date;

            }
            DrawLine();
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


        //protected void DataCreated()
        //{
        //    var db = DataContextFactory.Create();

        //    IQueryable<TempQueryEntity> querySource = (from a in db.FD_DataValue
        //                                               join b in db.FD_ScanPoint
        //                                               on a.PointId equals b.ID
        //                                               select new TempQueryEntity
        //                                               {
        //                                                   DataValue = a,
        //                                                   Point = b

        //                                               });


        //    if (this.cbxNode.SelectedIndex > 0)
        //    {
        //        int nodeId = cbxNode.Value.AsInt();

        //        querySource = querySource.Where(u => u.Point.NodeId == nodeId);
        //    }

        //    if (this.cbxProtocol.SelectedIndex > 0)
        //    {
        //        int protocolId = this.cbxProtocol.Value.AsInt();

        //        querySource = querySource.Where(u => u.Point.ScanTypeId == protocolId);
        //    }

        //    if (this.cbxPoint.SelectedIndex > 0)
        //    {
        //        int pointId = cbxPoint.Value.AsInt();

        //        querySource = querySource.Where(u => u.Point.ID == pointId);
        //    }

        //    //if (!string.IsNullOrEmpty(this.txtName.Text))
        //    //{
        //    //    querySource = querySource.Where(u => u.Point.PointName.Contains(this.txtName.Text));
        //    //}

        //    if (!string.IsNullOrEmpty(this.txtBeginDate.Text))
        //    {
        //        querySource = querySource.Where(u => u.DataValue.CreateDate >= this.txtBeginDate.Date);
        //    }

        //    if (!string.IsNullOrEmpty(this.txtEndDate.Text))
        //    {
        //        DateTime dd = this.txtEndDate.Date.AddDays(1);
        //        querySource = querySource.Where(u => u.DataValue.CreateDate < dd);
        //    }



        //    var Scan = querySource.OrderByDescending(u => u.DataValue.CreateDate).Select(u => new ScanValue
        //    {
        //        ID = u.DataValue.ID,
        //        PointId = u.DataValue.PointId,
        //        NodeName = db.FD_ScanNode.Where(y => y.ID == u.Point.NodeId).Select(y => y.NodeName).FirstOrDefault(),
        //        DeviceName = db.FD_Device.Where(y => y.ID == u.Point.DeviceId).Select(y => y.DeviceName).FirstOrDefault(),
        //        ProtocolName = u.Point.PointName,
        //        DataValue1 = u.DataValue.ScanValue,
        //        DataValue2 = u.DataValue.ScanValue2,
        //        CreateDate = u.DataValue.CreateDate
        //    }).OrderBy(u => u.PointId).ThenByDescending(u => u.CreateDate).ToList();

        //    FD_ScanPoint temp_point = db.FD_ScanPoint.SingleOrDefault(d => d.ID == cbxPoint.Value.AsInt());
        //    string PointName = ""; string MapdataName = "";
        //    //if (temp_point != null && temp_data.Count > 0)
        //    //{
        //    PointName = temp_point.PointName;
        //    MapdataName = temp_point.Channel2;

        //    //string PointName = DB.MK_Point.SingleOrDefault(d => d.ID == cmb_Point.Value.AsInt()).PointName;
        //    //string MapdataName = DB.MK_Point.SingleOrDefault(d => d.ID == cmb_Point.Value.AsInt()).MapDataName2;

        //    SetChart(temp_data, beginDate, endDate, PointName);
        //    if (MapdataName != null && MapdataName != "")
        //    {
        //        chart2.Visible = true;
        //        SetChart2(temp_data, beginDate, endDate, PointName);
        //    }


        //    //this.aspGrid.DataBind();

        //}


        private DateTime getBeginDate(DateTime date)
        {
            string temp = date.ToString("yyyy-MM-dd") + " " + "00:00:00";
            return temp.AsDatetime(DateTime.Now);
        }

        private DateTime getEndDate(DateTime date)
        {
            string temp = date.ToString("yyyy-MM-dd") + " " + "23:59:59";
            return temp.AsDatetime(DateTime.Now);
        }

        private void DrawLine()
        {

            var DB = DataContextFactory.Create();
            DateTime beginDate = getBeginDate(txtBeginDate.Value.AsDatetime(DateTime.Now));
            DateTime endDate = getEndDate(txtEndDate.Value.AsDatetime(DateTime.Now));
            if (cbxPoint.Value.AsInt() > 0)
            {
                List<FD_DataValue> temp_data = DB.FD_DataValue.Where(d => d.PointId == cbxPoint.Value.AsInt() && d.CreateDate >= beginDate && d.CreateDate <= endDate).OrderBy(d => d.CreateDate).ToList();
                FD_ScanPoint temp_point = DB.FD_ScanPoint.SingleOrDefault(d => d.ID == cbxPoint.Value.AsInt());
                string PointName = ""; string MapdataName = "";
                //if (temp_point != null && temp_data.Count > 0)
                //{
                PointName = temp_point.PointName;
                MapdataName = temp_point.Channel2;

                //string PointName = DB.MK_Point.SingleOrDefault(d => d.ID == cmb_Point.Value.AsInt()).PointName;
                //string MapdataName = DB.MK_Point.SingleOrDefault(d => d.ID == cmb_Point.Value.AsInt()).MapDataName2;

                SetChart(temp_data, beginDate, endDate, PointName);
                if (MapdataName != null && MapdataName != "")
                {
                    chart2.Visible = true;
                    SetChart2(temp_data, beginDate, endDate, PointName);
                }
                //  }
            }
            //else if (cmb_ScanPosition.Value.AsInt() > 0)            //当测点未选择时，显示所有数据
            //{
            //    List<MK_Point> temp_pot = DB.MK_Point.Where(d => d.PositionId == cmb_ScanPosition.Value.AsInt()).ToList();
            //    for (int i = 0; i < temp_pot.Count; i++)
            //    {
            //        List<MK_DataValue> temp_data = DB.MK_DataValue.Where(d => d.PointId == temp_pot[i].ID && d.CreateDate >= beginDate && d.CreateDate <= endDate).OrderBy(d => d.CreateDate).ToList();
            //        //if (temp_data.Count > 0)
            //        //{
            //        //SetChart(temp_data, beginDate, endDate);
            //        //SetChart2(temp_data, beginDate, endDate);
            //        //}
            //        if (temp_data.Count > 0)
            //        {
            //            SetChart(temp_data, beginDate, endDate, temp_pot[i].PointName);
            //            if (temp_pot[i].MapDataName2 != null && temp_pot[i].MapDataName2 != "")
            //            {
            //                chart2.Visible = true;
            //                SetChart2(temp_data, beginDate, endDate, temp_pot[i].PointName);
            //            }
            //        }
            //    }
            //}
        }







        private void SetChart(List<FD_DataValue> temp_data, DateTime beginDate, DateTime endDate, string pt_name)
        {

            if (temp_data.Count > 0)
            {

                string seriesName = "测点：" + pt_name + " 监测值1";                    //图表下方的title内容
                chart.Series.Add(seriesName);
                chart.Series[seriesName].ChartType = SeriesChartType.Spline;
                chart.Series[seriesName].BorderWidth = 1;
                chart.Series[seriesName].MarkerStyle = MarkerStyle.Circle;
                chart.Series[seriesName].MarkerSize = 3;

                chart.Series[seriesName].BorderWidth = 2;
                if (beginDate.ToString("yyyy-MM-dd").Equals(endDate.ToString("yyyy-MM-dd")))
                {
                    chart.Series[seriesName].XValueType = ChartValueType.Time;
                }
                else
                {
                    chart.Series[seriesName].XValueType = ChartValueType.DateTime;
                }
                chart.Series[seriesName].YValueType = ChartValueType.String;
                chart.Series[seriesName].ShadowOffset = 1;
                chart.Series[seriesName].MarkerStep = 1;
                chart.Series[seriesName].YValuesPerPoint = 10;

                for (int i = 0; i < temp_data.Count; i++)
                {

                    chart.Series[seriesName].Points.AddXY(temp_data[i].CreateDate, temp_data[i].ScanValue3.AsDouble().ToString("F2"));
                }
                int count = chart.Series[seriesName].Points.Count;
                for (int j = 0; j < chart.Series[seriesName].Points.Count; j++)
                {
                    chart.Series[seriesName].Points[j].ToolTip = "测点：" + pt_name + " " + temp_data[j].ScanValue3.AsDouble().ToString("F2") + " " + temp_data[j].CreateDate.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd HH:mm");
                }
            }

        }

        private void SetChart2(List<FD_DataValue> temp_data, DateTime beginDate, DateTime endDate, string pt_name)
        {

            var DB = DataContextFactory.Create();
            if (temp_data.Count > 0)
            {
                FD_ScanPoint DB_pt = DB.FD_ScanPoint.SingleOrDefault(d => d.ID == temp_data[0].PointId);
                if (DB_pt != null)
                {
                    string seriesName = "测点：" + DB_pt.PointName + " 监测值2";                    //图表下方的title内容

                    chart2.Series.Add(seriesName);
                    chart2.Series[seriesName].ChartType = SeriesChartType.Spline;
                    chart2.Series[seriesName].BorderWidth = 1;
                    chart2.Series[seriesName].MarkerStyle = MarkerStyle.Circle;
                    chart2.Series[seriesName].MarkerSize = 3;
                    chart2.Series[seriesName].BorderWidth = 2;

                    if (beginDate.ToString("yyyy-MM-dd").Equals(endDate.ToString("yyyy-MM-dd")))
                    {
                        chart2.Series[seriesName].XValueType = ChartValueType.Time;
                    }
                    else
                    {
                        chart2.Series[seriesName].XValueType = ChartValueType.DateTime;
                    }
                    chart2.Series[seriesName].YValueType = ChartValueType.Double;
                    chart2.Series[seriesName].ShadowOffset = 1;
                    chart2.Series[seriesName].MarkerStep = 1;
                    chart2.Series[seriesName].YValuesPerPoint = 10;

                    //for (int i = 0; i < temp_data.Count; i++)
                    //{
                    //    if (chart2.Visible == false)
                    //    {
                    //        if (temp_data[i].ScanValue2 != null && temp_data[i].ScanValue2 != 0)
                    //        {
                    //            chart2.Visible = true;
                    //        }
                    //    }
                    //    chart2.Series[seriesName].Points.AddXY(temp_data[i].CreateDate, temp_data[i].ScanValue2.AsDouble().ToString("F2"));
                    //}

                    for (int i = 0; i < temp_data.Count; i++)
                    {
                        //if (chart2.Visible == false)
                        //{
                        //    if (DB_pt.MapDataName2 == "" || DB_pt.MapDataName2 == null)
                        //    {
                        //        chart2.Visible = false;
                        //    }
                        //    else
                        //    {
                        //        chart2.Visible = true;
                        //    }
                        //}
                        chart2.Series[seriesName].Points.AddXY(temp_data[i].CreateDate, temp_data[i].ScanValue4.AsDouble().ToString("F2"));
                    }

                    int count = chart2.Series[seriesName].Points.Count;

                    for (int j = 0; j < count; j++)
                    {
                        chart2.Series[seriesName].Points[j].ToolTip = "测点：" + DB_pt.PointName + " " + temp_data[j].ScanValue4.AsDouble().ToString("F2") + " " + temp_data[j].CreateDate.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd HH:mm");
                    }
                }
            }
        }


        public override string PermissionCode
        {
            get { return PermissionDict.F_DataChart; }
        }
    }

    public class ScanValue
    {
        public int ID { set; get; }

        public int PointId { set; get; }

        public string NodeName { set; get; }

        public string DeviceName { set; get; }

        public string ProtocolName { set; get; }

        public double DataValue1 { set; get; }

        public double DataValue2 { set; get; }

        public DateTime CreateDate { set; get; }

    }

}