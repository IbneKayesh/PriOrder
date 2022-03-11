using Aio.Db.Client.Entrance;
using Aio.Model;
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
            OracleParameter inp_v = new OracleParameter(parameterName: "VDIST", type: OracleDbType.Varchar2, obj: distId, direction: ParameterDirection.Input);
            OracleParameter inp_g = new OracleParameter(parameterName: "VGROUP", type: OracleDbType.Varchar2, obj: groupId, direction: ParameterDirection.Input);
            OracleParameter inp_z = new OracleParameter(parameterName: "VZONE", type: OracleDbType.Varchar2, obj: zoneId, direction: ParameterDirection.Input);
            OracleParameter inp_b = new OracleParameter(parameterName: "VBASE", type: OracleDbType.Varchar2, obj: baseId, direction: ParameterDirection.Input);
            object[] inParams = new object[] { inp_v, inp_g, inp_z, inp_b };

            OracleParameter out_cur = new OracleParameter("OUTCURSPARM", OracleDbType.RefCursor, ParameterDirection.Output);
            object[] outParams = new object[] { out_cur };

            string sql = @"BEGIN RPGL.PRO_GET_PUSH_MSG(:VDIST,:VGROUP,:VZONE,:VBASE,:OUTCURSPARM); END;";

            var smsData = DatabaseOracleClient.GetDataSetSP(sql, inParams, outParams);

            return DatabaseOracleClient.DataTableToListObjectBind<WO_PUSH_MSG>(smsData.Item1.Tables[0]);

        }
        public static List<SelectListItem> getSupportCategory()
        {
            string sql_1 = @"SELECT T.CTYP_SLNO,T.CTYP_TYPE FROM T_CTYP T WHERE T.CTYP_ACTV='Y' ORDER BY T.CTYP_SLNO";
            Tuple<List<T_CTYP>, EQResult> _tpl = DatabaseOracleClient.SqlToListObjectBind<T_CTYP>(sql_1);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return _tpl.Item1.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.CTYP_TYPE,
                        Value = a.CTYP_SLNO.ToString(),
                        Selected = false,
                    };
                });
            }
            return new List<SelectListItem>();
        }

        public static string AddNewSupport(string typeId, string distId, string text)
        {
            string spId = distId + DateTime.Now.ToString("MMM").ToUpper() + DateTime.Now.ToString("yy") + "-" + new Random().Next(1, 1000);
            List<string> sqlList = new List<string>();
            sqlList.Add($@"INSERT INTO WO_SUP_MSG(SUP_NUMBER,CTYP_TYPE,DIST_ID,CREATE_USER)VALUES('{spId}','{typeId}', '{distId}','{distId}')");
            sqlList.Add($"INSERT INTO WO_SUP_MSG_BODY(SUP_NUMBER,BODY_TEXT,CREATE_USER)VALUES('{spId}','{text.Replace("'", "")}','0')");
            EQResult result = DatabaseOracleClient.PostSqlList(sqlList);
            if (result.SUCCESS && result.ROWS == 2)
            {
                return spId;
            }
            else
            {
                return "0";
            }
        }
        public static EQResult SupportReply(string spId, string text, string user = "0")
        {
            string sql = $"INSERT INTO WO_SUP_MSG_BODY(SUP_NUMBER,BODY_TEXT,CREATE_USER)VALUES('{spId}','{text?.Replace("'", "")}','{user}')";
            return DatabaseOracleClient.PostSql(sql);
        }


        public static Tuple<List<WO_SUP_MSG>, EQResult> getSupport(string distId)
        {
            string sql = $@"SELECT T.SUP_NUMBER,TC.CTYP_TYPE,T.DIST_ID,T.CLOSED_NOTE,T.IS_ACTIVE,T.CREATE_DATE,T.UPDATE_DATE,T.UPDATE_USER                FROM WO_SUP_MSG T JOIN T_CTYP TC ON TC.CTYP_SLNO=T.CTYP_TYPE WHERE T.DIST_ID='{distId}' ORDER BY T.IS_ACTIVE DESC";
            return DatabaseOracleClient.SqlToListObjectBind<WO_SUP_MSG>(sql);
        }


        public static Tuple<List<WO_SUP_MSG_BODY>, EQResult> getSupportBody(string supId)
        {
            string sql = $@"SELECT T.SUP_NUMBER,T.BODY_TEXT,T.CREATE_USER,T.CREATE_DATE,TM.IS_ACTIVE FROM WO_SUP_MSG_BODY T JOIN WO_SUP_MSG TM ON TM.SUP_NUMBER=T.SUP_NUMBER WHERE T.SUP_NUMBER='{supId}' ORDER BY T.CREATE_DATE";
            return DatabaseOracleClient.SqlToListObjectBind<WO_SUP_MSG_BODY>(sql);
        }

        //--------------------------------Back Office--------------------------------------------//

        public static Tuple<List<WO_SUP_MSG>, EQResult> getFeedback(string userId)
        {
            string sql = $@"SELECT T.SUP_NUMBER,TC.CTYP_TYPE,T.DIST_ID,T.CLOSED_NOTE,T.IS_ACTIVE,T.CREATE_DATE,T.UPDATE_DATE,T.UPDATE_USER                FROM WO_SUP_MSG T JOIN T_CTYP TC ON TC.CTYP_SLNO=T.CTYP_TYPE WHERE T.IS_ACTIVE=1";
            return DatabaseOracleClient.SqlToListObjectBind<WO_SUP_MSG>(sql);
        }
        public static EQResult FeedbackReply(SUP_MSG_REPL obj, string user)
        {
            List<string> sqlList = new List<string>();
            sqlList.Add($"INSERT INTO WO_SUP_MSG_BODY(SUP_NUMBER,BODY_TEXT,CREATE_USER)VALUES('{obj.replyId}','{obj.messagesText?.Replace("'", "")}','{user}')");
            if (obj.closereply == 1)
            {
                sqlList.Add($@"UPDATE WO_SUP_MSG SET CLOSED_NOTE='Done',IS_ACTIVE=0,UPDATE_DATE=SYSDATE,UPDATE_USER='{user}' WHERE SUP_NUMBER='{obj.replyId}'");
            }
            return DatabaseOracleClient.PostSqlList(sqlList);
        }


        public static EQResult AddNewSMS(string cat, string groupId, string zoneId, string baseId, string distId, string msgTxt)
        {
            string id = Guid.NewGuid().ToString();
            OracleParameter ip_vid = new OracleParameter(parameterName: "VID", type: OracleDbType.Varchar2, obj: id, direction: ParameterDirection.Input);
            OracleParameter ip_cid = new OracleParameter(parameterName: "VCATID", type: OracleDbType.Varchar2, obj: cat, direction: ParameterDirection.Input);
            OracleParameter ip_gid = new OracleParameter(parameterName: "VGROUPID", type: OracleDbType.Varchar2, obj: groupId, direction: ParameterDirection.Input);
            OracleParameter ip_zid = new OracleParameter(parameterName: "VZONEID", type: OracleDbType.Varchar2, obj: zoneId, direction: ParameterDirection.Input);
            OracleParameter ip_bid = new OracleParameter(parameterName: "VBASEID", type: OracleDbType.Varchar2, obj: baseId, direction: ParameterDirection.Input);
            OracleParameter ip_did = new OracleParameter(parameterName: "DISTID", type: OracleDbType.Int32, obj: distId, direction: ParameterDirection.Input);
            OracleParameter ip_txt = new OracleParameter(parameterName: "MSGTXT", type: OracleDbType.Varchar2, obj: msgTxt, direction: ParameterDirection.Input);

            object[] inParams = new object[] { ip_vid, ip_cid, ip_gid, ip_zid, ip_bid, ip_did, ip_txt };

            string sql = @"BEGIN RPGL.PRO_INS_PUSH_MSG(:VID,:VCATID,:VGROUPID,:VZONEID,:VBASEID,:DISTID,:MSGTXT); END;";

            return DatabaseOracleClient.PostSP(sql, inParams);
        }
    }
}