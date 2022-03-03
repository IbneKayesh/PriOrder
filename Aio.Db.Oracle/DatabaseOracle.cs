using Aio.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Aio.Db.Oracle
{
    public static class DatabaseOracle
    {
        public static string ExecuteNonQueryList(string _conStr, List<string> _command_list)
        {
            string result = AppKeys.PostSuccess;
            int rows = 0;
            OracleConnection con = new OracleConnection(_conStr);
            OracleCommand cmd = new OracleCommand();
            OracleTransaction trn;
            con.Open();
            cmd.Connection = con;
            cmd.CommandTimeout = int.MaxValue;
            trn = con.BeginTransaction();
            cmd.Transaction = trn;
            try
            {
                foreach (string _command in _command_list)
                {
                    cmd.CommandText = _command;
                    rows = rows + cmd.ExecuteNonQuery();
                }
                trn.Commit();
            }
            catch (OracleException ex)
            {
                trn.Rollback();
                result = ex.Message;
                rows = 0;
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string ExecuteNonQuery(string _conStr, string _command)
        {
            string result = AppKeys.PostSuccess;
            int rows = 0;
            OracleConnection con = new OracleConnection(_conStr);
            OracleCommand cmd = new OracleCommand();
            OracleTransaction trn;
            con.Open();
            cmd.Connection = con;
            cmd.CommandTimeout = int.MaxValue;
            trn = con.BeginTransaction();
            cmd.Transaction = trn;
            try
            {
                cmd.CommandText = _command;
                rows = cmd.ExecuteNonQuery();
                trn.Commit();
            }
            catch (OracleException ex)
            {
                trn.Rollback();
                result = ex.Message;
                rows = 0;
            }
            finally
            {
                con.Close();
            }
            return $"{result},{rows}";
        }
        public static string ExecuteNonQueryListSP(string _conStr, List<string> _command_list)
        {
            string result = AppKeys.PostSuccess;
            int rows = 0;
            OracleConnection con = new OracleConnection(_conStr);
            OracleCommand cmd = new OracleCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandTimeout = int.MaxValue;

            try
            {
                string _query = "BEGIN ";
                foreach (string command in _command_list)
                {
                    _query += command + ";";
                }
                _query += "COMMIT;END;";
                cmd.CommandText = _query;
                rows = cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                result = ex.Message;
                rows = 0;
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        /// <summary>
        /// _outId >>> id_param
        /// </summary>
        /// <param name="_conStr"></param>
        /// <param name="_command"></param>
        /// <param name="_outId"></param>
        /// <returns></returns>
        public static Tuple<string, string> ExecuteNonQueryOut(string _conStr, string _command, string _outId)
        {
            string result = AppKeys.PostSuccess;
            string output = string.Empty;
            OracleConnection con = new OracleConnection(_conStr);
            OracleCommand cmd = new OracleCommand();
            OracleTransaction trn;
            con.Open();
            cmd.Connection = con;
            trn = con.BeginTransaction();
            cmd.Transaction = trn;
            try
            {
                cmd.CommandText = _command;
                OracleParameter outputParameter = new OracleParameter(_outId, OracleDbType.Int64);
                outputParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParameter);
                cmd.ExecuteNonQuery();
                output = outputParameter.Value.ToString();
                trn.Commit();
            }
            catch (OracleException ex)
            {
                trn.Rollback();
                result = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return new Tuple<string, string>(result, output);
        }
        public static DataTable ExecuteQuery(string _conStr, string _command, object[] _parameters = null)
        {
            DataSet ds = new DataSet();
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter da = new OracleDataAdapter();
            OracleConnection con = new OracleConnection(_conStr);
            try
            {
                cmd = new OracleCommand(_command, con);
                if (_parameters != null)
                {
                    cmd.Parameters.AddRange(_parameters);
                }
                cmd.CommandTimeout = int.MaxValue;
                cmd.CommandType = CommandType.Text;
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (OracleException ex)
            {
                var dt = new DataTable();
                dt.Columns.Add(AppKeys.GetError, typeof(string));
                dt.Rows.Add(ex.Message);
                return dt;
            }
            finally
            {
                con.Close();
            }
            return ds.Tables[0];
        }
    }
}

