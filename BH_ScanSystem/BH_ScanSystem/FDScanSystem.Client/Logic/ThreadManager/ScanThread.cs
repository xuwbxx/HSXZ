using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FDScanSystem.Client.Logic.Config;
using FDScanSystem.Helper;
using Automation.BDaq;

namespace FDScanSystem.Client.Logic.ThreadManager
{
    public class ScanThread : IThead
    {

        Timer _Timer;
        ScanSettingConfig _Config;
        DevicePointDataConfig _DataConfig;
        bool isStart = false;

        public void Start()
        {
            _Timer = new Timer(new TimerCallback(TimeCallback), null, Timeout.Infinite, Timeout.Infinite);
            _Config = ConfigManager.GetConfig();
            _DataConfig = ConfigManager.GetDeviceConfig();
            if (_Config == null) { throw new Exception("参数配置不正确"); }
            isStart = true;
           
            var yushu = (DateTime.Now - _Config.Begin).Milliseconds;
            if (_Config.Interval != 0)
            {
                yushu = yushu % _Config.Interval;
            }
         
            //  AppContext.Success(yushu.ToString());
            _Timer.Change(yushu, yushu);
        }


        protected void TimeCallback(object state)
        {
            try
            {
                foreach (var item in _DataConfig.Devices)
                {
                    var task = ScanDataTask.Create(new ScanDataTaskConfig
                    {
                        IsAlert = false,
                        ActionCallback = (a, b, c) =>
                        {

                            if (a != null)
                            {
                                foreach (var tt in a.Keys)
                                {
                                    AppContext.Success(string.Format("=====Key:{0}======", DateTime.FromFileTime(tt)));
                                }
                            }
                            IDataManager dataManager = DataManager.GetInstance();
                            dataManager.Append(c, a);
                        },
                        DeviceId = item.DeviceId,
                        DeviceIdentity = item.DeviceIdentity,
                        Interval = item.Interval,
                        TDJZPL = item.TDJZPL,
                        ZDJZPL = item.ZDJZPL,
                        ScanTime = item.HowLong > 0 ? item.HowLong : _Config.ScanTime
                    });
                    task.Start();
                }
            }
            catch (Exception ex)
            {
                AppContext.Error(ex);
                LogWriter.Error(ex.ToString());
            }
            finally
            {
                if (isStart)
                    _Timer.Change(_Config.Interval, _Config.Interval);

            }
        }

        static bool BioFailed(ErrorCode err)
        {
            return err < ErrorCode.Success && err >= ErrorCode.ErrorHandleNotValid;
        }

        public void Stop()
        {
            isStart = false;
            Thread.Sleep(100);
            _Timer.Change(Timeout.Infinite, Timeout.Infinite);
            // throw new NotImplementedException();
            _Timer.Dispose();
        }

        public string ThreadName
        {
            get { return ThreadManager.CaijiThread; }
        }
    }
}
