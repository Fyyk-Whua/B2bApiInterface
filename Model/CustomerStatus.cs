using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CustomerStatus
    {
        /// <summary>
        /// auditStatus 认证状态  2认证失败   4已上传证件
        /// </summary>
        public int auditStatus { get; set; }
        /// <summary>
        /// 客户类型id
        /// </summary>
        public string customerId { get; set; }
        /// <summary>
        /// disableReason
        /// </summary>
        public string disableReason { get; set; }
        

    }
}
