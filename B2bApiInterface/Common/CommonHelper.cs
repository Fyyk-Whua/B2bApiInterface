
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace HebfdeaInterface.Common
{
    public class CommonHelper
    {

        #region StringIsNullOrEmpty
        /// <summary>
        /// StringIsNullOrEmpty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringIsNullOrEmpty(string value)
        {
            try
            {
                return string.IsNullOrEmpty(value) ? string.Empty : value;
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("Util.CommonHelper.StringIsNullOrEmpty() 执行失败，原因：{0}", ex.Message);
                Log4netUtil.Log4NetHelper.Info(logMessage, @"Exception");
                return string.Empty;
            }
        }
        #endregion

        #region StringRemoveChinese 移除字符窜中的中文
        /// <summary>
        /// StringRemoveChinese 移除字符窜中的中文
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringRemoveChinese(string value)
        {
            return Regex.Replace(value, @"[\u4e00-\u9fa5]", ""); //去除汉字  
        }
        #endregion

        #region StringExtractChinese 提取字符窜中的中文
        /// <summary>
        /// StringExtractChinese 提取字符窜中的中文
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringExtractChinese(string value)
        {
            return Regex.Replace(value, @"[^\u4e00-\u9fa5]", ""); //只留汉字
        }
        #endregion

        #region StringExtractNumber 提取字符窜中的数字  
        /// <summary>
        /// StringExtractNumber 提取字符窜中的数字  
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringExtractNumber(string value)
        {
            return Regex.Replace(value, "[0-9]", "", RegexOptions.IgnoreCase); ////取出字符串中所有的数字  
        }
        #endregion

        #region StringExtractEnglish 提取字符窜中的英文字母   
        /// <summary>
        /// StringExtractEnglish 提取字符窜中的英文字母   
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringExtractEnglish(string value)
        {
            return Regex.Replace(value, "[a-z]", "", RegexOptions.IgnoreCase);//取出字符串中所有的英文字母   
        }
        #endregion

        #region PhotoImageInsert
        /// <summary>
        /// PhotoImageInsert
        /// </summary>
        /// <param name="imgPhoto">图片对象</param>
        /// <returns>二进制</returns>
        public static byte[] PhotoImageInsert(System.Drawing.Image imgPhoto)
        {
            //将Image转换成流数据，并保存为byte[]
            MemoryStream mstream = new MemoryStream();
            imgPhoto.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] byData = new Byte[mstream.Length];
            mstream.Position = 0;
            mstream.Read(byData, 0, byData.Length);
            mstream.Close();
            return byData;
        }
        #endregion 

        #region SetCboDefault
        /// <summary>
        /// SetCboDefault
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultName"></param>
        public static void SetCboDefault(ComboBox obj, string defaultName)
        {
            for (int i = 0; i < obj.Items.Count; i++)
            {
                string value = ((ListItem)obj.Items[i]).Text;
                if (string.Equals(value, defaultName))
                {
                    obj.SelectedIndex = i;
                    break;
                }
            }
        }
        #endregion

        #region SetCboDefault
        /// <summary>
        /// SetCboDefault
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultName"></param>
        public static void SetCboDefault(ComboBox obj, string defaultName, string strKey)
        {
            for (int i = 0; i < obj.Items.Count; i++)
            {
                string value = ((ListItem)obj.Items[i]).Text;
                string afterValue = CopyFromAfterStr(value, "|", false);
                string beforeValue = CopyFromBeforeStr(value, "|", false);
                if (string.Equals(((ListItem)obj.Items[i]).Text, defaultName) ||
                   string.Equals(afterValue, defaultName) ||
                   string.Equals(beforeValue, defaultName))
                {
                    obj.SelectedIndex = i;
                    break;
                }
            }
        }
        #endregion

        #region SetCboDataBind
        /// <summary>
        /// SetCboDataBind
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="dt"></param>
        /// <param name="isCombinedDisplay"></param>
        /// <param name="isFirstLine"></param>
        public static void SetCboDataBind(System.Windows.Forms.ComboBox cmb, System.Data.DataTable dt, bool isCombinedDisplay, bool isFirstLine)
        {
            if (dt == null)
                return;
            ListItem item;
            if (isFirstLine)
            {
                item = new ListItem();
                item.Text = "";
                item.Value = "";
                cmb.Items.Add(item);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new ListItem();
                if (isCombinedDisplay)
                    item.Text = dt.Rows[i]["DataObject"].ToString() + "|" + dt.Rows[i]["DictTypeName"].ToString();
                else
                    item.Text = dt.Rows[i]["DataObject"].ToString();
                item.Value = dt.Rows[i]["DictTypeName"].ToString();
                cmb.Items.Add(item);
            }
        }
        #endregion

        #region SetCboDataDictBind
        /// <summary>
        /// SetCboDataDictBind
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="dt"></param>
        /// <param name="isCombinedDisplay"></param>
        /// <param name="isFirstLine"></param>
        public static void SetCboDataDictBind(System.Windows.Forms.ComboBox cmb, System.Data.DataTable dt, bool isCombinedDisplay, bool isFirstLine)
        {
            if (dt == null)
                return;
            ListItem item;
            if (isFirstLine)
            {
                item = new ListItem();
                item.Text = "";
                item.Value = "";
                cmb.Items.Add(item);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new ListItem();
                if (isCombinedDisplay)
                    item.Text = dt.Rows[i]["NativeName"].ToString() + "|" + dt.Rows[i]["EnglishName"].ToString();
                else
                    item.Text = dt.Rows[i]["NativeName"].ToString();
                item.Value = dt.Rows[i]["EnglishName"].ToString();
                cmb.Items.Add(item);
            }
        }
        #endregion

        #region cmbTextUpdate
        /// <summary>
        /// cmbTextUpdate
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="dt"></param>
        /// <param name="inputValue"></param>
        /// <param name="isCombinedDisplay"></param>
        public static void CmbTextUpdate(System.Windows.Forms.ComboBox cmb, System.Data.DataTable dt, string inputValue, bool isCombinedDisplay)
        {
            cmbTextUpdate(cmb, dt, inputValue, isCombinedDisplay);
        }
        private static void cmbTextUpdate(System.Windows.Forms.ComboBox cmb, System.Data.DataTable dt, string inputValue, bool isCombinedDisplay)
        {
            if (string.IsNullOrEmpty(inputValue))
                return;

            //提前下拉，以显示搜索结果（必须要在添加项之前下拉，否则会将第一项自动添加到编辑框内 覆盖掉输入的内容）
            cmb.DroppedDown = true;      //显示下拉列表，但是显示后鼠标指针就不见了
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default; //将指针显示出来
            try
            {
                if (dt == null)
                    return;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string dataObject = dt.Rows[i]["DataObject"].ToString();
                    if (isCombinedDisplay)
                        dataObject = dt.Rows[i]["DataObject"].ToString() + "|" + dt.Rows[i]["DictTypeName"].ToString();
                    string dictTypeName = dt.Rows[i]["DictTypeName"].ToString();
                    if (Util.ExtensionMethod.Contains(dataObject, inputValue, StringComparison.OrdinalIgnoreCase))
                    {
                        for (int j = 0; j < cmb.Items.Count; j++)
                        {
                            if (((ListItem)cmb.Items[i]).Text == dataObject)
                            {
                                cmb.SelectedIndex = i;
                                return;// break;
                            }
                        }
                    }
                    else if (Util.ExtensionMethod.Contains(dictTypeName, inputValue, StringComparison.OrdinalIgnoreCase)) //(dictTypeName.Contains(inputValue))
                    {
                        for (int j = 0; j < cmb.Items.Count; j++)
                        {
                            if (((ListItem)cmb.Items[i]).Value == dictTypeName)
                            {
                                cmb.SelectedIndex = i;
                                return;// break;
                            }
                        }
                    }

                    else
                        cmb.Items.Remove(inputValue);

                }
            }
            catch { }
        }
        #endregion

        #region cmbTextUpdate
        /// <summary>
        /// cmbTextUpdate
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="dt"></param>
        /// <param name="inputValue"></param>
        /// <param name="isCombinedDisplay"></param>
        public static void CmbTextDictUpdate(System.Windows.Forms.ComboBox cmb, System.Data.DataTable dt, string inputValue, bool isCombinedDisplay)
        {
            cmbTextDictUpdate(cmb, dt, inputValue, isCombinedDisplay);
        }
        private static void cmbTextDictUpdate(System.Windows.Forms.ComboBox cmb, System.Data.DataTable dt, string inputValue, bool isCombinedDisplay)
        {
            if (string.IsNullOrEmpty(inputValue))
                return;

            //提前下拉，以显示搜索结果（必须要在添加项之前下拉，否则会将第一项自动添加到编辑框内 覆盖掉输入的内容）
            cmb.DroppedDown = true;      //显示下拉列表，但是显示后鼠标指针就不见了
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default; //将指针显示出来
            try
            {
                if (dt == null)
                    return;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string dataObject = dt.Rows[i]["NativeName"].ToString();
                    if (isCombinedDisplay)
                        dataObject = dt.Rows[i]["NativeName"].ToString() + "|" + dt.Rows[i]["EnglishName"].ToString();
                    string dictTypeName = dt.Rows[i]["EnglishName"].ToString();
                    if (Util.ExtensionMethod.Contains(dataObject, inputValue, StringComparison.OrdinalIgnoreCase))
                    {
                        for (int j = 0; j < cmb.Items.Count; j++)
                        {
                            if (((ListItem)cmb.Items[i]).Text == dataObject)
                            {
                                cmb.SelectedIndex = i;
                                return;// break;
                            }
                        }
                    }
                    else if (Util.ExtensionMethod.Contains(dictTypeName, inputValue, StringComparison.OrdinalIgnoreCase)) //(dictTypeName.Contains(inputValue))
                    {
                        for (int j = 0; j < cmb.Items.Count; j++)
                        {
                            if (((ListItem)cmb.Items[i]).Value == dictTypeName)
                            {
                                cmb.SelectedIndex = i;
                                return;// break;
                            }
                        }
                    }

                    else
                        cmb.Items.Remove(inputValue);

                }
            }
            catch { }
        }
        #endregion

        #region SetHiddenDetails 隐藏明细
        /// <summary>
        /// SetHiddenDetails
        /// </summary>
        /// <param name="pnlHead"></param>
        /// <param name="pnlCentral"></param>
        /// <param name="pnlTail"></param>
        /// <param name="rtb"></param>
        /// <param name="tsb"></param>
        /// <param name="ssr"></param>
        /// <param name="dgv"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="intervalParame"></param>
        public static void SetHiddenDetails(System.Windows.Forms.Panel pnlHead,
                                    System.Windows.Forms.Panel pnlCentral,
                                    System.Windows.Forms.Panel pnlTail,
                                    RichTextBox rtb,
                                    ToolStripButton tsb,
                                    StatusStrip ssr,
                                    DataGridView dgv,
                                    int height, int width, int intervalParame)
        {
            setHiddenDetails(pnlHead, pnlCentral, pnlTail, rtb, tsb, ssr, dgv, height, width, intervalParame);
        }
        private static void setHiddenDetails(System.Windows.Forms.Panel pnlHead,
                                     System.Windows.Forms.Panel pnlCentral,
                                     System.Windows.Forms.Panel pnlTail,
                                     RichTextBox rtb,
                                     ToolStripButton tsb,
                                     StatusStrip ssr,
                                     DataGridView dgv,
                                     int height, int width, int intervalParame)
        {
            string ConfigFile = System.Windows.Forms.Application.StartupPath.ToString() + "\\Config.ini";
            int tailPanelHeight = 300;
            int tsbHeight = 0;
            if (ssr != null)
                tsbHeight = ssr.Height;
            try
            {
                tailPanelHeight = int.Parse(Util.INIOperationClass.INIGetStringValue(ConfigFile, "UserSet", "tailHeight", null));
            }
            catch
            {
                tailPanelHeight = 300;
            }
            if (tailPanelHeight <= 100)
                tailPanelHeight = 300;
            pnlTail.Height = tailPanelHeight;
            pnlTail.Width = width - 8;
            int headPanelHeight = pnlHead.Height;
            rtb.Visible = false;
            pnlTail.Visible = true;
            dgv.Visible = true;
            pnlCentral.Height = height - headPanelHeight - tailPanelHeight - tsb.Height - tsbHeight - tsbHeight - intervalParame - 5; // -38 - 45;  IntervalParame 
            pnlCentral.Width = width - 8;
            pnlCentral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
                  | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
            tsb.Text = "隐藏明细(F)";

        }

        public static void setHiddenDetailsFormsTool(System.Windows.Forms.Panel pnlHead,
                                     System.Windows.Forms.Panel pnlCentral,
                                     System.Windows.Forms.Panel pnlTail,
                                     RichTextBox rtb,
                                     System.Windows.Forms.ToolStrip tsFormsTool,
                                     DataGridView dgv,
                                     int height, int width, int intervalParame)
        {
            string ConfigFile = System.Windows.Forms.Application.StartupPath.ToString() + "\\Config.ini";
            int tailPanelHeight = 300;
            int tsbHeight = 0;
            ///if (ssr != null)
             //   tsbHeight = ssr.Height;
            try
            {
                tailPanelHeight = int.Parse(Util.INIOperationClass.INIGetStringValue(ConfigFile, "UserSet", "tailHeight", null));
            }
            catch
            {
                tailPanelHeight = 300;
            }
            if (tailPanelHeight <= 100)
                tailPanelHeight = 300;
            pnlTail.Height = tailPanelHeight;
            pnlTail.Width = width - 8;
            int headPanelHeight = pnlHead.Height;
            rtb.Visible = false;
            pnlTail.Visible = true;
            dgv.Visible = true;
            pnlCentral.Height = height - headPanelHeight - tailPanelHeight - tsFormsTool.Height - tsbHeight - tsbHeight - intervalParame - 5; // -38 - 45;  IntervalParame 
            pnlCentral.Width = width - 8;
            pnlCentral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
                  | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
            //tsb.Text = "隐藏明细(F)";

        }
        #endregion

        #region SetDisplayDetails
        /// <summary>
        /// SetDisplayDetails
        /// </summary>
        /// <param name="headPanel"></param>
        /// <param name="centralPanel"></param>
        /// <param name="tailPanel"></param>
        /// <param name="logtextBox"></param>
        /// <param name="toolStripButton"></param>
        /// <param name="dgv"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="IntervalParame"></param>
        public static void SetDisplayDetails(System.Windows.Forms.Panel pnlHead,
                                    System.Windows.Forms.Panel pnlCentral,
                                    System.Windows.Forms.Panel pnlTail,
                                    RichTextBox rtb,
                                    ToolStripButton tsb,
                                    StatusStrip ssr,
                                    DataGridView dgv,
                                    int height, int width, int intervalParame)
        {
            setDisplayDetails(pnlHead, pnlCentral, pnlTail, rtb, tsb, ssr, dgv, height, width, intervalParame);
        }

        private static void setDisplayDetails(System.Windows.Forms.Panel pnlHead,
                                     System.Windows.Forms.Panel pnlCentral,
                                     System.Windows.Forms.Panel pnlTail,
                                     RichTextBox rtb,
                                     ToolStripButton tsb,
                                     StatusStrip ssr,
                                     DataGridView dgv,
                                     int height, int width, int intervalParame)
        //(Panel headPanel, Panel centralPanel, Panel tailPanel, RichTextBox logtextBox, ToolStripButton toolStripButton, DataGridView dgv, int height, int width, int IntervalParame)
        {
            rtb.Visible = false;
            pnlTail.Visible = false;
            dgv.Visible = false;
            int headPanelHeight = pnlHead.Height;
            //centralPanel.Dock = DockStyle.Fill;
            pnlCentral.Height = height - headPanelHeight - intervalParame - 38;
            pnlCentral.Width = width - 5;
            tsb.Text = "显示明细(F)";
        }
        #endregion


        #region StartDateTime 设置开始日期
        /// <summary>
        /// StartDateTime 设置开始日期
        /// </summary>
        /// <param name="dgv"></param>
        public static void StartDateTime(System.Windows.Forms.DateTimePicker dtp)
        {
            string configFile = System.Windows.Forms.Application.StartupPath.ToString() + "\\Config.ini";
            string advanceDays = Util.INIOperationClass.INIGetStringValue(configFile, "UserSet", "AdvanceDays", null);
            int idvanceDays = 1; // Int32.Parse(advanceDays);
            try
            {
                idvanceDays = Int32.Parse(advanceDays);
            }
            catch
            {
                idvanceDays = 1;
            }
            dtp.Value = DateTime.Now.AddDays(-idvanceDays);
        }
        #endregion

        #region StartDateTime 设置开始日期
        /// <summary>
        /// StartDateTime 设置开始日期
        /// </summary>
        /// <param name="dgv"></param>
        public static void StartDateTime(System.Windows.Forms.DateTimePicker dtp, string advanceDays)
        {
            int idvanceDays = 1; // Int32.Parse(advanceDays);
            try
            {
                idvanceDays = Int32.Parse(advanceDays);
            }
            catch
            {
                idvanceDays = 1;
            }
            dtp.Value = DateTime.Now.AddDays(-idvanceDays);
        }
        #endregion


        #region  CopyFromAfterStr  取到某个特定字符后面的字符
        /// <summary>
        /// CopyFromStr 取到某个特定字符后面的字符 
        /// </summary>
        /// <param name="str_source"></param>
        /// <param name="str_key"></param>
        /// <param name="bl_contain_key">False不包含#，True包含</param>
        /// <returns></returns>
        public static string CopyFromAfterStr(string str_source, string str_key, bool bl_contain_key)
        {
            int i_startPosition = str_source.IndexOf(str_key);
            if (i_startPosition >= 0)
            {
                if (bl_contain_key)
                {
                    return str_source.Substring(i_startPosition, str_source.Length - i_startPosition);
                }
                else
                {
                    return str_source.Substring(i_startPosition + str_key.Length, str_source.Length - i_startPosition - str_key.Length);
                }
            }
            return str_source;
        }
        #endregion

        #region  CopyFromBeforeStr  取到某个特定字符前面的字符
        /// <summary>
        /// CopyFromStr 取到某个特定字符后面的字符 
        /// </summary>
        /// <param name="str_source"></param>
        /// <param name="str_key"></param>
        /// <param name="bl_contain_key">False不包含#，True包含</param>
        /// <returns></returns>
        public static string CopyFromBeforeStr(string str_source, string str_key, bool bl_contain_key)
        {
            int i_startPosition = str_source.IndexOf(str_key);
            if (i_startPosition >= 0)
            {
                if (bl_contain_key)
                {
                    return str_source.Substring(0, i_startPosition + str_key.Length);
                }
                else
                {
                    return str_source.Substring(0, i_startPosition);
                }
            }
            return str_source;
        }
        #endregion


    }
}

