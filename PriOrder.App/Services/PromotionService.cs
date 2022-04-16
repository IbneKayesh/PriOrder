using Aio.Db.Client.Entrance;
using Aio.Model;
using Oracle.ManagedDataAccess.Client;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PriOrder.App.Services
{
    public class PromotionService
    {
        public static Tuple<List<V_CHOITALY>, EQResult> getSlabPromoByDistId(string distId)
        {
            //string sql = $@"SELECT T.SPMS_FDAT,T.SPMS_TDAT,T.SPMS_NSLB,T.SMPL_ITEM,T1.ITEM_NAME,T.SPGM_GQTY,T.GRP1,T.ITEM_GROUP,T.SPMS_ITTY
            //            FROM RFL.V_CHOITALY T INNER JOIN ITEMS T1 ON T.SMPL_ITEM=T1.ITEM_ID
            //            WHERE T.SPMS_APOV='Y' AND T.SPMS_CNCL='N' AND TRUNC(T.SPMS_TDAT) >= TRUNC(SYSDATE)
            //            ORDER BY TRUNC(T.SPMS_TDAT)";

            //OracleParameter inp_menu = new OracleParameter(parameterName: "VMENU", type: OracleDbType.Varchar2, obj: "GET_SLAB_PROMO", direction: ParameterDirection.Input);
            //OracleParameter inp_dist = new OracleParameter(parameterName: "VDISTID", type: OracleDbType.Varchar2, obj: distId, direction: ParameterDirection.Input);
            //object[] inParams = new object[] { inp_menu, inp_dist };
            //OracleParameter out_cur = new OracleParameter("OUTCURSPARM", OracleDbType.RefCursor, ParameterDirection.Output);
            //object[] outParams = new object[] { out_cur };
            //string sql = @"BEGIN RPGL.PRO_WO_GET_ALL(:VMENU,:VDISTID,:OUTCURSPARM); END;";
            //var procData = DatabaseOracleClient.GetDataSetSP(sql, inParams, outParams);

            var procData = SpService.PRO_WO_GET_ALL("GET_SLAB_PROMO", distId);
            EQResult rslt = new EQResult();
            rslt.SUCCESS = false;
            rslt.ROWS = 0;
            if (procData.Result.SUCCESS && procData.Result.ROWS == 1)
            {
                var objList = DatabaseOracleClient.DataTableToListObjectBind<V_CHOITALY>(procData.Set.Tables[0]);
                if (objList.Count > 0)
                {
                    rslt.SUCCESS = true;
                    rslt.ROWS = objList.Count;
                    return new Tuple<List<V_CHOITALY>, EQResult>(objList, rslt);
                }
            }
            return new Tuple<List<V_CHOITALY>, EQResult>(new List<V_CHOITALY>(), rslt);
        }
    }
}