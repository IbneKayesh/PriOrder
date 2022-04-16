using Aio.Db.Client.Entrance;
using Aio.Model;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace PriOrder.App.Services
{
    public class SpService
    {
        public static EQResultSet PRO_WO_GET_ALL(string _menu, string _dist, string _item = "", string _a = "", string _b = "", string _c = "", string _d = "")
        {
            OracleParameter inp_menu = new OracleParameter(parameterName: "VMENU", type: OracleDbType.Varchar2, obj: _menu, direction: ParameterDirection.Input);
            OracleParameter inp_dist = new OracleParameter(parameterName: "VDISTID", type: OracleDbType.Varchar2, obj: _dist, direction: ParameterDirection.Input);
            OracleParameter inp_item = new OracleParameter(parameterName: "VITEMID", type: OracleDbType.Varchar2, obj: _item, direction: ParameterDirection.Input);
            OracleParameter inp_a = new OracleParameter(parameterName: "VA", type: OracleDbType.Varchar2, obj: _a, direction: ParameterDirection.Input);
            OracleParameter inp_b = new OracleParameter(parameterName: "VB", type: OracleDbType.Varchar2, obj: _b, direction: ParameterDirection.Input);
            OracleParameter inp_c = new OracleParameter(parameterName: "VC", type: OracleDbType.Varchar2, obj: _c, direction: ParameterDirection.Input);
            OracleParameter inp_d = new OracleParameter(parameterName: "VD", type: OracleDbType.Varchar2, obj: _d, direction: ParameterDirection.Input);
            object[] inParams = new object[] { inp_menu, inp_dist, inp_item, inp_a, inp_b, inp_c, inp_d };

            OracleParameter out_cur = new OracleParameter("OUTCURSPARM", OracleDbType.RefCursor, ParameterDirection.Output);
            object[] outParams = new object[] { out_cur };

            string sql = @"BEGIN RPGL.PRO_WO_GET_ALL(:VMENU,:VDISTID,:VITEMID,:VA,:VB,:VC,:VD,:OUTCURSPARM); END;";
            return DatabaseOracleClient.GetDataSetSP(sql, inParams, outParams);
        }
    }
}