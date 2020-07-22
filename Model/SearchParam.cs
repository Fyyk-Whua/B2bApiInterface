using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SearchParam
    {
        public string ProcedureName { get; set; }
        public string ModuleID { get; set; }
        public int IsMaintain { get; set; }
        public int FilterFlag { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Logogram { get; set; }
        public string BillCode { get; set; }
        public string TargetDatabase { get; set; }
        public bool IsDebug { get; set; }
        public Model.JobEntity jobInfo { get; set; }
    }
}
