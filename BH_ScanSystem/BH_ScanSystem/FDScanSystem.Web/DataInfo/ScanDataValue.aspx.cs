using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FDScanSystem.DAO;
using FD_ScanSystem.WebModel;
using OfficeShop.WebModel;
using FDScanSystem.Web.DataInfo.DataProcess;
using ZJFramework10.Extension;

namespace FDScanSystem.Web.DataInfo
{
    public partial class ScanDataValue : AdminPageBase
    {

        protected int DataValueId
        {
            get
            {
                return ZJFramework10.Utility.QueryStringManager.GetValue<int>("data");
            }
        }

        protected int PointId
        {
            get
            {
                return ZJFramework10.Utility.QueryStringManager.GetValue<int>("point");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Chart1.Series["Series1"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.FastLine;
            FillData();
        }

        public override string PermissionCode
        {
            get { return PermissionDict.DataInfo_Static_Query; }
        }

        private void FillData()
        {

            Chart1.Series[0].XValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Time;
            Chart1.Series[0].YValueType = System.Web.UI.DataVisualization.Charting.ChartValueType.Double;
          


            var db = DataContextFactory.Create();

            var pointInfo = db.FD_ScanPoint.FirstOrDefault(u => u.ID == PointId);
            if (pointInfo != null)
            {

                var dataValue = db.FD_DataValue2.FirstOrDefault(u => u.ID == DataValueId);
                if (dataValue != null)
                {
                    string MaxD = "";
                    string MinD = "";
                    //string MaxD1 = "";
                    //string MinD1 = "";
                    //string MaxD2 = "";
                    //string MinD2 = "";
                     List<DataViewEntity> dataList = DataTemplateFacade.CreateDataSource(dataValue.ScanLocation, pointInfo.Channel1.AsInt());
                    //List<DataViewEntity> dataList1 = DataTemplateFacade.CreateDataSource(getFileName(dataValue.ScanLocation, "1"), pointInfo.Channel1.AsInt(), 1);
                    //List<DataViewEntity> dataList2 = DataTemplateFacade.CreateDataSource3(getFileName(dataValue.ScanLocation, "3"), pointInfo.Channel1.AsInt()).OrderBy(u => u.ScanData2).ToList();


                    if (dataList.Count > 0)
                    {
                        MaxD = dataList.Max(u => u.ScanTime).ToString("HH:mm:ss.ffff");
                        MinD = dataList.Min(u => u.ScanTime).ToString("HH:mm:ss.ffff");
                    }
                    foreach (var item in dataList)
                    {
                        Chart1.Series[0].Points.AddXY(item.ScanTime, item.ScanData.ToString("F4"));
                    }
                    if (Chart1.Titles.Count > 0)
                    {
                        Chart1.Titles[0].Text = string.Format("测点名称： {0} 的时域曲线， 采集数据周期： {1} ~ {2}", pointInfo.PointName, MinD, MaxD);
                    }

                    //if (dataList1.Count > 0)
                    //{
                    //    MaxD1 = dataList1.Max(u => u.ScanTime).ToString("HH:mm:ss.ffff");
                    //    MinD1 = dataList1.Min(u => u.ScanTime).ToString("HH:mm:ss.ffff");
                    //}

                   



                    //if (dataList2.Count > 0)
                    //{
                    //    MaxD2 = dataList2.Max(u => u.ScanData2).ToString("F8");
                    //    MinD2 = dataList2.Min(u => u.ScanData2).ToString("F8");
                    //}

                }
            }



        }

        private string getFileName(string filename)
        {
            string[] f = filename.Split('.');
            if (f.Length > 1)
            {
                return f[0] + "-1." + f[1];
            }
            return "";
        }

        private string getFileName(string filename, string Number)
        {
            string[] f = filename.Split('.');
            if (f.Length > 1)
            {
                return f[0] + "-" + Number + "." + f[1];
            }
            return "";
        }
    }
}