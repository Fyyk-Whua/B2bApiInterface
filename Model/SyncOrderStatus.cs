using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    public class SyncOrderStatus
    {

        public class SyncOrderStatusRoot
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
            public string orderCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string orderStatus { get; set; }
            
        }
    }
}
