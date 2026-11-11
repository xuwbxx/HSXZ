using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FDScanSystem.DAO;
using FDScanSystem.DAO.Scan;
using FDScanSystem.Helper;
using FDScanSystem.Server;
using FDScanSystem.Server.Analysis;
using FDScanSystem.Server.Device;
using Interactive.Utility;

namespace FDScanSystem.Server.Job
{
    public class CommandDispatcher
    {

        static CommandDispatcher _instance;
        static object _obj = new object();

        static int _inWork = 0;

        const int InWorkStatus = 1;
        const int UnInWorkStatus = 0;

        public static CommandDispatcher GetInstance()
        {
            if (_instance == null)
            {
                lock (_obj)
                {
                    if (_instance == null)
                    {
                        _instance = new CommandDispatcher();
                    }
                }
            }

            return _instance;
        }


        protected CommandQueueManager _queueManager;
        protected Timer _timer;
        const int _span = 20000;

        protected CommandDispatcher()
        {
            _timer = new Timer(new TimerCallback(Callback), null, _span, _span);
            _queueManager = Singleton<CommandQueueManager>.Instance;
        }

        public virtual bool Dispatch(MK_Scan_Command command)
        {

            var communcation = ServerDAL.GetComDevice(command.CommunicationDeviceID ?? 0);
            if (communcation != null
                && !string.IsNullOrEmpty(communcation.CommunicationCode))
            {
                var connection = DeviceConnectionManager.GetInstance().Get(communcation.CommunicationCode);
                if (connection != null)
                {
                    AnalysisProtocolBase commandBuilder = COMAnalysisProtocolFactory.Create(connection);
                    var msg = commandBuilder.CommandBuilder.Build(command.ID, command.DeviceIden, command.Command);
                    msg.DataProtocol = (AnalysisProtocolType)communcation.DataProtocol;
                    msg.ConnectionKey = connection.Key;
                    msg.Status = CommandStatus.New;
                    msg.DeviceId = command.DeviceID ?? 0;
                    msg.CommunicationDeviceId = command.CommunicationDeviceID ?? 0;

                    _queueManager.Add(connection.Key, msg);
                    return true;
                }
            }
            return false;

        }

        public virtual bool Dispatch(CommandBase msg)
        {
            DeviceItem item = null;
            if (msg.Status == CommandStatus.InitializeConnection)
            {
                item = DeviceConnectionManager.GetInstance().GetWithoutStatus(msg.ConnectionKey);
                if (item == null)
                {
                    AppContext.Log("发送消息错误: 没有找到对应的连接.windID:" + msg.ConnectionKey);
                    // AppContext.Error("发送消息错误: 没有找到对应的连接.windID:" + msg.ConnectionKey);
                    return false;
                }
            }
            else if (msg.Status == CommandStatus.New)
            {
                item = DeviceConnectionManager.GetInstance().Get(msg.ConnectionKey);
                if (item == null)
                {
                    AppContext.Log("发送消息错误: 没有找到对应的连接.windID:" + msg.ConnectionKey);
                    // AppContext.Error("发送消息错误: 没有找到对应的连接.windID:" + msg.ConnectionKey);
                    return false;
                }
            }
            _queueManager.Add(item.Key, msg);
            return true;
        }



        protected void Send(string connectionKey)
        {
            try
            {
                var commandBase = _queueManager.DequeueCommand(connectionKey);
                if (commandBase == null) return;
                var deviceConnection = DeviceConnectionManager.GetInstance().GetWithoutStatus(connectionKey);
                if (deviceConnection != null)
                {
                    if (commandBase.Sending())
                    {
                        if (deviceConnection.Protocol == 0)
                            deviceConnection.SendSync(commandBase.GetCommand());
                        else
                            deviceConnection.SendByte(commandBase.GetCommand());
                        //   AppContext.Log("发送命令：" + commandBase.GetMsg());
                        LogWriter.Info("发送命令：" + commandBase.GetMsg());
                        commandBase.Sended();
                    }
                }
            }
            catch (Exception ex)
            {
                AppContext.Log("发送消息时出现错误：" + ex.ToString());
                //AppContext.Error("发送消息时出现错误：" + ex.ToString());
            }
        }

        protected void Callback(object state)
        {
            Tick();
        }

        public void SendAllMessage()
        {

            var hashs = _queueManager.ListAllHash();
            foreach (var item in hashs)
            {
                Send(item);
            }
        }


        public void Tick(int tickType = 0)
        {
            try
            {

                if (InWork())
                {
                    // AppContext.Log("CommandDispatcher 出发 Tick,类型:" + (tickType == 0 ? "获取命令触发" : "定时触发"));
                    LogWriter.Info("CommandDispatcher 出发 Tick,类型:" + (tickType == 0 ? "获取命令触发" : "定时触发"));
                    SendAllMessage();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                AppContext.Log(ex.Message);
                //AppContext.Error(ex);
            }
            finally
            {
                _timer.Change(_span, _span);
                OffWork();
            }
        }

        private bool InWork()
        {
            if (_inWork == InWorkStatus)
            {
                return false;
            }
            Interlocked.CompareExchange(ref _inWork, InWorkStatus, UnInWorkStatus);
            return true;
        }

        private void OffWork()
        {
            Interlocked.CompareExchange(ref _inWork, UnInWorkStatus, InWorkStatus);
        }

    }
}
