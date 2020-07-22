using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public class DataGridViewHelper
    {
        #region DataGridViewCheck
        /// <summary>
        /// dgvSubDt_DataBindingComplete
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="isOpt"></param>
        public static void DataGridViewCellContentCheck(System.Windows.Forms.DataGridView dgv,
                                                        System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                if (rowIndex == -1)
                    return;
                string headerName = dgv.Columns[dgv.CurrentCell.ColumnIndex].Name;
                if (headerName != "Opt")
                    return;
                if (dgv.Rows.Count <= 0)
                    return;
                string selectValue = dgv.Rows[rowIndex].Cells["Opt"].EditedFormattedValue.ToString();
                dgv.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Turquoise;  //背景绿色
                if (selectValue == "True")
                {
                    for (int j = 0; j < dgv.Rows.Count; j++)
                    {
                        if (string.Equals(dgv.Rows[j].Cells["Opt"].EditedFormattedValue.ToString(), "True"))
                        {
                            if (!int.Equals(rowIndex, j)) //行号不相等
                            {
                                dgv.EndEdit();
                                //去掉勾选
                                System.Windows.Forms.DataGridViewCheckBoxCell checkCell = (System.Windows.Forms.DataGridViewCheckBoxCell)dgv.Rows[j].Cells["Opt"];
                                Boolean flag = Convert.ToBoolean(checkCell.Value);
                                if (flag)
                                    checkCell.Value = false;
                                string colorFlag = string.Empty;
                                if (dgv.Columns.Contains("ColorFlag"))
                                     colorFlag = dgv.Rows[j].Cells["ColorFlag"].EditedFormattedValue.ToString();
                                switch (colorFlag)
                                {
                                    case "green":
                                        dgv.Rows[j].DefaultCellStyle.BackColor = System.Drawing.Color.MediumAquamarine;
                                        break;
                                    case "red":
                                        dgv.Rows[j].DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                                        break;
                                    case "yellow":
                                        dgv.Rows[j].DefaultCellStyle.BackColor = System.Drawing.Color.Wheat;
                                        break;
                                    default:
                                        dgv.Rows[j].DefaultCellStyle.BackColor = System.Drawing.Color.Empty;
                                        break;
                                }

                                dgv.EndEdit();
                                //return;
                            }
                        }
                    }
                    //Util.DataGridViewHelper.DataGridViewDataBindingComplete(dgv);
                }
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("DataGridViewCellContentCheck 勾选操作失败，原因：{0}", ex.Message);
                Log4netUtil.Log4NetHelper.Info(logMessage, @"Exception");
            }
        }
        #endregion




        #region SetDataGridViewHeaderText 初始化DataGridView数据集表头
        /// <summary>
        /// 初始化数据集表头
        /// </summary>
        /// <param name="dgv">DataGridView</param>
        /// <param name="strSql">表头定义sql语句</param>
        public static void SetDataGridViewHeaderText(System.Windows.Forms.DataGridView dgv, System.Data.DataTable HeaderDt)
        {
            setDataGridViewHeaderText(dgv, HeaderDt);
        }
        private static void setDataGridViewHeaderText(System.Windows.Forms.DataGridView dgv, System.Data.DataTable HeaderDt)
        {
            try
            {
                using (System.Data.DataSet ds = new System.Data.DataSet())
                {
                    ds.Tables.Add(HeaderDt);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int intI = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string fdName = ds.Tables[0].Rows[i]["FdName"].ToString().Trim();
                            string fdDesc = ds.Tables[0].Rows[i]["FdDesc"].ToString().Trim();
                            //string fdType = ds.Tables[0].Rows[i]["FdType"].ToString().Trim();
                            //string fdSize = ds.Tables[0].Rows[i]["FdSize"].ToString().Trim();
                            //string fdDec = ds.Tables[0].Rows[i]["FdDec"].ToString().Trim();
                            string isCheckBox = ds.Tables[0].Rows[i]["IsCheckBox"].ToString().Trim();
                            string isVisible = ds.Tables[0].Rows[i]["IsVisible"].ToString().Trim();
                            string isReadOnly = ds.Tables[0].Rows[i]["IsReadOnly"].ToString().Trim();
                            if (isCheckBox == "True") //是否复选框
                            {
                                System.Windows.Forms.DataGridViewCheckBoxColumn columncb = new System.Windows.Forms.DataGridViewCheckBoxColumn();
                                columncb.HeaderText = fdDesc;
                                columncb.Name = fdName;
                                columncb.TrueValue = 1;
                                columncb.FalseValue = 0;
                                columncb.DataPropertyName = fdName;
                                dgv.Columns.Add(columncb);
                                if (isVisible == "False")
                                    dgv.Columns[fdName].Visible = false;
                                if (isReadOnly == "True")
                                    dgv.Columns[fdName].ReadOnly = true;
                                dgv.Columns[fdName].DisplayIndex = i;

                            }
                            else
                            {
                                intI = dgv.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn()); //添加列
                                dgv.Columns[intI].DataPropertyName = fdName;
                                dgv.Columns[intI].HeaderText = fdDesc;
                                dgv.Columns[intI].Name = fdName;
                                dgv.Columns[intI].Width = 78;
                                if (isVisible == "False")
                                    dgv.Columns[fdName].Visible = false;
                                if (isReadOnly == "True")
                                    dgv.Columns[fdName].ReadOnly = true;
                                dgv.Columns[fdName].DisplayIndex = i;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("初始化表头失败，原因：" + ex.Message,
                                                     "错误提示",
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Hand,
                                                     System.Windows.Forms.MessageBoxDefaultButton.Button1);
            }
        }
        #endregion


        #region SetDataGridViewAttributes DataGridView 属性配制
        /// <summary>
        /// DataGridView 属性配制
        /// </summary>
        /// <param name="dgv">DataGridView</param>
        /// <param name="ReadOnly">是否只读</param>
        /// <param name="unHeadSequence">是否禁止点击头部排序</param>
        public static void SetDataGridViewAttributes(System.Windows.Forms.DataGridView dgv, bool ReadOnly, bool unHeadSequence)
        {
            setDataGridViewAttributes(dgv, ReadOnly, unHeadSequence);
        }
        private static void setDataGridViewAttributes(System.Windows.Forms.DataGridView dgv, bool ReadOnly, bool unHeadSequence)
        {
            dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            //dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;  //序号自适应
            //dgv.RowHeadersVisible = false;
            dgv.AutoGenerateColumns = false;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;  //设置列标题不换行
            dgv.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter; //设定标题居中


            dgv.MultiSelect = false;  //禁止多选
            dgv.AllowUserToAddRows = false; //不允许用户自行增加行
            dgv.AllowUserToDeleteRows = false; //不允许用户自行删除行
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;  //选择一整行
            dgv.AllowUserToAddRows = false;   //去掉最后空白行
            dgv.ReadOnly = ReadOnly;
            dgv.AutoGenerateColumns = false;
            //禁止点击头部排序
            if (unHeadSequence)
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    dgv.Columns[i].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                }
            }
            dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells; // AllCells;  // 设定包括Header和所有单元格的列宽自动调整
            //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;  //DisplayedCells
            dgv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;   //ColumnHeadersHeightSizeMode 设置成EnableResizing
            dgv.AutoGenerateColumns = false;

        }
        #endregion


        #region DataGridViewSelectedRowsToDataTable  指定行号转成DataTable
        /// <summary>
        /// DataGridViewSelectedRowsToDataTable
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static System.Data.DataTable DataGridViewSelectedRowsToDataTable(System.Windows.Forms.DataGridView dgv, int index)
        {
            System.Data.DataTable TotalDT = (System.Data.DataTable)dgv.DataSource;
            System.Data.DataTable gridSelectDT = TotalDT.Clone();
            System.Data.DataRow dr = TotalDT.Rows[index];
            gridSelectDT.ImportRow(dr);
            return gridSelectDT;
        }
        #endregion

        #region DataGridViewDataBindingComplete
        /// <summary>
        /// DataGridViewDataBindingComplete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DataGridViewDataBindingComplete1(System.Windows.Forms.DataGridView dgv)//, System.Windows.Forms.DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                if (dgv.Columns["ColorFlag"] == null)
                    return;
                foreach (System.Windows.Forms.DataGridViewRow dgvr in dgv.Rows)
                {
                    bool columnsFlag = true;
                    if (dgv.Columns.Contains("Opt"))
                    {
                        string opt = string.Empty;
                        if (dgv.Columns["Opt"] == null)
                            columnsFlag = false;
                        if (columnsFlag)
                        {
                            opt = object.Equals(dgvr.Cells["Opt"].Value, null) ? "0" : dgvr.Cells["Opt"].Value.ToString();
                            if (string.Equals(opt, "1"))
                            {
                                dgvr.DefaultCellStyle.BackColor = System.Drawing.Color.Turquoise;  //背景绿色
                                continue;
                            }
                        }
                    }
                    switch (dgvr.Cells["ColorFlag"].Value.ToString().ToLower())
                    {
                        case "green":
                            dgvr.DefaultCellStyle.BackColor = System.Drawing.Color.MediumAquamarine;
                            break;
                        case "red":
                            dgvr.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                            break;
                        case "yellow":
                            dgvr.DefaultCellStyle.BackColor = System.Drawing.Color.Wheat;
                            break;
                        default:
                            dgvr.DefaultCellStyle.BackColor = System.Drawing.Color.Empty;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("dgvSubDt_DataBindingComplete 操作有误，原因：{0}", ex.Message);
                Log4netUtil.Log4NetHelper.Info(logMessage, @"Exception");
            }

        }
        #endregion

        #region DataGridViewDataBindingComplete2
        /// <summary>
        /// DataGridViewDataBindingComplete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DataGridViewDataBindingComplete2(System.Windows.Forms.DataGridView dgv, string optColor = "#FF40E0D0")//, System.Windows.Forms.DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                bool isColorFlag = true;
                if (dgv.Columns["ColorFlag"] == null)
                    isColorFlag = false;
                dgv.EndEdit();
                foreach (System.Windows.Forms.DataGridViewRow dgvr in dgv.Rows)
                {
                    bool columnsFlag = true;
                    if (dgv.Columns.Contains("Opt"))
                    {
                        string opt = string.Empty;
                        if (dgv.Columns["Opt"] == null)
                            columnsFlag = false;
                        if (columnsFlag)
                        {
                            opt = object.Equals(dgvr.Cells["Opt"].Value, null) ? "0" : dgvr.Cells["Opt"].Value.ToString();
                            if (string.Equals(opt, "1"))
                            {
                                if (string.IsNullOrEmpty(optColor))
                                    optColor = "#FF40E0D0";
                                try
                                {
                                    dgvr.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(optColor); //System.Drawing.Color.Turquoise;  //背景绿色
                                }
                                catch
                                {
                                    dgvr.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;  //System.Drawing.Color.Empty;
                                }
                                continue;
                            }
                            else
                                dgvr.DefaultCellStyle.BackColor = System.Drawing.Color.Empty;
                        }
                    }
                    if (!isColorFlag)
                        continue;
                    string colorFlag = dgvr.Cells["ColorFlag"].Value.ToString().ToLower();
                    if (string.Equals(colorFlag, "green"))
                        colorFlag = "#FF66CDAA";
                    else if (string.Equals(colorFlag, "red"))
                        colorFlag = "#FFF08080";
                    else if (string.Equals(colorFlag, "yellow"))
                        colorFlag = "#FFF5DEB3";

                    if (!string.IsNullOrEmpty(colorFlag))
                    {
                        try
                        {
                            dgvr.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(colorFlag); //System.Drawing.Color.Turquoise;  //背景绿色
                        }
                        catch
                        {
                            dgvr.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;// System.Drawing.Color.Empty;
                        }
                    }
                }
                dgv.EndEdit();
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("DataGridViewDataBindingComplete2 操作有误，原因：{0}", ex.Message);
                Log4netUtil.Log4NetHelper.Info(logMessage, @"Exception");
            }

        }
        #endregion

        #region DataGridViewDataBindingComplete3   DataGridView色标 指定颜色
        /// <summary>
        /// DataGridViewDataBindingComplete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DataGridViewDataBindingComplete3(System.Windows.Forms.DataGridView dgv, string colorFlag)//, System.Windows.Forms.DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                dgv.EndEdit();
                foreach (System.Windows.Forms.DataGridViewRow dgvr in dgv.Rows)
                {
                    if (!string.IsNullOrEmpty(colorFlag))
                    {
                        try
                        {
                            dgvr.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(colorFlag); //System.Drawing.Color.Turquoise;  //背景绿色
                        }
                        catch
                        {
                            dgvr.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control; //System.Drawing.Color.Empty;
                        }
                    }
                }
                dgv.EndEdit();
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("DataGridViewDataBindingComplete3 操作有误，原因：{0}", ex.Message);
                Log4netUtil.Log4NetHelper.Info(logMessage, @"Exception");
            }

        }
        #endregion

        #region DataGridViewRowHeadersWidth  行号设置
        /// <summary>
        /// DataGridViewRowHeadersWidth
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public static int DataGridViewRowHeadersWidth(System.Windows.Forms.DataGridView dgv)
        {
            int count = dgv.Rows.Count;
            int rowHeadersWidth = 40;

            if (count > 9)
                rowHeadersWidth = 46;
            if (count > 99)
                rowHeadersWidth = 52;
            if (count > 999)
                rowHeadersWidth = 58;
            if (count > 9999)
                rowHeadersWidth = 64;
            if (count > 99999)
                rowHeadersWidth = 70;
            dgv.RowHeadersWidth = rowHeadersWidth;
            return rowHeadersWidth;
        }

        public static int DataGridViewRowHeadersWidth(System.Windows.Forms.DataGridView dgv, System.Windows.Forms.DataGridView dgvSub)
        {
            int count = dgv.Rows.Count;
            if (dgvSub.Rows.Count > count)
                count = dgvSub.Rows.Count;
            int rowHeadersWidth = 40;

            if (count > 9)
                rowHeadersWidth = 46;
            if (count > 99)
                rowHeadersWidth = 52;
            if (count > 999)
                rowHeadersWidth = 58;
            if (count > 9999)
                rowHeadersWidth = 64;
            if (count > 99999)
                rowHeadersWidth = 70;
            dgv.RowHeadersWidth = rowHeadersWidth;
            dgvSub.RowHeadersWidth = rowHeadersWidth;
            return rowHeadersWidth;
        }
        #endregion

    }
}
