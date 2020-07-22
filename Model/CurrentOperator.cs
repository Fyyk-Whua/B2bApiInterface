using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 操作员实体类
    /// </summary>
    public class CurrentOperator  //: ModelBase
    {
        public int OperatorId { get; set; }   //权限关系 ID 
        public string OperatorName { get; set; }   //权限关系 ID 
        public string OperatorPassword { get; set; }
    }
}
