using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Server.Analysis.DataReceiver;
using FDScanSystem.Server.Device;
using FDScanSystem.Server.Job;
using Interactive.Utility;

namespace FDScanSystem.Server.Analysis
{
    public enum AnalysisProtocolType
    {
        /// <summary>
        /// MCU32
        /// </summary>
        MCU = 0,

        HUAHENG = 1
    }


    public abstract class AnalysisProtocolBase
    {
        List<byte> buffer = new List<byte>(4096);
        byte[] binary_data_1 = new byte[9];//AA 44 05 01 02 03 04 05 EA


        public AnalysisProtocolBase() { }

        /// <summary>
        /// 命令创建器
        /// </summary>
        public abstract ICommandBuilder CommandBuilder { get; }

        /// <summary>
        /// 协议类型
        /// </summary>
        public AnalysisProtocolType Type { get; set; }


        /// <summary>
        /// 处理类类表
        /// </summary>
        public abstract List<IDataReceive> Receiver { get; }

        /// <summary>
        /// 默认处理类
        /// </summary>
        public abstract IDataReceive DefaultReceiver { get; }


        /// <summary>
        /// 收到消息时
        /// </summary>
        /// <param name="e"></param>
        /// <param name="receiveData"></param>
        public virtual void OnReceived(DeviceItem e)
        {
            try
            {
                var comm = e.COMPort;
                int n = comm.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致
                byte[] buf = new byte[n];//声明一个临时数组存储当前来的串口数据
                //  received_count += n;//增加接收计数
                comm.Read(buf, 0, n);//读取缓冲数据
                OnReceived(e, buf);
            }
            catch (Exception ex)
            {
                AppContext.Log(ex.Message);
              //  AppContext.Error(ex);
            }

        }


        public abstract void OnReceived(DeviceItem sender, byte[] receiveData);


        /// <summary>
        /// 创建连接时
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnConnected()
        {

        }

    }
}
