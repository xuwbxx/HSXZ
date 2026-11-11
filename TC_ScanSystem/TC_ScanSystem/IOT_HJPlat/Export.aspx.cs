using DataEntity;
using DataEntity.Services;
using Model.Common;
using Model.Echarts;
using Model.HJPlat.Request;
using Model.HJPlat.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ZJFramework10.Extension;
using Tool;
using System.Configuration;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

namespace IOT_HJPlat
{
    public partial class Export : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string source=  @"g:\Dtw\Inputdata.xls";
            if (File.Exists(source))
            {
                File.Delete(source);
            }

            using (FileStream fs = new FileStream(source, FileMode.Create, FileAccess.ReadWrite))
            {
                var begin = Request.QueryString["Begin"].Trim().AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 00:00:00";
                var end = Request.QueryString["End"].Trim().AsDatetime(DateTime.Now).ToString("yyyy-MM-dd") + " 23:59:59";
                var service = DataContextFactory.Create();
                HSSFWorkbook xssfworkbook = new HSSFWorkbook();
                ISheet sheet = xssfworkbook.CreateSheet("Sheet1");

                if (!string.IsNullOrWhiteSpace(Request.QueryString["PointID"]) && !Request.QueryString["PointID"].Trim().Equals("0"))
                {
                    var pointId = Request.QueryString["PointID"].Split(',').ToList();
                    var list = new List<MK_DataValue>();
                    var list1 = new List<MK_DataValue>();

                    if (pointId.Count > 1)
                    {
                        list = service.MK_DataValue.Where(u => u.PointId == pointId[0].AsInt(0) && u.CreateDate >= begin.AsDatetime(DateTime.Now) && u.CreateDate <= end.AsDatetime(DateTime.Now)).ToList();
                        list1 = service.MK_DataValue.Where(u => u.PointId == pointId[1].AsInt(0) && u.CreateDate >= begin.AsDatetime(DateTime.Now) && u.CreateDate <= end.AsDatetime(DateTime.Now)).ToList();
                    }

                    //数据
                    for (int i = 0; i < list.Count; i++)
                    {
                        IRow row1 = sheet.CreateRow(i);
                        ICell cell = row1.CreateCell(0);
                        cell.SetCellValue(list[i].CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        ICell cell2 = row1.CreateCell(1);
                        cell2.SetCellValue(list[i].ScanValue5.AsDouble(0));
                        //for (int j = 0; j < dt.Columns.Count; j++)
                        //{
                        //    ICell cell = row1.CreateCell(j);
                        //    cell.SetCellValue(dt.Rows[i][j].ToString());
                        //}
                    }
                    ISheet sheet1 = xssfworkbook.CreateSheet("Sheet2");
                    for (int i = 0; i < list1.Count; i++)
                    {
                        IRow row1 = sheet1.CreateRow(i);
                        ICell cell = row1.CreateCell(0);
                        cell.SetCellValue(list1[i].CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        ICell cell2 = row1.CreateCell(1);
                        cell2.SetCellValue(list1[i].ScanValue5.AsDouble(0));
                        //for (int j = 0; j < dt.Columns.Count; j++)
                        //{
                        //    ICell cell = row1.CreateCell(j);
                        //    cell.SetCellValue(dt.Rows[i][j].ToString());
                        //}
                    }
                }
                xssfworkbook.Write(fs);
            }

            RegisterStartupScript("key1", "<script>window. window.location.href = \"Dtw://\"</script>");

            // Process file = new Process();
            // file.StartInfo.FileName = "gtptools.exe";
            //file.StartInfo.WorkingDirectory = "g:";
            // // file.StartInfo.Arguments = "200805";
            // file.StartInfo.CreateNoWindow = false;
            // file.StartInfo.Verb = "open";
            // file.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            // file.StartInfo.RedirectStandardOutput = false;
            // file.StartInfo.UseShellExecute = true;
            // file.Start();



        }
    }
}