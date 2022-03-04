using Aio.Db.Client.Entrance;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Services
{
    public class OrderService
    {
        public static Tuple<List<WO_ORDER_CART>, string> getCartByDistId_TEMP(string distId)
        {
            List<WO_ORDER_CART> objList = new List<WO_ORDER_CART>();
            objList.Add(new WO_ORDER_CART { DSMA_DSID = "1", ITMA_ITID = "1", ITMA_NAME = "Product 1", ITEM_QTY = 10, ITMA_PRIC = 13.5M, NOTE_ID = "100", NOTE_TEXT = "General Delivery" });
            objList.Add(new WO_ORDER_CART { DSMA_DSID = "1", ITMA_ITID = "2", ITMA_NAME = "Product 2", ITEM_QTY = 50, ITMA_PRIC = 58.3M, NOTE_ID = "101", NOTE_TEXT = "Home Delivery" });
            objList.Add(new WO_ORDER_CART { DSMA_DSID = "1", ITMA_ITID = "3", ITMA_NAME = "Product 3", ITEM_QTY = 40, ITMA_PRIC = 28.3M, NOTE_ID = "100", NOTE_TEXT = "General Delivery" });
            return new Tuple<List<WO_ORDER_CART>, string>(objList, "AioSuccess");
        }
        public static string OrderSubmit(string distId, string itemId, string qty, string noteId, string noteText)
        {
            string sql = $@"insert into wo_order_cart(dsma_dsid,itma_itid,item_qty,note_id,note_text)values('{distId}','{itemId}',{qty},'{noteId}','{noteText}')";
            return DatabaseOracleClient.PostSql(sql);
        }
    }
}