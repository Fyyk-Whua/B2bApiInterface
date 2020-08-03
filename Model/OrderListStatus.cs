using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OrderListStatus
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        ///  认证状态 3支付完成 6拣货中 7商品已出库
        /// </summary>
        public int orderStatus { get; set; }
    }

}
