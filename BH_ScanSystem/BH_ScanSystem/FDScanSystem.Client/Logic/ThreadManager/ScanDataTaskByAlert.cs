using Automation.BDaq;
using FDScanSystem.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace FDScanSystem.Client.Logic.ThreadManager
{
    class ScanDataTaskByAlert
    {
        public static ScanDataTaskByAlert Create(ScanDataTaskConfig config, int pointId, int channel)
        {

            return new ScanDataTaskByAlert(config, pointId, channel);
        }

        private ScanDataTaskConfig _config;
        private int _pointId;
        private int _channel;
        protected ScanDataTaskByAlert(ScanDataTaskConfig config, int pointId, int channel)
        {
            _config = config;
            _pointId = pointId;
            _channel = channel;
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
            int channelCount = ScanDataTask.channelCount;
            ErrorCode errorCode = ErrorCode.Success;
            InstantAiCtrl instantAIContrl = new InstantAiCtrl();
            Dictionary<long, double> result = new Dictionary<long, double>();

            try
            {
                instantAIContrl.SelectedDevice = new DeviceInformation(deviceDescription);
                double[] scaledData = new double[channelCount];
                do
                {
                    errorCode = instantAIContrl.Read(startChannel, channelCount, scaledData);
                    if (BioFailed(errorCode))
                    {
                        break;
                    }
                    if (_channel < scaledData.Length)
                        result.Add(DateTime.Now.ToFileTime(), scaledData[_channel]);
                    else
                        AppContext.Error("索引值大于数据长度发生于阀值监测模块");
                    Thread.Sleep(scanConfig.Interval);
                } while (DateTime.Now < doneDate);
            }
            catch (Exception ex)
            {
                LogWriter.Error(ex.ToString());
            }
            finally
            {
                if (result.Count > 5)
                    scanConfig.ActionCallback2(result, scanConfig.DeviceId, context);
                AppContext.Success("采集任务执行结束");
            }
        }

        static bool BioFailed(ErrorCode err)
        {
            return err < ErrorCode.Success && err >= ErrorCode.ErrorHandleNotValid;
        }
    }
}
