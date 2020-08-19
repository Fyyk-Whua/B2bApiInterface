using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    
    public class CommodityPrice
    {
        /// <summary>
        /// Erp商品Id
        /// </summary>
        public string erpGoodsId { get; set; }
        /// <summary>
        /// 最高限量
        /// </summary>
        public int limitMax { get; set; }
        /// <summary>
        /// 最低限量
        /// </summary>
        public int limitMin { get; set; }
        /// <summary>
        /// 销售指导价
        /// </summary>
        public string lsj { get; set; }
        /// <summary>
        /// 统一售价
        /// </summary>
        public string lsjAbsolute { get; set; }
        /// <summary>
        /// 最高限价
        /// </summary>
        public string lsjMax { get; set; }
        /// <summary>
        /// 最低限价
        /// </summary>
        public string lsjMin { get; set; }

        public int taskId { get; set; }

    }
   

}
