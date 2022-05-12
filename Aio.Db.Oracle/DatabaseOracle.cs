using Aio.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Aio.Db.Oracle
{
    public static class DatabaseOracle
    {
        public static EQResult ExecuteNonQueryList(string _conStr, List<string> _command_list)
        {
            EQResult result = new EQResult();
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
                int r = 0;
                foreach (string _command in _command_list)
                {
                    cmd.CommandText = _command;
                    r += cmd.ExecuteNonQuery();
                }
                trn.Commit();

                result.SUCCESS = true;
                result.MESSAGES = AppKeys.SUCCESS_MESSAGES;
                result.ROWS = r;
            }
            catch (OracleException ex)
            {
                trn.Rollback();
                result.SUCCESS = false;
                result.MESSAGES = ex.Message;
                result.ROWS = 0;
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public static EQResult ExecuteNonQuery(string _conStr, string _command)
        {
            EQResult result = new EQResult();
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
                result.ROWS = cmd.ExecuteNonQuery();
                trn.Commit();

                result.SUCCESS = true;
                result.MESSAGES = AppKeys.SUCCESS_MESSAGES;
            }
            catch (OracleException ex)
            {
                trn.Rollback();
                result.SUCCESS = false;
                result.MESSAGES = ex.Message;
                result.ROWS = 0;
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public static EQResult ExecuteNonQueryListSP(string _conStr, List<string> _command_list)
        {
            EQResult result = new EQResult();
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
                result.ROWS = cmd.ExecuteNonQuery();

                result.SUCCESS = true;
                result.MESSAGES = AppKeys.SUCCESS_MESSAGES;
            }
            catch (OracleException ex)
            {
                result.SUCCESS = false;
                result.MESSAGES = ex.Message;
                result.ROWS = 0;
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
       // public static Tuple<string, string> ExecuteNonQueryOut(string _conStr, string _command, string _outId)
        public static EQResult ExecuteNonQueryOut(string _conStr, string _command, string _outId)
        {
            EQResult result = new EQResult();
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
                result.ROWS = cmd.ExecuteNonQuery();
                result.MESSAGES = outputParameter.Value.ToString();
                trn.Commit();

                result.SUCCESS = true;
            }
            catch (OracleException ex)
            {
                trn.Rollback();
                result.SUCCESS = false;
                result.MESSAGES = ex.Message;
                result.ROWS = 0;
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public static EQResultTable ExecuteQuery(string _conStr, string _command, object[] _parameters = null)
        {
            EQResult result = new EQResult();
            result.SUCCESS = true;
            result.MESSAGES = AppKeys.SUCCESS_MESSAGES;

            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
            OracleConnection con = new OracleConnection(_conStr);
            try
            {
                OracleCommand cmd = new OracleCommand(_command, con);
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
                result.SUCCESS = false;
                result.MESSAGES = ex.Message;
                result.ROWS = 0;
                return new EQResultTable() { Table = new DataTable(), Result = result };
            }
            finally
            {
                con.Close();
            }
            result.ROWS = ds.Tables[0].Rows.Count;
            return new EQResultTable() { Table = ds.Tables[0], Result = result };
        }



        public static EQResultTable ExecuteSPQuerySingle(string _conStr, string _command, object[] _in_parameters, object[] _out_parameters)
        {
            EQResult result = new EQResult();
            result.SUCCESS = true;
            result.MESSAGES = AppKeys.SUCCESS_MESSAGES;

            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
            OracleConnection con = new OracleConnection(_conStr);
            try
            {
                OracleCommand cmd = new OracleCommand(_command, con);
                if (_in_parameters != null)
                {
                    cmd.Parameters.AddRange(_in_parameters);
                }
                if (_out_parameters != null)
                {
                    cmd.Parameters.AddRange(_out_parameters);
                }
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
                return new EQResultTable() { Table = new DataTable(), Result = result };
            }
            finally
            {
                con.Close();
            }
            result.ROWS = ds.Tables.Count;
            return new EQResultTable() { Table = ds.Tables[0], Result = result };
        }
        public static EQResultSet ExecuteSPQuery(string _conStr, string _command, object[] _in_parameters, object[] _out_parameters)
        {
            EQResult result = new EQResult();
            result.SUCCESS = true;
            result.MESSAGES = AppKeys.SUCCESS_MESSAGES;

            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
            OracleConnection con = new OracleConnection(_conStr);
            try
            {
                OracleCommand cmd = new OracleCommand(_command, con);
                if (_in_parameters != null)
                {
                    cmd.Parameters.AddRange(_in_parameters);
                }
                if (_out_parameters != null)
                {
                    cmd.Parameters.AddRange(_out_parameters);
                }
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
                return new EQResultSet() { Set = new DataSet(), Result = result };
            }
            finally
            {
                con.Close();
            }
            result.ROWS = ds.Tables.Count;
            return new EQResultSet() { Set = ds, Result = result };
        }
        public static EQResult ExecuteSPNonQuery(string _conStr, string _command, object[] _in_parameters)
        {
            EQResult result = new EQResult();
            result.SUCCESS = true;
            result.MESSAGES = AppKeys.SUCCESS_MESSAGES;

            OracleConnection con = new OracleConnection(_conStr);
            OracleCommand cmd = new OracleCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandTimeout = int.MaxValue;
            try
            {
                if (_in_parameters != null)
                {
                    cmd.Parameters.AddRange(_in_parameters);
                }
                cmd.CommandText = _command;
                result.ROWS = cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                result.SUCCESS = false;
                result.MESSAGES = ex.Message;
                result.ROWS = 0;
            }
            finally
            {
                con.Close();
            }
            return result;
        }
    }
}
