﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Drawing;
using System.Threading;

namespace BLL
{

    //不允许此 Job 并发执行任务（禁止新开线程执行）
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public sealed class JobsItemsBLL : IJob
    {
        #region Execute 
        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Log4netUtil.LogAppendToForms logAppendToForms = (Log4netUtil.LogAppendToForms)context.JobDetail.JobDataMap.Get("ControlQueue");
            Version Ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string jobName = context.JobDetail.Key.Name;
            string jobGroup = context.JobDetail.Key.Group;
            string logMessage = string.Empty;
            string jobLogType = "QuartzManager";

            JobDataMap jobData = context.JobDetail.JobDataMap;
            Model.JobEntity jobInfo = GetJobEntity(logAppendToForms,jobData);
            if(jobInfo == null)
            {
                logMessage = string.Format("【Execute】GetJobEntity执行异常 jobInfo为空！{0}",string.Empty);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            try
            {
                switch (jobInfo.JobCode.ToString())
                {
                    case "DataApiCommodity":   ///dev-api/dataApi/commodity   同步商品数据接口
                        ExecuteDataApiCommodityJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiCommodityImage":   //dev-api/dataApi/commodityImage   同步商品图片接口
                        ExecuteCommodityImageJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiCommodityPrice":   ///dev-api/dataApi/commodityPrice  同步商品价格接口
                        ExecuteCommodityPriceJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiCommodityRepertory":   ///dev-api/dataApi/commodityRepertory  同步商品库存接口
                        ExecuteCommodityRepertoryJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiCustomer":   ///dev-api/dataApi/customer   同步客户数据接口
                        ExecuteDataApiCustomerJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiCustomerTest":   ///dev-api/dataApi/customer   同步客户数据接口
                        ExecuteDataApiCustomerTestJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiCustomerList":   ///dev-api/dataApi/customerList 获取新注册客户数据接口
                        ExecuteDataApiCustomerListJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiCustomerStatus":   ///dev-api/dataApi/customerStatus   同步新注册客户状态接口(审核不通过后告之)
                        ExecuteDataApiCustomerStatusJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiSalesman":   ///dev-api/dataApi/salesman 同步业务员数据接口
                        ExecuteDataApiSalesmanJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiOrderList":  ///dev-api/dataApi/orderList 获取订单数据接口
                        ExecuteDataApiOrderListJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiOrderStatus":   ///dev-api/dataApi/orderStatus  同步订单状态接口
                        ExecuteDataApiOrderStatusJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiOrderOutWarehouse":  ///dev-api/dataApi/orderOutWarehouse 同步订单出库信息接口
                        ExecuteDataApiOrderOutWarehouseJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiOrderOutWarehouseRefund":  ///dev-api/dataApi/orderOutWarehouseRefund 同步订单出库差异退款接口
                        ExecuteDataApiOrderOutWarehouseRefundJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiOrderInvoice":   //dev-api/dataApi/orderInvoice  同步订单发票接口
                        ExecuteDataApiOrderInvoiceJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiOrderExpress":   ///dev-api/dataApi/orderExpress  同步订单物流单号接口
                        ExecuteDataApiOrderExpressJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiOrderQualityInspectionImage":   ///dev-api/dataApi/orderQualityInspectionImage 上传订单质检图片数据接口
                        break;
                    case "DataApiGoodsSpike":   ///商品秒杀
                        ExecuteDataApiGoodsSpikeJob(logAppendToForms, jobInfo);
                        break;
                    case "GoodsSpikeTask":   ///商品秒杀定时任务
                        ExecuteGoodsSpikeTaskJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiCommodityPriceAsync":   ///dev-api/dataApi/commodityPriceAsync  同步商品价格接口(导步)
                        ExecuteCommodityPriceAsyncJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiCommodityRepertoryAsync":   ///dev-api/dataApi/commodityRepertoryAsync  同步商品库存接口(导步)
                        ExecuteCommodityRepertoryAsyncJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiGetAsyncFailCommodity":   ///dev-api/dataApi/getAsyncFailCommodity  获取异步任务失败接口
                        ExecuteGetAsyncFailCommodityJob(logAppendToForms, jobInfo);
                        break;
                    default:
                        break;
                }
                logMessage = string.Format("【{0}_{1}】Execute 执行完成！Ver.{2}", jobInfo.JobCode, jobInfo.JobName.ToString(), Ver.ToString());
                LogMessage(logAppendToForms, true, logMessage, jobLogType);
            }
            catch (Exception ex)
            {
                logMessage = string.Format("【{0}_{1}】 Execute 执行发生异常:{2}", jobInfo.JobCode, jobInfo.JobName.ToString(), ex.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

        }
        #endregion

        #region GetJobEntity  
        /// <summary>
        /// GetJobEntity 
        /// </summary>
        /// <param name="jobData"></param>
        /// <returns></returns>
        private Model.JobEntity GetJobEntity(Log4netUtil.LogAppendToForms logAppendToForms,
                                             JobDataMap jobData)
        {
            string logMessage = string.Empty;
            try
            {
                Model.JobEntity jobInfo = new Model.JobEntity();
                jobInfo.JobCode = jobData.GetString("JobCode");
                jobInfo.JobName = jobData.GetString("JobName");
                jobInfo.IsDebug = jobData.GetBoolean("IsDebug");
                jobInfo.DomainName = jobData.GetString("DomainName");
                jobInfo.ServiceName = jobData.GetString("ServiceName");
                jobInfo.InterfacePrefix = jobData.GetString("InterfacePrefix");
                jobInfo.ApiModuleType = jobData.GetString("ApiModuleType");
                jobInfo.ApiRequestType = jobData.GetString("ApiRequestType");

                jobInfo.TargetDatabase = jobData.GetString("TargetDatabase");
                jobInfo.ProcedureName = jobData.GetString("ProcedureName");
                jobInfo.ModuleID = jobData.GetString("ModuleID");
                jobInfo.FilterBillType = jobData.GetInt("FilterBillType");
                jobInfo.WritebackProcedureName = jobData.GetString("WritebackProcedureName");
                jobInfo.WritebackType = jobData.GetString("WritebackType");
                jobInfo.PageSize = jobData.GetInt("PageSize");
                jobInfo.InsertTableName = jobData.GetString("InsertTableName");
                jobInfo.CronExpression = jobData.GetString("CronExpression");
                jobInfo.CronExpressionDescription = jobData.GetString("CronExpressionDescription");
                
                jobInfo.EnterpriseId = jobData.GetString("EnterpriseId");
                jobInfo.EnterpriseName = jobData.GetString("EnterpriseName");
                jobInfo.ConfigInfo = Util.NewtonsoftCommon.DeserializeJsonToObject<Model.ConfigInfo>(jobData.GetString("StrConfigInfo"));
                return jobInfo;
            }
            catch (Exception ex)
            {
                logMessage = string.Format("【{0}_{1}】 Execute 执行发生异常:{2}", jobData.GetString("JobCode"), jobData.GetString("JobName"), ex.Message);
                LogError(logAppendToForms, true, logMessage, "QuartzManager");
                return null;
            }
        }
        #endregion


        
        #region ExecuteDataApiCommodityJob    同步商品数据接口
        /// <summary>
        /// ExecuteDataApiCommodityJob  同步商品数据接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiCommodityJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString(); 
            System.Data.DataTable commodityDt = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (commodityDt == null )
            {
                logMessage = string.Format("【{0}_{1}】  同步商品数据失败！  商品数据为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (commodityDt.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

            List<Model.Commodity> commodityItems = new List<Model.Commodity>();


            foreach (System.Data.DataRow dr in commodityDt.Rows)
            {
                Model.Commodity commodityItem = new Model.Commodity();

                commodityItem.appearance = Util.DataTableHelper.DataRowContains(dr, "appearance"); //Add 20200722
                commodityItem.approvalNo = Util.DataTableHelper.DataRowContains(dr, "approvalNo");
                commodityItem.bases = Util.DataTableHelper.DataRowContains(dr, "bases");  //Add 20200722
                commodityItem.brandName = Util.DataTableHelper.DataRowContains(dr, "brandName");
                commodityItem.businessScopeCode = Util.DataTableHelper.DataRowContains(dr, "businessScopeCode");
                //commodityItem.businessScopeName = Util.DataTableHelper.DataRowContains(dr, "businessScopeName");
                commodityItem.catagoryCode = Util.DataTableHelper.DataRowContains(dr, "catagoryCode");
                commodityItem.drugInteractions = Util.DataTableHelper.DataRowContains(dr, "drugInteractions"); //Add 20200722

                commodityItem.erpGoodsCode = Util.DataTableHelper.DataRowContains(dr, "erpGoodsCode");
                commodityItem.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                //commodityItem.firstLevel = Util.DataTableHelper.DataRowContains(dr, "firstLevel");
                commodityItem.formula = Util.DataTableHelper.DataRowContains(dr, "formula");
                commodityItem.logogram = Util.DataTableHelper.DataRowContains(dr, "logogram");

                commodityItem.isMedicalInstruments = Util.DataTableHelper.DataRowContains(dr, "isMedicalInstruments");  //isMedicalInstruments Add 20200722
                //commodityItem.goodsAttr = Util.DataTableHelper.DataRowContains(dr, "goodsAttr");
                commodityItem.goodsName = Util.DataTableHelper.DataRowContains(dr, "goodsName");
                commodityItem.goodsSpec = Util.DataTableHelper.DataRowContains(dr, "goodsSpec");
                commodityItem.goodsTradeName = Util.DataTableHelper.DataRowContains(dr, "goodsTradeName");
                commodityItem.goodsType = Util.DataTableHelper.DataRowContains(dr, "goodsType");
                commodityItem.majorFunctions = Util.DataTableHelper.DataRowContains(dr, "majorFunctions"); //Add 20200722

                commodityItem.manufacturer = Util.DataTableHelper.DataRowContains(dr, "manufacturer");
                commodityItem.manufacturerLogogram = Util.DataTableHelper.DataRowContains(dr, "manufacturerLogogram");
                commodityItem.marketingAuthorizationHolder = Util.DataTableHelper.DataRowContains(dr, "marketingAuthorizationHolder");
                commodityItem.middlePackAmount = Util.DataTableHelper.DataRowContainsInt(dr, "middlePackAmount");
                commodityItem.modCount = Util.DataTableHelper.DataRowContainsInt(dr, "modCount");
                commodityItem.originPlace = Util.DataTableHelper.DataRowContains(dr, "originPlace");  //originPlace
                commodityItem.packAmount = Util.DataTableHelper.DataRowContainsInt(dr, "packAmount");
                commodityItem.packUnits = Util.DataTableHelper.DataRowContains(dr, "packUnits");
                commodityItem.pharmacologicalAction = Util.DataTableHelper.DataRowContains(dr, "pharmacologicalAction");  //pharmacologicalAction
                commodityItem.prescriptionType = Util.DataTableHelper.DataRowContains(dr, "prescriptionType");
                commodityItem.qualityStandard = Util.DataTableHelper.DataRowContains(dr, "qualityStandard");
                commodityItem.recommend = Util.DataTableHelper.DataRowContainsInt(dr, "recommend");
                commodityItem.searchKey = Util.DataTableHelper.DataRowContains(dr, "searchKey");
                //commodityItem.secondLevel = Util.DataTableHelper.DataRowContains(dr, "secondLevel");
                commodityItem.sellCtrlAdmin = Util.DataTableHelper.DataRowContains(dr, "sellCtrlAdmin");
                commodityItem.sellCtrlBusinessType = Util.DataTableHelper.DataRowContains(dr, "sellCtrlBusinessType");
                commodityItem.sellingPoint = Util.DataTableHelper.DataRowContains(dr, "sellingPoint");
                commodityItem.sellState = Util.DataTableHelper.DataRowContainsInt(dr, "sellState");
                commodityItem.storageType = Util.DataTableHelper.DataRowContains(dr, "storageType");
                commodityItem.store = Util.DataTableHelper.DataRowContains(dr, "store");  //store
                commodityItem.suggestedRetailPrice = Math.Round(Util.DataTableHelper.DataRowContainsDecimal(dr, "suggestedRetailPrice"), 2, MidpointRounding.AwayFromZero).ToString("F2");//   suggestedRetailPrice.ToString("F2");
                commodityItem.taboo = Util.DataTableHelper.DataRowContains(dr, "taboo");
                commodityItem.untowardEffect = Util.DataTableHelper.DataRowContains(dr, "untowardEffect");
                commodityItem.usageDosage = Util.DataTableHelper.DataRowContains(dr, "usageDosage");
                commodityItem.warnings = Util.DataTableHelper.DataRowContains(dr, "warnings");
                commodityItem.sellingPoint = Util.DataTableHelper.DataRowContains(dr, "sellingPoint");
                commodityItem.activityType = Util.DataTableHelper.DataRowContainsInt(dr, "activityType");
                commodityItem.activityStartTime = string.Empty;
                commodityItem.activityEndTime = string.Empty;
                commodityItem.taskId = Util.DataTableHelper.DataRowContainsInt(dr, "taskId");

                commodityItems.Add(commodityItem);
            }
            if(commodityItems !=null && commodityItems.Count()<=0)
            {
                logMessage = string.Format("【{0}_{1}】  同步商品数据失败！  商品实体为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            int pagesSize = jobInfo.PageSize <= 0 ? commodityItems.Count() : jobInfo.PageSize;
            int pagesCount = commodityItems.Count();
            int splitCopies = (int)Math.Ceiling((double)pagesCount / pagesSize);
            int pageNum = 0; //页码
            for (int i = 0; i < splitCopies; i++)
            {
                pageNum = i;
                //pageSize ：表示一页多少条。
                //pageNum：表示页数，但是正确的页数是pageNum + 1。因为pageNum = 0，是第一页。pageNum = 1的时候，是第二页。
                //Skip ：表示从第pageNum* pageSize +1条数据开始，也就是说再这之前有pageNum* pageSize条数据。
                //Take：表示显示多少条数据，也就是pageSize条。
                //Take：表示显示多少条数据，也就是pageSize条。
                var pageItems = commodityItems.Skip(pageNum * pagesSize).Take(pagesSize).ToList();
                var arr = pageItems.Select(x => x.taskId).ToList();
                string taskIds = string.Format("{0}", string.Join(",", arr)); //Util.DataTableHelper.GetColumnValuesInt(commodityDt, "taskId");

                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(pageItems);
                string resultJson = string.Empty;
                jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());

                bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                if (!resultStatus)
                {
                    string msg = resultJObject["msg"].ToString();
                    if (string.Equals(msg, "操作超时"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，再次调用一次", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        Thread.Sleep(5000);//休眠时间
                        resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                    }
                }
                if (resultStatus)
                {
                    resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                    string resultJsonData = resultJObject["data"].ToString();  //resultJObject.Value<string>("data");
                    if (!string.IsNullOrEmpty(resultJsonData) && !string.Equals(resultJsonData, "[]"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} 部份商品失败！ 失败商品erpGoodsId:{2}", jobInfo.JobCode, jobInfo.JobName, resultJsonData);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    }

                   
                    string taskIdsReplace = taskIds.Replace(",", string.Empty);
                    if (string.IsNullOrEmpty(taskIdsReplace))
                    {
                        logMessage = string.Format("【{0}_{1}】  任务id:{2}  更新成功后回写失败， 商品taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, taskIds);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        continue;
                    }
                    ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, resultJsonData);
                }
                else
                    continue;
                pageItems = null;
            }
            commodityDt = null;
            commodityItems = null;
            jobInfo = null;
        }
        #endregion

        #region ExecuteCommodityImageJob    同步商品图片接口
        /// <summary>
        /// ExecuteCommodityImageJob  同步商品图片接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteCommodityImageJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null )
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.CommodityImage> items = new List<Model.CommodityImage>();

            dataTable = ImagesFileToBytes(logAppendToForms, jobInfo, dataTable);
            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 加载图片后数据为空1！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            //isImagesCompleted
            System.Data.DataTable imageDetailsDt = Util.DataTableHelper.GetNewDataTable(dataTable, "isImagesCompleted='Y' ");
            foreach (System.Data.DataRow dr in imageDetailsDt.Rows)
            {
                Model.CommodityImage item = new Model.CommodityImage();
                item.base64Str =  Util.DataTableHelper.DataRowContains(dr, "base64Str"); 
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                item.fileName = Util.DataTableHelper.DataRowContains(dr, "fileName");
                item.fileType = Util.DataTableHelper.DataRowContains(dr, "fileType").Replace(".",string.Empty);
                item.imageType = Util.DataTableHelper.DataRowContainsInt(dr, "imageType");  //图片类型1药品图片;2正面图;3背面图;4:45度角图;5条形码图;6拆包图
                item.taskId = Util.DataTableHelper.DataRowContainsInt(dr, "taskId");
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

            int pagesSize = jobInfo.PageSize <= 0 ? items.Count() : jobInfo.PageSize;
            int pagesCount = items.Count();
            int splitCopies = (int)Math.Ceiling((double)pagesCount / pagesSize);
            int pageNum = 0; //页码
            for (int i = 0; i < splitCopies; i++)
            {
                pageNum = i;
                //pageSize ：表示一页多少条。
                //pageNum：表示页数，但是正确的页数是pageNum + 1。因为pageNum = 0，是第一页。pageNum = 1的时候，是第二页。
                //Skip ：表示从第pageNum* pageSize +1条数据开始，也就是说再这之前有pageNum* pageSize条数据。
                //Take：表示显示多少条数据，也就是pageSize条。
                //Take：表示显示多少条数据，也就是pageSize条。
                var pageItems = items.Skip(pageNum * pagesSize).Take(pagesSize).ToList();
                string taskIds = string.Empty; //Util.DataTableHelper.GetColumnValuesInt(dataTable, "taskId");
                foreach (var pageItem in pageItems)
                {
                    string taskId = pageItem.taskId.ToString();
                    string imageType = pageItem.imageType.ToString();
                    taskIds = string.Format("{0}{1}:{2},", taskIds, taskId, imageType);
                }
                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(pageItems);
                string resultJson = string.Empty;
                jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());

                bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                if (!resultStatus)
                {
                    string msg = resultJObject["msg"].ToString();
                    if (string.Equals(msg, "操作超时"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，再次调用一次", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        Thread.Sleep(5000);//休眠时间
                        resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                    }
                }
                if (resultStatus)//(CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
                {
                    resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                    string resultJsonData = resultJObject["data"].ToString();  //resultJObject.Value<string>("data");
                    if (!string.IsNullOrEmpty(resultJsonData) && !string.Equals(resultJsonData, "[]"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1}  {2} 部份商品图片失败！ 失败商品erpGoodsId:{3}", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo,resultJsonData);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    }

                    string taskIdsReplace = taskIds.Replace(",", string.Empty);
                    if (string.IsNullOrEmpty(taskIdsReplace))
                    {
                        logMessage = string.Format("【{0}_{1}】  {2} 任务id:{3} 更新成功后回写失败， 商品图片taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, taskIds);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        continue;
                    }
                    ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, resultJsonData);
                }
                else
                    continue;
                pageItems.Clear();
            }
            imageDetailsDt.Clear();
            dataTable.Clear();
            imageDetailsDt = null;
            dataTable = null;
            jobInfo = null;
        }
        #endregion
   
        #region ExecuteCommodityPriceJob    同步商品价格接口
        /// <summary>
        /// ExecuteCommodityPriceJob  同步商品价格接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteCommodityPriceJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null )
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.CommodityPrice> items = new List<Model.CommodityPrice>();


            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.CommodityPrice item = new Model.CommodityPrice();
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                item.limitMax = Util.DataTableHelper.DataRowContainsInt(dr, "limitMax");
                item.limitMin = Util.DataTableHelper.DataRowContainsInt(dr, "limitMin");
                item.lsj = Math.Round(Util.DataTableHelper.DataRowContainsDecimal(dr, "lsj"), 2, MidpointRounding.AwayFromZero).ToString("F2");// item.lsj = Util.DataTableHelper.DataRowContains(dr, "lsj");
                item.lsjAbsolute = Math.Round(Util.DataTableHelper.DataRowContainsDecimal(dr, "lsjAbsolute"), 2, MidpointRounding.AwayFromZero).ToString("F2");// Util.DataTableHelper.DataRowContains(dr, "lsjAbsolute");
                item.lsjMax = Math.Round(Util.DataTableHelper.DataRowContainsDecimal(dr, "lsjMax"), 2, MidpointRounding.AwayFromZero).ToString("F2");//  Util.DataTableHelper.DataRowContains(dr, "lsjMax");
                item.lsjMin = Math.Round(Util.DataTableHelper.DataRowContainsDecimal(dr, "lsjMin"), 2, MidpointRounding.AwayFromZero).ToString("F2");//  Util.DataTableHelper.DataRowContains(dr, "lsjMin");
                item.taskId = Util.DataTableHelper.DataRowContainsInt(dr, "taskId");
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            int pagesSize = jobInfo.PageSize <= 0 ? items.Count() : jobInfo.PageSize;
            int pagesCount = items.Count();
            int splitCopies = (int)Math.Ceiling((double)pagesCount / pagesSize);
            int pageNum = 0; //页码
            for (int i = 0; i < splitCopies; i++)
            {
                pageNum = i;
                //pageSize ：表示一页多少条。
                //pageNum：表示页数，但是正确的页数是pageNum + 1。因为pageNum = 0，是第一页。pageNum = 1的时候，是第二页。
                //Skip ：表示从第pageNum* pageSize +1条数据开始，也就是说再这之前有pageNum* pageSize条数据。
                //Take：表示显示多少条数据，也就是pageSize条。
                //Take：表示显示多少条数据，也就是pageSize条。
                var pageItems = items.Skip(pageNum * pagesSize).Take(pagesSize).ToList();
                var arr = pageItems.Select(x => x.taskId).ToList();
                string taskIds = string.Format("{0}", string.Join(",", arr));
                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(pageItems);
                string resultJson = string.Empty;
                jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());

                bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                if (!resultStatus)
                {
                    string msg = resultJObject["msg"].ToString();
                    if (string.Equals(msg,"操作超时"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，再次调用一次", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        Thread.Sleep(5000);//休眠时间
                        resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                    }
                }
                if (resultStatus)
                {
                    resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                    string resultJsonData = resultJObject["data"].ToString();  //resultJObject.Value<string>("data");
                    if (!string.IsNullOrEmpty(resultJsonData) && !string.Equals(resultJsonData, "[]"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} {2} 部份商品价格失败！ 失败商品erpGoodsId:{3}", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, resultJsonData);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    }
                    string taskIdsReplace = taskIds.Replace(",", string.Empty);
                    if (string.IsNullOrEmpty(taskIdsReplace))
                    {
                        logMessage = string.Format("【{0}_{1}】  {2} 任务id:{3}  更新成功后回写失败， 商品价格taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, taskIds);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        continue;
                    }
                    ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, resultJsonData);
                }
                else
                    continue;
                pageItems = null;
            }
            dataTable = null;
            items = null;
            jobInfo = null;
        }
        #endregion

        #region ExecuteCommodityRepertoryJob    同步商品库存接口
        /// <summary>
        /// ExecuteCommodityRepertoryJob  同步商品库存接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteCommodityRepertoryJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null )
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

           

            List <Model.CommodityRepertory> items = new List<Model.CommodityRepertory>();


            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.CommodityRepertory item = new Model.CommodityRepertory();

                item.dateExpiration = Util.DataTableHelper.DataRowContains(dr, "dateExpiration");  //dateExpiration
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                item.productionDate = Util.DataTableHelper.DataRowContains(dr, "productionDate");
                item.repertory = Util.DataTableHelper.DataRowContainsInt(dr, "repertory");
                item.shelveStatus = Util.DataTableHelper.DataRowContainsInt(dr, "shelveStatus");
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

            int pagesSize = jobInfo.PageSize<=0 ? items.Count():jobInfo.PageSize;
            int pagesCount = items.Count();
            int splitCopies = (int)Math.Ceiling((double)pagesCount / pagesSize);
            int pageNum = 0; //页码
            for (int i = 0; i < splitCopies; i++)
            {
                pageNum = i;
                //pageSize ：表示一页多少条。
                //pageNum：表示页数，但是正确的页数是pageNum + 1。因为pageNum = 0，是第一页。pageNum = 1的时候，是第二页。
                //Skip ：表示从第pageNum* pageSize +1条数据开始，也就是说再这之前有pageNum* pageSize条数据。
                //Take：表示显示多少条数据，也就是pageSize条。
                //Take：表示显示多少条数据，也就是pageSize条。
                var pageItems = items.Skip(pageNum * pagesSize).Take(pagesSize).ToList();

                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(pageItems);
                string resultJson = string.Empty;
                jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());
                //CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                if (!resultStatus)
                {
                    string msg = resultJObject["msg"].ToString();
                    if (string.Equals(msg, "操作超时"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，第二次调用", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        Thread.Sleep(5000);//休眠时间
                        resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                        resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                        if (!resultStatus)
                        {
                            msg = resultJObject["msg"].ToString();
                            if (string.Equals(msg, "操作超时"))
                            {
                                logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时10秒，第三次调用", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                                Thread.Sleep(10000);//休眠时间
                                resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);

                            }
                        }

                    }
                }
                pageItems = null;
            }
            dataTable = null;
            items = null;
            jobInfo = null;
        }
        #endregion

        #region ExecuteCommodityPriceAsyncJob    同步商品价格接口（异步）
        /// <summary>
        /// ExecuteCommodityPriceAsyncJob  同步商品价格接口（异步）
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteCommodityPriceAsyncJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.CommodityPrice> items = new List<Model.CommodityPrice>();


            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.CommodityPrice item = new Model.CommodityPrice();
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                item.limitMax = Util.DataTableHelper.DataRowContainsInt(dr, "limitMax");
                item.limitMin = Util.DataTableHelper.DataRowContainsInt(dr, "limitMin");
                item.lsj = Math.Round(Util.DataTableHelper.DataRowContainsDecimal(dr, "lsj"), 2, MidpointRounding.AwayFromZero).ToString("F2");// item.lsj = Util.DataTableHelper.DataRowContains(dr, "lsj");
                item.lsjAbsolute = Math.Round(Util.DataTableHelper.DataRowContainsDecimal(dr, "lsjAbsolute"), 2, MidpointRounding.AwayFromZero).ToString("F2");// Util.DataTableHelper.DataRowContains(dr, "lsjAbsolute");
                item.lsjMax = Math.Round(Util.DataTableHelper.DataRowContainsDecimal(dr, "lsjMax"), 2, MidpointRounding.AwayFromZero).ToString("F2");//  Util.DataTableHelper.DataRowContains(dr, "lsjMax");
                item.lsjMin = Math.Round(Util.DataTableHelper.DataRowContainsDecimal(dr, "lsjMin"), 2, MidpointRounding.AwayFromZero).ToString("F2");//  Util.DataTableHelper.DataRowContains(dr, "lsjMin");
                item.taskId = Util.DataTableHelper.DataRowContainsInt(dr, "taskId");
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            int pagesSize = jobInfo.PageSize <= 0 ? items.Count() : jobInfo.PageSize;
            int pagesCount = items.Count();
            int splitCopies = (int)Math.Ceiling((double)pagesCount / pagesSize);
            int pageNum = 0; //页码
            for (int i = 0; i < splitCopies; i++)
            {
                pageNum = i;
                //pageSize ：表示一页多少条。
                //pageNum：表示页数，但是正确的页数是pageNum + 1。因为pageNum = 0，是第一页。pageNum = 1的时候，是第二页。
                //Skip ：表示从第pageNum* pageSize +1条数据开始，也就是说再这之前有pageNum* pageSize条数据。
                //Take：表示显示多少条数据，也就是pageSize条。
                //Take：表示显示多少条数据，也就是pageSize条。
                var pageItems = items.Skip(pageNum * pagesSize).Take(pagesSize).ToList();
                var arr = pageItems.Select(x => x.taskId).ToList();
                string taskIds = string.Format("{0}", string.Join(",", arr));
                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(pageItems);
                string resultJson = string.Empty;
                jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());

                bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                if (!resultStatus)
                {
                    string msg = resultJObject["msg"].ToString();
                    if (string.Equals(msg, "操作超时"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，再次调用一次", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        Thread.Sleep(5000);//休眠时间
                        resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                    }
                }
                if (resultStatus)
                {
                    resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                    string resultJsonData = resultJObject["data"].ToString();  //resultJObject.Value<string>("data");
                    if (!string.IsNullOrEmpty(resultJsonData) && !string.Equals(resultJsonData, "[]"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} {2} 部份商品价格失败！ 失败商品erpGoodsId:{3}", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, resultJsonData);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    }
                    string taskIdsReplace = taskIds.Replace(",", string.Empty);
                    if (string.IsNullOrEmpty(taskIdsReplace))
                    {
                        logMessage = string.Format("【{0}_{1}】  {2} 任务id:{3}  更新成功后回写失败， 商品价格taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, taskIds);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        continue;
                    }
                    ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, resultJsonData);
                }
                else
                    continue;
                pageItems = null;
            }
            dataTable = null;
            items = null;
            jobInfo = null;
        }
        #endregion

        #region ExecuteCommodityRepertoryAsyncJob    同步商品库存接口（异步）
        /// <summary>
        /// ExecuteCommodityRepertoryAsyncJob  同步商品库存接口（异步）
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteCommodityRepertoryAsyncJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }



            List<Model.CommodityRepertory> items = new List<Model.CommodityRepertory>();


            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.CommodityRepertory item = new Model.CommodityRepertory();

                item.dateExpiration = Util.DataTableHelper.DataRowContains(dr, "dateExpiration");  //dateExpiration
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                item.productionDate = Util.DataTableHelper.DataRowContains(dr, "productionDate");
                item.repertory = Util.DataTableHelper.DataRowContainsInt(dr, "repertory");
                item.shelveStatus = Util.DataTableHelper.DataRowContainsInt(dr, "shelveStatus");
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

            int pagesSize = jobInfo.PageSize <= 0 ? items.Count() : jobInfo.PageSize;
            int pagesCount = items.Count();
            int splitCopies = (int)Math.Ceiling((double)pagesCount / pagesSize);
            int pageNum = 0; //页码
            for (int i = 0; i < splitCopies; i++)
            {
                pageNum = i;
                //pageSize ：表示一页多少条。
                //pageNum：表示页数，但是正确的页数是pageNum + 1。因为pageNum = 0，是第一页。pageNum = 1的时候，是第二页。
                //Skip ：表示从第pageNum* pageSize +1条数据开始，也就是说再这之前有pageNum* pageSize条数据。
                //Take：表示显示多少条数据，也就是pageSize条。
                //Take：表示显示多少条数据，也就是pageSize条。
                var pageItems = items.Skip(pageNum * pagesSize).Take(pagesSize).ToList();

                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(pageItems);
                string resultJson = string.Empty;
                jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());
                //CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                if (!resultStatus)
                {
                    string msg = resultJObject["msg"].ToString();
                    if (string.Equals(msg, "操作超时"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，第二次调用", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        Thread.Sleep(5000);//休眠时间
                        resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                        resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                        if (!resultStatus)
                        {
                            msg = resultJObject["msg"].ToString();
                            if (string.Equals(msg, "操作超时"))
                            {
                                logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时10秒，第三次调用", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                                Thread.Sleep(10000);//休眠时间
                                resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);

                            }
                        }

                    }
                }
                pageItems = null;
            }
            dataTable = null;
            items = null;
            jobInfo = null;
        }
        #endregion


        #region ExecuteGetAsyncFailCommodityJob    获取异步任务失败接口
        /// <summary>
        /// ExecuteGetAsyncFailCommodityJob  获取异步任务失败接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteGetAsyncFailCommodityJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();


            var requestJObject = new Newtonsoft.Json.Linq.JObject();
            requestJObject.Add("key", "repertory_1601355485829");
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(requestJObject);
            string resultJson = string.Empty;


            // UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), "1d015867ee5446738d39c48445a05839", string.Empty, 4);
            //return;
            //resultJson = "{\"msg\":\"操作成功\",\"code\":200,\"data\":[{\"customerId\":\"1e24b4d465f645e7b041bf1015fc5e86\",\"customerTypeId\":1,\"companyName\":\"红太阳药店\",\"companyAddress\":\"摩尔城123号1\",\"companyLandline\":\"18911110012\",\"linkName\":\"小太阳1\",\"phone\":\"17502038345\",\"createTime\":\"2020-07-09T14:09:13.000+0800\",\"certificatesList\":[{\"imageType\":1,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/68cd69b026874e6e83206f3adaf8956c.jpg\"},{\"imageType\":2,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/06da3a351cc449978360616a33fd7ac6.jpg\"},{\"imageType\":3,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/1f2fd3984ea440299b99cb4ed7597914.jpg\"},{\"imageType\":4,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/5aac2e933dd3483d97d9c340971c49b9.jpg\"},{\"imageType\":5,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/641a0a0e58504c9fb61661a962d818eb.jpg\"},{\"imageType\":6,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/5ec3b06faf9247d1bc6a5786710476f9.jpg\"},{\"imageType\":7,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/18920a2ad06c45be913ec0de1ff03436.jpg\"},{\"imageType\":17,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/5728224f27fa411ebfc61c995278b3c8.jpg\"},{\"imageType\":18,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/7af70afb61eb4f0e8b7044e1027fab3c.jpg\"},{\"imageType\":19,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/f887f14c2d2e4bf29038bc034bb9ac35.jpg\"}]}]}";
            //resultJson = "{\"msg\":\"操作成功\",\"code\":200,\"data\":[{\"customerId\":\"5306d86b85ab4869aa673932da6651aa\",\"customerTypeId\":0,\"companyName\":\"湖北省武汉市牛科技\",\"companyAddress\":\"湖北省武汉市武昌区\",\"companyLandline\":null,\"linkName\":\"牛科技\",\"phone\":\"17786493669\",\"createTime\":\"2020-07-25T14:24:35.000+0800\",\"certificatesList\":[]},{\"customerId\":\"a1ddb221d3294fe0b56bf87ea9c10455\",\"customerTypeId\":2,\"companyName\":\"格林优药汇\",\"companyAddress\":\"东风大道1号\",\"companyLandline\":\"\",\"linkName\":\"刘洪\",\"phone\":\"13971171880\",\"createTime\":\"2020-07-28T10:26:26.000+0800\",\"certificatesList\":[{\"imageType\":1,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/16962c990007465a9439502b0b67903a.jpg\"},{\"imageType\":2,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/21fbb5cb866d430290478aac53c36e64.jpg\"},{\"imageType\":3,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/09bd41d994134686baeaa4c179340553.jpg\"},{\"imageType\":4,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/988cf69533624d80ab17b1ce0cd9404a.jpg\"},{\"imageType\":5,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/58be8102481442fe87672cf105987d09.jpg\"},{\"imageType\":6,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/3a7b676075784fec9e49718ea97cefc2.jpg\"},{\"imageType\":7,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/5d93a20777954538a99c64dfbd1b8db3.jpg\"},{\"imageType\":8,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/eac8cde0a7fc4941a1604f65d3d32ef5.jpg\"},{\"imageType\":10,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/b3a8de9d54424169ad4dee4e5bd792e7.jpg\"},{\"imageType\":11,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/b6b50ee59c2544f5bc21c465d09dd69e.jpg\"},{\"imageType\":12,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/77e776c1317b4998a828ff4db95db35d.jpg\"},{\"imageType\":13,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/b699e69e2a36435d9c26f3c4e13db00d.jpg\"},{\"imageType\":14,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/7b5c04c29d3a431284aff72ca3facb3f.jpg\"},{\"imageType\":17,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/893ba7b1ce6f4d93964ae9a0a069aae4.jpg\"},{\"imageType\":18,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/119bf0044f9b48e7a4eedf194c932fe3.jpg\"},{\"imageType\":19,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/16aa5596244e42e2b793ae8379646345.jpg\"}]},{\"customerId\":\"e81fb2eab30a4164a4b09a9640e62a18\",\"customerTypeId\":0,\"companyName\":\"yu药店02\",\"companyAddress\":\"一二三四五六七八九拾一二三四五六七八九廿一二三四五六七八九叁\",\"companyLandline\":null,\"linkName\":\"喻\",\"phone\":\"18672391726\",\"createTime\":\"2020-07-25T11:50:11.000+0800\",\"certificatesList\":[]}]}";
            //if (!string.IsNullOrEmpty(resultJson))
            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                /* Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                 string resultJsonData = resultJObject["data"].ToString();  //resultJObject.Value<string>("data");
                 if (string.IsNullOrEmpty(resultJsonData) || string.Equals(resultJsonData, "[]"))
                 {
                     logMessage = string.Format("【{0}_{1}】  {1} 无新注册客户！！！", jobInfo.JobCode, jobInfo.JobName);
                     LogWarning(logAppendToForms, true, logMessage, jobLogType);
                     return;
                 }
                 Newtonsoft.Json.Linq.JArray orderJArray = Newtonsoft.Json.Linq.JArray.Parse(resultJsonData);
                 if (orderJArray.Count() <= 0)
                 {
                     logMessage = string.Format("【{0}_{1}】  {1} 新注册客户主表失败！ 返回orderJArray数组为空！！！", jobInfo.JobCode, jobInfo.JobName);
                     LogWarning(logAppendToForms, true, logMessage, jobLogType);
                     return;
                 }
                 foreach (var item in orderJArray)
                 {
                     string companyName = item["companyName"].ToString();
                     string customerId = item["customerId"].ToString();
                     //UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                     //continue;
                     Newtonsoft.Json.Linq.JObject itemJObject = (Newtonsoft.Json.Linq.JObject)item;
                     string strCertificatesList = itemJObject["certificatesList"].ToString();
                     Newtonsoft.Json.Linq.JArray jArrayCertificatesList = new Newtonsoft.Json.Linq.JArray();
                     if (string.IsNullOrEmpty(strCertificatesList) || string.Equals(strCertificatesList, "[]"))
                     {
                         logMessage = string.Format("【{0}_{1}】  客户名称:{2} ;客户Id:{3}   证照图片资料失败！ 返回certificatesList数组为空！！！", jobInfo.JobCode, jobInfo.JobName, companyName, customerId);
                         LogWarning(logAppendToForms, true, logMessage, jobLogType);
                         itemJObject.Remove("certificatesList");
                         //continue;
                     }
                     else
                     {
                         jArrayCertificatesList = Newtonsoft.Json.Linq.JArray.Parse(strCertificatesList); //(Newtonsoft.Json.Linq.JArray)item["certificatesList"];
                         itemJObject.Remove("certificatesList");
                     }
                     string itemJObjectJson = itemJObject.ToString();
                     Newtonsoft.Json.Linq.JArray jArrayOrder = new Newtonsoft.Json.Linq.JArray();//Util.NewtonsoftCommon.ConvertJsonToJArray(logAppendToForms, jobInfo, itemJObjectJson);
                     jArrayOrder.Add(itemJObject);
                     System.Data.DataTable dataTable = Util.NewtonsoftCommon.ConvertJArrayToDataTable(logAppendToForms, jobInfo, jArrayOrder);
                     if (dataTable == null || dataTable.Rows.Count <= 0)
                     {
                         logMessage = string.Format("【{0}_{1}】  客户名称:{2} ;客户Id:{3}  转换DataTable失败！！！", jobInfo.JobCode, jobInfo.JobName, companyName, customerId);
                         LogError(logAppendToForms, true, logMessage, jobLogType);
                         UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                         continue;
                     }
                     if (!dataTable.Columns.Contains("updated"))
                         dataTable.Columns.Add("updated", typeof(string));
                     string updated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                     foreach (System.Data.DataRow itemsDr in dataTable.Rows)
                     {
                         itemsDr["customerId"] = customerId;
                         itemsDr["updated"] = updated;
                     }
                     string insertTableName = string.Empty;
                     if (string.IsNullOrEmpty(jobInfo.InsertTableName))
                     {
                         logMessage = string.Format("【{0}_{1}】  {1}失败 未定义新注册客户表！！！", jobInfo.JobCode, jobInfo.JobName);
                         LogError(logAppendToForms, true, logMessage, jobLogType);
                         UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                         continue;
                     }
                     string[] arr = jobInfo.InsertTableName.Split(',');
                     if (arr.Count() > 0)
                         insertTableName = arr[0].ToString();
                     if (string.IsNullOrEmpty(insertTableName))
                     {
                         logMessage = string.Format("【{0}_{1}】  {1}失败 未定义新注册客户主表！！！", jobInfo.JobCode, jobInfo.JobName);
                         LogError(logAppendToForms, true, logMessage, jobLogType);
                         UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                         continue;
                     }
                     int dbr = BulkInsertDatabaseInt(logAppendToForms, jobInfo, dataTable, insertTableName);
                     if (dbr > 0)
                     {
                         if (int.Equals(dbr, 2))
                         {
                             string strSql = CustomerDataTableToStrUpdate(dataTable, insertTableName, customerId);
                             if (!BulkInsertDatabase(logAppendToForms, jobInfo, insertTableName, strSql))
                             {
                                 UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                                 continue;
                             }
                         }
                         if (jArrayCertificatesList.Count() <= 0)
                         {
                             logMessage = string.Format("【{0}_{1}】  客户名称:{2} ;客户Id:{3}   证照图片资料失败！ 返回certificatesList数组为空2！！！", jobInfo.JobCode, jobInfo.JobName, companyName, customerId);
                             LogError(logAppendToForms, true, logMessage, jobLogType);
                             continue;
                         }
                         System.Data.DataTable itemsDt = Util.NewtonsoftCommon.ConvertJArrayToDataTable(logAppendToForms, jobInfo, jArrayCertificatesList);
                         if (itemsDt == null || itemsDt.Rows.Count <= 0)
                         {
                             logMessage = string.Format("【{0}_{1}】  客户名称:{2} ;客户Id:{3}   证照图片资料转换DataTable失败！！！", jobInfo.JobCode, jobInfo.JobName, companyName, customerId);
                             LogError(logAppendToForms, true, logMessage, jobLogType);
                             UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                             continue;
                         }
                         //加字段
                         if (!itemsDt.Columns.Contains("customerId"))
                             itemsDt.Columns.Add("customerId", typeof(string));
                         if (!itemsDt.Columns.Contains("updated"))
                             itemsDt.Columns.Add("updated", typeof(string));
                         foreach (System.Data.DataRow itemsDr in itemsDt.Rows)
                         {
                             itemsDr["customerId"] = customerId;
                             itemsDr["updated"] = updated;
                         }
                         insertTableName = string.Empty;
                         if (arr.Count() > 1)
                             insertTableName = arr[1].ToString();
                         if (string.IsNullOrEmpty(insertTableName))
                         {
                             logMessage = string.Format("【{0}_{1}】  {1} 新注册客户证照图片资料失败 未定义新注册客户证照图片资料表！！！", jobInfo.JobCode, jobInfo.JobName);
                             LogError(logAppendToForms, true, logMessage, jobLogType);
                             UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                             continue;
                         }
                         string itemsSql = DataTableToStrDeleteAndInsert(itemsDt, insertTableName);
                         if (!BulkInsertDatabase(logAppendToForms, jobInfo, insertTableName, itemsSql))
                         {
                             UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                             continue;
                         }
                         //新注册客户插入成功后回写Erp  自动生成首营申请单  ？？

                         //ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), customerId, string.Empty);

                     }
                     else
                     {
                         //插入数据库失败 
                         UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                         continue;
                     }
                 }*/
            }
        }
        #endregion

        #region ExecuteDataApiCustomerJob   同步客户数据接口
        /// <summary>
        /// ExecuteDataApiCustomerJob 同步客户数据接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiCustomerJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode;  //Model
            System.Data.DataTable customerDt = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (customerDt == null )
            {
                logMessage = string.Format("【{0}_{1}】  同步客户数据失败！  客户数据为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

            if (customerDt.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.Customer> customerItems = new List<Model.Customer>();

            foreach (System.Data.DataRow dr in customerDt.Rows)
            {
                Model.Customer customerItem = new Model.Customer();
                customerItem.address = Util.DataTableHelper.DataRowContains(dr, "address");
                customerItem.apparatusLicenseCode = Util.DataTableHelper.DataRowContains(dr, "apparatusLicenseCode");
                customerItem.apparatusLicenseValidate = Util.DataTableHelper.DataRowContains(dr, "apparatusLicenseValidate");
                customerItem.area = Util.DataTableHelper.DataRowContains(dr, "area");
                customerItem.bankAccount = Util.DataTableHelper.DataRowContains(dr, "bankAccount");
                customerItem.businessLicenseCode = Util.DataTableHelper.DataRowContains(dr, "businessLicenseCode");
                customerItem.businessLicenseValidate = Util.DataTableHelper.DataRowContains(dr, "businessLicenseValidate");
                customerItem.clientIdcard = Util.DataTableHelper.DataRowContains(dr, "clientIdcard");
                customerItem.clientName = Util.DataTableHelper.DataRowContains(dr, "clientName");
                customerItem.clientPhone = Util.DataTableHelper.DataRowContains(dr, "clientPhone");
                customerItem.clientTimelimit = Util.DataTableHelper.DataRowContains(dr, "clientTimelimit");
                customerItem.companyAddress = Util.DataTableHelper.DataRowContains(dr, "companyAddress");
                customerItem.companyName = Util.DataTableHelper.DataRowContains(dr, "companyName");
                customerItem.companyPrincipal = Util.DataTableHelper.DataRowContains(dr, "companyPrincipal");
                customerItem.consignee = Util.DataTableHelper.DataRowContains(dr, "consignee");
                customerItem.consigneePhone = Util.DataTableHelper.DataRowContains(dr, "consigneePhone");
                customerItem.contactWay = Util.DataTableHelper.DataRowContains(dr, "contactWay");
                customerItem.customerId = Util.DataTableHelper.DataRowContains(dr, "customerId");  //customerId Add 20200722
                customerItem.customerTypeId = Util.DataTableHelper.DataRowContains(dr, "customerTypeId");
                customerItem.depositBank = Util.DataTableHelper.DataRowContains(dr, "depositBank");
                customerItem.depotAddress = Util.DataTableHelper.DataRowContains(dr, "depotAddress");
                customerItem.drugInterceptScope = Util.DataTableHelper.DataRowContains(dr, "drugInterceptScope");
                customerItem.dutyNumber = Util.DataTableHelper.DataRowContains(dr, "dutyNumber");
                customerItem.erpCode = Util.DataTableHelper.DataRowContains(dr, "erpCode");
                customerItem.erpId = Util.DataTableHelper.DataRowContains(dr, "erpId");
                customerItem.erpSalesmanId = Util.DataTableHelper.DataRowContains(dr, "erpSalesmanId");
                customerItem.foodLicenseCode = Util.DataTableHelper.DataRowContains(dr, "foodLicenseCode");
                customerItem.foodLicenseValidate = Util.DataTableHelper.DataRowContains(dr, "foodLicenseValidate");
                customerItem.gspCode = Util.DataTableHelper.DataRowContains(dr, "gspCode");
                customerItem.gspValidate = Util.DataTableHelper.DataRowContains(dr, "gspValidate");
                customerItem.instrumentInterceptScope = Util.DataTableHelper.DataRowContains(dr, "instrumentInterceptScope");
                customerItem.isForProfit = Util.DataTableHelper.DataRowContainsInt(dr, "isForProfit");
                customerItem.isRefrigeration = Util.DataTableHelper.DataRowContainsInt(dr, "isRefrigeration");
                customerItem.legalPerson = Util.DataTableHelper.DataRowContains(dr, "legalPerson");
                customerItem.level = Util.DataTableHelper.DataRowContains(dr, "level");
                customerItem.licenceCode = Util.DataTableHelper.DataRowContains(dr, "licenceCode");
                customerItem.licenceStartDate = Util.DataTableHelper.DataRowContains(dr, "licenceStartDate");
                customerItem.licenceType = Util.DataTableHelper.DataRowContainsInt(dr, "licenceType");
                customerItem.licenceValidate = Util.DataTableHelper.DataRowContains(dr, "licenceValidate");
                customerItem.payType = Util.DataTableHelper.DataRowContains(dr, "payType");
                customerItem.qualityPrincipal = Util.DataTableHelper.DataRowContains(dr, "qualityPrincipal");
                customerItem.registeredCapital = Util.DataTableHelper.DataRowContains(dr, "registeredCapital");
                customerItem.status = Util.DataTableHelper.DataRowContainsInt(dr, "status");
                customerItem.scope = Util.DataTableHelper.DataRowContains(dr, "scope");
                customerItem.specifyQuotation = Util.DataTableHelper.DataRowContains(dr, "specifyQuotation");
                customerItem.twoApparatusLicenseCode = Util.DataTableHelper.DataRowContains(dr, "twoApparatusLicenseCode");
                customerItem.taskId = Util.DataTableHelper.DataRowContainsInt(dr, "taskId");
                customerItems.Add(customerItem);
            }
            if (customerItems != null && customerItems.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  同步客户数据失败！  客户实体为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            int pagesSize = jobInfo.PageSize <= 0 ? customerItems.Count() : jobInfo.PageSize;
            int pagesCount = customerItems.Count();
            int splitCopies = (int)Math.Ceiling((double)pagesCount / pagesSize);
            int pageNum = 0; //页码
            for (int i = 0; i < splitCopies; i++)
            {
                pageNum = i;
                //pageSize ：表示一页多少条。
                //pageNum：表示页数，但是正确的页数是pageNum + 1。因为pageNum = 0，是第一页。pageNum = 1的时候，是第二页。
                //Skip ：表示从第pageNum* pageSize +1条数据开始，也就是说再这之前有pageNum* pageSize条数据。
                //Take：表示显示多少条数据，也就是pageSize条。
                //Take：表示显示多少条数据，也就是pageSize条。
                var pageItems = customerItems.Skip(pageNum * pagesSize).Take(pagesSize).ToList();
                var arr = pageItems.Select(x => x.taskId).ToList();
                string taskIds = string.Format("{0}", string.Join(",", arr));
                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(pageItems);
                string resultJson = string.Empty;
                jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum+1).ToString());


                if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
                {
                    Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                    string resultJsonData = resultJObject["data"].ToString();  //resultJObject.Value<string>("data");
                    if (!string.IsNullOrEmpty(resultJsonData) && !string.Equals(resultJsonData, "[]"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1}  {2} 部份失败！ 失败erpId:{3}", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, resultJsonData);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    }

                    string taskIdsReplace = taskIds.Replace(",", string.Empty);
                    if (string.IsNullOrEmpty(taskIdsReplace))
                    {
                        logMessage = string.Format("【{0}_{1}】  {2} 任务id:{3}  更新成功后回写失败; 客户taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, taskIds);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        continue;
                    }
                    ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, resultJsonData);
                }
                else
                    continue;
                pageItems = null;
            }
            customerDt = null;
            customerItems = null;
            jobInfo = null;
        }
        #endregion


        #region ExecuteDataApiCustomerJob   同步客户数据接口Test
        /// <summary>
        /// ExecuteDataApiCustomerJob 同步客户数据接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiCustomerTestJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode;  //Model
            System.Data.DataTable customerDt = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (customerDt == null)
            {
                logMessage = string.Format("【{0}_{1}】  同步客户数据失败！  客户数据为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

            if (customerDt.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.Customer> customerItems = new List<Model.Customer>();

            foreach (System.Data.DataRow dr in customerDt.Rows)
            {
                Model.Customer customerItem = new Model.Customer();
                customerItem.address = Util.DataTableHelper.DataRowContains(dr, "address");
                customerItem.apparatusLicenseCode = Util.DataTableHelper.DataRowContains(dr, "apparatusLicenseCode");
                customerItem.apparatusLicenseValidate = Util.DataTableHelper.DataRowContains(dr, "apparatusLicenseValidate");
                customerItem.area = Util.DataTableHelper.DataRowContains(dr, "area");
                customerItem.bankAccount = Util.DataTableHelper.DataRowContains(dr, "bankAccount");
                customerItem.businessLicenseCode = Util.DataTableHelper.DataRowContains(dr, "businessLicenseCode");
                customerItem.businessLicenseValidate = Util.DataTableHelper.DataRowContains(dr, "businessLicenseValidate");
                customerItem.clientIdcard = Util.DataTableHelper.DataRowContains(dr, "clientIdcard");
                customerItem.clientName = Util.DataTableHelper.DataRowContains(dr, "clientName");
                customerItem.clientPhone = Util.DataTableHelper.DataRowContains(dr, "clientPhone");
                customerItem.clientTimelimit = Util.DataTableHelper.DataRowContains(dr, "clientTimelimit");
                customerItem.companyAddress = Util.DataTableHelper.DataRowContains(dr, "companyAddress");
                customerItem.companyName = Util.DataTableHelper.DataRowContains(dr, "companyName");
                customerItem.companyPrincipal = Util.DataTableHelper.DataRowContains(dr, "companyPrincipal");
                customerItem.consignee = Util.DataTableHelper.DataRowContains(dr, "consignee");
                customerItem.consigneePhone = Util.DataTableHelper.DataRowContains(dr, "consigneePhone");
                customerItem.contactWay = Util.DataTableHelper.DataRowContains(dr, "contactWay");
                customerItem.customerId = Util.DataTableHelper.DataRowContains(dr, "customerId");  //customerId Add 20200722
                customerItem.customerTypeId = Util.DataTableHelper.DataRowContains(dr, "customerTypeId");
                customerItem.depositBank = Util.DataTableHelper.DataRowContains(dr, "depositBank");
                customerItem.depotAddress = Util.DataTableHelper.DataRowContains(dr, "depotAddress");
                customerItem.drugInterceptScope = Util.DataTableHelper.DataRowContains(dr, "drugInterceptScope");
                customerItem.dutyNumber = Util.DataTableHelper.DataRowContains(dr, "dutyNumber");
                customerItem.erpCode = Util.DataTableHelper.DataRowContains(dr, "erpCode");
                customerItem.erpId = Util.DataTableHelper.DataRowContains(dr, "erpId");
                customerItem.erpSalesmanId = Util.DataTableHelper.DataRowContains(dr, "erpSalesmanId");
                customerItem.foodLicenseCode = Util.DataTableHelper.DataRowContains(dr, "foodLicenseCode");
                customerItem.foodLicenseValidate = Util.DataTableHelper.DataRowContains(dr, "foodLicenseValidate");
                customerItem.gspCode = Util.DataTableHelper.DataRowContains(dr, "gspCode");
                customerItem.gspValidate = Util.DataTableHelper.DataRowContains(dr, "gspValidate");
                customerItem.instrumentInterceptScope = Util.DataTableHelper.DataRowContains(dr, "instrumentInterceptScope");
                customerItem.isForProfit = Util.DataTableHelper.DataRowContainsInt(dr, "isForProfit");
                customerItem.isRefrigeration = Util.DataTableHelper.DataRowContainsInt(dr, "isRefrigeration");
                customerItem.legalPerson = Util.DataTableHelper.DataRowContains(dr, "legalPerson");
                customerItem.level = Util.DataTableHelper.DataRowContains(dr, "level");
                customerItem.licenceCode = Util.DataTableHelper.DataRowContains(dr, "licenceCode");
                customerItem.licenceStartDate = Util.DataTableHelper.DataRowContains(dr, "licenceStartDate");
                customerItem.licenceType = Util.DataTableHelper.DataRowContainsInt(dr, "licenceType");
                customerItem.licenceValidate = Util.DataTableHelper.DataRowContains(dr, "licenceValidate");
                customerItem.payType = Util.DataTableHelper.DataRowContains(dr, "payType");
                customerItem.qualityPrincipal = Util.DataTableHelper.DataRowContains(dr, "qualityPrincipal");
                customerItem.registeredCapital = Util.DataTableHelper.DataRowContains(dr, "registeredCapital");
                customerItem.status = Util.DataTableHelper.DataRowContainsInt(dr, "status");
                customerItem.scope = Util.DataTableHelper.DataRowContains(dr, "scope");
                customerItem.specifyQuotation = Util.DataTableHelper.DataRowContains(dr, "specifyQuotation");
                customerItem.twoApparatusLicenseCode = Util.DataTableHelper.DataRowContains(dr, "twoApparatusLicenseCode");
                customerItem.taskId = Util.DataTableHelper.DataRowContainsInt(dr, "taskId");
                customerItems.Add(customerItem);
            }
            if (customerItems != null && customerItems.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  同步客户数据失败！  客户实体为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            int pagesSize = jobInfo.PageSize <= 0 ? customerItems.Count() : jobInfo.PageSize;
            int pagesCount = customerItems.Count();
            int splitCopies = (int)Math.Ceiling((double)pagesCount / pagesSize);
            int pageNum = 0; //页码
            for (int i = 0; i < splitCopies; i++)
            {
                pageNum = i;
                //pageSize ：表示一页多少条。
                //pageNum：表示页数，但是正确的页数是pageNum + 1。因为pageNum = 0，是第一页。pageNum = 1的时候，是第二页。
                //Skip ：表示从第pageNum* pageSize +1条数据开始，也就是说再这之前有pageNum* pageSize条数据。
                //Take：表示显示多少条数据，也就是pageSize条。
                //Take：表示显示多少条数据，也就是pageSize条。
                var pageItems = customerItems.Skip(pageNum * pagesSize).Take(pagesSize).ToList();
                var arr = pageItems.Select(x => x.taskId).ToList();
                string taskIds = string.Format("{0}", string.Join(",", arr));
                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(pageItems);
                string resultJson = string.Empty;
                jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());


                if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
                {
                    Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                    string resultJsonData = resultJObject["data"].ToString();  //resultJObject.Value<string>("data");
                    if (!string.IsNullOrEmpty(resultJsonData) && !string.Equals(resultJsonData, "[]"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1}  {2} 部份失败！ 失败erpId:{3}", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, resultJsonData);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    }

                    string taskIdsReplace = taskIds.Replace(",", string.Empty);
                    if (string.IsNullOrEmpty(taskIdsReplace))
                    {
                        logMessage = string.Format("【{0}_{1}】  {2} 任务id:{3}  更新成功后回写失败; 客户taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, taskIds);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        continue;
                    }
                    ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, resultJsonData);
                }
                else
                    continue;
                pageItems = null;
            }
            customerDt = null;
            customerItems = null;
            jobInfo = null;

        }
        #endregion

        #region ExecuteDataApiCustomerListJob    获取新注册客户数据接口
        /// <summary>
        /// ExecuteDataApiCustomerListJob  获取新注册客户数据接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiCustomerListJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();


            var requestJObject = new Newtonsoft.Json.Linq.JObject();
            requestJObject.Add("dataApiCustomerList", "ExecuteDataApiCustomerListJob");
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(requestJObject);
            string resultJson = string.Empty;

            
            // UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), "1d015867ee5446738d39c48445a05839", string.Empty, 4);
            //return;
            //resultJson = "{\"msg\":\"操作成功\",\"code\":200,\"data\":[{\"customerId\":\"1e24b4d465f645e7b041bf1015fc5e86\",\"customerTypeId\":1,\"companyName\":\"红太阳药店\",\"companyAddress\":\"摩尔城123号1\",\"companyLandline\":\"18911110012\",\"linkName\":\"小太阳1\",\"phone\":\"17502038345\",\"createTime\":\"2020-07-09T14:09:13.000+0800\",\"certificatesList\":[{\"imageType\":1,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/68cd69b026874e6e83206f3adaf8956c.jpg\"},{\"imageType\":2,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/06da3a351cc449978360616a33fd7ac6.jpg\"},{\"imageType\":3,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/1f2fd3984ea440299b99cb4ed7597914.jpg\"},{\"imageType\":4,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/5aac2e933dd3483d97d9c340971c49b9.jpg\"},{\"imageType\":5,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/641a0a0e58504c9fb61661a962d818eb.jpg\"},{\"imageType\":6,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/5ec3b06faf9247d1bc6a5786710476f9.jpg\"},{\"imageType\":7,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/18920a2ad06c45be913ec0de1ff03436.jpg\"},{\"imageType\":17,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/5728224f27fa411ebfc61c995278b3c8.jpg\"},{\"imageType\":18,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/7af70afb61eb4f0e8b7044e1027fab3c.jpg\"},{\"imageType\":19,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/f887f14c2d2e4bf29038bc034bb9ac35.jpg\"}]}]}";
            //resultJson = "{\"msg\":\"操作成功\",\"code\":200,\"data\":[{\"customerId\":\"5306d86b85ab4869aa673932da6651aa\",\"customerTypeId\":0,\"companyName\":\"湖北省武汉市牛科技\",\"companyAddress\":\"湖北省武汉市武昌区\",\"companyLandline\":null,\"linkName\":\"牛科技\",\"phone\":\"17786493669\",\"createTime\":\"2020-07-25T14:24:35.000+0800\",\"certificatesList\":[]},{\"customerId\":\"a1ddb221d3294fe0b56bf87ea9c10455\",\"customerTypeId\":2,\"companyName\":\"格林优药汇\",\"companyAddress\":\"东风大道1号\",\"companyLandline\":\"\",\"linkName\":\"刘洪\",\"phone\":\"13971171880\",\"createTime\":\"2020-07-28T10:26:26.000+0800\",\"certificatesList\":[{\"imageType\":1,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/16962c990007465a9439502b0b67903a.jpg\"},{\"imageType\":2,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/21fbb5cb866d430290478aac53c36e64.jpg\"},{\"imageType\":3,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/09bd41d994134686baeaa4c179340553.jpg\"},{\"imageType\":4,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/988cf69533624d80ab17b1ce0cd9404a.jpg\"},{\"imageType\":5,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/58be8102481442fe87672cf105987d09.jpg\"},{\"imageType\":6,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/3a7b676075784fec9e49718ea97cefc2.jpg\"},{\"imageType\":7,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/5d93a20777954538a99c64dfbd1b8db3.jpg\"},{\"imageType\":8,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/eac8cde0a7fc4941a1604f65d3d32ef5.jpg\"},{\"imageType\":10,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/b3a8de9d54424169ad4dee4e5bd792e7.jpg\"},{\"imageType\":11,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/b6b50ee59c2544f5bc21c465d09dd69e.jpg\"},{\"imageType\":12,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/77e776c1317b4998a828ff4db95db35d.jpg\"},{\"imageType\":13,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/b699e69e2a36435d9c26f3c4e13db00d.jpg\"},{\"imageType\":14,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/7b5c04c29d3a431284aff72ca3facb3f.jpg\"},{\"imageType\":17,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/893ba7b1ce6f4d93964ae9a0a069aae4.jpg\"},{\"imageType\":18,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/119bf0044f9b48e7a4eedf194c932fe3.jpg\"},{\"imageType\":19,\"imageUrl\":\"http://oss.hbglyy.cn/img/certificates/16aa5596244e42e2b793ae8379646345.jpg\"}]},{\"customerId\":\"e81fb2eab30a4164a4b09a9640e62a18\",\"customerTypeId\":0,\"companyName\":\"yu药店02\",\"companyAddress\":\"一二三四五六七八九拾一二三四五六七八九廿一二三四五六七八九叁\",\"companyLandline\":null,\"linkName\":\"喻\",\"phone\":\"18672391726\",\"createTime\":\"2020-07-25T11:50:11.000+0800\",\"certificatesList\":[]}]}";
            //if (!string.IsNullOrEmpty(resultJson))
            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                string resultJsonData = resultJObject["data"].ToString();  //resultJObject.Value<string>("data");
                if (string.IsNullOrEmpty(resultJsonData) || string.Equals(resultJsonData, "[]"))
                {
                    logMessage = string.Format("【{0}_{1}】  {1} 无新注册客户！！！", jobInfo.JobCode, jobInfo.JobName);
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    return;
                }
                Newtonsoft.Json.Linq.JArray orderJArray = Newtonsoft.Json.Linq.JArray.Parse(resultJsonData);
                if (orderJArray.Count() <= 0)
                {
                    logMessage = string.Format("【{0}_{1}】  {1} 新注册客户主表失败！ 返回orderJArray数组为空！！！", jobInfo.JobCode, jobInfo.JobName);
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    return;
                }
                foreach (var item in orderJArray)
                {
                    string companyName = item["companyName"].ToString();
                    string customerId = item["customerId"].ToString();
                    //UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                    //continue;
                    Newtonsoft.Json.Linq.JObject itemJObject = (Newtonsoft.Json.Linq.JObject)item;
                    string strCertificatesList = itemJObject["certificatesList"].ToString();
                    Newtonsoft.Json.Linq.JArray jArrayCertificatesList = new Newtonsoft.Json.Linq.JArray();
                    if (string.IsNullOrEmpty(strCertificatesList) || string.Equals(strCertificatesList, "[]"))
                    {
                        logMessage = string.Format("【{0}_{1}】  客户名称:{2} ;客户Id:{3}   证照图片资料失败！ 返回certificatesList数组为空！！！", jobInfo.JobCode, jobInfo.JobName, companyName, customerId);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        itemJObject.Remove("certificatesList");
                        //continue;
                    }
                    else
                    {
                        jArrayCertificatesList = Newtonsoft.Json.Linq.JArray.Parse(strCertificatesList); //(Newtonsoft.Json.Linq.JArray)item["certificatesList"];
                        itemJObject.Remove("certificatesList");
                    }
                    string itemJObjectJson = itemJObject.ToString();
                    Newtonsoft.Json.Linq.JArray jArrayOrder = new Newtonsoft.Json.Linq.JArray();//Util.NewtonsoftCommon.ConvertJsonToJArray(logAppendToForms, jobInfo, itemJObjectJson);
                    jArrayOrder.Add(itemJObject);
                    System.Data.DataTable dataTable = Util.NewtonsoftCommon.ConvertJArrayToDataTable(logAppendToForms, jobInfo, jArrayOrder);
                    if (dataTable == null || dataTable.Rows.Count <= 0)
                    {
                        logMessage = string.Format("【{0}_{1}】  客户名称:{2} ;客户Id:{3}  转换DataTable失败！！！", jobInfo.JobCode, jobInfo.JobName, companyName, customerId);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                        continue;
                    }
                    if (!dataTable.Columns.Contains("updated"))
                        dataTable.Columns.Add("updated", typeof(string));
                    string updated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    foreach (System.Data.DataRow itemsDr in dataTable.Rows)
                    {
                        itemsDr["customerId"] = customerId;
                        itemsDr["updated"] = updated;
                    }
                    string insertTableName = string.Empty;
                    if (string.IsNullOrEmpty(jobInfo.InsertTableName))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1}失败 未定义新注册客户表！！！", jobInfo.JobCode, jobInfo.JobName);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                        continue;
                    }
                    string[] arr = jobInfo.InsertTableName.Split(',');
                    if (arr.Count() > 0)
                        insertTableName = arr[0].ToString();
                    if (string.IsNullOrEmpty(insertTableName))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1}失败 未定义新注册客户主表！！！", jobInfo.JobCode, jobInfo.JobName);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                        continue;
                    }
                    int dbr = BulkInsertDatabaseInt(logAppendToForms, jobInfo, dataTable, insertTableName);
                    if (dbr > 0)
                    {
                        if (int.Equals(dbr, 2))
                        {
                            string strSql = CustomerDataTableToStrUpdate(dataTable, insertTableName, customerId);
                            if (!BulkInsertDatabase(logAppendToForms, jobInfo, insertTableName, strSql))
                            {
                                UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                                continue;
                            }
                        }
                        if (jArrayCertificatesList.Count() <= 0)
                        {
                            logMessage = string.Format("【{0}_{1}】  客户名称:{2} ;客户Id:{3}   证照图片资料失败！ 返回certificatesList数组为空2！！！", jobInfo.JobCode, jobInfo.JobName, companyName, customerId);
                            LogError(logAppendToForms, true, logMessage, jobLogType);
                            continue;
                        }
                        System.Data.DataTable itemsDt = Util.NewtonsoftCommon.ConvertJArrayToDataTable(logAppendToForms, jobInfo, jArrayCertificatesList);
                        if (itemsDt == null || itemsDt.Rows.Count <= 0)
                        {
                            logMessage = string.Format("【{0}_{1}】  客户名称:{2} ;客户Id:{3}   证照图片资料转换DataTable失败！！！", jobInfo.JobCode, jobInfo.JobName, companyName, customerId);
                            LogError(logAppendToForms, true, logMessage, jobLogType);
                            UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                            continue;
                        }
                        //加字段
                        if (!itemsDt.Columns.Contains("customerId"))
                            itemsDt.Columns.Add("customerId", typeof(string));
                        if (!itemsDt.Columns.Contains("updated"))
                            itemsDt.Columns.Add("updated", typeof(string));
                        foreach (System.Data.DataRow itemsDr in itemsDt.Rows)
                        {
                            itemsDr["customerId"] = customerId;
                            itemsDr["updated"] = updated;
                        }
                        insertTableName = string.Empty;
                        if (arr.Count() > 1)
                            insertTableName = arr[1].ToString();
                        if (string.IsNullOrEmpty(insertTableName))
                        {
                            logMessage = string.Format("【{0}_{1}】  {1} 新注册客户证照图片资料失败 未定义新注册客户证照图片资料表！！！", jobInfo.JobCode, jobInfo.JobName);
                            LogError(logAppendToForms, true, logMessage, jobLogType);
                            UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                            continue;
                        }
                        string itemsSql = DataTableToStrDeleteAndInsert(itemsDt, insertTableName);
                        if (!BulkInsertDatabase(logAppendToForms, jobInfo, insertTableName, itemsSql))
                        {
                            UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                            continue;
                        }

                        //下载图片
                        //OpenUrlDownloadFile(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, string url, string filename)
                        /*foreach (System.Data.DataRow dr in itemsDt.Rows)
                        {
                            string url = dr["customerId"].ToString();

                        }*/
                        //新注册客户插入成功后回写Erp  自动生成首营申请单  ？？

                        //ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), customerId, string.Empty);

                    }
                    else
                    {
                        //插入数据库失败 
                        UpdataDataApiCustomerStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiCustomerStatus", "同步客户状态接口"), customerId, string.Empty, 4);
                        continue;
                    }
                }
            }
        }
        #endregion

        #region ExecuteDataApiCustomerStatusJob   获取新注册客户失败后  更新新注册客户状态接口
        /// <summary>
        /// ExecuteDataApiCustomerStatusJob   获取新注册客户失败后  更新新注册客户状态接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiCustomerStatusJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null )
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if ( dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.CustomerStatus> items = new List<Model.CustomerStatus>();


            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.CustomerStatus item = new Model.CustomerStatus();
                item.auditStatus = Util.DataTableHelper.DataRowContainsInt(dr, "auditStatus");
                item.customerId = Util.DataTableHelper.DataRowContains(dr, "customerId");
                //原因？？？？
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;

            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                string taskIds = Util.DataTableHelper.GetColumnValuesInt(dataTable, "taskId");
                string taskIdsReplace = taskIds.Replace(",", string.Empty);
                if (string.IsNullOrEmpty(taskIdsReplace))
                {
                    logMessage = string.Format("【{0}_{1}】  任务id:{2} 更新成功后回写失败，taskId为空！！！", jobInfo.JobCode, jobInfo.JobName,taskIds);
                    LogError(logAppendToForms, true, logMessage, jobLogType);
                    return;
                }
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, string.Empty);
            }
            else
                return;

            dataTable = null;
            items = null;
            jobInfo = null;
        }
        #endregion

        #region ExecuteDataApiOrderListJob    获取订单数据接口
        /// <summary>
        /// ExecuteDataApiOrderListJob  获取订单数据接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiOrderListJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();


            var requestJObject = new Newtonsoft.Json.Linq.JObject();
            requestJObject.Add("dataApiOrderList", "ExecuteDataApiOrderListJob");
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(requestJObject);
            string resultJson = string.Empty;
            //UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), "47322ff17670423ab07c5191f3e09ba2", 3);//回到已支付状态
            //UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), "f26d245508b246bbb3d9ebaac21e0eb3", 3);//回到已支付状态
            //return;
            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                string resultJsonData = resultJObject["data"].ToString();  //resultJObject.Value<string>("data");
                if (string.IsNullOrEmpty(resultJsonData) || string.Equals(resultJsonData, "[]"))
                {
                    logMessage = string.Format("【{0}_{1}】  {1} 无新订单！！！", jobInfo.JobCode, jobInfo.JobName);
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    return;
                }
                Newtonsoft.Json.Linq.JArray orderJArray = Newtonsoft.Json.Linq.JArray.Parse(resultJsonData);
                foreach (var item in orderJArray)
                {

                    string orderId = item["orderId"].ToString();
                    string orderCode = item["orderCode"].ToString();  //Util.NewGuid.GetIdentityGeneratorToString(jobInfo.ConfigInfo.OrderCodePrefix);//Util.NewGuid.GetSnowflakeIdWorkerToString();
                    Newtonsoft.Json.Linq.JObject itemJObject = (Newtonsoft.Json.Linq.JObject)item;

                    string strOrderCommodityList = itemJObject["orderCommodityList"].ToString();
                    Newtonsoft.Json.Linq.JArray jArrayOrderCommodityList = new Newtonsoft.Json.Linq.JArray();
                    if (string.IsNullOrEmpty(strOrderCommodityList) || string.Equals(strOrderCommodityList, "[]"))
                    {
                        logMessage = string.Format("【{0}_{1}】  订单Id:{2} ; 返回 OrderCommodityList 明细数组为空！！！", jobInfo.JobCode, jobInfo.JobName, orderId);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        itemJObject.Remove("orderCommodityList");
                        UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId, 3);//回到已支付状态
                        continue;
                    }
                    else
                    {
                        jArrayOrderCommodityList = Newtonsoft.Json.Linq.JArray.Parse(strOrderCommodityList); //(Newtonsoft.Json.Linq.JArray)item["certificatesList"];
                        itemJObject.Remove("orderCommodityList");
                    }
                    string itemJObjectJson = itemJObject.ToString();
                    Newtonsoft.Json.Linq.JArray jArrayOrder = new Newtonsoft.Json.Linq.JArray();
                    jArrayOrder.Add(itemJObject);

                    System.Data.DataTable dataTable = Util.NewtonsoftCommon.ConvertJArrayToDataTable(logAppendToForms, jobInfo, jArrayOrder);
                    if (dataTable == null || dataTable.Rows.Count <= 0)
                    {
                        logMessage = string.Format("【{0}_{1}】  订单Id:{2} ; 订单主表失败！ 订单主表数据为空！！！", jobInfo.JobCode, jobInfo.JobName, orderId);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        continue;
                    }
                   
                    var dtOrderMainResult = UpdateOrderMain(logAppendToForms, jobInfo,dataTable, orderCode, orderId);
                    if (dtOrderMainResult == null || dtOrderMainResult.Rows.Count <= 0)
                    {
                        logMessage = string.Format("【{0}_{1}】  订单Id:{2} ; 订单主表字段转换类型后数据为空！！！", jobInfo.JobCode, jobInfo.JobName, orderId);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId, 3);//回到已支付状态
                        continue;
                    }
                    string insertTableName = string.Empty;
                    if (string.IsNullOrEmpty(jobInfo.InsertTableName))
                    {
                        logMessage = string.Format("【{0}_{1}】   订单Id:{2} ; 未定义 订单主表！！！", jobInfo.JobCode, jobInfo.JobName, orderId);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId, 3);//回到已支付状态
                        continue;
                    }
                    string[] arr = jobInfo.InsertTableName.Split(',');
                    if (arr.Count() > 0)
                        insertTableName = arr[0].ToString();
                    if(string.IsNullOrEmpty(insertTableName))
                    {
                        logMessage = string.Format("【{0}_{1}】  订单Id:{2} ; 未定义订单主表！！！", jobInfo.JobCode, jobInfo.JobName, orderId);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId, 3);//回到已支付状态
                        continue;
                    }
                    if (BulkInsertDatabase(logAppendToForms, jobInfo, dtOrderMainResult, insertTableName))
                    {
                        if (jArrayOrderCommodityList.Count() <= 0)
                        {
                            logMessage = string.Format("【{0}_{1}】  订单Id:{2}; 返回 OrderCommodityList 订单明细数组为空2！！！", jobInfo.JobCode, jobInfo.JobName, orderId);
                            LogError(logAppendToForms, true, logMessage, jobLogType);
                            OrderToStrDelete(logAppendToForms, jobInfo, insertTableName, orderId);//删除主表？？？
                            UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId, 3);//回到已支付状态
                            continue;
                        }
                        System.Data.DataTable itemsDt = Util.NewtonsoftCommon.ConvertJArrayToDataTable(logAppendToForms, jobInfo, jArrayOrderCommodityList);
                        if (itemsDt == null || itemsDt.Rows.Count <= 0)
                        {
                            logMessage = string.Format("【{0}_{1}】  订单Id:{2}; 订单商品明细失败！ 订单商品明细数据为空！！！", jobInfo.JobCode, jobInfo.JobName, orderId);
                            LogWarning(logAppendToForms, true, logMessage, jobLogType);
                            OrderToStrDelete(logAppendToForms, jobInfo, insertTableName, orderId);//删除主表？？？
                            UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId, 3);//回到已支付状态
                            continue;
                        }
                        var dtOrderListResult = UpdateOrderList(logAppendToForms, jobInfo, itemsDt, orderCode, orderId);
                        if (dtOrderListResult == null || dtOrderListResult.Rows.Count <= 0)
                        {
                            logMessage = string.Format("【{0}_{1}】  订单Id:{2} ; 订单明细表字段转换类型后数据为空！！！", jobInfo.JobCode, jobInfo.JobName, orderId);
                            LogError(logAppendToForms, true, logMessage, jobLogType);
                            UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId, 3);//回到已支付状态
                            continue;
                        }
                       
                        string insertListTableName = string.Empty;
                        if (arr.Count() > 1)
                            insertListTableName = arr[1].ToString();
                        if (string.IsNullOrEmpty(insertListTableName))
                        {
                            logMessage = string.Format("【{0}_{1}】  订单Id:{2}; 订单商品明细失失败 未定义订单明细表！！！", jobInfo.JobCode, jobInfo.JobName,orderId);
                            LogWarning(logAppendToForms, true, logMessage, jobLogType);
                            OrderToStrDelete(logAppendToForms, jobInfo, insertTableName, orderId);////删除主表？？？
                            UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId, 3);//回到已支付状态
                            continue;
                        }

                        if (!BulkInsertDatabase(logAppendToForms, jobInfo, dtOrderListResult, insertListTableName))
                        {
                            //明细表插入数据失败
                            OrderToStrDelete(logAppendToForms, jobInfo, insertTableName, orderId);////删除主表？？？
                            UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId, 3);//回到已支付状态
                            continue;
                        }
                        else
                        {
                            //判断有无SSP打头  dtOrderListResult
                            var saleOutDetailsDt = Util.DataTableHelper.GetNewDataTable(dtOrderListResult, "erpGoodsIdlocal like 'SSP%' ");
                            if (dtOrderListResult != null && dtOrderListResult.Rows.Count > 0)
                            {
                                //处理秒杀等
                                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), "1", orderCode);
                            }
                        }
                    }
                    else
                    {
                        //主表插入数据库失败
                        UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId, 3);//回到已支付状态
                        continue;
                    }
                
                }

            }
            //else
              //  return;

            jobInfo = null;

        }
        #endregion

        #region ExecuteDataApiOrderStatusJob    同步订单状态接口
        /// <summary>
        /// ExecuteDataApiOrderStatusJob  同步订单状态接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiOrderStatusJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null )
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.OrderListStatus> items = new List<Model.OrderListStatus>();

            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.OrderListStatus item = new Model.OrderListStatus();
                item.orderId = Util.DataTableHelper.DataRowContains(dr, "orderId"); //订单Id
                item.orderStatus = Util.DataTableHelper.DataRowContainsInt(dr, "orderStatus"); // 认证状态 3支付完成 6拣货中 7商品已出库
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;

            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                string taskIds = Util.DataTableHelper.GetColumnValuesInt(dataTable, "taskId");
                string taskIdsReplace = taskIds.Replace(",", string.Empty);
                if (string.IsNullOrEmpty(taskIdsReplace))
                {
                    logMessage = string.Format("【{0}_{1}】  任务Id{2}失败！  更新成功后回写 taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, taskIds);
                    LogError(logAppendToForms, true, logMessage, jobLogType);
                    return;
                }
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, string.Empty);
            }
            else
                return;

        }
        #endregion

        #region ExecuteDataApiOrderOutWarehouseJob    同步订单出库信息接口
        /// <summary>
        /// ExecuteDataApiOrderOutWarehouseJob  同步订单出库信息接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiOrderOutWarehouseJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null )
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string erpOutboundIds = Util.DataTableHelper.GetColumnValues(dataTable, "erpOutboundId");
            string[] arr = erpOutboundIds.Split(',');
            if (arr.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 出库数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            for (int i = 0; i < arr.Count(); i++)
            {
                string erpOutboundId = arr[0].ToString();
                System.Data.DataTable erpOutboundDt = Util.DataTableHelper.GetNewDataTable(dataTable, "erpOutboundId='" + erpOutboundId + "' ");
                if (erpOutboundDt == null || erpOutboundDt.Rows.Count <= 0)
                {
                    logMessage = string.Format("【{0}_{1}】  出库单号{2} {1}失败！ b2b订单数据为空！！！", jobInfo.JobCode, jobInfo.JobName, erpOutboundId);
                    LogError(logAppendToForms, true, logMessage, jobLogType);
                    continue;
                }
                string orderId = Util.DataTableHelper.DateTableContains(erpOutboundDt,0, "orderId");
                List<Model.OrderOutWarehouse> items = new List<Model.OrderOutWarehouse>();
                foreach (System.Data.DataRow dr in erpOutboundDt.Rows)
                {
                    Model.OrderOutWarehouse item = new Model.OrderOutWarehouse();
                    item.batchCode = Util.DataTableHelper.DataRowContains(dr, "batchCode"); //批号
                    item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId"); //erp商品id
                    item.erpOutboundId = Util.DataTableHelper.DataRowContains(dr, "erpOutboundId"); //出库ID
                    item.erpOutboundItemId = Util.DataTableHelper.DataRowContains(dr, "erpOutboundItemId"); //出库明细序号
                    item.orderId = Util.DataTableHelper.DataRowContains(dr, "orderId"); //订单id
                    item.orderItemId = Util.DataTableHelper.DataRowContains(dr, "orderItemId"); //订单明细id
                    item.outboundDate = Util.DataTableHelper.DataRowContains(dr, "outboundDate"); //出库日期
                    item.outboundQuantity = Util.DataTableHelper.DataRowContains(dr, "outboundQuantity"); //出库数量
                    item.outboundTime = Util.DataTableHelper.DataRowContains(dr, "outboundTime"); //出库时间
                    item.productionDate = Util.DataTableHelper.DataRowContains(dr, "productionDate"); //生产日期
                    item.valDate = Util.DataTableHelper.DataRowContains(dr, "valDate"); //有效期
                    item.salesmanAccountingCostPrice = Util.DataTableHelper.DataRowContains(dr, "salesmanAccountingCostPrice"); //有效期//业务员核算成本价  ？？？？
                    item.salesmanGrossProfit = Util.DataTableHelper.DataRowContains(dr, "salesmanGrossProfit"); //有效期//业务员毛利？？？？？
                    items.Add(item);
                }
                if (items != null && items.Count() <= 0)
                {
                    logMessage = string.Format("【{0}_{1}】 出库单号{2}  {1}失败！  出库实体为空！！！", jobInfo.JobCode, jobInfo.JobName, erpOutboundId);
                    LogError(logAppendToForms, true, logMessage, jobLogType);
                    continue;
                }
                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
                string resultJson = string.Empty;

                if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
                {
                    string taskIds = Util.DataTableHelper.GetColumnValuesInt(erpOutboundDt, "taskId");
                    string taskIdsReplace = taskIds.Replace(",", string.Empty);
                    if (string.IsNullOrEmpty(taskIdsReplace))
                    {
                        logMessage = string.Format("【{0}_{1}】  任务Id{2}失败！  更新成功后回写 taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, taskIds);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        continue;
                    }

                    ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, erpOutboundId);
                    //string erpOutboundIds = Util.DataTableHelper.GetColumnValues(dataTable, "erpOutboundI");
                    //订单是否已完成
                    string isOrderCompleted = ExecuteScalar(logAppendToForms, jobInfo, "DataApiOrderOutWarehousCompleted", erpOutboundId, orderId);
                    if(string.Equals(isOrderCompleted,"Y"))
                    {
                        UpdateDataApiOrderOutWarehouseRefund(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderOutWarehouseRefund", "同步订单出库差异退款接口"), orderId, erpOutboundIds); //上传出库差异退款
                        UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId,7);  //B2B订单出库单已全部上传完毕
                    }
                    
                  
                }
                else
                    continue;
            }

        }
        #endregion

        #region ExecuteDataApiOrderOutWarehouseRefundJob    同步订单出库差异退款接口
        /// <summary>
        /// ExecuteDataApiOrderOutWarehouseRefundJob  同步订单出库差异退款接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiOrderOutWarehouseRefundJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null )
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string orderIds = Util.DataTableHelper.GetColumnValues(dataTable, "orderId");
            string[] arr = orderIds.Split(',');
            if (arr.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            for (int i = 0; i < arr.Count(); i++)
            {
                string orderId = arr[0].ToString();
                System.Data.DataTable orderOutWarehouseRefundDt = Util.DataTableHelper.GetNewDataTable(dataTable, "orderId='" + orderId + "' ");
                if (orderOutWarehouseRefundDt == null || orderOutWarehouseRefundDt.Rows.Count <= 0)
                {
                    logMessage = string.Format("【{0}_{1}】  订单Id{2} {1}失败！ b2b订单数据为空！！！", jobInfo.JobCode, jobInfo.JobName, orderId);
                    LogError(logAppendToForms, true, logMessage, jobLogType);
                    continue;
                }
                //string orderId = Util.DataTableHelper.DateTableContains(erpOutboundDt, 0, "orderId");

                List<Model.OrderOutWarehouseRefund> items = new List<Model.OrderOutWarehouseRefund>();

                foreach (System.Data.DataRow dr in orderOutWarehouseRefundDt.Rows)
                {
                    Model.OrderOutWarehouseRefund item = new Model.OrderOutWarehouseRefund();
                    item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId"); //erp商品id
                    item.erpOutboundId = Util.DataTableHelper.DataRowContains(dr, "erpOutboundId");
                    item.orderId = Util.DataTableHelper.DataRowContains(dr, "orderId"); //订单id
                    item.orderItemId = Util.DataTableHelper.DataRowContains(dr, "orderItemId"); //订单明细id
                    item.refundNum = Util.DataTableHelper.DataRowContainsInt(dr, "refundNum"); //退款数量
                    items.Add(item);
                }
                if (items != null && items.Count() <= 0)
                {
                    logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                    LogError(logAppendToForms, true, logMessage, jobLogType);
                    continue;
                }
                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
                string resultJson = string.Empty;

                if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
                {
                    UpdateDataApiOrderStatus(logAppendToForms, GetJobEntity(jobInfo, "DataApiOrderStatus", "同步订单状态接口"), orderId, 7);
                    string taskIds = Util.DataTableHelper.GetColumnValuesInt(orderOutWarehouseRefundDt, "taskId");
                    string taskIdsReplace = taskIds.Replace(",", string.Empty);
                    if (string.IsNullOrEmpty(taskIdsReplace))
                    {
                        logMessage = string.Format("【{0}_{1}】  任务Id{2}失败！  更新成功后回写 taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, taskIds);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        continue;
                    }
                    ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, string.Empty);
                }
                else
                    continue;
            }

            jobInfo = null;
        }
        #endregion
        
        #region ExecuteDataApiOrderExpressJob    同步订单物流单号接口
        /// <summary>
        /// ExecuteDataApiOrderExpressJob  同步订单物流单号接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiOrderExpressJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null )
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.OrderExpress> items = new List<Model.OrderExpress>();

            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.OrderExpress item = new Model.OrderExpress();
                item.expressCompany = Util.DataTableHelper.DataRowContains(dr, "expressCompany"); 
                item.expressNo = Util.DataTableHelper.DataRowContains(dr, "expressNo"); //物流单号
                item.orderId = Util.DataTableHelper.DataRowContains(dr, "orderId"); //订单id
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;

            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                string taskIds = Util.DataTableHelper.GetColumnValuesInt(dataTable, "taskId");
                string taskIdsReplace = taskIds.Replace(",", string.Empty);
                if (string.IsNullOrEmpty(taskIdsReplace))
                {
                    logMessage = string.Format("【{0}_{1}】  任务Id{2}失败！  更新成功后回写 taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, taskIds);
                    LogError(logAppendToForms, true, logMessage, jobLogType);
                    return;
                }
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, string.Empty);
            }
            //else
            //  return;
            jobInfo = null;
            dataTable = null;
        }
        #endregion

        #region ExecuteDataApiOrderInvoiceJob    同步订单发票接口
        /// <summary>
        /// ExecuteDataApiOrderInvoiceJob  同步订单发票接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiOrderInvoiceJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null )
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.OrderInvoice> items = new List<Model.OrderInvoice>();

            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.OrderInvoice item = new Model.OrderInvoice();
                item.amount = Util.DataTableHelper.DataRowContains(dr, "amount"); //发票金额
                item.fileUrl = Util.DataTableHelper.DataRowContains(dr, "fileUrl"); //发票链接地址
                item.invoiceCode = Util.DataTableHelper.DataRowContains(dr, "invoiceCode"); //发票代码
                item.invoiceNumber = Util.DataTableHelper.DataRowContains(dr, "invoiceNumber"); //发票号
                item.invoiceType = Util.DataTableHelper.DataRowContains(dr, "invoiceType"); //发票类型 1普票 2专票
                item.orderId = Util.DataTableHelper.DataRowContains(dr, "orderId"); //订单id
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;

            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                string taskIds = Util.DataTableHelper.GetColumnValuesInt(dataTable, "taskId");
                string taskIdsReplace = taskIds.Replace(",", string.Empty);
                if (string.IsNullOrEmpty(taskIdsReplace))
                {
                    logMessage = string.Format("【{0}_{1}】  任务Id{2}  更新成功后回写 taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, taskIds);
                    LogError(logAppendToForms, true, logMessage, jobLogType);
                    return;
                }
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, string.Empty);
            }
            //else
            //    return;
            jobInfo = null;
            dataTable = null;

        }
        #endregion

        #region ExecuteDataApiOrderQualityInspectionImageJob    上传订单质检图片数据接口
        /// <summary>
        /// ExecuteDataApiOrderQualityInspectionImageJob  上传订单质检图片数据接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiOrderQualityInspectionImageJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null )
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.OrderInvoice> items = new List<Model.OrderInvoice>();

            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.OrderInvoice item = new Model.OrderInvoice();
                item.amount = Util.DataTableHelper.DataRowContains(dr, "amount"); //发票金额
                item.fileUrl = Util.DataTableHelper.DataRowContains(dr, "fileUrl"); //发票链接地址
                item.invoiceCode = Util.DataTableHelper.DataRowContains(dr, "invoiceCode"); //发票代码
                item.invoiceNumber = Util.DataTableHelper.DataRowContains(dr, "invoiceNumber"); //发票号
                item.invoiceType = Util.DataTableHelper.DataRowContains(dr, "invoiceType"); //发票类型 1普票 2专票
                item.orderId = Util.DataTableHelper.DataRowContains(dr, "orderId"); //订单id
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;

            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                string orderIds = Util.DataTableHelper.GetColumnValues(dataTable, "orderId");
                //string writebackInfo = resultJson;
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), orderIds, string.Empty);
            }
            else
                return;

        }
        #endregion
        
        #region ExecuteDataApiSalesmanJob   同步业务员数据接口
        /// <summary>
        /// ExecuteDataApiSalesmanJob 同步业务员数据接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiSalesmanJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode;  //Model
            System.Data.DataTable salesmanDt = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (salesmanDt == null )
            {
                logMessage = string.Format("【{0}_{1}】  同步业务员数据失败！  业务员数据为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (salesmanDt.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.Salesman> salesmanItems = new List<Model.Salesman>();

            foreach (System.Data.DataRow dr in salesmanDt.Rows)
            {
                Model.Salesman salesmanItem = new Model.Salesman();
                salesmanItem.erpId = Util.DataTableHelper.DataRowContains(dr, "erpId");
                salesmanItem.salesmanDept = Util.DataTableHelper.DataRowContains(dr, "salesmanDept");
                salesmanItem.salesmanName = Util.DataTableHelper.DataRowContains(dr, "salesmanName");
                salesmanItem.salesmanPhone = Util.DataTableHelper.DataRowContains(dr, "salesmanPhone");
                salesmanItem.salesmanPost = Util.DataTableHelper.DataRowContains(dr, "salesmanPost");
                salesmanItem.status = Util.DataTableHelper.DataRowContainsInt(dr, "status");  //status
                salesmanItem.taskId = Util.DataTableHelper.DataRowContainsInt(dr, "taskId");  //status

                salesmanItems.Add(salesmanItem);
            }
            if (salesmanItems != null && salesmanItems.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  同步业务员数据失败！  业务员实体为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            int pagesSize = jobInfo.PageSize <= 0 ? salesmanItems.Count() : jobInfo.PageSize;
            int pagesCount = salesmanItems.Count();
            int splitCopies = (int)Math.Ceiling((double)pagesCount / pagesSize);
            int pageNum = 0; //页码
            for (int i = 0; i < splitCopies; i++)
            {
                pageNum = i;
                //pageSize ：表示一页多少条。
                //pageNum：表示页数，但是正确的页数是pageNum + 1。因为pageNum = 0，是第一页。pageNum = 1的时候，是第二页。
                //Skip ：表示从第pageNum* pageSize +1条数据开始，也就是说再这之前有pageNum* pageSize条数据。
                //Take：表示显示多少条数据，也就是pageSize条。
                //Take：表示显示多少条数据，也就是pageSize条。
                var pageItems = salesmanItems.Skip(pageNum * pagesSize).Take(pagesSize).ToList();
                var arr = pageItems.Select(x => x.taskId).ToList();
                string taskIds = string.Format("{0}", string.Join(",", arr));
                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(pageItems);
                string resultJson = string.Empty;
                jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());


                if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
                {
                    Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                    string resultJsonData = resultJObject["data"].ToString();  //resultJObject.Value<string>("data");
                    if (!string.IsNullOrEmpty(resultJsonData) && !string.Equals(resultJsonData, "[]"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} {2} 部份失败！ 失败erpId:{3}", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, resultJsonData);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    }

                    //string taskIds = Util.DataTableHelper.GetColumnValuesInt(salesmanDt, "taskId");
                    string taskIdsReplace = taskIds.Replace(",", string.Empty);
                    if (string.IsNullOrEmpty(taskIdsReplace))
                    {
                        logMessage = string.Format("【{0}_{1}】 {2} 任务Id{3}   更新成功后回写 taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, taskIds);
                        LogError(logAppendToForms, true, logMessage, jobLogType);
                        continue;
                    }
                    ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, resultJsonData);
                }
                else
                    continue;
            }
            jobInfo = null;
            salesmanDt = null;
        }
        #endregion

        #region ExecuteDataApiGoodsSpikeJob    商品秒杀 特价 买赠
        /// <summary>
        /// ExecuteDataApiGoodsSpikeJob  商品秒杀 特价 买赠
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteDataApiGoodsSpikeJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();

            //更新库存
            UpdataGoodsSpikeCommodityRepertoryAsync(logAppendToForms, jobInfo);

            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo);
            if (dataTable == null)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.OrderInvoice> items = new List<Model.OrderInvoice>();

            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.GoodsSpike item = new Model.GoodsSpike();
                item.startDate = Util.DataTableHelper.DataRowContains(dr, "startDate"); //
                item.startTime = Util.DataTableHelper.DataRowContains(dr, "startTime"); //
                item.endDate = Util.DataTableHelper.DataRowContains(dr, "endDate"); //
                item.endTime = Util.DataTableHelper.DataRowContains(dr, "endTime"); //
                item.spikeGoodsId = Util.DataTableHelper.DataRowContains(dr, "spikeGoodsId"); //
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId"); //
                item.activityNum = Util.DataTableHelper.DataRowContainsInt(dr, "activityNum"); //
                item.spikePrice = Math.Round(Util.DataTableHelper.DataRowContainsDecimal(dr, "spikePrice"), 2, MidpointRounding.AwayFromZero).ToString("F2");// Util.DataTableHelper.DataRowContains(dr, "spikePrice"); //
                item.spikeType = Util.DataTableHelper.DataRowContainsInt(dr, "spikeType"); //
                item.subtitle = Util.DataTableHelper.DataRowContains(dr, "spikePrice"); //
                item.spikelimitMax = Util.DataTableHelper.DataRowContainsInt(dr, "spikelimitMax"); //
                string originalPrice = Util.DataTableHelper.DataRowContains(dr, "originalPrice"); //
                decimal originalPriceDec = Util.ConvertHelper.ConvertStringToDecimal(originalPrice);

                if(originalPriceDec<=0)
                {
                    originalPrice = "99999";
                }
                item.originalPrice = originalPrice;// Util.DataTableHelper.DataRowContains(dr, "originalPrice"); //
                string isAdd = Util.DataTableHelper.DataRowContains(dr, "isAdd"); //
                string isStart = Util.DataTableHelper.DataRowContains(dr, "isStart"); //
                string isEnd = Util.DataTableHelper.DataRowContains(dr, "isEnd"); //
                int updateType = Util.DataTableHelper.DataRowContainsInt(dr, "updateType"); //
                string taskId = Util.DataTableHelper.DataRowContains(dr, "taskId"); //
                if(string.Equals(isAdd,"N")) //新增商品资料
                {
                    if (int.Equals(updateType, 1))
                    {
                        //更新商品资料
                        UpdataGoodsSpikeCommodity(logAppendToForms, jobInfo, item, item.spikeGoodsId, taskId, "isAdd");
                        //更新图片
                        UpdateGoodsSpikeCommodityImageJob(logAppendToForms, jobInfo, item.spikeGoodsId, taskId, "isAdd");
                    }
                    else if (int.Equals(updateType, 2)) //更新库存
                    {
                        //活动未开始更新库存为0 
                        UpdataGoodsSpikeCommodityRepertory(logAppendToForms, jobInfo, item.spikeGoodsId, item.activityNum, taskId, "isAdd");
                    }

                    else if (int.Equals(updateType, 3)) //更新价格
                    {
                        //UpdataGoodsSpikeCommodityPriceJob(logAppendToForms, jobInfo, item.spikeGoodsId, "99999.99",99999, taskId, "isAdd");
                        UpdataGoodsSpikeCommodityPriceJob(logAppendToForms, jobInfo, item.spikeGoodsId, item.originalPrice, 99999, taskId, "isAdd");
                        if (ComparisonDate(logAppendToForms, jobInfo, string.Format("{0} {1}", item.startDate, item.startTime)) == 0)
                        {
                            //更新价格
                            UpdataGoodsSpikeCommodityPriceJob(logAppendToForms, jobInfo, item.spikeGoodsId, item.spikePrice, item.spikelimitMax, taskId, "isStart");
                            UpdataGoodsSpikeCommodity(logAppendToForms, jobInfo, item, item.spikeGoodsId, taskId, "isStart");
                            UpdataGoodsSpikeCommodityRepertory(logAppendToForms, jobInfo, item.spikeGoodsId, item.activityNum, taskId, "isStart");

                        }
                       
                    }
                }
                else if (string.Equals(isAdd, "Y"))
                {
                    if (string.Equals(isStart,"N"))
                    {
                        //任务开始
                        if (ComparisonDate(logAppendToForms, jobInfo, string.Format("{0} {1}", item.startDate, item.startTime)) == 0) 
                        {
                            if (int.Equals(updateType, 1))
                            {
                                //更新商品资料
                                UpdataGoodsSpikeCommodity(logAppendToForms, jobInfo, item,item.spikeGoodsId, taskId, "isStart");
                                //更新图片
                                UpdateGoodsSpikeCommodityImageJob(logAppendToForms, jobInfo, item.spikeGoodsId, taskId, "isStart");
                            }
                            else if (int.Equals(updateType, 2)) //更新库存
                            {
                                //活动开始更新库存 
                                UpdataGoodsSpikeCommodityRepertory(logAppendToForms, jobInfo, item.spikeGoodsId, item.activityNum, taskId, "isStart");
                            }
                            else if (int.Equals(updateType, 3)) 
                            {
                                //更新价格
                                UpdataGoodsSpikeCommodityPriceJob(logAppendToForms, jobInfo, item.spikeGoodsId, item.spikePrice, item.spikelimitMax, taskId, "isStart");
                            }
                        }
                        //任务未开始
                        else if (ComparisonDate(logAppendToForms, jobInfo, string.Format("{0} {1}", item.startDate, item.startTime, taskId)) == 1)
                        {
                            if (int.Equals(updateType, 1))
                            {
                                //更新商品资料
                                UpdataGoodsSpikeCommodity(logAppendToForms, jobInfo, item, item.spikeGoodsId, taskId, "isAdd");
                                //更新图片
                                UpdateGoodsSpikeCommodityImageJob(logAppendToForms, jobInfo, item.spikeGoodsId, taskId, "isAdd");
                            }
                            else if (int.Equals(updateType, 2)) //更新库存
                            {
                                //活动未开始更新库存为0 
                                UpdataGoodsSpikeCommodityRepertory(logAppendToForms, jobInfo, item.spikeGoodsId, item.activityNum, taskId, "isAdd");
                            }
                            else if (int.Equals(updateType, 3)) //更新库存
                            {
                                //更新价格  item.originalPrice
                                UpdataGoodsSpikeCommodityPriceJob(logAppendToForms, jobInfo, item.spikeGoodsId, item.originalPrice, 99999, taskId, "isAdd");
                                //UpdataGoodsSpikeCommodityPriceJob(logAppendToForms, jobInfo, item.spikeGoodsId, "99999.99", 99999, taskId, "isAdd");
                            }
                        }
                    }
                    else if (string.Equals(isStart, "Y"))
                    {
                        //任务开始，但未结束
                        if (string.Equals(isEnd, "N"))
                        {
                            //结未日期已到
                            if (ComparisonDate(logAppendToForms, jobInfo, string.Format("{0} {1}", item.endDate, item.endTime)) == 0)
                            {
                                UpdataGoodsSpikeCommodityRepertory(logAppendToForms, jobInfo, item.spikeGoodsId, 0, taskId, "isEnd"); //更新库存为零
                                if (int.Equals(updateType, 1))
                                {
                                    //更新商品资料
                                    UpdataGoodsSpikeCommodity(logAppendToForms, jobInfo, item, item.spikeGoodsId, taskId, "isEnd");
                                    //更新图片
                                    //UpdateGoodsSpikeCommodityImageJob(logAppendToForms, jobInfo, item.spikeGoodsId, taskId, "isEnd");
                                }
                                else if (int.Equals(updateType, 2)) //更新库存
                                {
                                    //更新库存 
                                    UpdataGoodsSpikeCommodityRepertory(logAppendToForms, jobInfo, item.spikeGoodsId, 0, taskId, "isEnd");
                                }
                                else if (int.Equals(updateType, 3)) 
                                {
                                    //更新价格
                                    UpdataGoodsSpikeCommodityPriceJob(logAppendToForms, jobInfo, item.spikeGoodsId, "99999.99", 99999, taskId, "isEnd");
                                }
                            }
                            //活动中
                            else
                            {
                                if (int.Equals(updateType, 1))
                                {
                                    //更新商品资料
                                    UpdataGoodsSpikeCommodity(logAppendToForms, jobInfo, item, item.spikeGoodsId, taskId, "isStart");
                                    //更新图片
                                    UpdateGoodsSpikeCommodityImageJob(logAppendToForms, jobInfo, item.spikeGoodsId, taskId, "isStart");
                                }
                                else if (int.Equals(updateType, 2)) //更新库存
                                {
                                    //更新库存 
                                    UpdataGoodsSpikeCommodityRepertory(logAppendToForms, jobInfo, item.spikeGoodsId, item.activityNum, taskId, "isStart");
                                }
                                else if (int.Equals(updateType, 3)) 
                                {
                                    //更新价格
                                    UpdataGoodsSpikeCommodityPriceJob(logAppendToForms, jobInfo, item.spikeGoodsId, item.spikePrice, item.spikelimitMax, taskId, "isStart");
                                }
                            }
                        }//任务结束
                        else if (string.Equals(isEnd, "Y"))
                        {
                            UpdataGoodsSpikeCommodityRepertory(logAppendToForms, jobInfo, item.spikeGoodsId,0, taskId, "isEnd"); //更新库存为零
                            if (int.Equals(updateType, 1))
                            {
                                //更新商品资料
                                UpdataGoodsSpikeCommodity(logAppendToForms, jobInfo, item,item.spikeGoodsId, taskId, "isEnd");
                                //更新图片
                                //UpdateGoodsSpikeCommodityImageJob(logAppendToForms, jobInfo, item.spikeGoodsId, taskId, "isEnd");
                            }
                            else if (int.Equals(updateType, 2)) //更新库存
                            {
                                //更新库存 
                                UpdataGoodsSpikeCommodityRepertory(logAppendToForms, jobInfo, item.spikeGoodsId, 0, taskId, "isEnd");
                            }
                            else if (int.Equals(updateType, 3)) 
                            {
                                //更新价格
                                UpdataGoodsSpikeCommodityPriceJob(logAppendToForms, jobInfo, item.spikeGoodsId,"99999.99", 99999, taskId, "isEnd");
                            }
                        }
                    }

                }

            }
            jobInfo = null;
            dataTable = null;
        }
        #endregion

        #region UpdataGoodsSpikeCommodityRepertory    同步商品库存接口(异步)
        /// <summary>
        /// UpdataGoodsSpikeCommodityRepertory  同步商品库存接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void UpdataGoodsSpikeCommodityRepertory(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();

            jobInfo = GetJobEntity(jobInfo, "DataApiCommodityRepertoryAsync", " 同步商品库存接口(异步)(秒杀特价)");
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo, "GoodsSpikeCommodityRepertory");
            if (dataTable == null)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }



            List<Model.CommodityRepertory> items = new List<Model.CommodityRepertory>();


            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.CommodityRepertory item = new Model.CommodityRepertory();

                item.dateExpiration = Util.DataTableHelper.DataRowContains(dr, "dateExpiration");  //dateExpiration
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                item.productionDate = Util.DataTableHelper.DataRowContains(dr, "productionDate");
                item.repertory = Util.DataTableHelper.DataRowContainsInt(dr, "repertory");
                item.shelveStatus = Util.DataTableHelper.DataRowContainsInt(dr, "shelveStatus");
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

            int pagesSize = jobInfo.PageSize <= 0 ? items.Count() : jobInfo.PageSize;
            int pagesCount = items.Count();
            int splitCopies = (int)Math.Ceiling((double)pagesCount / pagesSize);
            int pageNum = 0; //页码
            for (int i = 0; i < splitCopies; i++)
            {
                pageNum = i;
                //pageSize ：表示一页多少条。
                //pageNum：表示页数，但是正确的页数是pageNum + 1。因为pageNum = 0，是第一页。pageNum = 1的时候，是第二页。
                //Skip ：表示从第pageNum* pageSize +1条数据开始，也就是说再这之前有pageNum* pageSize条数据。
                //Take：表示显示多少条数据，也就是pageSize条。
                //Take：表示显示多少条数据，也就是pageSize条。
                var pageItems = items.Skip(pageNum * pagesSize).Take(pagesSize).ToList();

                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(pageItems);
                string resultJson = string.Empty;
                jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());
                //CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                if (!resultStatus)
                {
                    string msg = resultJObject["msg"].ToString();
                    if (string.Equals(msg, "操作超时"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，第二次调用", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        Thread.Sleep(5000);//休眠时间
                        resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                        resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                        if (!resultStatus)
                        {
                            msg = resultJObject["msg"].ToString();
                            if (string.Equals(msg, "操作超时"))
                            {
                                logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时10秒，第三次调用", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                                Thread.Sleep(10000);//休眠时间
                                resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);

                            }
                        }

                    }
                }
                pageItems = null;
            }
            //dataTable = null;
            //items = null;
            //jobInfo = null;
        }
        #endregion

        #region UpdataGoodsSpikeCommodityRepertoryAsync    同步商品库存接口(异步)
        /// <summary>
        /// UpdataGoodsSpikeCommodityRepertory  同步商品库存接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void UpdataGoodsSpikeCommodityRepertoryAsync(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();

            jobInfo = GetJobEntity(jobInfo, "DataApiCommodityRepertoryAsync", " 同步商品库存接口(异步)(秒杀特价)");
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo, "GoodsSpikeCommodityRepertory");
            if (dataTable == null)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }



            List<Model.CommodityRepertory> items = new List<Model.CommodityRepertory>();


            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.CommodityRepertory item = new Model.CommodityRepertory();

                item.dateExpiration = Util.DataTableHelper.DataRowContains(dr, "dateExpiration");  //dateExpiration
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                item.productionDate = Util.DataTableHelper.DataRowContains(dr, "productionDate");
                item.repertory = Util.DataTableHelper.DataRowContainsInt(dr, "repertory");
                item.shelveStatus = Util.DataTableHelper.DataRowContainsInt(dr, "shelveStatus");
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

            int pagesSize = jobInfo.PageSize <= 0 ? items.Count() : jobInfo.PageSize;
            int pagesCount = items.Count();
            int splitCopies = (int)Math.Ceiling((double)pagesCount / pagesSize);
            int pageNum = 0; //页码
            for (int i = 0; i < splitCopies; i++)
            {
                pageNum = i;
                //pageSize ：表示一页多少条。
                //pageNum：表示页数，但是正确的页数是pageNum + 1。因为pageNum = 0，是第一页。pageNum = 1的时候，是第二页。
                //Skip ：表示从第pageNum* pageSize +1条数据开始，也就是说再这之前有pageNum* pageSize条数据。
                //Take：表示显示多少条数据，也就是pageSize条。
                //Take：表示显示多少条数据，也就是pageSize条。
                var pageItems = items.Skip(pageNum * pagesSize).Take(pagesSize).ToList();

                string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(pageItems);
                string resultJson = string.Empty;
                jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());
                //CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                if (!resultStatus)
                {
                    string msg = resultJObject["msg"].ToString();
                    if (string.Equals(msg, "操作超时"))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，第二次调用", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        Thread.Sleep(5000);//休眠时间
                        resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                        resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                        if (!resultStatus)
                        {
                            msg = resultJObject["msg"].ToString();
                            if (string.Equals(msg, "操作超时"))
                            {
                                logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时10秒，第三次调用", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                                Thread.Sleep(10000);//休眠时间
                                resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);

                            }
                        }

                    }
                }
                pageItems = null;
            }
           // dataTable = null;
            //items = null;
            //jobInfo = null;

        }
        #endregion 

        #region UpdataGoodsSpikeCommodityRepertory    更新指定商品库存接口
        /// <summary>
        ///  UpdataGoodsSpikeCommodityRepertory    更新指定商品库存接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="erpGoodsId"></param>
        /// <param name="repertory"></param>
        private bool UpdataGoodsSpikeCommodityRepertory(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, string erpGoodsId, int repertory,string taskId,string startType)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();

            jobInfo = GetJobEntity(jobInfo, "DataApiCommodityRepertory", " 同步商品库存接口(秒杀特价)");

            List<Model.CommodityRepertory> items = new List<Model.CommodityRepertory>();

            Model.CommodityRepertory item = new Model.CommodityRepertory();

            item.dateExpiration = string.Empty;  //dateExpiration
            item.erpGoodsId = erpGoodsId;
            item.productionDate = string.Empty;
            item.repertory = repertory;
            if (string.Equals(startType, "isEnd"))
                item.shelveStatus = 2;
            else
                item.shelveStatus = 1;
            items.Add(item);

            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return false;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;

            bool resultStatus = false;
            resultStatus=    CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
            Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
            if (!resultStatus)
            {
                string msg = resultJObject["msg"].ToString();
                if (string.Equals(msg, "操作超时"))
                {
                    logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，再次调用一次", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    Thread.Sleep(5000);//休眠时间
                    resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                }
            }
            if (resultStatus)
                ErpWriteback(logAppendToForms, jobInfo, "GoodsSpikeUpdataCommodityRepertory", erpGoodsId, taskId, startType);
            return resultStatus;

        }
        #endregion

        #region UpdataGoodsSpikeCommodityRepertoryAsync    更新指定商品库存接口(异步)(秒杀特价)
        /// <summary>
        ///  UpdataGoodsSpikeCommodityRepertoryAsync    更新指定商品库存接口(异步)(秒杀特价)
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="erpGoodsId"></param>
        /// <param name="repertory"></param>
        private bool UpdataGoodsSpikeCommodityRepertoryAsync(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, string erpGoodsId, int repertory, string taskId, string startType)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();

            jobInfo = GetJobEntity(jobInfo, "DataApiCommodityRepertoryAsync", " 同步商品库存接口(异步)(秒杀特价)");

            List<Model.CommodityRepertory> items = new List<Model.CommodityRepertory>();

            Model.CommodityRepertory item = new Model.CommodityRepertory();

            item.dateExpiration = string.Empty;  //dateExpiration
            item.erpGoodsId = erpGoodsId;
            item.productionDate = string.Empty;
            item.repertory = repertory;
            if (string.Equals(startType, "isEnd"))
                item.shelveStatus = 2;
            else
                item.shelveStatus = 1;
            items.Add(item);

            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return false;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;

            bool resultStatus = false;
            resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
            Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
            if (!resultStatus)
            {
                string msg = resultJObject["msg"].ToString();
                if (string.Equals(msg, "操作超时"))
                {
                    logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，再次调用一次", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    Thread.Sleep(5000);//休眠时间
                    resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                }
            }
            if (resultStatus)
                ErpWriteback(logAppendToForms, jobInfo, "GoodsSpikeUpdataCommodityRepertory", erpGoodsId, taskId, startType);
            return resultStatus;

        }
        #endregion

        #region UpdataGoodsSpikeCommodity    同步商品基础数据资料接口(秒杀特价)
        /// <summary>
        /// UpdataGoodsSpikeCommodity  同步商品基础数据资料接口(秒杀特价)
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void UpdataGoodsSpikeCommodity(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, Model.GoodsSpike goodsSpike, string erpGoodsId, string taskId, string startType)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            jobInfo = GetJobEntity(jobInfo, "DataApiCommodity", " 同步商品数据(秒杀特价)");
            jobInfo.ModuleID = "GoodsSpikeCommodity";
            System.Data.DataTable commodityDt = GetUpdataDataTable(logAppendToForms, jobInfo, erpGoodsId,string.Empty);
            if (commodityDt == null)
            {
                logMessage = string.Format("【{0}_{1}】  同步商品数据失败！  商品数据为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (commodityDt.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

            List<Model.Commodity> commodityItems = new List<Model.Commodity>();
            foreach (System.Data.DataRow dr in commodityDt.Rows)
            {
                Model.Commodity commodityItem = new Model.Commodity();

                commodityItem.appearance = Util.DataTableHelper.DataRowContains(dr, "appearance"); //Add 20200722
                commodityItem.approvalNo = Util.DataTableHelper.DataRowContains(dr, "approvalNo");
                commodityItem.bases = Util.DataTableHelper.DataRowContains(dr, "bases");  //Add 20200722
                commodityItem.brandName = Util.DataTableHelper.DataRowContains(dr, "brandName");
                commodityItem.businessScopeCode = Util.DataTableHelper.DataRowContains(dr, "businessScopeCode");
                //commodityItem.businessScopeName = Util.DataTableHelper.DataRowContains(dr, "businessScopeName");
                commodityItem.catagoryCode = Util.DataTableHelper.DataRowContains(dr, "catagoryCode");
                commodityItem.drugInteractions = Util.DataTableHelper.DataRowContains(dr, "drugInteractions"); //Add 20200722

                commodityItem.erpGoodsCode = Util.DataTableHelper.DataRowContains(dr, "erpGoodsCode");
                commodityItem.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                //commodityItem.firstLevel = Util.DataTableHelper.DataRowContains(dr, "firstLevel");
                commodityItem.formula = Util.DataTableHelper.DataRowContains(dr, "formula");
                commodityItem.logogram = Util.DataTableHelper.DataRowContains(dr, "logogram");

                commodityItem.isMedicalInstruments = Util.DataTableHelper.DataRowContains(dr, "isMedicalInstruments");  //isMedicalInstruments Add 20200722
                //commodityItem.goodsAttr = Util.DataTableHelper.DataRowContains(dr, "goodsAttr");
                commodityItem.goodsName = Util.DataTableHelper.DataRowContains(dr, "goodsName");
                commodityItem.goodsSpec = Util.DataTableHelper.DataRowContains(dr, "goodsSpec");
                commodityItem.goodsTradeName = Util.DataTableHelper.DataRowContains(dr, "goodsTradeName");
                commodityItem.goodsType = Util.DataTableHelper.DataRowContains(dr, "goodsType");
                commodityItem.majorFunctions = Util.DataTableHelper.DataRowContains(dr, "majorFunctions"); //Add 20200722

                commodityItem.manufacturer = Util.DataTableHelper.DataRowContains(dr, "manufacturer");
                commodityItem.manufacturerLogogram = Util.DataTableHelper.DataRowContains(dr, "manufacturerLogogram");
                commodityItem.marketingAuthorizationHolder = Util.DataTableHelper.DataRowContains(dr, "marketingAuthorizationHolder");
                commodityItem.middlePackAmount = Util.DataTableHelper.DataRowContainsInt(dr, "middlePackAmount");
                commodityItem.modCount = Util.DataTableHelper.DataRowContainsInt(dr, "modCount");
                commodityItem.originPlace = Util.DataTableHelper.DataRowContains(dr, "originPlace");  //originPlace
                commodityItem.packAmount = Util.DataTableHelper.DataRowContainsInt(dr, "packAmount");
                commodityItem.packUnits = Util.DataTableHelper.DataRowContains(dr, "packUnits");
                commodityItem.pharmacologicalAction = Util.DataTableHelper.DataRowContains(dr, "pharmacologicalAction");  //pharmacologicalAction
                commodityItem.prescriptionType = Util.DataTableHelper.DataRowContains(dr, "prescriptionType");
                commodityItem.qualityStandard = Util.DataTableHelper.DataRowContains(dr, "qualityStandard");
                commodityItem.sellState = Util.DataTableHelper.DataRowContainsInt(dr, "sellState");
                int recommend = Util.DataTableHelper.DataRowContainsInt(dr, "recommend");
                
                commodityItem.recommend =recommend<2 ? 999 : recommend;
                if (string.Equals(startType, "isEnd"))  //isEnd
                {
                    commodityItem.recommend = 0;
                    commodityItem.sellState = 2;
                }

                commodityItem.searchKey = Util.DataTableHelper.DataRowContains(dr, "searchKey");
                //commodityItem.secondLevel = Util.DataTableHelper.DataRowContains(dr, "secondLevel");
                commodityItem.sellCtrlAdmin = Util.DataTableHelper.DataRowContains(dr, "sellCtrlAdmin");
                commodityItem.sellCtrlBusinessType = Util.DataTableHelper.DataRowContains(dr, "sellCtrlBusinessType");
                commodityItem.sellingPoint = Util.DataTableHelper.DataRowContains(dr, "sellingPoint");
                
                commodityItem.storageType = Util.DataTableHelper.DataRowContains(dr, "storageType");
                commodityItem.store = Util.DataTableHelper.DataRowContains(dr, "store");  //store
                commodityItem.suggestedRetailPrice = Math.Round(Util.DataTableHelper.DataRowContainsDecimal(dr, "suggestedRetailPrice"), 2, MidpointRounding.AwayFromZero).ToString("F2");// Util.DataTableHelper.DataRowContains(dr, "suggestedRetailPrice");
                commodityItem.taboo = Util.DataTableHelper.DataRowContains(dr, "taboo");
                commodityItem.untowardEffect = Util.DataTableHelper.DataRowContains(dr, "untowardEffect");
                commodityItem.usageDosage = Util.DataTableHelper.DataRowContains(dr, "usageDosage");
                commodityItem.warnings = Util.DataTableHelper.DataRowContains(dr, "warnings");
                string sellingPoint = Util.DataTableHelper.DataRowContains(dr, "sellingPoint");
                int spikeType = goodsSpike.spikeType;
                string spikeTypeStr = string.Empty;
                if (int.Equals(spikeType, 1))
                    spikeTypeStr = "限时秒杀";
                else if (int.Equals(spikeType, 2))
                    spikeTypeStr = "超值特价";
                else if (int.Equals(spikeType, 3))
                    spikeTypeStr = "买赠";

                commodityItem.activityType = spikeType;
                commodityItem.activityStartTime = Util.DataTableHelper.DataRowContains(dr, "activityStartTime");
                commodityItem.activityEndTime = Util.DataTableHelper.DataRowContains(dr, "activityEndTime");

                if (string.Equals(startType, "isEnd"))
                {
                    string strInto = string.Format("【{0}】 活动已结束！！！【{1} {2} -{3} {4}】 ", spikeTypeStr, goodsSpike.startDate, goodsSpike.startTime, goodsSpike.endDate, goodsSpike.endTime);
                    if (int.Equals(spikeType, 3))
                        strInto = "【买赠】";
                        //commodityItem.sellingPoint = string.Format("活动结束！！！{0}", sellingPoint);
                    if (goodsSpike.spikelimitMax > 0)
                        //commodityItem.sellingPoint = string.Format("【{0}】 活动结束！！！【{1} {2} -{3} {4}】 限购{5}{6} {7}", spikeTypeStr,goodsSpike.startDate, goodsSpike.startTime, goodsSpike.endDate, goodsSpike.endTime, goodsSpike.spikelimitMax.ToString(), commodityItem.packUnits, sellingPoint);
                        commodityItem.sellingPoint = string.Format("{0}  限购{1}{2} {3}", strInto, goodsSpike.spikelimitMax.ToString(), commodityItem.packUnits, sellingPoint);
                    else
                        //commodityItem.sellingPoint = string.Format("【{0}】 活动结束！！！【{1} {2} -{3} {4}】 {5}", spikeTypeStr,goodsSpike.startDate, goodsSpike.startTime, goodsSpike.endDate, goodsSpike.endTime, sellingPoint);
                        commodityItem.sellingPoint = string.Format("{0}  {1}", strInto, sellingPoint);
                }
                else if (string.Equals(startType, "isAdd"))
                {
                    string strInto = string.Format("【{0}】 未开始！！！", spikeTypeStr);
                    
                    if (goodsSpike.spikelimitMax > 0)
                        //commodityItem.sellingPoint = string.Format("【{0}】【{1} {2}】开始！！！限购{3}{4} {5}", spikeTypeStr, goodsSpike.startDate, goodsSpike.startTime, goodsSpike.spikelimitMax.ToString(), commodityItem.packUnits, sellingPoint);
                        commodityItem.sellingPoint = string.Format("{0}  限购{1}  {2}  {3}", strInto,  goodsSpike.spikelimitMax.ToString(), commodityItem.packUnits, sellingPoint);
                    else
                        //commodityItem.sellingPoint = string.Format("【{0}】【{1} {2}】开始！！！ {3}", spikeTypeStr, goodsSpike.startDate, goodsSpike.startTime,  sellingPoint);
                        commodityItem.sellingPoint = string.Format("{0} {1}", strInto,  sellingPoint);
                }
                else if (string.Equals(startType, "isStart"))
                {
                    string strInto = string.Format("【{0}】【{1} {2} -{3} {4}】 ", spikeTypeStr, goodsSpike.startDate, goodsSpike.startTime, goodsSpike.endDate, goodsSpike.endTime);
                    if (int.Equals(spikeType, 3))
                        strInto = "【买赠】";
                    if (goodsSpike.spikelimitMax > 0)
                        //commodityItem.sellingPoint = string.Format("【{0}】【{1} {2} -{3} {4}】 限购{5}{6} {7}", spikeTypeStr, goodsSpike.startDate, goodsSpike.startTime, goodsSpike.endDate, goodsSpike.endTime, goodsSpike.spikelimitMax.ToString(), commodityItem.packUnits, sellingPoint);
                        commodityItem.sellingPoint = string.Format("{0}   限购{1}   {2}   {3}", strInto, goodsSpike.spikelimitMax.ToString(), commodityItem.packUnits, sellingPoint);
                    else
                        //commodityItem.sellingPoint = string.Format("【{0}】【{1} {2} -{3} {4}】 {5}", spikeTypeStr, goodsSpike.startDate, goodsSpike.startTime, goodsSpike.endDate, goodsSpike.endTime, sellingPoint);
                        commodityItem.sellingPoint = string.Format("{0} {1}", strInto, sellingPoint);

                }
                else
                    commodityItem.sellingPoint = sellingPoint;// commodityItem.sellingPoint = string.Format("活动结束！！！{0}", sellingPoint);  //sellingPoint;
                //commodityItem.taskId = Util.DataTableHelper.DataRowContainsInt(dr, "taskId");

                commodityItems.Add(commodityItem);
            }
            if (commodityItems != null && commodityItems.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  同步商品数据失败！  商品实体为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(commodityItems);
            string resultJson = string.Empty;
          
            bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
            Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
            if (!resultStatus)
            {
                string msg = resultJObject["msg"].ToString();
                if (string.Equals(msg, "操作超时"))
                {
                    logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，再次调用一次", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    Thread.Sleep(5000);//休眠时间
                    resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                }
            }
            if (resultStatus)
                ErpWriteback(logAppendToForms, jobInfo, "UpdataGoodsSpikeCommodity", erpGoodsId, taskId, startType);
            //commodityItems = null;
           // commodityDt = null;
            //jobInfo = null;
        }

        #endregion

        #region UpdateGoodsSpikeCommodityImageJob    同步商品图片接口(秒杀特价)
        /// <summary>
        /// UpdateGoodsSpikeCommodityImageJob  同步商品图片接口(秒杀特价)
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void UpdateGoodsSpikeCommodityImageJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, string erpGoodsId, string taskId, string startType)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            jobInfo = GetJobEntity(jobInfo, "DataApiCommodityImage", " 同步商品图片接口(秒杀特价)");
            jobInfo.ModuleID = "GoodsSpikeCommodityImage";
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo, erpGoodsId,string.Empty);
            if (dataTable == null)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            if (dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}无数据，不需要同步！！！", jobInfo.JobCode, jobInfo.JobName);
                LogWarning(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.CommodityImage> items = new List<Model.CommodityImage>();

            dataTable = ImagesFileToBytes(logAppendToForms, jobInfo, dataTable);
            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 加载图片后数据为空1！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            //isImagesCompleted
            System.Data.DataTable imageDetailsDt = Util.DataTableHelper.GetNewDataTable(dataTable, "isImagesCompleted='Y' ");
            foreach (System.Data.DataRow dr in imageDetailsDt.Rows)
            {
                Model.CommodityImage item = new Model.CommodityImage();
                item.base64Str = Util.DataTableHelper.DataRowContains(dr, "base64Str");
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "spikeGoodsId");
                item.fileName = Util.DataTableHelper.DataRowContains(dr, "fileName");
                item.fileType = Util.DataTableHelper.DataRowContains(dr, "fileType").Replace(".", string.Empty);
                item.imageType = Util.DataTableHelper.DataRowContainsInt(dr, "imageType");  //图片类型1药品图片;2正面图;3背面图;4:45度角图;5条形码图;6拆包图
                item.taskId = Util.DataTableHelper.DataRowContainsInt(dr, "taskId");
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }

            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;
            //jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());

            bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
            Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
            if (!resultStatus)
            {
                string msg = resultJObject["msg"].ToString();
                if (string.Equals(msg, "操作超时"))
                {
                    logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，再次调用一次", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    Thread.Sleep(5000);//休眠时间
                    resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                }
            }
            //if (resultStatus)
            //  ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskId, "UpdateCommodityImageJob");
            //dataTable = null;
           // imageDetailsDt = null;
        }
        #endregion

        #region UpdataGoodsSpikeCommodityPriceJob    同步商品价格接口(秒杀特价)
        /// <summary>
        /// UpdataGoodsSpikeCommodityPriceJob  同步商品价格接口(秒杀特价)
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void UpdataGoodsSpikeCommodityPriceJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, string erpGoodsId, string lsj, int limitMax, string taskId, string startType)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            jobInfo = GetJobEntity(jobInfo, "DataApiCommodityPrice", " 同步商品价格接口(秒杀特价)");

            List<Model.CommodityPrice> items = new List<Model.CommodityPrice>();

            Model.CommodityPrice item = new Model.CommodityPrice();
            item.erpGoodsId = erpGoodsId;
            item.limitMax = limitMax;
            item.limitMin = 0;
            item.lsj = lsj;
            item.lsjAbsolute = lsj;
            item.lsjMax = "0";
            item.lsjMin = "0";

            items.Add(item);

            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;
            //jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());

            bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
            Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
            if (!resultStatus)
            {
                string msg = resultJObject["msg"].ToString();
                if (string.Equals(msg, "操作超时"))
                {
                    logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，再次调用一次", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    Thread.Sleep(5000);//休眠时间
                    resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                }
            }
            if (resultStatus)
              ErpWriteback(logAppendToForms, jobInfo, "GoodsSpikeUpdataCommodityPriceJob", erpGoodsId, taskId, startType);
        }
        #endregion

        #region UpdataGoodsSpikeCommodityPriceJobAsync    同步商品价格接口(异步)(秒杀特价)
        /// <summary>
        /// UpdataGoodsSpikeCommodityPriceJobAsync   同步商品价格接口(异步)(秒杀特价)
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void UpdataGoodsSpikeCommodityPriceJobAsync(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, string erpGoodsId, string lsj, int limitMax, string taskId, string startType)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            jobInfo = GetJobEntity(jobInfo, "DataApiCommodityPriceAsync", " 同步商品价格接口(异步)(秒杀特价)");

            List<Model.CommodityPrice> items = new List<Model.CommodityPrice>();

            Model.CommodityPrice item = new Model.CommodityPrice();
            item.erpGoodsId = erpGoodsId;
            item.limitMax = limitMax;
            item.limitMin = 0;
            item.lsj = lsj;
            item.lsjAbsolute = lsj;
            item.lsjMax = "0";
            item.lsjMin = "0";

            items.Add(item);

            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;
            //jobInfo.JobInfo = string.Format("共{0}条;分{1}次上传;当前第{2}次", pagesCount.ToString(), splitCopies.ToString(), (pageNum + 1).ToString());

            bool resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
            Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
            if (!resultStatus)
            {
                string msg = resultJObject["msg"].ToString();
                if (string.Equals(msg, "操作超时"))
                {
                    logMessage = string.Format("【{0}_{1}】  {1} {2} 调用接口超时，延时5秒，再次调用一次", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo);
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    Thread.Sleep(5000);//休眠时间
                    resultStatus = CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson);
                }
            }
            if (resultStatus)
                ErpWriteback(logAppendToForms, jobInfo, "GoodsSpikeUpdataCommodityPriceJob", erpGoodsId, taskId, startType);
        }
        #endregion

        #region ExecuteGoodsSpikeTaskJob    商品秒杀
        /// <summary>
        /// ExecuteDataApiGoodsSpikeJob  商品秒杀
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        private void ExecuteGoodsSpikeTaskJob(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            ErpExecuteProcedure(logAppendToForms, jobInfo, "Green_YyhB2b_GoodsSpike");

        }
        #endregion

        #region GetUpdataDataTable
        /// <summary>
        /// GetUpdataDataTable
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        private System.Data.DataTable GetUpdataDataTable(Log4netUtil.LogAppendToForms logAppendToForms,
                                                    Model.JobEntity jobInfo)
        {
            Model.SearchParam searchParam = new Model.SearchParam();
            searchParam.TargetDatabase = jobInfo.TargetDatabase;// "Erp";
            searchParam.ProcedureName = jobInfo.ProcedureName;// "DtyPrepurchInterface_SqlView";
            searchParam.ModuleID = jobInfo.ModuleID;
            searchParam.IsMaintain = jobInfo.FilterBillType;
            searchParam.StartDate = DateTime.Now.ToString("yyyy-MM-dd");
            searchParam.EndDate = DateTime.Now.AddDays(-jobInfo.ConfigInfo.AutoAdvanceDays).ToString("yyyy-MM-dd");
            searchParam.Logogram = string.Empty;
            searchParam.BillCode = string.Empty;
            searchParam.IsDebug = jobInfo.IsDebug;
            searchParam.jobInfo = jobInfo;
            BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
            Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
            return ibll.GetDataTableAll(logAppendToForms, searchParam);
        }


        /// <summary>
        /// GetUpdataDataTable
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        private System.Data.DataTable GetUpdataDataTable(Log4netUtil.LogAppendToForms logAppendToForms,
                                                    Model.JobEntity jobInfo, string moduleID)
        {
            Model.SearchParam searchParam = new Model.SearchParam();
            searchParam.TargetDatabase = jobInfo.TargetDatabase;// "Erp";
            searchParam.ProcedureName = jobInfo.ProcedureName;// "DtyPrepurchInterface_SqlView";
            searchParam.ModuleID = moduleID;// jobInfo.ModuleID;
            searchParam.IsMaintain = jobInfo.FilterBillType;
            searchParam.StartDate = DateTime.Now.ToString("yyyy-MM-dd");
            searchParam.EndDate = DateTime.Now.AddDays(-jobInfo.ConfigInfo.AutoAdvanceDays).ToString("yyyy-MM-dd");
            searchParam.Logogram = string.Empty;
            searchParam.BillCode = string.Empty;
            searchParam.IsDebug = jobInfo.IsDebug;
            searchParam.jobInfo = jobInfo;
            BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
            Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
            return ibll.GetDataTableAll(logAppendToForms, searchParam);
        }



        /// <summary>
        /// GetUpdataDataTable
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        private System.Data.DataTable GetUpdataDataTable(Log4netUtil.LogAppendToForms logAppendToForms,
                                                    Model.JobEntity jobInfo,string billCode,string logogram)
        {
            Model.SearchParam searchParam = new Model.SearchParam();
            searchParam.TargetDatabase = jobInfo.TargetDatabase;// "Erp";
            searchParam.ProcedureName = jobInfo.ProcedureName;// "DtyPrepurchInterface_SqlView";
            searchParam.ModuleID = jobInfo.ModuleID;
            searchParam.IsMaintain = jobInfo.FilterBillType;
            searchParam.StartDate = DateTime.Now.ToString("yyyy-MM-dd");
            searchParam.EndDate = DateTime.Now.AddDays(-jobInfo.ConfigInfo.AutoAdvanceDays).ToString("yyyy-MM-dd");
            searchParam.Logogram = logogram;
            searchParam.BillCode = billCode;
            searchParam.IsDebug = jobInfo.IsDebug;
            searchParam.jobInfo = jobInfo;
            BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
            Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
            return ibll.GetDataTableAll(logAppendToForms, searchParam);
        }
        #endregion



        #region ExecuteScalar
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="moduleID"></param>
        /// <param name="billCode"></param>
        /// <param name="logogram"></param>
        /// <returns></returns>
        private string ExecuteScalar(Log4netUtil.LogAppendToForms logAppendToForms,
                                     Model.JobEntity jobInfo,string moduleID,string billCode,string logogram)
        {
            Model.SearchParam searchParam = new Model.SearchParam();
            searchParam.TargetDatabase = jobInfo.TargetDatabase;// "Erp";
            searchParam.ProcedureName = jobInfo.ProcedureName;// "DtyPrepurchInterface_SqlView";
            searchParam.ModuleID = moduleID;
            searchParam.IsMaintain = jobInfo.FilterBillType;
            searchParam.StartDate = DateTime.Now.ToString("yyyy-MM-dd");
            searchParam.EndDate = DateTime.Now.AddDays(-jobInfo.ConfigInfo.AutoAdvanceDays).ToString("yyyy-MM-dd");
            searchParam.Logogram = logogram;
            searchParam.BillCode = billCode;
            searchParam.IsDebug = jobInfo.IsDebug;
            searchParam.jobInfo = jobInfo;
            BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
            Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
            return ibll.ExecuteScalar(logAppendToForms, searchParam);
        }
        #endregion

        #region CallB2bApi
        /// <summary>
        /// CallB2bApi
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="requestData"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool CallB2bApi(Log4netUtil.LogAppendToForms logAppendToForms,
                                Model.JobEntity jobInfo,string requestData, out string result)
        {
            string logMessage = string.Empty;
            var b2bApi = new Util.B2bApi(jobInfo.DomainName, jobInfo.ServiceName, jobInfo.InterfacePrefix, jobInfo.ConfigInfo.SignKey, jobInfo.ConfigInfo.EncryptKey);

            result = b2bApi.B2bApiRequestByJson(logAppendToForms, jobInfo,requestData);
            Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(result);
            if (string.Equals(resultJObject.Value<int>("code"), 200))
            {
                logMessage = string.Format("【{0}_{1}】 {2} result:{3} 成功！ ", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo,result);
                //LogMessage(logAppendToForms, true, logMessage, jobInfo.JobCode);
                Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms,true, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                return true;
            }
            else
            {
                logMessage = string.Format("【{0}_{1}】 {2} result:{3} ", jobInfo.JobCode, jobInfo.JobName, jobInfo.JobInfo, result);
                //LogError(logAppendToForms, true, logMessage, jobInfo.JobCode);
                Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, true, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                return false;
            }
        }
        #endregion

        #region ErpWriteback  Erp回写
        /// <summary>
        /// ErpWriteback  Erp回写
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="type"></param>
        /// <param name="BillCodes"></param>
        /// <param name="WritebackInfo"></param>
        /// <returns></returns>
        private bool ErpWriteback(Log4netUtil.LogAppendToForms logAppendToForms,
                                Model.JobEntity jobInfo,string type,string BillCodes,string WritebackInfo)
        {
            Model.WritebackParam writebackParam = new Model.WritebackParam();
            writebackParam.TargetDatabase = jobInfo.TargetDatabase;
            writebackParam.ProcedureName = jobInfo.WritebackProcedureName;
            writebackParam.WritebackType = jobInfo.WritebackType;
            writebackParam.Type = type;
            writebackParam.BillCodes = BillCodes;
            writebackParam.WritebackInfo = WritebackInfo;
            writebackParam.IsDebug = jobInfo.IsDebug;
            writebackParam.Status = 1;
            writebackParam.jobInfo = jobInfo;
            BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
            Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
            return ibll.ErpWriteback(logAppendToForms, writebackParam);
        }

        private bool ErpWriteback(Log4netUtil.LogAppendToForms logAppendToForms,
                                Model.JobEntity jobInfo,string writebackType, string type, string BillCodes, string WritebackInfo)
        {
            Model.WritebackParam writebackParam = new Model.WritebackParam();
            writebackParam.TargetDatabase = jobInfo.TargetDatabase;
            writebackParam.ProcedureName = jobInfo.WritebackProcedureName;
            writebackParam.WritebackType = writebackType;
            writebackParam.Type = type;
            writebackParam.BillCodes = BillCodes;
            writebackParam.WritebackInfo = WritebackInfo;
            writebackParam.IsDebug = jobInfo.IsDebug;
            writebackParam.Status = 1;
            writebackParam.jobInfo = jobInfo;
            BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
            Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
            return ibll.ErpWriteback(logAppendToForms, writebackParam);
        }
        #endregion

        #region ErpWriteback  Erp回写
        /// <summary>
        /// ErpWriteback  Erp回写
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="type"></param>
        /// <param name="BillCodes"></param>
        /// <param name="WritebackInfo"></param>
        /// <returns></returns>
        private bool ErpWriteback1(Log4netUtil.LogAppendToForms logAppendToForms,
                                Model.JobEntity jobInfo, string type, string BillCodes, string WritebackInfo)
        {
            Model.WritebackParam writebackParam = new Model.WritebackParam();
            writebackParam.TargetDatabase = jobInfo.TargetDatabase;
            writebackParam.ProcedureName = jobInfo.WritebackProcedureName;
            writebackParam.WritebackType = jobInfo.WritebackType;
            writebackParam.Type = type;
            writebackParam.BillCodes = BillCodes;
            writebackParam.WritebackInfo = WritebackInfo;
            writebackParam.IsDebug = jobInfo.IsDebug;
            writebackParam.Status = 1;
            writebackParam.jobInfo = jobInfo;
            BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
            Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
            return ibll.ErpWriteback(logAppendToForms, writebackParam);
        }
        #endregion

        #region ErpExecuteProcedure
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="writebackType"></param>
        /// <returns></returns>
        private bool ErpExecuteProcedure(Log4netUtil.LogAppendToForms logAppendToForms,
                                Model.JobEntity jobInfo, string writebackType)
        {
            Model.WritebackParam writebackParam = new Model.WritebackParam();
            writebackParam.TargetDatabase = jobInfo.TargetDatabase;
            writebackParam.ProcedureName = jobInfo.WritebackProcedureName;
            writebackParam.WritebackType = writebackType;
            writebackParam.Type = string.Empty;
            writebackParam.BillCodes = string.Empty;
            writebackParam.WritebackInfo = string.Empty;
            writebackParam.IsDebug = jobInfo.IsDebug;
            writebackParam.Status = 1;
            writebackParam.jobInfo = jobInfo;
            BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
            Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
            return ibll.ErpExecuteProcedure(logAppendToForms, writebackParam);
        }
        #endregion

        #region BulkInsertDatabase 批量插入数据库
        /// <summary>
        /// BulkInsertDatabase 批量插入数据库
        /// </summary>
        /// <param name="dt">需插入数据集</param>
        /// <param name="rowguid">技帐ID</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        private bool BulkInsertDatabase(Log4netUtil.LogAppendToForms logAppendToForms, 
                                       Model.JobEntity jobInfo,
                                       System.Data.DataTable dt , string insertTableName)
        {
            BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
            Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
            int result = ibll.BulkInsertDatabase(logAppendToForms, jobInfo, dt, insertTableName);
            if (int.Equals(result,1))
                return true;
            else if (int.Equals(result, 2))  //违反了 PRIMARY KEY 约束
                return true;
            else
                return false;
        }
        #endregion

        #region BulkInsertDatabase 批量插入数据库
        /// <summary>
        /// BulkInsertDatabase 批量插入数据库
        /// </summary>
        /// <param name="dt">需插入数据集</param>
        /// <param name="rowguid">技帐ID</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        private int BulkInsertDatabaseInt(Log4netUtil.LogAppendToForms logAppendToForms,
                                       Model.JobEntity jobInfo,
                                       System.Data.DataTable dt, string insertTableName)
        {
            BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
            Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
            return  ibll.BulkInsertDatabase(logAppendToForms, jobInfo, dt, insertTableName);
            
        }
        #endregion

        #region BulkInsertDatabase 批量插入数据库
        /// <summary>
        /// BulkInsertDatabase 批量插入数据库
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="insertTableName"></param>
        /// <param name="strSql"></param>
        /// <returns></returns>
        private bool BulkInsertDatabase(Log4netUtil.LogAppendToForms logAppendToForms,
                                       Model.JobEntity jobInfo,
                                       string insertTableName,string strSql)
        {
            BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
            Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
            int result = ibll.BulkInsertDatabase(logAppendToForms, jobInfo, insertTableName, strSql);
            if (int.Equals(result, 1))
                return true;
            else if (int.Equals(result, 2))  //违反了 PRIMARY KEY 约束
                return true;
            else
                return false;
        }
        #endregion

        #region UpdateDataApiOrderStatus    上传订单状态接口
        /// <summary>
        /// UpdateDataApiOrderStatus    上传订单状态接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        private bool UpdateDataApiOrderStatus(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, string orderId, int orderStatus)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();

            Model.OrderListStatus item = new Model.OrderListStatus();
            item.orderId = orderId; //订单Id
            item.orderStatus = orderStatus; // 认证状态 3支付完成 6拣货中 7商品已出库
            List<Model.OrderListStatus> items = new List<Model.OrderListStatus>();
            items.Add(item);
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;
            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                return true;
            }
            else
                return false;

        }
        #endregion

        #region UpdateDataApiOrderOutWarehouseRefund    上传订单出库差异退款
        /// <summary>
        /// UpdateDataApiOrderOutWarehouseRefund    上传订单出库差异退款
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="orderId"></param>
        /// <param name="erpOutboundIds"></param>
        /// <returns></returns>
        private bool UpdateDataApiOrderOutWarehouseRefund(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo,string orderId,string erpOutboundIds)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();
            System.Data.DataTable dataTable = GetUpdataDataTable(logAppendToForms, jobInfo, orderId, erpOutboundIds);
            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1} ;订单号:{2}  无出库差异退款数据！！！", jobInfo.JobCode, jobInfo.JobName, orderId);
                LogError(logAppendToForms, jobInfo.IsDebug, logMessage, jobLogType);
                return false;
            }
            List<Model.OrderOutWarehouseRefund> items = new List<Model.OrderOutWarehouseRefund>();

            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.OrderOutWarehouseRefund item = new Model.OrderOutWarehouseRefund();
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId"); //erp商品id
                item.erpOutboundId = Util.DataTableHelper.DataRowContains(dr, "erpOutboundId");
                item.orderId = Util.DataTableHelper.DataRowContains(dr, "orderId"); //订单id
                item.orderItemId = Util.DataTableHelper.DataRowContains(dr, "orderItemId"); //订单明细id
                item.refundNum = Util.DataTableHelper.DataRowContainsInt(dr, "refundNum"); //退款数量
                items.Add(item);
            }
            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1} ;订单号:{2}   无出库差异退款实体！！！", jobInfo.JobCode, jobInfo.JobName, orderId);
                LogError(logAppendToForms, jobInfo.IsDebug, logMessage, jobLogType);
                return false;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;

            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                //string orderIds = Util.DataTableHelper.GetColumnValues(dataTable, "orderId");
                string taskIds = Util.DataTableHelper.GetColumnValuesInt(dataTable, "taskId");
                string taskIdsReplace = taskIds.Replace(",", string.Empty);
                if (string.IsNullOrEmpty(taskIdsReplace))
                {
                    logMessage = string.Format("【{0}_{1}】  任务Id{2}失败！  更新成功后回写 taskId为空！！！", jobInfo.JobCode, jobInfo.JobName, taskIds);
                    LogError(logAppendToForms, true, logMessage, jobLogType);
                    return false ;
                }
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), taskIds, string.Empty);
                //ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), orderIds, resultJson);
                return true;
            }
            else
                return false;

        }
        #endregion

        #region UpdataDataApiCustomerStatus   更新新注册客户状态接口
        /// <summary>
        /// UpdataDataApiCustomerStatus   更新新注册客户状态接口
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="customerId"></param>
        /// <param name="auditStatus"> 2 认证失败，4 待下载 </param>
        private bool UpdataDataApiCustomerStatus(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo,string customerId, string disableReason, int auditStatus)
        {
            string logMessage = string.Empty;
            string goodsIds = string.Empty;
            string jobLogType = jobInfo.JobCode.ToString();

            List<Model.CustomerStatus> items = new List<Model.CustomerStatus>();
            Model.CustomerStatus item = new Model.CustomerStatus();
            item.auditStatus = auditStatus;
            item.customerId = customerId;
            item.disableReason = disableReason; 
            items.Add(item);

            if (items != null && items.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】 {1}失败！  实体为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return false;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(items);
            string resultJson = string.Empty;

            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                return true;
            }
            else
                return false;
        }
        #endregion

        #region GetJobEntity  
        /// <summary>
        /// GetJobEntity
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <param name="jobCode"></param>
        /// <param name="jobName"></param>
        /// <returns></returns>
        private Model.JobEntity GetJobEntity(Model.JobEntity jobInfo, string jobCode, string jobName)
        {

            Model.JobEntity resultJobInfo = new Model.JobEntity();
            resultJobInfo.JobCode = jobCode;
            resultJobInfo.JobName = jobName;
            resultJobInfo.IsDebug = jobInfo.IsDebug;
            resultJobInfo.DomainName = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, jobCode, "DomainName", null); //jobData.GetString("DomainName");
            resultJobInfo.ServiceName = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, jobCode, "ServiceName", null); //jobData.GetString("ServiceName");
            resultJobInfo.InterfacePrefix = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, jobCode, "InterfacePrefix", null); // jobData.GetString("InterfacePrefix");
            resultJobInfo.ApiModuleType = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, jobCode, "ModuleType", null); //jobData.GetString("ApiModuleType");
            resultJobInfo.ApiRequestType = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, jobCode, "RequestType", null); //jobData.GetString("ApiRequestType");

            resultJobInfo.TargetDatabase = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, jobCode, "TargetDatabase", null);  //jobInfo.TargetDatabase;
            resultJobInfo.ProcedureName = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, jobCode, "ProcedureName", null);  //jobInfo.ProcedureName;
            resultJobInfo.ModuleID = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, jobCode, "ModuleID", null);  //jobInfo.ModuleID;
            resultJobInfo.FilterBillType =  jobInfo.FilterBillType;
            resultJobInfo.WritebackProcedureName = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, jobCode, "WritebackProcedureName", null);  //jobInfo.WritebackProcedureName;
            resultJobInfo.WritebackType = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, jobCode, "WritebackType", null);  //jobInfo.WritebackType;
            resultJobInfo.InsertTableName = Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, jobCode, "InsertTableName", null); //jobInfo.InsertTableName;
            resultJobInfo.CronExpression =  jobInfo.CronExpression;
            resultJobInfo.CronExpressionDescription = jobInfo.CronExpressionDescription;

            resultJobInfo.EnterpriseId = jobInfo.EnterpriseId;
            resultJobInfo.EnterpriseName = jobInfo.EnterpriseName;
            resultJobInfo.ConfigInfo = jobInfo.ConfigInfo;
            return resultJobInfo;


        }
        #endregion

        #region OrderToStrDelete  
        /// <summary>
        /// OrderToStrDelete
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="TableName"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool OrderToStrDelete(Log4netUtil.LogAppendToForms logAppendToForms,
                                       Model.JobEntity jobInfo, string TableName, string orderId)
        {
            string header = string.Empty;
            string query = string.Empty;
            header = string.Format("DELETE FROM {0} WHERE orderId= N'{1}';\r", TableName, orderId);
            query = string.Format("BEGIN \r {0} END; \r ", header);

            return BulkInsertDatabase(logAppendToForms, jobInfo, TableName, query);
        }
        #endregion

        #region DataTableToStrDeleteAndInsert  
        /// <summary>
        /// DataTableToStrDeleteAndInsert
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public string DataTableToStrDeleteAndInsert(System.Data.DataTable dt, string TableName)
        {
            string header = string.Empty;
            string query = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                header = string.Format("DELETE FROM {0} WHERE customerId= N'{1}' AND imageType = {2} ;\r", TableName, dt.Rows[i]["customerId"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["imageType"].ToString()));
                header += "INSERT INTO " + TableName + " (";
                foreach (System.Data.DataColumn item in dt.Columns)
                {
                    header += item.ColumnName + ",";
                }
                header = header.Remove(header.Length - 1) + ") \r VALUES( ";

                string values = string.Empty;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].DataType.FullName == "System.Decimal" ||
                        dt.Columns[j].DataType.FullName == "System.Double" ||
                        dt.Columns[j].DataType.FullName == "System.Single" ||
                        dt.Columns[j].DataType.FullName == "System.Int64" ||
                        dt.Columns[j].DataType.FullName == "System.Int32" ||
                        dt.Columns[j].DataType.FullName == "System.Int"
                        )
                        values += dt.Rows[i][j].ToString() + ",";
                    else if (dt.Columns[j].DataType.FullName == "System.String")
                        values += "N'" + dt.Rows[i][j].ToString().Trim() + "',";
                    else
                        values += "N'" + dt.Rows[i][j].ToString().Trim() + "',";
                }
                query += header + values.Remove(values.Length - 1) + " );\r";
            }
            query = "BEGIN \r" + query;
            query += "END; \r";
            return query;
        }
        #endregion

        #region CustomerDataTableToStrUpdate 
        /// <summary>
        /// CustomerDataTableToStrUpdate
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TableName"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public string CustomerDataTableToStrUpdate(System.Data.DataTable dt, string TableName,string customerId)
        {
            string header = string.Empty;
            string query = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                header = string.Format("UPDATE {0} \r SET \r", TableName);
                foreach (System.Data.DataColumn item in dt.Columns)
                {
                    var columnName = item.ColumnName;
                    if (!string.Equals(columnName, "customerId"))
                        header = string.Format("{0} {1} ='{2}' ,\r", header, item.ColumnName, dt.Rows[i][item.ColumnName].ToString());
                }

                header = string.Format("{0} isModify ='Y' \r", header);
                header = string.Format("{0} WHERE customerId ='{1}' ;\r", header, customerId);  

               
            }
            query = string.Format("BEGIN \r {0} END; \r ", header);
            return query;
        }
        #endregion

        #region ImagesFileToBytes
        /// <summary>
        /// ImagesFileToBytes
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="detailsDt"></param>
        /// <returns></returns>
        private System.Data.DataTable ImagesFileToBytes(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo,
                                                        System.Data.DataTable detailsDt)
        {

            string logMessage = string.Empty;
            if (!detailsDt.Columns.Contains("base64Str"))
                detailsDt.Columns.Add("base64Str", typeof(string));
            if (!detailsDt.Columns.Contains("images"))
                detailsDt.Columns.Add("images", typeof(byte[]));
            if (!detailsDt.Columns.Contains("isImagesCompleted"))  //下载完成
                detailsDt.Columns.Add("isImagesCompleted", typeof(string));
            if (!detailsDt.Columns.Contains("fileType"))  //下载完成
                detailsDt.Columns.Add("fileType", typeof(string));
            
            //Ftp图片路径
            if (detailsDt.Columns.Contains("imageFtpPath"))
                detailsDt = ImagesFtpToBytes(logAppendToForms, jobInfo, detailsDt);
            //本地图片路径
            else if (detailsDt.Columns.Contains("imageFilePath"))
                detailsDt = ImagesFilePathToBytes(logAppendToForms, jobInfo, detailsDt);
            //二进制文件
            else if (detailsDt.Columns.Contains("imageByte"))
                detailsDt = ImagesByteToBytes(logAppendToForms, jobInfo, detailsDt);
            //网页文件
            else if (detailsDt.Columns.Contains("imageUrl"))
                detailsDt = ImagesUrlFileToBytes(logAppendToForms, jobInfo, detailsDt);

            return detailsDt;
        }
        #endregion

        #region ImagesFtpToBytes  Ftp模式
        /// <summary>
        /// ImagesFtpToBytes  Ftp模式
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private System.Data.DataTable ImagesFtpToBytes(Log4netUtil.LogAppendToForms logAppendToForms,
                                                        Model.JobEntity jobInfo,
                                                        System.Data.DataTable dataTable)
        {
            foreach (System.Data.DataRow dataRow in dataTable.Rows)
            {
                string logMessage = string.Empty;
                string extension = string.Empty;
                string ftpFilePath = dataRow["imageFtpPath"].ToString().Trim();
                string fileSaveName = string.Format("{0}{1}", Guid.NewGuid().ToString(), System.IO.Path.GetExtension(ftpFilePath));
                string fileSavePath = string.Format("{0}\\Temp\\{1}\\FtpTemp", System.Windows.Forms.Application.StartupPath.ToString(), string.Format("{0:yyyyMMdd}", DateTime.Now));
                string result = FtpDownloadFile(logAppendToForms, jobInfo, ftpFilePath, fileSavePath, fileSaveName,out extension);
                Newtonsoft.Json.Linq.JObject resultjObject = Newtonsoft.Json.Linq.JObject.Parse(result);
                if (string.Equals(resultjObject["code"].ToString(), "0000"))
                {
                    string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileSaveName);
                    fileSaveName = string.Format("{0}{1}", fileNameWithoutExtension, extension);
                    string printPathName = string.Format("{0}\\{1}", fileSavePath, fileSaveName);
                    if (System.IO.File.Exists(printPathName))
                    {
                        try
                        {
                            string base64Str = ImageFile2Base64(printPathName);
                            dataRow["base64Str"] = base64Str;
                            dataRow["isImagesCompleted"] = "Y";
                            dataRow["fileType"] = extension;
                            /* using (System.IO.FileStream fs = new System.IO.FileStream(printPathName, System.IO.FileMode.Open, System.IO.FileAccess.Read))  //读取文件  Image  
                             {
                                 using (System.IO.BinaryReader br = new System.IO.BinaryReader(fs))
                                 {
                                     dataRow["images"] = br.ReadBytes((int)br.BaseStream.Length);
                                     byte[] bytes = br.ReadBytes((int)br.BaseStream.Length);
                                     //string base64 = Convert.ToBase64String(bytes);
                                     dataRow["base64Str"] = Convert.ToBase64String(bytes) ;// Util.Base64Util.EncodeBase64_UTF8(dataRow["images"].ToString());
                                     dataRow["isImagesCompleted"] = "Y";
                                 }
                             }*/
                        }
                        catch (Exception ex)
                        {
                            logMessage = string.Format("【{0}_{1}】  FTP下载文件失败！读取不了文件{2} ;原因:{3}找不到文件", jobInfo.JobCode, jobInfo.JobName, printPathName, ex.Message);
                            LogError(logAppendToForms, true, logMessage, @"Ftp");
                        }
                    }
                    else
                    {
                        logMessage = string.Format("【{0}_{1}】  FTP下载文件失败！;原因:{2}找不到文件", jobInfo.JobCode, jobInfo.JobName, printPathName);
                        LogError(logAppendToForms, true, logMessage, @"Ftp");
                    }
                }
                else if (string.Equals(resultjObject["code"].ToString(), "9998"))
                    break;
                else
                    continue;

            }
            return dataTable;
        }
        #endregion

        #region ImagesFilePathToBytes 本地图片模式
        /// <summary>
        /// ImagesFilePathToBytes 本地图片模式
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="configInfo"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private System.Data.DataTable ImagesFilePathToBytes(Log4netUtil.LogAppendToForms logAppendToForms,
                                                        Model.JobEntity jobInfo,
                                                        System.Data.DataTable dataTable)
        {
            foreach (System.Data.DataRow dataRow in dataTable.Rows)
            {
                string logMessage = string.Empty;
                string imageFilePath = dataRow["imageFilePath"].ToString().Trim();
                if (System.IO.File.Exists(imageFilePath))
                {
                    try
                    {
                        dataRow["base64Str"] = ImageFile2Base64(imageFilePath);
                        dataRow["isImagesCompleted"] = "Y";
                        /*using (System.IO.FileStream fs = new System.IO.FileStream(imageFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))  //读取文件  Image  
                        {
                            using (System.IO.BinaryReader br = new System.IO.BinaryReader(fs))
                            {
                                dataRow["images"] = br.ReadBytes((int)br.BaseStream.Length);
                                dataRow["base64Str"] = Util.Base64Util.EncodeBase64_UTF8(dataRow["images"].ToString());
                                dataRow["isImagesCompleted"] = "Y";
                            }
                        }*/
                    }
                    catch (Exception ex)
                    {
                        logMessage = string.Format("【{0}_{1}】  本地文件读取失败！读取不了文件{2} ;原因:{3}", jobInfo.JobCode, jobInfo.JobName, imageFilePath, ex.Message);
                        LogError(logAppendToForms, true, logMessage, @"Ftp");
                    }

                }
                else
                {
                    logMessage = string.Format("【{0}_{1}】  本地文件读取失败！原因: {2}找不到! ", jobInfo.JobCode, jobInfo.JobName, imageFilePath);
                    LogError(logAppendToForms, true, logMessage, @"Ftp");
                }
            }
            return dataTable;
        }
        #endregion

        #region ImagesByteToBytes 二进制模式
        /// <summary>
        /// ImagesByteToBytes 二进制模式
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private System.Data.DataTable ImagesByteToBytes(Log4netUtil.LogAppendToForms logAppendToForms,
                                                        Model.JobEntity jobInfo,
                                                        System.Data.DataTable dataTable)
        {
            foreach (System.Data.DataRow dataRow in dataTable.Rows)
            {
                string logMessage = string.Empty;
                string imageByte = dataTable.Columns.Contains("imageByte") ? dataRow["imageByte"].ToString().Trim() : string.Empty;

                try
                {
                    Type dataType = dataRow["imageByte"].GetType();
                    byte[] imagebytes;
                    if (string.Equals(dataType.Name, "String"))
                        imagebytes = System.Text.Encoding.Default.GetBytes(imageByte); //(byte[])detailsDr["ImageByte"]; // System.Text.Encoding.Default.GetBytes(imageByte);
                    else
                        imagebytes = (byte[])dataRow["imageByte"];

                    //dataRow["images"] = imagebytes;
                    dataRow["base64Str"] = Convert.ToBase64String(imagebytes);//Util.Base64Util.EncodeBase64_UTF8(dataRow["images"].ToString());
                    dataRow["isImagesCompleted"] = "Y";
                }
                catch (Exception ex)
                {
                    logMessage = string.Format("【{0}_{1}】  二进制转换图片失败！;原因:{2} ", jobInfo.JobCode, jobInfo.JobName, ex.Message);
                    LogError(logAppendToForms, true, logMessage, @"Ftp");
                }
            }
            return dataTable;
        }
        #endregion

        #region ImagesUrlFileToBytes 网页文件 模式
        /// <summary>
        /// ImagesUrlFileToBytes 网页文件 模式
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="configInfo"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private System.Data.DataTable ImagesUrlFileToBytes(Log4netUtil.LogAppendToForms logAppendToForms,
                                                        Model.JobEntity jobInfo,
                                                        System.Data.DataTable dataTable)
        {
            foreach (System.Data.DataRow dataRow in dataTable.Rows)
            {
                string logMessage = string.Empty;
                string imageUrl = dataRow["imageUrl"].ToString().Trim();
                string fileSaveName = string.Format("{0}{1}", Guid.NewGuid().ToString(), System.IO.Path.GetExtension(imageUrl));
                string fileSavePath = string.Format("{0}\\Temp\\{1}\\UrlTemp", System.Windows.Forms.Application.StartupPath.ToString(), string.Format("{0:yyyyMMdd}", DateTime.Now));
                string fileName = string.Format("{0}\\{1}", fileSavePath, fileSaveName);
                //创建文件夹
                if (!System.IO.Directory.Exists(fileSavePath))//如果不存在就创建 dir 文件夹  
                    System.IO.Directory.CreateDirectory(fileSavePath);
                if (Util.ImageDownloadPrint.ImagePrint(logAppendToForms, jobInfo, imageUrl, fileName))
                {
                    if (System.IO.File.Exists(fileName))
                    {
                        try
                        {
                            dataRow["base64Str"] = ImageFile2Base64(fileName);
                            dataRow["isImagesCompleted"] = "Y";
                            /*using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))  //读取文件  Image  
                            {
                                using (System.IO.BinaryReader br = new System.IO.BinaryReader(fs))
                                {
                                    dataRow["images"] = br.ReadBytes((int)br.BaseStream.Length);
                                    dataRow["base64Str"] = Util.Base64Util.EncodeBase64_UTF8(dataRow["images"].ToString());
                                    dataRow["isImagesCompleted"] = "Y";
                                }
                            }*/
                        }
                        catch (Exception ex)
                        {
                            logMessage = string.Format("【{0}_{1}】  Url下载文件失败，读取不了文件{2};原因:{3} ", jobInfo.JobCode, jobInfo.JobName, fileName, ex.Message);
                            LogError(logAppendToForms, true, logMessage, @"Ftp");
                        }
                    }
                    else
                    {
                        logMessage = string.Format("【{0}_{1}】  Url下载文件失败，原因:{2} 找不到 ", jobInfo.JobCode, jobInfo.JobName, fileName);
                        LogError(logAppendToForms, true, logMessage, @"Ftp");
                    }
                }
            }
            return dataTable;
        }
        #endregion

        #region ImageFile2Base64
        /// <summary>
        /// ImageFile2Base64
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        private string ImageFile2Base64(string imageFile)
        {
            /*Image image = Image.FromFile(imageFile);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, image.RawFormat);
            byte[] byteArray = ms.ToArray();
            ms.Close();
            return Convert.ToBase64String(byteArray);*/

            Bitmap bmp = new Bitmap(imageFile);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] byteArray = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(byteArray, 0, (int)ms.Length);
            ms.Close();
            bmp.Dispose();
            bmp = null;
            return Convert.ToBase64String(byteArray);
        }
        #endregion 

        #region FtpDownloadFile
        /// <summary>
        /// FtpDownloadFile
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="ftpfilepath"></param>
        /// <param name="fileSavePath"></param>
        /// <param name="fileSavaName"></param>
        /// <param name="billCode"></param>
        /// <returns></returns>
        private string FtpDownloadFile(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo,
                                     string ftpfilepath, string fileSavePath, string fileSavaName,out string extension)
        {
            string logMessage = string.Empty;
            Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
            try
            {
                BLLFactory.FactoryBLL bllfact = new BLLFactory.FactoryBLL();
                Facade.ICommonBLL ibll = bllfact.CreateCommonBLL();
                string result = ibll.FtpDownloadToFile(logAppendToForms, jobInfo, ftpfilepath, fileSavePath, fileSavaName, out extension);
                Newtonsoft.Json.Linq.JObject resultjObject = Newtonsoft.Json.Linq.JObject.Parse(result);
                if (string.Equals(resultjObject["code"].ToString(), "0000"))
                {
                    jObject.Add("code", "0000");
                    jObject.Add("msg", "Ftp下载成功!");
                    jObject.Add("data", string.Format(" Ftp文件:{0} ,下载后路径:{1} 文件名:{2}", ftpfilepath, fileSavePath, fileSavaName));
                    logMessage = string.Format("【{0}_{1}】  Ftp下载成功！ Ftp文件:{2} ,下载后路径:{3} 文件名:{4}", jobInfo.JobCode, jobInfo.JobName, ftpfilepath, fileSavePath, fileSavaName);
                    LogMessage(logAppendToForms, jobInfo.IsDebug, logMessage, @"Ftp");
                    return jObject.ToString();
                }
                else
                {
                    jObject.Add("code", resultjObject["code"].ToString());
                    jObject.Add("msg", resultjObject["msg"].ToString());
                    jObject.Add("data", string.Format("Ftp下载失败！ 原因:{0} ;Ftp文件:{1} ", resultjObject["msg"].ToString(), ftpfilepath));
                    logMessage = string.Format("【{0}_{1}】  Ftp下载失败！ 原因:{2} ;Ftp文件:{3} ", jobInfo.JobCode, jobInfo.JobName, resultjObject["msg"].ToString(),ftpfilepath);
                    LogError(logAppendToForms, true, logMessage, @"Ftp");
                    return jObject.ToString();
                }

            }
            catch (Exception ex)
            {
                extension = string.Empty;
                jObject.Add("code", "9999");
                jObject.Add("msg", ex.Message);
                jObject.Add("data", string.Format("Ftp下载失败！ 原因:{0} ;Ftp文件:{1} ", ex.Message, ftpfilepath));
                logMessage = string.Format("【{0}_{1}】  Ftp下载失败！ 原因:{2} ;Ftp文件:{3} ", jobInfo.JobCode, jobInfo.JobName, ex.Message, ftpfilepath);
                LogError(logAppendToForms, true, logMessage, @"Ftp");
                return jObject.ToString();
            }
        }
        #endregion

        #region UpdateOrderList
        /// <summary>
        /// UpdateOrderList
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="argDataTable"></param>
        /// <param name="orderCode"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private System.Data.DataTable UpdateOrderList(Log4netUtil.LogAppendToForms logAppendToForms,
                                                      Model.JobEntity jobInfo,
                                                      System.Data.DataTable argDataTable, string orderCode, string orderId)
        {
            string logMessage = string.Empty;
            System.Data.DataTable dtResult = new System.Data.DataTable();
            //克隆表结构
            dtResult = argDataTable.Clone();
            if (!dtResult.Columns.Contains("orderId"))
                dtResult.Columns.Add("orderId", typeof(string));
            if (!dtResult.Columns.Contains("orderRows"))
                dtResult.Columns.Add("orderRows", typeof(int));
            if (!dtResult.Columns.Contains("orderCode"))
                dtResult.Columns.Add("orderCode", typeof(string));
            if (!dtResult.Columns.Contains("erpGoodsIdlocal"))
                dtResult.Columns.Add("erpGoodsIdlocal", typeof(string));

            try
            {
                //修改数据列类型
                foreach (System.Data.DataColumn col in dtResult.Columns)
                {
                    if (col.ColumnName == "originalAmount")//原金额
                        col.DataType = typeof(decimal);
                    if (col.ColumnName == "originalPrice")//原价格
                        col.DataType = typeof(decimal);
                    if (col.ColumnName == "paymentAmount")//结算金额
                        col.DataType = typeof(decimal);
                    if (col.ColumnName == "paymentPrice")//结算价
                        col.DataType = typeof(decimal);
                    if (col.ColumnName == "quantity")//数量
                        col.DataType = typeof(decimal);
                }

                int orderRows = 0;
                foreach (System.Data.DataRow row in argDataTable.Rows)
                {
                    orderRows++;
                    System.Data.DataRow rowNew = dtResult.NewRow();
                    foreach (System.Data.DataColumn item in argDataTable.Columns)
                    {
                        string columnName = item.ColumnName;
                        if (string.Equals(columnName, "originalAmount"))//原金额
                            rowNew[columnName] = Util.ConvertHelper.ConvertStringToDecimal(row[columnName].ToString());
                        else if (string.Equals(columnName, "originalPrice")) //原价格
                            rowNew[columnName] = Util.ConvertHelper.ConvertStringToDecimal(row[columnName].ToString());
                        else if (string.Equals(columnName, "paymentAmount")) //结算金额
                            rowNew[columnName] = Util.ConvertHelper.ConvertStringToDecimal(row[columnName].ToString());
                        else if (string.Equals(columnName, "paymentPrice")) //结算价
                            rowNew[columnName] = Util.ConvertHelper.ConvertStringToDecimal(row[columnName].ToString());
                        else if (string.Equals(columnName, "quantity")) //数量
                            rowNew[columnName] = Util.ConvertHelper.ConvertStringToInt(row[columnName].ToString());
                        else
                            rowNew[columnName] = row[columnName];
                    }
                    rowNew["orderId"] = orderId;
                    rowNew["orderRows"] = orderRows;
                    rowNew["orderCode"] = orderCode;
                    rowNew["erpGoodsIdlocal"] = rowNew["erpGoodsId"];
                    dtResult.Rows.Add(rowNew);

                }

                return dtResult;
            }
            catch (Exception ex)
            {
                logMessage = string.Format("【{0}_{1}】  订单Id:{2} ; UpdateOrderList转换字段类型失败！ 原因：{3}", jobInfo.JobCode, jobInfo.JobName, orderId, ex.Message);
                LogError(logAppendToForms, true, logMessage, jobInfo.JobCode);
                return null;
            }

        }
        #endregion

        #region UpdateOrderMain
        /// <summary>
        /// UpdateOrderMain
        /// </summary>
        /// <param name="argDataTable"></param>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        private System.Data.DataTable UpdateOrderMain(Log4netUtil.LogAppendToForms logAppendToForms,
                                                      Model.JobEntity jobInfo,
                                                      System.Data.DataTable argDataTable,string orderCode,string orderId)
        {
            string logMessage = string.Empty;
            System.Data.DataTable dtResult = new System.Data.DataTable();
            //克隆表结构
            dtResult = argDataTable.Clone();
            if (!dtResult.Columns.Contains("orderCode"))
                dtResult.Columns.Add("orderCode", typeof(string));

            try
            {
                //修改数据列类型
                foreach (System.Data.DataColumn col in dtResult.Columns)
                {
                    if (col.ColumnName == "freePostAmount")//免邮起送金额
                        col.DataType = typeof(decimal);
                    if (col.ColumnName == "paymentAmount")//付款金额
                        col.DataType = typeof(decimal);
                    if (col.ColumnName == "postPrice")//快递费
                        col.DataType = typeof(decimal);
                    if (col.ColumnName == "totalAmount")//订单金额
                        col.DataType = typeof(decimal);
                }

                foreach (System.Data.DataRow row in argDataTable.Rows)
                {
                    System.Data.DataRow rowNew = dtResult.NewRow();
                    foreach (System.Data.DataColumn item in argDataTable.Columns)
                    {
                        string columnName = item.ColumnName;
                        if (string.Equals(columnName, "freePostAmount"))//免邮起送金额
                            rowNew[columnName] = Util.ConvertHelper.ConvertStringToDecimal(row[columnName].ToString());
                        else if (string.Equals(columnName, "paymentAmount")) //付款金额
                            rowNew[columnName] = Util.ConvertHelper.ConvertStringToDecimal(row[columnName].ToString());
                        else if (string.Equals(columnName, "postPrice")) //快递费
                            rowNew[columnName] = Util.ConvertHelper.ConvertStringToDecimal(row[columnName].ToString());
                        else if (string.Equals(columnName, "totalAmount")) //订单金额
                            rowNew[columnName] = Util.ConvertHelper.ConvertStringToDecimal(row[columnName].ToString());
                        else
                            rowNew[columnName] = row[columnName];
                    }
                    rowNew["orderCode"] = orderCode;
                    dtResult.Rows.Add(rowNew);
                }

                return dtResult;
            }catch(Exception ex)
            {
                logMessage = string.Format("【{0}_{1}】  订单Id:{2} ; UpdateOrderMain转换字段类型失败！ 原因：{3}", jobInfo.JobCode, jobInfo.JobName, orderId,ex.Message);
                LogError(logAppendToForms, true, logMessage, jobInfo.JobCode);
                return null;
            }
        }
        #endregion

        #region  ComparisonDate
        /// <summary>
        /// ComparisonDate
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <returns></returns>
        private int ComparisonDate(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, string endDate) //,string strDateTime)
        {
            DateTime dateEnd, dateNow;

            try
            {

                string strDateNow = string.Format("{0:yyyyMMddHHmmss}", System.DateTime.Now);
                //DateTime dateNet = DateTime.ParseExact(strDateNow, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                string strDateTime = endDate.Replace("-", string.Empty).Replace(" ", string.Empty).Replace(":",string.Empty);
                dateEnd = DateTime.ParseExact(strDateTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);  //授权期时间
                dateNow = DateTime.ParseExact(strDateNow, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture); ; //当前互联网时间

                if (DateTime.Compare(dateEnd, dateNow) > 0)
                    return 1;// DateTime.Compare(dateEnd, dateNow);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("比较日期失败，原因:{0} ", ex.Message);
                Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, true, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                return -1;
            }
        }
        #endregion

        #region OpenUrlDownloadFile 打开网址并下载文件
        private  bool OpenUrlDownloadFile(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo,string url,string filename)
        {
            System.Net.HttpWebRequest Myrq = null;
            System.Net.HttpWebResponse myrp = null;
            System.IO.Stream st = null;
            System.IO.Stream so = null;
            try
            {
                Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                st = myrp.GetResponseStream();
                so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    so.Write(by, 0, osize);
                    osize = st.Read(by, 0, (int)by.Length);
                }
                so.Close();
                st.Close();
                myrp.Close();
                Myrq.Abort();
                return true;
            }
            catch (Exception e)
            {
                string strMsg = string.Format("Url:{0} ; filename :{1} 下载文件失败 原因：{2}", url, filename, e.Message);
                LogError(logAppendToForms, true, strMsg, jobInfo.JobCode);
                return false;
            }
            finally
            {
                if (so != null)
                    so.Close();
                if (st != null)
                    st.Close();
                if (myrp != null)
                    myrp.Close();
                if (Myrq != null)
                    Myrq.Abort();
            }
        }
        #endregion

        #region LogMessage
        /// <summary>
        /// LogMessage
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="isDebug"></param>
        /// <param name="logMessage"></param>
        /// <param name="jobType"></param>
        private void LogMessage(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
            if (isDebug)
                Log4netUtil.LogDisplayHelper.LogMessage(logAppendToForms, logMessage);
        }
        #endregion

        #region LogWarning
        /// <summary>
        /// LogWarning
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="isDebug"></param>
        /// <param name="logMessage"></param>
        /// <param name="jobType"></param>
        private void LogWarning(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
            Log4netUtil.Log4NetHelper.Warn(logMessage, "QuartzManager");
            if (isDebug)
                Log4netUtil.LogDisplayHelper.LogWarning(logAppendToForms, logMessage);
        }
        #endregion

        #region LogError
        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="isDebug"></param>
        /// <param name="logMessage"></param>
        /// <param name="jobType"></param>
        private void LogError(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
            Log4netUtil.Log4NetHelper.Error(logMessage, "QuartzManager");
            if (isDebug)
                Log4netUtil.LogDisplayHelper.LogError(logAppendToForms, logMessage);
        }
        #endregion


    }

}

