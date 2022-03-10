using Aio.Db.Client.Entrance;
using Oracle.ManagedDataAccess.Client;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace PriOrder.App.Services
{
    public class MessageService
    {

        public static List<WO_PUSH_MSG> getSms(string distId, string groupId = "", string zoneId = "", string baseId = "")
        {

            OracleParameter inp_v = new OracleParameter(parameterName: "VDIST", type: OracleDbType.Varchar2, obj: "837075", direction: ParameterDirection.Input);
            OracleParameter inp_g = new OracleParameter(parameterName: "VGROUP", type: OracleDbType.Varchar2, obj: "", direction: ParameterDirection.Input);
            OracleParameter inp_z = new OracleParameter(parameterName: "VZONE", type: OracleDbType.Varchar2, obj: "", direction: ParameterDirection.Input);
            OracleParameter inp_b = new OracleParameter(parameterName: "VBASE", type: OracleDbType.Varchar2, obj: "", direction: ParameterDirection.Input);
            object[] inParams = new object[] { inp_v, inp_g, inp_z, inp_b };

            OracleParameter out_cur = new OracleParameter("OUTCURSPARM", OracleDbType.RefCursor, ParameterDirection.Output);
            object[] outParams = new object[] { out_cur };

            string sql = @"BEGIN RPGL.PRO_GET_PUSH_MSG(:VDIST,:VGROUP,:VZONE,:VBASE,:OUTCURSPARM); END;";

            var smsData = DatabaseOracleClient.GetDataSetSP(sql, inParams, outParams);

            return DatabaseOracleClient.DataTableToListObjectBind<WO_PUSH_MSG>(smsData.Item1.Tables[0]);

        }
        public static List<WO_SUP_MSG> getSupport(string distId, bool IsOpen = true)
        {
            List<WO_SUP_MSG> objList = new List<WO_SUP_MSG>();
            objList.Add(new WO_SUP_MSG { SUP_NUMBER = "abc", SUP_TYPE = "Common", DIST_ID = distId, IS_ACTIVE = false, CREATE_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now, CLOSED_BY = "Mr.X", CLOSED_NOTE = "Solved" });
            objList.Add(new WO_SUP_MSG { SUP_NUMBER = "zsd", SUP_TYPE = "Common", DIST_ID = distId, IS_ACTIVE = true, CREATE_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now, CLOSED_BY = "Mr.X", CLOSED_NOTE = "Solved" });
            objList.Add(new WO_SUP_MSG { SUP_NUMBER = "35s", SUP_TYPE = "Common", DIST_ID = distId, IS_ACTIVE = false, CREATE_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now, CLOSED_BY = "Mr.X", CLOSED_NOTE = "Solved" });
            objList.Add(new WO_SUP_MSG { SUP_NUMBER = "sdf", SUP_TYPE = "Common", DIST_ID = distId, IS_ACTIVE = true, CREATE_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now, CLOSED_BY = "Mr.X", CLOSED_NOTE = "Solved" });
            return objList;
        }

        public static List<WO_SUP_MSG_BODY> getSupportBody(string supId)
        {
            List<WO_SUP_MSG_BODY> objList = new List<WO_SUP_MSG_BODY>();
            objList.Add(new WO_SUP_MSG_BODY { SUP_NUMBER = supId, BODY_TEXT = "Hello", CREATE_DATE = DateTime.Now, SUP_BY = "0" });
            objList.Add(new WO_SUP_MSG_BODY { SUP_NUMBER = supId, BODY_TEXT = "How can I help you", CREATE_DATE = DateTime.Now, SUP_BY = "Mr Y" });
            objList.Add(new WO_SUP_MSG_BODY { SUP_NUMBER = supId, BODY_TEXT = "Need help about products", CREATE_DATE = DateTime.Now, SUP_BY = "0" });
            return objList;
        }
    }
}