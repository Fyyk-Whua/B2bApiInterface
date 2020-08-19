using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    public class Salesman
    {
        /// <summary>
        /// 
        /// </summary>
        public string erpId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string salesmanDept { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string salesmanName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string salesmanPhone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string salesmanPost { get; set; }
        /// <summary>
        /// 状态 0正常 9禁用
        /// </summary>
        public int status { get; set; }
        public int taskId { get; set; }
    }

}
