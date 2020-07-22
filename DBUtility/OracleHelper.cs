using System.Data;
using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Text;
//using System.IO;
//using Util.FrameUtil;

namespace DBUtility
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: Oracle.cs   
    ///* 命名空间: DBUtility
    ///* 功    能: Oracle数据库操作基础类
    ///* 内    容: 
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 版权说明:   Copyright 2018-2027 武汉飞宇益克科技有限公司
    ///*
    ///**************************************************************************/
    
    public class OracleHelper : DBHelper
    {

        #region ConnectTestInfo 测式
        /// <summary>
        /// ConnectTestInfo
        /// </summary>
        /// <returns></returns>
        public override bool ConnectTestInfo(string connectionType)
        {
            return ConnectionTestInfo(connectionType);
        }
        #endregion

        #region PrepareCommand 执行数据库命令前的准备工作
        /// <summary>  
        /// 执行数据库命令前的准备工作  
        /// </summary>  
        /// <param name="command">Command对象</param>  
        /// <param name="connection">数据库连接对象</param>  
        /// <param name="trans">事务对象</param>  
        /// <param name="cmdType">Command类型</param>  
        /// <param name="cmdText">Oracle存储过程名称或PL/SQL命令</param>  
        /// <param name="commandParameters">命令参数集合</param>  
        private static void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction trans, CommandType cmdType, string cmdText, string systemType, DbParameter[] commandParameters)
        {
            if (connection.State != ConnectionState.Open) connection.Open();

            command.Connection = connection;
            command.CommandText = cmdText;
            command.CommandType = cmdType;

            if (trans != null) command.Transaction = trans;

            if (commandParameters != null)
            {
                foreach (OracleParameter parm in commandParameters)
                    command.Parameters.Add(parm);
            }
        }
        #endregion

        #region ExecuteNonQuery 执行Transact-SQL语句并返回受影响的行数
        /// <summary> 
        /// 执行Transact-SQL语句并返回受影响的行数
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回int类型</returns>
        public override int ExecuteNonQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            //对于 UPDATE、INSERT 和 DELETE 语句，返回值为该命令所影响的行数。
            //对于 CREATE TABLE 和 DROP TABLE 语句，返回值为 0。
            //对于其他所有类型的语句，返回值为 -1。
            using (OracleCommand cmd = new OracleCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, null, cmdParams);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                if (val == -1)
                    return 1;
                return val;
            }
        }

        /// <summary>
        /// 在事务中执行Transact-SQL语句并返回受影响的行数
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回int类型</returns>
        public override int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText,systemType, trans, cmdParams);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        #endregion

        #region ExecuteQuery 执行Transact-SQL语句并返回数据集DataSet
        /// <summary>
        /// 执行Transact-SQL语句并返回数据集DataSet
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回DataSet集合</returns>
        public override DataSet ExecuteQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                DataSet dataset = null;
                try
                {
                    PrepareCmd(cmd, cmdType, cmdText, systemType, null, cmdParams);
                    OracleDataAdapter sda = new OracleDataAdapter();
                    sda.SelectCommand = cmd;
                    dataset = new DataSet();
                    sda.Fill(dataset);
                    cmd.Parameters.Clear();

                }
                catch
                {
                    cmd.Dispose();
                    throw;
                }
                finally
                {
                    cmd.Dispose();
                }

                return dataset;
               
            }
        }

        /// <summary>
        /// 在事务中执行Transact-SQL语句并返回数据集DataSet
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回DataSet集合</returns>
        public override DataSet ExecuteQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                DataSet dataset = null;
                try
                {
                    PrepareCmd(cmd, cmdType, cmdText, systemType, trans, cmdParams);
                    OracleDataAdapter sda = new OracleDataAdapter();
                    sda.SelectCommand = cmd;
                    dataset = new DataSet();
                    sda.Fill(dataset);
                    cmd.Parameters.Clear();

                }
                catch
                {
                    cmd.Dispose();
                    throw;
                }
                finally
                {
                    cmd.Dispose();
                }

                return dataset;

            }
        }
        #endregion

        #region ExecuteNQuery  执行Transact-SQL语句返回内存中数据的一个表
        /// <summary>
        /// 执行Transact-SQL语句返回内存中数据的一个表
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回内存中数据的一个表</returns>
        public override DataTable ExecuteNQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand command = new OracleCommand())
            {
                DataTable table = null;
                try
                {
                    PrepareCmd(command, cmdType, cmdText, systemType, null, cmdParams);
                    OracleDataAdapter adapter = new OracleDataAdapter();
                    adapter.SelectCommand = command;
                    table = new DataTable();
                    adapter.Fill(table);
                    command.Parameters.Clear();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    command.Dispose();
                }
                return table;
            }

        }

        /// <summary>
        /// 在事务中执行Transact-SQL语句返回内存中数据的一个表
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回内存中数据的一个表</returns>
        public override DataTable ExecuteNQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand command = new OracleCommand())
            {
                DataTable table = null;
                try
                {
                    PrepareCmd(command, cmdType, cmdText, systemType, trans, cmdParams);
                    OracleDataAdapter adapter = new OracleDataAdapter();
                    adapter.SelectCommand = command;
                    table = new DataTable();
                    adapter.Fill(table);
                    command.Parameters.Clear();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    command.Dispose();

                }

                return table;
            }

        }
        #endregion

        public override DataTable ExecutePQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand command = new OracleCommand())
            {
                DataTable table = null;
                try
                {
                    PrepareCmd(command, cmdType, cmdText, systemType, null, cmdParams);
                    OracleDataAdapter adapter = new OracleDataAdapter();
                    adapter.SelectCommand = command;
                    table = new DataTable();
                    adapter.Fill(table);
                    command.Parameters.Clear();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    command.Dispose();
                }
                return table;
            }

        }

        #region ExecuteLQuery 执行Transact-SQL语句返回List
        /// <summary>
        /// 执行Transact-SQL语句返回List<List<string>>
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns></returns>
        public override List<List<string>> ExecuteLQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                DataSet dataset = null;

                PrepareCmd(cmd, cmdType, cmdText, systemType, null, cmdParams);
                OracleDataAdapter sda = new OracleDataAdapter();
                sda.SelectCommand = cmd;
                dataset = new DataSet();
                sda.Fill(dataset);
                cmd.Parameters.Clear();
                using (DataTable dt = new DataTable())
                {
                    List<List<string>> lls = new List<List<string>>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        List<string> ls = new List<string>();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ls.Add(dt.Rows[i][j].ToString());
                        }
                        lls.Add(ls);
                    }
                    return lls;
                }
            }

        }
        

        /// <summary>
        /// 在事务中执行Transact-SQL语句返回List<List<string>>
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns></returns>
        public override List<List<string>> ExecuteLQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                DataSet dataset = null;

                PrepareCmd(cmd, cmdType, cmdText, systemType, trans, cmdParams);
                OracleDataAdapter sda = new OracleDataAdapter();
                sda.SelectCommand = cmd;
                dataset = new DataSet();
                sda.Fill(dataset);
                cmd.Parameters.Clear();
                using (DataTable dt = new DataTable())
                {
                    List<List<string>> lls = new List<List<string>>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        List<string> ls = new List<string>();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ls.Add(dt.Rows[i][j].ToString());
                        }
                        lls.Add(ls);
                    }
                    return lls;
                }
            }
        }
        #endregion

        #region ExecuteReader 执行查询，返回DataReader
        /// <summary>
        /// 执行查询，返回DataReader
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回DataReader</returns>
        public override DbDataReader ExecuteReader(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, null, cmdParams);
                OracleDataReader sdr;
                sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return sdr;
            }
        }

        /// <summary>
        /// 在事务中执行执行查询，返回DataReader
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回DataReader</returns>
        public override DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, trans, cmdParams);
                OracleDataReader sdr;
                sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return sdr;
            }
        }
        #endregion

        #region ExecuteScalar 执行查询，返回结果集中的第一行第一列，忽略其它行其它列
        /// <summary>
        /// 执行查询，返回结果集中的第一行第一列，忽略其它行其它列
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回结果集中的第一行第一列，忽略其它行其它列</returns>
        public override object ExecuteScalar(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, null, cmdParams);
                object o = new object();
                o = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return o;
            }
        }

        /// <summary>
        /// 在事务中执行查询，返回结果集中的第一行第一列，忽略其它行其它列
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回结果集中的第一行第一列，忽略其它行其它列</returns>
        public override object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (OracleCommand cmd = new OracleCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, trans, cmdParams);
                object o = new object();
                o = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return o;
            }
        }
        #endregion

        public override bool BulkCopyInsert(DataTable dt, string rowguid, string TableName)
        {
            OracleConnection conn = (OracleConnection)PrepareCon();
            return false;
        }

    }
}
