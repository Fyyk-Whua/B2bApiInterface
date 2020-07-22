using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System.Reflection;

namespace Util
{
    public class QuartzManager
    {
        private static ISchedulerFactory schedulerFactory = null;  //定义一个标准调度器工厂
        private static IScheduler scheduler = null;
        //public delegate void updateUIContorl(string txt);

        public QuartzManager()
        {
            schedulerFactory = new StdSchedulerFactory(); //创建一个标准调度器工厂
            scheduler = schedulerFactory.GetScheduler(); //通过从标准调度器工厂获得一个调度器，用来启动任务
        }

        #region StartAllJobs 启动所有任务
        /// <summary>
        /// 启动所有任务
        /// </summary>
        /// <returns> 1 启动成功   2 重复启动  -1 启动失败</returns>
        public int StartAllJobs()
        {
            try
            {
                if (scheduler.IsStarted)
                    return 2;
                scheduler.Start(); //调度器的线程开始执行，用以触发Trigger
                return 1;  //启动成功
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(string.Format("定时调度任务启动失败{0}", ex.Message), "QuartzManager");
                return -1; //启动失败
            }
        }
        #endregion

        #region ShutDownJobs 关闭所有任务
        /// <summary>
        /// 关闭所有任务
        /// </summary>
        /// <returns></returns>
        public int ShutDownJobs()
        {
            try
            {
                if (!scheduler.IsShutdown)
                {
                    scheduler.Shutdown();
                    return 1;
                }
                else
                    return 2;
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(string.Format("关闭定时调度任务失败{0}", ex.Message), "QuartzManager");
                return -1;
            }
        }
        #endregion

        #region ValidExpression 校验字符串是否为正确的Cron表达式  
        /// <summary>
        /// 校验字符串是否为正确的Cron表达式
        /// </summary>
        /// <param name="cronExpression">带校验表达式</param>
        /// <returns></returns>
        public static bool ValidExpression(string cronExpression)
        {
            return CronExpression.IsValidExpression(cronExpression);
        }
        #endregion

        #region 从程序集中加载指定类
        /// <summary>
        /// 从程序集中加载指定类
        /// </summary>
        /// <param name="assemblyName">含后缀的程序集名</param>
        /// <param name="className">含命名空间完整类名</param>
        /// <returns></returns>
        private Type GetClassInfo(string assemblyName, string className)
        {
            Type type = null;
            try
            {
                Assembly assembly = null;
                assembly = Assembly.Load(assemblyName);
                type = assembly.GetType(className, true, true);
            }
            catch { }
            return type;
        }
        #endregion

        #region ScheduleJob
        /// <summary>
        /// ScheduleJob
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="jobInfo"></param>
        public void ScheduleJob(Model.JobEntity jobInfo, Log4netUtil.LogAppendToForms logAppendToForms)  //IScheduler scheduler,
        {
            if (ValidExpression(jobInfo.CronExpression))
            {
                Type type = GetClassInfo("BLL", "BLL.JobsItemsBLL");
                if (type != null)
                {
                    //使用组别、名称创建一个工作明细，此处为所需要执行的任务
                    IJobDetail job = new JobDetailImpl(jobInfo.JobCode.ToString(), string.Format("{0}_{1}_JobGroup", jobInfo.JobCode.ToString(), jobInfo.JobName), type);
                    job.JobDataMap.Add("JobCode", jobInfo.JobCode);
                    job.JobDataMap.Add("JobName", jobInfo.JobName);
                    job.JobDataMap.Add("IsDebug", jobInfo.IsDebug); //调式模式

                    job.JobDataMap.Add("DomainName", jobInfo.DomainName);
                    job.JobDataMap.Add("ServiceName", jobInfo.ServiceName);
                    job.JobDataMap.Add("InterfacePrefix", jobInfo.InterfacePrefix);
                    job.JobDataMap.Add("ApiModuleType", jobInfo.ApiModuleType);
                    job.JobDataMap.Add("ApiRequestType", jobInfo.ApiRequestType);

                    job.JobDataMap.Add("TargetDatabase", jobInfo.TargetDatabase);
                    job.JobDataMap.Add("ProcedureName", jobInfo.ProcedureName);
                    job.JobDataMap.Add("ModuleID", jobInfo.ModuleID);
                    job.JobDataMap.Add("FilterBillType", jobInfo.FilterBillType);
                    job.JobDataMap.Add("WritebackProcedureName", jobInfo.WritebackProcedureName);
                    job.JobDataMap.Add("WritebackType", jobInfo.WritebackType);

                    job.JobDataMap.Add("CronExpression", jobInfo.CronExpression); //调式模式
                    job.JobDataMap.Add("CronExpressionDescription", jobInfo.CronExpressionDescription); //调式模式
                    job.JobDataMap.Add("EnterpriseId", jobInfo.EnterpriseId); //调式模式
                    job.JobDataMap.Add("EnterpriseName", jobInfo.EnterpriseName); //调式模式

                    job.JobDataMap.Add("StrConfigInfo", jobInfo.StrConfigInfo); //调式模式
                    job.JobDataMap.Put("ControlQueue", logAppendToForms);

                    //使用组别、名称创建一个触发器，其中触发器立即执行，且每隔1秒或3秒执行一个任务，重复执行
                    CronTriggerImpl trigger = new CronTriggerImpl();
                    trigger.CronExpressionString = jobInfo.CronExpression;  // 任务执行的cron表达式
                    trigger.Name  = string.Format("{0}_{1}", jobInfo.JobCode.ToString(), jobInfo.JobName);  //触发器名称，同一个分组中的名称必须不同
                    trigger.Description  = jobInfo.CronExpressionDescription;    //触发器描述 
                    trigger.StartTimeUtc = DateTime.UtcNow;
                    trigger.Group = string.Format("{0}_{1}_TriggerGroup", jobInfo.JobCode.ToString(), jobInfo.JobName);//触发器组
                    scheduler.ScheduleJob(job, trigger); //作业和触发器设置到调度器中 
                }
            }
        }
        #endregion

        #region ScheduleJob
        /// <summary>
        /// ScheduleJob
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="jobInfo"></param>
        public void ScheduleDelBackUpFiles(Log4netUtil.LogAppendToForms logAppendToForms, int LogRetentionDays)  //IScheduler scheduler,
        {

            Type type = GetClassInfo("BLL", "BLL.log4netDelBLL");
            if (type != null)
            {
                //使用组别、名称创建一个工作明细，此处为所需要执行的任务
                IJobDetail job = new JobDetailImpl("清除日志计划", string.Format("{0}_JobGroup", "清除日志计划"), type);

                job.JobDataMap.Add("JobType", "log4net清除日志");
                job.JobDataMap.Add("LogRetentionDays", LogRetentionDays);
                job.JobDataMap.Put("ControlQueue", logAppendToForms);

                //使用组别、名称创建一个触发器，其中触发器立即执行，且每隔1秒或3秒执行一个任务，重复执行
                CronTriggerImpl trigger = new CronTriggerImpl();
                trigger.CronExpressionString = "0 0 9 1/1 * ? "; //"0/30 * * ? * 1,2,3,4,5 "; //  任务执行的cron表达式
                trigger.Name = "清除日志计划";  //触发器名称，同一个分组中的名称必须不同
                trigger.Description = "每天9点执行一次";    //触发器描述 
                trigger.StartTimeUtc = DateTime.UtcNow;
                trigger.Group = string.Format("{0}_TriggerGroup", "清除日志计划");//触发器组
                scheduler.ScheduleJob(job, trigger); //作业和触发器设置到调度器中 
            }

        }
        #endregion


    }
}

