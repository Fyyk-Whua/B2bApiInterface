using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public class DbSqlLog
    {
        public static string SqlToJson(string success,string strSql, System.Data.Common.DbParameter[] cmdParams)
        {
            return Util.NewtonsoftCommon.SerializeObjToJson(GetDbSqlLog(success,strSql, "", cmdParams));
        }

        public static string SqlToJson(string success, string strSql, string ExceptionMessage, System.Data.Common.DbParameter[] cmdParams)
        {
            return Util.NewtonsoftCommon.SerializeObjToJson(GetDbSqlLog(success,strSql, ExceptionMessage, cmdParams));
        }

        public static Model.DbSql GetDbSqlLog(string success, string strSql, string ExceptionMessage, System.Data.Common.DbParameter[] cmdParams)
        {
            Model.DbSql dbSql = new Model.DbSql();
            dbSql.Success = success;
            dbSql.Sql = strSql;
            dbSql.ExceptionMessage = ExceptionMessage;
            try
            {
                List<Model.DbSqlParams> ParamsItems = new List<Model.DbSqlParams>();
                if (cmdParams == null)
                {
                    Model.DbSqlParams dbSqlParams = new Model.DbSqlParams();
                    dbSqlParams.ParameterName = string.Empty;
                    dbSqlParams.Value = string.Empty;
                    ParamsItems.Add(dbSqlParams);
                }
                else
                {
                    for (int i = 0; i < cmdParams.Count(); i++)
                    {
                        Model.DbSqlParams dbSqlParams = new Model.DbSqlParams();
                        dbSqlParams.ParameterName = cmdParams[i].ParameterName;
                        dbSqlParams.Value = cmdParams[i].Value;
                        ParamsItems.Add(dbSqlParams);
                    }
                    if (ParamsItems.Count < 1)
                    {
                        Model.DbSqlParams dbSqlParams = new Model.DbSqlParams();
                        dbSqlParams.ParameterName = string.Empty;
                        dbSqlParams.Value = string.Empty;
                        ParamsItems.Add(dbSqlParams);
                    }
                }
                
                dbSql.ParamsItems = ParamsItems;
                return dbSql;
            }
            catch (Exception ex)
            {
                dbSql.Success = "9999";
                dbSql.ExceptionMessage = ex.Message;
                Model.DbSqlParams dbSqlParams = new Model.DbSqlParams();
                dbSqlParams.ParameterName = string.Empty;
                dbSqlParams.Value = string.Empty;
                List<Model.DbSqlParams> ParamsItems = new List<Model.DbSqlParams>();
                ParamsItems.Add(dbSqlParams);
                dbSql.ParamsItems = ParamsItems;
                return dbSql;
            }

        }
    }
}
