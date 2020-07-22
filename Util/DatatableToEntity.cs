using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Util
{
    public class DatatableToEntity<T> where T : new()
    {
        /// <summary>
        /// 填充对象列表：用DataSet的第一个表填充实体类
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns></returns>
        public List<T> FillModel(DataSet ds)
        {
            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(null,ds.Tables[0]);
            }
        }

        // <summary>  
        /// 填充对象列表：用DataSet的第index个表填充实体类
        /// </summary>  
        public List<T> FillModel(DataSet ds, int index)
        {
            if (ds == null || ds.Tables.Count <= index || ds.Tables[index].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(null,ds.Tables[index]);
            }
        }

        /// <summary>  
        /// 填充对象列表：用DataTable填充实体类
        /// </summary>  
        public List<T> FillModel(Log4netUtil.LogAppendToForms logAppendToForms, DataTable dt,string lineMessage = "")
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> modelList = new List<T>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //T model = (T)Activator.CreateInstance(typeof(T));  
                    T model = new T();
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (propertyInfo != null && dr[i] != DBNull.Value)
                        {
                            string value = dr[i] == null ? "" : dr[i].ToString();
                            //string typestr = dr[i].GetType().Name;
                            //if(typestr.Equals("DateTime"))
                            //value = dr[i].ToString();
                            propertyInfo.SetValue(model, value, null);
                        }

                    }

                    modelList.Add(model);
                }
                return modelList;
            }
            catch (Exception ex)
            {
                string logMessage = string.Format(" {0} FillModel  DataTable 转换实体类List<T>失败，原因：{1}", lineMessage, ex.Message);
                Log4netUtil.LogDisplayHelper.LogError(logAppendToForms, logMessage);
                return null;
            }
        }

        /// <summary>  
        /// 填充对象：用DataRow填充实体类
        /// </summary>  
        public T FillModel(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }

            //T model = (T)Activator.CreateInstance(typeof(T));  
            T model = new T();

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null && dr[i] != DBNull.Value)
                {
                    string value = dr[i] == null ? "" : dr[i].ToString();
                    //string typestr = dr[i].GetType().Name;
                    //if(typestr.Equals("DateTime"))
                    //value = dr[i].ToString();
                    propertyInfo.SetValue(model, value, null);
                }
            }
            return model;
        }

    }
}
