using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace DataWinService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller progressInstaller;
        private ServiceInstaller serviceInstaller;
        public ProjectInstaller()
        {
            progressInstaller = new ServiceProcessInstaller();
            progressInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = "横江大道数据服务";
            serviceInstaller.DisplayName = "横江大道数据服务";
            serviceInstaller.Description = "进出管理数据生成服务";
            Installers.Add(progressInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
