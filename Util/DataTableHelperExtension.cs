using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace Util
{
  
    public static class DataTableHelperExtension
    {
        /// <summary>
        /// CopyToDataTable 匿名类型var转换成DataTable对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> array)
        {
            var ret = new DataTable();
            try
            {
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                    // if (!dp.IsReadOnly)
                    ret.Columns.Add(dp.Name, dp.PropertyType);
                foreach (T item in array)
                {
                    var Row = ret.NewRow();
                    foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                        // if (!dp.IsReadOnly)
                        Row[dp.Name] = dp.GetValue(item);
                    ret.Rows.Add(Row);
                }
                return ret;
            }catch(Exception ex)
            {
                Log4netUtil.Log4NetHelper.Info(String.Format("CopyToDataTable 匿名类型var转换成DataTable对象 {0}", ex.Message), @"Exception");
                return ret;
            }
        }

    }
}


