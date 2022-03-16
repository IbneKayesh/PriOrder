using Aio.Db.Client.Entrance;
using Aio.Model;
using Oracle.ManagedDataAccess.Client;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace PriOrder.App.Services
{
    public class ProductService
    {
        public static Tuple<List<WO_ITEM_TYPE>, EQResult> getCategoryListByDistId(string distId)
        {
            OracleParameter inp_menu = new OracleParameter(parameterName: "VMENU", type: OracleDbType.Varchar2, obj: "ITMTYPE", direction: ParameterDirection.Input);
            OracleParameter inp_dist = new OracleParameter(parameterName: "VDISTID", type: OracleDbType.Varchar2, obj: distId, direction: ParameterDirection.Input);
            object[] inParams = new object[] { inp_menu, inp_dist };
            OracleParameter out_cur = new OracleParameter("OUTCURSPARM", OracleDbType.RefCursor, ParameterDirection.Output);
            object[] outParams = new object[] { out_cur };
            string sql = @"BEGIN RPGL.PRO_WO_GET_ALL(:VMENU,:VDISTID,:OUTCURSPARM); END;";
            var procData = DatabaseOracleClient.GetDataSetSP(sql, inParams, outParams);

            EQResult rslt = new EQResult();
            rslt.SUCCESS = false;
            rslt.ROWS = 0;
            if (procData.Item2.SUCCESS && procData.Item2.ROWS == 1)
            {
                var objList = DatabaseOracleClient.DataTableToListObjectBind<WO_ITEM_TYPE>(procData.Item1.Tables[0]);
                if (objList.Count > 0)
                {
                    rslt.SUCCESS = true;
                    rslt.ROWS = objList.Count;
                    return new Tuple<List<WO_ITEM_TYPE>, EQResult>(objList, rslt);
                }
            }
            return new Tuple<List<WO_ITEM_TYPE>, EQResult>(new List<WO_ITEM_TYPE>(), rslt);
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

        //public static Tuple<List<WO_ITEMS>, EQResult> getProductsByClassId(string classId, string categoryId)
        //{
        //    string sql = $@"SELECT IM.ITEM_ID,ROUND(IM.D_SALE_PRICE*IM.D_U_FACT,2)ITEM_RATE,IM.ITEM_NAME,DU.ITEM_D_UNITS_NAME ITEM_UNIT,IM.D_U_FACT ITEM_FACTOR FROM RPGL.ITEMS IM 
        //        INNER JOIN RFL.ITEM_DIST_UNITS du ON DU.ITEM_D_UNITS_ID=IM.ITEM_D_UNITS_ID
        //        WHERE IM.D_SALE_PRICE > 0 AND IM.ITEM_CLASS_ID NOT IN('RC9000000', 'RC9500050')
        //        AND IM.INACTIVE = 'N' AND IM.ITEM_GROUP_ID = 'RFLGR001'
        //        AND IM.ITEM_CLASS_ID='{classId}'
        //        AND IM.ITEM_TYPE_ID = '{categoryId}'";
        //    var Items = DatabaseOracleClient.SqlToListObjectBind<WO_ITEMS>(sql);
        //    Items.Item1.ForEach(x => x.WO_NOTE = getItemNotes());
        //    return Items;
        //}
        public static Tuple<List<WO_ITEMS>, EQResult> getProductsByClassId(string classId, string categoryId, string distId)
        {
            string sql = $@"SELECT IM.ITEM_ID,ROUND(IM.D_SALE_PRICE*IM.D_U_FACT,2)ITEM_RATE,IM.ITEM_NAME,DU.ITEM_D_UNITS_NAME ITEM_UNIT,IM.D_U_FACT ITEM_FACTOR,
                    ROUND(NVL(STK.CL,0),0)NIST_STOCK,ROUND(NVL(AVG20,0),0)WHOC_AVG,
                    CASE WHEN (ROUND(STK.CL,0)>ROUND(AVG20,0)) THEN ROUND(ROUND(STK.CL,0)/IM.D_U_FACT,0)
                    ELSE 0 END NEW_STOCK
                    FROM RPGL.ITEMS IM 
                    INNER JOIN RFL.ITEM_DIST_UNITS DU ON DU.ITEM_D_UNITS_ID=IM.ITEM_D_UNITS_ID
                    LEFT OUTER JOIN RPGL.T_NIST STK ON STK.ITEM=IM.ITEM_ID AND STK.WH=(SELECT WH_ID FROM RFL.DISTRIBUTOR_MASTER WHERE DIST_ID='{distId}')
                    LEFT OUTER JOIN RPGL.T_WHOC WHOC ON STK.WH=WHOC.WH_ID AND STK.ITEM=WHOC.ITEM_ID
                    WHERE IM.D_SALE_PRICE > 0 AND IM.ITEM_CLASS_ID NOT IN('RC9000000','RC9500050')
                    AND IM.INACTIVE='N' AND IM.ITEM_GROUP_ID='RFLGR001'
                    AND IM.ITEM_CLASS_ID='{classId}'
                    AND IM.ITEM_TYPE_ID='{categoryId}'";
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
            if (noteId == "0")
            {
                noteId = "";
            }

            string sql = $"UPDATE WO_ORDER_CART SET ITEM_QTY=ITEM_QTY+{qty} WHERE DSMA_DSID='{distId}' AND ITEM_ID='{itemId}'";
            EQResult result = DatabaseOracleClient.PostSql(sql);
            if (result.SUCCESS && result.ROWS == 0)
            {
                sql = $@"INSERT INTO WO_ORDER_CART(DSMA_DSID,ITEM_ID,ITEM_QTY,NOTE_ID,NOTE_VALUE)VALUES('{distId}','{itemId}',{qty},'{noteId}','{noteText.Replace("'", "")}')";
                result = DatabaseOracleClient.PostSql(sql);
            }
            return result;
        }


        public static Tuple<List<WO_ITEMS>, EQResult> getFavoriteProductsByDistid(string distId)
        {
            string sql = $@"SELECT IM.ITEM_ID,ROUND(IM.D_SALE_PRICE*IM.D_U_FACT ,2)ITEM_RATE,IM.ITEM_NAME,DU.ITEM_D_UNITS_NAME ITEM_UNIT,IM.D_U_FACT ITEM_FACTOR,
                    ROUND(NVL(STK.CL,0),0)NIST_STOCK,ROUND(NVL(AVG20,0),0)WHOC_AVG,
                    CASE WHEN (ROUND(STK.CL,0)>ROUND(AVG20,0)) THEN ROUND(ROUND(STK.CL,0)/IM.D_U_FACT,0)
                    ELSE 0 END NEW_STOCK
                    FROM RPGL.ITEMS IM
                    INNER JOIN RFL.ITEM_DIST_UNITS du ON DU.ITEM_D_UNITS_ID=IM.ITEM_D_UNITS_ID
                    INNER JOIN WO_DIST_FAV FVI ON FVI.ITEM_ID=IM.ITEM_ID AND FVI.IS_ACTIVE=1 AND FVI.DSMA_DSID='{distId}'
                    LEFT OUTER JOIN RPGL.T_NIST STK ON STK.ITEM=IM.ITEM_ID AND STK.WH=(SELECT WH_ID FROM RFL.DISTRIBUTOR_MASTER WHERE DIST_ID=FVI.DSMA_DSID)
                    LEFT OUTER JOIN RPGL.T_WHOC WHOC ON STK.WH=WHOC.WH_ID AND STK.ITEM=WHOC.ITEM_ID
                    WHERE IM.D_SALE_PRICE > 0 AND IM.ITEM_CLASS_ID NOT IN('RC9000000', 'RC9500050')
                    AND IM.INACTIVE = 'N' AND IM.ITEM_GROUP_ID = 'RFLGR001'";


            var Items = DatabaseOracleClient.SqlToListObjectBind<WO_ITEMS>(sql);
            Items.Item1.ForEach(x => x.WO_NOTE = getItemNotes());
            return Items;
        }

        public static EQResult AddToFav(string distId, string itemId)
        {
            string sql = $@"insert into WO_DIST_FAV(DSMA_DSID,ITEM_ID)values('{distId}','{itemId}')";
            return DatabaseOracleClient.PostSql(sql);
        }
        public static EQResult DelFromFav(string distId, string itemId)
        {
            string sql = $@"delete WO_DIST_FAV where DSMA_DSID='{distId}' and ITEM_ID='{itemId}'";
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


    }
}