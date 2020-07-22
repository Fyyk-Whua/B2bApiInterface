using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
//using System.Data.OracleClient;
using IDAL;

namespace DBUtility
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: DBHelper.cs   
    ///* 命名空间: DALFactory
    ///* 功    能: DBHelper抽象层定义
    ///* 内    容: 
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 版权说明:   Copyright 2018-2027 
    ///*
    ///**************************************************************************/
    
    public abstract class DBHelper : IDBHelper
    {
       
        #region  连接数据库
        /// <summary>
        /// 
        /// </summary>
        protected static DbConnection _ErpCon;
        protected static DbConnection _WmsCon;
        private string _DataBaseType = String.Empty;
        private string _ConnectionType = String.Empty;

        /// <summary>
        /// CreateConnection
        /// </summary>
        /// <param name="connectionType"></param>
        public void CreateConnection(string connectionType)
        {
            string conStr = "";
            _ConnectionType = connectionType;
            if (_DataBaseType == String.Empty)
                _DataBaseType = GetDbTypeFromConfig(connectionType);


            string erpDataBaseType = Util.DalConst.ErpDBType;
            string wmsDataBaseType = Util.DalConst.WmsDBType;

            switch (erpDataBaseType)
            {
                case "MySql":
                    _ErpCon = new MySqlConnection(Util.DalConst.getMySqlConnection(0, "Erp"));
                    break;
                case "SqlServer":
                    _ErpCon = new SqlConnection(Util.DalConst.getSqlServerConnection(0, "Erp"));
                    break;
                case "Oracle":
                    _ErpCon = new OracleConnection(Util.DalConst.getOracleConnection(0, "Erp"));
                    break;
                default:
                    _ErpCon = new SqlConnection(conStr);
                    break;
            }

            switch (wmsDataBaseType)
            {
                case "MySql":
                    _WmsCon = new MySqlConnection(Util.DalConst.getMySqlConnection(0, "Wms"));
                    break;
                case "SqlServer":
                    _WmsCon = new SqlConnection(Util.DalConst.getSqlServerConnection(0, "Wms"));
                    break;
                case "Oracle":
                    _WmsCon = new OracleConnection(Util.DalConst.getOracleConnection(0, "Wms"));
                    break;
                default:
                    _WmsCon = new SqlConnection(conStr);
                    break;
            }

        }
        
        /// <summary>
        /// CreateConnection
        /// </summary>
        /// <param name="dataBaseType"></param>
        public void CreateConnection(string connectionType, string dataBaseType)
        {
            string conStr = "";
            _DataBaseType = dataBaseType;
            _ConnectionType = connectionType;
            if (string.Equals(_ConnectionType, "Erp"))
            {
                switch (_DataBaseType)
                {
                    case "MySql":
                        _ErpCon = new MySqlConnection(Util.DalConst.getMySqlConnection(0, connectionType));
                        break;
                    case "SqlServer":
                        _ErpCon = new SqlConnection(Util.DalConst.getSqlServerConnection(0, connectionType));
                        break;
                    case "Oracle":
                        _ErpCon = new OracleConnection(Util.DalConst.getOracleConnection(0, connectionType));
                        break;
                    default:
                        _ErpCon = new SqlConnection(conStr);
                        break;
                }
            }

            if (string.Equals(_ConnectionType, "Wms"))
            {
                switch (_DataBaseType)
                {
                    case "MySql":
                        _WmsCon = new MySqlConnection(Util.DalConst.getMySqlConnection(0, connectionType));
                        break;
                    case "SqlServer":
                        _WmsCon = new SqlConnection(Util.DalConst.getSqlServerConnection(0, connectionType));
                        break;
                    case "Oracle":
                        _WmsCon = new OracleConnection(Util.DalConst.getOracleConnection(0, connectionType));
                        break;
                    default:
                        _WmsCon = new SqlConnection(conStr);
                        break;
                }
            }
            /*switch (dataBaseType)
            {
                case "MySql":
                    _Con = new MySqlConnection(Util.DalConst.getMySqlConnection(1, _ConnectionType));
                    break;
                case "SqlServer":
                    _Con = new SqlConnection(Util.DalConst.getSqlServerConnection(1, _ConnectionType));
                    break;
                case "Oracle":
                    _Con = new OracleConnection(Util.DalConst.getOracleConnection(1, _ConnectionType));
                    break;
                default :
                    _Con = new SqlConnection(conStr);
                    break;
            }*/
        }
        #endregion

        #region ConnectionTestInfo 测式数据库连接
        /// <summary>
        /// ConnectionTestInfo
        /// </summary>
        /// <returns></returns>
        public bool ConnectionTestInfo(string connectionType)
        {
            CreateConnection(connectionType);
            try
            {
                if (string.Equals(connectionType, "Erp"))
                {
                    _ErpCon.Open();
                    if (_ErpCon.State == ConnectionState.Open)
                        return true;
                }
                else if (string.Equals(connectionType, "Wms"))
                {
                    _WmsCon.Open();
                    if (_WmsCon.State == ConnectionState.Open)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("数据库连接失败," + ex.Message, _DataBaseType);
                //return false;
            }
        }
        #endregion

        #region 数据库接口类
        public abstract bool ConnectTestInfo(string connectionType);

        public abstract int ExecuteNonQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract DataSet ExecuteQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract DataSet ExecuteQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract DataTable ExecuteNQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract DataTable ExecuteNQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract DataTable ExecutePQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract List<List<string>> ExecuteLQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract List<List<string>> ExecuteLQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract DbDataReader ExecuteReader(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract object ExecuteScalar(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);

        public abstract bool BulkCopyInsert(DataTable dt, string rowguid, string TableName);

        #endregion

        #region 从配置节读出使用的数据库类型
        /// <summary>
        /// 从配置节读出使用的数据库类型
        /// </summary>
        /// <returns></returns>
        private string GetDbTypeFromConfig(string connectionType)
        {
            if(string.Compare(connectionType,"Erp")==0)
                return Util.DalConst.ErpDBType;
            else 
                return Util.DalConst.WmsDBType;
        }
        #endregion


        #region DbParameter

        #region CreateParam
        /// <summary>
        /// CreateParam
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public DbParameter CreateParam(string SystemType,string paramName, DbType dbType, object value, int size, ParameterDirection direction)
        {
            DbParameter param;
            string strParamName = "";

            //添加
            if (_DataBaseType == String.Empty)
                _DataBaseType = GetDbTypeFromConfig(SystemType);

            strParamName = DataBaseReplaceParamName(_DataBaseType, paramName);
            param = null;
            switch (_DataBaseType)
            {
                case "MySql":
                    param = new MySqlParameter(strParamName, value);
                    break;
                case "SqlServer":
                    param = new SqlParameter(strParamName, value);
                    break;
                case "Oracle":
                    if (direction == ParameterDirection.Output)   ///处理输出数据集的问题 
                    {
                        param = new OracleParameter(strParamName, OracleDbType.RefCursor);
                        param.Direction = direction;
                    }
                    else
                    {
                        param = new OracleParameter(strParamName, value);
                        param.Direction = direction;
                    }
                    if (!Convert.IsDBNull(value))
                        param.Value = value;
                    if (size > 0)
                        param.Size = size;
                    if (DbType.Object != dbType)
                        param.DbType = dbType;
                    break;
                default:
                    strParamName = paramName;
                    param = new SqlParameter(strParamName, value);

                    break;
            }
            return param;

        }
        #endregion

        #region CreateInParam
        /// <summary>
        /// CreateInParam
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DbParameter CreateInParam(string SystemType, string paramName, DbType dbType, object value, int size)
        {
            return CreateParam(SystemType, paramName, dbType, value, size, ParameterDirection.Input);
        }

        public DbParameter CreateInParam(string SystemType, string paramName, DbType dbType, object value)
        {
            return CreateParam(SystemType, paramName, dbType, value, 0, ParameterDirection.Input);
        }

        public DbParameter CreateInParam(string SystemType, string paramName, object value)
        {
            return CreateParam(SystemType, paramName, DbType.Object, value, 0, ParameterDirection.Input);
        }
        #endregion

        #region CreateOutParam
        public DbParameter CreateOutParam(string SystemType, string paramName, DbType dbType, int size)
        {
            return CreateParam(SystemType, paramName, dbType, null, size, ParameterDirection.Output);
        }

        public DbParameter CreateOutParam(string SystemType, string paramName, DbType dbType)//(string paramName, DbType dbType)
        {
            return CreateParam(SystemType, paramName, dbType, null, 0, ParameterDirection.Output);
        }
        #endregion

        #region CreateReturnParam
        /// <summary>
        /// CreateReturnParam
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DbParameter CreateReturnParam(string SystemType, string paramName, DbType dbType, int size)
        {
            return CreateParam(SystemType, paramName, dbType, null, size, ParameterDirection.ReturnValue);
        }

        public DbParameter CreateReturnParam(string SystemType, string paramName, DbType dbType)
        {
            return CreateParam(SystemType,paramName, dbType, null, 0, ParameterDirection.ReturnValue);
        }

        #endregion

        #endregion


        #region  PrepareCon 获取一个Connection对象
        public  DbConnection PrepareCon()
        {
            string conStr = "";
            DbConnection _Con = null;
            switch (_DataBaseType)
            {
                case "MySql":
                    _Con = new MySqlConnection(Util.DalConst.getMySqlConnection(0,_ConnectionType));
                    break;
                case "SqlServer":
                    _Con = new SqlConnection(Util.DalConst.getSqlServerConnection(0, _ConnectionType));
                    break;
                case "Oracle":
                    _Con = new OracleConnection(Util.DalConst.getOracleConnection(0, _ConnectionType));
                    break;
                default :
                    _Con = new SqlConnection(conStr);
                    break;
            }
            return _Con;
        }
        #endregion

        #region PrepareCmd 获取一个Command对象
        /// <summary>
        /// 获取一个Command对象
        /// </summary>
        /// <param name="con">数据库的连接</param>
        /// <param name="cmd">表示要对数据源执行的Transact-SQL语句或存储过程</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="trans">事务</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回Command对象</returns>
        public void PrepareCmd(DbCommand cmd, CommandType cmdType, string cmdText, string systemType, DbTransaction trans, params DbParameter[] cmdParams)
        {
            DbConnection _Con = null;
            if (string.Equals(systemType, "Erp"))
            {
                if (_ErpCon == null)
                    CreateConnection(systemType);
                _Con = _ErpCon;
            }
            else if (string.Equals(systemType, "Wms"))
            {
                if (_WmsCon == null)
                    CreateConnection(systemType);
                _Con = _WmsCon;
               
            }
            if (_Con.State != ConnectionState.Open)
            {
                _Con.Open();
            }
            string strCmdText = "";
            if (cmdType == CommandType.Text)
            {
                strCmdText = DataBaseReplaceCmdText("", cmdText);
            }
            else if (cmdType == CommandType.StoredProcedure)
            {
                strCmdText = DataBaseReplaceCmdTextStoredProcedure("", cmdText);
            }
            cmd.Connection = _Con;
            cmd.CommandType = cmdType;
            cmd.CommandText = strCmdText;

            if (trans != null)
                cmd.Transaction = trans;

            if (cmdParams != null)
            {
                cmd.Parameters.AddRange(cmdParams);
            }
        }
        #endregion

        
     
        
        

        #region DataBaseReplaceParamName 处理 不同数据库DbParameter 参数前缀
        /// <summary>
        /// 处理 不同数据库DbParameter 参数前缀  
        /// </summary>
        /// <param name="DataBaseType"></param>
        /// <param name="ParamName">MySql ？ ; SqlServer @ ;  Oracle : </param>
        /// <returns></returns>
        private string DataBaseReplaceParamName(string DataBaseType, string ParamName)
        {
            string retParamName = "";
            //添加
            if (DataBaseType == String.Empty)
                DataBaseType = _DataBaseType;

            switch (DataBaseType)
            {
                case "MySql":
                    if (ParamName != String.Empty)
                        retParamName= ParamName.Replace(":","?");
                    break;
                case "SqlServer":
                    if (ParamName != String.Empty)
                        retParamName= ParamName.Replace(":","@");
                    break;
                case "Oracle":
                    if (ParamName != String.Empty)
                        retParamName= ParamName.Replace(":","");
                    break;
                default:
                    retParamName = ParamName;
                    break;
            }
            return retParamName;
        }
        #endregion

        #region DataBaseReplaceCmdText 处理 不同数据库CmdText sql语句 前缀
        /// <summary>
        ///  处理 不同数据库CmdText语句 前缀
        /// </summary>
        /// <param name="DataBaseType"></param>
        /// <param name="ParamName">MySql ？ ; SqlServer @ ;  Oracle : </param>
        /// <returns></returns>
        private string DataBaseReplaceCmdText(string DataBaseType, string CmdText)
        {
            string retCmdText = "";
            //添加
            if (DataBaseType == String.Empty)
                DataBaseType = _DataBaseType;//GetdbTypeFromConfig();

            switch (DataBaseType)
            {
                case "MySql":
                    if (CmdText != String.Empty)
                        retCmdText = CmdText.Replace(":", "?");
                    break;
                case "SqlServer":
                    if (CmdText != String.Empty)
                        retCmdText = CmdText.Replace(":", "@");
                    break;
                case "Oracle":
                    if (CmdText != String.Empty)
                    {
                        retCmdText = CmdText.Replace(":", ":");
                        if (retCmdText.IndexOf("dbo.FYYK_SplitToTable") != -1)
                        {
                            retCmdText = retCmdText.Replace("dbo.FYYK_SplitToTable", "TABLE(FYYK_SplitToTable");
                            retCmdText = retCmdText.Replace("WHERE 999=999 AND [value]", " ) WHERE  NAME ");
                        }
                    }
                    break;
                default:
                    retCmdText = CmdText;
                    break;
            }
            return retCmdText;
        }
        #endregion

        #region DataBaseReplaceCmdText 处理 不同数据库CmdText 过程语句 前缀
        /// <summary>
        ///  处理 不同数据库CmdText语句 前缀
        /// </summary>
        /// <param name="DataBaseType"></param>
        /// <param name="ParamName">MySql  ; SqlServer @ ;  Oracle  </param>
        /// <returns></returns>
        private string DataBaseReplaceCmdTextStoredProcedure(string DataBaseType, string CmdText)
        {
            string retCmdText = "";
            //添加
            if (DataBaseType == String.Empty)
                DataBaseType =DataBaseType;// GetdbTypeFromConfig();

            switch (DataBaseType)
            {
                case "MySql":
                    if (CmdText != String.Empty)
                        retCmdText = CmdText.Replace(":", "");
                    break;
                case "SqlServer":
                    if (CmdText != String.Empty)
                        retCmdText = CmdText.Replace(":", "@");
                    break;
                case "Oracle":
                    if (CmdText != String.Empty)
                        retCmdText = CmdText.Replace(":", "");
                    break;
                default:
                    retCmdText = CmdText;
                    break;
            }
            return retCmdText;
        }
        #endregion
    }
}
