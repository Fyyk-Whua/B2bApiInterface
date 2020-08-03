using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Util
{
    public class ImageDownloadPrint
    {
        #region ImagePrint 
        /// <summary>
        /// EleInvPrint 打印电子发票
        /// </summary>
        /// <param name="isAuto"></param>
        /// <param name="eInvoicePrintCopies"></param>
        /// <param name="paperSizeRawKind"></param>
        /// <param name="url"></param>
        /// <param name="DataExchangeId"></param>
        /// <param name="printerName"></param>
        /// <returns></returns>
        public static bool ImagePrint(Log4netUtil.LogAppendToForms logAppendToForms,
                                      Model.JobEntity jobInfo,
                                      string url, string fileName)
        {
            string logMessage = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    logMessage = string.Format("【{0}_{1}】  下载及打印文件失败 ;原因:文件路径为空", jobInfo.JobCode, jobInfo.JobName);
                    Log4netUtil.Log4NetHelper.LogError(logAppendToForms, true, logMessage, @"Ftp");
                    return false;
                }
                return OpenUrlDownloadFile(logAppendToForms, jobInfo, url, fileName);
            }
            catch (Exception ex)
            {
                logMessage = string.Format("【{0}_{1}】  下载及打印文件失败 ;原因:{2}", jobInfo.JobCode, jobInfo.JobName, ex.Message);
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, true, logMessage, @"Ftp");
                return false;
            }

        }
        #endregion




        #region OpenUrlDownloadFile 打开网址并下载文件
        /// <summary>
        /// 打开网址并下载文件
        /// </summary>
        /// <param name="URL">下载文件地址</param>
        /// <param name="Filename">下载后另存为（全路径）</param>
        private static bool OpenUrlDownloadFile(Log4netUtil.LogAppendToForms logAppendToForms,
                                                Model.JobEntity jobInfo,
                                                string url, string filename)
        {
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, (int)by.Length);
                }
                so.Close();
                st.Close();
                myrp.Close();
                Myrq.Abort();
                return true;
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("【{0}_{1}】  下载文件失败 ;原因:{2}", jobInfo.JobCode, jobInfo.JobName, ex.Message);
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, true, logMessage, @"Ftp");
                return false;
            }
        }
        #endregion

        #region  CopyToFile 复制文件到指写文件夹
        /// <summary>
        /// 复制文件到指写文件夹
        /// </summary>
        /// <param name="sourceFile">源文件</param>
        /// <param name="destinationFile">目标文件</param>
        /// <returns> 成功 true  失败 false </returns>
        private static bool CopyToFile(Log4netUtil.LogAppendToForms logAppendToForms,
                                       Model.JobEntity jobInfo,
                                       string sourceFile, string destinationFile)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(sourceFile);
            try
            {
                if (file.Exists)
                {
                    file.CopyTo(destinationFile, true);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("【{0}_{1}】  复制文件失败 ;原因:{2}", jobInfo.JobCode, jobInfo.JobName, ex.Message);
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, true, logMessage, @"Ftp");
                return false;
            }
        }

        #endregion
    }
}
