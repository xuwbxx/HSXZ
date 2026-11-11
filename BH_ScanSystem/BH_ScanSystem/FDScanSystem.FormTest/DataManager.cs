using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace FDScanSystem.FormTest
{
    public class DataManager
    {
        static Queue<QueueData> Datas = new Queue<QueueData>();

        //static Timer _timer = new Timer(new TimerCallback(Callback));


        public static void Start()
        {
            //_timer.Change(60 * 1000 * 10, 60 * 1000);
        }


        public static void Stop()
        {
            //_timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public static void Callback(object s)
        {
            try
            {
                Queue<QueueData> tempData = null;
                lock (Datas)
                {
                    tempData = new Queue<QueueData>(Datas);
                    Datas.Clear();
                }

                if (tempData.Count <= 0) return;
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", DateTime.Now.ToString("yyMMddHH"));
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);


                Stack<string> fileStack = new Stack<string>();

                int pageCount = (int)Math.Ceiling(tempData.Count / (65536 * 1.0));

                for (int p = 0; p < pageCount; p++)
                {
                    string fileName = Path.Combine(dir, DateTime.Now.ToString("yyMMddHHmmssff-") + p + ".xls");

                    HSSFWorkbook workBook = new HSSFWorkbook();

                    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                    dsi.Company = "中交上海三航科学研究院有限公司";
                    workBook.DocumentSummaryInformation = dsi;


                    ISheet sheet1 = workBook.CreateSheet("原始数据");

                    var row = sheet1.CreateRow(0);
                    row.CreateCell(0).SetCellValue("采集时间");
                    for (int i = 1; i <= 16; i++)
                    {
                        row.CreateCell(i).SetCellValue("通道" + i);
                    }

                    int rowIndex = 1;
                    while (tempData.Count > 0)
                    {
                        var temp = tempData.Dequeue();
                        row = sheet1.CreateRow(rowIndex);
                        row.CreateCell(0).SetCellValue(temp.Time.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        for (int j = 1; j < temp.Data.Length + 1; j++)
                        {
                            row.CreateCell(j).SetCellValue(temp.Data[j - 1].ToString("F4"));
                        }
                        //  fsw.WriteLine(string.Format("{0} {1}", temp.Time.ToString("yyyy-MM-dd HH:mm:ss.ffff"), string.Join(" ", temp.Data)));
                        rowIndex++;
                        if (rowIndex > 65535)
                        {
                            break;
                        }
                    }

                    using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                    {
                        workBook.Write(fs);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //_timer.Change(60 * 1000 * 10, 60 * 1000);
            }
        }

        public static void Append(QueueData data)
        {
            lock (Datas)
            {
                Datas.Enqueue(data);
            }
        }
    }


    public class QueueData
    {
        public DateTime Time { get; set; }

        public double[] Data { get; set; }
    }
}
