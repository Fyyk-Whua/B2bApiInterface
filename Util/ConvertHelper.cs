using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;

namespace Util
{
    public class ConvertHelper
    {
        #region ConvertStringToDecimal
        /// <summary>
        /// ConvertStringToDecimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ConvertStringToDecimal(string value)
        {
            string strResult = string.IsNullOrEmpty(value) ? "0" : value;
            return Util.Common.IsDecimal(strResult) ? Convert.ToDecimal(strResult) : 0;
        }
        #endregion

        #region ConvertStringToInt
        /// <summary>
        /// ConvertStringToInt
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ConvertStringToInt(string value)
        {
            string strResult = string.IsNullOrEmpty(value) ? "0" : value;
            return Util.Common.IsInt(strResult) ? Convert.ToDecimal(strResult) : 0;
        }
        #endregion


        #region DataGridViewToDataTable 把DataGridView控件数据，转成DataTable
        /// <summary>
        /// 把DataGridView控件数据，转成DataTable
        /// </summary>
        /// <param name="dgv">DataGridView控件数据</param>
        /// <returns></returns>
        public static DataTable DataGridViewToDataTable(DataGridView dgv)
        {
            dgv.EndEdit();
            DataTable dt = new DataTable();

            for (int count = 0; count < dgv.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dgv.Columns[count].Name.ToString());
                dt.Columns.Add(dc);
            }
            for (int count = 0; count < dgv.Rows.Count; count++)
            {
                DataRow dr = dt.NewRow();
                for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
                {
                    dr[countsub] = Convert.ToString(dgv.Rows[count].Cells[countsub].Value);
                }
                dt.Rows.Add(dr);

            }
            return dt;
        }
        #endregion

        #region DataGridViewToDataTable
        /// <summary>
        /// DataGridViewToDataTable
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DataTable DataGridViewToDataTable(DataGridView dgv,string fieldName)
        {
            dgv.EndEdit();
            DataTable dt = new DataTable();

            for (int count = 0; count < dgv.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dgv.Columns[count].Name.ToString());
                dt.Columns.Add(dc);
            }
            for (int count = 0; count < dgv.Rows.Count; count++)
            {
                if (dgv.Rows[count].Cells[fieldName] != null)
                {
                    DataRow dr = dt.NewRow();
                    for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
                    {
                            dr[countsub] = Convert.ToString(dgv.Rows[count].Cells[countsub].Value);
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }
        #endregion

        #region DataGridViewToDataTable
        /// <summary>
        /// 把DataGridView控件数据，转成DataTable
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="Index"> 选中的一行</param>
        /// <returns></returns>
        public static DataTable DataGridViewToDataTable(DataGridView dgv, int Index)
        {
            DataTable dt = new DataTable();
            if (dgv.Rows.Count > 0)
            {
                for (int k = 0; k < dgv.Columns.Count; k++)
                {
                    dt.Columns.Add(dgv.Columns[k].Name.ToString());
                }
                int j = Index;// this.dgv.CurrentRow.Index - 1;  //选中行

                DataRow dr = dt.NewRow();
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    if (dgv.Rows[j].Cells[i].Value != null)
                    {
                        dr[i] = dgv.Rows[j].Cells[i].Value.ToString();
                    }
                    else
                    {
                        dr[i] = "";
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

        #region DataGridViewToDataTable 把DataGridView控件数据，转成DataTable
        /// <summary>
        /// 把DataGridView控件数据，转成DataTable
        /// </summary>
        /// <param name="dgv">DataGridView控件数据</param>
        /// <param name="boolCheck">是否只转换勾选</param>
        /// <returns></returns>
        public static DataTable DataGridViewToDataTable(DataGridView dgv, bool boolCheck, string CellsName)
        {
            DataTable dt = new DataTable();

            for (int count = 0; count < dgv.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dgv.Columns[count].Name.ToString());
                dt.Columns.Add(dc);
            }
            for (int count = 0; count < dgv.Rows.Count; count++)
            {
                DataRow dr = dt.NewRow();
                if (boolCheck)
                {
                    if (dgv.Rows[count].Cells[CellsName].EditedFormattedValue.ToString() == "True")
                    {

                        for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
                        {

                            dr[countsub] = Convert.ToString(dgv.Rows[count].Cells[countsub].Value);
                        }
                        dt.Rows.Add(dr);
                    }

                }
                else
                {
                    for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
                    {
                        dr[countsub] = Convert.ToString(dgv.Rows[count].Cells[countsub].Value);
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }
        #endregion

        #region DataTable 转 List
        /// <summary>
        /// DataTable 转 List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> convertToList1<T>(DataTable dt) where T : new()
        {
            List<T> ts = new List<T>();
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite)
                        {
                            continue;
                        }
                        object value = dr[tempName];

                        if (value != null)
                        {
                            pi.SetValue(t, value, null);
                           // pi.SetValue(t, Convert.ChangeType(value, p.PropertyType), null);

                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
        #endregion

        #region   convertToList 
        /// <summary>
        /// DataTableToEntities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> convertToList<T>(DataTable table)
        {

            if (null == table || table.Rows.Count <= 0) return default(List<T>);

            List<T> list = new List<T>();

            List<string> keys = new List<string>();

            foreach (DataColumn c in table.Columns)
            {

                keys.Add(c.ColumnName.ToLower());

            }

            for (int i = 0; i < table.Rows.Count; i++)
            {

                T entity = Activator.CreateInstance<T>();

                PropertyInfo[] attrs = entity.GetType().GetProperties();

                foreach (PropertyInfo p in attrs)
                {

                    if (keys.Contains(p.Name.ToLower()))
                    {

                        if (!DBNull.Value.Equals(table.Rows[i][p.Name]))
                        {

                            p.SetValue(entity, Convert.ChangeType(table.Rows[i][p.Name], p.PropertyType), null);

                        }

                    }

                }

                list.Add(entity);

            }

            return list;

        }
        #endregion

        #region DataTable 转 Object
        /// <summary>
        /// 将查询表结果一行转化为对应的对象
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Object</returns>
        public static T convertToObject<T>(DataRow dataRow)where T : new(){
            T instance = new T();
            Type columnType = instance.GetType();
            PropertyInfo[] properties = columnType.GetProperties();
            DataColumnCollection columns = dataRow.Table.Columns;
            int columnCount = columns.Count;
            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < properties.Length; j++)
                {
                    if ((columns[i].ColumnName).ToLower() == (properties[j].Name).ToLower())
                    {
                        string tp = properties[j].PropertyType.FullName.ToString();
                        if (dataRow[i] == System.DBNull.Value)
                        {
                            
                            switch (tp)
                            {
                                case "System.Decimal":
                                    dataRow[i] = 0;
                                    break;
                                case "System.String":
                                    dataRow[i] = "";
                                    break;
                                case "System.Int16":
                                case "System.Int32":
                                    dataRow[i] = 0;
                                    break;
                                default:
                                    dataRow[i] = null;
                                    break;
                            }
                        }
                        //properties[j].SetValue(instance, dataRow[i], null);
                        properties[j].SetValue(instance, Convert.ChangeType(dataRow[i], properties[j].PropertyType), null);
                    }
                }
            }
            return instance;
        }
        #endregion

        #region  convertDataGridViewToDataTablet DataGridView To DataTablet
        /// <summary>
        /// 将DataGridViewRows先中的一行转换成转成DataTable数据  
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public static DataTable convertDataGridViewToDataTablet(DataGridView dgv)  
        {
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;  //选择一整行
            DataGridViewRow row = dgv.Rows[dgv.CurrentRow.Index];
            row.Selected = true;
            dgv.CurrentCell = row.Cells[0];
            DataGridViewSelectedRowCollection rowColl = dgv.SelectedRows;
            
            if (rowColl  == null)
                return null;

            DataTable totalDT = (DataTable)dgv.DataSource;
            if (totalDT == null)
                return null ;
 
            DataTable gridSelectDT = totalDT.Clone();
 
            for (int i = 0; i < rowColl.Count; i++)
            {
                DataRow dataRow = (rowColl[i].DataBoundItem as DataRowView).Row; 
                gridSelectDT.ImportRow(dataRow);
            }
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            return gridSelectDT;  
        }
        #endregion

        #region convertDataGridViewToDataRow  DataGridView To DataRow
        /// <summary>
        /// 将DataGridViewRows先中的一行转换成转成DataRow数据  
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public static DataRow convertDataGridViewToDataRow(DataGridView dgv)
        {
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;  //选择一整行
            DataGridViewRow row = dgv.Rows[dgv.CurrentRow.Index];
            row.Selected = true;
            dgv.CurrentCell = row.Cells[0];
            DataGridViewSelectedRowCollection rowColl = dgv.SelectedRows;

            if (rowColl == null)
                return null;

            DataTable totalDT = (DataTable)dgv.DataSource;
            if (totalDT == null)
                return null;

            DataTable gridSelectDT = totalDT.Clone();

            for (int i = 0; i < rowColl.Count; i++)
            {
                DataRow dataRow = (rowColl[i].DataBoundItem as DataRowView).Row;
                gridSelectDT.ImportRow(dataRow);
            }
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DataRow dr = gridSelectDT.Rows[0];
            return dr;
        }  
        #endregion


        #region GetColumnsByDataTable 
        /// <summary>
        /// GetColumnsByDataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string GetColumnsByDataTable(DataTable dt, string TableName)
        {
            string strInsert = "INSERT INTO " + TableName + "( ";
            int columnNum = dt.Columns.Count-1;
            if (dt.Columns.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i == columnNum)
                        strInsert = strInsert + dt.Columns[i].ColumnName + ")";
                    else
                        strInsert = strInsert + dt.Columns[i].ColumnName + ",";
                }
            }
            return strInsert;
        }
        #endregion


        #region GetValueByDataTable 
        /// <summary>
        /// GetValueByDataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetValueByDataTable(DataTable dt)
        {
            string strInsert = " ";
            int columnNum = dt.Columns.Count - 1;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strInsertRow = " VALUES( ";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j == columnNum)
                            strInsertRow =strInsertRow+dt.Rows[i][j]+" );";
                        else
                            strInsertRow = strInsertRow + dt.Rows[i][j] + ",";
                    }
                    strInsert = strInsert + strInsertRow + "/r/n";
                }
            }
            return strInsert;
        }
        #endregion

        #region DataTableToStrInsert  
        /// <summary>
        /// DataTableToStrInsert
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string DataTableToStrInsert(DataTable dt, string TableName)
        {
            string header = string.Empty;
            string query = string.Empty;
            
            header = "INSERT INTO " + TableName + " (";
            foreach (DataColumn item in dt.Columns)
            {
                header +=  item.ColumnName + ",";
            }
            header = header.Remove(header.Length - 1) + ") \r VALUES( ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string values = string.Empty;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].DataType.FullName == "System.Decimal" ||
                        dt.Columns[j].DataType.FullName == "System.Double" ||
                        dt.Columns[j].DataType.FullName == "System.Single" ||
                        dt.Columns[j].DataType.FullName == "System.Int64" ||
                        dt.Columns[j].DataType.FullName == "System.Int32" ||
                        dt.Columns[j].DataType.FullName == "System.Int"
                        )
                        values += dt.Rows[i][j].ToString() + ",";
                    else if (dt.Columns[j].DataType.FullName == "System.String")
                        values += "N'" + dt.Rows[i][j].ToString().Trim() + "',";
                    else
                        values += "N'" + dt.Rows[i][j].ToString().Trim() + "',";
                }
                query += header + values.Remove(values.Length - 1) + " );\r";

            }
            query = "BEGIN \r" + query;
            query += "END; \r";
            return query;
        }
        #endregion

       

        #region DataTableToStrInsert
        /// <summary>
        /// DataTableToStrInsert
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string DataTableToStrInsert(DataTable dt,System.Data.DataRow dr, string TableName)
        {
            string header = string.Empty;
            string query = string.Empty;

            header = "INSERT INTO " + TableName + " (";
            foreach (DataColumn item in dt.Columns)
            {
                header += item.ColumnName + ",";
            }
            header = header.Remove(header.Length - 1) + ") \r VALUES( ";

            string values = string.Empty;
            foreach (DataColumn item in dt.Columns)
            {
                values += string.Format(":{0},",item.ColumnName);
            }
            query += header + values.Remove(values.Length - 1) + " );\r";
            query = "BEGIN \r" + query;
            query += "END; \r"; 
            /*
            string values = string.Empty;
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Columns[j].DataType.FullName == "System.Decimal" ||
                    dt.Columns[j].DataType.FullName == "System.Double" ||
                    dt.Columns[j].DataType.FullName == "System.Single" ||
                    dt.Columns[j].DataType.FullName == "System.Int64" ||
                    dt.Columns[j].DataType.FullName == "System.Int32" ||
                    dt.Columns[j].DataType.FullName == "System.Int"
                    )
                    values += dr[dt.Columns[j].ColumnName].ToString().Trim() + ",";
                else if (dt.Columns[j].DataType.FullName == "System.String")
                    values += "N'" + dr[dt.Columns[j].ColumnName].ToString().Trim() + "',";
                else
                    values += "N'" + dr[dt.Columns[j].ColumnName].ToString().Trim() + "',";
            }
            query += header + values.Remove(values.Length - 1) + " );\r";
            query = "BEGIN \r" + query;
            query += "END; \r";*/
            return query;
        }
        #endregion

        #region  DecimalToString
        /// <summary>
        /// DecimalToString
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string DecimalToString(decimal d)
        {
            return d.ToString("#0.######");
        }
        #endregion

        #region PdfToBase64 
        /// <summary>
        /// PdfToBase64
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string PdfToBase64(string filePath)
        {
            Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
            jObject.Add("FileType", ".pdf");
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath.Trim()))
                {
                    if (File.Exists(filePath))
                    {
                        FileStream fileStream = new FileStream(filePath, FileMode.Open);
                        byte[] bt = new byte[fileStream.Length];
                        fileStream.Read(bt, 0, bt.Length);
                        fileStream.Close();
                        jObject.Add("Reason", "PdfToBase64成功！");
                        jObject.Add("ResultCode", 0000);
                        jObject.Add("Success", true);
                        jObject.Add("FileBase", Convert.ToBase64String(bt));
                    }
                    else
                    {
                        jObject.Add("Reason", string.Format("PdfToBase64失败,{0}文件不存在！", filePath));
                        jObject.Add("ResultCode", 9999);
                        jObject.Add("Success", false);
                        jObject.Add("FileBase", string.Empty);
                    }
                }
                else
                {
                    jObject.Add("Reason", string.Format("PdfToBase64失败,{0}文件路径为空！", filePath));
                    jObject.Add("ResultCode", 9999);
                    jObject.Add("Success", false);
                    jObject.Add("FileBase", string.Empty);
                }
                return JsonConvert.SerializeObject(jObject);
            }
            catch (Exception ex)
            {
                jObject.Add("Reason", string.Format("PdfToBase64失败,失败原因{0}", ex.Message));
                jObject.Add("ResultCode", 9999);
                jObject.Add("Success", false);
                jObject.Add("FileBase", string.Empty);
                return JsonConvert.SerializeObject(jObject);
            }
        }
        #endregion

        #region JpgToBase64 
        /// <summary>
        /// JpgToBase64
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string JpgToBase64(string filePath)
        {
            Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
            jObject.Add("FileType", ".jpg");
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath.Trim()))
                {
                    if (File.Exists(filePath))
                    {
                        Bitmap bmp = new Bitmap(filePath);
                        MemoryStream ms = new MemoryStream();
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] arr = new byte[ms.Length];
                        ms.Position = 0;
                        ms.Read(arr, 0, (int)ms.Length);
                        ms.Close();
                        jObject.Add("Reason", "JpgToBase64成功！");
                        jObject.Add("ResultCode", 0000);
                        jObject.Add("Success", true);
                        jObject.Add("FileBase", Convert.ToBase64String(arr));
                    }
                    else
                    {
                        jObject.Add("Reason", string.Format("JpgToBase64失败,{0}文件不存在！", filePath));
                        jObject.Add("ResultCode", 9999);
                        jObject.Add("Success", false);
                        jObject.Add("FileBase", string.Empty);
                    }
                }
                else
                {
                    jObject.Add("Reason", string.Format("JpgToBase64失败,{0}文件路径为空！", filePath));
                    jObject.Add("ResultCode", 9999);
                    jObject.Add("Success", false);
                    jObject.Add("FileBase", string.Empty);
                }
                return JsonConvert.SerializeObject(jObject);
            }
            catch (Exception ex)
            {
                jObject.Add("Reason", string.Format("JpgToBase64失败,失败原因{0}", ex.Message));
                jObject.Add("ResultCode", 9999);
                jObject.Add("Success", false);
                jObject.Add("FileBase", string.Empty);
                return JsonConvert.SerializeObject(jObject);
            }
        }
        #endregion



        /* #region GetNewDataTable 执行DataTable中的查询返回新的DataTable
         /// <summary>
         /// 执行DataTable中的查询返回新的DataTable
         /// </summary>
         /// <param name="dt">源数据DataTable</param>
         /// <param name="condition">查询条件</param>
         /// <returns></returns>
         public static DataTable GetNewDataTable(DataTable dt, string condition)
         {
             DataTable newdt = new DataTable();
             dt.DefaultView.RowFilter = condition;
             newdt = dt.DefaultView.ToTable();


             return newdt;//返回的查询结果
         }
         #endregion
         */

    }
}

