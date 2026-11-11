using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZJFramework10.Extension;
namespace FDScanSystem.Protocol.ScanData
{
    public class ScanDataResolver
    {
        public static void ToFile(int deviceId, Dictionary<long, double[]> data, string fileName)
        {
            if (File.Exists(fileName))
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter fsw = new StreamWriter(fs, Encoding.UTF8))
                    {

                        fsw.WriteLine(string.Join("#", deviceId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                        foreach (var item in data.Keys)
                        {
                            double[] datas = data[item];
                            fsw.WriteLine(string.Format("{0}#{1}", item, string.Join("#", datas)));
                        }
                    }
                }
            }
        }

        public static void ToFile(int deviceId, int pointId, double alertValue, Dictionary<long, double> data, string fileName)
        {
            if (File.Exists(fileName))
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter fsw = new StreamWriter(fs, Encoding.UTF8))
                    {

                        fsw.WriteLine(string.Join("#", deviceId, pointId, alertValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                        foreach (var item in data.Keys)
                        {
                            fsw.WriteLine(string.Format("{0}#{1}", item, data[item]));
                        }
                    }
                }
            }

        }


        public static Dictionary<DateTime, double> ToData(string fileName, out int deviceId, out int pointId, out double alertValue, out DateTime dt)
        {
            deviceId = 0;
            pointId = 0;
            alertValue = 0.0;
            dt = DateTime.Now;
            using (FileStream ms = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    string headerInfo = sr.ReadLine();
                    if (!string.IsNullOrEmpty(headerInfo))
                    {
                        string[] headValue = headerInfo.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                        if (headValue.Length > 3)
                        {
                            deviceId = headValue[0].AsInt();
                            pointId = headValue[1].AsInt();
                            alertValue = headValue[2].AsDouble();
                            dt = headValue[3].AsDatetime(DateTime.Now);
                        }


                        Dictionary<DateTime, double> result = new Dictionary<DateTime, double>();
                        while (sr.Peek() != -1)
                        {
                            string t = sr.ReadLine();
                            if (!string.IsNullOrEmpty(t))
                            {
                                string[] tVal = t.Split(new char[] { '#' });
                                DateTime time = DateTime.FromFileTime(tVal[0].AsInt64());
                                double d = tVal[1].AsDouble();
                                result.Add(time, d);
                            }
                        }
                        return result;
                    }
                }
            }



            return new Dictionary<DateTime, double>();
        }

        public static Dictionary<DateTime, double[]> ToData(string fileName, out int deviceId)
        {
            deviceId = 0;


            using (FileStream ms = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    string headerInfo = sr.ReadLine();
                    if (!string.IsNullOrEmpty(headerInfo))
                    {
                        string[] headValue = headerInfo.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                        deviceId = headValue[0].AsInt();

                        Dictionary<DateTime, double[]> result = new Dictionary<DateTime, double[]>();
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
                                    d[i - 1] = tVal[i].AsDouble();
                                }
                                result.Add(time, d);
                            }
                        }
                        return result;
                    }
                }
            }



            return new Dictionary<DateTime, double[]>();
        }




        public static Dictionary<int, double[]> ToData3(string fileName, out int deviceId)
        {
            deviceId = 0;


            using (FileStream ms = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    //string headerInfo = sr.ReadLine();
                    //if (!string.IsNullOrEmpty(headerInfo))
                    //{
                    //    string[] headValue = headerInfo.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                    //    deviceId = headValue[0].AsInt();

                    Dictionary<int, double[]> result = new Dictionary<int, double[]>();
                    int count = 0;
                    while (sr.Peek() != -1)
                    {
                        string t = sr.ReadLine();
                        if (!string.IsNullOrEmpty(t))
                        {
                            string[] tVal = t.Split(new char[] { '#' });
                            // DateTime time = DateTime.FromFileTime(tVal[0].AsInt64());
                            double[] d = new double[tVal.Length];
                            for (int i = 0; i < tVal.Length; i++)
                            {
                                d[i] = tVal[i].AsDouble();
                            }
                            result.Add(count, d);
                            count++;
                        }
                    }
                    return result;
                    // }
                }
            }



            //return new Dictionary<int, double[]>();
        }


        public static double[] ToData(string fileName, out int deviceId, int Number)
        {
            deviceId = 0;


            using (FileStream ms = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(ms))
                {

                    string headerInfo = sr.ReadLine();
                    if (!string.IsNullOrEmpty(headerInfo))
                    {
                        //string[] headValue = headerInfo.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                        //deviceId = headValue[0].AsInt();

                        //Dictionary<DateTime, double[]> result = new Dictionary<DateTime, double[]>();
                        while (sr.Peek() != -1)
                        {
                            //  string t = sr.ReadLine();
                            //if (!string.IsNullOrEmpty(t))
                            //{
                            string[] t = headerInfo.Split(new char[] { '#' });
                            double[] d = new double[t.Length];
                            for (int i = 0; i < t.Length; i++)
                            {
                                d[i] = t[i].AsDouble();
                            }
                            return d;
                        }

                    }
                }
            }



            return new double[] { };
        }




        public static Dictionary<DateTime, double[]> ToData1(string fileName, out int deviceId)
        {
            deviceId = 0;


            using (FileStream ms = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    //string headerInfo = sr.ReadLine();
                    //if (!string.IsNullOrEmpty(headerInfo))
                    //{
                    //    string[] headValue = headerInfo.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                    //    deviceId = headValue[0].AsInt();

                    Dictionary<DateTime, double[]> result = new Dictionary<DateTime, double[]>();
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
                                d[i - 1] = tVal[i].AsDouble();
                            }
                            result.Add(time, d);
                        }
                    }
                    return result;
                    //}
                }
            }



            return new Dictionary<DateTime, double[]>();
        }



    }
}
