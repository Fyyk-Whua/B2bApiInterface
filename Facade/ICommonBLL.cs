using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Facade
{
    public interface ICommonBLL
    {
        string GetIniValue(string section, string key);

        bool INIWriteValue(string oldValue, string newValue, string section, string key);

        bool INIWriteValue(string oldValue, string newValue, string section, string key, bool isEmpty);

        bool INIWritePwdValue(string oldValue, string newValue, string section, string key);

        void SetDataGridViewHeaderText(System.Windows.Forms.DataGridView dgv, System.Data.DataTable HeaderDt);

        void GetDataGridViewHeader(System.Windows.Forms.DataGridView dgv, string DgvName);

        System.Data.DataTable GetDataTableViewHeader(string DgvName);

        void SetDataGridViewAttributes(System.Windows.Forms.DataGridView dgv, bool ReadOnly, bool unHeadSequence);

        void SetDataGridViewSelectAll(System.Windows.Forms.DataGridView dgv, System.Drawing.Color color);

        void SetDataGridViewUnSelectAll(System.Windows.Forms.DataGridView dgv, System.Drawing.Color color);

        void GetDgvDtAll(Log4netUtil.LogAppendToForms logAppendToForms, System.Windows.Forms.DataGridView dgv, Model.SearchParam searchParam);

        System.Data.DataTable GetDataTableAll(Log4netUtil.LogAppendToForms logAppendToForms, Model.SearchParam searchParam);

        bool ErpWriteback(Log4netUtil.LogAppendToForms logAppendToForms, Model.WritebackParam writebackParam);

        int BulkInsertDatabase(Log4netUtil.LogAppendToForms logAppendToForms,
                                       Model.JobEntity jobInfo,
                                       System.Data.DataTable dt, string insertTableName);

        int BulkInsertDatabase(Log4netUtil.LogAppendToForms logAppendToForms,
                                       Model.JobEntity jobInfo,
                                       string insertTableName, string strSql);

        string ExecuteScalar(Log4netUtil.LogAppendToForms logAppendToForms, Model.SearchParam searchParam);

        string FtpDownloadToFile(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo,
                               string ftpfilepath, string fileSavePath, string fileSaveName);



    }
}
