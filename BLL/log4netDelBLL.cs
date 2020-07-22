using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;

namespace BLL
{

    //不允许此 Job 并发执行任务（禁止新开线程执行）
    [DisallowConcurrentExecution]
    public sealed class log4netDelBLL : IJob
    {
        //private static System.Windows.Forms.RichTextBox rtxLog = null;
        #region Execute 
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Log4netUtil.LogAppendToForms logAppendToForms = (Log4netUtil.LogAppendToForms)context.JobDetail.JobDataMap.Get("ControlQueue");
            Version Ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string jobName = context.JobDetail.Key.Name;
            string jobGroup = context.JobDetail.Key.Group;
            string logMessage = string.Empty;
            string jobLogType = "QuartzManager";
            JobDataMap jobData = context.JobDetail.JobDataMap;
            try
            {
                int logRetentionDays = jobData.GetInt("LogRetentionDays");
                if (logRetentionDays > 0)
                {
                    string path = System.Windows.Forms.Application.StartupPath.ToString() + "\\Logs";
                    Log4netUtil.FilesHelper.DeleteALLFiles(logAppendToForms, path, logRetentionDays);
                    logMessage = string.Format("【{0}】 Execute 执行成功！Ver.{1}", "清除日志计划", Ver.ToString());
                    LogMessage(logAppendToForms, true, logMessage, jobLogType);
                }
                else
                {
                    logMessage = string.Format("【{0}】 Execute 无执行计划！Ver.{1}", "清除日志计划", Ver.ToString());
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                }
            }
            catch (Exception ex)
            {
                logMessage = string.Format("【{0}】 Execute 执行发生异常:{1}", "清除日志计划", ex.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
            }

        }
        #endregion



        #region LogMessage
        /// <summary>
        /// LogMessage
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="isDebug"></param>
        /// <param name="logMessage"></param>
        /// <param name="jobType"></param>
        private void LogMessage(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
            if (isDebug)
                Log4netUtil.LogDisplayHelper.LogError(logAppendToForms, logMessage);
        }
        #endregion

        #region LogWarning
        /// <summary>
        /// LogWarning
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="isDebug"></param>
        /// <param name="logMessage"></param>
        /// <param name="jobType"></param>
        private void LogWarning(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
            if (isDebug)
                Log4netUtil.LogDisplayHelper.LogWarning(logAppendToForms, logMessage);
        }
        #endregion

        #region LogError
        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="isDebug"></param>
        /// <param name="logMessage"></param>
        /// <param name="jobType"></param>
        private void LogError(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
            if (isDebug)
                Log4netUtil.LogDisplayHelper.LogError(logAppendToForms, logMessage);
        }
        #endregion


    }
}

