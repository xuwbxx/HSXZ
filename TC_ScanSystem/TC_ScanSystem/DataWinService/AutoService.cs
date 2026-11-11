using Business.DataHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace DataWinService
{
    partial class AutoService : ServiceBase
    {
        System.Timers.Timer t = null;
        //15分钟
        int timespan = 1000 * 60 * 15;
        public AutoService()
        {
            InitializeComponent();

            t = new System.Timers.Timer(timespan);
            //设置是执行一次（false）还是一直执行(true)； 
            t.AutoReset = true;
            //是否执行System.Timers.Timer.Elapsed事件； 
            t.Enabled = true;
            //到达时间的时候执行事件(theout方法)； 
            t.Elapsed += new System.Timers.ElapsedEventHandler(theout);
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            LogUtils.Instance.LogText("横江大道服务开始");

            DataCreateService service = new DataCreateService();
            service.CreatePersonInAndOutData();
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            LogUtils.Instance.LogText("横江大道服务结束");
        }

        protected void theout(object source, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour <= 18)
                {
                    DataCreateService service = new DataCreateService();
                    service.CreatePersonInAndOutData();

                    LogUtils.Instance.LogText("生成数据成功！");
                }

               

            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogText("服务出错：" + ex.ToString());
            }
        }
    }
}
