using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    

    public class CommodityRepertory
    {
        /// <summary>
        /// 有效期
        /// </summary>
        public string dateExpiration { get; set; }
        /// <summary>
        /// Erp商品Id
        /// </summary>
        public string erpGoodsId { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        public string productionDate { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public int repertory { get; set; }
    }


}
