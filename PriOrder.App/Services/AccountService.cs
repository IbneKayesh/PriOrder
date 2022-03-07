using Aio.Db.Client.Entrance;
using Aio.Model;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Services
{
    public class AccountService
    {
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
    }
}