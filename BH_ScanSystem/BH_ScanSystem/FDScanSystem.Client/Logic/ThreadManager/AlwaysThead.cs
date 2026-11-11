using Automation.BDaq;
using FDScanSystem.Client.Logic.Config;
using FDScanSystem.Helper;
using FDScanSystem.Protocol.ScanConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZJFramework10.Extension;

namespace FDScanSystem.Client.Logic.ThreadManager
{
    public class AlwaysThead : IThead
    {
        private DeviceInfo _DeviceInfo;
        public AlwaysThead(DeviceInfo deviceInfo)
        {
            _DeviceInfo = deviceInfo;
        }


        Thread _thread;
        bool _start = false;
        public void Start()
        {
            _thread = new Thread(new ThreadStart(ThreadCallback));
            _start = true;
            _thread.Start();

        }

        public void ThreadCallback()
        {
            if (_DeviceInfo == null || string.IsNullOrEmpty(_DeviceInfo.DeviceIdentity)) { return; }
            string deviceDescription = _DeviceInfo.DeviceIdentity;
            int startChannel = 0;
            const int channelCount = 16;

            ErrorCode errorCode = ErrorCode.Success;
            InstantAiCtrl instantAIContrl = new InstantAiCtrl();
            var _Config = ConfigManager.GetConfig();

            try
            {
                var device = ConfigManager.GetDeviceConfig();
                if (device != null && device.Devices.Count(u => u.DeviceId == _DeviceInfo.DeviceId) > 0)
                {
                    _DeviceInfo = device.Devices.FirstOrDefault(u => u.DeviceId == _DeviceInfo.DeviceId);
                }

                instantAIContrl.SelectedDevice = new DeviceInformation(deviceDescription);
                int channelCountMax = instantAIContrl.Features.ChannelCountMax;
                double[] scaledData = new double[channelCount];
                do
                {
                    errorCode = instantAIContrl.Read(startChannel, channelCount, scaledData);
                    if (BioFailed(errorCode))
                    {
                        break;
                    }
                    foreach (var item in _DeviceInfo.Points.Values)
                    {
                        if (item.Channel1.AsInt() < scaledData.Length)
                        {

                           // double val = item.lingmingValue;
                            double val = scaledData[item.Channel1.AsInt()];
                            val = val - item.StandardValue;
                            if (item.lingmingValue != 0)
                            {
                                val = val / item.lingmingValue;
                            }

                            if ((val < item.AlertMin || val > item.AlertMax) && PointAlertStatusManager.AllowAlert(item.PointId))
                            {
                                var task = ScanDataTaskByAlert.Create(new ScanDataTaskConfig
                                {
                                    IsAlert = true,
                                    ActionCallback2 = (a, b, c) =>
                                    {
                                        IDataManager dataManager = DataManager.GetInstance();
                                        dataManager.Append(c, a);
                                    },
                                    DeviceId = _DeviceInfo.DeviceId,
                                    DeviceIdentity = _DeviceInfo.DeviceIdentity,
                                    Interval = _DeviceInfo.Interval,
                                    Channel = item.Channel1.AsInt(),
                                    PointId = item.PointId,
                                    AlertValue = val,
                                    ScanTime = _DeviceInfo.HowLong > 0 ? _DeviceInfo.HowLong : _Config.ScanTime
                                }, item.PointId, item.Channel1.AsInt());
                                task.Start();

                                PointAlertStatusManager.Alert(item.PointId);
                            }
                        }
                    }
                    Thread.Sleep(20);
                } while (_start);
            }
            catch (Exception ex)
            {
                LogWriter.Error(ex.ToString());
            }
            finally
            {

            }
        }

        public void Stop()
        {
            _start = false;
            Thread.Sleep(10);
            _thread.Abort();
        }

        static bool BioFailed(ErrorCode err)
        {
            return err < ErrorCode.Success && err >= ErrorCode.ErrorHandleNotValid;
        }


        public string ThreadName
        {
            get { return "AlwaysThread"; }
        }
    }


    internal class PointAlertStatusManager
    {
        public static Dictionary<int, long> _TempDatas = new Dictionary<int, long>();

        public static object _LOCK = new object();

        public static void Alert(int pointId)
        {
            lock (_LOCK)
            {
                DateTime now = DateTime.Now.AddMinutes(3);

                if (!_TempDatas.ContainsKey(pointId))
                {
                    _TempDatas.Add(pointId, now.ToFileTime());
                }
                else
                {
                    _TempDatas[pointId] = now.ToFileTime();
                }

            }
        }

        public static bool AllowAlert(int pointId)
        {
            lock (_LOCK)
            {
                long t = 0;
                DateTime now = DateTime.Now;
                if (_TempDatas.TryGetValue(pointId, out t))
                {
                    return now.ToFileTime() > t;
                }
            }
            return true;
        }
    }
}
