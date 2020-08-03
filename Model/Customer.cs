using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    public class Customer
    {
        /// <summary>
        /// 
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apparatusLicenseCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apparatusLicenseValidate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bankAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string businessLicenseCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string businessLicenseValidate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientIdcard { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientPhone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientTimelimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string companyAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string companyPrincipal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string consignee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string consigneePhone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string contactWay { get; set; }
        /// <summary>
        /// 客户Id  Add 20200722
        /// </summary>
        public string customerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerTypeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string depositBank { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string depotAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string drugInterceptScope { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dutyNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string erpCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string erpId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string erpSalesmanId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string foodLicenseCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string foodLicenseValidate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gspCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gspValidate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string instrumentInterceptScope { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isForProfit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isRefrigeration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string legalPerson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string licenceCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string licenceStartDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int licenceType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string licenceValidate { get; set; }
        /// <summary>
        /// 付款方式： 1线上，2线下，3货到付款  可多选,分隔
        /// </summary>
        public string payType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string qualityPrincipal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string registeredCapital { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 报价方式 1 批发价，2连锁价，3 药店价 ，4 医疗机构价
        /// </summary>
        public string specifyQuotation { get; set; }
        /// <summary>
        /// 0正常 ,9禁用
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string twoApparatusLicenseCode { get; set; }
    }
}
