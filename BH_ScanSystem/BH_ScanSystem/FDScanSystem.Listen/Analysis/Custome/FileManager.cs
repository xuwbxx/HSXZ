using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FDScanSystem.DAO;
using FDScanSystem.Protocol.ScanData;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using ZJFramework10.Extension;

namespace FDScanSystem.Listen.Analysis.Custome
{
    public class FileManager
    {
        public static string SaveToFile(int deviceId, long time, string content)
        {
            string fileName = FileNameCreator.GetInstance().CreateFile(deviceId, time);
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    writer.WriteLine(content);
                }
            }
            return fileName;
        }


        public static void SaveToXls(string FileName)
        {
           // Dictionary<DateTime, double[]> result = new Dictionary<DateTime, double[]>();
            string filexls = getFileName(FileName);
            // string ltr = "";
            if (File.Exists(FileName))
            {
                using (FileStream ms = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        HSSFWorkbook workBook = new HSSFWorkbook();
                        DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                        dsi.Company = "中交上海三航科学研究院有限公司";
                        workBook.DocumentSummaryInformation = dsi;
                        ISheet sheet1 = workBook.CreateSheet("原始数据");
                        int i = 0;
                        string headerInfo = sr.ReadLine();
                        if (!string.IsNullOrEmpty(headerInfo))
                        {
                            while (sr.Peek() != -1)
                            {
                                string t = sr.ReadLine();
                                if (!string.IsNullOrEmpty(t))
                                {
                                    string[] tVal = t.Split(new char[] { '#' });
                                    DateTime time = DateTime.FromFileTime(tVal[0].AsInt64());
                                    var row = sheet1.CreateRow(i);
                                    row.CreateCell(0).SetCellValue(time.ToString());
                                    for (int j = 1; j < tVal.Length; j++)
                                    {
                                        row.CreateCell(j).SetCellValue(tVal[j]);
                                    }
                                    i++;
                                }
                            }
                            using (FileStream fs = new FileStream(filexls, FileMode.Create, FileAccess.Write))
                            {
                                workBook.Write(fs);
                            }
                        }

                    }
                }
            }
        }

        private static string getFileName(string filename)
        {
            string[] f = filename.Split('.');
            if (f.Length > 1)
            {
                return f[0] + ".xls";
            }
            return "";
        }

        public static void SaveToFile(string FileName, List<FD_ScanPoint> points)
        {
            int deviceId = 0;
            Dictionary<DateTime, double[]> result = new Dictionary<DateTime, double[]>();
            // string ltr = "";
            if (File.Exists(FileName))
            {

                using (FileStream ms = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        string headerInfo = sr.ReadLine();
                        if (!string.IsNullOrEmpty(headerInfo))
                        {
                            // string[] headValue = headerInfo.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                            string[] headValue = headerInfo.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                            deviceId = headValue[0].AsInt();

                            while (sr.Peek() != -1)
                            {
                                string t = sr.ReadLine();
                                if (!string.IsNullOrEmpty(t))
                                {
                                    string[] tVal = t.Split(new char[] { '#' });
                                    DateTime time = DateTime.FromFileTime(tVal[0].AsInt64());
                                    double[] d = new double[tVal.Length - 1];

                                    for (int i = 1; i < tVal.Length; i++)
                                    {

                                        var point = points.FirstOrDefault(u => u.DeviceId == deviceId && u.Channel1.AsInt() == (i - 1));
                                        if (point != null)
                                        {
                                            double a = tVal[i].AsDouble();
                                            a = a - point.Other1.AsDouble();
                                            if (point.Other2.AsDouble() != 0)
                                            {
                                                a = a / point.Other2.AsDouble();
                                            }
                                            d[i - 1] = a;
                                        }
                                        else
                                        {
                                            d[i - 1] = tVal[i].AsDouble();
                                        }
                                    }
                                    result.Add(time, d);
                                }
                            }
                        }
                    }




                }
                string filename = "";
                string[] f = FileName.Split('.');
                if (f.Length > 1)
                {
                    filename = f[0] + "-" + 4 + "." + f[1];
                }

                using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter fsw = new StreamWriter(fs, Encoding.UTF8))
                    {

                        fsw.WriteLine(string.Join("#", deviceId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                        foreach (var item in result.Keys)
                        {
                            fsw.WriteLine(string.Format("{0}#{1}", item.ToFileTime(), result[item]));
                        }
                    }
                }



            }
        }

    }
}
