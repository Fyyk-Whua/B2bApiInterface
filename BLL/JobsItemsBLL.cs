using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Drawing;

namespace BLL
{

    //不允许此 Job 并发执行任务（禁止新开线程执行）
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
                    case "DataApiCustomerList":   ///dev-api/dataApi/customerList 获取新注册客户数据接口
                        ExecuteDataApiCustomerListJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiOrderList":  ///dev-api/dataApi/orderList 获取订单数据接口
                        ExecuteDataApiOrderListJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiOrderOutWarehouse":  ///dev-api/dataApi/orderOutWarehouse 同步订单出库信息接口
                        ExecuteDataApiOrderOutWarehouseJob(logAppendToForms, jobInfo);
                        break;
                    case "DataApiSalesman":   ///dev-api/dataApi/salesman 同步业务员数据接口
                        ExecuteDataApiSalesmanJob(logAppendToForms, jobInfo);
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
            if (commodityDt == null || commodityDt.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  同步商品数据失败！  商品数据为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
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
                commodityItem.firstLevel = Util.DataTableHelper.DataRowContains(dr, "firstLevel");
                commodityItem.formula = Util.DataTableHelper.DataRowContains(dr, "formula");

                commodityItem.isMedicalInstruments = Util.DataTableHelper.DataRowContains(dr, "isMedicalInstruments");  //isMedicalInstruments Add 20200722
                //commodityItem.goodsAttr = Util.DataTableHelper.DataRowContains(dr, "goodsAttr");
                commodityItem.goodsName = Util.DataTableHelper.DataRowContains(dr, "goodsName");
                commodityItem.goodsSpec = Util.DataTableHelper.DataRowContains(dr, "goodsSpec");
                commodityItem.goodsTradeName = Util.DataTableHelper.DataRowContains(dr, "goodsTradeName");
                commodityItem.goodsType = Util.DataTableHelper.DataRowContains(dr, "goodsType");
                commodityItem.majorFunctions = Util.DataTableHelper.DataRowContains(dr, "majorFunctions"); //Add 20200722

                commodityItem.manufacturer = Util.DataTableHelper.DataRowContains(dr, "manufacturer");
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
                commodityItem.secondLevel = Util.DataTableHelper.DataRowContains(dr, "secondLevel");
                commodityItem.sellCtrlAdmin = Util.DataTableHelper.DataRowContains(dr, "sellCtrlAdmin");
                commodityItem.sellCtrlBusinessType = Util.DataTableHelper.DataRowContains(dr, "sellCtrlBusinessType");
                commodityItem.sellState = Util.DataTableHelper.DataRowContainsInt(dr, "sellState");
                commodityItem.storageType = Util.DataTableHelper.DataRowContains(dr, "storageType");
                commodityItem.store = Util.DataTableHelper.DataRowContains(dr, "store");  //store
                commodityItem.suggestedRetailPrice = Util.DataTableHelper.DataRowContains(dr, "suggestedRetailPrice");
                commodityItem.taboo = Util.DataTableHelper.DataRowContains(dr, "taboo");
                commodityItem.untowardEffect = Util.DataTableHelper.DataRowContains(dr, "untowardEffect");
                commodityItem.usageDosage = Util.DataTableHelper.DataRowContains(dr, "usageDosage");
                commodityItem.warnings = Util.DataTableHelper.DataRowContains(dr, "warnings");

                commodityItems.Add(commodityItem);
            }
            if(commodityItems !=null && commodityItems.Count()<=0)
            {
                logMessage = string.Format("【{0}_{1}】  同步商品数据失败！  商品实体为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(commodityItems);
            string resultJson = string.Empty;

            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                string erpGoodsIds = Util.DataTableHelper.GetColumnValues(commodityDt, "erpGoodsId");
                //string writebackInfo = resultJson;
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), erpGoodsIds, resultJson);
            }
            else
                return;

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
            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.CommodityImage> items = new List<Model.CommodityImage>();


            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.CommodityImage item = new Model.CommodityImage();
                string filePath = Util.DataTableHelper.DataRowContains(dr, "filePath");
                item.base64Str = string.Empty;// Util.DataTableHelper.DataRowContains(dr, "base64Str"); 
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                item.fileName = Util.DataTableHelper.DataRowContains(dr, "fileName");
                item.fileType = Util.DataTableHelper.DataRowContains(dr, "fileType");
                item.imageType = Util.DataTableHelper.DataRowContainsInt(dr, "imageType");  //图片类型1药品图片;2正面图;3背面图;4:45度角图;5条形码图;6拆包图


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
                string erpGoodsIds = Util.DataTableHelper.GetColumnValues(dataTable, "erpGoodsId");
                //string writebackInfo = resultJson;
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), erpGoodsIds, resultJson);
            }
            else
                return;

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
            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.CommodityPrice> items = new List<Model.CommodityPrice>();


            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.CommodityPrice item = new Model.CommodityPrice();
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                item.lsj = Util.DataTableHelper.DataRowContains(dr, "lsj");

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
                string erpGoodsIds = Util.DataTableHelper.GetColumnValues(dataTable, "erpGoodsId");
                //string writebackInfo = resultJson;
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), erpGoodsIds, resultJson);
            }
            else
                return;

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
            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.CommodityRepertory> items = new List<Model.CommodityRepertory>();


            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.CommodityRepertory item = new Model.CommodityRepertory();
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId");
                item.repertory = Util.DataTableHelper.DataRowContainsInt(dr, "repertory");

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
                string erpGoodsIds = Util.DataTableHelper.GetColumnValues(dataTable, "erpGoodsId");
                //string writebackInfo = resultJson;
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), erpGoodsIds, resultJson);
            }
            else
                return;

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
            if (customerDt == null || customerDt.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  同步客户数据失败！  客户数据为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
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
                customerItem.qualityPrincipal = Util.DataTableHelper.DataRowContains(dr, "qualityPrincipal");
                customerItem.registeredCapital = Util.DataTableHelper.DataRowContains(dr, "registeredCapital");
                customerItem.scope = Util.DataTableHelper.DataRowContains(dr, "scope");
                customerItem.twoApparatusLicenseCode = Util.DataTableHelper.DataRowContains(dr, "twoApparatusLicenseCode");
                customerItems.Add(customerItem);
            }
            if (customerItems != null && customerItems.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  同步客户数据失败！  客户实体为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(customerItems);
            string resultJson = string.Empty;
            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                string erpIds = Util.DataTableHelper.GetColumnValues(customerDt, "erpId");
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), erpIds, resultJson);
            }
            else
                return;

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

            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                Newtonsoft.Json.Linq.JArray resultJArray = Newtonsoft.Json.Linq.JArray.Parse(resultJObject.Value<string>("data"));
                System.Data.DataTable dataTable =Util.NewtonsoftCommon.ConvertJArrayToDataTable(logAppendToForms, jobInfo, resultJArray);
                if (dataTable == null || dataTable.Rows.Count <= 0)
                {
                    logMessage = string.Format("【{0}_{1}】  {1}失败或无新客户！ 返回data数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    return;
                }
                string insertTableName = jobInfo.InsertTableName;
                if (string.IsNullOrEmpty(insertTableName))
                {
                    logMessage = string.Format("【{0}_{1}】  {1}失败 未定义订单主表！！！", jobInfo.JobCode, jobInfo.JobName);
                    LogWarning(logAppendToForms, true, logMessage, jobLogType);
                    return;
                }
                if (BulkInsertDatabase(logAppendToForms,jobInfo, dataTable, insertTableName))
                {

                }
                else
                {

                }

            }
            else
                return;


        }
        #endregion

        #region ExecuteDataApiOrderListJob    获取订单数据接口
        /// <summary>
        /// ExecuteDataApiOrderListJob  获取新注册客户数据接口
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

            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                Newtonsoft.Json.Linq.JArray orderJArray = Newtonsoft.Json.Linq.JArray.Parse(resultJObject.Value<string>("data"));
                foreach (var item in orderJArray)
                {
                    Newtonsoft.Json.Linq.JObject jObject = (Newtonsoft.Json.Linq.JObject)item;
                    Newtonsoft.Json.Linq.JArray jArrayOrderList = (Newtonsoft.Json.Linq.JArray)item["orderCommodityList"];
                    jObject.Remove("orderCommodityList");
                    Newtonsoft.Json.Linq.JArray jArrayOrder = Util.NewtonsoftCommon.ConvertJsonToJArray(logAppendToForms, jobInfo, jObject.ToString());
                    System.Data.DataTable dataTable = Util.NewtonsoftCommon.ConvertJArrayToDataTable(logAppendToForms, jobInfo, jArrayOrder);
                    if (dataTable == null || dataTable.Rows.Count <= 0)
                    {
                        logMessage = string.Format("【{0}_{1}】  {1} 订单主表失败！ 订单主表数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        return;
                    }
                    string insertTableName = string.Empty;
                    string[] arr = jobInfo.InsertTableName.Split(',');
                    if (arr.Count() > 0)
                        insertTableName = arr[0].ToString();
                    if(string.IsNullOrEmpty(insertTableName))
                    {
                        logMessage = string.Format("【{0}_{1}】  {1}失败 未定义订单主表！！！", jobInfo.JobCode, jobInfo.JobName);
                        LogWarning(logAppendToForms, true, logMessage, jobLogType);
                        return;
                    }
                    if (BulkInsertDatabase(logAppendToForms, jobInfo, dataTable, insertTableName))
                    {
                        System.Data.DataTable itemsDt = Util.NewtonsoftCommon.ConvertJArrayToDataTable(logAppendToForms, jobInfo, jArrayOrderList);
                        if (itemsDt == null || itemsDt.Rows.Count <= 0)
                        {
                            logMessage = string.Format("【{0}_{1}】  {1} 订单商品明细失败！ 订单商品明细数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                            LogWarning(logAppendToForms, true, logMessage, jobLogType);
                            return;
                        }
                        insertTableName = string.Empty;
                        if (arr.Count() > 1)
                            insertTableName = arr[1].ToString();
                        if (string.IsNullOrEmpty(insertTableName))
                        {
                            logMessage = string.Format("【{0}_{1}】  {1} 订单商品明细失失败 未定义订单明细表！！！", jobInfo.JobCode, jobInfo.JobName);
                            LogWarning(logAppendToForms, true, logMessage, jobLogType);
                            return;
                        }
                        
                        if (BulkInsertDatabase(logAppendToForms, jobInfo, itemsDt, insertTableName))
                        {

                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                }

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
            if (dataTable == null || dataTable.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  {1}失败！ 数据为空！！！", jobInfo.JobCode, jobInfo.JobName);
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            List<Model.OrderOutWarehouse> items = new List<Model.OrderOutWarehouse>();

            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                Model.OrderOutWarehouse item = new Model.OrderOutWarehouse();
                item.batchCode = Util.DataTableHelper.DataRowContains(dr, "batchCode"); //批号
                item.erpGoodsId = Util.DataTableHelper.DataRowContains(dr, "erpGoodsId"); //erp商品id
                item.orderId = Util.DataTableHelper.DataRowContains(dr, "orderId"); //订单id
                item.orderItemId = Util.DataTableHelper.DataRowContains(dr, "orderItemId"); //订单明细id
                item.outboundDate = Util.DataTableHelper.DataRowContains(dr, "outboundDate"); //出库日期
                item.outboundQuantity = Util.DataTableHelper.DataRowContains(dr, "outboundQuantity"); //出库数量
                item.outboundTime = Util.DataTableHelper.DataRowContains(dr, "outboundTime"); //出库时间
                item.productionDate = Util.DataTableHelper.DataRowContains(dr, "productionDate"); //生产日期
                item.valDate = Util.DataTableHelper.DataRowContains(dr, "valDate"); //有效期

                //erpOutboundId erp 出库ID
                //erpOutboundItemId 出库明细序号 

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
                string erpOutboundId = Util.DataTableHelper.GetColumnValues(dataTable, "erpOutboundId");
                //string writebackInfo = resultJson;
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), erpOutboundId, resultJson);
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
            if (salesmanDt == null || salesmanDt.Rows.Count <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  同步业务员数据失败！  业务员数据为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
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

                salesmanItems.Add(salesmanItem);
            }
            if (salesmanItems != null && salesmanItems.Count() <= 0)
            {
                logMessage = string.Format("【{0}_{1}】  同步业务员数据失败！  业务员实体为空！！！", jobInfo.JobCode, jobInfo.JobName.ToString());
                LogError(logAppendToForms, true, logMessage, jobLogType);
                return;
            }
            string requestJson = Util.NewtonsoftCommon.SerializeObjToJson(salesmanItems);
            string resultJson = string.Empty;
            if (CallB2bApi(logAppendToForms, jobInfo, requestJson, out resultJson))
            {
                string erpIds = Util.DataTableHelper.GetColumnValues(salesmanDt, "erpId");
                ErpWriteback(logAppendToForms, jobInfo, jobInfo.JobCode.ToString(), erpIds, resultJson);
            }
            else
                return;

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
        #endregion


        #region CallDtyApi
        /// <summary>
        /// CallDtyApi
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
                logMessage = string.Format("【{0}_{1}】 result:{2} 成功！ ", jobInfo.JobCode, jobInfo.JobName.ToString(), result);
                //LogMessage(logAppendToForms, true, logMessage, jobInfo.JobCode);
                Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, jobInfo.IsDebug, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                return true;
            }
            else
            {
                logMessage = string.Format("【{0}_{1}】 result:{2} ", jobInfo.JobCode, jobInfo.JobName.ToString(), result);
                //LogError(logAppendToForms, true, logMessage, jobInfo.JobCode);
                Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, jobInfo.IsDebug, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
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
            return ibll.BulkInsertDatabase(logAppendToForms, jobInfo, dt, insertTableName);
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

