using FDScanSystem.Client.Logic.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FDScanSystem.Client.Logic.ThreadManager
{
    public class ThreadManager
    {

        public const string CaijiThread = "Caiji";
        public const string AlwaysThread = "AlwaysThread";
        public const string DataToFileThread = "DataToFile";

        static Dictionary<string, IThead> _thead;

        static ThreadManager()
        {
            _thead = new Dictionary<string, IThead>();
            _thead.Add(CaijiThread, new ScanThread());

            _thead.Add(DataToFileThread, DataManager.GetInstance());


            var deviceList = ConfigManager.GetDeviceConfig();
            foreach (var item in deviceList.Devices)
            {
                _thead.Add(AlwaysThread + 1, new AlwaysThead(item));
            }
        }


        public static void Start()
        {
            //return;
            foreach (string key in _thead.Keys)
            {
                try
                {
                    _thead[key].Start();
                }
                catch (Exception ex)
                {
                    AppContext.Error(ex);
                }
            }
        }

        public static void ReStart()
        {
            AppContext.Success("开始重启线程");
            Stop();
            Thread.Sleep(500);

            _thead = new Dictionary<string, IThead>();
            _thead.Add(CaijiThread, new ScanThread());
            _thead.Add(DataToFileThread, DataManager.GetInstance());
            var deviceList = ConfigManager.GetDeviceConfig();
            foreach (var item in deviceList.Devices)
            {
                _thead.Add(AlwaysThread + 1, new AlwaysThead(item));
            }
            Start();
            AppContext.Success("结束重启线程");
        }

        public static void Stop()
        {
            foreach (string key in _thead.Keys)
            {
                try
                {
                    _thead[key].Stop();
                }
                catch (Exception ex)
                {
                    AppContext.Error(ex);
                }
            }
        }

        public static void ReStart(string key)
        {
            IThead t = null;
            if (_thead.TryGetValue(key, out t))
            {
                t.Stop();
                System.Threading.Thread.Sleep(1000);
                t.Start();
            }
        }
    }
}
