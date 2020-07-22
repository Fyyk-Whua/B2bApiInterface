using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    public class OrderOutWarehouse
    {
        /// <summary>
        /// 
        /// </summary>
        public string batchCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string erpGoodsId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderItemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string outboundDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string outboundQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string outboundTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string productionDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string valDate { get; set; }
    }

}
