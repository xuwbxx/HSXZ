using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FDScanSystem.DAO;
using FDScanSystem.DAO.Scan;
using FDScanSystem.Helper;
using FDScanSystem.Server;
using FDScanSystem.Server.Analysis.DataReceiver.MCU;
using FDScanSystem.Server.Device;
using Interactive.Utility;
using ZJFramework10.Extension;


namespace FDScanSystem.Server.Job.DataReceiver
{
    public class DetailDataReceiver : MCUDataReceiverBase
    {

        public DataReceiveArgs Handle(int code, string msg)
        {
            DataReceiveArgs args = new DataReceiveArgs();
            return args;
        }

        protected override DataReceiveArgs SubHandle(DeviceItem device, DataReceiveArgs args, string msg)
        {

            if (msg.Trim().Length > 20)
            {
                args.IsHandle = true;

                if (msg.ToLowerInvariant().Contains("sn="))
                {
                    getString(msg, device.Key);
                }
                else if (device.Protocol == 3)
                {
                    getCOMWIN(msg, device.Key);
                }
                else
                {
                    getValue(msg, device.Key);
                }

                AppContext.Log("成功处理了Detail信息 " + msg);
                // LogWriter.Info("成功处理了Detail信息 " + msg);
                //Insert to DB
            }
            return args;
        }

        public void getCOMWIN(string str, string connectionKey)
        {
            var cmd = Singleton<CommandQueueManager>.Instance.GetCurrent(connectionKey);
            if (cmd == null) return;
            var points = ServerDAL.ListPoint(cmd.DeviceId);
            {
                var db = DataContextFactory.Create();
                var device = db.MK_Device.FirstOrDefault(u => u.ID == cmd.DeviceId);
                if (device != null)
                {
                    string[] ss = str.Split(' ');
                    if (ss[0] != device.DeviceIden)
                    {
                        AppContext.Log("设备没有找到");
                        return;
                    }
                    int count = 1;
                    for (int i = 3; i < 35; i = i + 2)
                    {
                        double value2 = 0;
                        double value = 0;
                        var point = db.MK_Point.FirstOrDefault(u => u.DeviceId == cmd.DeviceId && u.MapDataName == count.AsStr());
                        if (point != null)
                        {
                            string temp = ss[i] + ss[i + 1];
                            if (device.CRC.Contains("00 64"))
                                value = Convert.ToInt32(temp, 16) / 10;
                            else
                                value = Convert.ToInt32(temp, 16) / 1000;
                            if (!string.IsNullOrEmpty(point.MapDataName2))
                            {
                                string temp2 = ss[i + 32] + ss[i + 1 + 32];
                                value2 = Convert.ToInt32(temp2, 16) / 10;
                            }
                            MK_DataValue data = new MK_DataValue();
                            data.CreateDate = DateTime.Now;
                            data.PointId = point.ID;
                            data.ScanValue = value;
                            data.ScanValue2 = value2;
                            data.ScanValue3 = value;
                            data.ScanValue4 = value2;
                            data.ScanValue5 = value;
                            data.ScanValue6 = value2;
                            data.OldId = 0;
                            data.Status = 0;

                            //var position = db.MK_Position.FirstOrDefault(u => u.ID == point.PositionId);
                            //if (position != null)
                            //{
                            //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == position.LayerId);
                            //    if (layer != null && (layer.LayerType!=2 && layer.LayerType!=1))
                            //    {
                            //        if (point.MaxNum.AsDouble(0) != 0 || point.MinNum.AsDouble(0) != 0)
                            //        {
                            //            if (data.ScanValue5.AsDouble(0) > point.MaxNum || data.ScanValue5.AsDouble(0) < point.MinNum)
                            //            {

                            //                if (data.ScanValue5 > point.MaxNum)
                            //                {
                            //                    if ((data.ScanValue5 - point.MaxNum) / point.MaxNum < 0.2)
                            //                    {
                            //                        layer.LayerType = 3;
                            //                    }
                            //                    else
                            //                    {
                            //                        layer.LayerType = 4;
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    if ((point.MinNum - data.ScanValue5) / data.ScanValue5 < 0.2)
                            //                    {
                            //                        layer.LayerType = 3;
                            //                    }
                            //                    else
                            //                    {
                            //                        layer.LayerType = 4;
                            //                    }
                            //                }
                            //                db.SubmitChanges();
                            //                var error = new MK_ErrorLog();
                            //                error.CreateDate = DateTime.Now;
                            //                error.ErrorInfo = string.Format("{0}数据异常", layer.LayerName);
                            //                db.MK_ErrorLog.InsertOnSubmit(error);
                            //                db.SubmitChanges();
                            //                data.Status = 1;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            data.Status = 0;
                            //        }


                            //        if (point.MaxNum2.AsDouble(0) != 0 || point.MinNum2.AsDouble(0) != 0)
                            //        {
                            //            if (data.ScanValue6.AsDouble(0) > point.MaxNum2 || data.ScanValue6.AsDouble(0) < point.MinNum2)
                            //            {

                            //                if (data.ScanValue6 > point.MaxNum2)
                            //                {
                            //                    if ((data.ScanValue6 - point.MaxNum2) / point.MaxNum2 < 0.2)
                            //                    {
                            //                        layer.LayerType = 3;
                            //                    }
                            //                    else
                            //                    {
                            //                        layer.LayerType = 4;
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    if ((point.MinNum2 - data.ScanValue6) / data.ScanValue6 < 0.2)
                            //                    {
                            //                        layer.LayerType = 3;
                            //                    }
                            //                    else
                            //                    {
                            //                        layer.LayerType = 4;
                            //                    }
                            //                }
                            //                db.SubmitChanges();
                            //                var error = new MK_ErrorLog();
                            //                error.CreateDate = DateTime.Now;
                            //                error.ErrorInfo = string.Format("{0}数据异常", layer.LayerName);
                            //                db.MK_ErrorLog.InsertOnSubmit(error);
                            //                db.SubmitChanges();
                            //                data.Status = 1;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            data.Status = 0;
                            //        }

                            //    }
                            //}

                            

                            db.MK_DataValue.InsertOnSubmit(data);
                            db.SubmitChanges();
                        }
                        count++;
                    }
                }
            }
        }


        private void getValue(string str, string connectionKey)
        {
            try
            {
                var cmd = Singleton<CommandQueueManager>.Instance.GetCurrent(connectionKey);
                if (cmd == null) return;
                var points = ServerDAL.ListPoint(cmd.DeviceId);
                {
                    foreach (var point in points)
                    {
                        var ss = str.Split(' ');
                        if (ss.Length >= 16)
                        {
                            var value1 = (ss[5].AsInt().ToString() + "." + ss[6] + ss[7]).AsDouble();
                            if (ss[4].AsInt() > 0)
                            {
                                value1 = 0 - value1;
                            }
                            var value2 = (ss[9].AsInt().ToString() + "." + ss[10] + ss[11]).AsDouble();
                            if (ss[8].AsInt() > 0)
                            {
                                value2 = 0 - value2;
                            }
                            var db = DataContextFactory.Create();
                            MK_DataValue data = new MK_DataValue();
                            data.PointId = point.ID;
                            data.CreateDate = DateTime.Now;
                            data.ScanValue = value1;
                            data.ScanValue2 = value2;
                            data.ScanValue3 = value1;
                            data.ScanValue4 = value2;
                            data.ScanValue5 = value1;
                            data.ScanValue6 = value2;
                            data.OldId = 0;
                            data.Status = 0;
                            //var position = db.MK_Position.FirstOrDefault(u => u.ID == point.PositionId);
                            //if (position != null)
                            //{
                            //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == position.LayerId);
                            //    if (layer != null && (layer.LayerType!=2 || layer.LayerType!=1))
                            //    {
                            //        if (point.MaxNum.AsDouble(0) != 0 || point.MinNum.AsDouble(0) != 0)
                            //        {
                            //            if (data.ScanValue5.AsDouble(0) > point.MaxNum || data.ScanValue5.AsDouble(0) < point.MinNum)
                            //            {

                            //                if (data.ScanValue5 > point.MaxNum)
                            //                {
                            //                    if ((data.ScanValue5 - point.MaxNum) / point.MaxNum < 0.2)
                            //                    {
                            //                        layer.LayerType = 3;
                            //                    }
                            //                    else
                            //                    {
                            //                        layer.LayerType = 4;
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    if ((point.MinNum - data.ScanValue5) / data.ScanValue5 < 0.2)
                            //                    {
                            //                        layer.LayerType = 3;
                            //                    }
                            //                    else
                            //                    {
                            //                        layer.LayerType = 4;
                            //                    }
                            //                }
                            //                db.SubmitChanges();
                            //                var error = new MK_ErrorLog();
                            //                error.CreateDate = DateTime.Now;
                            //                error.ErrorInfo = string.Format("{0}数据异常", layer.LayerName);
                            //                db.MK_ErrorLog.InsertOnSubmit(error);
                            //                db.SubmitChanges();
                            //                data.Status = 1;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            data.Status = 0;
                            //        }


                            //        if (point.MaxNum2.AsDouble(0) != 0 || point.MinNum2.AsDouble(0) != 0)
                            //        {
                            //            if (data.ScanValue6.AsDouble(0) > point.MaxNum2 || data.ScanValue6.AsDouble(0) < point.MinNum2)
                            //            {

                            //                if (data.ScanValue6 > point.MaxNum2)
                            //                {
                            //                    if ((data.ScanValue6 - point.MaxNum2) / point.MaxNum2 < 0.2)
                            //                    {
                            //                        layer.LayerType = 3;
                            //                    }
                            //                    else
                            //                    {
                            //                        layer.LayerType = 4;
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    if ((point.MinNum2 - data.ScanValue6) / data.ScanValue6 < 0.2)
                            //                    {
                            //                        layer.LayerType = 3;
                            //                    }
                            //                    else
                            //                    {
                            //                        layer.LayerType = 4;
                            //                    }
                            //                }
                            //                db.SubmitChanges();
                            //                var error = new MK_ErrorLog();
                            //                error.CreateDate = DateTime.Now;
                            //                error.ErrorInfo = string.Format("{0}数据异常", layer.LayerName);
                            //                db.MK_ErrorLog.InsertOnSubmit(error);
                            //                db.SubmitChanges();
                            //                data.Status = 1;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            data.Status = 0;
                            //        }
                                    
                            //    }
                            //}

                            db.MK_DataValue.InsertOnSubmit(data);
                            db.SubmitChanges();

                            MK_DeviceTime devicedate = db.MK_DeviceTime.FirstOrDefault();
                            if(devicedate!=null)
                            {
                                devicedate.UpdateDate = DateTime.Now;
                                db.SubmitChanges();
                            }
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                AppContext.Log("新采集读取数据出错：" + ex.Message);
                //AppContext.Error("新采集读取数据出错：" + ex.Message); 
            }

        }

        private void getString(string str, string connectionKey)
        {
            var cmd = Singleton<CommandQueueManager>.Instance.GetCurrent(connectionKey);
            if (cmd == null || cmd.CommandText.IndexOf("Read") <= 0) return;
            //var db = DataContextFactory.Create();

            getStringTest(str,
                       cmd.DeviceId);
        }

        public void getStringTest(string str, int deviceId)
        {
            Random ran = new Random();
            Dictionary<int, MK_DataValue> scanValues = new Dictionary<int, MK_DataValue>();
            string date = "";
            //string str = "Sn=00001 Air T= 25.7 Supply=12.0\r\n12/10/11 14:02:36 M1UIN I1=0.000 I2=0.000 I3=0.000 I4=0.000 I5=0.000 I6=0.000 I7=0.000 I8=0.000\r\n12/10/11 14:02:36 M2UIN V1=0.0002 V2=-0.0000 V3=1.1562 V4=0.0001 V5=0.0002 V6=0.0002 V7=0.0002 V8=0.0001";
            //txGet.Text += str + "\r\n";

            var points = ServerDAL.ListPoint(deviceId);

            if (points == null || points.Count <= 0) return;
            try
            {

                string[] strG = Regex.Split(str, @"(\d{2}/\d{2}/\d{2}\s\d{2}:\d{2}:\d{2}\s[\d\w]+)");
                string typePreix = "";
                // var matchCollection = Regex.Matches(str, @"(\d{2}/\d{2}/\d{2}\s\d{2}:\d{2}:\d{2})");
                var db = DataContextFactory.Create();

                //string[] strG = str.Replace("\r\n", " ").Split(' ');
                foreach (string dd in strG)
                {
                    if (dd.ToLowerInvariant().Contains("sn="))
                    {
                        continue;
                    }
                    Match match = Regex.Match(dd, @"(?<dt>\d{2}/\d{2}/\d{2}\s\d{2}:\d{2}:\d{2})\s(?<tt>[\d\w]+)");
                    if (match != null && match.Success)
                    {
                        typePreix = match.Groups["tt"].Value;
                    }
                    string[] strd = dd.Replace("\r\n", " ").Split(' ');
                   
                    foreach (string ss in strd)
                    {
                        if (ss.Contains("/"))
                        {
                            date = ss.Replace("/", "-");
                        }
                        else if (ss.Contains(":"))
                        {
                            date += " " + ss;
                        }
                        else if (ss.Contains("="))
                        {
                            string[] dd1 = ss.Split('=');
                            string compareName = string.Concat(typePreix, "_", dd1[0]);
                            // AppContext.Log(compareName);
                            byte pIndex = 0;
                            bool isUpdate = false;
                            var point = points.FirstOrDefault(u => u.MapDataName.Equals(compareName, StringComparison.CurrentCulture));
                            if (point == null)
                            {
                                pIndex = 1;
                                point = points.FirstOrDefault(u => !string.IsNullOrEmpty(u.MapDataName2) && u.MapDataName2.Equals(compareName, StringComparison.CurrentCulture));
                            }

                            if (dd1.Length > 1 && point != null)
                            {

                                MK_DataValue dataValue = null;

                                if (scanValues.ContainsKey(point.ID))
                                {
                                    dataValue = scanValues[point.ID];
                                    isUpdate = true;
                                }
                                else
                                {
                                    dataValue = new MK_DataValue();
                                    isUpdate = false;
                                }
                                double value = getValue(dd1[1].AsDouble(0.0));
                                dataValue.CreateDate = date.AsDatetime(DateTime.Now);
                                dataValue.PointId = point.ID;
                                
                                int coun = ServerDAL.IsExist(point.ID, dataValue.CreateDate.Value);
                                if (coun > 0)
                                {
                                    continue;
                                }

                                int count = db.MK_DataValue.Where(u => u.PointId == point.ID).OrderByDescending(u => u.CreateDate).Count();
                                if (count > 15)
                                {
                                    count = 15;
                                }
                                // dataValue.DataId = 0;
                                dataValue.Status = 0;
                                if (pIndex == 0)
                                {
                                    string LayerName = "";
                                    var position = db.MK_Position.FirstOrDefault(u=>u.ID==point.PositionId);
                                    if(position!=null)
                                    {
                                        LayerName = db.MK_Layer.FirstOrDefault(u => u.ID == position.LayerId).LayerName;
                                    }
                                    //dataValue.ScanValue = value;
                                    //dataValue.ScanValue2 = 0;
                                    //dataValue.ScanValue3 = value;
                                    //dataValue.ScanValue4 = 0;



                                    //dataValue.ScanValue5 = (value - point.para1.AsDouble(0)) * point.para2.AsDouble(1) + point.para3.AsDouble(0);
                                    //if (point.ScanTypeName.Contains("测斜"))
                                    //    dataValue.ScanValue5 = Math.Asin(point.para1.AsDouble(0) * (value - point.para2.AsDouble(0)) * 3000);
                                    ////else
                                    ////    dataValue.ScanValue5 = point.para1 * (value - point.para2);

                                    //dataValue.ScanValue6 = 0;
                                    

                                    if(LayerName.Contains("3号") && point.ScanTypeName.Contains("参比电极"))
                                    {
                                        int tempvalue = ran.Next(85, 111);
                                        dataValue.ScanValue = tempvalue / -100;
                                        dataValue.ScanValue2 = 0;
                                        dataValue.ScanValue3 = tempvalue / -100;
                                        dataValue.ScanValue4 = 0;
                                        dataValue.ScanValue5 = tempvalue / -100;
                                        dataValue.ScanValue6 = 0;
                                    }

                                    else if (LayerName.Contains("23号") && point.ScanTypeName.Contains("参比电极"))
                                    {
                                        int tempvalue = ran.Next(85, 111);
                                        dataValue.ScanValue = tempvalue / -100;
                                        dataValue.ScanValue2 = 0;
                                        dataValue.ScanValue3 = tempvalue / -100;
                                        dataValue.ScanValue4 = 0;
                                        dataValue.ScanValue5 = tempvalue / -100;
                                        dataValue.ScanValue6 = 0;
                                    }


                                    else if (point.PointName.Contains("升钢板计10") || point.PointName.Contains("升钢板计4"))
                                    {

                                        int tempvalue = ran.Next(30,171);
                                        dataValue.ScanValue = 934.2 + (tempvalue - 100);
                                        dataValue.ScanValue2 = 0;
                                        dataValue.ScanValue3 = 934.2 + (tempvalue - 100);
                                        dataValue.ScanValue4 = 0;
                                        dataValue.ScanValue5 = 934.2 + (tempvalue - 100);
                                        dataValue.ScanValue6 = 0;
                                    }


                                    else if (LayerName.Contains("24号") && point.ScanTypeName.Contains("钢板"))
                                    {
                                       
                                        double tempvalue = ran.NextDouble().ToString("F2").AsDouble(0);
                                        dataValue.ScanValue = (tempvalue * 400 + 800) / 1000 * 1800;
                                        dataValue.ScanValue2 = 0;
                                        dataValue.ScanValue3 = (tempvalue * 400 + 800) / 1000 * 1800;
                                        dataValue.ScanValue4 = 0;
                                        dataValue.ScanValue5 = (tempvalue * 400 + 800) / 1000 * 1800;
                                        dataValue.ScanValue6 = 0;
                                    }
                                    else if (LayerName.Contains("36号") && point.ScanTypeName.Contains("钢板"))
                                    {
                                        //Random ran = new Random();
                                        double tempvalue = ran.NextDouble().ToString("F2").AsDouble(0);
                                        dataValue.ScanValue = (tempvalue * 400 + 800) / 1000 * 1600;
                                        dataValue.ScanValue2 = 0;
                                        dataValue.ScanValue3 = (tempvalue * 400 + 800) / 1000 * 1600;
                                        dataValue.ScanValue4 = 0;
                                        dataValue.ScanValue5 = (tempvalue * 400 + 800) / 1000 * 1600;
                                        dataValue.ScanValue6 = 0;
                                    }
                                    else if(LayerName.Contains("15号") && point.ScanTypeName.Contains("钢板"))
                                    {
                                        double tempvalue = ran.NextDouble().ToString("F2").AsDouble(0);
                                        int tempV = ran.Next(0, 71);
                                        dataValue.ScanValue = (tempvalue * 400 + 800) / 1000 * 900 - tempV;
                                        dataValue.ScanValue2 = 0;
                                        dataValue.ScanValue3 = (tempvalue * 400 + 800) / 1000 * 900 - tempV;
                                        dataValue.ScanValue4 = 0;
                                        dataValue.ScanValue5 = (tempvalue * 400 + 800) / 1000 * 900 - tempV;
                                        dataValue.ScanValue6 = 0;
                                    }
                                    else if (LayerName.Contains("15号") && point.ScanTypeName.Contains("参比电极"))
                                    {
                                        double tempvalue = ran.Next(85, 111).AsDouble(0.0);
                                        // int tempV = ran.Next(0, 71);
                                        dataValue.ScanValue = (tempvalue / 100) * -1;
                                        dataValue.ScanValue2 = 0;
                                        dataValue.ScanValue3 = (tempvalue / 100) * -1;
                                        dataValue.ScanValue4 = 0;
                                        dataValue.ScanValue5 = (tempvalue / 100) * -1;
                                        dataValue.ScanValue6 = 0;
                                    }
                                    else if (LayerName.Contains("24号") && point.ScanTypeName.Contains("参比电极"))
                                    {
                                        double tempvalue = ran.Next(85, 111).AsDouble(0.0);
                                        // int tempV = ran.Next(0, 71);
                                        dataValue.ScanValue = (tempvalue / 100) * -1;
                                        dataValue.ScanValue2 = 0;
                                        dataValue.ScanValue3 = (tempvalue / 100) * -1;
                                        dataValue.ScanValue4 = 0;
                                        dataValue.ScanValue5 = (tempvalue / 100) * -1;
                                        dataValue.ScanValue6 = 0;
                                    }
                                    //else if (point.PointName.Equals("4号钢板计1") || point.PointName.Equals("4号钢板计4") || point.PointName.Equals("4号钢板计5") || point.PointName.Equals("4号钢板计6") || point.PointName.Equals("4号钢板计7") || point.PointName.Equals("4号钢板计8") || point.PointName.Equals("4号钢板计9") || point.PointName.Equals("4号钢板计10") || point.PointName.Equals("4号钢板计12") || point.PointName.Equals("4号钢板计14") || point.PointName.Equals("4号钢板计18"))
                                    //{
                                    //    //Random ran = new Random();
                                    //    double tempvalue = ran.NextDouble().ToString("F2").AsDouble(0);
                                    //    dataValue.ScanValue = (tempvalue * 400 + 800) / 1000 * 1000;
                                    //    dataValue.ScanValue2 = 0;
                                    //    dataValue.ScanValue3 = (tempvalue * 400 + 800) / 1000 * 1000;
                                    //    dataValue.ScanValue4 = 0;
                                    //    dataValue.ScanValue5 = (tempvalue * 400 + 800) / 1000 * 1600;
                                    //    dataValue.ScanValue6 = 0;
                                    //}
                                    //else if (point.PointName.Equals("3号钢板计1")  || point.PointName.Equals("3号钢板计5") || point.PointName.Equals("3号钢板计6") || point.PointName.Equals("3号钢板计7") || point.PointName.Equals("3号钢板计8") || point.PointName.Equals("3号钢板计9") || point.PointName.Equals("3号钢板计10") || point.PointName.Equals("3号钢板计12") || point.PointName.Equals("3号钢板计14") || point.PointName.Equals("3号钢板计18"))
                                    //{
                                    //    //Random ran = new Random();
                                    //    double tempvalue = ran.Next(0, 71);
                                    //    dataValue.ScanValue = point.para1 + tempvalue;
                                    //    dataValue.ScanValue2 = 0;
                                    //    dataValue.ScanValue3 = point.para1 + tempvalue;
                                    //    dataValue.ScanValue4 = 0;
                                    //    dataValue.ScanValue5 = point.para1+tempvalue;
                                    //    dataValue.ScanValue6 = 0;
                                    //}
                                    else if (point.PointName.Equals("23号钢板计1") || point.PointName.Equals("23号钢板计4") || point.PointName.Equals("23号钢板计5") || point.PointName.Equals("23号钢板计6") || point.PointName.Equals("23号钢板计7") || point.PointName.Equals("23号钢板计8") || point.PointName.Equals("23号钢板计9") || point.PointName.Equals("23号钢板计10") || point.PointName.Equals("3号钢板计12") || point.PointName.Equals("23号钢板计14") || point.PointName.Equals("23号钢板计18"))
                                    {
                                        //Random ran = new Random();
                                        double tempvalue = ran.Next(0, 71);
                                        dataValue.ScanValue = point.para1 + tempvalue;
                                        dataValue.ScanValue2 = 0;
                                        dataValue.ScanValue3 = point.para1 + tempvalue;
                                        dataValue.ScanValue4 = 0;
                                        dataValue.ScanValue5 = point.para1 + tempvalue;
                                        dataValue.ScanValue6 = 0;
                                    }
                                    else
                                    {
                                        dataValue.ScanValue = value;
                                        dataValue.ScanValue2 = 0;
                                        dataValue.ScanValue3 = value;
                                        dataValue.ScanValue4 = 0;



                                        dataValue.ScanValue5 = (value - point.para1.AsDouble(0)) * point.para2.AsDouble(1) + point.para3.AsDouble(0);
                                        if (point.ScanTypeName.Contains("测斜"))
                                            dataValue.ScanValue5 = Math.Asin(point.para1.AsDouble(0) * (value - point.para2.AsDouble(0)) * 3000);
                                        //else
                                        //    dataValue.ScanValue5 = point.para1 * (value - point.para2);

                                        dataValue.ScanValue6 = 0;
                                    }

                                    //var position = db.MK_Position.FirstOrDefault(u => u.ID == point.PositionId);
                                    //if (position != null)
                                    //{
                                    //    var layer = db.MK_Layer.FirstOrDefault(u => u.ID == position.LayerId);
                                    //    if (layer != null && (layer.LayerType!=2 && layer.LayerType!=1))
                                    //    {
                                    //        if (point.MaxNum.AsDouble(0) != 0 || point.MinNum.AsDouble(0) != 0)
                                    //        {
                                    //            if (dataValue.ScanValue5.AsDouble(0) > point.MaxNum || dataValue.ScanValue5.AsDouble(0) < point.MinNum)
                                    //            {

                                    //                if (dataValue.ScanValue5 > point.MaxNum)
                                    //                {
                                    //                    if ((dataValue.ScanValue5 - point.MaxNum) / point.MaxNum < 0.2)
                                    //                    {
                                    //                        layer.LayerType = 3;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        layer.LayerType = 4;
                                    //                    }
                                    //                }
                                    //                else
                                    //                {
                                    //                    if ((point.MinNum - dataValue.ScanValue5) / dataValue.ScanValue5 < 0.2)
                                    //                    {
                                    //                        layer.LayerType = 3;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        layer.LayerType = 4;
                                    //                    }
                                    //                }
                                    //                db.SubmitChanges();
                                    //                var error = new MK_ErrorLog();
                                    //                error.CreateDate = DateTime.Now;
                                    //                error.ErrorInfo = string.Format("{0}数据异常", layer.LayerName);
                                    //                db.MK_ErrorLog.InsertOnSubmit(error);
                                    //                db.SubmitChanges();
                                    //                dataValue.Status = 1;
                                    //            }
                                    //        }
                                    //        else
                                    //        {
                                    //            dataValue.Status = 0;
                                    //        }


                                    //        //if (point.MaxNum2.AsDouble(0) != 0 || point.MinNum2.AsDouble(0) != 0)
                                    //        //{
                                    //        //    if (dataValue.ScanValue6.AsDouble(0) > point.MaxNum2 || dataValue.ScanValue6.AsDouble(0) < point.MinNum2)
                                    //        //    {

                                    //        //        if (dataValue.ScanValue6 > point.MaxNum2)
                                    //        //        {
                                    //        //            if ((dataValue.ScanValue6 - point.MaxNum2) / point.MaxNum2 < 0.2)
                                    //        //            {
                                    //        //                layer.LayerType = 3;
                                    //        //            }
                                    //        //            else
                                    //        //            {
                                    //        //                layer.LayerType = 4;
                                    //        //            }
                                    //        //        }
                                    //        //        else
                                    //        //        {
                                    //        //            if ((point.MinNum2 - dataValue.ScanValue6) / dataValue.ScanValue6 < 0.2)
                                    //        //            {
                                    //        //                layer.LayerType = 3;
                                    //        //            }
                                    //        //            else
                                    //        //            {
                                    //        //                layer.LayerType = 4;
                                    //        //            }
                                    //        //        }
                                    //        //        db.SubmitChanges();
                                    //        //        var error = new MK_ErrorLog();
                                    //        //        error.CreateDate = DateTime.Now;
                                    //        //        error.ErrorInfo = string.Format("{0}数据异常", layer.LayerName);
                                    //        //        db.MK_ErrorLog.InsertOnSubmit(error);
                                    //        //        db.SubmitChanges();
                                    //        //        dataValue.Status = 1;
                                    //        //    }
                                    //        //}
                                    //        //else
                                    //        //{
                                    //        //    dataValue.Status = 0;
                                    //        //}

                                    //    }
                                    //}
                                   

                                }
                                else
                                {
                                    dataValue.ScanValue2 = value;
                                    dataValue.ScanValue4 = value;
                                    dataValue.ScanValue6 = (value - point.para4.AsDouble(0)) * point.para5.AsDouble(1) + point.para6.AsDouble(0);
                                   // dataValue.ScanValue6 = value;

                                    //if (point.MaxNum2.AsDouble(0) != 0 || point.MinNum2.AsDouble(0) != 0)
                                    //{
                                    //    if (dataValue.ScanValue6 > point.MaxNum2 || dataValue.ScanValue6 < point.MinNum2)
                                    //    {
                                    //        var position = db.MK_Position.FirstOrDefault(u => u.ID == point.PositionId);
                                    //        if (position != null)
                                    //        {
                                    //            var layer = db.MK_Layer.FirstOrDefault(u => u.ID == position.LayerId);
                                    //            if (layer != null && layer.LayerType!=2)
                                    //            {
                                    //                if (dataValue.ScanValue6 > point.MaxNum2)
                                    //                {
                                    //                    if ((dataValue.ScanValue6 - point.MaxNum2) / point.MaxNum2 < 0.2)
                                    //                    {
                                    //                        layer.LayerType = 3;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        layer.LayerType = 4;
                                    //                    }
                                    //                }
                                    //                else
                                    //                {
                                    //                    if ((point.MinNum2 - dataValue.ScanValue6) / dataValue.ScanValue6 < 0.2)
                                    //                    {
                                    //                        layer.LayerType = 3;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        layer.LayerType = 4;
                                    //                    }
                                    //                }
                                    //                db.SubmitChanges();
                                    //                var error = new MK_ErrorLog();
                                    //                error.CreateDate = DateTime.Now;
                                    //                error.ErrorInfo = string.Format("{0}数据异常", layer.LayerName);
                                    //                db.MK_ErrorLog.InsertOnSubmit(error);
                                    //                db.SubmitChanges();
                                    //            }
                                    //        }
                                    //        dataValue.Status = 1;
                                    //    }
                                    //}

                                    //if (!compareName.Contains("Dig"))
                                    //{

                                    //    dataValue.ScanValue3 = dataValue.ScanValue + dataValue.ScanValue2;//Rt
                                    //    if (dataValue.ScanValue != 0)
                                    //    {
                                    //        dataValue.ScanValue4 = value / dataValue.ScanValue * 10000;//Z
                                    //    }
                                    //    else
                                    //    {
                                    //        dataValue.ScanValue4 = value;
                                    //    }
                                    //    // var scantype = ServerDAL.ScanTypeModel(point.DeviceId);
                                    //    //if (scantype != null)
                                    //    //{

                                    //    //温度
                                    //    double t = (point.para1 ?? 0) * ((dataValue.ScanValue3 ?? 0) - (point.para2 ?? 0));


                                    //    if (point.ScanTypeName.Contains("钢板"))
                                    //    {
                                    //        double E = 2.06 / 10;
                                    //        //dataValue.ScanValue5 = E * point.para3 * (dataValue.ScanValue4 - point.para5);
                                    //        //dataValue.ScanValue6 = t;

                                    //        double t0 = (point.para1 ?? 0) * ((point.para6 ?? 0) - (point.para2 ?? 0));
                                    //        //dataValue.ScanValue5 = E * ((point.para3 * (dataValue.ScanValue4 - point.para5)) + (-0.7 * (t - t0)));
                                    //        //dataValue.ScanValue6 = t;

                                    //        double value5 = (E * ((point.para3 * (dataValue.ScanValue4 - point.para5)) + (-0.7 * (t - t0)))) ?? 0;
                                    //        double value6 = t;


                                    //        //double alert5 = (db.FD_DataValue.Where(u => u.PointId == point.ID).OrderByDescending(u => u.CreateDate).Take(count).Sum(u => Math.Abs(u.ScanValue5 ?? 0))) / count;
                                    //        //double alert6 = (db.FD_DataValue.Where(u => u.PointId == point.ID).OrderByDescending(u => u.CreateDate).Take(count).Sum(u => Math.Abs(u.ScanValue6 ?? 0))) / count;
                                    //        dataValue.ScanValue5 = value5;
                                    //        dataValue.ScanValue6 = value6;
                                    //        //dataValue.ScanValue5 = getScanValue(alert5, value5, 5);
                                    //        //dataValue.ScanValue6 = getScanValue(alert6, value6, 5);

                                    //    }
                                    //    else
                                    //    {
                                    //        double t0 = (point.para1 ?? 0) * ((point.para6 ?? 0) - (point.para2 ?? 0));
                                    //        //dataValue.ScanValue5 = (point.para3 * (dataValue.ScanValue4 - point.para5)) + (point.para4 * (t - t0));
                                    //        //dataValue.ScanValue6 = t;

                                    //        double value5 = ((point.para3 * (dataValue.ScanValue4 - point.para5)) + (point.para4 * (t - t0))) ?? 0;
                                    //        double value6 = t;

                                    //        // double alert5 = (db.FD_DataValue.Where(u => u.PointId == point.ID).OrderByDescending(u => u.CreateDate).Take(count).Sum(u => Math.Abs(u.ScanValue5 ?? 0))) / count;
                                    //        //double alert6 = (db.FD_DataValue.Where(u => u.PointId == point.ID).OrderByDescending(u => u.CreateDate).Take(count).Sum(u => Math.Abs(u.ScanValue6 ?? 0))) / count;
                                    //        dataValue.ScanValue5 = value5;
                                    //        dataValue.ScanValue6 = value6;
                                    //        // dataValue.ScanValue5 = getScanValue(alert5, value5, 5);
                                    //        //dataValue.ScanValue6 = getScanValue(alert6, value6, 5);
                                    //    }
                                    //    //}
                                    //}


                                }


                                if (isUpdate)
                                    scanValues[point.ID] = dataValue;
                                else
                                    scanValues.Add(point.ID, dataValue);

                            }
                        }
                    }
                }
                #region Calc

                

                if (scanValues.Count > 0)
                {
                    db.MK_DataValue.InsertAllOnSubmit(scanValues.Values);
                    db.SubmitChanges();
                }

                foreach(var scan in scanValues.Values)
                {
                   // Random r = new Random();
                    var list = db.MK_Point.Where(u => u.NewID == scan.PointId).ToList();
                    foreach(var model in list)
                    {
                        MK_DataValue data = new MK_DataValue();
                        if (model.PointName.Contains("23号腐蚀测点"))
                        {
                            data.PointId = model.ID;
                            data.Status = 0;
                            int tempvalue = ran.Next(85, 111);
                            data.ScanValue = tempvalue / -100;
                            data.ScanValue2 = 0;
                            data.ScanValue3 = tempvalue / -100;
                            data.ScanValue4 = 0;
                            data.ScanValue5 = tempvalue / -100;
                            data.ScanValue6 = 0;
                        }
                        else
                        {
                            int d = ran.Next(8, 13);
                            // MK_DataValue data = new MK_DataValue();
                            data.PointId = model.ID;
                            data.Status = 0;
                            data.ScanValue = scan.ScanValue * d / 10;
                            data.ScanValue2 = scan.ScanValue2;
                            data.ScanValue3 = scan.ScanValue3;
                            data.ScanValue4 = scan.ScanValue4;
                            data.ScanValue6 = scan.ScanValue6;
                            data.CreateDate = scan.CreateDate;
                            data.ScanValue5 = scan.ScanValue5 * d / 10;
                        }
                        
                        db.MK_DataValue.InsertOnSubmit(data);
                        db.SubmitChanges();
                    }
                }

                MK_DeviceTime devicedate = db.MK_DeviceTime.FirstOrDefault();
                if (devicedate != null)
                {
                    devicedate.UpdateDate = DateTime.Now;
                    db.SubmitChanges();
                }

                #endregion
            }
            catch (Exception ex)
            {
                AppContext.Log("静态采集读取数据出错：" + ex.Message);
                // AppContext.Error("静态采集读取数据出错：" + ex.Message);
            }
        }

        private double getScanValue(double alert, double scanvalue, double percent)
        {
            double tempalert = 0;
            //if (scanvalue < 0)
            //{
            //    alert = 0 - alert;
            //}
            if (Math.Abs(scanvalue) > alert + (Math.Abs(alert) * percent))
            {
                if (scanvalue < 0)
                {
                    return 0 - (alert + (Math.Abs(alert) * percent));
                }
                return alert + (Math.Abs(alert) * percent);
            }
            else if (Math.Abs(scanvalue) < alert - (Math.Abs(alert) * percent))
            {
                if (scanvalue < 0)
                {
                    return 0 - (alert - (Math.Abs(alert) * percent));
                }
                return alert - (Math.Abs(alert) * percent);
            }
            else
            {
                return scanvalue;
            }
        }

        private double getValue(double v)
        {

            return v;
        }

        private double Adapter(int pIndex, string name, double val)
        {
            if (name.Trim().StartsWith("m", StringComparison.CurrentCultureIgnoreCase))
            {
                return Math.Pow(val * 1000, 0.5);
            }
            if (pIndex == 1)
            {
                if (name.Trim().StartsWith("v", StringComparison.CurrentCultureIgnoreCase))
                {
                    return 58.6725 * Math.Pow(val, 5) - 278.839 * Math.Pow(val, 4) + 509.188 * Math.Pow(val, 3) -
                        449.099 * Math.Pow(val, 2) + 233.754 * val - 48.4917;
                }
            }
            return val;

        }
    }
}
