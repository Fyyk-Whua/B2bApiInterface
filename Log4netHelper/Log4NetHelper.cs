using System;
using System.Data;
using System.Text;
using log4net;

namespace Log4netUtil
{
    public class Log4NetHelper
    {
        #region 变量定义
        public static event Action<string> OutputMessage;  //定义信息的二次处理
        #endregion

        #region 定义信息二次处理方式
        /// <summary>
        /// HandMessage
        /// </summary>
        /// <param name="Msg"></param>
        private static void HandMessage(object Msg)
        {
            if (OutputMessage != null)
                OutputMessage(Msg.ToString());
        }

        /// <summary>
        /// HandMessage
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="ex"></param>
        private static void HandMessage(object Msg, Exception ex)
        {
            if (OutputMessage != null)
                OutputMessage(string.Format("{0}:{1}", Msg.ToString(), ex.ToString()));
        }

        /// <summary>
        /// HandMessage
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        private static void HandMessage(string format, params object[] args)
        {
            if (OutputMessage != null)
                OutputMessage(string.Format(format, args));
        }
        #endregion

        #region 封装Log4net
        #region Debug 调试信息
        /// <summary>
        /// Debug 调试信息
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="loggerName">日志文件名</param>
        /// <param name="category">日志路径</param>
        /// <param name="additivity"></param>
        public static void Debug(object message,  string category = null, bool additivity = false)
        {
            HandMessage(message);
            string loggerName = GetLoggerName("Debug", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Debug", category));
            logger.Debug(message);
        }

        /// <summary>
        /// Debug 调试信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Debug(object message, Exception ex, string category = null, bool additivity = false)
        {
            HandMessage(message, ex);
            string loggerName = GetLoggerName("Debug", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Debug", category));
            logger.Debug(message,ex);
        }

        /// <summary>
        /// DebugFormat 调试信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void DebugFormat(string format, string category = null, bool additivity = false,params object[] args)
        {
            HandMessage(format, args);
            string loggerName = GetLoggerName("Debug", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Debug", category));
            logger.DebugFormat(format, args);
        }
        #endregion

        #region Error  一般错误
        /// <summary>
        /// Error 一般错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        public static void Error(object message,  string category = null, bool additivity = false)
        {
            HandMessage(message);
            string loggerName = GetLoggerName("Error", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Error", category));
            logger.Error(message);        
        }

        /// <summary>
        /// Error 一般错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        public static void Error(object message, Exception ex, string category = null, bool additivity = false)
        {
            HandMessage(message, ex);
            string loggerName = GetLoggerName("Error", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Error", category));
            if (logger.IsErrorEnabled)
                logger.Error(message,ex);
            
        }

        /// <summary>
        /// ErrorFormat 一般错误
        /// </summary>
        /// <param name="format"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        /// <param name="args"></param>
        public static void ErrorFormat(string format, string category = null, bool additivity = false, params object[] args)
        {
            HandMessage(format, args);
            string loggerName = GetLoggerName("Error", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Error", category));
            if (logger.IsErrorEnabled)
                logger.ErrorFormat(format, args);
        }
        #endregion

        #region Fatal 致命错误
        /// <summary>
        /// Fatal 致命错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        public static void Fatal(object message, string category = null, bool additivity = false)
        {
            HandMessage(message);
            string loggerName = GetLoggerName("Fatal", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Fatal", category));
            if (logger.IsFatalEnabled)
                logger.Fatal(message);
        }

        /// <summary>
        /// Fatal 致命错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        public static void Fatal(object message, Exception ex, string category = null, bool additivity = false)
        {

            HandMessage(message, ex);
            string loggerName = GetLoggerName("Fatal", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Fatal", category));
            if (logger.IsFatalEnabled)
                logger.Fatal(message, ex);
        }

        /// <summary>
        /// FatalFormat 致命错误
        /// </summary>
        /// <param name="format"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        /// <param name="args"></param>
        public static void FatalFormat(string format, string category = null, bool additivity = false, params object[] args)
        {
            HandMessage(format, args);
            string loggerName = GetLoggerName("Fatal", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Fatal", category));
            if (logger.IsFatalEnabled)
                logger.FatalFormat(format, args);
        }
        #endregion

        #region Info 一般信息
        /// <summary>
        /// Info 一般信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        public static void Info(object message, string category = null, bool additivity = false)
        {
            string loggerName = GetLoggerName("Info", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Info", category));
            if (logger.IsInfoEnabled)
                logger.Info(message);
        }

        

        /// <summary>
        /// Info 一般信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        public static void Info(object message, Exception ex, string category = null, bool additivity = false)
        {
            string loggerName = GetLoggerName("Info", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Info", category));
            if (logger.IsInfoEnabled)
                logger.Info(message, ex);
        }

        /// <summary>
        /// InfoFormat
        /// </summary>
        /// <param name="format"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        /// <param name="args"></param>
        public static void InfoFormat(string format, string category = null, bool additivity = false, params object[] args)
        {
            HandMessage(format, args);
            string loggerName = GetLoggerName("Info", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Info", category));
            if (logger.IsInfoEnabled)
                logger.InfoFormat(format, args);
        }
        #endregion

        #region  Warn 警告
        /// <summary>
        /// Warn 警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        public static void Warn(object message,string category = null, bool additivity = false)
        {
            HandMessage(message);
            string loggerName = GetLoggerName("Warn", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Warn", category));
            if (logger.IsWarnEnabled)
                logger.Warn(message);
        }

        /// <summary>
        /// Warn 警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        public static void Warn(object message, Exception ex, string category = null, bool additivity = false)
        {
            HandMessage(message, ex);
            string loggerName = GetLoggerName("Warn", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Warn", category));
            if (logger.IsWarnEnabled)
                logger.Warn(message, ex);
        }

        /// <summary>
        /// WarnFormat 警告
        /// </summary>
        /// <param name="format"></param>
        /// <param name="category"></param>
        /// <param name="additivity"></param>
        /// <param name="args"></param>
        public static void WarnFormat(string format, string category = null, bool additivity = false, params object[] args)
        {
            HandMessage(format, args);
            string loggerName = GetLoggerName("Warn", category);
            ILog logger = CustomRollingFileLogger.GetCustomLogger(loggerName, GetCategory("Warn", category));
            if (logger.IsWarnEnabled)
                logger.WarnFormat(format, args);
        }
        #endregion

        #endregion

        #region 定义常规应用程序中未处理的异常信息记录方式
        public static void LoadUnhandledException()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((sender, e) =>
            {
                Fatal("未处理的异常", e.ExceptionObject as Exception);
            });
        }
        #endregion

        #region GetCategory 格式化日志文件路径
        /// <summary>
        /// GetCategory 格式化日志文件路径
        /// </summary>
        /// <param name="level"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        private static string GetCategory(string level, string category)
        {
            return  string.Format(@"{0}\{1}\{2}", string.Format("{0:yyyy-MM-dd}", DateTime.Now),category,level);
        }
        #endregion

        #region GetLoggerName 格式化日志文件名称
        /// <summary>
        /// GetLoggerName 格式化日志文件名称
        /// </summary>
        /// <param name="level"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        private static string GetLoggerName(string level, string category)
        {
            string loggerName = category.Replace(@"\", "_").Replace("\\","_");
            return string.Format("{0}_{1}", loggerName, level);
        }
        #endregion

        #region 记录日志并输出至UI

        #region LogMessage
        /// <summary>
        /// LogMessage
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="isDebug"></param>
        /// <param name="logMessage"></param>
        /// <param name="jobType"></param>
        public static void LogMessage(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, jobType);
            if (isDebug)
                Log4netUtil.LogDisplayHelper.LogMessage(logAppendToForms, logMessage);
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
        public static void LogWarning(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, jobType);
            Log4netUtil.Log4NetHelper.Warn(logMessage, jobType);
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
        public static void LogError(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, jobType);
            Log4netUtil.Log4NetHelper.Error(logMessage, jobType);
            if (isDebug)
                Log4netUtil.LogDisplayHelper.LogError(logAppendToForms, logMessage);
        }
        #endregion

        #endregion

    }



}


