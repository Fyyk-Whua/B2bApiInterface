 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace B2bApiInterface
{

    public partial class frmMain : Form
    {
        private static Util.QuartzManager _QuartzManager = null;
        private static Log4netUtil.LogAppendToForms _LogAppendToForms = null;
        private static Model.ConfigInfo _ConfigInfo = null;
        private static string _EnterpriseName = string.Empty;
        private static string _EnterpriseId = string.Empty;
        private static string _B2bPlatformName = string.Empty;
        private static string _ErrorMsg = string.Empty;
        private static string _AutReg = string.Format("电商平台同步接口 V.{0}", "1.0");
        private static bool _IsShowLog = false;
        public static int _Width = 800;
        public static int _Height = 800;

        #region Form OnLoad
        /// <summary>
        /// frmMain
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            this.IsMdiContainer = true; //主窗体容器
            this.MainMenuStrip = MainMenu;
            _Width = this.Width;
            _Height = this.Height;
            toolStripContainer1.TopToolStripPanel.Width = this.Width;
            this.tsFormsTool.Width = toolStripContainer1.TopToolStripPanel.Width;
            RemoveMdiBackColor();
        }

        /// <summary>
        /// frmMain_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            _ConfigInfo = Util.ConfigInfoLoad.GetConfigInfo();
            _LogAppendToForms = new Log4netUtil.LogAppendToForms();  //
            _LogAppendToForms._LogAppendDelegate = DisplayJobtimes;
            rtxLog.Visible = false;
            _EnterpriseName = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, "EnterpriseConfig", "EnterpriseName", null);
            _EnterpriseId = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, "EnterpriseConfig", "EnterpriseId", null);
            _B2bPlatformName = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, "EnterpriseConfig", "B2bPlatformName", null);
            _AutReg = string.Format("{0}{1}", _B2bPlatformName, _AutReg);
            this.Text = _AutReg;

        
            HideLog();
            this.tsbtnStop.Enabled = false;
           
            if (!_ConfigInfo.IsShowServiceAndSupport)
            {
                //ToptsmServiceAndSupport.Enabled = false;
                //ToptsmServiceAndSupport.Visible = false;
            }
        }

        #endregion

        #region UI Event

        #region frmMain_FormClosing  关闭事件
        /// <summary>
        /// frmMain_FormClosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注意判断关闭事件reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //取消"关闭窗口"事件
                e.Cancel = true; // 取消关闭窗体 

                //使关闭时窗口向右下角缩小的效果
                this.WindowState = FormWindowState.Minimized;
                this.icnMain.Visible = true;
                //this.m_cartoonForm.CartoonClose();
                this.Hide();
                return;
            }
        }
        #endregion

        #region icnMain_MouseDoubleClick   双击图标事件
        /// <summary>
        /// icnMain_MouseDoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void icnMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Minimized;
                this.icnMain.Visible = true;
                this.Hide();
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Maximized;
                this.Activate();
            }
        }
        #endregion

        #region tsmiStop_Click  停止
        /// <summary>
        /// tsmiStop_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void tsmiStop_Click(object sender, EventArgs e)
        {
            StopQuartzManager();
        }
        #endregion

        #region tsbtnStop_Click 停止
        /// <summary>
        /// tsbtnStop_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnStop_Click(object sender, EventArgs e)
        {
            StopQuartzManager();
        }
        #endregion

        #region tsmiStart_Click  开启
        /// <summary>
        /// tsmiStart_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void tsmiStart_Click(object sender, EventArgs e)
        {
            StartQuartzManager();
        }
        #endregion

        #region tsbtnStart_Click  开启
        /// <summary>
        /// tsbtnStart_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnStart_Click(object sender, EventArgs e)
        {
            StartQuartzManager();
        }
        #endregion

        #region toolStripContainer1_TopToolStripPanel_SizeChanged 工具条两行排列变为一行
        /// <summary>
        /// toolStripContainer1_TopToolStripPanel_SizeChanged  工具条两行排列变为一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripContainer1_TopToolStripPanel_SizeChanged(object sender, EventArgs e)
        {
            toolStripContainer1.TopToolStripPanel.Width = this.Width;
            this.tsFormsTool.Width = toolStripContainer1.TopToolStripPanel.Width;
            toolStripContainer1.Height = toolStripContainer1.TopToolStripPanel.Height;
            
        }
        #endregion

        #region tsbtnVisible_Click 隐藏或显示日志
        /// <summary>
        ///  tsbtnVisible_Click 隐藏或显示日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnVisible_Click(object sender, EventArgs e)
        {
            if (_IsShowLog)
            {
                this.rtxLog.Visible = true;
                _IsShowLog = false;
            }
            else
            {
                this.rtxLog.Visible = false;
                _IsShowLog = true;
            }
        }
        #endregion

        #region tsbtnClear_Click 清除日志
        /// <summary>
        /// tsbtnClear_Click  清除日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnClear_Click(object sender, EventArgs e)
        {
            this.rtxLog.Clear();
        }
        #endregion

        #region tsmExit_Click
        /// <summary>
        /// tsmExit_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("你确定要退出【{0}】吗？", _AutReg), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                this.icnMain.Visible = false;
                this.Close();
                this.Dispose();
                System.Environment.Exit(System.Environment.ExitCode);
            }
        }
        #endregion

        #region tsmiCloseWindow_Click
        /// <summary>
        /// tsmiCloseWindow_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiCloseWindow_Click(object sender, EventArgs e)
        {
            CloseWindow();
            /*if (MessageBox.Show("你确定要退出？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {

                this.icnMain.Visible = false;
                this.Close();
                this.Dispose();
                System.Environment.Exit(System.Environment.ExitCode);

            }*/
        }
        #endregion

   

        #region timer1_Tick 改新任务栏时间
        /// <summary>
        /// 改新任务栏时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel10.Text = System.DateTime.Now.ToString("T");
        }
        #endregion

        #endregion

        #region Private Function

        #region ExistsMdiChildrenInstance 激活活动窗体
        /// <summary>
        /// 激活活动窗体
        /// </summary>
        private bool ExistsMdiChildrenInstance(string MdiChildrenClassName)
        {

            foreach (Form childFrm in this.MdiChildren)   
            {
                if (childFrm.Name == MdiChildrenClassName)
                {
                    if (childFrm.WindowState == FormWindowState.Minimized)
                    {
                        childFrm.WindowState = FormWindowState.Maximized;
                    }
                    childFrm.WindowState = FormWindowState.Maximized;
                    HideLog();
                    childFrm.Activate();
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region MainMenu_ItemAdded  去掉MainMenu左角的ICO
        /// <summary>
        /// MainMenu_ItemAdded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            if (e.Item.Text.Length == 0)
                e.Item.Visible = false;
        }
        #endregion

        #region  RemoveMdiBackColor 设置MDI背景
        /// <summary>
        /// 设置MDI背景
        /// </summary>
        private void RemoveMdiBackColor()
        {
            foreach (Control c in this.Controls)
            {
                if (c is MdiClient)
                {
                    c.BackColor = this.BackColor; //颜色 
                    c.BackgroundImage = this.BackgroundImage; //背景 
                }
            }
        }
        #endregion

        #region CloseWindow 关闭窗体
        /// <summary>
        /// CloseWindow  关闭窗体
        /// </summary>
        public void CloseWindow()
        {
            if (MessageBox.Show(string.Format("你确定要退出【{0}】吗？",_AutReg), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                StopQuartzManager();
                this.icnMain.Visible = false;
                this.Close();
                this.Dispose();
                System.Environment.Exit(System.Environment.ExitCode);
            }
        }
        #endregion

        #region QuartzManager Function

        #region StartQuartzManager
        /// <summary>
        /// StartQuartzManager 启动任务管理器
        /// </summary>
        public void StartQuartzManager()
        {
            try
            {
                string logMessage = string.Empty;
              
                ShowLog();
                _QuartzManager = new Util.QuartzManager();
                int r = _QuartzManager.StartAllJobs();

                _ConfigInfo = GetConfigInfo();

                if (_ConfigInfo == null)
                {
                    logMessage = string.Format("【{0}】 初始化ConfigInfo失败.....任务管理器启动失败！", "QuartzManagerJob", string.Empty);
                    Log4netUtil.LogDisplayHelper.LogWarning(_LogAppendToForms, logMessage);
                    this.tsbtnStart.Enabled = true;
                    return;
                }
                switch (r)
                {
                    case -1:
                        logMessage = string.Format("【{0}】 启动任务调度器失败，原因详见日志!", "InterfaceJob");
                        Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
                        Log4netUtil.LogDisplayHelper.LogError(_LogAppendToForms, logMessage);
                        break;
                    case 1:
                        logMessage = string.Format("【{0}】 任务调度器启动成功!{1}", _AutReg, false ? "(调式模式)" : string.Empty);
                        Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
                        Log4netUtil.LogDisplayHelper.LogMessage(_LogAppendToForms, logMessage);
                        IList<Model.JobEntity> jobInfoList = new List<Model.JobEntity>();
                        jobInfoList = GetJobEntityListAll();
                        GetDelBackUpFilesJob(_ConfigInfo);
                        foreach (var item in jobInfoList)
                        {
                            item.EnterpriseId = _ConfigInfo.EnterpriseId;
                            item.EnterpriseName = _ConfigInfo.EnterpriseName;
                            //item.IsDebug = _ConfigInfo.IsDebug;
                            item.StrConfigInfo  =Util.NewtonsoftCommon.SerializeObjToJson(_ConfigInfo);
                            GetScheduleJob(item);
                        }
                        this.tsmiStop.Enabled = true;
                        this.tsbtnStop.Enabled = true;
                        this.tsbtnStart.Enabled = false;
                        this.tsmiStart.Enabled = false;
                        break;
                    case 2:
                        logMessage = string.Format("【{0}】 启动任务调度器已在运行中，不能重复启动！", "InterfaceJob");
                        Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
                        Log4netUtil.LogDisplayHelper.LogWarning(_LogAppendToForms, logMessage);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("【{0}】 所有任务启动失败,失败原因{1}", "InterfaceJob", ex.Message);
                Log4netUtil.LogDisplayHelper.LogWarning(_LogAppendToForms, logMessage);
            }
        }
        #endregion

        #region StopQuartzManager
        /// <summary>
        /// StopQuartzManager  停止任务管理器
        /// </summary>
        public void StopQuartzManager()
        {
            try
            {
                ShowLog();
                string logMessage = string.Empty;
                int r = _QuartzManager.ShutDownJobs();
                switch (r)
                {
                    case 1:
                        logMessage = string.Format("【{0}】 任务调度器已停止！!", "InterfaceJob");
                        Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
                        Log4netUtil.LogDisplayHelper.LogMessage(_LogAppendToForms, logMessage);
                        this.tsmiStop.Enabled = false;
                        this.tsbtnStop.Enabled = false;
                        this.tsbtnStart.Enabled = true;
                        this.tsmiStart.Enabled = true;
                        break;
                    case 2:
                        logMessage = string.Format("【{0}】 任务调度器已停止，不需要停止！", "InterfaceJob");
                        Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
                        Log4netUtil.LogDisplayHelper.LogWarning(_LogAppendToForms, logMessage);
                        break;
                    case -1:
                        logMessage = string.Format("【{0}】 任务调度器停止失败，失败原因详见日志!", "InterfaceJob");
                        Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
                        Log4netUtil.LogDisplayHelper.LogError(_LogAppendToForms, logMessage);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("【{0}】 任务调度器停止失败，失败原因{1}", "InterfaceJob", ex.Message);
                Log4netUtil.LogDisplayHelper.LogError(_LogAppendToForms, logMessage);
            }
        }
        #endregion

        #region GetJobEntityListAll  获取调度任务
        /// <summary>
        /// GetJobEntityListAll 获取调度任务
        /// </summary>
        /// <returns></returns>
        private IList<Model.JobEntity> GetJobEntityListAll()
        {
            return _ConfigInfo.JobEntityList;
        }
        #endregion

        #region GetScheduleJob
        /// <summary>
        /// GetScheduleJob
        /// </summary>
        /// <param name="jobInfo"></param>
        private void GetScheduleJob(Model.JobEntity jobInfo)
        {
            _QuartzManager.ScheduleJob(jobInfo, _LogAppendToForms);
            string logMessage = string.Empty;
            if (jobInfo.IsDebug)
            {
                logMessage = string.Format("【{0}_{1}】(调式模式) 加入调度器成功!", jobInfo.JobCode, jobInfo.JobName.ToString());
                Log4netUtil.LogDisplayHelper.LogWarning(_LogAppendToForms, logMessage);
            }
            else
            {
                logMessage = string.Format("【{0}_{1}】加入调度器成功!", jobInfo.JobCode, jobInfo.JobName.ToString());
                Log4netUtil.LogDisplayHelper.LogMessage(_LogAppendToForms, logMessage);
            }
            Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
        }
        #endregion

        #region GetDelBackUpFilesJob
        /// <summary>
        /// GetScheduleJob
        /// </summary>
        /// <param name="jobInfo"></param>
        private void GetDelBackUpFilesJob(Model.ConfigInfo configInfo)
        {
            _QuartzManager.ScheduleDelBackUpFiles(_LogAppendToForms, configInfo.LogRetentionDays);
            string logMessage = string.Format("【{0}_{1}】 加入调度器成功!", "DelBackUplogFiles", "清除日志计划");
            Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
            Log4netUtil.LogDisplayHelper.LogMessage(_LogAppendToForms, logMessage);
        }
        #endregion

        #endregion

        #region HideLog
        /// <summary>
        /// ShowLog 隐藏日志 
        /// </summary>
        private void HideLog()
        {
            this.rtxLog.Visible = false;
            this.tsbtnVisible.Text = "显示日志";
            _IsShowLog = true;
        }
        #endregion

        #region ShowLog
        /// <summary>
        /// ShowLog 显示日志
        /// </summary>
        private void ShowLog()
        {
            this.rtxLog.Visible = true;
            this.tsbtnVisible.Text = "隐藏日志";
            _IsShowLog = false;
        }
        #endregion

        #region MethodFormCloseStatus
        /// <summary>
        /// MethodFormCloseStatus 所有子窗体关闭后显示日志
        /// </summary>
        private void MethodFormCloseStatus()
        {
            int i = this.MdiChildren.Count();
            if (i <=1)
                ShowLog();
        }
        #endregion

        #region  DisplayJobtimes
        /// <summary>
        /// DisplayJobtimes
        /// </summary>
        /// <param name="color"></param>
        /// <param name="msg"></param>
        private void DisplayJobtimes(Color color, string msg)
        {
            this.rtxLog.BeginInvoke(new Action(() =>
            {
                rtxLog.SelectionColor = color;
                //rtxLog.AppendText("\r\n");
                rtxLog.AppendText(string.Format("{0}{1}{2}",">>>",msg.ToString(),"\r\n"));
                //rtxLog.ScrollToCaret();
                rtxLog.Focus();//让文本框获取焦点 
                rtxLog.Select(rtxLog.TextLength, 0);//设置光标的位置到文本尾
                rtxLog.ScrollToCaret();//滚动到控件光标处

            }));
        }
        #endregion

        #region  GetConfigInfo
        /// <summary>
        /// GetConfigInfo
        /// </summary>
        private Model.ConfigInfo GetConfigInfo()
        {
            Model.ConfigInfo configInfo = new Model.ConfigInfo();
            configInfo = Util.ConfigInfoLoad.GetConfigInfo();
            return configInfo;
        }






        #endregion

        #endregion

       
    }




}
