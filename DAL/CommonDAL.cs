using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public  class CommonDAL :IDAL.ICommonDAL
    {
        #region GetDataGridViewHeader  获取DataGrid表头
        /// <summary>
        /// 获取DataGrid表头
        /// </summary>
        /// <param name="DgvName"></param>
        /// <returns></returns>
        public System.Data.DataTable GetDataGridViewHeader(string DgvName)
        {
            string strSql = string.Empty;
            if (DgvName == "frmWmsInterface")
            {
                strSql = "  SELECT  'JobId' as FdName, '任务Id' as FdDesc, 'int' as FdType, '10' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'JobName' as FdName, '任务名称' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'SourceDatabase' as FdName, '源数据库' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'TargetDatabase' as FdName, '目标数据库' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'TargetTable' as FdName, '目标表' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'JobType' as FdName, '任务类型' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'CronExpression' as FdName, 'Cron表达式' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'CronExpressionDescription' as FdName, 'Cron表达式描述' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'IsBeactive' as FdName, '活动状态' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'OrganizationDataSql' as FdName, '组织数据Sql' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'IsOrgDataStoredProcedure' as FdName, '组织数据Sql是否过程' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'WriteBackSql' as FdName, '回写Sql' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'IsWriteBackStoredProcedure' as FdName, '回写Sql是否过程' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'ProcessingSql' as FdName, '处理Sql' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r"
                       + "  UNION ALL  \r"
                       + "  SELECT  'IsProStoredProcedure' as FdName, '处理Sql是否过程' as FdDesc, 'varchar' as FdType, '50' as FdSize, '0' as FdDec,'False' as IsCheckBox, 'True' as IsVisible ,'True' as IsReadOnly \r";       
            }
            IDAL.IDBHelper _idbHelper = DALFactory.DBHelperFactory.CreateInstance("Wms");//创建接口
            System.Data.Common.DbParameter[] cmdParams = null;
            return _idbHelper.ExecuteNQuery(System.Data.CommandType.Text, strSql,"Wms", cmdParams);//调用接口的方法
        }
        #endregion

        #region ConnectTestInfo
        /// <summary>
        /// ConnectTestInfo
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ConnectTestInfo(string type)
        {
            IDAL.IDBHelper _idbHelper = DALFactory.DBHelperFactory.CreateInstance(type);
            return _idbHelper.ConnectionTestInfo(type);
        }
        #endregion

        #region GetDgvDtAll
        /// <summary>
        /// GetDgvDtAll
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public System.Data.DataTable GetDgvDtAll(Log4netUtil.LogAppendToForms logAppendToForms, Model.SearchParam searchParam)
        {
            string storedProcedureName = searchParam.ProcedureName;
            string targetDatabase = searchParam.TargetDatabase;
            string logMessage = string.Empty;
            IDAL.IDBHelper _idbHelper = DALFactory.DBHelperFactory.CreateInstance(targetDatabase);//创建接口
            System.Data.Common.DbParameter[] cmdParams = { _idbHelper.CreateInParam(targetDatabase,":i_ModuleID" ,searchParam.ModuleID ),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_IsMaintain",  searchParam.IsMaintain),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_FilterFlag",searchParam.FilterFlag),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_StartDate",searchParam.StartDate),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_EndDate", searchParam.EndDate),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_Logogram",searchParam.Logogram),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_BillCode",searchParam.BillCode ),
                                                           _idbHelper.CreateOutParam(targetDatabase ,":o_return", System.Data.DbType.Object )
                                                         };
            System.Data.DataTable dt = null;
            try
            {
                dt = _idbHelper.ExecuteNQuery(System.Data.CommandType.StoredProcedure, storedProcedureName, targetDatabase, cmdParams);//调用接口的方法
                string jsonSql = Util.DbSqlLog.SqlToJson("0000", storedProcedureName, cmdParams);
                logMessage = string.Format("GetDgvDtAll  获取数据 执行ExecuteNQuery成功!{0}", string.Empty);
                Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("0000"));
                resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                logMessage = string.Format("【{0}_{1}】 {2}", searchParam.jobInfo.JobCode, searchParam.jobInfo.JobName.ToString(), Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, searchParam.IsDebug, logMessage, @"Database");
            }
            catch (Exception ex)
            {
                string jsonSql = Util.DbSqlLog.SqlToJson("9999", storedProcedureName, cmdParams);
                logMessage = string.Format("GetDgvDtAll  获取数据 执行ExecuteNQuery失败;失败原因：{0}", ex.Message);
                Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("9999"));
                resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                logMessage = string.Format("【{0}_{1}】 {2}", searchParam.jobInfo.JobCode, searchParam.jobInfo.JobName.ToString(), Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, searchParam.IsDebug, logMessage, @"Database");
                dt = null;
            }
            return dt;
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public string ExecuteScalar(Log4netUtil.LogAppendToForms logAppendToForms, Model.SearchParam searchParam)
        {
            string storedProcedureName = searchParam.ProcedureName;
            string targetDatabase = searchParam.TargetDatabase;
            string logMessage = string.Empty;
            IDAL.IDBHelper _idbHelper = DALFactory.DBHelperFactory.CreateInstance(targetDatabase);//创建接口
            System.Data.Common.DbParameter[] cmdParams = { _idbHelper.CreateInParam(targetDatabase,":i_ModuleID" ,searchParam.ModuleID ),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_IsMaintain",  searchParam.IsMaintain),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_FilterFlag",searchParam.FilterFlag),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_StartDate",searchParam.StartDate),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_EndDate", searchParam.EndDate),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_Logogram",searchParam.Logogram),
                                                           _idbHelper.CreateInParam(targetDatabase ,":i_BillCode",searchParam.BillCode ),
                                                           _idbHelper.CreateOutParam(targetDatabase ,":o_return", System.Data.DbType.Object )
                                                         };
            try
            {
                var result = _idbHelper.ExecuteScalar(System.Data.CommandType.StoredProcedure, storedProcedureName, targetDatabase, cmdParams);//调用接口的方法
                string jsonSql = Util.DbSqlLog.SqlToJson("0000", storedProcedureName, cmdParams);
                logMessage = string.Format("ExecuteScalar  获取数据 执行ExecuteScalar成功!{0}", string.Empty);
                Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("0000"));
                resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                logMessage = string.Format("【{0}_{1}】 {2}", searchParam.jobInfo.JobCode, searchParam.jobInfo.JobName.ToString(), Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, searchParam.IsDebug, logMessage, @"Database");
                return result.ToString();
            }
            catch (Exception ex)
            {
                string jsonSql = Util.DbSqlLog.SqlToJson("9999", storedProcedureName, cmdParams);
                logMessage = string.Format("ExecuteScalar  获取数据 执行ExecuteScalar失败;失败原因：{0}", ex.Message);
                Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("9999"));
                resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                logMessage = string.Format("【{0}_{1}】 {2}", searchParam.jobInfo.JobCode, searchParam.jobInfo.JobName.ToString(), Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, searchParam.IsDebug, logMessage, @"Database");
                return string.Empty;
            }

        }
        #endregion

        #region ErpWriteback
        /// <summary>
        /// ErpWriteback
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="writebackParam"></param>
        /// <returns></returns>
        public bool ErpWriteback(Log4netUtil.LogAppendToForms logAppendToForms, Model.WritebackParam writebackParam)
        {
            string logMessage = string.Empty;
            string procedureName = writebackParam.ProcedureName;
            string targetDatabase = writebackParam.TargetDatabase;
            IDAL.IDBHelper _idbHelper = DALFactory.DBHelperFactory.CreateInstance(targetDatabase);//创建接口
            System.Data.Common.DbParameter[] cmdParams = { _idbHelper.CreateInParam(targetDatabase,":i_WritebackType",writebackParam.WritebackType),
                                                           _idbHelper.CreateInParam(targetDatabase,":i_BillCodes", string.IsNullOrEmpty(writebackParam.BillCodes)?string.Empty:writebackParam.BillCodes),
                                                           _idbHelper.CreateInParam(targetDatabase,":i_Type", string.IsNullOrEmpty(writebackParam.Type)?string.Empty:writebackParam.Type),
                                                           _idbHelper.CreateInParam(targetDatabase,":i_Status", writebackParam.Status),
                                                           _idbHelper.CreateInParam(targetDatabase,":i_WritebackInfo", string.IsNullOrEmpty(writebackParam.WritebackInfo)?string.Empty:writebackParam.WritebackInfo)
                                                         };
            try
            {
                writebackParam.IsDebug = true;
                int r = _idbHelper.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, procedureName, targetDatabase, cmdParams);
                if (r > 0)
                {
                    string jsonSql = Util.DbSqlLog.SqlToJson("0000", procedureName, cmdParams);
                    logMessage = string.Format("ErpWriteback回写成功!!!{0}", string.Empty);
                    Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                    resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("0000"));
                    resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                    resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                    logMessage = string.Format("【{0}_{1}】 {2}", writebackParam.jobInfo.JobCode, writebackParam.jobInfo.JobName.ToString(), Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                    Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, writebackParam.IsDebug, logMessage, @"Database");
                    return true;
                }
                else
                {
                    string jsonSql = Util.DbSqlLog.SqlToJson("9999", procedureName, cmdParams);
                    logMessage = string.Format("ErpWriteback回写失败!!!!{0}", string.Empty);
                    Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                    resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("9999"));
                    resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                    resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                    logMessage = string.Format("【{0}_{1}】 {2}", writebackParam.jobInfo.JobCode, writebackParam.jobInfo.JobName.ToString(), Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                    Log4netUtil.Log4NetHelper.LogError(logAppendToForms, writebackParam.IsDebug, logMessage, @"Database");
                    return false;
                }
            }
            catch (Exception ex)
            {
                string jsonSql = Util.DbSqlLog.SqlToJson("9999", procedureName, cmdParams);
                logMessage = string.Format("ErpWriteback回写失败!!!原因：{0};", ex.Message);
                Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("9999"));
                resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                logMessage = string.Format("【{0}_{1}】 {2}", writebackParam.jobInfo.JobCode, writebackParam.jobInfo.JobName.ToString(), Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, writebackParam.IsDebug, logMessage, @"Database");
                return false;
            }
        }
        #endregion


        #region BulkCopyInsert  批量插入数据库
        /// <summary>
        /// BulkCopyInsert  批量插入数据库
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int BulkCopyInsert(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, System.Data.DataTable dt, string tableName)
        {
            string logMessage = string.Empty;
            string strSql = Util.ConvertHelper.DataTableToStrInsert(dt, tableName) + "\r";
            string targetDatabase = jobInfo.TargetDatabase;
            IDAL.IDBHelper _idbHelper = DALFactory.DBHelperFactory.CreateInstance(targetDatabase);//创建接口/创建接口
            System.Data.Common.DbParameter[] cmdParams = null;
            string jsonSql = Util.DbSqlLog.SqlToJson("9999", strSql, cmdParams);
            logMessage = string.Format("【{0}_{1}】 JsonSql：{2} ", jobInfo.JobCode,jobInfo.JobName, jsonSql);
            Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, jobInfo.IsDebug, logMessage, @"Database");
            try
            {
                if (_idbHelper.ExecuteNonQuery(System.Data.CommandType.Text, strSql, targetDatabase, cmdParams) > 0)
                {

                    logMessage = string.Format("【{0}_{1}】  执行BulkCopyInsert成功!", jobInfo.JobCode, jobInfo.JobName);
                    Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                    resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("0000"));
                    resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                    resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                    logMessage = string.Format("【{0}_{1}】 {2}", jobInfo.JobCode, jobInfo.JobName, Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                    Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, jobInfo.IsDebug, logMessage, @"Database");
                    return 1;
                }
                else
                {
                    logMessage = string.Format("【{0}_{1}】  执行BulkCopyInsert失败!", jobInfo.JobCode, jobInfo.JobName);
                    Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                    resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("9999"));
                    resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                    resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                    logMessage = string.Format("【{0}_{1}】 {2}", jobInfo.JobCode, jobInfo.JobName, Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                    Log4netUtil.Log4NetHelper.LogError(logAppendToForms, jobInfo.IsDebug, logMessage, @"Database");
                    return -1;

                }
            }
            catch (Exception ex)
            {
                //违反了 PRIMARY KEY 约束

                logMessage = string.Format("【{0}_{1}】  执行BulkCopyInsert失败! 失败原因:{2}", jobInfo.JobCode, jobInfo.JobName, ex.Message);
                Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("9999"));
                resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                logMessage = string.Format("【{0}_{1}】 {2}", jobInfo.JobCode, jobInfo.JobName, Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, jobInfo.IsDebug, logMessage, @"Database");
                if (ex.Message.Contains("违反了 PRIMARY KEY 约束"))
                    return 2;
                else
                    return -1;
            }

        }
        #endregion

        #region BulkCopyInsert  批量插入数据库
        /// <summary>
        /// BulkCopyInsert  批量插入数据库
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int BulkCopyInsert(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, string tableName,string strSql)
        {
            string logMessage = string.Empty;
            //string strSql = Util.ConvertHelper.DataTableToStrInsert(dt, tableName) + "\r";
            string targetDatabase = jobInfo.TargetDatabase;
            IDAL.IDBHelper _idbHelper = DALFactory.DBHelperFactory.CreateInstance(targetDatabase);//创建接口/创建接口
            System.Data.Common.DbParameter[] cmdParams = null;
            string jsonSql = Util.DbSqlLog.SqlToJson("9999", strSql, cmdParams);
            logMessage = string.Format("【{0}_{1}】 JsonSql：{2} ", jobInfo.JobCode, jobInfo.JobName, jsonSql);
            Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, jobInfo.IsDebug, logMessage, @"Database");
            try
            {
                if (_idbHelper.ExecuteNonQuery(System.Data.CommandType.Text, strSql, targetDatabase, cmdParams) > 0)
                {

                    logMessage = string.Format("【{0}_{1}】  执行BulkCopyInsert成功!", jobInfo.JobCode, jobInfo.JobName);
                    Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                    resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("0000"));
                    resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                    resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                    logMessage = string.Format("【{0}_{1}】 {2}", jobInfo.JobCode, jobInfo.JobName, Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                    Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, jobInfo.IsDebug, logMessage, @"Database");
                    return 1;
                }
                else
                {
                    logMessage = string.Format("【{0}_{1}】  执行BulkCopyInsert失败!", jobInfo.JobCode, jobInfo.JobName);
                    Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                    resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("9999"));
                    resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                    resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                    logMessage = string.Format("【{0}_{1}】 {2}", jobInfo.JobCode, jobInfo.JobName, Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                    Log4netUtil.Log4NetHelper.LogError(logAppendToForms, jobInfo.IsDebug, logMessage, @"Database");
                    return -1;

                }
            }
            catch (Exception ex)
            {
                //违反了 PRIMARY KEY 约束

                logMessage = string.Format("【{0}_{1}】  执行BulkCopyInsert失败! 失败原因:{2}", jobInfo.JobCode, jobInfo.JobName, ex.Message);
                Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("9999"));
                resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                logMessage = string.Format("【{0}_{1}】 {2}", jobInfo.JobCode, jobInfo.JobName, Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, jobInfo.IsDebug, logMessage, @"Database");
                if (ex.Message.Contains("违反了 PRIMARY KEY 约束"))
                    return 2;
                else
                    return -1;
            }

        }
        #endregion

        #region ErpExecuteProcedure
        /// <summary>
        /// ErpExecuteProcedure
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="writebackParam"></param>
        /// <returns></returns>
        public bool ErpExecuteProcedure(Log4netUtil.LogAppendToForms logAppendToForms, Model.WritebackParam writebackParam)
        {
            string logMessage = string.Empty;
            string procedureName = writebackParam.ProcedureName;
            string targetDatabase = writebackParam.TargetDatabase;
            IDAL.IDBHelper _idbHelper = DALFactory.DBHelperFactory.CreateInstance(targetDatabase);//创建接口
            System.Data.Common.DbParameter[] cmdParams = null;
            try
            {
                writebackParam.IsDebug = true;
                int r = _idbHelper.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, procedureName, targetDatabase, cmdParams);
                if (r > 0)
                {
                    string jsonSql = Util.DbSqlLog.SqlToJson("0000", procedureName, cmdParams);
                    logMessage = string.Format(" ErpExecuteProcedure 执行成功!!!{0}", string.Empty);
                    Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                    resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("0000"));
                    resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                    resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                    logMessage = string.Format("【{0}_{1}】 {2}", writebackParam.jobInfo.JobCode, writebackParam.jobInfo.JobName.ToString(), Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                    Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, writebackParam.IsDebug, logMessage, @"Database");
                    return true;
                }
                else
                {
                    string jsonSql = Util.DbSqlLog.SqlToJson("9999", procedureName, cmdParams);
                    logMessage = string.Format("  ErpExecuteProcedure 执行失败!!!!{0}", string.Empty);
                    Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                    resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("9999"));
                    resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                    resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                    logMessage = string.Format("【{0}_{1}】 {2}", writebackParam.jobInfo.JobCode, writebackParam.jobInfo.JobName.ToString(), Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                    Log4netUtil.Log4NetHelper.LogError(logAppendToForms, writebackParam.IsDebug, logMessage, @"Database");
                    return false;
                }
            }
            catch (Exception ex)
            {
                string jsonSql = Util.DbSqlLog.SqlToJson("9999", procedureName, cmdParams);
                logMessage = string.Format("  ErpExecuteProcedure 执行失败!!!原因：{0};", ex.Message);
                Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                resultJObject.Add("code", new Newtonsoft.Json.Linq.JValue("9999"));
                resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
                resultJObject.Add("sql", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(jsonSql)));
                logMessage = string.Format("【{0}_{1}】 {2}", writebackParam.jobInfo.JobCode, writebackParam.jobInfo.JobName.ToString(), Util.NewtonsoftCommon.SerializeObjToJson(resultJObject));
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, writebackParam.IsDebug, logMessage, @"Database");
                return false;
            }
        }
        #endregion


    }
}
