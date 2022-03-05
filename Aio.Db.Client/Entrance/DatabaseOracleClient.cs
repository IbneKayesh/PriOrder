using Aio.Db.Client.DataModels;
using Aio.Db.ConnectionString;
using Aio.Db.Oracle;
using Aio.Db.OracleEF;
using Aio.Model;
using Aio.Utility.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Aio.Db.Client.Entrance
{
    public static class DatabaseOracleClient
    {
        #region Oracle_DB_Method
        public static DataTable GetDataTable(string sql, object[] _parameters = null, string _con = "")
        {
            return DatabaseOracle.ExecuteQuery(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command: sql, _parameters: _parameters);
        }
        public static ENQResult PostSqlList(List<string> sqlList, string _con = "")
        {
            return DatabaseOracle.ExecuteNonQueryList(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command_list: sqlList);
        }
        public static ENQResult PostSql(string sql, string _con = "")
        {
            return DatabaseOracle.ExecuteNonQuery(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command: sql);
        }
        // public static Tuple<string, string> PostSqlOut(string sql, string _out, string _con = "")
        public static ENQResult PostSqlOut(string sql, string _out, string _con = "")
        {
            return DatabaseOracle.ExecuteNonQueryOut(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command: sql, _outId: _out);
        }
        #endregion

        #region OracleEF

        private static T SqlToModel<T>(string sql) where T : new()
        {
            T t = new T();
            DataTable _dt = GetDataTable(sql: sql);
            if (_dt.Rows.Count == 0)
            {
                t = TableEntity.DataTableToListModel<T>(_dt).ToList().FirstOrDefault();
            }
            return t;
        }

        public static Tuple<List<T>, string> SqlToListObjectBind<T>(string sql, object[] _parameters = null) where T : new()
        {
            List<T> lst = new List<T>();
            DataTable _dt = GetDataTable(sql: sql, _parameters: _parameters);
            Tuple<DataTable, string> _tpl = Table.Filter(_dt);
            if (_tpl.Item2 == AppKeys.PostSuccess)
            {
                lst = TableEntity.BindObjectList<T>(_dt).ToList();
            }
            return new Tuple<List<T>, string>(lst, _tpl.Item2);
        }
        /// <summary>
        /// Getting error in number system
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> SqlToListModel<T>(string sql) where T : new()
        {
            List<T> lst = new List<T>();
            DataTable _dt = GetDataTable(sql: sql);
            if (_dt.Rows.Count > 0)
            {
                lst = TableEntity.DataTableToListModel<T>(_dt).ToList();
            }
            return lst;
        }

        //public static List<T> SqlToListObjectBind<T>(string sql) where T : new()
        //{
        //    List<T> lst = new List<T>();
        //    DataTable _dt = GetDataTable(sql: sql);
        //    if (_dt.Rows.Count > 0)
        //    {
        //        lst = TableEntity.BindObjectList<T>(_dt).ToList();
        //    }
        //    return lst;
        //}
        public static List<string> ModelListToInsertQuery<T>(List<T> objList)
        {
            List<string> lst = new List<string>();
            try
            {
                foreach (T _obj in objList)
                {
                    lst.Add(TableEntity.ModelToInsertQuery(_obj));
                }
            }
            catch
            {
                lst = new List<string>();
            }
            return lst;
        }
        public static string ModelToInsertQuery<T>(T obj)
        {
            try
            {
                return TableEntity.ModelToInsertQuery(obj);
            }
            catch
            {
                return AppKeys.GetError;
            }
        }
        public static string ModelToInsertQuery<T>(T obj, string columns)
        {
            try
            {
                return TableEntity.ModelToInsertQuery(obj, columns);
            }
            catch
            {
                return AppKeys.GetError;
            }
        }
        public static string ModelToUpdateQuery<T>(T obj, string columns, string condition)
        {
            try
            {
                return TableEntity.ModelToUpdateQuery(obj, columns, condition);
            }
            catch
            {
                return AppKeys.GetError;
            }
        }
        #endregion
    }
}
