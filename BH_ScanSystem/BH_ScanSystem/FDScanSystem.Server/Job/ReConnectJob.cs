using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.DAO;
using FDScanSystem.Helper;
using FDScanSystem.Server.Device;
using Interactive.Utility;

namespace FDScanSystem.Server.Job
{
    public class ReConnectJob : MoniterBase
    {
        protected ReConnectJob() : base(72000000) { }
        static MoniterBase _myInstance;
        static object _lock = new object();
        //private string now = "2013-10-8 00:00:00";
        protected override void PostData()
        {
            DeviceConnectionManager.GetInstance().ReConnect();
        }
        /// <summary>
        /// 获取单例实例
        /// </summary>
        /// <returns></returns>
        public static MoniterBase GetInstance()
        {
            if (_myInstance == null)
            {
                lock (_lock)
                {
                    if (_myInstance == null)
                    {
                        _myInstance = new ReConnectJob();
                    }
                }
            }
            return _myInstance;
        }
    }
}
