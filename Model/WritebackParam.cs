using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class WritebackParam
    {
        public string TargetDatabase { get; set; }
        public string ProcedureName { get; set; }
        public string WritebackType { get; set; }
        public string Type { get; set; }
        public string BillCodes { get; set; }
        public string WritebackInfo { get; set; }
        public bool IsDebug { get; set; }
        public int Status { get; set; }
        public Model.JobEntity jobInfo { get; set; }

    }
}
