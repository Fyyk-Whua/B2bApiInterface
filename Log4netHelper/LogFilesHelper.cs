
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
namespace Log4netUtil
{
    public class FilesHelper
    {
        //private static string _Path = string.Empty;// ConfigurationManager.AppSettings["path"];
        //private static int _BeforeDay = 0; //int.Parse(ConfigurationManager.AppSettings["day"].ToString());
        public static void DelFiles(string path,int beforeDay)
        {
            string[] folders = Directory.GetDirectories(path);
            for (int i = 0; i < folders.Length; i++)
            {
                DirectoryInfo di = new DirectoryInfo(folders[i]);
                string folderName = di.Name;
                if (folderName == "_gsdata_")//排除_gsdata_
                {
                    continue;
                }

                string[] files = Directory.GetFiles(folders[i]);
                for (int j = 0; j < files.Length; j++)
                {
                    string fileName = Path.GetFileName(files[j]);
                    FileInfo file = new FileInfo(files[j]);
                    DateTime dt = file.CreationTime;
                    TimeSpan ts = DateTime.Parse(DateTime.Now.ToShortDateString()) - dt;

                    int day = ts.Days;
                    if (day > beforeDay)
                    {
                        //File.Delete(files[j]);
                        string msg = string.Format("delete source file -{0}", files[j]);
                        //LogHelper.Info(msg);
                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + msg);
                    }
                }
            }
        }

        /// <summary>
        /// 删除文件夹及子文件内文件
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public static void DeleteALLFiles(LogAppendToForms logAppendToForms, string path,int beforeDay)
        {
            string jobType = "DelLogFiles";
            string logMessage = string.Empty;
            try
            {
                DirectoryInfo fatherFolder = new DirectoryInfo(path);
                //删除当前文件夹内文件
                FileInfo[] files = fatherFolder.GetFiles();
                foreach (FileInfo f in files)
                {
                    string filePath = f.FullName;
                    string fileName = Path.GetFileName(filePath);
                    FileInfo file = new FileInfo(filePath);
                    DateTime dt = DateTime.Parse(file.CreationTime.ToShortDateString());
                    TimeSpan ts = DateTime.Parse(DateTime.Now.ToShortDateString()) - dt;

                    int day = ts.Days;
                    if (day > beforeDay)
                    {
                        File.Delete(file.FullName);
                        logMessage = string.Format("【{0}】删除日志文件成功！日志路径：{1}", "清除日志计划", filePath);
                        Log4NetHelper.LogMessage(logAppendToForms, true, logMessage, jobType);
                        // Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + msg);
                    }
                }
                if (Directory.Exists(path))
                {
                    if (Directory.GetDirectories(path).Length == 0 && Directory.GetFiles(path).Length == 0)
                    {
                        try
                        {
                            fatherFolder.Delete();
                            logMessage = string.Format("【{0}】删除日志文件夹成功！日志路径：{1}", "清除日志计划", path);
                            Log4NetHelper.LogMessage(logAppendToForms, true, logMessage, jobType);
                        }
                        catch (Exception ex)
                        {
                            logMessage = string.Format("【{0}】删除日志文件夹失败！失败原因：{1}", "清除日志计划", ex.Message);
                            Log4NetHelper.LogError(logAppendToForms, true, logMessage, jobType);
                        }
                    }
                    else
                    {
                        foreach (DirectoryInfo childFolder in fatherFolder.GetDirectories())
                        {
                            path = childFolder.FullName;
                            DeleteALLFiles(logAppendToForms, path, beforeDay);
                        }
                    }
                }
                

            }
            catch (Exception ex)
            {
                logMessage = string.Format("【{0}】删除日志文件失败！失败原因：{1}", "清除日志计划",ex.Message);
                Log4NetHelper.LogError(logAppendToForms, true, logMessage, jobType);
            }
        }
    }
}
