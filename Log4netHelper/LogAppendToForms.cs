using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Log4netUtil
{
    public delegate void LogAppendDelegate(Color color, string text);

    public class LogAppendToForms
    {
        public string _LogMessage = string.Empty;   //日志消息
        public Color _Color; //显示颜色
        public LogAppendDelegate _LogAppendDelegate; //委托
        
        /// <summary>
        /// 输出UI
        /// </summary>
        public  void Display()
        {
            _LogAppendDelegate(_Color, "【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString().Trim() + "】 :" + _LogMessage + "\r\n");
        }
     
    }

    public class LogDisplayHelper
    {
        #region LogMessage
        /// <summary>
        /// LogMessage 显示日志
        /// </summary>
        /// <param name="logMessage"></param>
        public static void LogMessage(LogAppendToForms logAppendToForms, string logMessage)
        {

            try
            {
                if (logAppendToForms != null)
                {
                    logAppendToForms._LogMessage = logMessage;
                    logAppendToForms._Color = Color.White;
                    logAppendToForms.Display();
                }
                Log4netUtil.Log4NetHelper.Info(logMessage, @"LogDisplay");
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(string.Format("消息{0};日志显示UI界面失败,失败原因{1}", logMessage, ex.Message), "QuartzManager");
            }
        }
        #endregion

        #region LogError
        /// <summary>
        /// LogError  显示错误日志
        /// </summary>
        /// <param name="logMessage"></param>
        public static void LogError(LogAppendToForms logAppendToForms, string logMessage)
        {
            try
            {
                if (logAppendToForms != null)
                {
                    logAppendToForms._LogMessage = logMessage;
                    logAppendToForms._Color = Color.LightCoral;
                    logAppendToForms.Display();
                }

                //logAppendToForms._LogMessage = logMessage;
                //logAppendToForms._Color = Color.LightCoral;
                //logAppendToForms.Display();
                Log4netUtil.Log4NetHelper.Info(logMessage, @"LogDisplay");
                Log4netUtil.Log4NetHelper.Error(logMessage, @"LogDisplay");
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(string.Format("消息{0};日志显示UI界面失败,失败原因{1}", logMessage, ex.Message), "QuartzManager");
            }
        }
        #endregion

        #region LogWarning
        /// <summary>
        /// LogWarning  显示警告信息
        /// </summary>
        /// <param name="color"></param>
        /// <param name="logMessage"></param>
        public static void LogWarning(LogAppendToForms logAppendToForms, string logMessage)
        {
            try
            {
                if (logAppendToForms != null)
                {
                    logAppendToForms._LogMessage = logMessage;
                    logAppendToForms._Color = Color.DarkGoldenrod;
                    logAppendToForms.Display();
                }
                //logAppendToForms._LogMessage = logMessage;
                //logAppendToForms._Color = Color.DarkGoldenrod;
                //logAppendToForms.Display();
                Log4netUtil.Log4NetHelper.Info(logMessage, @"LogDisplay");
                Log4netUtil.Log4NetHelper.Warn(logMessage, @"LogDisplay");
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(string.Format("消息{0};日志显示UI界面失败,失败原因{1}", logMessage, ex.Message), "QuartzManager");
            }
        }
                
        #endregion
    }



}
