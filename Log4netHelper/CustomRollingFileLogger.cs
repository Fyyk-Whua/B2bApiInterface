using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Configuration;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;
 
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Log4netUtil
{
    public  static class CustomRollingFileLogger
    {
        #region  变量定义
        private static readonly ConcurrentDictionary<string, ILog> loggerContainer = new ConcurrentDictionary<string, ILog>();
 
        private static readonly Dictionary<string, ReadParamAppender> appenderContainer = new Dictionary<string, ReadParamAppender>();
        private static object lockObj = new object();
 
        //默认配置
        private const int MAX_SIZE_ROLL_BACKUPS = 20;
        private const string LAYOUT_PATTERN ="%n【记录时间】%date%n【描    述】%message%n";  //"%d [%t] %-5p %c  - %m%n";
        private const string DATE_PATTERN ="yyyyMMdd\".log\"";
        private const string MAXIMUM_FILE_SIZE = "256MB";
        private const string LEVEL = "debug";
        #endregion



        #region CustomRollingFileLogger 读取配置文件并缓存
        /// <summary>
        /// CustomRollingFileLogger
        /// </summary>
        static CustomRollingFileLogger()
        {
            IAppender[] appenders = LogManager.GetRepository().GetAppenders();
            for (int i = 0; i < appenders.Length; i++)
            {
                if (appenders[i] is ReadParamAppender)
                {
                    ReadParamAppender appender = (ReadParamAppender)appenders[i];
                    if (appender.MaxSizeRollBackups == 0)
                    {
                        appender.MaxSizeRollBackups = MAX_SIZE_ROLL_BACKUPS; //设置无限备份=-1 ，最大备份数为1000
                    }
                    if (appender.Layout != null && appender.Layout is log4net.Layout.PatternLayout)
                    {
                        appender.LayoutPattern = ((log4net.Layout.PatternLayout)appender.Layout).ConversionPattern;
                    }
                    if (string.IsNullOrEmpty(appender.LayoutPattern))
                    {
                        appender.LayoutPattern = LAYOUT_PATTERN;
                    }
                    if (string.IsNullOrEmpty(appender.DatePattern)) 
                    {
                        appender.DatePattern = DATE_PATTERN;  //文件名称 
                    }
                    if (string.IsNullOrEmpty(appender.MaximumFileSize))
                    {
                        appender.MaximumFileSize = MAXIMUM_FILE_SIZE; //每个文件的大小
                    }
                    if (string.IsNullOrEmpty(appender.Level))
                    {
                        appender.Level = LEVEL;
                    }
                    lock(lockObj)
                    {
                        appenderContainer[appenders[i].Name] = appender;
                    }
                }
            }
        }
        #endregion

        #region GetCustomLogger
        /// <summary>
        /// GetCustomLogger
        /// </summary>
        /// <param name="loggerName">名称</param>
        /// <param name="category">路径</param>
        /// <param name="additivity">取值是true或false，默认值是true。设置为false时将阻止父logger中的appender</param>
        /// <returns></returns>
        public static ILog GetCustomLogger(string loggerName, string category = null, bool additivity = false)
        {
            loggerName = string.Format("{0:yyyy-MM-dd HH}{1}_{2}", DateTime.Now, "时", loggerName);
            string a = loggerName;
            return loggerContainer.GetOrAdd(loggerName, delegate(string name)
            {
                RollingFileAppender newAppender = null;
                ReadParamAppender appender = null;
                if (appenderContainer.ContainsKey(loggerName))
                {
                    appender = appenderContainer[loggerName];
                    newAppender = GetNewFileApender(loggerName, string.IsNullOrEmpty(appender.File) ? GetFile(category, loggerName) : appender.File, appender.MaxSizeRollBackups,
                        appender.AppendToFile, true, appender.MaximumFileSize, RollingFileAppender.RollingMode.Composite, appender.DatePattern, appender.LayoutPattern);
                }
                else
                {
                    newAppender = GetNewFileApender(loggerName, GetFile(category, loggerName), MAX_SIZE_ROLL_BACKUPS, true, true, MAXIMUM_FILE_SIZE, RollingFileAppender.RollingMode.Composite, 
                        DATE_PATTERN, LAYOUT_PATTERN);
                }
                log4net.Repository.Hierarchy.Hierarchy repository = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
                Logger logger = repository.LoggerFactory.CreateLogger(repository, loggerName);
                logger.Hierarchy = repository;
                logger.Parent = repository.Root;
                logger.Level = GetLoggerLevel(appender == null ? LEVEL : appender.Level);
                logger.Additivity = additivity;
                logger.AddAppender(newAppender);
                logger.Repository.Configured = true;
                return new LogImpl(logger);
            });
        }
        #endregion


        //如果没有指定文件路径则在运行路径下建立 Log\{loggerName}.txt
        #region GetFile
        /// <summary>
        /// GetFile
        /// </summary>
        /// <param name="category"></param>
        /// <param name="loggerName"></param>
        /// <returns></returns>
        private static string GetFile(string category, string loggerName)
        {
            if (string.IsNullOrEmpty(category))
            {
                return string.Format(@"Logs\{0}.log", loggerName);
            }
            else
            {
                return string.Format(@"Logs\{0}\{1}.log", category, loggerName);
            }
        }
        #endregion

        #region GetLoggerLevel
        /// <summary>
        /// GetLoggerLevel
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static Level GetLoggerLevel(string level)
        {
            if (!string.IsNullOrEmpty(level))
            {
                switch (level.ToLower().Trim())
                {
                    case "debug":
                        return Level.Debug;
 
                    case "info":
                        return Level.Info;
 
                    case "warn":
                        return Level.Warn;
 
                    case "error":
                        return Level.Error;
 
                    case "fatal":
                        return Level.Fatal;
                }
            }
            return Level.Debug;
        }
        #endregion

        #region GetNewFileApender
        /// <summary>
        /// GetNewFileApender
        /// </summary>
        /// <param name="appenderName"></param>
        /// <param name="file"></param>
        /// <param name="maxSizeRollBackups"></param>
        /// <param name="appendToFile"></param>
        /// <param name="staticLogFileName"></param>
        /// <param name="maximumFileSize"></param>
        /// <param name="rollingMode"></param>
        /// <param name="datePattern"></param>
        /// <param name="layoutPattern"></param>
        /// <returns></returns>
        private static RollingFileAppender GetNewFileApender(string appenderName, string file, int maxSizeRollBackups, bool appendToFile = true, bool staticLogFileName = false, string maximumFileSize = "5MB", RollingFileAppender.RollingMode rollingMode = RollingFileAppender.RollingMode.Composite, string datePattern = "yyyyMMdd\".txt\"", string layoutPattern = "%d [%t] %-5p %c  - %m%n")
        {
            RollingFileAppender appender = new RollingFileAppender
            {
                LockingModel = new FileAppender.MinimalLock(),
                Name = appenderName,
                File = file,
                AppendToFile = appendToFile,
                MaxSizeRollBackups = maxSizeRollBackups,
                MaximumFileSize = maximumFileSize,
                StaticLogFileName = staticLogFileName,
                RollingStyle = rollingMode,
                DatePattern = datePattern
            };
            PatternLayout layout = new PatternLayout(layoutPattern);
            appender.Layout = layout;
            layout.ActivateOptions();
            appender.ActivateOptions();
            return appender;
        }
        #endregion
    }
}


