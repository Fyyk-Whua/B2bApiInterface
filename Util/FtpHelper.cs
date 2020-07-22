using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Util
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: FtpHelper.cs   
    ///* 命名空间: Util.FrameUtil
    ///* 功    能: 操作Ftp基类
    ///* 内    容: 
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 调用方法展示，
    ///*     var ftp = new FtpHelper(“127.0.0.1”, “fyyk”, “fyyk”);//初始化ftp，创建ftp对象 
    ///*     ftp.DelAll(“test”);//删除ftptest目录及其目录下的所有文件 
    ///*     ftp.UploadAllFile(“F:\test\wms.zip”);//上传单个文件到指定目录 
    ///*     ftp.UploadAllFile(“F:\test”);//将本地test目录的所有文件上传 
    ///*     ftp.DownloadFile(“test\wms.zip”, “F:\test1”);//下载单个目录 
    ///*     ftp.DownloadAllFile(“test”, “F:\test1”);//批量下载整个目录 
    ///*     ftp.MakeDir(“aaa\bbb\ccc\ddd”);//创建多级目录
    ///*
    ///* 版权说明:  Copyright 2018-2027 武汉飞宇益克科技有限公司
    ///**************************************************************************/
    public class FtpHelper
    {
        #region Ftp参数

        #region LogAppendToForms
        private Log4netUtil.LogAppendToForms logAppendToForms = null;
        /// <summary>
        /// 主机
        /// </summary>
        public Log4netUtil.LogAppendToForms _LogAppendToForms
        {
            get { return this.logAppendToForms ?? null; }
        }
        #endregion

        #region _FtpHostIP 主机
        private string ftpHostIP = string.Empty;
        /// <summary>
        /// 主机
        /// </summary>
        public string _FtpHostIP
        {
            get { return this.ftpHostIP ?? string.Empty; }
        }
        #endregion

        #region _FtpUserName 登录用户名
        private string ftpUserName = string.Empty;
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string _FtpUserName
        {
            get { return this.ftpUserName; }
        }
        #endregion

        #region _FtpPassword 密码
        private string ftpPassword = string.Empty;
        /// <summary>
        /// 密码
        /// </summary>
        public string _FtpPassword
        {
            get { return this.ftpPassword; }
        }

        #endregion

        #region _FtpProxy 代理
        IWebProxy ftpProxy = null;
        /// <summary>
        /// 代理
        /// </summary>
        public IWebProxy _FtpProxy
        {
            get { return this.ftpProxy; }
            set { this.ftpProxy = value; }
        }
        #endregion

        #region _FtpPort 端口
        private int ftpPort = 21;
        /// <summary>
        /// 端口
        /// </summary>
        public int _FtpPort
        {
            get { return ftpPort; }
            set { this.ftpPort = value; }
        }
        #endregion

        #region _FtpEnableSsl 设置是否允许Ssl
        private bool ftpEnableSsl = false;
        /// <summary>
        /// EnableSsl
        /// </summary>
        public bool _FtpEnableSsl
        {
            get { return ftpEnableSsl; }
        }
        #endregion

        #region _FtpUsePassive 使用被动模式
        private bool ftpUsePassive = true;
        /// <summary>
        /// 被动模式
        /// </summary>
        public bool _FtpUsePassive
        {
            get { return ftpUsePassive; }
            set { this.ftpUsePassive = value; }
        }
        #endregion

        #region _FtpUseBinary 二进制方式
        private bool ftpUseBinary = true;
        /// <summary>
        /// 二进制方式
        /// </summary>
        public bool _FtpUseBinary
        {
            get { return ftpUseBinary; }
            set { this.ftpUseBinary = value; }
        }
        #endregion

        #region _FtpRemotePath 远端路径
        private string ftpRemotePath = "/";
        /// <summary>
        /// 远端路径
        /// <para>
        ///     返回FTP服务器上的当前路径(可以是 / 或 /a/../ 的形式)
        /// </para>
        /// </summary>
        public string _FtpRemotePath
        {
            get { return ftpRemotePath; }
            set
            {
                string result = "/";
                if (!string.IsNullOrEmpty(value) && value != "/")
                    result = "/" + value.TrimStart('/').TrimEnd('/') + "/";
                this.ftpRemotePath = result;
            }
        }
        #endregion

        #region _FtpBuffLength
        private int ftpBuffLength = 2048;
        /// <summary>
        ///_FtpBuffLength
        /// </summary>
        public int _FtpBuffLength
        {
            get { return ftpBuffLength; }
            set { this.ftpBuffLength = value; }
        }
        #endregion

        #region _FtpURI
        private string ftpURI = string.Empty;
        /// <summary>
        /// _FtpURI
        /// </summary>
        public string _FtpURI
        {
            get { return this._FtpURI ?? string.Empty; }
        }
        #endregion
        #endregion

        #region FtpHelper 初始化ftp参数
        /// <summary>
        /// 创建FTP工具
        /// <para>
        /// 默认不使用SSL,使用二进制传输方式,使用被动模式
        /// </para>
        /// </summary>
        /// <param name="host">主机名称</param>
        /// <param name="userId">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="buffLength">buffLength</param>
        public FtpHelper(Log4netUtil.LogAppendToForms logAppendToForms,
                         string ftpHostIP, string username, string password, int buffLength) : this(logAppendToForms,ftpHostIP, username, password, 21, null, false, true, true, buffLength)
        { }

        /// <summary>
        /// 创建FTP工具
        /// </summary>
        /// <param name="host">主机名称</param>
        /// <param name="userId">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="port">端口</param>
        /// <param name="enableSsl">允许Ssl</param>
        /// <param name="proxy">代理</param>
        /// <param name="useBinary">允许二进制</param>
        /// <param name="usePassive">允许被动模式</param>
        /// <param name="buffLength">buffLength</param>
        public FtpHelper(Log4netUtil.LogAppendToForms logAppendToForms, 
                         string ftpHostIP, string ftpUserName, string ftpPassword, int ftpPort, IWebProxy ftpProxy, bool ftpEnableSsl, bool ftpUseBinary, bool ftpUsePassive, int ftpBuffLength)
        {
            this.logAppendToForms = logAppendToForms;
            this.ftpHostIP = ftpHostIP;
            this.ftpUserName = ftpUserName;
            this.ftpPassword = ftpPassword;
            this.ftpPort = ftpPort;
            this.ftpProxy = ftpProxy;
            this.ftpEnableSsl = ftpEnableSsl;
            this.ftpUseBinary = ftpUseBinary;
            this.ftpUsePassive = ftpUsePassive;
            this.ftpBuffLength = ftpBuffLength;
            if (ftpHostIP.ToLower().StartsWith("ftp://"))
                this.ftpURI = String.Format("{0}:{1}/", ftpHostIP, ftpPort.ToString());
            else
                this.ftpURI = String.Format("ftp://{0}:{1}/", ftpHostIP, ftpPort.ToString());
        }
        #endregion

        #region CheckFtp
        /// <summary>
        /// CheckFtp
        /// </summary>
        /// <param name="DomainName"></param>
        /// <param name="port"></param>
        /// <param name="address"></param>
        /// <param name="FtpUserName"></param>
        /// <param name="FtpUserPwd"></param>
        /// <returns></returns>
        public bool CheckFtp()
        {
            try
            {
                FtpWebRequest ftprequest = (FtpWebRequest)WebRequest.Create(this.ftpURI);
                ftprequest.Credentials = new NetworkCredential(this.ftpUserName, this.ftpPassword);
                ftprequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails; //equestMethods.Ftp.ListDirectory; 
                ftprequest.Timeout = 3000;
                FtpWebResponse ftpResponse = (FtpWebResponse)ftprequest.GetResponse();
                ftpResponse.Close();
                return true;
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Error(String.Format(@"连接失败！ {0}", ex.Message), @"Ftp");
                System.Windows.Forms.MessageBox.Show("连接失败！" + ex.Message, "提示", System.Windows.Forms.MessageBoxButtons.OK);
                return false;
            }
        }
        #endregion

        #region MethodInvoke 异常方法委托
        /// <summary>
        /// 异常方法委托,通过Lamda委托统一处理异常，方便改写
        /// </summary>
        /// <param name="method">当前执行的方法</param>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool MethodInvoke(string method, Action action)
        {
            if (action != null)
            {
                try
                {
                    action();
                    return true;
                }
                catch (Exception ex)
                {
                    Log4netUtil.Log4NetHelper.Error(String.Format(@"FtpHelper({0},{1}).{2}:执行失败:\n {3}", this.ftpURI, this.ftpUserName, method, ex.Message), @"Ftp");
                    throw new Exception(String.Format(@"FtpHelper({0},{1}).{2}:执行失败:\n {3}", this.ftpURI, this.ftpUserName, method, ex));
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region MethodInvoke 异常方法委托
        /// <summary>
        /// 异常方法委托,通过Lamda委托统一处理异常，方便改写
        /// </summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private T MethodInvoke<T>(string method, Func<T> func)
        {
            if (func != null)
            {
                try
                {
                    //FluentConsole.Magenta.Line($@"FtpHelper({ftpHostIP},{username},{password}).{method}:执行成功");
                    return func();
                }
                catch (Exception ex)
                {
                    string logMessage = String.Format(@"FtpHelper({0},{1}).{2}:执行失败:\n {3}", this.ftpURI, this.ftpUserName, method, ex.Message);
                    Log4netUtil.Log4NetHelper.Error(logMessage, @"Ftp");
                    Log4netUtil.LogDisplayHelper.LogError(logAppendToForms, logMessage);
                    Log4netUtil.LogDisplayHelper.LogMessage(logAppendToForms, logMessage);
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }
        #endregion

        #region GetRequest 根据服务器信息FtpWebRequest创建类的对象
        /// <summary>
        /// 根据服务器信息FtpWebRequest创建类的对象
        /// </summary>
        /// <param name="URI"></param>
        /// <returns></returns>
        private FtpWebRequest GetRequest(string URI, string Method)
        {
            FtpWebRequest result = (FtpWebRequest)WebRequest.Create(URI);
            result.Credentials = new NetworkCredential(_FtpUserName, _FtpPassword); //指定登录ftp服务器的用户名和密码
            result.KeepAlive = false; //指定连接是应该关闭还是在请求完成之后关闭，默认为true
            result.UsePassive = this.ftpUsePassive;//指定使用主动模式还是被动模式  默认是 true也就是被动模式，主动模式false
            result.UseBinary = this.ftpUseBinary; //指定文件传输的类型  一种是Binary，另一种是ASCII
            result.EnableSsl = this.ftpEnableSsl;
            result.Proxy = this.ftpProxy;
            result.Method = Method;
            //reqFTP.UsePassive = true;
            //reqFTP.KeepAlive = true;
            //reqFtp.UseBinary = true;
            //reqFtp.KeepAlive = false;//一定要设置此属性，否则一次性下载多个文件的时候，会出现异常。
            return result;
        }
        #endregion

        #region UploadFile 上传文件
        /// <summary> 上传文件</summary>
        /// <param name="localFullPath">需要上传完整路径的文件名</param>
        /// <param name="remoteFileName">要在FTP服务器上面保存文件名</param>
        /// <param name="isMakeDir">isMakeDir</param>
        public bool UploadFile(string localFullPath, string remoteFileName, string remoteDirectoryName = "", bool isMakeDir = false)  //RemoteDirName
        {
            FileInfo fileInfo = new FileInfo(localFullPath);
            if (isMakeDir)
                if (remoteDirectoryName != "")
                    MakeDir(remoteDirectoryName); //检查文件目录，不存在就自动创建
            string uri = Path.Combine(this.ftpURI, remoteDirectoryName, string.Format("{0}{1}", remoteFileName, Path.GetExtension(localFullPath)));
            return MethodInvoke(String.Format(@"uploadFile({0},{1})", localFullPath, remoteDirectoryName), () =>
            {
                FtpWebRequest ftp = GetRequest(uri, WebRequestMethods.Ftp.UploadFile);
                ftp.ContentLength = fileInfo.Length; // 上传文件时通知服务器文件的大小
                int buffLength = this.ftpBuffLength;
                byte[] buff = new byte[buffLength];
                int contentLen;
                using (FileStream fs = fileInfo.OpenRead())
                {
                    try
                    {
                        using (Stream strm = ftp.GetRequestStream())
                        {
                            contentLen = fs.Read(buff, 0, buffLength);
                            while (contentLen != 0)
                            {
                                strm.Write(buff, 0, contentLen);
                                contentLen = fs.Read(buff, 0, buffLength);
                            }
                            strm.Close();
                            strm.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4netUtil.Log4NetHelper.Error(string.Format("Ftp执行UploadFile 上传文件异常！，错误{0}", ex.Message), @"Ftp");
                        throw new IOException(String.Format("Ftp上传文件异常：{0}", ex.Message));
                    }
                    finally
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
            });
        }
        #endregion

        #region UploadAllFile 从一个目录将其内容复制到另一目录
        /// <summary>
        /// 从一个目录将其内容复制到另一目录
        /// </summary>
        /// <param name="localDir">源目录</param>
        /// <param name="directoryName">目标目录</param>
        public void UploadAllFile(string localDir, string directoryName = "")
        {
            string localDirName = string.Empty;
            int targIndex = localDir.LastIndexOf("\\");
            if (targIndex > -1 && targIndex != (localDir.IndexOf(":\\") + 1))
                localDirName = localDir.Substring(0, targIndex);
            localDirName = localDir.Substring(targIndex + 1);
            string newDir = Path.Combine(directoryName, localDirName);
            MethodInvoke(String.Format(@"UploadAllFile({0},{1})", localDir, directoryName), () =>
            {
                MakeDir(newDir);
                DirectoryInfo directoryInfo = new DirectoryInfo(localDir);
                FileInfo[] files = directoryInfo.GetFiles();
                //复制所有文件  
                foreach (FileInfo file in files)
                {
                    UploadFile(file.FullName, newDir);
                }
                //最后复制目录  
                DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
                foreach (DirectoryInfo dir in directoryInfoArray)
                {
                    UploadAllFile(Path.Combine(localDir, dir.Name), newDir);
                }
            });
        }
        #endregion

        #region DelFile 删除单个文件
        /// <summary> 
        /// 删除单个文件
        /// </summary>
        /// <param name="filePath"></param>
        public bool DelFile(string filePath)
        {
            string uri = Path.Combine(ftpURI, filePath);
            return MethodInvoke(String.Format(@"DelFile({0})", filePath), () =>
            {
                FtpWebRequest ftp = GetRequest(uri, WebRequestMethods.Ftp.DeleteFile);
                //ftp.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            });
        }
        #endregion

        #region DelDir 删除最末及空目录
        /// <summary> 
        /// 删除最末及空目录
        /// </summary>
        /// <param name="dirName"></param>
        private bool DelDir(string dirName)
        {
            string uri = Path.Combine(ftpURI, dirName);
            return MethodInvoke(String.Format(@"DelDir({0})", dirName), () =>
            {
                FtpWebRequest ftp = GetRequest(uri, WebRequestMethods.Ftp.RemoveDirectory);
                //ftp.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            });
        }
        #endregion

        #region DelAll  删除目录或者其目录下所有的文件
        /// <summary> 删除目录或者其目录下所有的文件 </summary>
        /// <param name="dirName">目录名称</param>
        /// <param name="ifDelSub">是否删除目录下所有的文件</param>
        public bool DelAll(string DirectoryPath)
        {
            var list = GetAllFtpFile(new List<FileStruct>(), DirectoryPath);
            if (list == null) return DelDir(DirectoryPath);
            if (list.Count == 0) return DelDir(DirectoryPath);//删除当前目录
            var newlist = list.OrderByDescending(x => x.Level);
            /*foreach (var item in newlist)
            {
                FluentConsole.Yellow.Line(String.Format(@"level:{0},isDir:{1},path:{3}", item.Level, item.IsDirectory, item.Path));
            }*/
            string uri = Path.Combine(ftpURI, DirectoryPath);
            return MethodInvoke(String.Format(@"DelAll({0})", DirectoryPath), () =>
            {
                foreach (var item in newlist)
                {
                    if (item.IsDirectory)//判断是目录调用目录的删除方法
                        DelDir(item.Path);
                    else
                        DelFile(item.Path);
                }
                DelDir(DirectoryPath);//删除当前目录
                return true;
            });
        }
        #endregion

        #region DownloadFile 下载单个文件
        /// <summary>
        /// 下载单个文件
        /// </summary>
        /// <param name="ftpFilePath">从ftp要下载的文件路径</param>
        /// <param name="saveDir">下载至本地路径</param>
        /// <param name="filename">文件名</param>
        public bool DownloadFile(string ftpFilePath, string saveDir, string saveFileName)
        {
            string filename = ftpFilePath.Substring(ftpFilePath.LastIndexOf("\\") + 1);
            if (string.IsNullOrEmpty(saveFileName))
                saveFileName = Guid.NewGuid().ToString();
            string uri = Path.Combine(ftpURI, ftpFilePath);
            return MethodInvoke(String.Format(@"DownloadFile({0},{1},{2})", ftpFilePath, saveDir, saveFileName), () =>
            {
                if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
                FtpWebRequest ftp = GetRequest(uri, WebRequestMethods.Ftp.DownloadFile);
                using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (FileStream fs = new FileStream(Path.Combine(saveDir, saveFileName), FileMode.CreateNew))
                        {
                            byte[] buffer = new byte[ftpBuffLength];
                            int read = 0;
                            do
                            {
                                read = responseStream.Read(buffer, 0, buffer.Length);
                                fs.Write(buffer, 0, read);
                            } while (!(read == 0));
                            responseStream.Close();
                            fs.Flush();
                            fs.Close();
                        }
                        responseStream.Close();
                    }
                    response.Close();
                }
            });
        }
        #endregion

        #region DownloadAllFile 从FTP下载整个文件夹  
        /// <summary>    
        /// 从FTP下载整个文件夹    
        /// </summary>    
        /// <param name="DirectoryPath">FTP文件夹路径</param>    
        /// <param name="SaveDir">保存的本地文件夹路径</param>    
        public void DownloadAllFile(string DirectoryPath, string SaveDir)
        {
            MethodInvoke(String.Format(@"DownloadAllFile({0},{1})", DirectoryPath, SaveDir), () =>
            {
                List<FileStruct> files = GetFtpFile(DirectoryPath);
                if (!Directory.Exists(SaveDir))
                    Directory.CreateDirectory(SaveDir);
                foreach (var f in files)
                {
                    if (f.IsDirectory) //文件夹，递归查询  
                        DownloadAllFile(Path.Combine(DirectoryPath, f.Name), Path.Combine(SaveDir, f.Name));
                    else //文件，直接下载  
                        DownloadFile(Path.Combine(DirectoryPath, f.Name), SaveDir, "");
                }
            });
        }
        #endregion

        #region GetFtpFile 获取当前目录下的目录及文件
        /// <summary>
        /// 获取当前目录下的目录及文件
        /// </summary>
        /// param name="ftpfileList"></param>
        /// <param name="DirectoryPath"></param>
        /// <returns></returns>
        public List<FileStruct> GetFtpFile(string DirectoryPath, int ilevel = 0)
        {
            //var ftpfileList = new List<ActFile>();
            List<FileStruct> myListArray = new List<FileStruct>();
            string uri = Path.Combine(ftpURI, DirectoryPath);
            return MethodInvoke(String.Format(@"GetFtpFile({0})", DirectoryPath), () =>
            {
                FtpWebRequest ftp = GetRequest(uri, WebRequestMethods.Ftp.ListDirectoryDetails);
                //ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                string Datastring = string.Empty;
                //StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名
                using (Stream st = ftp.GetResponse().GetResponseStream())
                {
                    using (StreamReader stream = new StreamReader(st, System.Text.Encoding.UTF8)) //
                    {
                        Datastring = stream.ReadToEnd();
                        //stream = null;
                        //st = null;
                        //stream.Close();
                        //stream.Dispose();
                    }
                    //st.Close();
                    //st.Dispose();
                }
                string[] dataRecords = Datastring.Split('\n');
                FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);
                foreach (string s in dataRecords)
                {
                    if (_directoryListStyle != FileListStyle.Unknown && s != "")
                    {
                        FileStruct f = new FileStruct();
                        f.Name = "..";
                        switch (_directoryListStyle)
                        {
                            case FileListStyle.UnixStyle:
                                f = ParseFileStructFromUnixStyleRecord(s, DirectoryPath, ilevel);
                                break;
                            case FileListStyle.WindowsStyle:
                                f = ParseFileStructFromWindowsStyleRecord(s, DirectoryPath, ilevel);
                                break;
                        }
                        if (!(f.Name == "." || f.Name == ".."))
                        {
                            myListArray.Add(f);
                        }
                    }
                }
                return myListArray;
            });


        }
        #endregion

        #region ListFilesAndDirectories 列出FTP服务器上面当前目录的所有文件和目录
        /// <summary>
        /// 列出FTP服务器上面当前目录的所有文件和目录
        /// </summary>
        public FileStruct[] ListFilesAndDirectories(string DirectoryPath)
        {
            string uri = Path.Combine(ftpURI, DirectoryPath);
            //string uri = ftpURI.TrimEnd('/') + DirectoryPath;
            return MethodInvoke(String.Format(@"ListFilesAndDirectories({0})", DirectoryPath), () =>
            {
                FtpWebRequest ftp = GetRequest(uri, WebRequestMethods.Ftp.ListDirectoryDetails);
                //ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                string Datastring = string.Empty;
                try
                {
                    Stream st = ftp.GetResponse().GetResponseStream();
                    using (StreamReader stream = new StreamReader(st, System.Text.Encoding.Default))
                    {
                        Datastring = stream.ReadToEnd();
                    }
                    st.Close();
                    st.Dispose();
                    FileStruct[] list = GetList(Datastring, DirectoryPath);
                    return list;
                }
                catch (Exception ep)
                {
                    Log4netUtil.Log4NetHelper.Error(string.Format("Ftp执行ListFilesAndDirectories列出FTP服务器上面当前目录的所有文件和目录 失败！，错误{0}", ep.Message), @"Ftp");
                    throw new IOException(string.Format("Ftp执行ListFilesAndDirectories列出FTP服务器上面当前目录的所有文件和目录 失败！，错误{0}", ep.Message));

                }
            });
        }
        #endregion

        #region GetAllFtpFile  获取FTP目录下的所有目录及文件包括其子目录和子文件
        /// <summary>
        /// 获取FTP目录下的所有目录及文件包括其子目录和子文件
        /// </summary>
        /// param name="result"></param>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public List<FileStruct> GetAllFtpFile(List<FileStruct> result, string DirectoryPath, int level = 0)
        {
            var ftpfileList = new List<FileStruct>();
            string uri = Path.Combine(ftpURI, DirectoryPath);
            return MethodInvoke(String.Format(@"GetAllFtpFile({0})", DirectoryPath), () =>
            {
                ftpfileList = GetFtpFile(DirectoryPath, level);
                result.AddRange(ftpfileList);
                var newlist = ftpfileList.Where(x => x.IsDirectory).ToList();
                foreach (var item in newlist)
                {
                    GetAllFtpFile(result, item.Name, level + 1);
                }
                return result;
            });

        }
        #endregion

        #region DirectoryExist 判断当前目录下指定的子目录是否存在
        /// <summary>
        /// 判断当前目录下指定的子目录是否存在
        /// </summary>
        /// <param name="RemoteDirectoryName">指定的目录名</param>
        public bool DirectoryExist(string directoryPath, string remoteDirectoryName)
        {
            return MethodInvoke(String.Format(@"DirectoryExist({0}{1})", directoryPath, remoteDirectoryName), () =>
            {
                try
                {
                    if (!Util.FileHelper.IsValidPathChars(remoteDirectoryName))
                    {
                        throw new Exception("目录名非法！");
                    }
                    FileStruct[] listDir = ListDirectories(directoryPath);
                    foreach (FileStruct dir in listDir)
                    {
                        if (dir.Name == remoteDirectoryName)
                            return true;
                    }
                    return false;
                }
                catch (Exception ep)
                {
                    Log4netUtil.Log4NetHelper.Error(string.Format("Ftp执行DirectoryExist判断当前目录下指定的子目录是否存在 失败！，错误{0}", ep.Message), @"Ftp");
                    throw new IOException(string.Format("Ftp执行DirectoryExist判断当前目录下指定的子目录是否存在 失败！，错误{0}", ep.Message));
                }
            });
        }
        #endregion

        #region FileExist 判断一个远程文件是否存在服务器当前目录下面
        /// <summary>
        /// 判断一个远程文件是否存在服务器当前目录下面
        /// </summary>
        /// <param name="RemoteFileName">远程文件名</param>
        public bool FileExist(string directoryPath, string RemoteFileName)
        {
            return MethodInvoke(String.Format(@"DirectoryExist({0}{1})", directoryPath, RemoteFileName), () =>
            {
                try
                {
                    if (!Util.FileHelper.IsValidFileChars(RemoteFileName))
                        throw new Exception("文件名非法！");
                    FileStruct[] listFile = ListFiles(directoryPath);
                    foreach (FileStruct file in listFile)
                    {
                        if (file.Name == RemoteFileName)
                            return true;
                    }
                    return false;
                }
                catch (Exception ep)
                {
                    throw ep;
                }
            });
        }
        #endregion

        #region MakeDir 在ftp服务器上创建指定目录,父目录不存在则创建
        ///  </summary>
        /// 在ftp服务器上创建指定目录,父目录不存在则创建
        /// </summary>
        /// <param name="dirName">创建的目录名称</param>
        public bool MakeDir(string remoteDirectoryName)
        {
            var dirs = remoteDirectoryName.Split('\\').ToList();//针对多级目录分割
            string currentDir = string.Empty;
            return MethodInvoke(String.Format("MakeDir(\"{0}\")", remoteDirectoryName), () =>
            {
                foreach (var dir in dirs)
                {
                    if (!DirectoryExist(currentDir, dir))
                    {
                        currentDir = Path.Combine(currentDir, dir);
                        string uri = Path.Combine(ftpURI, currentDir);
                        FtpWebRequest ftp = GetRequest(uri, WebRequestMethods.Ftp.MakeDirectory);
                        FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                        response.Close();
                        response = null;
                    }
                    else
                        currentDir = Path.Combine(currentDir, dir);
                }

            });

        }
        #endregion

        #region Rename 文件重命名
        /// <summary>文件重命名 </summary>
        /// <param name="currentFilename">当前名称</param>
        /// <param name="newFilename">重命名名称</param>
        /// <param name="currentFilename">所在的目录</param>
        public bool Rename(string currentFilename, string newFilename, string dirName = "")
        {
            string uri = Path.Combine(_FtpURI, dirName, currentFilename);
            return MethodInvoke(String.Format(@"Rename({0},{1},{2})", currentFilename, newFilename, dirName), () =>
            {
                FtpWebRequest ftp = GetRequest(uri, WebRequestMethods.Ftp.Rename);
                //ftp.Method = WebRequestMethods.Ftp.Rename;
                ftp.RenameTo = newFilename;
                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            });
        }
        #endregion

        #region ListFiles 列出FTP服务器上面当前目录的所有文件
        /// <summary>
        /// 列出FTP服务器上面当前目录的所有文件
        /// </summary>
        private FileStruct[] ListFiles(string dirName)
        {
            FileStruct[] listAll = ListFilesAndDirectories(dirName);
            List<FileStruct> listFile = new List<FileStruct>();
            foreach (FileStruct file in listAll)
            {
                if (!file.IsDirectory)
                    listFile.Add(file);
            }
            return listFile.ToArray();
        }
        #endregion

        #region ListDirectories 列出FTP服务器上面当前目录的所有的目录
        /// <summary>
        /// 列出FTP服务器上面当前目录的所有的目录
        /// </summary>
        private FileStruct[] ListDirectories(string dirName)
        {
            FileStruct[] listAll = ListFilesAndDirectories(dirName);
            List<FileStruct> listDirectory = new List<FileStruct>();
            foreach (FileStruct file in listAll)
            {
                if (file.IsDirectory)
                    listDirectory.Add(file);
            }
            return listDirectory.ToArray();
        }
        #endregion

        #region GetList 获得文件和目录列表
        /// <summary>
        /// 获得文件和目录列表
        /// </summary>
        /// <param name="datastring">FTP返回的列表字符信息</param>
        private FileStruct[] GetList(string datastring, string DirectoryPath, int iLevel = 0)
        {
            List<FileStruct> myListArray = new List<FileStruct>();
            string[] dataRecords = datastring.Split('\n');
            FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);
            foreach (string s in dataRecords)
            {
                if (_directoryListStyle != FileListStyle.Unknown && s != "")
                {
                    FileStruct f = new FileStruct();
                    f.Name = "..";
                    switch (_directoryListStyle)
                    {
                        case FileListStyle.UnixStyle:
                            f = ParseFileStructFromUnixStyleRecord(s, DirectoryPath);
                            break;
                        case FileListStyle.WindowsStyle:
                            f = ParseFileStructFromWindowsStyleRecord(s, DirectoryPath);
                            break;
                    }
                    if (!(f.Name == "." || f.Name == ".."))
                        myListArray.Add(f);
                }
            }
            return myListArray.ToArray();
        }
        #endregion

        #region GuessFileListStyle 判断文件列表的方式Window方式还是Unix方式
        /// <summary>
        /// 判断文件列表的方式Window方式还是Unix方式
        /// </summary>
        /// <param name="recordList">文件信息列表</param>
        private FileListStyle GuessFileListStyle(string[] recordList)
        {
            foreach (string s in recordList)
            {
                if (s.Length > 10
                 && Regex.IsMatch(s.Substring(0, 10), "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)"))
                    return FileListStyle.UnixStyle;
                else if (s.Length > 8
                 && Regex.IsMatch(s.Substring(0, 8), "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                    return FileListStyle.WindowsStyle;
            }
            return FileListStyle.Unknown;
        }
        #endregion

        #region ParseFileStructFromWindowsStyleRecord 从Windows格式中返回文件信息
        /// <summary>
        /// 从Windows格式中返回文件信息
        /// </summary>
        /// <param name="Record">文件信息</param>
        private FileStruct ParseFileStructFromWindowsStyleRecord(string Record, string DirectoryPath, int iLevel = 0)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            string dateStr = processstr.Substring(0, 8);
            processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            string timeStr = processstr.Substring(0, 7);
            processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
            myDTFI.ShortTimePattern = "t";
            f.CreateTime = DateTime.Parse(dateStr + " " + timeStr, myDTFI);
            if (processstr.Substring(0, 5) == "<DIR>")
            {
                f.IsDirectory = true;
                processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            }
            else
            {
                string[] strs = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);   // true);
                processstr = strs[1];
                f.IsDirectory = false;
            }
            f.Name = processstr;
            f.Path = Path.Combine(DirectoryPath, f.Name.Trim());
            f.Level = iLevel;
            return f;
        }
        #endregion

        #region ParseFileStructFromUnixStyleRecord 从Unix格式中返回文件信息
        /// <summary>
        /// 从Unix格式中返回文件信息
        /// </summary>
        /// <param name="Record">文件信息</param>
        private FileStruct ParseFileStructFromUnixStyleRecord(string Record, string DirectoryPath, int iLevel = 0)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            try
            {
                f.Flags = processstr.Substring(0, 10);
                f.IsDirectory = (f.Flags[0] == 'd');
                processstr = (processstr.Substring(11)).Trim();
                _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //跳过一部分
                f.Owner = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
                f.Group = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
                _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //跳过一部分
                string yearOrTime = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
                if (yearOrTime.IndexOf(":") >= 0)  //time
                    processstr = processstr.Replace(yearOrTime, DateTime.Now.Year.ToString());
                f.CreateTime = DateTime.Parse(_cutSubstringFromStringWithTrim(ref processstr, ' ', 8));
                f.Name = processstr;   //最后就是名称
                f.Path = Path.Combine(DirectoryPath, f.Name.Trim());
                f.Level = iLevel;
                return f;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                f.Name = Record;
                return f;
            }
        }
        #endregion

        #region _cutSubstringFromStringWithTrim 按照一定的规则进行字符串截取
        /// <summary>
        /// 按照一定的规则进行字符串截取
        /// </summary>
        /// <param name="s">截取的字符串</param>
        /// <param name="c">查找的字符</param>
        /// <param name="startIndex">查找的位置</param>
        private string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
        {
            int pos1 = s.IndexOf(c, startIndex);
            string retString = s.Substring(0, pos1);
            s = (s.Substring(pos1)).Trim();
            return retString;
        }
        #endregion

    }

    public struct FileStruct
    {
        public string Flags;
        public string Owner;
        public string Group;
        public bool IsDirectory;
        public DateTime CreateTime;
        public string Name;
        public string Path;
        public int Level;
    }

    public enum FileListStyle
    {
        UnixStyle,
        WindowsStyle,
        Unknown
    }


}
