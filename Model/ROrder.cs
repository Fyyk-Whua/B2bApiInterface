using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   
    public class SerialNosItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Nbr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ManufactureDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ExpiryDate { get; set; }
    }

    public class ROrderItemsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ManufactureDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ExpiryDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SerialNosItem> SerialNos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SterilizationLot { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SterilizationDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InvoiceNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Decimal Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OriginCountry { get; set; }
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
        public int Qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }
    }

    public class ROrder
    {
        /// <summary>
        /// 
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Mawb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Hawb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Flow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ForecastDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeliveryInfor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WhsCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OwnerCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Msgid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ROrderItemsItem> Items { get; set; }
    }

}
