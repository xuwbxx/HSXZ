using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FDScanSystem.DAO;
using FDScanSystem.Helper;
using FDScanSystem.Listen;
using FDScanSystem.Listen.CommandBuilder.Custome;
using FDScanSystem.Listen.Logic;
using FDScanSystem.Protocol;
using FDScanSystem.Protocol.ScanConfig;
using HosterServices;
using Interactive.Utility;
using TcpTransmit.Core;
using TcpTransmit.Core.Server;



namespace FDScanSystem.Listen
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

            //try
            //{
            //    //    DeviceConnectionManager.GetInstance();

            //    Host host = Host.DefaultHost;
            //    host.Start();
            //}
            //catch (Exception ex)
            //{
            //    _logWriter.Write(ex.ToString());
            //}

            // UDPWIndMoniter.GetInstance().Start();
             HSReadMonitor.GetInstance().Start();
            //OtherReadMonitor read = new OtherReadMonitor();
            //read.PostData();
            OtherReadMonitor.GetInstance().Start();
            //DefaultWindMoniter
            //    .GetInstance()
            //    .Start();

            //DefaultDataRead
            //    .GetInstance()
            //    .Start();

            //DataCalculateWriter
            //    .GetInstance()
            //    .Start();

            _logWriter.WriteLine("开始监控.........");
        }

        void formMain_Disposed(object sender, EventArgs e)
        {
            //Singleton<ServicesStatuesMachine>
            //    .Instance
            //    .Dispose();

            //DefaultWindMoniter
            // .GetInstance()
            // .Dispose();

            //DefaultDataRead
            //    .GetInstance()
            //    .Dispose();

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
            WinServices.ServiceStop();
            notifyIcon.Visible = false;

            Application.Exit();
            Environment.Exit(0);
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
            WinServices.ServiceStart();
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

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var host = Host.DefaultHost;

            BaseSocketConnection[] infos = host.GetSocketConnections();

            AppContext.Log("\r\n--------------------------------------");
            AppContext.Log(" " + infos.Length + "个客户端!\r\n");

            foreach (BaseSocketConnection info in infos)
            {
                AppContext.Log(" Connection Id " + info.ConnectionId);
                AppContext.Log(" Ip Address " + info.RemoteEndPoint);
                //AppContext.Log(" Ip Local Address " + info.LocalEndPoint);

                AppContext.Log("--------------------------------------");
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

        private void tsmTest_Click(object sender, EventArgs e)
        {

            ClientMsgEntity msg = new ClientMsgEntity();
            msg.Command = ClientMsgCode.ServerToClient_SetPeriod;
            ScanSetting settingInfo = new ScanSetting();
            settingInfo.Begin = new DateTime(2015, 1, 1, 1, 1, 1);
            settingInfo.Interval = 5 * 60 * 1000;
            settingInfo.ScanTime = 60 * 1000;
            msg.AddParameter("Content", ZJFramework10.Serialize.Helper.SerializeString(settingInfo));

            var db = DataContextFactory.Create();
            db.FD_Command.InsertOnSubmit(new FD_Command
            {
                CommunicationDeviceId = 2,
                CommunicationIden = "192.168.1.110",
                CreateDate = DateTime.Now,
                DeviceId = 4,
                DeviceIden = "",
                IsSystem = 1,
                LastPointId = 0,
                Result = string.Empty,
                Status = 0,
                Command = ZJFramework10.Serialize.Helper.SerializeString(msg)
            });
            db.SubmitChanges();
        }

        //   private void Test
    }
}