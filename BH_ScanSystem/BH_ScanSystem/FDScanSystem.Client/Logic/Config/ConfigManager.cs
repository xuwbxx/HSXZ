using FDScanSystem.Protocol.ScanConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FDScanSystem.Client.Logic.Config
{
    public class ConfigManager
    {

       

        public static ScanSettingConfig GetConfig()
        {
            return ScanSettingConfig.GetInstance();
        }



        public static DevicePointDataConfig GetDeviceConfig()
        {
            return DevicePointDataConfig.GetInstance();
        }
    }
}
