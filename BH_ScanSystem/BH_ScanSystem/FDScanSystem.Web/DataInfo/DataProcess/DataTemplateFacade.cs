using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FD_ScanSystem.WebModel.Cache;
using System.IO;
using System.Text;
using FDScanSystem.Protocol.ScanData;

namespace FDScanSystem.Web.DataInfo.DataProcess
{
    public class DataTemplateFacade
    {
        public static List<DataViewEntity> CreateDataSource(string fileName, int channel)
        {
            var result = CacheProxy.Get<List<DataViewEntity>>(() =>
            {

                return GetByFileName(fileName);
            }, string.Format("{0}_{1}", Path.GetFileName(fileName), "FILEDATA"));
            if (result != null)
            {
                return result.Where(u => u.Channel == channel).ToList();
            }
            else
            {
                return new List<DataViewEntity>(0);
            }
        }

        public static List<DataViewEntity> CreateDataSource3(string fileName, int channel)
        {
            var result = CacheProxy.Get<List<DataViewEntity>>(() =>
            {

                return GetByFileName3(fileName);
            }, string.Format("{0}_{1}", Path.GetFileName(fileName), "FILEDATA"));
            if (result != null)
            {
                return result.Where(u => u.Channel == channel).ToList();
            }
            else
            {
                return new List<DataViewEntity>(0);
            }
        }

        public static List<DataViewEntity> CreateDataSource(string fileName, int channel, int Number)
        {
            var result = CacheProxy.Get<List<DataViewEntity>>(() =>
            {

                return GetByFileName(fileName, Number);
            }, string.Format("{0}_{1}", Path.GetFileName(fileName), "FILEDATA"));
            if (result != null)
            {
                return result.Where(u => u.Channel == channel).ToList();
            }
            else
            {
                return new List<DataViewEntity>(0);
            }
        }


        public static List<DataViewEntity> CreateDataSource2(string filename, int Channel, int Number)
        {
            var result = CacheProxy.Get<List<DataViewEntity>>(() =>
            {

                return GetByFileName(filename, Number);
            }, string.Format("{0}_{1}", Path.GetFileName(filename), "FILEDATA"));
            if (result != null)
            {
                return result.Where(u => u.Channel == Channel).ToList();
            }
            else
            {
                return new List<DataViewEntity>(0);
            }
        }


        public static List<DataViewEntity> CreateAlertDataSource(string fileName)
        {
            var result = CacheProxy.Get<List<DataViewEntity>>(() =>
            {

                return GetByFileName2(fileName);
            }, string.Format("{0}_{1}", Path.GetFileName(fileName), "FILEDATA"));
            if (result != null)
            {
                return result.ToList();
            }
            else
            {
                return new List<DataViewEntity>(0);
            }
        }


        static List<DataViewEntity> GetByFileName(string fileName)
        {
            List<DataViewEntity> returnSource = new List<DataViewEntity>();
            if (!File.Exists(fileName))
            {
                return new List<DataViewEntity>(0);
            }
            int deviceId = 0;

            Dictionary<DateTime, double[]> data = ScanDataResolver.ToData(fileName, out deviceId);


            foreach (DateTime time in data.Keys)
            {
                // DateTime scanTime = DateTime.FromFileTime(time);
                DataViewEntity dv = null;
                for (int i = 0; i < data[time].Length; i++)
                {
                    dv = new DataViewEntity();
                    dv.Channel = i;
                    dv.ScanData = data[time][i];
                    dv.ScanTime = time;

                    returnSource.Add(dv);
                }
            }
            return returnSource;

        }




        static List<DataViewEntity> GetByFileName3(string fileName)
        {
            List<DataViewEntity> returnSource = new List<DataViewEntity>();
            if (!File.Exists(fileName))
            {
                return new List<DataViewEntity>(0);
            }
            int deviceId = 0;

            Dictionary<int, double[]> data = ScanDataResolver.ToData3(fileName, out deviceId);


            foreach (int count in data.Keys)
            {
                // DateTime scanTime = DateTime.FromFileTime(time);
                DataViewEntity dv = null;
                for (int i = 0; i < data[count].Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        dv = new DataViewEntity();

                        dv.Channel = i / 2;
                        dv.ScanData = data[count][i];
                    }
                    else
                    {
                        dv.ScanData2 = data[count][i];
                        returnSource.Add(dv);
                    }
                    //dv.ScanTime = time; 
                }
            }
            return returnSource;

        }




        static List<DataViewEntity> GetByFileName(string fileName, int number)
        {
            List<DataViewEntity> returnSource = new List<DataViewEntity>();
            if (!File.Exists(fileName))
            {
                return new List<DataViewEntity>(0);
            }
            int deviceId = 0;
            Dictionary<DateTime, double[]> data = new Dictionary<DateTime, double[]>();
            if (number == 0)
            {
                 data = ScanDataResolver.ToData(fileName, out deviceId);
            }
            else
            {
                data = ScanDataResolver.ToData1(fileName, out deviceId);
            }

            foreach (DateTime time in data.Keys)
            {
                // DateTime scanTime = DateTime.FromFileTime(time);
                DataViewEntity dv = null;
                for (int i = 0; i < data[time].Length; i++)
                {
                    dv = new DataViewEntity();
                    dv.Channel = i;
                    dv.ScanData = data[time][i];
                    dv.ScanTime = time;

                    returnSource.Add(dv);
                }
            }
            return returnSource;

        }



        static List<DataViewEntity> GetByFileName2(string fileName, int number, int DeviceId)
        {
            List<DataViewEntity> returnSource = new List<DataViewEntity>();
            if (!File.Exists(fileName))
            {
                return  new List<DataViewEntity>(0);;
            }
            int deviceId = 0;
            double[] data = ScanDataResolver.ToData(fileName, out deviceId, number);
            //if (number == 0)
            //{
            //    data = ScanDataResolver.ToData(fileName, out deviceId);
            //}
            //else
            //{
            //    data = ScanDataResolver.ToData1(fileName, out deviceId);
            //}

            //foreach (DateTime time in data.Keys)
            //{
            // DateTime scanTime = DateTime.FromFileTime(time);
            DataViewEntity dv = null;
            for (int i = 0; i < data.Length; i++)
            {
                //if (DeviceId == i)
                //{
                //    return data[i];
                //}
                dv = new DataViewEntity();
                dv.Channel = i;
                dv.ScanData = data[i];
                //dv.ScanData = data[time][i];
                //dv.ScanTime = time;

                returnSource.Add(dv);
            }
            // }
            return returnSource;

        }





        static List<DataViewEntity> GetByFileName2(string fileName)
        {
            List<DataViewEntity> returnSource = new List<DataViewEntity>();
            if (!File.Exists(fileName))
            {
                return new List<DataViewEntity>(0);
            }
            int deviceId = 0;
            int pointId = 0;
            double alertVal = 0.0;
            DateTime alertTime = DateTime.Now;
            Dictionary<DateTime, double> data = ScanDataResolver.ToData(fileName, out deviceId, out pointId, out alertVal, out alertTime);

            DataViewEntity dv = null;
            foreach (DateTime time in data.Keys)
            {

                dv = new DataViewEntity();
                dv.ScanData = data[time];
                dv.ScanTime = time;

                returnSource.Add(dv);

            }
            return returnSource;

        }

    }


    public class DataViewEntity
    {
        public int Channel { get; set; }

        public DateTime ScanTime { get; set; }

        public double ScanData { get; set; }

        public double ScanData2 { set; get; }
    }
}