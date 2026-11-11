using Business.DataHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace DataWinService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            try
            {

                DataCreateService service = new DataCreateService();

                service.CreatePersonInAndOutData();

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new AutoService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                LogUtils.Instance.LogText(ex.ToString());
            }

        }
    }
}
