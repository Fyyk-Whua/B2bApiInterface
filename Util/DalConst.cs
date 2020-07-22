using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Util
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: DalConst.cs   
    ///* 命名空间: Util.FrameUtil
    ///* 功    能: 保存数据库连接字符串以及反射所需的项目名称
    ///* 内    容: 
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 版权说明:  Copyright 2016-2017 武汉飞宇益克科技有限公司
    ///*
    ///**************************************************************************/
    public class DalConst
    {
      
        #region 配制文件路径
        /// <summary>
        /// 配制文件路径
        /// </summary>
        public static string _ConfigFile = System.IO.Directory.GetCurrentDirectory() + "\\Config.ini";
        #endregion

        #region 数据库类型
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static readonly string ErpDBType = INIOperationClass.INIGetStringValue(_ConfigFile, "DBService", "ErpDBServiceType", null);
        public static readonly string WmsDBType = INIOperationClass.INIGetStringValue(_ConfigFile, "DBService", "WmsDBServiceType", null);
        #endregion

        #region 反射DAL数据访问层项目名称
        /// <summary>
        /// 反射DAL数据访问层项目名称
        /// </summary>
        public static readonly string AssemblyPathDAL = INIOperationClass.INIGetStringValue(_ConfigFile, "DBService", "AssemblyDAL", null);
        public static readonly string NamespacePathDAL = INIOperationClass.INIGetStringValue(_ConfigFile, "DBService", "NamespaceDAL", null);
        #endregion

        #region 反射BLL层项目名称
        /// <summary>
        /// 反射BLL层项目名称
        /// </summary>
        public static readonly string AssemblyPathBLL = INIOperationClass.INIGetStringValue(_ConfigFile, "DBService", "AssemblyBLL", null);
        public static readonly string NamespacePathBLL = INIOperationClass.INIGetStringValue(_ConfigFile, "DBService", "NamespaceBLL", null);
        #endregion
        
        #region 数据库连接字符串

        #region getSqlServerConnection 得到SqlServer连接字符串
        /// <summary>
        /// 得到SqlServer连接字符串
        /// </summary>
        /// <returns>连接字符串</returns>
        public static string getSqlServerConnection(int timeout, string connectionType)
        {
            string ConfigFile = System.IO.Directory.GetCurrentDirectory() + "\\Config.ini";
            string connectionSql = "";
            string strServer = INIOperationClass.INIGetStringValue(ConfigFile, string.Format("{0}{1}", connectionType, "SqlServer"), "IP", null);    //服务器
            string strDataBase = INIOperationClass.INIGetStringValue(ConfigFile, string.Format("{0}{1}", connectionType, "SqlServer"), "DBNAME", null);  //数据库名称
            string strPORT = INIOperationClass.INIGetStringValue(ConfigFile, string.Format("{0}{1}", connectionType, "SqlServer"), "PORT", null);   //密码
            string strUid = INIOperationClass.INIGetStringValue(ConfigFile, string.Format("{0}{1}", connectionType, "SqlServer"), "USER", null);   //用户
            string strPwdEncrypt = INIOperationClass.INIGetStringValue(ConfigFile, string.Format("{0}{1}", connectionType, "SqlServer"), "PWD", null);   //密码
            string strPwd = EncAndDec.DESDecrypt(strPwdEncrypt);  //解密
            if (timeout == 0)
                connectionSql = "Server=" + strServer + "," + strPORT + ";DataBase=" + strDataBase + ";User ID=" + strUid + ";Password=" + strPwd + ";Persist Security Info=True;MultipleActiveResultSets=true;Application Name=Wms接口管理器;  ";
            else
                connectionSql = "Server=" + strServer + "," + strPORT + ";DataBase=" + strDataBase + ";User ID=" + strUid + ";Password=" + strPwd + ";Persist Security Info=True;MultipleActiveResultSets=true;Application Name=Wms接口管理器;Connection Timeout=" + timeout + ";  ";  //Connection Timeout=30
            return connectionSql;
        }
        #endregion

        #region getOracleConnection 得到Oracle连接字符串
        /// <summary>
        /// 得到Oracle连接字符串
        /// </summary>
        /// <returns>连接字符串</returns>
        /// 
        public static string getOracleConnection(int timeout, string connectionType)
        {
            string configFile = string.Format("{0}\\{1}",System.IO.Directory.GetCurrentDirectory(), "Config.ini");
            string connectionStrings = string.Empty;
            string userId = INIOperationClass.INIGetStringValue(configFile, string.Format("{0}{1}", connectionType, "Oracle"), "USER", null);   //用户
            string passwordEncrypt = INIOperationClass.INIGetStringValue(configFile, string.Format("{0}{1}", connectionType, "Oracle"), "PWD", null);  //密码
            string pwd = EncAndDec.DESDecrypt(passwordEncrypt);  //解密
            string host = INIOperationClass.INIGetStringValue(configFile, string.Format("{0}{1}", connectionType, "Oracle"), "IP", null); //服务器IP
            string port = INIOperationClass.INIGetStringValue(configFile, string.Format("{0}{1}", connectionType, "Oracle"), "PORT", null); //端口
            string serviceName  = INIOperationClass.INIGetStringValue(configFile, string.Format("{0}{1}", connectionType, "Oracle"), "DBNAME", null);
            if (timeout == 0)
                /*ConnectionStrings = "User Id=" + strUserId.Trim() + ";Password=" + strPwd.Trim()
                               + ";Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + strHOST.Trim()
                               + ")(PORT=" + strPORT.Trim() + ")))(CONNECT_DATA=(SERVICE_NAME=" + strSERVICE_NAME.Trim() + ")))";*/
                connectionStrings = string.Format("User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVICE_NAME={4})(SERVER = DEDICATED))) ; ",
                         userId, pwd, host, port, serviceName);
            else
                connectionStrings = string.Format("User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVICE_NAME={4})(SERVER = DEDICATED))) ;Connection Timeout={5};",
                         userId, pwd, host, port, serviceName, timeout);
                /*ConnectionStrings = "User Id=" + strUserId.Trim() + ";Password=" + strPwd.Trim()
                            + ";Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + strHOST.Trim()
                            + ")(PORT=" + strPORT.Trim() + ")))(CONNECT_DATA=(SERVICE_NAME=" + strSERVICE_NAME.Trim() + "))) ;Connection Timeout=" + timeout + "; "; //;Connection Timeout=" + timeout + ";*/
            return connectionStrings;
        }
        #endregion

        #region getMySqlConnection 得到MySql连接字符串
        /// <summary>
        /// 得到MySql连接字符串
        /// </summary>
        /// <returns>连接字符串</returns>
        public static string getMySqlConnection(int timeout, string connectionType)
        {
            string ConfigFile = System.IO.Directory.GetCurrentDirectory() + "\\Config.ini";
            string connectionSql = "";
            string strServer = INIOperationClass.INIGetStringValue(ConfigFile, string.Format("{0}{1}", connectionType, "MySql"), "IP", null);    //服务器
            string strDataBase = INIOperationClass.INIGetStringValue(ConfigFile, string.Format("{0}{1}", connectionType, "MySql"), "DBNAME", null);  //数据库名称
            string strPORT = INIOperationClass.INIGetStringValue(ConfigFile, string.Format("{0}{1}", connectionType, "MySql"), "PORT", null);
            string strUid = INIOperationClass.INIGetStringValue(ConfigFile, string.Format("{0}{1}", connectionType, "MySql"), "USER", null);   //用户
            string strPwdEncrypt = INIOperationClass.INIGetStringValue(ConfigFile, string.Format("{0}{1}", connectionType, "MySql"), "PWD", null);   //密码
            string strPwd = EncAndDec.DESDecrypt(strPwdEncrypt);  //解密
            if (timeout == 0)
                connectionSql = "Server=" + strServer + ";DataBase=" + strDataBase + ";User ID=" + strUid + ";Password=" + strPwd + ";port=" + strPORT + ";CharSet=utf8;pooling=true;";  //ConnectionTimeout=30
            else
                connectionSql = "Server=" + strServer + ";DataBase=" + strDataBase + ";User ID=" + strUid + ";Password=" + strPwd + ";port=" + strPORT + ";CharSet=utf8;pooling=true;Connection Timeout=" + timeout + "; ";  //ConnectionTimeout=30
            return connectionSql;
        }
        #endregion

        #region StrReplaceText 
        /// <summary>
        /// StrReplaceText MySql ？ ; SqlServer @ ;  Oracle : 
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string StrReplaceText(string strText, string dBType)
        {
            string retText =string.Empty;
            string dataBaseType = dBType;

            switch (dBType)
            {
                case "MySql":
                    retText = strText.Replace("?", ":");
                    break;
                case "SqlServer":
                    retText = strText.Replace("@", ":");
                    break;
                case "Oracle":
                    retText = strText.Replace(":", ":");
                    break;
                default:
                    retText = strText.Replace(":", ":");
                    break;
            }
            return retText;
        }
        #endregion

        #endregion


    }
}
