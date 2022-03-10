using Aio.Db.Client.Entrance;
using Aio.Model;
using Oracle.ManagedDataAccess.Client;
using PriOrder.App.Models;
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
            string sql = $@"SELECT T1.DSTO_NIDN,T.DIST_ID,T1.DSMA_NAME,T2.DGIG_DNAM,TO_CHAR(T.OPDATE)OPDATE,TO_CHAR(T.CLDATE)CLDATE, CASE WHEN T.CANCELLED = 'N' THEN 'Active' ELSE 'Inactive' END STATUS, CASE WHEN T.IS_ACTIVE = 1 THEN 'Approved' ELSE 'Not Approved' END APPROVAL FROM NIF_DIST T
            INNER JOIN T_DSMA T1 ON T.DIST_ID = T1.DSMA_DSID
            LEFT OUTER JOIN T_DGIG T2 ON T1.DSMA_GRUP = T2.DGIG_DSGP
            WHERE T.APPL_NID IN (SELECT T.APPL_NID
            FROM  RPGL.NIF_DIST T WHERE T.DIST_ID='{distId}')
            ORDER BY T2.DGIG_DNAM";
            return DatabaseOracleClient.SqlToListObjectBind<NIF_DIST>(sql);
        }

        public static void ExecuteSP()
        {
            OracleParameter ip_menu = new OracleParameter(parameterName: "VMENU", type: OracleDbType.Varchar2, obj: "sp check controller", direction: ParameterDirection.Input);
            OracleParameter ip_spid = new OracleParameter(parameterName: "VSUPID", type: OracleDbType.Varchar2, obj: "1", direction: ParameterDirection.Input);

            object[] inParams = new object[] { ip_menu, ip_spid };

            string sql = @"BEGIN RPGL.PRO_GET_SUPP_MSG(:VMENU,:VSUPID); END;";

            var xxxx = DatabaseOracleClient.PostSP(sql, inParams);
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
            string sql = $@"select tdist.dsto_nidn,tdtp.dist_type_id,tdtp.dist_type, tdist.dsma_dsid,tdist.dsma_name,rdist.addr1,rcnt.contacts,rdist.addr2 || ', '||rdist.addr3 addr2,tg.digr_text,tg.digr_name,tsz.sales_zone_id,tsz.sales_zone,tdz.dist_zone_id,tdz.dist_zone,rzcnt.contacts zcontacts from rpgl.t_dsma tdist
                join rfl.distributor_master rdist on rdist.dist_id=tdist.dsma_dsid
                join rfl.t_digr tg on  tdist.dsma_grup=tg.digr_text
                join rfl.sales_zone tsz on  rdist.sales_zone_id=tsz.sales_zone_id
                join rfl.distributor_zone tdz on rdist.dist_zone_id=tdz.dist_zone_id
                join rfl.distributor_type tdtp on rdist.dist_type_id=tdtp.dist_type_id
                left outer join rfl.distributor_contacts rcnt on tdist.dsma_dsid=rcnt.dist_id and rcnt.seqn=101
                left outer join rfl.distributor_contacts rzcnt on tdist.dsma_dsid=rzcnt.dist_id and rzcnt.seqn=103
                where tdist.dsma_dsid='{distId}'";
            return DatabaseOracleClient.SqlToListObjectBind<T_DSMA>(sql);
        }
    }
}