using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SyncOrderLogistics
    {
      
        public class SyncOrderLogisticsRoot
        {
            /// <summary>
            /// 
            /// </summary>
            public string t { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string order_code { get; set; }
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
            public string billnumber { get; set; }
            /// <summary>
            /// 姚东
            /// </summary>
            public string billreceiver { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string bizdate { get; set; }
            /// <summary>
            /// 山东省济南市历下区窑头
            /// </summary>
            public string sendaddress { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string startdate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string allissuecount { get; set; }
            /// <summary>
            /// 北京泰华伟业
            /// </summary>
            public string wls { get; set; }
            /// <summary>
            /// 
            /// </summary>
            
            public string logisticsNo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<BillnumberitemsItem> billnumberitems { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<LogisticsItem> logistics { get; set; }
        }


        public class BillnumberitemsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string batchnumber { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int issuecount { get; set; }
            /// <summary>
            /// 复方延胡索喷雾剂
            /// </summary>
            public string mtname { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string mtnumber { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double netweight { get; set; }
            /// <summary>
            /// 件/200H
            /// </summary>
            public string unitname { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double xpric { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double xallp { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string validity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string remark { get; set; }
        }

        public class LogisticsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string infoDate { get; set; }
            /// <summary>
            /// 南昌中转
            /// </summary>
            public string place { get; set; }
            /// <summary>
            /// 姚东娟
            /// </summary>
            public string receiver { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string remark { get; set; }
            /// <summary>
            /// 在途
            /// </summary>
            public string state { get; set; }
        }

        

        
    }

        
    
}
