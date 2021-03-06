﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAL
{
    public interface ICommonDAL
    {
        System.Data.DataTable GetDataGridViewHeader(string DgvName);

        bool ConnectTestInfo(string type);

        System.Data.DataTable GetDgvDtAll(Log4netUtil.LogAppendToForms logAppendToForms, Model.SearchParam searchParam);

        bool ErpWriteback(Log4netUtil.LogAppendToForms logAppendToForms, Model.WritebackParam writebackParam);

        int BulkCopyInsert(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, System.Data.DataTable dt, string tableName);

        int BulkCopyInsert(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, string tableName, string strSql);

        string ExecuteScalar(Log4netUtil.LogAppendToForms logAppendToForms, Model.SearchParam searchParam);

        bool ErpExecuteProcedure(Log4netUtil.LogAppendToForms logAppendToForms, Model.WritebackParam writebackParam);
    }
}
