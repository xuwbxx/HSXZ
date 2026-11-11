namespace FDScanSystem.Server
{
    partial class formMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.iconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.iconMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.mainMenuItemFunc = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuItemCopyLog = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuItemService = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuItemInstall = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuItemUninstall = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListTaskTree = new System.Windows.Forms.ImageList(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.tCPConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.iconMenu.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.iconMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "FDScanSystem.Server";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // iconMenu
            // 
            this.iconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iconMenuItemExit});
            this.iconMenu.Name = "iconMenu";
            this.iconMenu.Size = new System.Drawing.Size(113, 26);
            this.iconMenu.Text = "iconMenu";
            // 
            // iconMenuItemExit
            // 
            this.iconMenuItemExit.Name = "iconMenuItemExit";
            this.iconMenuItemExit.Size = new System.Drawing.Size(112, 22);
            this.iconMenuItemExit.Text = "Exit(&X)";
            this.iconMenuItemExit.Click += new System.EventHandler(this.iconMenuItemExit_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuItemFunc,
            this.mainMenuItemService,
            this.tCPConsoleToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(792, 25);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "mainMenu";
            // 
            // mainMenuItemFunc
            // 
            this.mainMenuItemFunc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuItemCopyLog,
            this.toolStripSeparator1,
            this.mainMenuItemExit});
            this.mainMenuItemFunc.Name = "mainMenuItemFunc";
            this.mainMenuItemFunc.Size = new System.Drawing.Size(60, 21);
            this.mainMenuItemFunc.Text = "Func(&F)";
            // 
            // mainMenuItemCopyLog
            // 
            this.mainMenuItemCopyLog.Name = "mainMenuItemCopyLog";
            this.mainMenuItemCopyLog.Size = new System.Drawing.Size(148, 22);
            this.mainMenuItemCopyLog.Text = "Copy Log(&C)";
            this.mainMenuItemCopyLog.Click += new System.EventHandler(this.mainMenuItemCopyLog_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // mainMenuItemExit
            // 
            this.mainMenuItemExit.Name = "mainMenuItemExit";
            this.mainMenuItemExit.Size = new System.Drawing.Size(148, 22);
            this.mainMenuItemExit.Text = "Exit(&X)";
            this.mainMenuItemExit.Click += new System.EventHandler(this.mainMenuItemExit_Click);
            // 
            // mainMenuItemService
            // 
            this.mainMenuItemService.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuItemInstall,
            this.mainMenuItemUninstall});
            this.mainMenuItemService.Name = "mainMenuItemService";
            this.mainMenuItemService.Size = new System.Drawing.Size(76, 21);
            this.mainMenuItemService.Text = "Service(&S)";
            // 
            // mainMenuItemInstall
            // 
            this.mainMenuItemInstall.Name = "mainMenuItemInstall";
            this.mainMenuItemInstall.Size = new System.Drawing.Size(142, 22);
            this.mainMenuItemInstall.Text = "Install(&I)";
            this.mainMenuItemInstall.Click += new System.EventHandler(this.mainMenuItemInstall_Click);
            // 
            // mainMenuItemUninstall
            // 
            this.mainMenuItemUninstall.Name = "mainMenuItemUninstall";
            this.mainMenuItemUninstall.Size = new System.Drawing.Size(142, 22);
            this.mainMenuItemUninstall.Text = "Uninstall(&U)";
            this.mainMenuItemUninstall.Click += new System.EventHandler(this.mainMenuItemUninstall_Click);
            // 
            // imageListTaskTree
            // 
            this.imageListTaskTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageListTaskTree.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListTaskTree.TransparentColor = System.Drawing.Color.Fuchsia;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageLog);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 25);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(792, 541);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.richTextBoxLog);
            this.tabPageLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(784, 515);
            this.tabPageLog.TabIndex = 0;
            this.tabPageLog.Text = "Log";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxLog.Size = new System.Drawing.Size(778, 509);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.TabStop = false;
            this.richTextBoxLog.Text = "";
            this.richTextBoxLog.Enter += new System.EventHandler(this.richTextBoxLog_Enter);
            // 
            // tCPConsoleToolStripMenuItem
            // 
            this.tCPConsoleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.tCPConsoleToolStripMenuItem.Name = "tCPConsoleToolStripMenuItem";
            this.tCPConsoleToolStripMenuItem.Size = new System.Drawing.Size(93, 21);
            this.tCPConsoleToolStripMenuItem.Text = "TCP Console";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "查看连接数";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "重新连接";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "formMain";
            this.Text = "FDScanSystem.Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formMain_FormClosing);
            this.Load += new System.EventHandler(this.formMain_Load);
            this.iconMenu.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageLog.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip iconMenu;
        private System.Windows.Forms.ToolStripMenuItem iconMenuItemExit;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem mainMenuItemFunc;
        private System.Windows.Forms.ToolStripMenuItem mainMenuItemExit;
        private System.Windows.Forms.ImageList imageListTaskTree;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageLog;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.ToolStripMenuItem mainMenuItemCopyLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mainMenuItemService;
        private System.Windows.Forms.ToolStripMenuItem mainMenuItemInstall;
        private System.Windows.Forms.ToolStripMenuItem mainMenuItemUninstall;
        private System.Windows.Forms.ToolStripMenuItem tCPConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}