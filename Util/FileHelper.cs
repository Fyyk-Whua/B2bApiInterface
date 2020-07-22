using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Util
{
    public class FileHelper
    {

        #region 文件、目录名称有效性判断
        /// <summary>
        /// 判断目录名中字符是否合法
        /// </summary>
        /// <param name="DirectoryName">目录名称</param>
        public static bool IsValidPathChars(string DirectoryName)
        {
            char[] invalidPathChars = Path.GetInvalidPathChars();
            char[] DirChar = DirectoryName.ToCharArray();
            foreach (char C in DirChar)
            {
                if (Array.BinarySearch(invalidPathChars, C) >= 0)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 判断文件名中字符是否合法
        /// </summary>
        /// <param name="FileName">文件名称</param>
        public static bool IsValidFileChars(string FileName)
        {
            char[] invalidFileChars = Path.GetInvalidFileNameChars();
            char[] NameChar = FileName.ToCharArray();
            foreach (char C in NameChar)
            {
                if (Array.BinarySearch(invalidFileChars, C) >= 0)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region DelectDir 删除文件夹下所有文件
        /// <summary>
        /// 删除文件夹下所有文件
        /// </summary>
        /// <param name="srcPath"></param>
        public static bool DelectDir(string srcPath)
        {
            if (!Directory.Exists(srcPath))
                throw new ArgumentException("文件夹无效", "srcPath");
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        string filePaht = i.FullName;
                        if ((File.GetAttributes(filePaht) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            File.SetAttributes(filePaht, FileAttributes.Normal); // 如果是将文件的属性设置为Normal
                        File.Delete(filePaht);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new IOException(String.Format("删除文件夹失败：{ex.Message}", ex.Message));
            }
        }
        #endregion

        #region DelectDir 删除文件
        /// <summary>
        /// 删除文件夹下所有文件
        /// </summary>
        /// <param name="srcPath"></param>
        public static bool DelFileName(Log4netUtil.LogAppendToForms logAppendToForms, string fileFullPath,string info)
        {
            try
            {
                // 删除该文件
                System.IO.File.Delete(fileFullPath);
                return true;
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("{0} 删除文件失败！文件路径:{1} ; 原因,{2}", info, fileFullPath, ex.Message);
                Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, true, logMessage, @"Util\FileHelper");
                return false;
            }
        }
        #endregion

        #region DelFileName 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="FileName"></param>
        public static bool DelFileName(string FileName, bool IsError)
        {
            string srcPath = Path.GetDirectoryName(FileName);
            if (!File.Exists(FileName))
                throw new ArgumentException("文件夹无效", "FileName");
            try
            {
                if ((File.GetAttributes(FileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    File.SetAttributes(FileName, FileAttributes.Normal); // 如果是将文件的属性设置为Normal
                File.Delete(FileName); // 删除
                return true;
            }
            catch (Exception ex)
            {
                if (IsError)
                    throw new IOException(String.Format("删除文件夹失败：{0}", ex.Message));
                else
                    return false;
            }
        }
        #endregion

        #region ModifyFileName 文件改名
        /// <summary>
        /// 文件改名
        /// </summary>
        /// <param name="srcRelativePath"></param>
        /// <returns></returns>
        private static bool ModifyFileName(string srcRelativePath)
        {
            if (!File.Exists(srcRelativePath))
                throw new ArgumentException("文件无效", "srcRelativePath");
            try
            {
                string getExtension = Path.GetExtension(srcRelativePath);//修改文件后缀
                string desRelativePath = srcRelativePath.Replace(getExtension, ".fyyk");
                //string newDirectoryName = Path.Combine(path1, newFileName);
                // fi.MoveTo(newDirectoryName);//必须有这步
                System.IO.File.Move(srcRelativePath, desRelativePath);
                return true;
            }
            catch (Exception ex)
            {
                throw new IOException(String.Format("文件改名失败失败：{ex.Message}", ex.Message));
            }
        }
        #endregion

        #region CreateFilePath
        /// <summary>
        /// CreateFilePath
        /// </summary>
        /// <param name="srcRelativePath"></param>
        /// <param name="tempFilePath"></param>
        /// <returns></returns>
        public static string CreateFilePath(string srcRelativePath, string tempFilePath)
        {
            string fileSavePath = System.Windows.Forms.Application.StartupPath + tempFilePath;// "\\KjjImgTemp\\FtpTemp";
            try
            {
                if (!System.IO.Directory.Exists(fileSavePath))
                    System.IO.Directory.CreateDirectory(fileSavePath);
                return fileSavePath;

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("创建目录失败！" + ex.Message,
                                                     "错误",
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Error);
                return "9999";
            }
        }
        #endregion

        #region CreateFilePath
        /// <summary>
        /// CreateFilePath
        /// </summary>
        /// <param name="srcRelativePath"></param>
        /// <param name="tempFilePath"></param>
        /// <returns></returns>
        public static string CreateFilePath(string srcRelativePath)
        {
            string fileSavePath = srcRelativePath;
            try
            {
                if (!System.IO.Directory.Exists(fileSavePath))
                    System.IO.Directory.CreateDirectory(fileSavePath);
                return fileSavePath;

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("创建目录失败！" + ex.Message,
                                                     "错误",
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Error);
                return "9999";
            }
        }
        #endregion

        #region OpenUrlDownloadFile 打开网址并下载文件
        /// <summary>
        /// 打开网址并下载文件
        /// </summary>
        /// <param name="url">下载文件地址</param>
        /// <param name="Filename">下载后另存为（全路径）</param>
        public static bool OpenUrlDownloadFile(Log4netUtil.LogAppendToForms logAppendToForms, string url, string filename, bool isDebug)
        {
            return openUrlDownloadFile(logAppendToForms,url, filename, isDebug);
        }
        private static bool openUrlDownloadFile(Log4netUtil.LogAppendToForms logAppendToForms, string url, string filename, bool isDebug)
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
                string logMessage = string.Format("【随货同行下载任务】 Url {0} 下载失败！原因,{1}", url, ex.Message);
                Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, isDebug, logMessage, @"Util\FileHelper");
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 打印指定文件的详细信息
        /// </summary>
        /// <param name="path">指定文件的路径</param>
        public static double PrintFileVersionInfo(string path)
        {
            System.IO.FileInfo fileInfo = null;
            try
            {
                fileInfo = new System.IO.FileInfo(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // 其他处理异常的代码
            }
            // 如果文件存在
            if (fileInfo != null && fileInfo.Exists)
            {
                System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
                /* Console.WriteLine("文件名称=" + info.FileName);
                 Console.WriteLine("产品名称=" + info.ProductName);
                 Console.WriteLine("公司名称=" + info.CompanyName);
                 Console.WriteLine("文件版本=" + info.FileVersion);
                 Console.WriteLine("产品版本=" + info.ProductVersion);
                 // 通常版本号显示为「主版本号.次版本号.生成号.专用部件号」
                 Console.WriteLine("系统显示文件版本：" + info.ProductMajorPart + '.' + info.ProductMinorPart + '.' + info.ProductBuildPart + '.' + info.ProductPrivatePart);
                 Console.WriteLine("文件说明=" + info.FileDescription);
                 Console.WriteLine("文件语言=" + info.Language);
                 Console.WriteLine("原始文件名称=" + info.OriginalFilename);
                 Console.WriteLine("文件版权=" + info.LegalCopyright);*/

                //Console.WriteLine("文件大小=" + System.Math.Ceiling(fileInfo.Length / 1024.0) + " KB");
                //const double GB = 1024 * 1024 * 1024.0;
                const double MB = 1024 * 1024.0;
                //const double KB = 1024.0;
                //return System.Math.Ceiling(fileInfo.Length / (MB));
                return fileInfo.Length / (MB);
            }
            else
            {
                //Console.WriteLine("指定的文件路径不正确!");
                return 0;
            }
            // 末尾空一行
            //Console.WriteLine();
        }


        /// <summary>
        /// Bytes到KB,MB,GB,TB单位智能转换
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ConvertBytes(long len)
        {
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}

