using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OrderList
    {
        /// <summary>
        /// 
        /// </summary>
        public string agioAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string erpBuyerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string freePostAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string freePostStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string invoiceType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OrderCommodityListItem> orderCommodityList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderNote { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string paymentAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string postPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string shippingAddr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string shippingReceiver { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string shippingTel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalAmount { get; set; }
    }

    public class OrderCommodityListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string erpGoodsId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderCommodityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderItemNote { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string originalAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string originalPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string paymentAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string paymentPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quantity { get; set; }
    }

    

}
