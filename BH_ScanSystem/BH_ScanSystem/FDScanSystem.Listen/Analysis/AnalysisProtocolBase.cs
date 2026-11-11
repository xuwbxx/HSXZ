using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Listen.Logic;
using FDScanSystem.Listen.Analysis.DataReceiver;
using TcpTransmit.Core;
using FDScanSystem.Listen.Device;

using Interactive.Utility;

namespace FDScanSystem.Listen.Analysis
{
    public enum AnalysisProtocolType
    {
        /// <summary>
        /// MCU32
        /// </summary>
        MCU = 0,

        /// <summary>
        /// HUAHENG数字式
        /// </summary>
        HuaHeng = 1,
        /// <summary>
        /// 自定义
        /// </summary>
        Customer = 5
    }


    public abstract class AnalysisProtocolBase
    {
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
        public virtual void OnReceived(BaseSocketConnection e, byte[] receiveData)
        {

        }

        /// <summary>
        /// 链接断开时
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnDisconnected(BaseSocketConnection e)
        {
            DeviceItem item = DeviceConnectionManager.GetInstance().Get(e.ConnectionId);
            if (item != null)
            {
                Singleton<CommandQueueManager>.Instance.Rollback(item.Key);

                DeviceConnectionManager.GetInstance().Disconnect(item);
            }
        }

        /// <summary>
        /// 创建连接时
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnConnected(BaseSocketConnection e)
        {

        }

    }
}
