using FDScanSystem.DAO;
using FDScanSystem.Listen.Analysis.Custome;
using FDScanSystem.Protocol;
using System;
using MathWorks.MATLAB.NET.Arrays;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJFramework10.Extension;

namespace FDScanSystem.Listen.Analysis.DataReceiver.Custome
{
    public class AlertDataInfoDataReceiver : CusomerDataReceiverBase
    {
        protected override Logic.DataReceiveArgs SubHandle(Device.DeviceItem device, Logic.DataReceiveArgs args, string msg)
        {
            try
            {

                ClientMsgEntity msgEntity = ClientMsgEntity.DeSerialize(msg);
                if (msgEntity.Command.Equals(ClientMsgCode.ClientToServer_DataAlertInfo))
                {
                    args.IsHandle = true;
                    int deviceId = msgEntity.Paramters["DEVICEID"].AsInt();
                    long startTime = msgEntity.Paramters["STARTTIME"].AsInt64();
                    int pointId = msgEntity.Paramters["POINTID"].AsInt();
                    double alertVal = msgEntity.Paramters["ALERTVAL"].AsDouble();
                    string content = msgEntity.Paramters["Content"].ToString();
                    int Perpiod = msgEntity.Paramters["Interval"].AsInt(1);
                    int TDJZPL = msgEntity.Paramters["TDJZPL"].AsInt(1);
                    int ZDJZPL = msgEntity.Paramters["ZDJZPL"].AsInt(1);

                    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Callback), msgEntity);
                    //   FileManager.SaveToFile(deviceId, startTime, content);


                    ClientMsgEntity response = new ClientMsgEntity();
                    response.Command = ClientMsgCode.Response_OK;
                    response.Identity = msgEntity.Identity;
                    args.ResponseMsg = response;
                    AppContext.Success(string.Format("收到一条数据信息 : {0} - {1} - {2} - {3}", msgEntity.Command, msgEntity.Identity, deviceId, DateTime.FromFileTime(startTime).ToString("yyyy-MM-dd HH:mm:ss.fff")));
                }
            }
            catch
            {

            }
            return args;
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

        private string getFileName(string filename,string Number)
        {
            string[] f = filename.Split('.');
            if (f.Length > 1)
            {
                return f[0] + "-" + Number + "." + f[1];
            }
            return "";
        }

        protected void Callback(object state)
        {
            try
            {
                ClientMsgEntity msgEntity = state as ClientMsgEntity;
                int deviceId = msgEntity.Paramters["DEVICEID"].AsInt();
                long startTime = msgEntity.Paramters["STARTTIME"].AsInt64();
                int pointId = msgEntity.Paramters["POINTID"].AsInt();
                double alertVal = msgEntity.Paramters["ALERTVAL"].AsDouble();
                int Perpiod = msgEntity.Paramters["Interval"].AsInt(1);
                int TDJZPL = msgEntity.Paramters["TDJZPL"].AsInt(1);
                int ZDJZPL = msgEntity.Paramters["ZDJZPL"].AsInt(1);
               
                string content = msgEntity.Paramters["Content"].ToString();
                string filelocation = FileManager.SaveToFile(deviceId, startTime, content);

                var db = DataContextFactory.Create();
                FD_DataValue3 valueEntity = new FD_DataValue3();
                valueEntity.CreateDate = DateTime.Now;
                valueEntity.PointId = pointId;
                valueEntity.ScanValue = alertVal;
                valueEntity.DeviceId = deviceId;
                valueEntity.ScanEvent = "阀值-原始";
                valueEntity.ScanLocation = filelocation;
                valueEntity.ScanTime = DateTime.FromFileTime(startTime);
                db.FD_DataValue3.InsertOnSubmit(valueEntity);
                db.SubmitChanges();


                try
                {
                    //string[] array = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    //double[,] inputArray = null;// new double[array.Length, array2.Length];
                    //if (array != null)
                    //{
                    //    string[] array2 = array[1].Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);


                    //    inputArray = new double[array.Length, array2.Length];
                    //    int i = 0;
                    //    int Count = 0;
                    //    foreach (string l1 in array)
                    //    {
                    //        if (Count == 0)
                    //        {
                    //            //i++;
                    //            Count++;
                    //            continue;
                    //        }
                    //        int j = 0;
                    //        array2 = l1.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                    //        foreach (string l2 in array2)
                    //        {
                    //            inputArray[i, j] = Convert.ToDouble(l2);
                    //            j++;
                    //        }
                    //        i++;
                    //    }
                    //}

                    lpfilter.lowpass lowpass = new lpfilter.lowpass();
                    //var temp = ((MWNumericArray)inputArray);
                    string filename = getFileName(filelocation, "1");
                    string filename2 = getFileName(filelocation, "2");
                    string filename3 = getFileName(filelocation, "3");
                    //    Console.WriteLine(temp.ToString());
                    var result = lowpass.lp(filelocation, (MWArray)(1000 / Perpiod).AsInt(), TDJZPL, ZDJZPL, filename, filename2, filename3);

                }
                catch (Exception ex)
                {
                    AppContext.Log("文件操作失败：" + ex.ToString());
                }


            }
            catch
            {
            }
        }
    }
}
