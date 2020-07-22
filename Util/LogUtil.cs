using System;
using System.Data;
using System.Text;
//using log4net;
//using log4net.Appender;
//using log4net.Config;
//using log4net.Layout;
//using log4net.Repository.Hierarchy;


namespace Util
{
    public class Log4NetHelp
    {
        /*
        #region 变量定义
        public static event Action<string> OutputMessage;  //定义信息的二次处理
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); //ILog对象

        //记录异常日志数据库连接字符串
        private const string _ConnectionString = @"data source=your server;initial catalog=your db;integrated security=false;persist security info=True;User ID=sa;Password=1111";

        //信息模板
        private const string _ConversionPattern = "%n【记录时间】%date%n【描述】%message%n";
        #endregion

 

        #region 定义信息二次处理方式

        private static void HandMessage(object Msg)
        {
            if (OutputMessage != null)
                OutputMessage(Msg.ToString());
        }

        private static void HandMessage(object Msg, Exception ex)
        {
            if (OutputMessage != null)
                OutputMessage(string.Format("{0}:{1}", Msg.ToString(), ex.ToString()));
        }

        private static void HandMessage(string format, params object[] args)
        {
            if (OutputMessage != null)
                OutputMessage(string.Format(format, args));
        }
        #endregion

 

        #region 封装Log4net

        #region Debug 调试信息
        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(object message)
        {
            HandMessage(message);
            if (log.IsDebugEnabled)
                log.Debug(message);
        }

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Debug(object message, Exception ex)
        {
            HandMessage(message, ex);
            if (log.IsDebugEnabled)
                log.Debug(message, ex);
        }

        /// <summary>
        /// DebugFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void DebugFormat(string format, params object[] args)
        {
            HandMessage(format, args);
            if (log.IsDebugEnabled)
                log.DebugFormat(format, args);
        }
        #endregion

        #region Error  一般错误
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message)
        {
            HandMessage(message);
            if (log.IsErrorEnabled)
                log.Error(message);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(object message, Exception ex)
        {
            HandMessage(message, ex);
            if (log.IsErrorEnabled)
                log.Error(message, ex);
        }

        /// <summary>
        /// ErrorFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void ErrorFormat(string format, params object[] args)
        {
            HandMessage(format, args);
            if (log.IsErrorEnabled)
                log.ErrorFormat(format, args);
        }
        #endregion

        #region Fatal 致命错误
        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(object message)
        {
            HandMessage(message);
            if (log.IsFatalEnabled)
                log.Fatal(message);
        }

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Fatal(object message, Exception ex)
        {

            HandMessage(message, ex);
            if (log.IsFatalEnabled)
                log.Fatal(message, ex);
        }

        /// <summary>
        /// FatalFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void FatalFormat(string format, params object[] args)
        {
            HandMessage(format, args);
            if (log.IsFatalEnabled)
                log.FatalFormat(format, args);
        }
        #endregion

        #region Info 一般信息
        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message"></param>
        public static void Info(object message)
        {
            if (log.IsInfoEnabled)
                log.Info(message);
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Info(object message, Exception ex)
        {
            if (log.IsInfoEnabled)
                log.Info(message, ex);
        }

        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void InfoFormat(string format, params object[] args)
        {
            HandMessage(format, args);
            if (log.IsInfoEnabled)
                log.InfoFormat(format, args);
        }
        #endregion

        #region  Warn 警告
        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(object message)
        {
            HandMessage(message);
            if (log.IsWarnEnabled)
                log.Warn(message);
        }

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Warn(object message, Exception ex)
        {
            HandMessage(message, ex);
            if (log.IsWarnEnabled)
                log.Warn(message, ex);
        }

        /// <summary>
        /// WarnFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WarnFormat(string format, params object[] args)
        {
            HandMessage(format, args);
            if (log.IsWarnEnabled)
                log.WarnFormat(format, args);
        }
        #endregion

        #endregion

 

        #region 手动加载配置

        #region LoadADONetAppender
        /// <summary>
        /// LoadADONetAppender
        /// </summary>
        public static void LoadADONetAppender()
        {
            Hierarchy hier = LogManager.GetLoggerRepository() as log4net.Repository.Hierarchy.Hierarchy;
            if (hier != null)
            {
                log4net.Appender.AdoNetAppender appender = new log4net.Appender.AdoNetAppender();
                appender.Name = "AdoNetAppender";
                appender.CommandType = CommandType.Text;
                appender.BufferSize = 1;
                appender.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                appender.ConnectionString = _ConnectionString;
                appender.CommandText = @"INSERT INTO Log ([Date],[Thread],[Level],[Logger],[Message],[Exception]) VALUES (@log_date, @thread, @log_level, @logger, @message, @exception)";
                appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_date", DbType = System.Data.DbType.DateTime, Layout = new log4net.Layout.RawTimeStampLayout() });
                appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@thread", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%thread")) });
                appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@log_level", DbType = System.Data.DbType.String, Size = 50, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
                appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@logger", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });
                appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@message", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%message")) });
                appender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@exception", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });
                appender.ActivateOptions();
                BasicConfigurator.Configure(appender);
            }
        }
        #endregion
 
        #region LoadFileAppender
        /// <summary>
        /// LoadFileAppender
        /// </summary>
        public static void LoadFileAppender()
        {
            FileAppender appender = new FileAppender();
            appender.Name = "FileAppender";
            appender.File = "error.log";
            appender.AppendToFile = true;
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = _ConversionPattern;
            patternLayout.ActivateOptions();
            appender.Layout = patternLayout;
            appender.Encoding = Encoding.UTF8; //选择UTF8编码，确保中文不乱码。
            appender.ActivateOptions();
            BasicConfigurator.Configure(appender);
        }
        #endregion
 
        #region LoadRollingFileAppender
        /// <summary>
        /// LoadRollingFileAppender
        /// </summary>
        public static void LoadRollingFileAppender()
        {
            RollingFileAppender appender = new RollingFileAppender();
            appender.AppendToFile = true;
            appender.Name = "RollingFileAppender";
            appender.DatePattern = "yyyy-MM-dd HH'时.log'";
            appender.File = "Logs/";
            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            appender.MaximumFileSize = "500kb";
            appender.MaxSizeRollBackups = 10;
            appender.StaticLogFileName = false;
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = _ConversionPattern;
            patternLayout.ActivateOptions();
            appender.Layout = patternLayout;
            //选择UTF8编码，确保中文不乱码。
            appender.Encoding = Encoding.UTF8;
            appender.ActivateOptions();
            BasicConfigurator.Configure(appender);
        }
        #endregion
 
        #region LoadConsoleAppender
        /// <summary>
        /// LoadConsoleAppender
        /// </summary>
        public static void LoadConsoleAppender()
        {
            ConsoleAppender appender = new ConsoleAppender();
            appender.Name = "ConsoleAppender";
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = _ConversionPattern;
            patternLayout.ActivateOptions();
            appender.Layout = patternLayout;
            appender.ActivateOptions();
            BasicConfigurator.Configure(appender);
        }
        #endregion
 
        #region LoadTraceAppender
        /// <summary>
        /// LoadTraceAppender
        /// </summary>
        public static void LoadTraceAppender()
        {
            TraceAppender appender = new TraceAppender();
            appender.Name = "TraceAppender";
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = _ConversionPattern;
            patternLayout.ActivateOptions();
            appender.Layout = patternLayout;
            appender.ActivateOptions();
            BasicConfigurator.Configure(appender);
        }
        #endregion
 
        #region LoadEventLogAppender
        /// <summary>
        /// LoadEventLogAppender
        /// </summary>
        public static void LoadEventLogAppender()
        {
            EventLogAppender appender = new EventLogAppender();
            appender.Name = "EventLogAppender";
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = _ConversionPattern;
            patternLayout.ActivateOptions();
            appender.Layout = patternLayout;
            appender.ActivateOptions();
            BasicConfigurator.Configure(appender);
        }
        #endregion

        #endregion


        #region 定义常规应用程序中未处理的异常信息记录方式
        public static void LoadUnhandledException()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((sender, e) =>
            {
               log.Fatal("未处理的异常", e.ExceptionObject as Exception);
            });
        }
        #endregion
        */
    }

}
