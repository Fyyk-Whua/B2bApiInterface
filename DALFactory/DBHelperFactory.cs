using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
//using System.Data.SqlClient;
//using MySql.Data.MySqlClient;
using IDAL;
using Util;
using DBUtility;

namespace DALFactory
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: DBHelperFactory.cs   
    ///* 命名空间: DALFactory.FrameDALFactory
    ///* 功    能: DB抽象工厂层
    ///* 内    容: 根据数据库访问层不同数据 创建不同数据访问类对象
    ///            使用静态工厂模式，通过传入参数，动态创建访问实例
    ///            实现模式上采用基本实现接口，派生类继承基类的虚函数，从而实现代码的耦合较低，有很好的扩展性
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 版权说明:  Copyright 2018-2027 武汉飞宇益克科技有限公司
    ///*
    ///**************************************************************************/
    public class DBHelperFactory
    {
        #region 根据类型创建不同数据访问类对象
        /// <summary>
        /// 根据类型创建不同数据访问类对象
        /// </summary>
        public static IDBHelper CreateInstance(string type)
        {
            IDBHelper _iDBHelper ;
            string dbType = string.Empty;
            switch (type)
            {
                case "Erp":
                    dbType = Util.DalConst.ErpDBType;
                    break;
                case "Wms":
                    dbType = Util.DalConst.WmsDBType;
                    break;
                default:
                    break;
            }
            switch (dbType)
            {
                case "MySql":
                    _iDBHelper = new MySqlDBHelper();
                    break;
                case "SqlServer":
                    _iDBHelper = new SqlDBHelper();
                    break;
                case "Oracle":
                    _iDBHelper = new OracleHelper();
                    break;
                default:
                    _iDBHelper = new SqlDBHelper();
                    break;
            }
            return _iDBHelper;
        } 
        #endregion

        #region  CreateErpInstance
        public static IDBHelper CreateErpInstance()
        {
            IDBHelper _iDBHelper ;
            string dbType = Util.DalConst.ErpDBType;
            switch (dbType)
            {
                case "MySql":
                    _iDBHelper = new MySqlDBHelper();
                    break;
                case "SqlServer":
                    _iDBHelper = new SqlDBHelper();
                    break;
                case "Oracle":
                    _iDBHelper = new OracleHelper();
                    break;
                default:
                    _iDBHelper = new SqlDBHelper();
                    break;
            }
            return _iDBHelper;
        } 
        #endregion
     
    }
}
