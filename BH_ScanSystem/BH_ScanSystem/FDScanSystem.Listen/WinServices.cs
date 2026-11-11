using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using FDScanSystem.Helper;

namespace HosterServices {
    partial class WinServices : ServiceBase {
        public WinServices() {
            InitializeComponent();
        }


        protected override void OnStart(string[] args) {
            ServiceStart();
        }

        protected override void OnStop() {
            ServiceStop();
        }

        internal static void ServiceStart() {
            //RefreshMemDB();
            //DisplayRemotingInfo();
            //  DisplayWcfInfo(Helper.WcfServiceAssemblyNames);
            //  GatewayClient.Instance.RegisterMessageHandler(ReceiveMessageHandler);
            //   GatewayClient.Instance.RegisterMessageHandler(GoldensunClient.BattleProxy.Instance.OnReceiveHandler);
            //GoldensunClient.BattleProxy.Instance.RegisterBattleCreateEventHandler(BattleSvc.BattleCreateFinished);
            //GoldensunClient.BattleProxy.Instance.RegisterBattleCreateEventHandler(MainBattleSvc.DungeonBattleCreateCallback);
            //GoldensunClient.BattleProxy.Instance.RegisterBattleFinishedEventHandler(MainBattleSvc.DungeonBattleFinishedCallback);
            //BrushMonsterInterface.Instance.Start();
            //GatewayClient.Instance.Start();
            //StaticDataIniter.Init();
            LogWriter.Info( "程序启动");
            Console.WriteLine("This is log area.");

        }

        //private static void ReceiveMessageHandler(object sender, TcpConnectionReceiveEventArgs e) {
        //    return;
        //}


        internal static void ServiceStop() {
            Console.WriteLine("In ExitApp");
        }



        internal static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Exception ex = (Exception)(e.ExceptionObject);
            LogWriter.Error(Guid.Empty, 3, "CurrentDomain_UnhandledException", ex);
        }

        internal static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
            Exception ex = e.Exception;
            LogWriter.Error(Guid.Empty, 3, "Application_ThreadException", ex);
        }
    }
}
