using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcpTransmit.Core.Server;
using FDScanSystem.Listen.Analysis;

namespace FDScanSystem.Listen.Device {

  
    /// <summary>
    /// 设备实体类
    /// </summary>
    public class DeviceItem {
        /// <summary>
        /// 字典的Key
        /// </summary>
        public string Key {
            get {

                return string.Concat(IP, ":" + Port);
            }
        }


        /// <summary>
        /// 连接ID
        /// </summary>
        public int ConnectionId { get; set; }

        /// <summary>
        /// SIM卡号
        /// </summary>
        public string SIMId { get; set; }

        /// <summary>
        /// 传输协议类型
        /// </summary>
        public AnalysisProtocolType Type { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceId { get; set; }

        /// <summary>
        /// 远程IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 远程端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 连接之时间
        /// </summary>
        public DateTime ConnectionDate { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public ConnectionStatus Status { get; set; }

        /// <summary>
        /// 消息缓存
        /// </summary>
        public List<byte> Buffer { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        /// <param name="data"></param>
        public void SendSync(byte[] data) {
            var connect = Host.DefaultHost.GetSocketConnectionById(ConnectionId);
            if (connect != null) {
                connect.BeginSend(data);
            }
        }

    }
}
