using Aio.Db.Client.Entrance;
using Aio.Model;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Services
{
    public class PromotionService
    {
        public static Tuple<List<V_CHOITALY>, EQResult> getSlabPromo()
        {
            string sql = $@"SELECT T.SPMS_FDAT,T.SPMS_TDAT,T.SPMS_NSLB,T.SMPL_ITEM,T1.ITEM_NAME,T.SPGM_GQTY,T.GRP1,T.ITEM_GROUP,T.SPMS_ITTY
                        FROM RFL.V_CHOITALY T INNER JOIN ITEMS T1 ON T.SMPL_ITEM=T1.ITEM_ID
                        WHERE T.SPMS_APOV='Y' AND T.SPMS_CNCL='N' AND TRUNC(T.SPMS_TDAT) >= TRUNC(SYSDATE)
                        ORDER BY TRUNC(T.SPMS_TDAT)";
            return DatabaseOracleClient.SqlToListObjectBind<V_CHOITALY>(sql);
        }
    }
}