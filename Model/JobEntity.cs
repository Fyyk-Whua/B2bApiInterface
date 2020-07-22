using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class JobEntity
    {
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public string DomainName { get; set; }  //Api 域名
        public string ServiceName { get; set; } //Api 服务器
        public string InterfacePrefix { get; set; } //Api 接口前缀
        public string ApiModuleType { get; set; } //Api 模块
        public string ApiRequestType { get; set; } //Api 请求名称

        public string TargetDatabase { get; set; } = "Erp";
        public string ProcedureName { get; set; } = "Fyyk_B2b_SqlView";  //ProcedureName
        public string ModuleID { get; set; }
        public int FilterBillType { get; set; }
        public string WritebackProcedureName { get; set; } = "Fyyk_B2b_Writeback";  //ProcedureName
        public string WritebackType { get; set; }
        public string InsertTableName { get; set; }  
        
        public string CronExpression { get; set; } //TimeSpanType
        public string CronExpressionDescription { get; set; } //CronExpression  //CronExpressionDescription TimeSpan
        public bool IsDebug { get; set; }
        public string EnterpriseId { get; set; }
        public string EnterpriseName { get; set; }
        //public int FilterIsExportXml { get; set; }
        public string StrConfigInfo { get; set; }
        public Model.ConfigInfo ConfigInfo { get; set; }
    }
}
