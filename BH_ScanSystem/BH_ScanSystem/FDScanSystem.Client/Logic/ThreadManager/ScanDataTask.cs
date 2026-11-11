using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Automation.BDaq;
using FDScanSystem.Client.Logic.Config;
using FDScanSystem.Helper;
using ZJFramework10.Extension;

namespace FDScanSystem.Client.Logic.ThreadManager
{
    public class ScanDataTask
    {
        public const int channelCount = 16;
        public static ScanDataTask Create(ScanDataTaskConfig config)
        {

            return new ScanDataTask(config);
        }

        private ScanDataTaskConfig _config;
        protected ScanDataTask(ScanDataTaskConfig config)
        {
            _config = config;
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadCallback), _config);
        }

        void ThreadCallback(object state)
        {
            AppContext.Success("采集任务开始执行");
            var scanConfig = (state as ScanDataTaskConfig);
            ScanThreadContext context
                 = new ScanThreadContext(scanConfig);
            DateTime now = DateTime.Now;
            DateTime doneDate = now.AddMilliseconds(scanConfig.ScanTime);
            string deviceDescription = scanConfig.DeviceIdentity;
            int startChannel = 0;

            ErrorCode errorCode = ErrorCode.Success;
            InstantAiCtrl instantAIContrl = new InstantAiCtrl();
            Dictionary<long, double[]> result = new Dictionary<long, double[]>();

            try
            {
                instantAIContrl.SelectedDevice = new DeviceInformation(deviceDescription);
              //  var deviceList = ConfigManager.GetDeviceConfig();




                //Stopwatch watch = new Stopwatch();
                //long timespan = 0;
                do
                {
                    //watch.Start();
                    new System.Threading.Tasks.Task(() =>
                    {
                        try
                        {
                            double[] scaledData = new double[channelCount];
                            errorCode = instantAIContrl.Read(startChannel, channelCount, scaledData);

                            //foreach (var _DeviceInfo in deviceList.Devices)
                            //{
                            //    foreach (var item in _DeviceInfo.Points.Values)
                            //    {
                            //        if (item.Channel1.AsInt() < scaledData.Length)
                            //        {
                            //            var val = scaledData[item.Channel1.AsInt()];
                            //            val = val - item.StandardValue;
                            //            if (item.lingmingValue != 0)
                            //            {
                            //                val = val / item.lingmingValue;
                            //            }
                            //            scaledData[item.Channel1.AsInt()] = val;
                            //        }
                            //    }
                            //}

                            if (BioFailed(errorCode))
                            {
                                return;
                            }
                            long key = DateTime.Now.ToFileTime();
                            if (!result.ContainsKey(key))
                                result.Add(key, scaledData);
                        }
                        catch (Exception ex)
                        {
                            AppContext.Error(ex);
                        }
                    }).Start();

                    //watch.Stop();

                    //int interval = (int)(scanConfig.Interval - (watch.ElapsedMilliseconds - timespan));
                    //timespan = watch.ElapsedMilliseconds;
                    //if (interval > 1)
                    //{
                    Thread.Sleep(scanConfig.Interval);
                    //}
                } while (DateTime.Now < doneDate);
            }
            catch (Exception ex)
            {
                LogWriter.Error(ex.ToString());
            }
            finally
            {
                if (result.Count > 5)
                    scanConfig.ActionCallback(result, scanConfig.DeviceId, context);
                AppContext.Success("采集任务执行结束");
            }
        }

        static bool BioFailed(ErrorCode err)
        {
            return err < ErrorCode.Success && err >= ErrorCode.ErrorHandleNotValid;
        }
    }



    public class ScanDataTaskConfig
    {

        public bool IsAlert { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceId { get; set; }

        /// <summary>
        /// 监测点ID
        /// </summary>
        public int PointId { get; set; }

        /// <summary>
        /// 报警值
        /// </summary>
        public double AlertValue { get; set; }

        /// <summary>
        /// 通道号
        /// </summary>
        public int Channel { get; set; }

        /// <summary>
        /// 链接标识
        /// </summary>
        public string DeviceIdentity { get; set; }

        /// <summary>
        /// 豪秒为单位
        /// </summary>
        public int ScanTime { get; set; }

        /// <summary>
        /// 间隔
        /// </summary>
        public int Interval { get; set; }

        public int TDJZPL { set; get; }

        public int ZDJZPL { set; get; }


        /// <summary>
        /// 采集完成的回调
        /// </summary>
        public Action<Dictionary<long, double[]>, int, ScanThreadContext> ActionCallback { get; set; }

        /// <summary>
        /// 回调2
        /// </summary>
        public Action<Dictionary<long, double>, int, ScanThreadContext> ActionCallback2 { get; set; }

    }
}
