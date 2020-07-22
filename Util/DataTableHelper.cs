using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Reflection;
using System.ComponentModel;


namespace Util
{
    public class DataTableHelper
    {
        #region DateTableContains 判断DateTable中是否有此列并返回例值
        /// <summary>
        /// DateTableContains 判断DateTable中是否有此列并返回例值
        /// </summary>
        /// <param name="vTable"></param>
        /// <param name="row"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public static string DateTableContains(System.Data.DataTable vTable, int row,string ColumnName)
        {
             return vTable.Columns.Contains(ColumnName) ? vTable.Rows[row][ColumnName].ToString().Trim(): string.Empty;
        }
        #endregion

        //  if( dr.Table.Columns.Contains("列名"))
        #region DataRowContains 判断DataRow中是否有此列并返回例值
        /// <summary>
        /// DataRowContains
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public static string DataRowContains(System.Data.DataRow dr, string ColumnName)
        {
            return dr.Table.Columns.Contains(ColumnName) ? dr[ColumnName].ToString().Trim(): string.Empty ;
        }
        #endregion

        #region DataRowContainsInt 判断DataRow中是否有此列并返回例值
        /// <summary>
        /// DataRowContainsInt
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public static int DataRowContainsInt(System.Data.DataRow dr, string ColumnName)
        {
            string value =  dr.Table.Columns.Contains(ColumnName) ? dr[ColumnName].ToString() : "0";
            return Util.Common.IsInt(value) ? Convert.ToInt32(value) : 0;
        }
        #endregion

        #region DataRowContainsDecimal 判断DataRow中是否有此列并返回例值
        /// <summary>
        /// DataRowContainsDecimal
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public static decimal DataRowContainsDecimal(System.Data.DataRow dr, string ColumnName)
        {
            string value = dr.Table.Columns.Contains(ColumnName) ? dr[ColumnName].ToString() : "0";
            return Util.Common.IsDecimal(value) ? Convert.ToDecimal(value) : 0;
        }
        #endregion



        #region 为已有DateTable添加一新列，其设置为默认值
        /// <summary>
        /// 为已有DateTable添加一新列为string类型，其设置为默认值
        /// </summary>
        /// <param name="vTable"></param>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnValue"></param>
        /// <returns></returns>
        public static System.Data.DataTable DataColumnAddString(System.Data.DataTable vTable, string ColumnName, string ColumnValue)
        {
            System.Data.DataColumn dc = new System.Data.DataColumn(ColumnName, typeof(string));
            dc.DefaultValue = ColumnValue;
            vTable.Columns.Add(dc);
            return vTable;
        }

        /// <summary>
        /// 为已有DateTable添加一新列为decimal类型，其设置为默认值
        /// </summary>
        /// <param name="vTable"></param>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnValue"></param>
        /// <returns></returns>
        public static System.Data.DataTable DataColumnAddDecimal(System.Data.DataTable vTable, string ColumnName, decimal ColumnValue)
        {
            System.Data.DataColumn dc = new System.Data.DataColumn(ColumnName, typeof(decimal));
            dc.DefaultValue = ColumnValue;
            vTable.Columns.Add(dc);
            return vTable;
        }

        /// <summary>
        /// 为已有DateTable添加一新列为int类型，其设置为默认值
        /// </summary>
        /// <param name="vTable"></param>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnValue"></param>
        /// <returns></returns>
        public static System.Data.DataTable DataColumnAddInt(System.Data.DataTable vTable, string ColumnName, int ColumnValue)
        {
            System.Data.DataColumn dc = new System.Data.DataColumn(ColumnName, typeof(int));
            dc.DefaultValue = ColumnValue;
            vTable.Columns.Add(dc);
            return vTable;
        }

        /// <summary>
        /// 为已有DateTable添加一新列为bool类型，其设置为默认值
        /// </summary>
        /// <param name="vTable"></param>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnValue"></param>
        /// <returns></returns>
        public static System.Data.DataTable DataColumnAddBool(System.Data.DataTable vTable, string ColumnName, bool ColumnValue)
        {
            System.Data.DataColumn dc = new System.Data.DataColumn(ColumnName, typeof(int));
            dc.DefaultValue = ColumnValue;
            vTable.Columns.Add(dc);
            return vTable;
        }
        #endregion

        #region GetNewDataTable 执行DataTable中的查询返回新的DataTable
        /// <summary>
        /// 执行dataTable中的查询返回新的dataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static System.Data.DataTable GetNewDataTable(System.Data.DataTable dt, string condition)
        {
            try
            {
                //System.Data.DataTable newdt = dt.Clone();//仅复制表结构  本次开票数据整理//new System.Data.DataTable();
                System.Data.DataTable newdt = new System.Data.DataTable(); //仅复制表结构  本次开票数据整理//new System.Data.DataTable();
                dt.DefaultView.RowFilter = condition;
                newdt = dt.DefaultView.ToTable();
                //dt.DefaultView.RowFilter = string.Empty;
                //dt.DefaultView.RowStateFilter = DataViewRowState.None;
                return newdt;//返回的查询结果
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
        #endregion

        #region DataTableToStrInsert 根据datatable获得列名 生成Insert 语句头
        /// <summary>
        /// 根据datatable获得列名 生成Insert 语句头
        /// </summary>
        /// <param name="dt">表对象</param>
        /// <returns>返回结果的数据列数组</returns>
        public static string DataTableToStrInsert(System.Data.DataTable dt, string TableName)
        {
            string header = string.Empty;
            string query = string.Empty;
            header = "INSERT INTO " + TableName + " (";
            foreach (System.Data.DataColumn item in dt.Columns)
            {
                header += item.ColumnName + ",";
            }
            header = header.Remove(header.Length - 1) + ") \r\n VALUES( ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string values = string.Empty;
                string aaa = string.Empty;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].DataType.FullName == "System.Decimal" ||
                        dt.Columns[j].DataType.FullName == "System.Int32" ||
                        dt.Columns[j].DataType.FullName == "System.Int"
                        )
                        values += dt.Rows[i][j].ToString() + ",";
                    if (dt.Columns[j].DataType.FullName == "System.String")
                        values += "N'" + dt.Rows[i][j].ToString().Trim() + "',";
                }
                query += header + values.Remove(values.Length - 1) + " );\r\n";

            }
            return query;
        }
        #endregion

        #region DataTableAddDataColumn
        /// <summary>
        /// DataTableAddDataColumn
        /// </summary>
        /// <param name="headerDt"></param>
        /// <returns></returns>
        public static System.Data.DataTable DataTableAddDataColumn(System.Data.DataTable headerDt)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            foreach (DataRow dr in headerDt.Rows)
            {
                string fdName = dr["FdName"].ToString().Trim();
                string fdType = dr["FdType"].ToString().Trim();
                if (fdType == "varchar")
                {
                    CreateTableString(dt, fdName);
                }
                else if (fdType == "int")
                {
                    CreateTableInt(dt, fdName);
                }
                else if (fdType == "decimal")
                {
                    CreateTableDecimal(dt, fdName);
                }
            }
            return dt;
        }
        #endregion

        #region CreateTableString
        /// <summary>
        /// CreateTableString
        /// </summary>
        /// <param name="vTable"></param>
        /// <param name="fdName"></param>
        private static void CreateTableString(DataTable vTable, string fdName)
        {
            //为已有DataTable添加一新列
            DataColumn dc1 = new DataColumn(fdName, typeof(string));
            vTable.Columns.Add(dc1);
        }
        #endregion

        #region  CreateTableInt
        /// <summary>
        /// CreateTableInt
        /// </summary>
        /// <param name="vTable"></param>
        /// <param name="fdName"></param>
        private static void CreateTableInt(DataTable vTable, string fdName)
        {
            //为已有DataTable添加一新列
            DataColumn dc1 = new DataColumn(fdName, typeof(Int32));
            vTable.Columns.Add(dc1);
        }
        #endregion

        #region CreateTableDecimal
        /// <summary>
        /// CreateTableDecimal
        /// </summary>
        /// <param name="vTable"></param>
        /// <param name="fdName"></param>
        private static void CreateTableDecimal(DataTable vTable, string fdName)
        {
            //为已有DataTable添加一新列
            DataColumn dc1 = new DataColumn(fdName, typeof(decimal));
            vTable.Columns.Add(dc1);
        }
        #endregion

        #region GetColumnValues 获取某一列的所有值
        /// <summary>
        /// GetColumnValues 获取某一列的所有值
        /// </summary>
        /// <typeparam name="T">列数据类型</typeparam>
        /// <param name="dtSource">数据表</param>
        /// <param name="filedName">列名</param>
        /// <returns></returns>
        public static List<T> GetColumnValues<T>(DataTable dtSource, string filedName)
        {
            return (from r in dtSource.AsEnumerable() select r.Field<T>(filedName)).ToList<T>();
        }
        #endregion

        #region GetColumnValues 获取某一列的所有值,逗号分隔
        /// <summary>
        ///  GetColumnValues 获取某一列的所有值,逗号分隔
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="filedName"></param>
        /// <returns></returns>
        public static string GetColumnValues(DataTable dtSource, string filedName)
        {
            string[] idIn = dtSource.AsEnumerable().Select(r => r.Field<string>(filedName)).ToArray();
            string skus = string.Format("{0}", string.Join(",", idIn));
            return skus;

        }
        #endregion

        #region ListToDataTable  实体列表转换成DataTable
        /// <summary>
        /// ListToDataTable 实体列表转换成DataTable
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="list"> 实体列表</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(IList<T> list)
            where T : class
        {
            if (list == null || list.Count <= 0)
                return null;
            DataTable dt = new DataTable(typeof(T).Name);
            DataColumn column;
            DataRow row;

            PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int length = myPropertyInfo.Length;
            bool createColumn = true;
            foreach (T t in list)
            {
                if (t == null)
                    continue;
                row = dt.NewRow();
                for (int i = 0; i < length; i++)
                {
                    PropertyInfo pi = myPropertyInfo[i];
                    string name = pi.Name;
                    if (createColumn)
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                    }
                    row[name] = pi.GetValue(t, null);
                }

                if (createColumn)
                    createColumn = false;
                dt.Rows.Add(row);
            }
            return dt;
        }
        #endregion


        

        #region GetDataGridViewHeader
        /// <summary>
        /// GetDataGridViewHeader
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataTable GetDataGridViewHeader(string fileName)
        {
            string filePath = string.Format(@"{0}\DataGridViewHeaderJson\{1}.txt", System.Windows.Forms.Application.StartupPath, fileName);
            System.Data.DataTable dt = new DataTable();
            try
            {
                string jsonString = System.IO.File.ReadAllText(filePath);
                Newtonsoft.Json.Linq.JArray jAarray = Newtonsoft.Json.Linq.JArray.Parse(jsonString) as Newtonsoft.Json.Linq.JArray;
                List<Model.GridControlHeader> listDataGridViewHeader = jAarray.ToObject<List<Model.GridControlHeader>>();
                dt = Util.DataTableHelper.ListToDataTable<Model.GridControlHeader>(listDataGridViewHeader);
                return dt;
            }catch(Exception ex)
            {
                Log4netUtil.Log4NetHelper.Info(String.Format("GetDataGridViewHeader Json转换成DataTable失败 {0}", ex.Message), @"Exception");
                return dt;
            }
        }
        #endregion


        #region GetAllDataTable 合并多个结构相同的DataTable
        /// <summary>
        /// GetAllDataTable 合并多个结构相同的DataTable
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataTable GetAllDataTable(DataSet ds)
        {
            DataTable newDataTable = ds.Tables[0].Clone();                //创建新表 克隆以有表的架构。
            object[] objArray = new object[newDataTable.Columns.Count];   //定义与表列数相同的对象数组 存放表的一行的值。
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                {
                    ds.Tables[i].Rows[j].ItemArray.CopyTo(objArray, 0);    //将表的一行的值存放数组中。
                    newDataTable.Rows.Add(objArray);                       //将数组的值添加到新表中。
                }
            }
            return newDataTable;                                           //返回新表。
        }
        #endregion

        /*
        #region CopyToDataTable
        /// <summary>
        /// CopyToDataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> array)
        {
            var ret = new DataTable();
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
        }
        #endregion
         */
    }
}


/*

        /// <summary>
        /// 实体类转换成DataSet
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public DataSet FillDataSet(IList<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            else
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(FillDataTable(modelList));
                return ds;
            }
        }
 
 
        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public DataTable FillDataTable(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData(modelList[0]);
 
 
            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }
 
 
        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private DataTable CreateData(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }
        #endregion
*/
