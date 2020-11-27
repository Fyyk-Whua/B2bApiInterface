namespace B2bApiInterface
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.icnMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiStart = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.ToptsmWin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel11 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel10 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tsFormsTool = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnVisible = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.rtxLog = new System.Windows.Forms.RichTextBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.cmsMain.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.MainStatusStrip.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tsFormsTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // icnMain
            // 
            this.icnMain.ContextMenuStrip = this.cmsMain;
            this.icnMain.Icon = ((System.Drawing.Icon)(resources.GetObject("icnMain.Icon")));
            this.icnMain.Text = "B2B电商平台同步接口";
            this.icnMain.Visible = true;
            this.icnMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.icnMain_MouseDoubleClick);
            // 
            // cmsMain
            // 
            this.cmsMain.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiStart,
            this.tsmiStop,
            this.toolStripMenuItem3,
            this.tsmiCloseWindow});
            this.cmsMain.Name = "cmsMain";
            this.cmsMain.Size = new System.Drawing.Size(147, 92);
            // 
            // tsmiStart
            // 
            this.tsmiStart.Name = "tsmiStart";
            this.tsmiStart.Size = new System.Drawing.Size(146, 22);
            this.tsmiStart.Text = "启动";
            this.tsmiStart.Click += new System.EventHandler(this.tsmiStart_Click);
            // 
            // tsmiStop
            // 
            this.tsmiStop.Name = "tsmiStop";
            this.tsmiStop.Size = new System.Drawing.Size(146, 22);
            this.tsmiStop.Text = "停止";
            this.tsmiStop.Click += new System.EventHandler(this.tsmiStop_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItem3.Text = "--------------";
            // 
            // tsmiCloseWindow
            // 
            this.tsmiCloseWindow.Name = "tsmiCloseWindow";
            this.tsmiCloseWindow.Size = new System.Drawing.Size(146, 22);
            this.tsmiCloseWindow.Text = "退出";
            this.tsmiCloseWindow.Click += new System.EventHandler(this.tsmiCloseWindow_Click);
            // 
            // MainMenu
            // 
            this.MainMenu.AllowMerge = false;
            this.MainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToptsmWin});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.MdiWindowListItem = this.ToptsmWin;
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(722, 25);
            this.MainMenu.TabIndex = 44;
            this.MainMenu.Text = "menuStrip1";
            this.MainMenu.ItemAdded += new System.Windows.Forms.ToolStripItemEventHandler(this.MainMenu_ItemAdded);
            // 
            // ToptsmWin
            // 
            this.ToptsmWin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmExit});
            this.ToptsmWin.Name = "ToptsmWin";
            this.ToptsmWin.Size = new System.Drawing.Size(64, 21);
            this.ToptsmWin.Text = "窗口(&W)";
            // 
            // tsmExit
            // 
            this.tsmExit.Name = "tsmExit";
            this.tsmExit.Size = new System.Drawing.Size(115, 22);
            this.tsmExit.Text = "退出(&E)";
            this.tsmExit.Click += new System.EventHandler(this.tsmExit_Click);
            // 
            // MainStatusStrip
            // 
            this.MainStatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel8,
            this.toolStripStatusLabel11,
            this.toolStripStatusLabel9,
            this.toolStripStatusLabel10});
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 414);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.Size = new System.Drawing.Size(722, 26);
            this.MainStatusStrip.TabIndex = 52;
            this.MainStatusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(19, 21);
            this.toolStripStatusLabel1.Text = "1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(15, 21);
            this.toolStripStatusLabel2.Text = "2";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(19, 21);
            this.toolStripStatusLabel3.Text = "3";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(15, 21);
            this.toolStripStatusLabel4.Text = "4";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(19, 21);
            this.toolStripStatusLabel5.Text = "5";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(15, 21);
            this.toolStripStatusLabel6.Text = "6";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(19, 21);
            this.toolStripStatusLabel7.Text = "7";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(15, 21);
            this.toolStripStatusLabel8.Text = "8";
            // 
            // toolStripStatusLabel11
            // 
            this.toolStripStatusLabel11.Name = "toolStripStatusLabel11";
            this.toolStripStatusLabel11.Size = new System.Drawing.Size(501, 21);
            this.toolStripStatusLabel11.Spring = true;
            this.toolStripStatusLabel11.Text = "11";
            // 
            // toolStripStatusLabel9
            // 
            this.toolStripStatusLabel9.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            this.toolStripStatusLabel9.Size = new System.Drawing.Size(23, 21);
            this.toolStripStatusLabel9.Text = " 9";
            // 
            // toolStripStatusLabel10
            // 
            this.toolStripStatusLabel10.Name = "toolStripStatusLabel10";
            this.toolStripStatusLabel10.Size = new System.Drawing.Size(47, 21);
            this.toolStripStatusLabel10.Text = "time10";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(722, 33);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 25);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(722, 83);
            this.toolStripContainer1.TabIndex = 54;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.tsFormsTool);
            this.toolStripContainer1.TopToolStripPanel.SizeChanged += new System.EventHandler(this.toolStripContainer1_TopToolStripPanel_SizeChanged);
            // 
            // tsFormsTool
            // 
            this.tsFormsTool.AutoSize = false;
            this.tsFormsTool.BackColor = System.Drawing.SystemColors.Window;
            this.tsFormsTool.Dock = System.Windows.Forms.DockStyle.None;
            this.tsFormsTool.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsFormsTool.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.tsFormsTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.tsbtnStart,
            this.toolStripSeparator6,
            this.tsbtnClear,
            this.toolStripSeparator3,
            this.tsbtnVisible,
            this.toolStripSeparator1,
            this.tsbtnStop,
            this.toolStripSeparator5});
            this.tsFormsTool.Location = new System.Drawing.Point(3, 0);
            this.tsFormsTool.Name = "tsFormsTool";
            this.tsFormsTool.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.tsFormsTool.Size = new System.Drawing.Size(147, 50);
            this.tsFormsTool.TabIndex = 49;
            this.tsFormsTool.Text = "窗体工具栏";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 50);
            // 
            // tsbtnStart
            // 
            this.tsbtnStart.Image = global::B2bApiInterface.Properties.Resources.Start_32;
            this.tsbtnStart.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnStart.Name = "tsbtnStart";
            this.tsbtnStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tsbtnStart.Size = new System.Drawing.Size(84, 47);
            this.tsbtnStart.Text = "启动(&A)";
            this.tsbtnStart.Click += new System.EventHandler(this.tsbtnStart_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 50);
            // 
            // tsbtnClear
            // 
            this.tsbtnClear.Image = global::B2bApiInterface.Properties.Resources.ClearLog_32;
            this.tsbtnClear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnClear.ImageTransparentColor = System.Drawing.Color.White;
            this.tsbtnClear.Name = "tsbtnClear";
            this.tsbtnClear.Size = new System.Drawing.Size(109, 36);
            this.tsbtnClear.Text = "清除日志(&D)";
            this.tsbtnClear.Click += new System.EventHandler(this.tsbtnClear_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 50);
            // 
            // tsbtnVisible
            // 
            this.tsbtnVisible.Image = global::B2bApiInterface.Properties.Resources.Clear_32_png;
            this.tsbtnVisible.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnVisible.ImageTransparentColor = System.Drawing.Color.White;
            this.tsbtnVisible.Name = "tsbtnVisible";
            this.tsbtnVisible.Size = new System.Drawing.Size(109, 36);
            this.tsbtnVisible.Text = "隐藏日志(&D)";
            this.tsbtnVisible.Click += new System.EventHandler(this.tsbtnVisible_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 50);
            // 
            // tsbtnStop
            // 
            this.tsbtnStop.Image = global::B2bApiInterface.Properties.Resources.Stop_32;
            this.tsbtnStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtnStop.ImageTransparentColor = System.Drawing.Color.White;
            this.tsbtnStop.Name = "tsbtnStop";
            this.tsbtnStop.Size = new System.Drawing.Size(83, 36);
            this.tsbtnStop.Text = "停止(&S)";
            this.tsbtnStop.Click += new System.EventHandler(this.tsbtnStop_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 50);
            // 
            // rtxLog
            // 
            this.rtxLog.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.rtxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxLog.Location = new System.Drawing.Point(0, 108);
            this.rtxLog.Name = "rtxLog";
            this.rtxLog.ReadOnly = true;
            this.rtxLog.Size = new System.Drawing.Size(722, 306);
            this.rtxLog.TabIndex = 55;
            this.rtxLog.Text = "";
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            // 
            // timer3
            // 
            this.timer3.Interval = 1000;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(722, 440);
            this.Controls.Add(this.rtxLog);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.MainStatusStrip);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "frmMain";
            this.Text = "B2B电商平台同步接口";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.cmsMain.ResumeLayout(false);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.MainStatusStrip.ResumeLayout(false);
            this.MainStatusStrip.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.tsFormsTool.ResumeLayout(false);
            this.tsFormsTool.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon icnMain;
        private System.Windows.Forms.ContextMenuStrip cmsMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiStart;
        private System.Windows.Forms.ToolStripMenuItem tsmiStop;
        private System.Windows.Forms.ToolStripMenuItem tsmiCloseWindow;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem ToptsmWin;
        private System.Windows.Forms.ToolStripMenuItem tsmExit;
        private System.Windows.Forms.StatusStrip MainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel11;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel10;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip tsFormsTool;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbtnStart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbtnStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbtnVisible;
        private System.Windows.Forms.RichTextBox rtxLog;
        private System.Windows.Forms.ToolStripButton tsbtnClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        //private System.Windows.Forms.SplitContainer splMain;
        //private System.Windows.Forms.SplitContainer splTool;
        //private Util.PasswordTextBox rbtnS;
        //private System.Windows.Forms.SplitContainer splMain;
    }
}

