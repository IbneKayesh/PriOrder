using Aio.Db.Client.DataModels;
using Aio.Db.ConnectionString;
using Aio.Db.Oracle;
using System;
using System.Collections.Generic;
using System.Data;

namespace Aio.Db.Client.Entrance
{
    public static class DatabaseOracleClient
    {
        #region Oracle_DB_Method
        public static DataTable GetDataTable(string sql, object[] _parameters = null, string _con = "")
        {
            return DatabaseOracle.ExecuteQuery(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command: sql, _parameters: _parameters);
        }
        public static string PostSqlList(List<string> sqlList, string _con = "")
        {
            return DatabaseOracle.ExecuteNonQueryList(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command_list: sqlList);
        }
        public static string PostSql(string sql, string _con = "")
        {
            return DatabaseOracle.ExecuteNonQuery(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command: sql);
        }
        public static Tuple<string, string> PostSqlOut(string sql, string _out, string _con = "")
        {
            return DatabaseOracle.ExecuteNonQueryOut(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command: sql, _outId: _out);
        }
        #endregion
    }
}
