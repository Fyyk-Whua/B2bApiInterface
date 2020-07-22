using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBUtility
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: MySqlDBHelper.cs   
    ///* 命名空间: DBUtility
    ///* 功    能: MySql数据库操作基础类
    ///* 内    容: 各派生类的具体实现，实现MySql的各个虚函数。
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 版权说明:   Copyright 2018-2027 武汉飞宇益克科技有限公司
    ///*
    ///**************************************************************************/

    public class MySqlDBHelper : DBHelper
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
            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, null, cmdParams);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
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
            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, trans, cmdParams);
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
            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, null, cmdParams);
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    cmd.Parameters.Clear();
                    return ds;
                }
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
            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, trans, cmdParams);
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    cmd.Parameters.Clear();
                    return ds;
                }
            }
        }
        #endregion

        #region ExecuteNQuery 执行Transact-SQL语句返回内存中数据的一个表
        /// <summary>
        /// 执行Transact-SQL语句返回内存中数据的一个表
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回内存中数据的一个表</returns>
        public override DataTable ExecuteNQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, null, cmdParams);
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        cmd.Parameters.Clear();
                        return ds.Tables[0];
                    }
                }
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
            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, trans, cmdParams);
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        cmd.Parameters.Clear();
                        return ds.Tables[0];
                    }
                }
            }
        }
        #endregion

        public override DataTable ExecutePQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, null, cmdParams);
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        cmd.Parameters.Clear();
                        return ds.Tables[0];
                    }
                }
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
            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, null, cmdParams);
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
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
            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, trans, cmdParams);
                using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                {
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
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
            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, null, cmdParams);
                MySqlDataReader sdr;
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
            using (MySqlCommand cmd = new MySqlCommand())
            {
                PrepareCmd(cmd, cmdType, cmdText, systemType, trans, cmdParams);
                MySqlDataReader sdr;
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
            using (MySqlCommand cmd = new MySqlCommand())
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
            using (MySqlCommand cmd = new MySqlCommand())
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
            MySqlConnection conn = (MySqlConnection)PrepareCon();
            return false;
        }
    }
}

