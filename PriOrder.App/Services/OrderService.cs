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
    public class OrderService
    {

        public static Tuple<List<WO_ORDER_CART>, EQResult> getCartByDistId(string distId)
        {
            //string sql_1 = $@"SELECT IM.ITEM_ID,IM.ITEM_NAME,ROUND(IM.D_SALE_PRICE*IM.D_U_FACT ,2)ITEM_RATE,DU.ITEM_D_UNITS_NAME ITEM_UNIT,IM.D_U_FACT ITEM_FACTOR,CRT.NOTE_ID,NT.NOTE_TEXT,CRT.NOTE_TEXT NOTES,CRT.ITEM_QTY,
            //        CASE WHEN (ROUND(STK.CL,0)>ROUND(AVG20,0)) THEN ROUND(ROUND(STK.CL,0)/IM.D_U_FACT,0)
            //        ELSE 0 END NEW_STOCK
            //                FROM RPGL.ITEMS IM
            //                INNER JOIN RFL.ITEM_DIST_UNITS DU ON DU.ITEM_D_UNITS_ID = IM.ITEM_D_UNITS_ID
            //                INNER JOIN RPGL.WO_ORDER_CART CRT ON IM.ITEM_ID = CRT.ITEM_ID
            //                LEFT OUTER JOIN RPGL.WO_NOTE NT ON NT.NOTE_ID=CRT.NOTE_ID
            //                LEFT OUTER JOIN RPGL.T_NIST STK ON STK.ITEM=IM.ITEM_ID AND STK.WH=(SELECT WH_ID FROM RFL.DISTRIBUTOR_MASTER WHERE DIST_ID=CRT.DSMA_DSID)
            //                LEFT OUTER JOIN RPGL.T_WHOC WHOC ON STK.WH=WHOC.WH_ID AND STK.ITEM=WHOC.ITEM_ID
            //                WHERE CRT.DSMA_DSID = '{distId}'";
            //var Items = DatabaseOracleClient.SqlToListObjectBind<WO_ORDER_CART>(sql_1);
            //Items.Item1.ForEach(x => x.WO_NOTE = ProductService.getItemNotes(x.NOTE_ID));



            //OracleParameter inp_menu = new OracleParameter(parameterName: "VMENU", type: OracleDbType.Varchar2, obj: "GET_CART_ITEM", direction: ParameterDirection.Input);
            //OracleParameter inp_dist = new OracleParameter(parameterName: "VDISTID", type: OracleDbType.Varchar2, obj: distId, direction: ParameterDirection.Input);
            //object[] inParams = new object[] { inp_menu, inp_dist };
            //OracleParameter out_cur = new OracleParameter("OUTCURSPARM", OracleDbType.RefCursor, ParameterDirection.Output);
            //object[] outParams = new object[] { out_cur };
            //string sql = @"BEGIN RPGL.PRO_WO_GET_ALL(:VMENU,:VDISTID,:OUTCURSPARM); END;";
            //var procData = DatabaseOracleClient.GetDataSetSP(sql, inParams, outParams);

            var procData = SpService.PRO_WO_GET_ALL("GET_CART_ITEM", distId);
            EQResult rslt = new EQResult();
            rslt.SUCCESS = false;
            rslt.ROWS = 0;
            if (procData.Result.SUCCESS && procData.Result.ROWS == 1)
            {
                var objList = DatabaseOracleClient.DataTableToListObjectBind<WO_ORDER_CART>(procData.Set.Tables[0]);
                if (objList.Count > 0)
                {
                    rslt.SUCCESS = true;
                    rslt.ROWS = objList.Count;
                    return new Tuple<List<WO_ORDER_CART>, EQResult>(objList, rslt);
                }
            }
            return new Tuple<List<WO_ORDER_CART>, EQResult>(new List<WO_ORDER_CART>(), rslt);
        }

        public static EQResult DelMyCartItem(string distId, string itemId)
        {
            string sql = $@"delete wo_order_cart t where t.dsma_dsid='{distId}' and t.item_id='{itemId}'";
            return DatabaseOracleClient.PostSql(sql);
        }
        public static int getCartItemsCount(string distId)
        {
            string sql = $"select count(ITEM_ID)CART_COUNT from WO_ORDER_CART where DSMA_DSID='{distId}' AND IS_ACTIVE=1";
            Tuple<List<WO_COUNT>, EQResult> _tpl = DatabaseOracleClient.SqlToListObjectBind<WO_COUNT>(sql);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS == 1)
            {
                return _tpl.Item1.FirstOrDefault().CART_COUNT;
            }
            else
            {
                return 0;
            }
        }

        public static EQResult OrderSubmit(string distId, string itemId, string qty, string noteId, string noteText)
        {
            string sql = $@"insert into wo_order_cart(dsma_dsid,itma_itid,item_qty,note_id,note_text)values('{distId}','{itemId}',{qty},'{noteId}','{noteText}')";
            return DatabaseOracleClient.PostSql(sql);
        }

        public static EQResult ChanageOrderNote(string _distId, string _itemCode, string _noteId, string _noteValue)
        {
            string sql = string.Empty;
            if (_itemCode == "0")
            {
                sql = $"UPDATE WO_ORDER_CART SET NOTE_ID='{_noteId}',NOTE_VALUE='{_noteValue.Replace("'", "")}' WHERE DSMA_DSID='{_distId}'";
            }
            else
            {
                sql = $"UPDATE WO_ORDER_CART SET NOTE_ID='{_noteId}',NOTE_VALUE='{_noteValue.Replace("'", "")}' WHERE DSMA_DSID='{_distId}' AND ITEM_ID='{_itemCode}'";
            }
            return DatabaseOracleClient.PostSql(sql);
        }


        public static EQResult UpdateCart(string distId, List<WO_ORDER_CART> cartItems)
        {
            if (cartItems.Count > 0)
            {
                List<string> sqlList = new List<string>();
                foreach (WO_ORDER_CART item in cartItems)
                {
                    sqlList.Add($"UPDATE WO_ORDER_CART SET ITEM_QTY={item.ITEM_QTY} WHERE DSMA_DSID='{distId}' AND ITEM_ID='{item.ITEM_ID}'");
                }
                EQResult r = DatabaseOracleClient.PostSqlList(sqlList);
                //Create Order
                OracleParameter ip_did = new OracleParameter(parameterName: "DISTID", type: OracleDbType.Int32, obj: distId, direction: ParameterDirection.Input);
                object[] inParams = new object[] { ip_did };
                string sql = @"BEGIN RPGL.PRO_WO_CREATE_ORDER(:DISTID); END;";
                DatabaseOracleClient.PostSP(sql, inParams);
                return r;
            }
            return new EQResult();
        }



        public static Tuple<List<T_MBDO>, EQResult> getPendingActiveOrderByDistId(string distId)
        {
            var procData = SpService.PRO_WO_GET_ALL("GET_MBDO_PEND_ACT", distId);

            EQResult rslt = new EQResult();
            rslt.SUCCESS = false;
            rslt.ROWS = 0;
            if (procData.Result.SUCCESS && procData.Result.ROWS == 1)
            {
                var objList = DatabaseOracleClient.DataTableToListObjectBind<T_MBDO>(procData.Set.Tables[0]);
                if (objList.Count > 0)
                {
                    rslt.SUCCESS = true;
                    rslt.ROWS = objList.Count;
                    return new Tuple<List<T_MBDO>, EQResult>(objList, rslt);
                }
            }
            return new Tuple<List<T_MBDO>, EQResult>(new List<T_MBDO>(), rslt);
        }

        public static Tuple<T_MBDO_INCV, EQResult> getIncentive(string menuId, string userId, string p1, string p2, string p3, string p4)
        {
            DataTable procData = DatabaseOracleClientLegacy.WebAutoCommon(menuId, userId, p1, p2, p3, p4);

            EQResult rslt = new EQResult();
            rslt.SUCCESS = false;
            rslt.ROWS = 0;
            if (procData.Rows.Count == 1)
            {
                var objList = DatabaseOracleClient.DataTableToListObjectBind<T_MBDO_INCV>(procData);
                if (objList.Count > 0)
                {
                    rslt.SUCCESS = true;
                    rslt.ROWS = objList.Count;
                    T_MBDO_INCV obj = objList.FirstOrDefault();
                    return new Tuple<T_MBDO_INCV, EQResult>(obj, rslt);
                }
            }
            return new Tuple<T_MBDO_INCV, EQResult>(new T_MBDO_INCV(), rslt);
        }


        public static Tuple<List<T_ITMA_FD>, EQResult> getDirectDeliveryAmount(string distId)
        {
            //OracleParameter inp_menu = new OracleParameter(parameterName: "VMENU", type: OracleDbType.Varchar2, obj: "GET_CART_DIRECT_DELV", direction: ParameterDirection.Input);
            //OracleParameter inp_dist = new OracleParameter(parameterName: "VDISTID", type: OracleDbType.Varchar2, obj: distId, direction: ParameterDirection.Input);
            //object[] inParams = new object[] { inp_menu, inp_dist };
            //OracleParameter out_cur = new OracleParameter("OUTCURSPARM", OracleDbType.RefCursor, ParameterDirection.Output);
            //object[] outParams = new object[] { out_cur };
            //string sql = @"BEGIN RPGL.PRO_WO_GET_ALL(:VMENU,:VDISTID,:OUTCURSPARM); END;";
            //var procData = DatabaseOracleClient.GetDataSetSP(sql, inParams, outParams);


            var procData = SpService.PRO_WO_GET_ALL("GET_CART_DIRECT_DELV", distId);
            EQResult rslt = new EQResult();
            rslt.SUCCESS = false;
            rslt.ROWS = 0;
            if (procData.Result.SUCCESS && procData.Result.ROWS == 1)
            {
                var objList = DatabaseOracleClient.DataTableToListObjectBind<T_ITMA_FD>(procData.Set.Tables[0]);
                if (objList.Count > 0)
                {
                    rslt.SUCCESS = true;
                    rslt.ROWS = objList.Count;
                    return new Tuple<List<T_ITMA_FD>, EQResult>(objList, rslt);
                }
            }
            return new Tuple<List<T_ITMA_FD>, EQResult>(new List<T_ITMA_FD>(), rslt);
        }

        public static EQResult DeleteOrderItem(string orderId, string itemId, string distId)
        {
            List<string> sqlList = new List<string>();
            sqlList.Add($"UPDATE RPGL.T_MBDO SET MBDO_CANL='Y' WHERE MBDO_DONO='{orderId}' AND MBDO_ITID='{itemId}'");
            sqlList.Add($"DELETE FROM WO_ORDER_CART WHERE DSMA_DSID='{distId}' AND ITEM_ID='{itemId}'");
            return DatabaseOracleClient.PostSqlList(sqlList);
        }

        public static EQResult ActiveOrder(string distId)
        {
            List<string> sqlList = new List<string>();
            string sql = $"UPDATE rpgl.T_MBDO SET MBDO_ACTV='Y' where MBDO_CANL='N' and to_char(MBDO_DODT,'dd-mm-yyyy')=to_char(SYSDATE,'dd-mm-yyyy') AND MBDO_ACTV='N' and MBDO_DSID='{distId}'";
            sqlList.Add(sql);
            sqlList.Add($"UPDATE WO_ORDER_CART SET IS_ACTIVE=0 WHERE DSMA_DSID='{distId}'");
            return DatabaseOracleClient.PostSqlList(sqlList);
        }
    }
}