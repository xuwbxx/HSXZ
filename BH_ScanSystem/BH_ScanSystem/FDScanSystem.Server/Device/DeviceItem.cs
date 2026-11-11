using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using FDScanSystem.DAO.Model;
using FDScanSystem.Server.Analysis;

namespace FDScanSystem.Server.Device
{


    /// <summary>
    /// 设备实体类
    /// </summary>
    public class DeviceItem
    {
        /// <summary>
        /// 字典的Key
        /// </summary>
        public string Key
        {
            get
            {

                return COMPort.PortName;
            }
        }

        public SerialPort COMPort { get; set; }

        public int Length { set; get; }

        /// <summary>
        /// 传输协议类型
        /// </summary>
        public AnalysisProtocolType Type { get; set; }

        public int Protocol { set; get; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceId { get; set; }

        //  public string CRC { set; get; }

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
        /// 消息缓存
        /// </summary>
        public List<byte> Buffer1 { get; set; }

        public ScanDeviceCollection ScanDevices
        { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        /// <param name="data"></param>
        public void SendSync(string msg)
        {
            if (COMPort != null)
                COMPort.WriteLine(msg);
        }

        /// <summary>
        /// 发送二进制内容
        /// </summary>
        /// <param name="data"></param>
        public void SendByte(string msg)
        {
            if (COMPort != null)
            {
                var ch = msg.Split(' ');
                byte[] temp = new byte[ch.Length];

                for (int i = 0; i < ch.Length; i++)
                {
                    temp[i] = Convert.ToByte(ch[i]);
                }
                COMPort.Write(temp, 0, temp.Length);
                //COMPort.WriteLine(temp.ToString());
            }
        }

    }
}
