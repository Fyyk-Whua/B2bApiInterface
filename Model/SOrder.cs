using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    
    public class SOrderItemsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int Msgitem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Partno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PartName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SerialNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Supplier { get; set; }
    }

    public class SOrder
    {
        /// <summary>
        /// 
        /// </summary>
        public string WhsCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Msgid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TransportationMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CarrierCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CarrierName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal InsureAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SalesCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SalesTel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Shipper { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShipperDistrict { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShipperAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShipperPro { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShipperCity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShipperMobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShipperTel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShipperZip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConsigneePro { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConsigneeCity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConsigneeDistrict { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConsigneeAdd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConsigneeMobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConsigneeTel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConsigneeZip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ForecastDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DeliveryReq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientShippingNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SOrderItemsItem> Items { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OwnerCode { get; set; }
    }

}
