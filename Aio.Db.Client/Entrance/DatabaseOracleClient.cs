using Aio.Db.Client.DataModels;
using Aio.Db.ConnectionString;
using Aio.Db.Oracle;
using Aio.Db.OracleEF;
using Aio.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Aio.Db.Client.Entrance
{
    public static class DatabaseOracleClient
    {
        #region Oracle_DB_Method
        public static Tuple<DataTable, EQResult> GetDataTable(string sql, object[] _parameters = null, string _con = "")
        {
            return DatabaseOracle.ExecuteQuery(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command: sql, _parameters: _parameters);
        }
        public static EQResult PostSqlList(List<string> sqlList, string _con = "")
        {
            return DatabaseOracle.ExecuteNonQueryList(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command_list: sqlList);
        }
        public static EQResult PostSql(string sql, string _con = "")
        {
            return DatabaseOracle.ExecuteNonQuery(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command: sql);
        }
        // public static Tuple<string, string> PostSqlOut(string sql, string _out, string _con = "")
        public static EQResult PostSqlOut(string sql, string _out, string _con = "")
        {
            return DatabaseOracle.ExecuteNonQueryOut(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con), _command: sql, _outId: _out);
        }
        #endregion

        #region OracleEF

        private static T SqlToModel<T>(string sql) where T : new()
        {
            T t = new T();
            Tuple<DataTable, EQResult> _tpl = GetDataTable(sql: sql);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                t = TableEntity.DataTableToListModel<T>(_tpl.Item1).ToList().FirstOrDefault();
            }
            return t;
        }

        public static Tuple<List<T>, EQResult> SqlToListObjectBind<T>(string sql, object[] _parameters = null) where T : new()
        {
            List<T> lst = new List<T>();
            Tuple<DataTable, EQResult> _tpl = GetDataTable(sql: sql, _parameters: _parameters);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                lst = TableEntity.BindObjectList<T>(_tpl.Item1).ToList();
            }
            return new Tuple<List<T>, EQResult>(lst, _tpl.Item2);
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
            Tuple<DataTable, EQResult> _tpl = GetDataTable(sql: sql);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                lst = TableEntity.DataTableToListModel<T>(_tpl.Item1).ToList();
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



        public static Tuple<DataSet, EQResult> TestSP()
        {
            OracleParameter[] inParams = new OracleParameter[] {
                new OracleParameter("VDIST",OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("VGROUP",OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("VZONE",OracleDbType.Varchar2, ParameterDirection.Input),
                new OracleParameter("VBASE",OracleDbType.Varchar2, ParameterDirection.Input),
            };

            OracleParameter[] outParams = new OracleParameter[] {
               new OracleParameter("OUTCURSPARM", OracleDbType.RefCursor, ParameterDirection.Output)
           };

            string sql = @"BEGIN RPGL.PRO_GET_PUSH_MSG(:VDIST,:VGROUP,:VZONE,:VBASE,:OUTCURSPARM); END;";


            return ExecuteSPQuery(sql, inParams, outParams);
        }

        public static Tuple<DataSet, EQResult> ExecuteSPQuery(string sql, object[] _parameters, object[] _out_parameters, string _con = "")
        {
            EQResult result = new EQResult();
            result.SUCCESS = true;
            result.MESSAGES = AppKeys.SUCCESS_MESSAGES;

            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter da = new OracleDataAdapter();
            OracleConnection con = new OracleConnection(DbLink.GET(_con == string.Empty ? AppsData.ORACLE_CON_STR : _con));
            try
            {

                cmd = new OracleCommand(sql, con);
                cmd.Parameters.AddRange(_parameters);
                cmd.Parameters.AddRange(_out_parameters);
                cmd.CommandTimeout = int.MaxValue;
                cmd.CommandType = CommandType.Text;
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (OracleException ex)
            {
                result.SUCCESS = false;
                result.MESSAGES = ex.Message;
                result.ROWS = 0;
                return new Tuple<DataSet, EQResult>(new DataSet(), result);
            }
            finally
            {
                con.Close();
            }
            result.ROWS = ds.Tables.Count;
            return new Tuple<DataSet, EQResult>(ds, result);
        }
    }
}
