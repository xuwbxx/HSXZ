using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Configuration;


namespace HosterServices {
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer {
        public ProjectInstaller() {
            InitializeComponent();

            // My code from here
            string serviceName = ConfigurationManager.AppSettings["FormTitle"];
            if (serviceName != null) {
                this.winServiceInstanller.ServiceName = serviceName;
            }
        }

        public string WinServiceName {
            get {
                return this.winServiceInstanller.ServiceName;
            }
        }
    }
}
