using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Server.Job {
    public class CommandQueue {

        /// <summary>
        /// 当前正在之情的命令
        /// </summary>
        public CommandBase CurrentProcess { get; set; }


        /// <summary>
        /// 命令队列
        /// </summary>
        public Queue<CommandBase> CmdQueue { get; set; }

        /// <summary>
        /// 状态 
        /// 0 没有等待回复的指令
        /// 1 正在等待回复
        /// 2 完成
        /// </summary>
        public int Status { get; set; }

    }
}
