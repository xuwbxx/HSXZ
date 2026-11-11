using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interactive.Utility;
using FDScanSystem.Listen.Analysis;
using FDScanSystem.DAO;


namespace FDScanSystem.Listen.Logic
{

    public enum CommandStatus
    {
        New = 0,//- 新增命令
        Reset = 1,// - 重置命令
        Sended = 2,// - 已发送命令
        WaitSending = 3,//- 待发送命令
        SendOutTimes = 10,//发送超过预定的次数没有相应
        InitializeConnection = 100 //初始化命令

    }

    public abstract class CommandBase
    {

        public string ConnectionKey { get; set; }

        public string CommandText { get; set; }

        public int DeviceId { get; set; }

        public string Identity { get; set; }

        public AnalysisProtocolType DataProtocol { get; set; }

        public int ID { get; set; }

        public int PointId { get; set; }

        public CommandStatus Status { get; set; }

        public abstract int Type { get; }

        public string GetMsg()
        {
            return string.Format("SN:{0}-Cmd:{1}-Status:{2}", Identity, CommandText, Status);
        }

        public string GetCommand()
        {

            return string.Format(CommandText + "\r\n", Identity);
        }

        public virtual byte[] GetCommandBytes()
        {
            return Encoding.ASCII.GetBytes(GetCommand());
        }


        public virtual bool Sending()
        {
            if (_sendCount > 5)
            {
                var db = DataContextFactory.Create();
                if (this.ID > 0)
                {
                    //    AppContext.Error(string.Format("{0}--{1}--{2}", ID, Identity, _sendCount));
                    var command = db.FD_Command.FirstOrDefault(u => u.ID == ID);
                    if (command != null)
                    {
                        command.Status = (int)CommandStatus.SendOutTimes;
                        db.SubmitChanges();

                        Singleton<CommandQueueManager>.Instance.CommandIsDone(this.ConnectionKey);
                        return false;
                    }
                }
            }
            return true;
        }

        private int _sendCount = 1;
        public virtual void Sended()
        {
            var db = DataContextFactory.Create();
            if (this.ID > 0)
            {
                var command = db.FD_Command.FirstOrDefault(u => u.ID == ID);
                if (command != null)
                {
                    command.Status = (int)CommandStatus.Sended;


                    db.SubmitChanges();
                }
            }
            _sendCount++;

            //      AppContext.Log("发送次数 ：" + _sendCount);
        }

    }
}
