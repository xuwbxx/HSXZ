using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;

using HosterServices;
using XPDScanSystem.Services;
using TcpTransmit.Core.Server;
using TcpTransmit.Core;
using Interactive.Utility;
using FDScanSystem.Helper;
using FDScanSystem.Client;
using TcpTransmit.Core.Client;
using FDScanSystem.Client.Logic.ThreadManager;
using FDScanSystem.Client.Logic.MsgSender;



namespace FDScanSystem.Client
{
    public partial class formMain : Form
    {
        private FormLogHelper _logWriter = null;
        private BalloonTipManager _balloonTipManager = null;
        private FormWindowState _windowState;



        public formMain()
        {
            InitializeComponent();

            this.Disposed += new EventHandler(formMain_Disposed);
            log4net.Config.XmlConfigurator.Configure();
            string settingTitle = ConfigurationManager.AppSettings["FormTitle"];
            if (settingTitle != null)
            {
                this.Text = settingTitle.Trim();
                this.notifyIcon.Text = this.Text;
            }
            string settingTimeout = ConfigurationManager.AppSettings["BalloonTipTimeoutSeconds"];
            if (settingTimeout != null)
            {
                _balloonTipManager = new BalloonTipManager(this.notifyIcon, int.Parse(settingTimeout));
            }
            else
            {
                _balloonTipManager = new BalloonTipManager(this.notifyIcon);
            }
            _logWriter = new FormLogHelper(this.richTextBoxLog);

            Console.SetOut(_logWriter);
            Console.SetError(_logWriter);
            string path = System.Windows.Forms.Application.StartupPath + "\\";
            AppContext.Initialize(_logWriter);


            if (AppContext.IsStartedProcess())
            {

                MessageBox.Show("已经有一个进程正在运行,监测服务只允许一个进程", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            //Singleton<ServicesStatuesMachine>.Instance.Initialize();

            //DefaultWindMoniter
            //    .GetInstance()
            //    .Start();

            //DefaultDataRead
            //    .GetInstance()
            //    .Start();

            //DataCalculateWriter
            //    .GetInstance()
            //    .Start();

        }

        void formMain_Disposed(object sender, EventArgs e)
        {
            ThreadManager.Stop();
            //Singleton<ServicesStatuesMachine>
            //    .Instance
            //    .Dispose();

            //DefaultWindMoniter
            // .GetInstance()
            // .Dispose();

            //DefaultDataRead
            //    .GetInstance()
            //    .Dispose();
            HostManager.Instance["Farm"].Stop();
            if (_logWriter != null)
            {
                _logWriter.Dispose();
            }
            if (_balloonTipManager != null)
            {
                _balloonTipManager.Dispose();
            }
        }

        //~formMain() {

        //}

        private void ExitApp()
        {
            ThreadManager.Stop();

            WinServices.ServiceStop();
            notifyIcon.Visible = false;

            Application.Exit();
        }

        private void mainMenuItemExit_Click(object sender, EventArgs e)
        {
            ExitApp();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                ShowForm();
            }
            else
            {
                HideForm();
            }
        }

        private void ShowForm()
        {
            this.Show();
            this.WindowState = _windowState;
        }

        private void HideForm()
        {
            _windowState = this.WindowState;
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
        }

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                HideForm();
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MINIMIZE = 0xF020;

            switch (m.Msg)
            {
                case (WM_SYSCOMMAND):
                    if ((int)m.WParam == SC_MINIMIZE)
                    {
                        HideForm();
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void iconMenuItemExit_Click(object sender, EventArgs e)
        {
            ExitApp();
        }

        private void richTextBoxLog_Enter(object sender, EventArgs e)
        {
            this.FindForm().SelectNextControl(this, true, true, true, false);
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadCallback));
        }

        private void LoadCallback(object state)
        {

            //AppContext.Log("jjkk");
            WinServices.ServiceStart();

            ThreadManager.Start();
            Thread.Sleep(1000);
            try
            {
                //    DeviceConnectionManager.GetInstance();
                HostManager.Instance["Farm"].Start();
                //    Host host = Host.DefaultHost;
                //    host.Start();
            }
            catch (Exception ex)
            {
                //_logWriter.Write(ex.ToString());
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    CommandDispatcher.GetInstance().Dispatch(new SendDataMsg(new ScanThreadContext(new ScanDataTaskConfig()
            //    {
            //        DeviceId = i,
            //        DeviceIdentity = "00001",
            //        IsAlert = false,
            //        ScanTime = 1000,
            //    }), @"E:\工作\中交\风电监测系统\Source\FDScanSystem\FDScanSystem.Client\ScanData\1\150814043118380.dat"));
            //}
            AppContext.Log("初始化完成");
        }

        private void mainMenuItemCopyLog_Click(object sender, EventArgs e)
        {
            richTextBoxLog.SelectAll();
            richTextBoxLog.Copy();
        }

        private void mainMenuItemInstall_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectInstaller pi = new ProjectInstaller();
                if (MessageBox.Show(this, "确定要安装" + pi.WinServiceName + "吗？", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SelfInstaller.Install();
                    MessageBox.Show(this, "安装" + pi.WinServiceName + "成功", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "安装失败：" + ex.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mainMenuItemUninstall_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectInstaller pi = new ProjectInstaller();
                if (MessageBox.Show(this, "确定要卸载" + pi.WinServiceName + "吗？", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SelfInstaller.Uninstall();
                    MessageBox.Show(this, "卸载" + pi.WinServiceName + "成功", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "卸载失败：" + ex.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void viewMsgtool_Click(object sender, EventArgs e)
        {
            //var keys = Singleton<CommandQueueManager>.Instance.ListAllHash();
            //AppContext.Log("\r\n--------------------------------------");
            //AppContext.Log(" " + keys.Count + "个队列!\r\n");
            //CommandQueue queue = null;
            //foreach (var keyItem in keys) {
            //    queue = Singleton<CommandQueueManager>.Instance[keyItem];
            //    if (queue != null) {
            //        AppContext.Log(string.Format("============开始链接名称:{0}=================", keyItem));

            //        if (queue.CurrentProcess != null) {
            //            AppContext.Log("\r\n===========开始当前正在执行的命令====================");
            //            AppContext.Log(string.Format("\r\n=== ID:{0}     ", queue.CurrentProcess.ID));
            //            AppContext.Log(string.Format("\r\n=== Identity:{0}     ", queue.CurrentProcess.Identity));
            //            AppContext.Log(string.Format("\r\n=== Status:{0}     ", queue.CurrentProcess.Status));
            //            AppContext.Log(string.Format("\r\n=== Type:{0}     ", queue.CurrentProcess.Type));
            //            AppContext.Log(string.Format("\r\n=== CommandText:{0}     ", queue.CurrentProcess.CommandText));
            //            AppContext.Log(string.Format("\r\n=== Key:{0}     ", queue.CurrentProcess.ConnectionKey));
            //            AppContext.Log(string.Format("\r\n=== DataProtocol:{0}     ", queue.CurrentProcess.DataProtocol));
            //            AppContext.Log("\r\n===========结束当前正在执行的命令====================");
            //        }
            //        CommandBase[] arr = queue.CmdQueue.ToArray();
            //        foreach (var subItem in arr) {
            //            AppContext.Log(string.Format("\r\n=== ID:{0}     ", subItem.ID));
            //            AppContext.Log(string.Format("\r\n=== Identity:{0}     ", subItem.Identity));
            //            AppContext.Log(string.Format("\r\n=== Status:{0}     ", subItem.Status));
            //            AppContext.Log(string.Format("\r\n=== Type:{0}     ", subItem.Type));
            //            AppContext.Log(string.Format("\r\n=== CommandText:{0}     ", subItem.CommandText));
            //            AppContext.Log(string.Format("\r\n=== Key:{0}     ", subItem.ConnectionKey));
            //            AppContext.Log(string.Format("\r\n=== DataProtocol:{0}     ", subItem.DataProtocol));
            //        }
            //        AppContext.Log(string.Format("============结束链接名称:{0}=================", keyItem));
            //    }
            //}
        }
    }
}