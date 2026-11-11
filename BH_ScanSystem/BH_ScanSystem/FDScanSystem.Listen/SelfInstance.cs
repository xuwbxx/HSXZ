using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using HosterServices;

namespace FDScanSystem.Listen {
    internal class SelfInstaller {
        public static void Install() {
            TransactedInstaller ti = new TransactedInstaller();
            ProjectInstaller pi = new ProjectInstaller();
            ti.Installers.Add(pi);
            string path = string.Format("/assemblypath={0}",
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            string[] cmdLine = new string[] { path };
            InstallContext ctx = new InstallContext(null, cmdLine);
            ti.Context = ctx;
            ti.Install(new System.Collections.Hashtable());
        }

        public static void Uninstall() {
            TransactedInstaller ti = new TransactedInstaller();
            ProjectInstaller pi = new ProjectInstaller();
            ti.Installers.Add(pi);
            string path = string.Format("/assemblypath={0}",
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            string[] cmdLine = { path };
            InstallContext ctx = new InstallContext(null, cmdLine);
            ti.Context = ctx;
            ti.Uninstall(null);
        }
    }
}
