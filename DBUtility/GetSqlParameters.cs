using System;
using System.Collections.Generic;
//using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Data.Common;
namespace DBUtility
{

    public class GetSqlParameters
    {
        /// <summary>
        /// 过滤参数的规则
        /// </summary>
        private static Regex reg = new Regex(@"@\S{1,}?(,|\s|;|--|\)|$)");

        private static char[] filterChars = new char[] { ' ', ',', ';', '-', ')' };

        /// <summary>
        /// 根据sql语句和实体对象自动生成参数化查询SqlParameter列表
        /// </summary>
        /// <typeparam name="T">实体对象类型</typeparam>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="obj">实体对象</param>
        /// <returns>SqlParameter列表</returns>
        public static List<DbParameter> From<T>(String sqlStr, T obj)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            List<string> listStr = new List<string>();
            Match mymatch = reg.Match(sqlStr);
            while (mymatch.Success)
            {
                listStr.Add(mymatch.Value.TrimEnd(filterChars).TrimStart(':'));
                mymatch = mymatch.NextMatch();
            }
            Type t = typeof(T);

            PropertyInfo[] pinfo = t.GetProperties();

            foreach (var item in listStr)
            {
                for (int i = 0; i < pinfo.Length; i++)
                {
                    if (item.Equals(pinfo[i].Name, StringComparison.OrdinalIgnoreCase))
                    {
                        //parameters.Add(new DbParameter() { ParameterName = "@" + item, Value = pinfo[i].GetValue(obj, null) });
                        break;
                    }
                    else
                    {
                        if (i == pinfo.Length - 1)
                        {
                            throw new Exception("查询参数@" + item + "在类型" + t.ToString() + "中未找到赋值属性");
                        }
                    }
                }
            }

            return parameters;
        }
        /// <summary>
        /// 根据sql语句和实体对象自动生成参数化查询SqlParameter列表
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="obj">实体对象</param>
        /// <returns>SqlParameter列表</returns>
        public static List<DbParameter> From(String sqlStr, object obj)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            List<string> listStr = new List<string>();
            Match mymatch = reg.Match(sqlStr);
            while (mymatch.Success)
            {
                listStr.Add(mymatch.Value.TrimEnd(filterChars).TrimStart('@'));
                mymatch = mymatch.NextMatch();
            }
            Type t = obj.GetType();

            PropertyInfo[] pinfo = t.GetProperties();

            foreach (var item in listStr)
            {
                for (int i = 0; i < pinfo.Length; i++)
                {
                    if (item.Equals(pinfo[i].Name, StringComparison.OrdinalIgnoreCase))
                    {
                        //parameters.Add(new DbParameter() { ParameterName = "@" + item, Value = pinfo[i].GetValue(obj, null) });
                        //_idbHelper.CreateInParam(":ModuleID", searchParam.ModuleID )
                        break;
                    }
                    else
                    {
                        if (i == pinfo.Length - 1)
                        {
                            throw new Exception("查询参数@" + item + "在类型" + t.ToString() + "中未找到赋值属性");
                        }
                    }
                }
            }

            return parameters;
        }

        /// <summary>
        /// 根据sql语句和ExpandoObject对象自动生成参数化查询SqlParameter列表
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="obj">ExpandoObject对象</param>
        /// <returns>SqlParameter列表</returns>
        public static List<DbParameter> From(String sqlStr, ExpandoObject obj)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            List<string> listStr = new List<string>();
            Match mymatch = reg.Match(sqlStr);
            while (mymatch.Success)
            {
                listStr.Add(mymatch.Value.TrimEnd(filterChars).TrimStart('@'));
                mymatch = mymatch.NextMatch();
            }
            IDictionary<String, Object> dic = (IDictionary<String, Object>)obj;

            foreach (var item in listStr)
            {
                int reachCount = 0;
                foreach (var property in dic)
                {
                    if (item.Equals(property.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        //parameters.Add(new DbParameter() { ParameterName = "@" + item, Value = property.Value });
                        break;
                    }
                    else
                    {
                        if (reachCount == dic.Count - 1)
                        {
                            throw new Exception("查询参数@" + item + "在类型ExpandoObject中未找到赋值属性");
                        }
                    }
                    reachCount++;
                }
            }
            return parameters;
        }
    }
}


