using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Util
{
    /// <summary>
    /// 公共类库
    /// </summary>
    public class Common
    {

        #region StringIsNullOrEmpty
        /// <summary>
        /// StringIsNullOrEmpty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringIsNullOrEmpty(string value)
        {
            try
            {
                return string.IsNullOrEmpty(value) ? string.Empty : value;
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("Util.Common.StringIsNullOrEmpty() 执行失败，原因：{0}", ex.Message);
                Log4netUtil.Log4NetHelper.Info(logMessage, @"Util\Common");
                return string.Empty;
            }
        }
        #endregion

        #region IsInt 字符是否整形
        /// <summary>
        /// IsInt 字符是否整形
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            try
            {
                return Regex.IsMatch(value, @"^[+-]?\d*$");
            }catch(Exception ex)
            {
                string logMessage = string.Format("Util.Common.IsInt() 执行失败，原因：{0}", ex.Message);
                Log4netUtil.Log4NetHelper.Info(logMessage, @"Util\Common");
                return false;
            }
        }
        #endregion

        #region IsDouble 字符是否Double
        /// <summary>
        /// IsDouble 字符是否Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDouble(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            try
            {
                Convert.ToDouble(value);
                return true;
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("Util.Common.IsDouble() 执行失败，原因：{0}", ex.Message);
                Log4netUtil.Log4NetHelper.Info(logMessage, @"Util\Common");
                return false;
            }
        }
        #endregion

        #region IsDecimal 字符是否Decimal
        /// <summary>
        /// IsDecimal 字符是否Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            try
            {
                Convert.ToDecimal(value);
                return true;
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("Util.Common.ToDecimal() 执行失败，原因：{0}", ex.Message);
                Log4netUtil.Log4NetHelper.Info(logMessage, @"Util\Common");
                return false;
            }
        }
        #endregion

        #region StringRemoveChinese 移除字符窜中的中文
        /// <summary>
        /// StringRemoveChinese 移除字符窜中的中文
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringRemoveChinese(string value)
        {
            return Regex.Replace(value, @"[\u4e00-\u9fa5]", ""); //去除汉字  
        }
        #endregion

        #region StringExtractChinese 提取字符窜中的中文
        /// <summary>
        /// StringExtractChinese 提取字符窜中的中文
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringExtractChinese(string value)
        {
            return Regex.Replace(value, @"[^\u4e00-\u9fa5]", ""); //只留汉字
        }
        #endregion

        #region StringExtractNumber 提取字符窜中的数字  
        /// <summary>
        /// StringExtractNumber 提取字符窜中的数字  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringExtractNumber(string value)
        {
            return Regex.Replace(value, "[0-9]", "", RegexOptions.IgnoreCase); ////取出字符串中所有的数字  
        }
        #endregion

        #region StringExtractEnglish 提取字符窜中的英文字母   
        /// <summary>
        /// StringExtractEnglish 提取字符窜中的英文字母   
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringExtractEnglish(string value)
        {
            return Regex.Replace(value, "[a-z]", "", RegexOptions.IgnoreCase);//取出字符串中所有的英文字母   
        }
        #endregion

        #region StartDateTime 设置开始日期
        /// <summary>
        /// StartDateTime 设置开始日期
        /// </summary>
        /// <param name="dgv"></param>
        public static void StartDateTime(System.Windows.Forms.DateTimePicker dtp)
        {
            string configFile = System.Windows.Forms.Application.StartupPath.ToString() + "\\Config.ini";
            string advanceDays = Util.INIOperationClass.INIGetStringValue(configFile, "UserSet", "AdvanceDays", null);
            int idvanceDays = 1; // Int32.Parse(advanceDays);
            try
            {
                idvanceDays = Int32.Parse(advanceDays);
            }
            catch
            {
                idvanceDays = 1;
            }
            dtp.Value = DateTime.Now.AddDays(-idvanceDays);
        }
        #endregion


    }
}
