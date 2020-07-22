using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    public class Commodity
    {
        /// <summary>
        /// 性状 20200722
        /// </summary>
        public string appearance { get; set; }
        /// <summary>
        /// 批准文号
        /// </summary>
        public string approvalNo { get; set; }
        /// <summary>
        /// 成分 20200722
        /// </summary>
        public string bases { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string brandName { get; set; }
        /// <summary>
        /// 经营范围代码
        /// </summary>
        public string businessScopeCode { get; set; }
        /// <summary>
        /// 经营范围描述
        /// </summary>
        //public string businessScopeName { get; set; }
        /// <summary>
        /// 商品用药分类代码
        /// </summary>
        public string catagoryCode { get; set; }
        /// <summary>
        /// 药物相互作用 20200722
        /// </summary>
        public string drugInteractions { get; set; }
        /// <summary>
        /// Erp商品编号
        /// </summary>
        public string erpGoodsCode { get; set; }
        /// <summary>
        /// Erp商品Id
        /// </summary>
        public string erpGoodsId { get; set; }
        /// <summary>
        /// 商品用药一级分类
        /// </summary>
        public string firstLevel { get; set; }
        /// <summary>
        /// 剂型
        /// </summary>
        public string formula { get; set; }
        /// <summary>
        /// 商品品说明书内容标注(功能主治、用法用量、主要成分、不良反应、禁忌等)  del 20200722
        /// </summary>
        //public string goodsAttr { get; set; }
        /// <summary>
        /// 商品通用名称
        /// </summary>
        public string goodsName { get; set; }
        /// <summary>
        /// 商品规格
        /// </summary>
        public string goodsSpec { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string goodsTradeName { get; set; }
        /// <summary>
        /// 商品类别
        /// </summary>
        public string goodsType { get; set; }
        /// <summary>
        /// 是否医疗器械
        /// </summary>
        public string isMedicalInstruments { get; set; }
        /// <summary>
        /// 适应症/功能主治  20200722
        /// </summary>
        public string majorFunctions { get; set; }
        /// <summary>
        /// 生产厂商
        /// </summary>
        public string manufacturer { get; set; }
        /// <summary>
        /// 上市许可持有人
        /// </summary>
        public string marketingAuthorizationHolder { get; set; }
        /// <summary>
        /// 中包装
        /// </summary>
        public int middlePackAmount { get; set; }
        /// <summary>
        /// 最小不拆零数量(此数量大于1即是不拆零商品)
        /// </summary>
        public int modCount { get; set; }
        /// <summary>
        /// 产地
        /// </summary>
        public string originPlace { get; set; }
        /// <summary>
        /// 件包装
        /// </summary>
        public int packAmount { get; set; }
        /// <summary>
        /// 最小包装单位
        /// </summary>
        public string packUnits { get; set; }
        /// <summary>
        /// 药理作用  20200722
        /// </summary>
        public string pharmacologicalAction { get; set; }
        /// <summary>
        /// 处方分类
        /// </summary>
        public string prescriptionType { get; set; }
        /// <summary>
        /// 质量标准
        /// </summary>
        public string qualityStandard { get; set; }
        /// <summary>
        /// 商品推荐指数(用于搜索强制排序)指数越大排序越靠前
        /// </summary>
        public int recommend { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string searchKey { get; set; }
        /// <summary>
        /// 商品用药二级分类
        /// </summary>
        public string secondLevel { get; set; }
        /// <summary>
        /// 行政区控销
        /// </summary>
        public string sellCtrlAdmin { get; set; }
        /// <summary>
        /// 经营类型控销
        /// </summary>
        public string sellCtrlBusinessType { get; set; }
        /// <summary>
        /// 销售状态
        /// </summary>
        public int sellState { get; set; }
        /// <summary>
        /// 储运条件
        /// </summary>
        public string storageType { get; set; }
        /// <summary>
        /// 贮藏  20200722
        /// </summary>
        public string store { get; set; }
        /// <summary>
        /// 建议零售价
        /// </summary>
        public string suggestedRetailPrice { get; set; }
        /// <summary>
        /// 禁忌  20200722
        /// </summary>
        public string taboo { get; set; }
        /// <summary>
        /// 不良反应  20200722
        /// </summary>
        public string untowardEffect { get; set; }
        /// <summary>
        /// 用法用量  20200722
        /// </summary>
        public string usageDosage { get; set; }
        /// <summary>
        /// 注意事项 20200722
        /// </summary>
        public string warnings { get; set; }
    }
   
}
