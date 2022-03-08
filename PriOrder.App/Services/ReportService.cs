using Aio.Db.Client.Entrance;
using Aio.Model;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Services
{
    public class ReportService
    {
        public static Tuple<List<T_UNDELIVERED>, EQResult> getUndelivered(string distId)
        {
            string sql = $@"SELECT DIST_ID, DO_NO DONO, DO_LINE_NO, TO_CHAR(DODATE,'dd-mm-yyyy') DODATE, ITEM ITEMID, ITEMNAME, ROUND(UNDQTY,0) QTY, ROUND(UNDSQTY,0) SQTY, (UNDQTY + UNDSQTY) TQTY, ROUND(RATE,2) RATE,ROUND((UNDQTY * RATE),2) AMOUNT, WH_ID DEPOT FROM(
                    SELECT RFL.DO_MASTER.DIST_ID DIST_ID, RFL.DO_MASTER.DO_DATE DODATE, RFL.DO_MASTER.DO_NO DO_NO, RFL.DO_DETAIL.ITEM_ID ITEM, RFL.DO_DETAIL.DO_LINE_NO,
                    NVL(NVL(RFL.DO_DETAIL.QTY, 0) - SUM(NVL(RFL.CHALLAN_DETAIL.QTY, 0)), 0) UNDQTY,
                    NVL(NVL(RFL.DO_DETAIL.S_QTY, 0) - SUM(NVL(RFL.CHALLAN_DETAIL.S_QTY, 0)), 0) UNDSQTY,
                    RFL.DO_DETAIL.RATE RATE, RFL.DO_MASTER.WH_ID, IM.ITEM_NAME ITEMNAME
                    FROM RFL.DO_DETAIL,
                    RFL.DO_MASTER,
                    RFL.CHALLAN_DETAIL,
                    RFL.DISTRIBUTOR_MASTER,
                    RFL.ITEM_MASTER IM
                    WHERE  RFL.DO_DETAIL.CANCELLED = 'N'
                    AND RFL.DO_DETAIL.ACTION_TAKEN <> 'Y'
                    AND RFL.DO_MASTER.DO_DATE >= SYSDATE - 90
                    AND RFL.DISTRIBUTOR_MASTER.CANCELLED = 'N'
                    AND RFL.DISTRIBUTOR_MASTER.REGION_ID BETWEEN 'RFLRE000' AND 'RFLRE101'
                    AND RFL.DISTRIBUTOR_MASTER.SALES_ZONE_ID BETWEEN 'RFLSZ000' AND 'RFLSZ999'
                    AND RFL.DISTRIBUTOR_MASTER.CANCELLED = 'N'
                    AND RFL.DISTRIBUTOR_MASTER.SETTLED = 'N'
                    AND RFL.DO_DETAIL.DO_NO = RFL.DO_MASTER.DO_NO
                    AND RFL.CHALLAN_DETAIL.DO_LINE_NO(+) = RFL.DO_DETAIL.DO_LINE_NO
                    AND RFL.CHALLAN_DETAIL.DO_NO(+) = RFL.DO_DETAIL.DO_NO
                    AND RFL.DO_MASTER.DIST_ID = RFL.DISTRIBUTOR_MASTER.DIST_ID
                    AND RFL.DO_MASTER.DO_TRN = 'DO'
                    AND RFL.DO_DETAIL.ITEM_ID = IM.ITEM_ID
                    AND RFL.DO_MASTER.DIST_ID = '837075'
                    GROUP BY RFL.DO_MASTER.DO_NO,
                    RFL.DO_DETAIL.DO_LINE_NO,
                    DO_DETAIL.ITEM_ID,
                    RFL.DO_DETAIL.QTY,
                    RFL.DO_DETAIL.S_QTY,
                    RFL.DO_MASTER.DIST_ID,
                    RFL.DO_DETAIL.RATE,
                    RFL.DO_MASTER.DO_DATE,
                    RFL.DO_MASTER.WH_ID,
                    IM.ITEM_NAME                         
                    )
                    WHERE UNDQTY > 0 OR UNDSQTY > 0
                    GROUP BY DIST_ID,DODATE,DO_NO,DO_LINE_NO,ITEM,UNDQTY,UNDSQTY,RATE,WH_ID,ITEMNAME
                    ORDER BY DIST_ID,DODATE,DO_NO,DO_LINE_NO,ITEM";

            return DatabaseOracleClient.SqlToListObjectBind<T_UNDELIVERED>(sql);
        }

    }
}