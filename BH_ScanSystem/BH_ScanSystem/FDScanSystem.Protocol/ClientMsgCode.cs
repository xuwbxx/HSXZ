using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Protocol
{
    public class ClientMsgCode
    {

        public const string Response_OK = "200";

        public const string Response_Error = "500";
        /// <summary>
        /// 远程召测采集
        /// </summary>
        public const string ServerToClient_Caiji = "10001";


        /// <summary>
        /// 设置采集周过期参数
        /// </summary>
        public const string ServerToClient_SetPeriod = "10002";


        /// <summary>
        /// 设置通道采集周过期参数
        /// </summary>
        public const string ServerToClient_SetPeriod2 = "10003";

        /// <summary>
        /// 重启服务
        /// </summary>
        public const string ServerToClient_ShutDown = "10011";




        /// <summary>
        /// 上报日常采集监测值
        /// </summary>
        public const string ClientToServer_DataInfo = "20001";


        /// <summary>
        /// 上报阀值监测值
        /// </summary>
        public const string ClientToServer_DataAlertInfo = "20003";
    }
}
