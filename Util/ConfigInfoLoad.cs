using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public class ConfigInfoLoad
    {
        #region  getConfigInfo
        public static Model.ConfigInfo GetConfigInfo()
        {
            return getConfigInfo();
        }

        private static Model.ConfigInfo getConfigInfo()
        {
            Model.ConfigInfo configInfo = new Model.ConfigInfo();
            string ConfigFile = System.Windows.Forms.Application.StartupPath.ToString() + "\\Config.ini";
            configInfo.ModuleCode = "C13";

           

            configInfo.WebApiUrl = Util.INIOperationClass.INIGetStringValue(ConfigFile, "WebApi", "WebApiUrl", null);
            configInfo.SignKey = Util.INIOperationClass.INIGetStringValue(ConfigFile, "WebApi", "SignKey", null);
            configInfo.EncryptKey = Util.INIOperationClass.INIGetStringValue(ConfigFile, "WebApi", "EncryptKey", null);


            configInfo.FtpHostIP = Util.INIOperationClass.INIGetStringValue(ConfigFile, "FTP", "IP", null);
            string ftpPort = Util.INIOperationClass.INIGetStringValue(ConfigFile, "FTP", "PORT", null);
            configInfo.FtpPort = Util.Common.IsInt(ftpPort) ? Convert.ToInt32(ftpPort) : 21;

            configInfo.FtpUserName = Util.INIOperationClass.INIGetStringValue(ConfigFile, "FTP", "USER", null);
            string ftpPasswordEncrypt = Util.INIOperationClass.INIGetStringValue(ConfigFile, "FTP", "PWD", null);
            configInfo.FtpPassword = Util.EncAndDec.DESDecrypt(ftpPasswordEncrypt);  //解密

            configInfo.FtpRootPath = Util.INIOperationClass.INIGetStringValue(ConfigFile, "FTP", "RootPath", null);
            string ftpBuffLength = Util.INIOperationClass.INIGetStringValue(ConfigFile, "FTP", "BuffLength", null);
            configInfo.FtpBuffLength = string.IsNullOrEmpty(ftpBuffLength) ? 2048 : Convert.ToInt32(ftpBuffLength);

            string FtpUsePassiveStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, "FTP", "UsePassive", null);
            FtpUsePassiveStr = string.IsNullOrEmpty(FtpUsePassiveStr) ? "否" : FtpUsePassiveStr;
            configInfo.FtpUsePassive = string.Equals(FtpUsePassiveStr, "是") ? true : false;

          
            configInfo.LastLoginName = Util.INIOperationClass.INIGetStringValue(ConfigFile, "Login", "LastLoginName", null);

            string b2bPlatformNameStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, "EnterpriseConfig", "B2bPlatformName", null);
            configInfo.B2bPlatformName = string.IsNullOrEmpty(b2bPlatformNameStr) ? "药药购 " : b2bPlatformNameStr;
            configInfo.EnterpriseName = Util.INIOperationClass.INIGetStringValue(ConfigFile, "EnterpriseConfig", "EnterpriseName", null);
            configInfo.EnterpriseLicenseNo = Util.INIOperationClass.INIGetStringValue(ConfigFile, "EnterpriseConfig", "EnterpriseLicenseNo", null);
            configInfo.EnterpriseId = Util.INIOperationClass.INIGetStringValue(ConfigFile, "EnterpriseConfig", "EnterpriseId", null);
            configInfo.LoginName = Util.INIOperationClass.INIGetStringValue(ConfigFile, "EnterpriseConfig", "LoginName", null);
            configInfo.Password = Util.INIOperationClass.INIGetStringValue(ConfigFile, "EnterpriseConfig", "Password", null);
           

   
            configInfo.LogRetentionDays = Util.Common.IsInt(Util.INIOperationClass.INIGetStringValue(ConfigFile, "Login", "LogRetentionDays", null)) ? Convert.ToInt32(Util.INIOperationClass.INIGetStringValue(ConfigFile, "Login", "LogRetentionDays", null)) : 15;

            string autoAdvanceDaysStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, "Login", "AutoAdvanceDays", null);
            autoAdvanceDaysStr = string.IsNullOrEmpty(autoAdvanceDaysStr) ? "3" : autoAdvanceDaysStr;
            configInfo.AutoAdvanceDays = Util.Common.IsInt(autoAdvanceDaysStr) ? Convert.ToInt32(autoAdvanceDaysStr) : 3;

            string orderCodePrefix = Util.INIOperationClass.INIGetStringValue(ConfigFile, "Login", "OrderCodePrefix", null);
            configInfo.OrderCodePrefix = string.IsNullOrEmpty(orderCodePrefix) ? "FYYK" : orderCodePrefix; //订单前缀
           
            string serviceAndSupport = Util.INIOperationClass.INIGetStringValue(ConfigFile, "Login", "ServiceAndSupport", null);
            configInfo.ServiceAndSupport = string.IsNullOrEmpty(serviceAndSupport) ? "武汉飞宇益克科技有限公司" : serviceAndSupport;

            string isShowServiceAndSupportStr = string.IsNullOrEmpty(Util.INIOperationClass.INIGetStringValue(ConfigFile, "Login", "IsShowServiceAndSupport", null)) ? "Y" : Util.INIOperationClass.INIGetStringValue(ConfigFile, "Login", "IsShowServiceAndSupport", null);
            configInfo.IsShowServiceAndSupport = string.Equals(isShowServiceAndSupportStr, "N") ? false:true ;

            string xmlFileMaxSizeStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, "Login", "XmlFileMaxSize", null);
            xmlFileMaxSizeStr = string.IsNullOrEmpty(xmlFileMaxSizeStr) ? "2.0" : xmlFileMaxSizeStr;
            configInfo.XmlFileMaxSize = Util.Common.IsDouble(xmlFileMaxSizeStr) ? Convert.ToDouble(xmlFileMaxSizeStr):2.0;

            configInfo.JobEnable= Util.INIOperationClass.INIGetStringValue(ConfigFile, "JobConfig", "JobEnable", null);
            List<Model.JobEntity> JobEntityList = new List<Model.JobEntity>();
            if (!String.IsNullOrEmpty(configInfo.JobEnable))
            {
                string[] array = configInfo.JobEnable.Split(',');
                for (int i = 0; i < array.Count(); i++)
                {
                    string jobCode = array[i].ToString().Trim();
                    if (string.IsNullOrEmpty(jobCode))
                        continue;

                    Model.JobEntity jobEntity = new Model.JobEntity();
                    jobEntity.JobCode = jobCode;
                    jobEntity.JobName = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "JobName", null);
                    jobEntity.CronExpression = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "CronExpression", null);
                    jobEntity.CronExpressionDescription = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "CronExpressionDescription", null);

                    string isDebugJob = string.IsNullOrEmpty(Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "IsDebug", null)) ? "否" : Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "IsDebug", null);
                    jobEntity.IsDebug = string.Equals(isDebugJob, "是") ? true : false;

                    string domainNameStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "DomainName", null);
                    jobEntity.DomainName = string.IsNullOrEmpty(domainNameStr) ? "tsm.fyyk.com" : domainNameStr;  //Api 域名

                    string serviceNameStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "ServiceName", null);
                    jobEntity.ServiceName = string.IsNullOrEmpty(serviceNameStr) ? "fyyk" : serviceNameStr;//Api 服务器

                    string interfacePrefixStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "InterfacePrefix", null);
                    jobEntity.InterfacePrefix = string.IsNullOrEmpty(interfacePrefixStr) ? "dev-api" : interfacePrefixStr;//Api 接口前缀

                    string moduleTypeStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "ModuleType", null);
                    jobEntity.ApiModuleType = string.IsNullOrEmpty(moduleTypeStr) ? "dataApi" : moduleTypeStr;//Api 模块

                    string requestTypeStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "RequestType", null);
                    jobEntity.ApiRequestType = string.IsNullOrEmpty(requestTypeStr) ? string.Empty : requestTypeStr;//Api 请求名称


                    string targetDatabaseStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "TargetDatabase", null);
                    jobEntity.TargetDatabase = string.IsNullOrEmpty(targetDatabaseStr) ? "Erp" : targetDatabaseStr;

                    string procedureNameStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "ProcedureName", null);
                    jobEntity.ProcedureName = string.IsNullOrEmpty(procedureNameStr) ? "Fyyk_B2b_SqlView" : procedureNameStr;

                    string moduleIDStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "ModuleID", null);
                    jobEntity.ModuleID = string.IsNullOrEmpty(moduleIDStr) ? string.Empty : moduleIDStr;

                    string filterBillTypeStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "FilterBillType", null);
                    filterBillTypeStr = string.IsNullOrEmpty(filterBillTypeStr) ? "1" : filterBillTypeStr;
                    jobEntity.FilterBillType = Util.Common.IsInt(filterBillTypeStr) ? Convert.ToInt32(filterBillTypeStr) : 1;

                    string writebackProcedureNameStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "WritebackProcedureName", null);
                    jobEntity.WritebackProcedureName = string.IsNullOrEmpty(writebackProcedureNameStr) ? "Fyyk_B2b_Writeback" : writebackProcedureNameStr;

                    string writebackTypeStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "WritebackType", null);
                    jobEntity.WritebackType = string.IsNullOrEmpty(writebackTypeStr) ? string.Empty : writebackTypeStr;

                    string insertTableNameStr = Util.INIOperationClass.INIGetStringValue(ConfigFile, jobCode, "InsertTableName", null);
                    jobEntity.InsertTableName = string.IsNullOrEmpty(insertTableNameStr) ? string.Empty : insertTableNameStr;
                    
                    JobEntityList.Add(jobEntity);
                }
            }
            configInfo.JobEntityList = JobEntityList;

            return configInfo;
        }
        #endregion
    }
}
