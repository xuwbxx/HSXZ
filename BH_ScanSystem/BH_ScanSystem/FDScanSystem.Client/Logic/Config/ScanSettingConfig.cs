using FDScanSystem.Protocol.ScanConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FDScanSystem.Client.Logic.Config
{
    public class ScanSettingConfig
    {

        readonly string CONFIG_LOCATION = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/scan_setting.txt");

        public DateTime Begin { get; set; }

        public int Interval { get; set; }

        public int ScanTime { get; set; }

        static bool IsLoaded = false;

        public void Load()
        {
            using (FileStream fs = new FileStream(CONFIG_LOCATION, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader fsr = new StreamReader(fs, Encoding.UTF8))
                {
                    string content = fsr.ReadToEnd();
                    ScanSetting setting = ZJFramework10.Serialize.Helper.DeserializeString<ScanSetting>(content);
                    if (setting != null)
                    {
                        this.Begin = setting.Begin;
                        this.Interval = setting.Interval;
                        this.ScanTime = setting.ScanTime;
                    }
                    IsLoaded = true;
                }
            }
        }

        public void Update(ScanSetting newConfig)
        {
            using (FileStream fs = new FileStream(CONFIG_LOCATION, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter fsw = new StreamWriter(fs, Encoding.UTF8))
                {
                    fsw.WriteLine(ZJFramework10.Serialize.Helper.SerializeString(newConfig));
                    IsLoaded = false;
                }
            }


        }

        static ScanSettingConfig config = new ScanSettingConfig();

        public static ScanSettingConfig GetInstance()
        {

            if (!IsLoaded)
            {
                config.Load();
            }
            return config;
        }
    }
}
