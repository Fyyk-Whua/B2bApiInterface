using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    
    public class OrderOutWarehouseRefund
    {
        /// <summary>
        /// erp 商品ID
        /// </summary>
        public string erpGoodsId { get; set; }
        /// <summary>
        /// erp 出库
        /// </summary>
        public string erpOutboundId { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 订单明细id
        /// </summary>
        public string orderItemId { get; set; }
        /// <summary>
        /// 退款数量
        /// </summary>
        public int refundNum { get; set; }
    }


}
