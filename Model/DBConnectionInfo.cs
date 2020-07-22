using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DBConnectionInfo
    {
        public string ConfigFile { get; set; }
        public string EnterpriseName { get; set; }
        public string EnterpriseLicenseNo { get; set; }
        public string EnterpriseId { get; set; }
        public string WebServiceUrl { get; set; }

        public string FtpHostIP { get; set; }
        public string FtpPort { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPasswordEn { get; set; }
        public string FtpPassword { get; set; }
        public string FtpRootPath { get; set; }
        public string FtpBuffLength { get; set; }

      
        public string ErpDBServiceType { get; set; }
        public string ErpDBServer { get; set; }
        public string ErpDBPort { get; set; }
        public string ErpDBDataBase { get; set; }
        public string ErpDBUid { get; set; }
        public string ErpDBPwdEn { get; set; }
        public string ErpDBPwd { get; set; }

        public string WmsDBServiceType { get; set; }
        public string WmsDBServer { get; set; }
        public string WmsDBPort { get; set; }
        public string WmsDBDataBase { get; set; }
        public string WmsDBUid { get; set; }
        public string WmsDBPwdEn { get; set; }
        public string WmsDBPwd { get; set; }

        
    }
}
