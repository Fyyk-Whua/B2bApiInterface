using FluentFTP;
//using newsPlatform.Model.Comm;
using System;
using System.Collections.Generic;
using System.IO;

/******************************************************************
 * Content: FTP操作类(FluentFTP封装)
 ******************************************************************/

namespace Util
{
    /// <summary>
    /// FTP操作类(FluentFTP封装)
    /// </summary>
    public class FluentFtpHelper
    {
        #region 相关参数
        /// <summary>
        /// FtpClient
        /// </summary>
        private FluentFTP.FtpClient ftpClient = null;
        /// <summary>
        /// FTP IP地址(127.0.0.1)
        /// </summary>
        private string strFtpUri = string.Empty;
        /// <summary>
        /// FTP端口
        /// </summary>
        private int intFtpPort = 21;
        /// <summary>
        /// FTP用户名
        /// </summary>
        private string strFtpUserID = string.Empty;
        /// <summary>
        /// FTP密码
        /// </summary>
        private string strFtpPassword = string.Empty;
        /// <summary>
        /// 重试次数
        /// </summary>
        private int intRetryTimes = 3;
        /// <summary>
        /// FTP工作目录
        /// </summary>
        private string _workingDirectory = string.Empty;
        /// <summary>
        /// FTP工作目录
        /// </summary>
        public string WorkingDirectory
        {
            get
            {
                return _workingDirectory;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ftpConfig">FTP配置封装</param>
        public FluentFtpHelper(FtpConfig ftpConfig)
        {
            this.strFtpUri = ftpConfig.str_FtpUri;  //System.Web.HttpUtility.UrlEncode(ftpConfig.str_FtpUri);  //ftpConfig.str_FtpUri;
            this.intFtpPort = ftpConfig.int_FtpPort;
            this.strFtpUserID = ftpConfig.str_FtpUserID;
            this.strFtpPassword = ftpConfig.str_FtpPassword;
            this.intRetryTimes = ftpConfig.int_RetryTimes;
            //创建ftp客户端
            GetFtpClient();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">FTP IP地址</param>
        /// <param name="port">FTP端口</param>
        /// <param name="username">FTP用户名</param>
        /// <param name="password">FTP密码</param>
        public FluentFtpHelper(string host, int port, string username, string password)
        {
            this.strFtpUri = host; //System.Web.HttpUtility.UrlEncode(host);  //
            this.intFtpPort = port;
            this.strFtpUserID = username;
            this.strFtpPassword = password;
            //创建ftp客户端
            GetFtpClient();
        }
        #endregion

        #region 创建ftp客户端
        /// <summary>
        /// 创建ftp客户端
        /// </summary>
        private void GetFtpClient()
        {
            if (CheckPara())
            {
                try
                {
                    ftpClient = new FluentFTP.FtpClient(strFtpUri, intFtpPort, strFtpUserID, strFtpPassword);
                    ftpClient.RetryAttempts = intRetryTimes;
                }
                catch (Exception ex)
                {
                    Log4netUtil.Log4NetHelper.Error(String.Format(@"GetFtpClient->创建ftp客户端异常:{0}", ex.Message), @"Ftp");
                }
            }
        }
        #endregion

        #region 校验参数
        /// <summary>
        /// 校验参数
        /// </summary>
        /// <returns></returns>
        private bool CheckPara()
        {
            bool boolResult = true;

            if (string.IsNullOrEmpty(strFtpUri))
            {
                Log4netUtil.Log4NetHelper.Error(String.Format(@"CheckPara->FtpUri为空:{0}", string.Empty), @"Ftp");
                return false;
            }
            if (string.IsNullOrEmpty(strFtpUserID))
            {
                //Log4NetUtil.Error(this, "CheckPara->FtpUserID为空");
                Log4netUtil.Log4NetHelper.Error(String.Format(@"CheckPara->FtpUserID为空:{0}", string.Empty), @"Ftp");
                return false;
            }
            if (string.IsNullOrEmpty(strFtpPassword))
            {
                //Log4NetUtil.Error(this, "CheckPara->FtpPassword为空");
                Log4netUtil.Log4NetHelper.Error(String.Format(@"CheckPara->FtpPassword为空:{0}", string.Empty), @"Ftp");
                return false;
            }
            if (intFtpPort == 0 || intFtpPort == int.MaxValue || intFtpPort == int.MinValue)
            {
                Log4netUtil.Log4NetHelper.Error(String.Format(@"CheckPara->intFtpPort异常:{0}", intFtpPort.ToString()), @"Ftp");
                return false;
            }
            return boolResult;
        }
        #endregion

        #region FTP是否已连接
        /// <summary>
        /// FTP是否已连接
        /// </summary>
        /// <returns></returns>
        public bool isConnected()
        {
            bool result = false;
            if (ftpClient != null)
            {
                result = ftpClient.IsConnected;
            }
            return result;
        }
        #endregion

        #region 连接FTP
        /// <summary>
        /// 连接FTP
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            bool result = false;
            if (ftpClient != null)
            {
                if (ftpClient.IsConnected)
                {
                    Log4netUtil.Log4NetHelper.Info(String.Format(@"Connect->连接FTP成功:{0}", string.Empty), @"Ftp");
                    return true;
                }
                else
                {
                    try
                    {
                        ftpClient.Connect();
                        Log4netUtil.Log4NetHelper.Info(String.Format(@"Connect->连接FTP成功:{0}", string.Empty), @"Ftp");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Log4netUtil.Log4NetHelper.Error(String.Format(@"Connect->连接FTP异常:{0}", ex.Message), @"Ftp");
                        return false;
                    }
                }
            }
            return result;
        }
        #endregion

        #region 断开FTP
        /// <summary>
        /// 断开FTP
        /// </summary>
        public void DisConnect()
        {
            if (ftpClient != null)
            {
                if (ftpClient.IsConnected)
                {
                    ftpClient.Disconnect();
                }
            }
        }
        #endregion

        #region 取得文件或目录列表
        /// <summary>
        /// 取得文件或目录列表
        /// </summary>
        /// <param name="remoteDic">远程目录</param>
        /// <param name="type">类型:file-文件,dic-目录</param>
        /// <returns></returns>
        public List<string> ListDirectory(string remoteDic, string type = "file")
        {
            List<string> list = new List<string>();
            type = type.ToLower();

            try
            {
                if (Connect())
                {
                    FtpListItem[] files = ftpClient.GetListing(remoteDic);
                    foreach (FtpListItem file in files)
                    {
                        if (type == "file")
                        {
                            if (file.Type == FtpFileSystemObjectType.File)
                            {
                                list.Add(file.Name);
                            }
                        }
                        else if (type == "dic")
                        {
                            if (file.Type == FtpFileSystemObjectType.Directory)
                            {
                                list.Add(file.Name);
                            }
                        }
                        else
                        {
                            list.Add(file.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(String.Format(@"ListDirectory->取得文件或目录列表 异常:{0}", ex.Message), @"Ftp");
            }
            finally
            {
                DisConnect();
            }

            return list;
        }
        #endregion

        #region 上传单文件
        /// <summary>
        /// 上传单文件
        /// </summary>
        /// <param name="localPath">本地路径(@"D:\abc.txt")</param>
        /// <param name="remoteDic">远端目录("/test")</param>
        /// <param name="remoteName">远端文件名称("/test")</param>
        /// <returns></returns>
        public bool UploadFile(string localPath, string remoteDic, string remoteName)
        {
            Log4netUtil.Log4NetHelper.Info(String.Format(@"调用 UploadFile->上传单文件  参数 localPath：{0} ; remoteDic: {1} ;remoteName: {2} ;", localPath, remoteDic, remoteName), @"Ftp");
            bool boolResult = false;
            FileInfo fileInfo = null;

            try
            {
                //本地路径校验
                if (!File.Exists(localPath))
                {
                    //Log4NetUtil.Error(this, "UploadFile->本地文件不存在:" + localPath);
                    Log4netUtil.Log4NetHelper.Error(String.Format(@"UploadFile->本地文件不存在:{0}", localPath), @"Ftp");
                    return boolResult;
                }
                else
                {
                    fileInfo = new FileInfo(localPath);
                }
                //远端路径校验
                if (string.IsNullOrEmpty(remoteDic))
                {
                    remoteDic = "/";
                }
                if (!remoteDic.StartsWith("/"))
                {
                    remoteDic = "/" + remoteDic;
                }
                if (!remoteDic.EndsWith("/"))
                {
                    remoteDic += "/";
                }

                //拼接远端路径
                remoteDic = string.Format("{0}{1}{2}", remoteDic, remoteName, Path.GetExtension(localPath));
                if (Connect())
                {
                    using (FileStream fs = fileInfo.OpenRead())
                    {
                        //重名覆盖
                        boolResult = ftpClient.Upload(fs, remoteDic, FtpExists.Overwrite, true);
                    }
                }
                if (boolResult)
                    Log4netUtil.Log4NetHelper.Info(String.Format(@" UploadFile->上传单文件成功！  ;参数 localPath：{0} ; remoteDic: {1} ;remoteName: {2} ;", localPath, remoteDic, remoteName), @"Ftp");
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(String.Format(@"UploadFile->上传文件 异常:{0}  ; 参数 localPath：{1} ; remoteDic: {2} ;remoteName: {3}", ex.Message, localPath, remoteDic, remoteName), @"Ftp");
            }
            finally
            {
                DisConnect();
            }

            return boolResult;
        }
        #endregion

        #region 上传多文件
        /// <summary>
        /// 上传多文件
        /// </summary>
        /// <param name="localFiles">本地路径列表</param>
        /// <param name="remoteDic">远端目录("/test")</param>
        /// <returns></returns>
        public int UploadFiles(IEnumerable<string> localFiles, string remoteDic)
        {
            int count = 0;
            List<FileInfo> listFiles = new List<FileInfo>();

            if (localFiles == null)
            {
                return 0;
            }

            try
            {
                foreach (string file in localFiles)
                {
                    if (!File.Exists(file))
                    {
                        Log4netUtil.Log4NetHelper.Error(String.Format(@"UploadFile->本地文件不存在:{0}", file), @"Ftp");
                        continue;
                    }
                    listFiles.Add(new FileInfo(file));
                }

                //远端路径校验
                if (string.IsNullOrEmpty(remoteDic))
                {
                    remoteDic = "/";
                }
                if (!remoteDic.StartsWith("/"))
                {
                    remoteDic = "/" + remoteDic;
                }
                if (!remoteDic.EndsWith("/"))
                {
                    remoteDic += "/";
                }

                if (Connect())
                {
                    if (listFiles.Count > 0)
                    {
                        count = ftpClient.UploadFiles(listFiles, remoteDic, FtpExists.Overwrite, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(String.Format(@"UploadFile->上传文件 异常:{0}", ex.ToString()), @"Ftp");
            }
            finally
            {
                DisConnect();
            }

            return count;
        }
        #endregion

        #region 下载单文件
        /// <summary>
        /// 下载单文件
        /// </summary>
        /// <param name="localDic">本地目录(@"D:\test")</param>
        /// <param name="downloadFileName">下载文件名称("abc.txt")</param>
        /// <param name="remotePath">远程路径("/test/abc.txt")</param>
        /// <returns></returns>
        public bool DownloadFile(string localDic, string downloadFileName, string remotePath)
        {
            Log4netUtil.Log4NetHelper.Info(String.Format(@"调用 DownloadFile->下载文件  参数 localDic：{0} ; downloadFileName: {1} ;remotePath: {2} ;", localDic, downloadFileName, remotePath), @"Ftp");
            bool boolResult = false;
            string strFileName = string.Empty;
            string remotePathUrlEncode = remotePath;
            try
            {
                //本地目录不存在，则自动创建
                if (!Util.FileHelper.IsValidPathChars(localDic))
                {
                    Log4netUtil.Log4NetHelper.Error(String.Format(@"DownloadFile->下载文件 异常:{0}  参数 localDic：{1} ; downloadFileName: {2} ;remotePath: {3} ;remotePathUrlEncode:{4}", "目录有非法字符", localDic, downloadFileName, remotePath, remotePathUrlEncode), @"Ftp");
                    return boolResult;
                }
                if (!Directory.Exists(localDic))
                    Directory.CreateDirectory(localDic);
                //取下载文件的文件名
                if (string.IsNullOrEmpty(downloadFileName))
                    strFileName = Path.GetFileName(remotePath);
                else
                    strFileName = downloadFileName;
                //拼接本地路径
                localDic = Path.Combine(localDic, strFileName);

                if (Connect())
                    boolResult = ftpClient.DownloadFile(localDic, remotePathUrlEncode, FtpLocalExists.Overwrite);
                if (boolResult)
                    Log4netUtil.Log4NetHelper.Info(String.Format(@"DownloadFile->下载文件成功！ ;参数 localDic：{0} ; downloadFileName: {1} ;remotePath: {2} ;", localDic, downloadFileName, remotePath), @"Ftp");
            }
            catch (Exception ex)
            {
                 Log4netUtil.Log4NetHelper.Error(String.Format(@"DownloadFile->下载文件 异常:{0}  参数 localDic：{1} ; downloadFileName: {2} ;remotePath: {3} ;remotePathUrlEncode:{4}", ex.ToString(), localDic, downloadFileName, remotePath, remotePathUrlEncode), @"Ftp");
            }
            finally
            {
                DisConnect();
            }
            return boolResult;
        }
        #endregion

        #region 下载多文件
        /// <summary>
        /// 下载多文件
        /// </summary>
        /// <param name="localDic">本地目录(@"D:\test")</param>
        /// <param name="remotePath">远程路径列表</param>
        /// <returns></returns>
        public int DownloadFiles(string localDic, IEnumerable<string> remoteFiles)
        {
            int count = 0;
            if (remoteFiles == null)
            {
                return 0;
            }

            try
            {
                //本地目录不存在，则自动创建
                if (!Directory.Exists(localDic))
                {
                    Directory.CreateDirectory(localDic);
                }

                if (Connect())
                {
                    count = ftpClient.DownloadFiles(localDic, remoteFiles, FtpLocalExists.Overwrite); //FtpExists.Overwrite);//  FtpLocalExists.Overwrite);
                }
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(String.Format(@"DownloadFile->下载文件 异常:{0}", ex.ToString()), @"Ftp");
            }
            finally
            {
                DisConnect();
            }

            return count;
        }
        #endregion

        #region 判断文件是否存在
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="remotePath">远程路径("/test/abc.txt")</param>
        /// <returns></returns>
        public bool IsFileExists(string remotePath)
        {
            bool boolResult = false;

            try
            {
                if (Connect())
                {
                    boolResult = ftpClient.FileExists(remotePath);
                }
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(String.Format(@"IsFileExists->判断文件是否存在 异常:{0} |*|remotePath: {1}", ex.ToString(), remotePath), @"Ftp");
            }
            finally
            {
                DisConnect();
            }

            return boolResult;
        }
        #endregion

        #region 判断目录是否存在
        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="remotePath">远程路径("/test")</param>
        /// <returns></returns>
        public bool IsDirExists(string remotePath)
        {
            bool boolResult = false;

            try
            {
                if (Connect())
                {
                    boolResult = ftpClient.DirectoryExists(remotePath);
                }
            }
            catch (Exception ex)
            {
                 Log4netUtil.Log4NetHelper.Error(String.Format(@"IsDirExists->判断目录是否存在 异常:{0} |*|remotePath: {1}", ex.ToString(), remotePath), @"Ftp");
            }
            finally
            {
                DisConnect();
            }

            return boolResult;
        }
        #endregion

        #region 新建目录
        /// <summary>
        /// 新建目录
        /// </summary>
        /// <param name="remoteDic">远程目录("/test")</param>
        /// <returns></returns>
        public bool MakeDir(string remoteDic)
        {
            bool boolResult = false;
            try
            {
                if (Connect())
                {
                    ftpClient.CreateDirectory(remoteDic);
                    boolResult = true;
                }
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(String.Format(@"MakeDir->新建目录 异常:{0} |*|remoteDic: {1}", ex.ToString(), remoteDic), @"Ftp");
            }
            finally
            {
                DisConnect();
            }
            return boolResult;
        }
        #endregion

        #region 清理
        /// <summary>
        /// 清理
        /// </summary>
        public void Clean()
        {
            //断开FTP
            DisConnect();

            if (ftpClient != null)
            {
                ftpClient.Dispose();
            }
        }
        #endregion

        #region  GetResult
        /// <summary>
        /// GetResult
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetResult(string code, string msg, string data)
        {
            Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();

            jObject.Add("code", code);
            jObject.Add("msg", msg);
            jObject.Add("data", data);
            return jObject.ToString();
        }
        #endregion

        public string GB2312_ISO8859(string write)
        {
            //声明字符集  
            System.Text.Encoding iso8859, gb2312;
            //iso8859  
            iso8859 = System.Text.Encoding.GetEncoding("iso8859-1");
            //国标2312  
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] gb;
            gb = gb2312.GetBytes(write);
            //返回转换后的字符  
            return iso8859.GetString(gb);
        }
    }

    #region Ftp配置结构
    /// <summary>
    /// Ftp配置结构
    /// </summary>
    public class FtpConfig
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public FtpConfig()
        {
            this.int_FtpReadWriteTimeout = 60000;
            this.bool_FtpUseBinary = true;
            this.bool_FtpUsePassive = true;
            this.bool_FtpKeepAlive = true;
            this.bool_FtpEnableSsl = false;
            this.int_RetryTimes = 3;
        }
        #endregion

        /// <summary>
        /// Ftp 标识
        /// </summary>
        public string str_Name { get; set; }
        /// <summary>
        /// FTP地址
        /// </summary>
        public string str_FtpUri { get; set; }
        /// <summary>
        /// FTP端口
        /// </summary>
        public int int_FtpPort { get; set; }
        /// <summary>
        /// FTP路径(/test)
        /// </summary>
        public string str_FtpPath { get; set; }
        /// <summary>
        /// FTP用户名
        /// </summary>
        public string str_FtpUserID { get; set; }
        /// <summary>
        /// FTP密码
        /// </summary>
        public string str_FtpPassword { get; set; }
        /// <summary>
        /// FTP密码是否被加密
        /// </summary>
        public bool bool_IsEncrypt { get; set; }
        /// <summary>
        /// 读取或写入超时之前的毫秒数。默认值为 30,000 毫秒。
        /// </summary>
        public int int_FtpReadWriteTimeout { get; set; }
        /// <summary>
        /// true，指示服务器要传输的是二进制数据；false，指示数据为文本。默认值为true。
        /// </summary>
        public bool bool_FtpUseBinary { get; set; }
        /// <summary>
        /// true，被动模式；false，主动模式(主动模式可能被防火墙拦截)。默认值为true。
        /// </summary>
        public bool bool_FtpUsePassive { get; set; }
        /// <summary>
        /// 是否保持连接。
        /// </summary>
        public bool bool_FtpKeepAlive { get; set; }
        /// <summary>
        /// 是否启用SSL。
        /// </summary>
        public bool bool_FtpEnableSsl { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string str_Describe { get; set; }
        /// <summary>
        /// 重试次数
        /// </summary>
        public int int_RetryTimes { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string str_Ver { get; set; }
    }
    #endregion

}


