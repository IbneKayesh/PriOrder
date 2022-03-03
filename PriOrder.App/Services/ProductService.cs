using Aio.Db.Client.Entrance;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Services
{
    public class ProductService
    {
        public static Tuple<List<WO_ITEM_CATEGORY>, string> getCategoryListByDistGroup(string groupId)
        {
            string sql = $@"select t.category_id,t.category_name,'/Assets/images/items/item.jpg' category_image,t.item_class_count,t.item_count from wo_item_category t where t.is_active=1 order by t.category_name";
            return DatabaseOracleClient.SqlToListObjectBind<WO_ITEM_CATEGORY>(sql);
        }

        public static Tuple<List<WO_ITEM_CLASS>, string> getClassByCategoryId(string categoryId)
        {
            string sql = $@"select t.item_class_id,t.item_class_name,'/Assets/images/items/item.jpg' item_class_image,t.item_count  from wo_item_class t where t.is_active=1 and t.category_id='{categoryId}' order by t.item_class_name";
            return DatabaseOracleClient.SqlToListObjectBind<WO_ITEM_CLASS>(sql);
        }

        public static Tuple<List<T_ITMA>, string> getProductsByClassId(string classId)
        {
            string sql = $@"select t.itma_itid,t.itma_name,t.itma_pric,t.itma_grup,t.itma_fact,t.itma_class,'/Assets/images/items/item.jpg' itma_imge,0 itma_stock from t_itma t where t.itma_class='{classId}'";
            return DatabaseOracleClient.SqlToListObjectBind<T_ITMA>(sql);
        }

        public static string AddToCart(string distId, string itemId, string qty, string noteId, string noteText)
        {
            string sql = $@"insert into wo_order_cart(dsma_dsid,itma_itid,item_qty,note_id,note_text)values('{distId}','{itemId}',{qty},'{noteId}','{noteText}')";
            return DatabaseOracleClient.PostSql(sql);
        }
    }
}