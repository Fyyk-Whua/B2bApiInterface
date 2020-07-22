using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace IDAL
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: IDBHelper.cs   
    ///* 命名空间: IDAL.FrameIDAL
    ///* 功    能: 数据库操作接口类
    ///* 内    容: 定义数据库操作的标准接口IDBHelper,定义接口的基本功能；通过基本的接口设置，完成数据访问的统一抽象。
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 版权说明:  Copyright 2018-2027 武汉飞宇益克科技有限公司
    ///*
    ///**************************************************************************/
       
    public interface IDBHelper
    {
        #region ConnectionTestInfo 测式数据库连接
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        //bool ConnectionTestInfo();
        bool ConnectionTestInfo(string connectionType);
        #endregion

        #region CreateConnection 创建数据库连接
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        //void CreateConnection();
        void CreateConnection(string connectionType);
        #endregion

        #region ExecuteNonQuery 执行Transact-SQL语句并返回受影响的行数
        /// <summary>
        /// 执行Transact-SQL语句并返回受影响的行数
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回int类型</returns>
        int ExecuteNonQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecuteNonQuery 在事务中执行Transact-SQL语句并返回受影响的行数
        /// <summary>
        /// 在事务中执行Transact-SQL语句并返回受影响的行数
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回int类型</returns>
        int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecuteQuery 执行Transact-SQL语句并返回数据集DataSet
        /// <summary>
        /// 执行Transact-SQL语句并返回数据集DataSet
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回DataSet集合</returns>
        DataSet ExecuteQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecuteQuery 在事务中执行Transact-SQL语句并返回数据集DataSet
        /// <summary>
        /// 在事务中执行Transact-SQL语句并返回数据集DataSet
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回DataSet集合</returns>
        DataSet ExecuteQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecuteNQuery 执行Transact-SQL语句返回内存中数据的一个表
        /// <summary>
        /// 执行Transact-SQL语句返回内存中数据的一个表
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回内存中数据的一个表</returns>
        DataTable ExecuteNQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecutePQuery
        /// <summary>
        /// ExecutePQuery
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="systemType"></param>
        /// <param name="cmdParams"></param>
        /// <returns></returns>
        DataTable ExecutePQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecuteNQuery 在事务中执行Transact-SQL语句返回内存中数据的一个表
        /// <summary>
        /// 在事务中执行Transact-SQL语句返回内存中数据的一个表
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回内存中数据的一个表</returns>
        DataTable ExecuteNQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecuteLQuery 执行Transact-SQL语句返回List
        /// <summary>
        /// 执行Transact-SQL语句返回List<List<string>>
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns></returns>
        List<List<string>> ExecuteLQuery(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecuteLQuery 在事务中执行Transact-SQL语句返回List<List<string>>
        /// <summary>
        /// 在事务中执行Transact-SQL语句返回List<List<string>>
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns></returns>
        List<List<string>> ExecuteLQuery(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecuteReader 执行查询，返回DataReader
        /// <summary>
        /// 执行查询，返回DataReader
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回DataReader</returns>
        DbDataReader ExecuteReader(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecuteReader 在事务中执行执行查询，返回DataReader
        /// <summary>
        /// 在事务中执行执行查询，返回DataReader
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回DataReader</returns>
        DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecuteScalar 执行查询，返回结果集中的第一行第一列，忽略其它行其它列
        /// <summary>
        /// 执行查询，返回结果集中的第一行第一列，忽略其它行其它列
        /// </summary>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回结果集中的第一行第一列，忽略其它行其它列</returns>
        object ExecuteScalar(CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region ExecuteScalar 在事务中执行查询，返回结果集中的第一行第一列，忽略其它行其它列
        /// <summary>
        /// 在事务中执行查询，返回结果集中的第一行第一列，忽略其它行其它列
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="cmdType">命令字符串类型</param>
        /// <param name="cmdText">执行语句</param>
        /// <param name="cmdParams">命令参数列表</param>
        /// <returns>返回结果集中的第一行第一列，忽略其它行其它列</returns>
        object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText, string systemType, params DbParameter[] cmdParams);
        #endregion

        #region CreateParam 返回DbParameter的对象
        /// <summary>
        /// 返回DbParameter的对象
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="value">参数值</param>
        /// <param name="size">参数大小</param>
        /// <param name="direction">参数值方向</param>
        /// <returns></returns>
        DbParameter CreateParam(string systemType, string paramName, DbType dbType, object value, int size, ParameterDirection direction);
        #endregion

        #region CreateInParam 返回一个输入类型的DbParameter对象
        /// <summary>
        /// 返回一个输入类型的DbParameter对象
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="value">参数值</param>
        /// <param name="size">参数大小</param>
        /// <returns></returns>
        DbParameter CreateInParam(string systemType, string paramName, DbType dbType, object value, int size);
        #endregion

        #region CreateInParam 返回一个输入类型的DbParameter对象
        /// <summary>
        /// 返回一个输入类型的DbParameter对象
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        DbParameter CreateInParam(string systemType, string paramName, DbType dbType, object value);
        #endregion

        #region CreateInParam 返回一个输入类型的DbParameter对象
        /// <summary>
        /// 返回一个输入类型的DbParameter对象
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        DbParameter CreateInParam(string systemType, string paramName, object value);
        #endregion

        #region CreateOutParam 返回一个输出类型的DbParameter对象
        /// <summary>
        /// 返回一个输出类型的DbParameter对象
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <returns></returns>
        DbParameter CreateOutParam(string systemType, string paramName, DbType dbType, int size);
        #endregion

        #region CreateOutParam 返回一个输出类型的DbParameter对象
        /// <summary>
        /// 返回一个输出类型的DbParameter对象
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <returns></returns>
        DbParameter CreateOutParam(string systemType, string paramName, DbType dbType);
        #endregion

        #region CreateReturnParam 返回一个返回类型的DbParameter对象
        /// <summary>
        /// 返回一个返回类型的DbParameter对象
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <returns></returns>
        DbParameter CreateReturnParam(string systemType, string paramName, DbType dbType, int size);
        #endregion

        #region CreateReturnParam 返回一个返回类型的DbParameter对象
        /// <summary>
        ///  返回一个返回类型的DbParameter对象
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <returns></returns>
        DbParameter CreateReturnParam(string systemType, string paramName, DbType dbType);
        #endregion

        #region BulkCopyInsert 批量插入
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowguid"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        bool BulkCopyInsert(DataTable dt, string rowguid, string TableName);
        #endregion

    }
}
