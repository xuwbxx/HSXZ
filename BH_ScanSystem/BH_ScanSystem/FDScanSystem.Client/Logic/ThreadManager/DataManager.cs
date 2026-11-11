using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using FDScanSystem.Client.Logic.MsgSender;
using FDScanSystem.Client.Logic.ThreadManager;
using FDScanSystem.Protocol.ScanData;

namespace FDScanSystem.Client.Logic
{

    public interface IDataManager
    {
        void Append(ScanThreadContext context, Dictionary<long, double[]> data);

        void Append(ScanThreadContext context, Dictionary<long, double> data);
    }



    public class DataManager : IDataManager, IThead
    {
        int _span = 10 * 1000;

        static Queue<ScanDataQueueInfo> _InnerQueue = new Queue<ScanDataQueueInfo>(100);
        Timer _InnerTimer;

        static DataManager _instance;
        static object _lock = new object();

        public static DataManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DataManager();
                    }
                }
            }
            return _instance;
        }

        protected DataManager()
        {
            _InnerTimer = new Timer(new TimerCallback(TimerCallback));
            _InnerTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }


        protected void TimerCallback(object state)
        {

            try
            {
                Queue<ScanDataQueueInfo> newQueue = null;
                lock (_InnerQueue)
                {
                    newQueue = new Queue<ScanDataQueueInfo>(_InnerQueue);
                    _InnerQueue.Clear();
                }

                while (newQueue.Count > 0)
                {
                    var info = newQueue.Dequeue();
                    Process(info);
                }

            }
            catch (Exception ex)
            {
                AppContext.Error(ex);
            }
            finally
            {
                _InnerTimer.Change(_span, _span);
            }
        }

        private void Process(ScanDataQueueInfo info)
        {

            string fileName = FileNameCreator.GetInstance().CreateFile(info.Context.DeviceId, info.Context.StartDateTime);
            if (File.Exists(fileName))
            {
                if (info.Context.IsAlert)
                {

                    ScanDataResolver.ToFile(info.Context.DeviceId, info.Context.AlertPointId, info.Context.AlertValue, info.Data2, fileName);
                }
                else
                {
                    ScanDataResolver.ToFile(info.Context.DeviceId, info.Data, fileName);
                }

                AppContext.Success(string.Format("Send MSG : {0}-{1}", info.Context.DeviceId, fileName));

            }
            else
            {
                AppContext.Success(string.Format("{0}-{1} File Not Found"));
            }
            CommandDispatcher.GetInstance().Dispatch(new SendDataMsg(info.Context, fileName));
        }

        public void Append(ScanThreadContext context, Dictionary<long, double[]> data)
        {
            lock (_InnerQueue)
            {
                _InnerQueue.Enqueue(new ScanDataQueueInfo
                {
                    Context = context,
                    Data = data
                });
            }
        }

        public void Append(ScanThreadContext context, Dictionary<long, double> data)
        {
            lock (_InnerQueue)
            {
                _InnerQueue.Enqueue(new ScanDataQueueInfo
                {
                    Context = context,
                    Data2 = data
                });
            }
        }

        public void Start()
        {
            _InnerTimer.Change(_span, _span);
        }

        public void Stop()
        {
            _InnerTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public string ThreadName
        {
            get { return ThreadManager.ThreadManager.DataToFileThread; }
        }
    }

    public class ScanDataQueueInfo
    {

        public ScanThreadContext Context { get; set; }

        public Dictionary<long, double[]> Data { get; set; }

        public Dictionary<long, double> Data2 { get; set; }



    }
}
