using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using FYYK.Util;

namespace  FYYK.DBUtility
{
    //abstract
    public  class SqlHelper1
    {
        ///*************************************************************************/
        ///*
        ///* 文 件 名: SqlDBHelper.cs   
        ///* 命名空间: FYYK.DBUtility
        ///* 功    能: SqlServer数据访问的抽象基础类。
        ///* 内    容: 
        ///* 原创作者: lau 
        ///* 生成日期: 2018.08.08
        ///* 版 本 号: V1.0.0.0
        ///* 修改日期:
        ///* 版权说明:  Copyright 2016-2017 武汉飞宇益克科技有限公司
        ///*
        ///**************************************************************************/
       
       
        //public static readonly string ConnectionStringProfile = ConfigurationManager.ConnectionStrings["LoggingDb"].ConnectionString;
        internal static readonly string ConnectionStringLocalTransaction = getConnectionSql();

        #region Private functions
        #region getConnectionSql 得到连接字符串
        /// <summary>
        /// 得到连接字符串
        /// </summary>
        /// <returns>连接字符串</returns>
        private static string getConnectionSql()
        {
            string ConfigFile = System.IO.Directory.GetCurrentDirectory() + "\\Config.ini";
            string connectionSql = "";
            string P_str_Server = INIOperationClass.INIGetStringValue(ConfigFile, "SqlServer", "IP", null);    //服务器
            string P_str_DataBase = INIOperationClass.INIGetStringValue(ConfigFile, "SqlServer", "DBNAME", null);  //数据库名称
            string P_str_uid = INIOperationClass.INIGetStringValue(ConfigFile, "SqlServer", "USER", null);   //用户
            string P_str_pwd_Encrypt = INIOperationClass.INIGetStringValue(ConfigFile, "SqlServer", "PWD", null);   //密码
            string P_str_pwd = EncAndDec.MD5Decrypt(P_str_pwd_Encrypt);  //解密

            connectionSql = "Server=" + P_str_Server + ";DataBase=" + P_str_DataBase + ";User ID=" + P_str_uid + ";Password=" + P_str_pwd + ";Persist Security Info=True;  ";
            return connectionSql;
        }
        #endregion

        #region PrepareCommand
        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Demos</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        #endregion
        #endregion

        #region Private functions2

        #region 执行查询，并返回结果集中第一行的第一列。
        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        private static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }
        #endregion

        #region ExecuteReader 执行查询，并返回SqlDataReader。
        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        private static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        #endregion

        #region ExecuteNonQuery 执行一条 Transact-SQL 语句，并返回受影响的行数。

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        private static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }



        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing sql transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        private static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }



        #endregion

        #region ExecuteDataSet 执行查询，并返回DataSet 数据集。
        private static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    conn.Open();
                    SqlDataAdapter cmd = new SqlDataAdapter(cmdText, conn);
                    cmd.Fill(ds, "ds");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    conn.Close();
                }
                return ds;
            }
        }
        #endregion

        #region ExecuteDataTable 执行查询，并返回数据集。
        private static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    conn.Open();
                    SqlDataAdapter cmd = new SqlDataAdapter(cmdText, conn);
                    cmd.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    conn.Close();
                }
                return dt;
            }
        }
        #endregion

        #region 执行多条 Transact-SQL 语句，并作为事务处理，返回是否成功。
        /// <summary>
        /// 执行 一组SQL语句(列表)，并作为事务处理。
        /// </summary>
        /// <param name="SQLStringList"> 一组SQL语句(列表)</param>
        /// <returns>成功执行返回true，否则返回false</returns>
        private static bool ExecuteSqlTran(string connectionString, ArrayList SQLStringList)
        {
            bool blnReturn = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (object strsql in SQLStringList)
                    {
                        if (strsql.ToString().Trim().Length > 0)
                        {
                            cmd.CommandText = strsql.ToString().Trim();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    blnReturn = true;
                }
                catch (Exception ex)
                {
                    blnReturn = false;
                    tx.Rollback();
                    throw new Exception(ex.Message, ex);
                }
                return blnReturn;
            }
        }
        #endregion

        #endregion

        #region Public functions
        ///Query--1(DataSet)
        ///ExecuteScalar--2(object)
        ///ExecuteReader--3(SqlDataReader)
        ///ExecuteNonQuery--4(int)
        ///ExecuteSqlTrans--5(bool)      
     
        #region ExecuteScalar 执行查询，并返回结果集中第一行的第一列。
        /// <summary>
        /// 执行查询，并返回结果集中第一行的第一列。
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string SQLString)
        {
            return SqlHelper.ExecuteScalar(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SQLString, null);
        }
        #endregion

        #region ExecuteReader 执行查询，并返回SqlDataReader。
        /// <summary>
        /// 执行查询，并返回SqlDataReader。
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string SQLString)
        {
            return SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SQLString, null);
        }
        #endregion

        #region ExecuteNonQuery 执行一条 Transact-SQL 语句，并返回受影响的行数。
        /// <summary>
        /// 执行一条 Transact-SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string SQLString)
        {
            return SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SQLString, null);
        }
        #endregion

        #region ExecuteDataSet 执行查询，并返回DataSet 数据集。
        /// <summary> 
        /// 执行查询，并返回DataSet 数据集。
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string SQLString)
        {
            return SqlHelper.ExecuteDataSet(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SQLString, null);
        }
        #endregion

        #region ExecuteDataTable 执行查询，并返回DataTable 数据集。
        /// <summary>
        /// 执行查询，并返回DataTable 数据集。
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string SQLString, params SqlParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataTable(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, SQLString, commandParameters);
        }
        #endregion

        #region ExecuteSqlTran 执行多条 Transact-SQL 语句，并作为事务处理，返回是否成功。
        /// <summary>
        /// 执行多条 Transact-SQL 语句，并作为事务处理，返回是否成功。
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static bool ExecuteSqlTran(ArrayList arr)
        {
            return SqlHelper.ExecuteSqlTran(SqlHelper.ConnectionStringLocalTransaction, arr);
        }
        #endregion

        #endregion

        #region 暂时未使用的方法。
        /// <summary>
        /// Hashtable to store cached parameters
        /// </summary>
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }
        #endregion

    }
}


