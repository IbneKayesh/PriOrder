using Aio.Db.Client.Entrance;
using Aio.Model;
using Oracle.ManagedDataAccess.Client;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Services
{
    public class AccountService
    {
        public static Tuple<List<NIF_DIST>, EQResult> getMyBusiness(string distId)
        {
            string sql = $@"SELECT T.APPL_NID, T1.DSTO_NIDN,T.DIST_ID,T1.DSMA_NAME,T2.DGIG_DNAM,TO_CHAR(T.OPDATE)OPDATE,TO_CHAR(T.CLDATE)CLDATE, CASE WHEN T.CANCELLED = 'N' THEN 'Active' ELSE 'Inactive' END STATUS, CASE WHEN T.IS_ACTIVE = 1 THEN 'Approved' ELSE 'Not Approved' END APPROVAL FROM NIF_DIST T
            INNER JOIN T_DSMA T1 ON T.DIST_ID = T1.DSMA_DSID
            LEFT OUTER JOIN T_DGIG T2 ON T1.DSMA_GRUP = T2.DGIG_DSGP
            WHERE T.APPL_NID IN (SELECT T.APPL_NID
            FROM  RPGL.NIF_DIST T WHERE T.DIST_ID='{distId}')
            ORDER BY T2.DGIG_DNAM";
            return DatabaseOracleClient.SqlToListObjectBind<NIF_DIST>(sql);
        }
        public static Tuple<List<T_BANKCOL>, EQResult> getBankList()
        {
            string sql = $@"SELECT BNAME,ANAME,ANUMBER,BRNAME FROM RPGL.T_BANKCOL WHERE BNAME<>'N' ORDER BY BNAME";
            return DatabaseOracleClient.SqlToListObjectBind<T_BANKCOL>(sql);
        }
        public static Tuple<List<MR_STATUS>, EQResult> getMRStatusByDistId(string distId)
        {
            string sql = $@"SELECT MRDO_TRN||'-'||SUBSTR(MRDO_MRNO,-12,9)MRDO_MRNO,MRDO_DIST,TO_CHAR(MRDO_DATE,'DD/MM/YYYY') MRDO_DATE,MRDO_SORC,MRDO_CHEK,MRDO_AMNT from RPGL.T_MRDO WHERE MRDO_DIST='{distId}' AND MRDO_DATE >= SYSDATE -30";
            return DatabaseOracleClient.SqlToListObjectBind<MR_STATUS>(sql);
        }
        public static Tuple<List<T_DSMA>, EQResult> getDistProfile(string distId)
        {
            string sql = $@"select nfd.appl_nid,tdist.dsto_nidn,tdtp.dist_type_id,tdtp.dist_type,rdist.domo_text,tdc.dist_class_name, tdist.dsma_dsid,tdist.dsma_name,rdist.addr1,rcnt.contacts,rdist.addr2,tg.digr_text,tg.digr_name,tsz.sales_zone_id,tsz.sales_zone,tdz.dist_zone_id,tdz.dist_zone,rzcnt.contacts zcontacts from rpgl.t_dsma tdist
                join rfl.distributor_master rdist on rdist.dist_id=tdist.dsma_dsid
                join rfl.t_digr tg on  tdist.dsma_grup=tg.digr_text
                join rfl.sales_zone tsz on  rdist.sales_zone_id=tsz.sales_zone_id
                join rfl.distributor_zone tdz on rdist.dist_zone_id=tdz.dist_zone_id
                join rfl.distributor_type tdtp on rdist.dist_type_id=tdtp.dist_type_id
                join rfl.distributor_class tdc on rdist.domo_text=tdc.dist_class_id
                left outer join rfl.distributor_contacts rcnt on tdist.dsma_dsid=rcnt.dist_id and rcnt.seqn=101
                left outer join rfl.distributor_contacts rzcnt on tdist.dsma_dsid=rzcnt.dist_id and rzcnt.seqn=103
                left outer join nif_dist nfd on tdist.dsma_dsid=nfd.dist_id
                where tdist.dsma_dsid='{distId}'";
            return DatabaseOracleClient.SqlToListObjectBind<T_DSMA>(sql);
        }


        public static EQResult changePassword(USER_PASSWORD obj, string distId)
        {
            EQResult rslt = new EQResult();
            rslt.SUCCESS = false;
            rslt.ROWS = 0;
            rslt.MESSAGES = "The old Password does not match";

            if (obj.USER_PASSWORD_NEW != obj.USER_PASSWORD_CONFIRM)
            {
                rslt.MESSAGES = "New and Confirm Password does not match";
                return rslt;
            }



            string sql_1 = $"SELECT T.DSMA_PASS USER_PASSWORD_OLD FROM T_DSMA T WHERE T.DSMA_DSID='{distId}' AND T.DSMA_PASS='{obj.USER_PASSWORD_OLD}'";
            Tuple<List<USER_PASSWORD>, EQResult> _tpl = DatabaseOracleClient.SqlToListObjectBind<USER_PASSWORD>(sql_1);

            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS == 1)
            {
                sql_1 = $"UPDATE T_DSMA SET DSMA_PASS='{obj.USER_PASSWORD_NEW}' WHERE DSMA_DSID ='{distId}'";
                EQResult result = DatabaseOracleClient.PostSql(sql_1);
                if (result.SUCCESS && result.ROWS == 1)
                {
                    rslt.SUCCESS = true;
                    rslt.ROWS = 1;
                    rslt.MESSAGES = "Password change succeed";
                    return rslt;
                }
                else
                {
                    rslt.MESSAGES = "Password change failed, try again";
                }
            }
            return rslt;
        }


        public static DataTable pro_legacy_do_wa(string menuId, string userId, string p1, string p2, string p3, string p4)
        {
            //OracleParameter inp_menu = new OracleParameter(parameterName: "VMENU", type: OracleDbType.Varchar2, obj: menuId, direction: ParameterDirection.Input);
            //OracleParameter inp_dist = new OracleParameter(parameterName: "VUSER", type: OracleDbType.Varchar2, obj: userId, direction: ParameterDirection.Input);
            //OracleParameter inp_par1 = new OracleParameter(parameterName: "VPARA1", type: OracleDbType.Varchar2, obj: p1, direction: ParameterDirection.Input);
            //OracleParameter inp_par2 = new OracleParameter(parameterName: "VPARA2", type: OracleDbType.Varchar2, obj: p2, direction: ParameterDirection.Input);
            //OracleParameter inp_par3 = new OracleParameter(parameterName: "VPARA3", type: OracleDbType.Varchar2, obj: p3, direction: ParameterDirection.Input);
            //OracleParameter inp_par4 = new OracleParameter(parameterName: "VPARA4", type: OracleDbType.Varchar2, obj: p4, direction: ParameterDirection.Input);
            //object[] inParams = new object[] { inp_menu, inp_dist, inp_par1, inp_par2, inp_par3, inp_par4 };

            //OracleParameter out_cur = new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            //object[] outParams = new object[] { out_cur };
            //string sql = @"BEGIN RFL.PRO_LEGACY_DO_WA(:VMENU,:VUSER,:VPARA1,:VPARA2,:VPARA3,:VPARA4,:P_CURSOR); END;";
            //return DatabaseOracleClient.GetDataSetSP(sql, inParams, outParams);

            return DatabaseOracleClientLegacy.WebAutoCommon(menuId, userId, p1, p2, p3, p4);
        }


        public static Tuple<List<T_DSMA_BAL>, EQResult> getDistBalance(string menuId, string userId, string p1, string p2, string p3, string p4)
        {
            var procData = pro_legacy_do_wa(menuId, userId, p1, p2, p3, p4);

            EQResult rslt = new EQResult();
            rslt.SUCCESS = false;
            rslt.ROWS = 0;
            if (procData.Rows.Count == 1)
            {
                var objList = DatabaseOracleClient.DataTableToListObjectBind<T_DSMA_BAL>(procData);
                if (objList.Count > 0)
                {
                    rslt.SUCCESS = true;
                    rslt.ROWS = objList.Count;
                    return new Tuple<List<T_DSMA_BAL>, EQResult>(objList, rslt);
                }
            }
            return new Tuple<List<T_DSMA_BAL>, EQResult>(new List<T_DSMA_BAL>(), rslt);
        }
    }
}