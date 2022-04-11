using System;
using System.Data.OracleClient;
using System.Data;
using Aio.Db.ConnectionString;
using Aio.Db.Client.DataModels;

namespace Aio.Db.Client.Entrance
{
    public static class DatabaseOracleClientLegacy
    {
        public static DataTable WebAutoCommon(string vmenu, string vuser, string vpara1, string vpara2, string vpara3, string vpara4)
        {
            DataTable dt = new DataTable();

            string[] x1 = { "VMENU", vmenu };
            string[] x2 = { "VUSER", vuser };
            string[] x3 = { "VPARA1", vpara1 };
            string[] x4 = { "VPARA2", vpara2 };
            string[] x5 = { "VPARA3", vpara3 };
            string[] x6 = { "VPARA4", vpara4 };
            string[][] aParaNameValue = { x1, x2, x3, x4, x5, x6 };
            dt = DataSetPro("RFL.PRO_LEGACY_DO_WA", aParaNameValue, "dt").Tables[0];
            return dt;
        }
        public static DataSet DataSetPro(string ProName, string[][] aParaNameValue, string tblName)
        {
            OracleConnection _cn = new OracleConnection();
            try
            {
                using (_cn = new OracleConnection(DbLink.GET(AppsData.ORACLE_CON_STR)))
                {
                    _cn.Open();
                    OracleCommand _cmd = new OracleCommand(ProName, _cn);
                    _cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter oraPara;
                    for (int j = 0; j <= aParaNameValue.GetUpperBound(0); j++)
                    {
                        oraPara = new OracleParameter(aParaNameValue[j][0].ToString(), OracleType.VarChar);
                        oraPara.Direction = ParameterDirection.Input;
                        oraPara.Value = aParaNameValue[j][1].ToString();
                        _cmd.Parameters.Add(oraPara);
                    }

                    oraPara = new OracleParameter("P_CURSOR", OracleType.Cursor, 1);
                    oraPara.Direction = ParameterDirection.Output;
                    _cmd.Parameters.Add(oraPara);
                    OracleDataAdapter odAdapter = new OracleDataAdapter();
                    odAdapter = new OracleDataAdapter(_cmd);
                    DataSet dSet = new DataSet("dSet");
                    dSet = new DataSet();
                    odAdapter.Fill(dSet, tblName);
                    return dSet;
                }

            }
            catch (Exception Ex)
            {
                _cn.Close();
                // ErrorSave(sql, Ex.Message.ToString());
                //Ex = Ex.Message.ToString();
                return null;
            }
            finally
            {
                _cn.Dispose();
                _cn.Close();
                OracleConnection.ClearPool(_cn);
            }
        }
    }
}
