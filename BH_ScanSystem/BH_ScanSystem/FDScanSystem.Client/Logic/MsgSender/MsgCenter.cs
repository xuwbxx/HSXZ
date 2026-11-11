using FDScanSystem.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TcpTransmit.Core.Client;

namespace FDScanSystem.Client.Logic.MsgSender
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


        protected Queue<ClientToServerMsgBase> _queueManager;
        protected ClientToServerMsgBase _Current;



        protected Timer _timer;
        const int _span = 30000;

        protected CommandDispatcher()
        {
            _timer = new Timer(new TimerCallback(Callback), null, _span, _span);

            _queueManager = new Queue<ClientToServerMsgBase>(1000);
        }

        public ClientToServerMsgBase CurrentCommand
        {
            get
            {
                return _Current;
            }
        }

        public void Done(string identity)
        {
            if (_Current != null && _Current.Identity.Equals(identity, StringComparison.CurrentCultureIgnoreCase))
            {
                Done();
            }
            else
            {
                Done();
                throw new ArgumentException(string.Format("命令未收到相应返回值,内容为:{0}", _Current != null ? _Current.Command : "没有内容"));
            }
        }


        public void Done()
        {
            if (_Current != null) _Current.Callback();
            _Current = null;
            Callback(null);
        }

        public void Offline()
        {
            _Current = null;
            Callback(null);
        }

        public virtual bool Dispatch(ClientToServerMsgBase msg)
        {
            _queueManager.Enqueue(msg);
            return true;
        }


        protected void Callback(object state)
        {
            Tick();
        }

        public void Tick(int tickType = 0)
        {
            try
            {

                if (InWork())
                {
                    if (_Current != null && _Current.GetTickCount() >= 10)
                    {
                        Offline();
                    }
                    AppContext.Log("CommandDispatcher 出发 Tick,类型:" + (tickType == 0 ? "获取命令触发" : "定时触发"));
                    if (_Current == null)
                    {
                        lock (_queueManager)
                        {
                            if (_Current == null && _queueManager.Count > 0)
                            {
                                _Current = _queueManager.Dequeue();
                            }
                        }
                    }
                    if (_Current != null)
                    {
                        HostManager.Instance["Farm"].BeginSend(_Current.ToSerializeMsg());
                        _Current.Sended();
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                AppContext.Error(ex);
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
