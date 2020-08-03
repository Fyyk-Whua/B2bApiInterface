using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    

    public class CertificatesListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int imageType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imageUrl { get; set; }
    }

    public class NewCustomerList
    {
        /// <summary>
        /// 
        /// </summary>
        public List<CertificatesListItem> certificatesList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string companyAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string companyLandline { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string companyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int customerTypeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string linkName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string phone { get; set; }
    }


}
