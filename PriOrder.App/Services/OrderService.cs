using Aio.Db.Client.Entrance;
using Aio.Model;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Services
{
    public class OrderService
    {

        public static Tuple<List<WO_ORDER_CART>, EQResult> getCartByDistId(string distId)
        {
            string sql_1 = $@"SELECT IM.ITEM_ID,IM.ITEM_NAME,ROUND(IM.D_SALE_PRICE*IM.D_U_FACT ,2)ITEM_RATE,DU.ITEM_D_UNITS_NAME ITEM_UNIT,IM.D_U_FACT ITEM_FACTOR,CRT.NOTE_ID,NT.NOTE_TEXT,CRT.NOTE_TEXT NOTES,CRT.ITEM_QTY,
                    CASE WHEN (ROUND(STK.CL,0)>ROUND(AVG20,0)) THEN ROUND(ROUND(STK.CL,0)/IM.D_U_FACT,0)
                    ELSE 0 END NEW_STOCK
                            FROM RPGL.ITEMS IM
                            INNER JOIN RFL.ITEM_DIST_UNITS DU ON DU.ITEM_D_UNITS_ID = IM.ITEM_D_UNITS_ID
                            INNER JOIN RPGL.WO_ORDER_CART CRT ON IM.ITEM_ID = CRT.ITEM_ID
                            LEFT OUTER JOIN RPGL.WO_NOTE NT ON NT.NOTE_ID=CRT.NOTE_ID
                            LEFT OUTER JOIN RPGL.T_NIST STK ON STK.ITEM=IM.ITEM_ID AND STK.WH=(SELECT WH_ID FROM RFL.DISTRIBUTOR_MASTER WHERE DIST_ID=CRT.DSMA_DSID)
                            LEFT OUTER JOIN RPGL.T_WHOC WHOC ON STK.WH=WHOC.WH_ID AND STK.ITEM=WHOC.ITEM_ID
                            WHERE CRT.DSMA_DSID = '{distId}'";
            var Items = DatabaseOracleClient.SqlToListObjectBind<WO_ORDER_CART>(sql_1);
            Items.Item1.ForEach(x => x.WO_NOTE = ProductService.getItemNotes(x.NOTE_ID));
            return Items;
        }

        public static EQResult DelMyCartItem(string distId, string itemId)
        {
            string sql = $@"delete wo_order_cart t where t.dsma_dsid='{distId}' and t.item_id='{itemId}'";
            return DatabaseOracleClient.PostSql(sql);
        }

        public static EQResult OrderSubmit(string distId, string itemId, string qty, string noteId, string noteText)
        {
            string sql = $@"insert into wo_order_cart(dsma_dsid,itma_itid,item_qty,note_id,note_text)values('{distId}','{itemId}',{qty},'{noteId}','{noteText}')";
            return DatabaseOracleClient.PostSql(sql);
        }


       
    }
}