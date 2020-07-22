using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class InventoryQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public string Partno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OwnerCode { get; set; }
    }

}
