using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Interactive.Utility;
using FDScanSystem.DAO;

namespace FDScanSystem.Listen.Logic
{
    /// <summary>
    /// 两次循环的命令
    /// </summary>
    public class TwoWayCommand : CommandBase
    {
        public override int Type
        {
            get { return 3; }
        }

        /// <summary>
        /// 发送次数
        /// </summary>
        public int SendCount { get; set; }

        public override bool Sending()
        {
            return true;
        }

        public override void Sended()
        {
            base.Sended();
            this.SendCount++;
        }

        public void Offline()
        {
            var db = DataContextFactory.Create();
            if (this.ID > 0)
            {
                var command = db.FD_Command.FirstOrDefault(u => u.ID == ID);
                if (command != null)
                {
                    command.Status = (int)CommandStatus.SendOutTimes;
                    db.SubmitChanges();

                    Singleton<CommandQueueManager>.Instance.CommandIsDone(this.ConnectionKey);
                }
            }
        }
    }
}
