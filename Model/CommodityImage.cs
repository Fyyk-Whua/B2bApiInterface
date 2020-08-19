using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CommodityImage
    {
        /// <summary>
        /// 文件base64编码内容
        /// </summary>
        public string base64Str { get; set; }
        /// <summary>
        /// Erp商品Id
        /// </summary>
        public string erpGoodsId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string fileType { get; set; }
        /// <summary>
        /// 图片类型1药品图片;2正面图;3背面图;4:45度角图;5条形码图;6拆包图
        /// </summary>
        public int imageType { get; set; }

        public int taskId { get; set; }
         
    }

}
