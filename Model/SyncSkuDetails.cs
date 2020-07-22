using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SyncSkuDetails
    {

        public class SyncSkuRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public List<ItemsItem> items { get; set; }
        }


        public class ItemsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string jdeCodel { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string skGoodsCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string mtName { get; set; }
            /// <summary>
            /// 盒
            /// </summary>
            public string baseUnit { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string bigUnit { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string mtNumber { get; set; }
            /// <summary>
            /// 10*20*2 板
            /// </summary>
            public string mtModel { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int qualityBillNum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int attachmentNum { get; set; }
            /// <summary>
            /// 北京医药科技
            /// </summary>
            public string manufacuturer { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int stockNum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double basePrice { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double terminalPrice { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double wholeSalePrice { get; set; }
            /// <summary>
            /// 生活用品
            /// </summary>
            public string middleUnit { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string mtType { get; set; }
            /// <summary>
            /// 的
            /// </summary>
            public string policy { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string batchNumber { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string updType { get; set; }

            public string skuType { get; set; }
            
        }
    }

}
