﻿using Aio.Db.Client.Entrance;
using Aio.Model;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PriOrder.App.Services
{
    public class ProductService
    {
        public static Tuple<List<WO_ITEM_TYPE>, EQResult> getCategoryListByDistId(string distId)
        {
            string sql = $@"SELECT TYP.ITEM_TYPE_ID,INITCAP(TYP.ITEM_TYPE_NAME)ITEM_TYPE_NAME,COUNT(DISTINCT IM.ITEM_CLASS_ID)ITEM_CLASS_COUNT
                        FROM RFL.ITEM_TYPE TYP
                        INNER JOIN RPGL.ITEMS IM ON TYP.ITEM_TYPE_ID=IM.ITEM_TYPE_ID
                        INNER JOIN RFL.V_DIST_CLASS D_CLASS ON D_CLASS.ITEM_CLASS_ID=IM.ITEM_CLASS_ID
                        INNER JOIN RPGL.T_DSMA DI ON D_CLASS.DIGR_DGRP=DI.DSMA_GRUP
                        WHERE IM.D_SALE_PRICE>0 AND IM.ITEM_CLASS_ID NOT IN('RC9000000','RC9500050') 
                        AND IM.INACTIVE='N' AND IM.ITEM_GROUP_ID='RFLGR001'
                        AND DI.DSMA_DSID='{distId}'
                        GROUP BY TYP.ITEM_TYPE_ID,TYP.ITEM_TYPE_NAME
                        ORDER BY TYP.ITEM_TYPE_NAME";
            return DatabaseOracleClient.SqlToListObjectBind<WO_ITEM_TYPE>(sql);

        }
        public static Tuple<List<WO_ITEM_CLASS>, EQResult> getClassByCategoryId(string distId, string categoryId)
        {
            string sql = $@"SELECT IM.ITEM_CLASS_ID,ICLS.ITEM_CLASS_NAME,IM.ITEM_TYPE_ID,COUNT(IM.ITEM_ID)ITEMS_COUNT
                        FROM RFL.ITEM_CLASS ICLS 
                        INNER JOIN RPGL.ITEMS IM ON ICLS.ITEM_CLASS_ID =IM.ITEM_CLASS_ID
                        INNER JOIN RFL.V_DIST_CLASS D_CLASS ON D_CLASS.ITEM_CLASS_ID = IM.ITEM_CLASS_ID
                        INNER JOIN RPGL.T_DSMA DI ON D_CLASS.DIGR_DGRP = DI.DSMA_GRUP
                        WHERE IM.D_SALE_PRICE > 0 AND IM.ITEM_CLASS_ID NOT IN('RC9000000', 'RC9500050')
                        AND IM.INACTIVE = 'N' AND IM.ITEM_GROUP_ID = 'RFLGR001'
                        AND DI.DSMA_DSID = '{distId}' 
                        AND IM.ITEM_TYPE_ID = '{categoryId}'
                        GROUP BY IM.ITEM_CLASS_ID,ICLS.ITEM_CLASS_NAME,IM.ITEM_TYPE_ID";

            return DatabaseOracleClient.SqlToListObjectBind<WO_ITEM_CLASS>(sql);
        }

        public static Tuple<List<WO_ITEMS>, EQResult> getProductsByClassId(string classId, string categoryId)
        {
            string sql = $@"SELECT IM.ITEM_ID,ROUND(IM.D_SALE_PRICE*IM.D_U_FACT ,2)ITEM_RATE,IM.ITEM_NAME,DU.ITEM_D_UNITS_NAME ITEM_UNIT,IM.D_U_FACT ITEM_FACTOR
                FROM RPGL.ITEMS IM 
                INNER JOIN RFL.ITEM_DIST_UNITS du ON DU.ITEM_D_UNITS_ID=IM.ITEM_D_UNITS_ID
                WHERE IM.D_SALE_PRICE > 0 AND IM.ITEM_CLASS_ID NOT IN('RC9000000', 'RC9500050')
                AND IM.INACTIVE = 'N' AND IM.ITEM_GROUP_ID = 'RFLGR001'
                AND IM.ITEM_CLASS_ID='{classId}'
                AND IM.ITEM_TYPE_ID = '{categoryId}'";
            var Items = DatabaseOracleClient.SqlToListObjectBind<WO_ITEMS>(sql);
            Items.Item1.ForEach(x => x.WO_NOTE = getItemNotes());
            return Items;
        }


        public static List<SelectListItem> getItemNotes(string id = "0")
        {
            string sql_1 = @"SELECT T.NOTE_ID,T.NOTE_TEXT FROM RPGL.WO_NOTE T WHERE T.IS_ACTIVE=1";
            Tuple<List<WO_NOTE>, EQResult> _tpl = DatabaseOracleClient.SqlToListObjectBind<WO_NOTE>(sql_1);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return _tpl.Item1.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.NOTE_TEXT,
                        Value = a.NOTE_ID,
                        Selected = (a.NOTE_ID == id)
                    };
                });
            }
            return new List<SelectListItem>();

            //objList.Add(new WO_NOTE { NOTE_ID = "2", NOTE_TEXT = "Direct Delivery for SP" });
        }

        public static EQResult AddToCart(string distId, string itemId, string qty, string noteId, string noteText)
        {
            string sql = $@"insert into wo_order_cart(dsma_dsid,ITEM_ID,item_qty,note_id,note_text)values('{distId}','{itemId}',{qty},'{noteId}','{noteText}')";
            return DatabaseOracleClient.PostSql(sql);
        }









        //SELECT distinct TYP.ITEM_TYPE_ID,INITCAP(TYP.ITEM_TYPE_NAME)ITEM_TYPE_NAME,IM.ITEM_CLASS_ID
        //FROM RFL.ITEM_TYPE TYP
        //INNER JOIN RPGL.ITEMS IM ON TYP.ITEM_TYPE_ID = IM.ITEM_TYPE_ID
        //INNER JOIN RFL.V_DIST_CLASS D_CLASS ON D_CLASS.ITEM_CLASS_ID = IM.ITEM_CLASS_ID
        //INNER JOIN RPGL.T_DSMA DI ON D_CLASS.DIGR_DGRP = DI.DSMA_GRUP
        //WHERE IM.D_SALE_PRICE > 0 AND IM.ITEM_CLASS_ID NOT IN('RC9000000', 'RC9500050')
        //AND IM.INACTIVE = 'N' AND IM.ITEM_GROUP_ID = 'RFLGR001'
        //AND DI.DSMA_DSID = '837075' and typ.ITEM_TYPE_ID = 'RFLTP025'
        //GROUP BY TYP.ITEM_TYPE_ID,TYP.ITEM_TYPE_NAME,IM.ITEM_CLASS_ID

        public static Tuple<List<WO_ITEMS>, string> getProductsByClassId_TEMP(string classId)
        {
            List<WO_ITEMS> objList = new List<WO_ITEMS>();
            //objList.Add(new T_ITMA { ITMA_ITID = "123", ITMA_NAME = "Product 1", ITMA_PRIC = 10, ITMA_GRUP = "1", ITMA_FACT = 1, ITMA_CLASS = "1", ITMA_STOCK = 15 });
            //objList.Add(new T_ITMA { ITMA_ITID = "234", ITMA_NAME = "Product 2", ITMA_PRIC = 11, ITMA_GRUP = "1", ITMA_FACT = 1, ITMA_CLASS = "1", ITMA_STOCK = 15 });
            //objList.Add(new T_ITMA { ITMA_ITID = "345", ITMA_NAME = "Product 2", ITMA_PRIC = 12, ITMA_GRUP = "1", ITMA_FACT = 1, ITMA_CLASS = "1", ITMA_STOCK = 15 });
            return new Tuple<List<WO_ITEMS>, string>(objList, "AioSuccess");
        }
        public static Tuple<List<WO_ITEM_TYPE>, string> getCategoryListByDistGroup_TEMP(string groupId)
        {
            List<WO_ITEM_TYPE> objList = new List<WO_ITEM_TYPE>();
            //objList.Add(new WO_ITEM_CATEGORY { CATEGORY_ID = "1", CATEGORY_NAME = "Home Appliance", ITEM_CLASS_COUNT = 1, ITEM_COUNT = 1 });
            //objList.Add(new WO_ITEM_CATEGORY { CATEGORY_ID = "2", CATEGORY_NAME = "Electroics", ITEM_CLASS_COUNT = 1, ITEM_COUNT = 1 });
            //objList.Add(new WO_ITEM_CATEGORY { CATEGORY_ID = "3", CATEGORY_NAME = "Plastics", ITEM_CLASS_COUNT = 1, ITEM_COUNT = 1 });
            return new Tuple<List<WO_ITEM_TYPE>, string>(objList, "AioSuccess");
        }
        public static Tuple<List<WO_ITEM_CLASS>, string> getClassByCategoryId_TEMP(string categoryId)
        {
            List<WO_ITEM_CLASS> objList = new List<WO_ITEM_CLASS>();
            objList.Add(new WO_ITEM_CLASS { ITEM_CLASS_ID = "1", ITEM_CLASS_NAME = "Class 1", ITEMS_COUNT = 10 });
            objList.Add(new WO_ITEM_CLASS { ITEM_CLASS_ID = "2", ITEM_CLASS_NAME = "Class 2", ITEMS_COUNT = 14 });
            objList.Add(new WO_ITEM_CLASS { ITEM_CLASS_ID = "3", ITEM_CLASS_NAME = "Class 3", ITEMS_COUNT = 34 });
            return new Tuple<List<WO_ITEM_CLASS>, string>(objList, "AioSuccess");
        }
    }
}