using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ConfigInfo
    {
        
        public string B2bPlatformName { get; set; } = "药药购";
        public string WebApiUrl { get; set; } = string.Empty;//"http://prepurch.ddky.com/skuDetailApi";
        public string SignKey { get; set; }
        public string EncryptKey { get; set; }

        public string LineMessage { get; set; } = string.Empty;
        public int LogRetentionDays { get; set; }
        public int AutoAdvanceDays { get; set; }
        public string ModuleCode { get; set; } = "F21";
        public string LastLoginName { get; set; } = string.Empty;

        public string EnterpriseName { get; set; }
        public string EnterpriseLicenseNo { get; set; }
        public string EnterpriseId { get; set; }

        public string LoginName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string XmlExportPath { get; set; } = string.Empty;
        public bool IsShowServiceAndSupport{ get; set; } =true;
        public string ServiceAndSupport { get; set; } = string.Empty;
        public double XmlFileMaxSize { get; set; } = 2.0;
        public string JobEnable { get; set; }

        public List<JobEntity> JobEntityList { get; set; }
    }
}
