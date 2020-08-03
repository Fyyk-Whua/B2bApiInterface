using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class CommonBLL : Facade.ICommonBLL
    {
        #region GetIniValue 获取INI 
        /// <summary>
        /// 获取INI 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetIniValue(string section, string key)
        {
            //string ConfigFile = Application.StartupPath.ToString() + "\\Config.ini";
            return Util.INIOperationClass.INIGetStringValue(Util.DalConst._ConfigFile, section, key, null);
        }
        #endregion 

        #region INIWriteValue 写入配制文件
        /// <summary>
        /// 写入配制文件
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool INIWriteValue(string oldValue, string newValue, string section, string key)
        {
            bool flag = false;
            if (oldValue != newValue)
            {
                Util.INIOperationClass.INIWriteValue(Util.DalConst._ConfigFile, section, key, newValue);
                flag = true;
                /*if (!newValue.Trim().Equals(string.Empty) || newValue.Length != 0)
                {
                    INIOperationClass.INIWriteValue(DalConst._ConfigFile, section, key, newValue);
                    flag = true;
                }*/
            }
            return flag;
        }

        public bool INIWriteValue(string oldValue, string newValue, string section, string key, bool isEmpty)
        {
            bool flag = false;
            if (oldValue != newValue)
            {
                if (!string.IsNullOrEmpty(newValue))
                {
                    Util.INIOperationClass.INIWriteValue(Util.DalConst._ConfigFile, section, key, newValue);
                    flag = true;
                }
                if (isEmpty)
                {
                    Util.INIOperationClass.INIWriteValue(Util.DalConst._ConfigFile, section, key, newValue);
                    flag = true;
                }
            }
            return flag;
        }
        #endregion 

        #region  INIWritePwdValue 将密码经过加密写入配制文件
        /// <summary>
        /// INIWritePwdValue
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool INIWritePwdValue(string oldValue, string newValue, string section, string key)
        {
            bool flag = false;
            if (oldValue != newValue)
            {
                if (!newValue.Trim().Equals(string.Empty) || newValue.Length != 0)
                {
                    string newValueEn = Util.EncAndDec.DESEncrypt(newValue);  //加密
                    Util.INIOperationClass.INIWriteValue(Util.DalConst._ConfigFile, section, key, newValueEn);
                    flag = true;
                }

            }
            return flag;
        }
        #endregion 

        #region SetDataGridViewHeaderText 初始化DataGridView数据集表头
        /// <summary>
        /// 初始化数据集表头
        /// </summary>
        /// <param name="dgv">DataGridView</param>
        /// <param name="strSql">表头定义sql语句</param>
        public void SetDataGridViewHeaderText(System.Windows.Forms.DataGridView dgv, System.Data.DataTable HeaderDt)
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
                            string fdType = ds.Tables[0].Rows[i]["FdType"].ToString().Trim();
                            string fdSize = ds.Tables[0].Rows[i]["FdSize"].ToString().Trim();
                            string fdDec = ds.Tables[0].Rows[i]["FdDec"].ToString().Trim();
                            string isCheckBox = ds.Tables[0].Rows[i]["IsCheckBox"].ToString().Trim();
                            string isVisible = ds.Tables[0].Rows[i]["IsVisible"].ToString().Trim();
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
                                {
                                    dgv.Columns[fdName].Visible = false;
                                }
                            }
                            else
                            {
                                intI = dgv.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn()); //添加列
                                dgv.Columns[intI].DataPropertyName = fdName;
                                dgv.Columns[intI].HeaderText = fdDesc;
                                dgv.Columns[intI].Name = fdName;
                                dgv.Columns[intI].Width = 78;
                                if (isVisible == "False")
                                {
                                    dgv.Columns[fdName].Visible = false;
                                }
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

        #region GetDataGridViewHeader 获取表头定义
        /// <summary>
        /// 获取表头定义
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="ModuleID"></param>
        /// <param name="IsMaintain"></param>
        public void GetDataGridViewHeader(System.Windows.Forms.DataGridView dgv, string DgvName)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            dt = Util.DataTableHelper.GetDataGridViewHeader(DgvName);
            if (dt == null && dt.Rows.Count <= 0)
                return;
            else
                Util.DataGridViewHelper.SetDataGridViewHeaderText(dgv, dt);
        }
        #endregion

        #region GetDataTableViewHeader 获取表头定义
        /// <summary>
        /// 获取表头定义
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="ModuleID"></param>
        /// <param name="IsMaintain"></param>
        public System.Data.DataTable GetDataTableViewHeader(string DgvName)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            DALFactory.FactoryDAL fact = new DALFactory.FactoryDAL();
            IDAL.ICommonDAL idal = fact.CreateCommonDAL();
            return idal.GetDataGridViewHeader(DgvName);

        }
        #endregion


        #region SetDataGridViewAttributes DataGridView 属性配制
        /// <summary>
        /// DataGridView 属性配制
        /// </summary>
        /// <param name="dgv">DataGridView</param>
        /// <param name="ReadOnly">是否只读</param>
        /// <param name="unHeadSequence">是否禁止点击头部排序</param>
        public void SetDataGridViewAttributes(System.Windows.Forms.DataGridView dgv, bool ReadOnly, bool unHeadSequence)
        {
            dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;  //序号自适应
            //dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;  //设置列标题不换行
            dgv.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter; //设定标题居中
            dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;  // 设定包括Header和所有单元格的列宽自动调整
            dgv.MultiSelect = false;  //禁止多选
            dgv.AllowUserToAddRows = false; //不允许用户自行增加行
            dgv.AllowUserToDeleteRows = false; //不允许用户自行删除行
            //dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;  //选择一整行
            dgv.AllowUserToAddRows = false;   //去掉最后空白行
            dgv.ReadOnly = ReadOnly;
            dgv.AutoGenerateColumns = false;
            dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;  //无边框
            dgv.RowHeadersWidth = 40;
            dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //禁止点击头部排序
            if (unHeadSequence)
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    dgv.Columns[i].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                }
            }
        }
        #endregion

        #region SetDataGridViewSelectAll DataGridView全选
        /// <summary>
        /// SetDataGridViewSelectAll DataGridView全选
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="color"></param>
        public void SetDataGridViewSelectAll(System.Windows.Forms.DataGridView dgv, System.Drawing.Color color)
        {
            if (dgv.Rows.Count <= 0)
                return;
            if (!dgv.Columns.Contains("Opt"))
                return;
            dgv.EndEdit();
            /*using (DataTable dt = (DataTable)dgv.DataSource)
            {
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    dr["Opt"] = 1;
                }
                dgv.DataSource = dt;
            }*/
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
                        if (string.Equals(opt, "False"))
                            opt = "0";
                        else if (string.Equals(opt, "True"))
                            opt = "1";
                        if (string.Equals(opt, "0"))
                        {
                            dgvr.Cells["Opt"].Value = true;
                            dgvr.DefaultCellStyle.BackColor = color;//  System.Drawing.ColorTranslator.FromHtml(optColor); //System.Drawing.Color.Turquoise;  //背景绿色
                            continue;
                        }
                    }
                }
            }
            dgv.EndEdit();

        }






        #endregion

        #region  SetDataGridViewUnSelectAll 全不选
        /// <summary>
        /// SetDataGridViewUnSelectAll 全不选
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="color"></param>
        public void SetDataGridViewUnSelectAll(System.Windows.Forms.DataGridView dgv, System.Drawing.Color color)
        {
            if (dgv.Rows.Count > 0)
            {
                dgv.EndEdit();
                bool isColorFlag = true;
                if (dgv.Columns["ColorFlag"] == null)
                    isColorFlag = false;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if ((Convert.ToBoolean(dgv.Rows[i].Cells["Opt"].Value) == true))
                    {
                        dgv.Rows[i].Cells["Opt"].Value = false;// "True";
                        if (isColorFlag)
                        {
                            string colorFlag = dgv.Rows[i].Cells["ColorFlag"].Value.ToString().ToLower();
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
                                    dgv.Rows[i].DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(colorFlag); //System.Drawing.Color.Turquoise;  //背景绿色
                                }
                                catch
                                {
                                    dgv.Rows[i].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;// System.Drawing.Color.Empty;
                                }
                            }
                            else
                                dgv.Rows[i].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                        }
                        else
                            dgv.Rows[i].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                    }
                    else
                        continue;
                }
                dgv.EndEdit();
            }

        }
        #endregion

        #region GetDgvDtAll
        /// <summary>
        /// GetDgvDtAll
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="searchParam"></param>
        public void GetDgvDtAll(Log4netUtil.LogAppendToForms logAppendToForms, System.Windows.Forms.DataGridView dgv, Model.SearchParam searchParam)
        {
            DALFactory.FactoryDAL fact = new DALFactory.FactoryDAL();
            IDAL.ICommonDAL idal = fact.CreateCommonDAL();
            System.Data.DataTable dataTable = idal.GetDgvDtAll(logAppendToForms, searchParam);
            dgv.DataSource = dataTable;
        }
        #endregion


        #region GetDataTableAll
        /// <summary>
        /// GetDataTableAll
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public System.Data.DataTable GetDataTableAll(Log4netUtil.LogAppendToForms logAppendToForms, Model.SearchParam searchParam)
        {
            DALFactory.FactoryDAL fact = new DALFactory.FactoryDAL();
            IDAL.ICommonDAL idal = fact.CreateCommonDAL();
            return idal.GetDgvDtAll(logAppendToForms, searchParam);
        }
        #endregion

        #region ErpWriteback
        /// <summary>
        /// ErpWriteback
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="writebackParam"></param>
        /// <returns></returns>
        public bool ErpWriteback(Log4netUtil.LogAppendToForms logAppendToForms, Model.WritebackParam writebackParam)
        {
            DALFactory.FactoryDAL fact = new DALFactory.FactoryDAL();
            IDAL.ICommonDAL idal = fact.CreateCommonDAL();
            return idal.ErpWriteback(logAppendToForms, writebackParam);
        }
        #endregion

        #region BulkInsertDatabase
        /// <summary>
        /// BulkInsertDatabase
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="dt"></param>
        /// <param name="insertTableName"></param>
        /// <returns></returns>
        public int BulkInsertDatabase(Log4netUtil.LogAppendToForms logAppendToForms,
                                       Model.JobEntity jobInfo,
                                       System.Data.DataTable dt,string insertTableName)
        {
            DALFactory.FactoryDAL fact = new DALFactory.FactoryDAL();
            IDAL.ICommonDAL idal = fact.CreateCommonDAL();
            return idal.BulkCopyInsert(logAppendToForms, jobInfo,dt, insertTableName);
        }
        #endregion

        #region BulkInsertDatabase
        /// <summary>
        /// BulkInsertDatabase
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="dt"></param>
        /// <param name="insertTableName"></param>
        /// <returns></returns>
        public int BulkInsertDatabase(Log4netUtil.LogAppendToForms logAppendToForms,
                                       Model.JobEntity jobInfo,
                                       string insertTableName, string strSql)
        {
            DALFactory.FactoryDAL fact = new DALFactory.FactoryDAL();
            IDAL.ICommonDAL idal = fact.CreateCommonDAL();
            return idal.BulkCopyInsert(logAppendToForms, jobInfo, insertTableName, strSql);
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public string ExecuteScalar(Log4netUtil.LogAppendToForms logAppendToForms, Model.SearchParam searchParam)
        {
            DALFactory.FactoryDAL fact = new DALFactory.FactoryDAL();
            IDAL.ICommonDAL idal = fact.CreateCommonDAL();
            return idal.ExecuteScalar(logAppendToForms, searchParam);
        }
        #endregion

        #region FtpDownloadToFile
        /// <summary>
        /// FtpDownloadToFile
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="ftpfilepath"></param>
        /// <param name="fileSavePath"></param>
        /// <param name="fileSaveName"></param>
        /// <returns></returns>
        public string FtpDownloadToFile(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo,
                                string ftpfilepath, string fileSavePath, string fileSaveName)
        {
            Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
            var ftp = new Util.FluentFtpHelper(jobInfo.ConfigInfo.FtpHostIP, jobInfo.ConfigInfo.FtpPort, jobInfo.ConfigInfo.FtpUserName, jobInfo.ConfigInfo.FtpPassword);
            if (!ftp.Connect())
            {
                jObject.Add("code", "9998");
                jObject.Add("msg", "Ftp连接异常!");
                jObject.Add("data", string.Empty);
                return jObject.ToString();
            }
            if (!ftp.isConnected())
            {
                jObject.Add("code", "9998");
                jObject.Add("msg", "连接Ftp失败!");
                jObject.Add("data", string.Empty);
                return jObject.ToString();
            }
            string localDic = string.Format("{0}\\{1}", fileSavePath, fileSaveName);
            string remotePath = ftpfilepath;
            if (ftp.DownloadFile(fileSavePath, fileSaveName, remotePath))
            {
                jObject.Add("code", "0000");
                jObject.Add("msg", "Ftp下载文件成功!");
                jObject.Add("data", string.Empty);
            }
            else
            {
                jObject.Add("code", "9999");
                jObject.Add("msg", string.Format("localDic：{0} ;remotePath:{1} ;Ftp下载文件失败!", localDic, remotePath));
                jObject.Add("data", string.Empty);
            }
            Log4netUtil.Log4NetHelper.Error(String.Format(@"ftpDownloadToFile->下载文件 Result:{0}", jObject.ToString()), @"Ftp");
            return jObject.ToString();
        }
        #endregion


        #region CallDtyApi
        /// <summary>
        /// CallDtyApi
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="dtyApi"></param>
        /// <param name="jobInfo"></param>
        /// <param name="apiName"></param>
        /// <param name="requestData"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool CallDtyApi(Log4netUtil.LogAppendToForms logAppendToForms,
                                Util.DtyApi dtyApi,
                                Model.JobEntity jobInfo, string apiName, string requestData, out string result)
        {
            string logMessage = string.Empty;
            //string passKey = "9A7OTA7JTAAJOE3153J17TEEAAJOTJO9";
            result = dtyApi.DtyEDIApiByJson(apiName, requestData);
            Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(result);
            if (string.Equals(resultJObject.Value<string>("ErrCode"), "000"))
            {
                logMessage = string.Format("【{0}_{1}】 result:{2} 成功！ ", jobInfo.JobCode, jobInfo.JobName.ToString(), result);
                LogMessage(logAppendToForms, true, logMessage, apiName);
                return true;
            }
            else
            {
                logMessage = string.Format("【{0}_{1}】 result:{2} ;失败！原因：{3} ", jobInfo.JobCode, jobInfo.JobName.ToString(), result, resultJObject.Value<string>("ErrMsg"));
                LogError(logAppendToForms, true, logMessage, apiName);
                return false;
            }
        }
        #endregion

        #region LogMessage
        /// <summary>
        /// LogMessage
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="isDebug"></param>
        /// <param name="logMessage"></param>
        /// <param name="jobType"></param>
        private void LogMessage(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
            if (isDebug)
                Log4netUtil.LogDisplayHelper.LogMessage(logAppendToForms, logMessage);
        }
        #endregion

        #region LogWarning
        /// <summary>
        /// LogWarning
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="isDebug"></param>
        /// <param name="logMessage"></param>
        /// <param name="jobType"></param>
        private void LogWarning(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
            Log4netUtil.Log4NetHelper.Warn(logMessage, "QuartzManager");
            if (isDebug)
                Log4netUtil.LogDisplayHelper.LogWarning(logAppendToForms, logMessage);
        }
        #endregion

        #region LogError
        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="isDebug"></param>
        /// <param name="logMessage"></param>
        /// <param name="jobType"></param>
        private void LogError(Log4netUtil.LogAppendToForms logAppendToForms, bool isDebug, string logMessage, string jobType)
        {
            Log4netUtil.Log4NetHelper.Info(logMessage, "QuartzManager");
            Log4netUtil.Log4NetHelper.Error(logMessage, "QuartzManager");
            if (isDebug)
                Log4netUtil.LogDisplayHelper.LogError(logAppendToForms, logMessage);
        }
        #endregion
    }
}
