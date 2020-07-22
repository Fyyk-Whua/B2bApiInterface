using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DbSql
    {
        public string Success { get; set; }
        public string Sql { get; set; }
        public List<DbSqlParams> ParamsItems { get; set; }
        public string ExceptionMessage { get; set; }
    }

    public class DbSqlParams
    {
        public string ParameterName { get; set; }
        public object Value { get; set; }
    }

}
