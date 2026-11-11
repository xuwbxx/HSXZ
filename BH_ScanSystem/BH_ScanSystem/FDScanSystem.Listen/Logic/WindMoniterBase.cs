using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace FDScanSystem.Listen.Logic {
    /// <summary>
    /// 监控基类
    /// </summary>
    public abstract class WindMoniterBase : IDisposable {
        private int _timeSpan;

        private Timer _timer;

        protected WindMoniterBase()
            : this(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["readtime"]))
        {

            //_timeSpan = 10000;
            //_timer = new Timer(new TimerCallback(Callback), null, Timeout.Infinite, Timeout.Infinite);
            

        }

        protected WindMoniterBase(int timespan)
        {
            _timeSpan = timespan;
            _timer = new Timer(new TimerCallback(Callback), null, Timeout.Infinite, Timeout.Infinite);
        }


        public void Start() {
            AppContext.Log("定时命令扫描线程启动......");
            _timer.Change(_timeSpan, _timeSpan);
        }


        protected void Callback(object state) {
            try {
                AppContext.Log("WindMoniterBase 触发 Tick");
                PostData();
                //  _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            catch (Exception ex) {
                AppContext.Error(ex);
            }
            finally {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer.Change(_timeSpan, 0);
            }
        }

        protected abstract void PostData();


        public void Dispose() {
            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);

                _timer.Dispose();
            }
        }
    }
}
