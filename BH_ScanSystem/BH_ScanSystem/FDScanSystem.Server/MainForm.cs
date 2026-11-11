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
using FDScanSystem.DAO.Scan;
using FDScanSystem.Helper;
using FDScanSystem.Server;
using FDScanSystem.Server.Device;
using FDScanSystem.Server.Job;
using HosterServices;
using FDScanSystem.Server.Models;
using Interactive.Utility;



namespace FDScanSystem.Server
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
            try
            {
                DeviceConnectionManager.GetInstance();
                AppContext.Log("设备初始化成功");
                //AppContext.Success("设备初始化成功");
            }
            catch (Exception ex)
            {
                AppContext.Log("设备初始化失败");
                //  AppContext.Error("设备初始化失败");
            }

            Thread.Sleep(1000);

            try
            {
                //Send.SendSMS("13917683906", "短信测试");
                 DataReadJob.GetInstance().Start();
                //COMWINDataReadJob.GetInstance().Start();
                // NewDataReadJob.GetInstance().Start();
                 ReConnectJob.GetInstance().Start();
                //DataAddJob.GetInstance().Start();
                AppContext.Log("自动读取服务启动成功");
                //  AppContext.Success("自动读取服务启动成功");
            }
            catch
            {
                AppContext.Log("自动读取服务启动失败");
                //AppContext.Error("自动读取服务启动失败");
            }

            try
            {
                CommandJob.GetInstance().Start();
                // AppContext.Success("命令发送服务启动成功");
                AppContext.Log("命令发送服务启动成功");

            }
            catch
            {
                AppContext.Log("命令发送服务启动失败");
                //AppContext.Error("命令发送服务启动失败");
            }
            //try
            //{
            //    var comdeviceList = ServerDAL.ListComDevice();
            //    foreach (var item in comdeviceList)
            //    {
            //        AppContext.Log(string.Format("\t--ID:{0};DeviceName:{1};Code:{2}", item.ID, item.DeviceName, item.CommunicationCode));
            //        foreach (var subItem in item.Devices)
            //        {
            //            AppContext.Log(string.Format("\t\t----ID:{0};DeviceName:{1};Code:{2}", subItem.ID, subItem.DeviceName, subItem.DeviceIdentity));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    AppContext.Error(ex);

            //}
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

            //     _logWriter.WriteLine("开始监控.........");
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DeviceConnectionManager.GetInstance().WriteStatus(str =>
            {
                AppContext.Log(str);
            });
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DeviceConnectionManager.GetInstance().ReConnect();
        }


    }
}