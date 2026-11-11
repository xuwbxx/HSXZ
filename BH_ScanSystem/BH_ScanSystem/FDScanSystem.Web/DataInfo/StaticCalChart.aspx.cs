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
    public partial class StaticCalChart : AdminPageBase
    {

        public int ScanTypeId { set; get; }
        public int ScanNodeId { set; get; }
        public int PointId { get; set; }

        public string PointList { set; get; }
        public int Id = ZJFramework10.Utility.QueryStringManager.GetValue<int>("pid");
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
                this.txtBeginHour.Items.Add("未选择", "-1");
                for (int i = 0; i < 24; i++)
                {
                    this.txtBeginHour.Items.Add(i.ToString(), i.ToString());
                }
                this.txtBeginHour.SelectedIndex = 0;


                this.txtEndHour.Items.Add("未选择", "-1");
                for (int i = 0; i < 24; i++)
                {
                    this.txtEndHour.Items.Add(i.ToString(), i.ToString());
                }
                this.txtEndHour.SelectedIndex = 0;

                this.cbxNode.SelectedIndex = 1;
                this.cbxProtocol.SelectedIndex = 1;
                txtBeginDate.Value = DateTime.Now.Date;
                txtEndDate.Value = DateTime.Now.Date;

                if (Id > 0)
                {
                    txtPoint.Text = Id.ToString();

                    var point = db.FD_ScanPoint.SingleOrDefault(u => u.ID == Id);
                    if (point != null)
                    {
                        cbxProtocol.Value = point.ScanTypeId;
                        cbxNode.Value = point.NodeId;
                    }

                }
                else
                {
 
                }

            }
            DrawLine();
        }


        private DateTime getBeginDate(DateTime date)
        {
            if (txtBeginHour.Value.AsInt() > -1)
            {
                string temp = date.ToString("yyyy-MM-dd") + " " + txtBeginHour.Value.AsInt() + ":00:00";
                return temp.AsDatetime(DateTime.Now);
            }
            else
            {
                string temp = date.ToString("yyyy-MM-dd") + " " + "00:00:00";
                return temp.AsDatetime(DateTime.Now);
            }
        }

        private DateTime getEndDate(DateTime date)
        {
            if (txtEndHour.Value.AsInt() > -1)
            {
                string temp = date.ToString("yyyy-MM-dd") + " " + txtEndHour.Value.AsInt() + ":59:59";
                return temp.AsDatetime(DateTime.Now);
            }
            else
            {
                string temp = date.ToString("yyyy-MM-dd") + " " + "23:59:59";
                return temp.AsDatetime(DateTime.Now);
            }
        }

        private void DrawLine()
        {

            var DB = DataContextFactory.Create();
            DateTime beginDate = getBeginDate(txtBeginDate.Value.AsDatetime(DateTime.Now));
            DateTime endDate = getEndDate(txtEndDate.Value.AsDatetime(DateTime.Now));
            ltTitle.Visible = false;
            ltTitle2.Visible = false;
            if (!string.IsNullOrEmpty(hfPoint.Value))
            {
                string[] str = hfPoint.Value.Split(',');
                if (str.Length > 0)
                {
                    List<int> ss = new List<int>();
                    foreach (var s in str)
                    {
                        if (ss.Contains(s.AsInt()))
                        {
                            continue;
                        }
                        FD_ScanPoint temp_point = DB.FD_ScanPoint.SingleOrDefault(d => d.ID == s.AsInt());

                        // FD_ScanPoint temp_point = DB.FD_ScanPoint.SingleOrDefault(d => d.ID == s.AsInt());
                        string PointName = ""; string MapdataName = "";
                        //if (temp_point != null && temp_data.Count > 0)
                        //{
                        if (temp_point != null)
                        {
                            int PointId = temp_point.ID;
                            ; ;
                            if (temp_point.NewID != 0)
                            {
                                PointId = temp_point.NewID ?? 0;
                            }

                            List<FD_DataValue> temp_data = DB.FD_DataValue.Where(d => d.PointId == PointId && d.CreateDate >= beginDate && d.CreateDate <= endDate).OrderBy(d => d.CreateDate).ToList();
                            PointName = temp_point.PointName;
                            MapdataName = temp_point.Channel2;

                            //string PointName = DB.MK_Point.SingleOrDefault(d => d.ID == cmb_Point.Value.AsInt()).PointName;
                            //string MapdataName = DB.MK_Point.SingleOrDefault(d => d.ID == cmb_Point.Value.AsInt()).MapDataName2;
                            if (temp_data.Count > 0)
                            {
                                ltTitle.Visible = true;

                                string Name = cbxProtocol.SelectedItem.Text;
                                ltTitle.Text = "应力";
                                if (Name.Equals("无应力计") || Name.Equals("混凝土应变计"))
                                {
                                    ltTitle.Text = "应变";
                                }

                                ltTitle.Text = ltTitle.Text + "(" + temp_point.Unit + ")";
                                if (temp_point.Channel1.Contains("Dig"))
                                {
                                    ltTitle.Text = "相对角度（°)";
                                }
                                SetChart(temp_data, beginDate, endDate, PointName);
                                if (!string.IsNullOrEmpty(MapdataName))
                                {
                                    ltTitle2.Text = "温度(℃)";
                                    chart2.Visible = true;
                                    ltTitle2.Visible = true;
                                    SetChart2(temp_data, beginDate, endDate, PointName);
                                }
                            }
                        }
                        ss.Add(s.AsInt());
                    }
                }
            }


            //if (cbxPoint.Value.AsInt() > 0)
            //{
            //List<FD_DataValue> temp_data = DB.FD_DataValue.Where(d => d.PointId == cbxPoint.Value.AsInt() && d.CreateDate >= beginDate && d.CreateDate <= endDate).OrderBy(d => d.CreateDate).ToList();
            //FD_ScanPoint temp_point = DB.FD_ScanPoint.SingleOrDefault(d => d.ID == cbxPoint.Value.AsInt());
            //string PointName = ""; string MapdataName = "";
            ////if (temp_point != null && temp_data.Count > 0)
            ////{
            //PointName = temp_point.PointName;
            //MapdataName = temp_point.Channel2;

            ////string PointName = DB.MK_Point.SingleOrDefault(d => d.ID == cmb_Point.Value.AsInt()).PointName;
            ////string MapdataName = DB.MK_Point.SingleOrDefault(d => d.ID == cmb_Point.Value.AsInt()).MapDataName2;

            //SetChart(temp_data, beginDate, endDate, PointName);
            //if (MapdataName != null && MapdataName != "")
            //{
            //    chart2.Visible = true;
            //    SetChart2(temp_data, beginDate, endDate, PointName);
            //}
            //  }
            //}
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




        protected void cbxNode_Select(object sender, EventArgs e)
        {
            txtPoint.Text = "";
        }

        protected void cbxProtocol_select(object sender, EventArgs e)
        {
            txtPoint.Text = "";
        }


        private void SetChart(List<FD_DataValue> temp_data, DateTime beginDate, DateTime endDate, string pt_name)
        {

            if (temp_data.Count > 0)
            {
                List<FD_DataValue> temp = new List<FD_DataValue>();
                string seriesName = "测点：" + pt_name;                    //图表下方的title内容
                chart.Series.Add(seriesName);
                chart.Series[seriesName].ChartType = SeriesChartType.Line;
                chart.Series[seriesName].BorderWidth = 1;
                chart.Series[seriesName].MarkerStyle = MarkerStyle.Circle;
                chart.Series[seriesName].MarkerSize = 3;

               // chart.Series[seriesName].BorderWidth = 1;
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
                    //if (txtBeginHour.Value.AsInt() > -1)
                    //{
                    //    if (temp_data[i].CreateDate.Hour < txtBeginHour.Value.AsInt())
                    //    {
                    //        continue;
                    //    }
                    //}
                    //if (txtEndHour.Value.AsInt() > -1)
                    //{
                    //    if (temp_data[i].CreateDate.Hour > txtEndHour.Value.AsInt())
                    //    {
                    //        continue;
                    //    }
                    //}
                    FD_DataValue t = new FD_DataValue();
                    t.ScanValue5 = temp_data[i].ScanValue5;
                    t.ScanValue6 = temp_data[i].ScanValue6;
                    t.CreateDate = temp_data[i].CreateDate;
                    temp.Add(t);
                    chart.Series[seriesName].Points.AddXY(temp_data[i].CreateDate, temp_data[i].ScanValue5.AsDouble().ToString("F8"));
                }
                int count = chart.Series[seriesName].Points.Count;
                for (int j = 0; j < count; j++)
                {
                    //if (txtBeginHour.Value.AsInt() > -1)
                    //{
                    //    if (temp_data[j].CreateDate.Hour < txtBeginHour.Value.AsInt())
                    //    {
                    //        continue;
                    //    }
                    //}

                    //if (txtEndHour.Value.AsInt() > -1)
                    //{
                    //    if (temp_data[j].CreateDate.Hour > txtEndHour.Value.AsInt())
                    //    {
                    //        continue;
                    //    }
                    //}
                    chart.Series[seriesName].Points[j].ToolTip = "测点：" + pt_name + " " + temp[j].ScanValue5.AsDouble().ToString("F8") + " " + temp[j].CreateDate.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd HH:mm");

                }
            }

        }



        private void SetChart2(List<FD_DataValue> temp_data, DateTime beginDate, DateTime endDate, string pt_name)
        {

            var DB = DataContextFactory.Create();
            if (temp_data.Count > 0)
            {
                List<FD_DataValue> temp = new List<FD_DataValue>();
                FD_ScanPoint DB_pt = DB.FD_ScanPoint.SingleOrDefault(d => d.ID == temp_data[0].PointId);
                if (DB_pt != null)
                {
                    string seriesName = "测点：" + DB_pt.PointName;                    //图表下方的title内容

                    chart2.Series.Add(seriesName);
                    chart2.Series[seriesName].ChartType = SeriesChartType.Line;
                    chart2.Series[seriesName].BorderWidth = 1;
                    chart2.Series[seriesName].MarkerStyle = MarkerStyle.Circle;
                    chart2.Series[seriesName].MarkerSize = 3;
                    //chart2.Series[seriesName].BorderWidth = 2;


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

                        if (txtBeginHour.Value.AsInt() > -1)
                        {
                            if (temp_data[i].CreateDate.Hour < txtBeginHour.Value.AsInt())
                            {
                                continue;
                            }
                        }

                        if (txtEndHour.Value.AsInt() > -1)
                        {
                            if (temp_data[i].CreateDate.Hour > txtEndHour.Value.AsInt())
                            {
                                continue;
                            }
                        }
                        FD_DataValue t = new FD_DataValue();
                        t.ScanValue5 = temp_data[i].ScanValue5;
                        t.ScanValue6 = temp_data[i].ScanValue6;
                        t.CreateDate = temp_data[i].CreateDate;
                        temp.Add(t);
                        chart2.Series[seriesName].Points.AddXY(temp_data[i].CreateDate, temp_data[i].ScanValue6.AsDouble().ToString("F8"));
                    }

                    int count = chart2.Series[seriesName].Points.Count;

                    for (int j = 0; j < count; j++)
                    {
                        //if (txtBeginHour.Value.AsInt() > -1)
                        //{
                        //    if (temp_data[j].CreateDate.Hour < txtBeginHour.Value.AsInt())
                        //    {
                        //        continue;
                        //    }
                        //}

                        //if (txtEndHour.Value.AsInt() > -1)
                        //{
                        //    if (temp_data[j].CreateDate.Hour > txtEndHour.Value.AsInt())
                        //    {
                        //        continue;
                        //    }
                        //}

                        chart2.Series[seriesName].Points[j].ToolTip = "测点：" + DB_pt.PointName + " " + temp[j].ScanValue6.AsDouble().ToString("F8") + " " + temp[j].CreateDate.AsDatetime(DateTime.Now).ToString("yyyy-MM-dd HH:mm");
                    }
                }
            }
        }

        public override string PermissionCode
        {
            get { return PermissionDict.DataInfo_StaticCalChart; }
        }
    }
}