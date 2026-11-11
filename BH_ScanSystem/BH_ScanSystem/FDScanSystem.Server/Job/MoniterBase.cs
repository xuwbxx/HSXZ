using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FDScanSystem.Helper;

namespace FDScanSystem.Server.Job
{
    /// <summary>
    /// 监控基类
    /// </summary>
    public abstract class MoniterBase : IDisposable
    {
        protected int _timeSpan;

        private Timer _timer;

        protected MoniterBase()
            : this(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["readtime"]))
        {


        }

        protected MoniterBase(int timespan)
        {
            _timeSpan = timespan;
            _timer = new Timer(new TimerCallback(Callback), null, Timeout.Infinite, Timeout.Infinite);
        }

        protected void Callback(object state)
        {
            try
            {

                PostData();
                //  _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                AppContext.Log(ex.Message);
                // AppContext.Error(ex);
            }
            finally
            {
                //_timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer.Change(_timeSpan, _timeSpan);
            }
        }

        public void Start()
        {
            AppContext.Log("自动定时读取模块启动");
            //LogWriter.Info("自动定时读取模块启动");
            _timer.Change(_timeSpan, _timeSpan);
        }

        protected abstract void PostData();

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);

                _timer.Dispose();
            }


        }
    }
}
