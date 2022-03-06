using Aio.Db.Client.Entrance;
using Aio.Model;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;

namespace PriOrder.App.Services
{
    public class SetupService
    {
        public static Tuple<List<WO_ITEM_TYPE>, EQResult> getCategoryList()
        {
            string sql = $@"SELECT TYP.ITEM_TYPE_ID,INITCAP(TYP.ITEM_TYPE_NAME)ITEM_TYPE_NAME,COUNT(DISTINCT IM.ITEM_CLASS_ID)ITEM_CLASS_COUNT
                        FROM RFL.ITEM_TYPE TYP
                        INNER JOIN RPGL.ITEMS IM ON TYP.ITEM_TYPE_ID=IM.ITEM_TYPE_ID
                        WHERE IM.D_SALE_PRICE>0 AND IM.ITEM_CLASS_ID NOT IN('RC9000000','RC9500050') 
                        AND IM.INACTIVE='N' AND IM.ITEM_GROUP_ID='RFLGR001'
                        GROUP BY TYP.ITEM_TYPE_ID,TYP.ITEM_TYPE_NAME
                        ORDER BY TYP.ITEM_TYPE_NAME";
            return DatabaseOracleClient.SqlToListObjectBind<WO_ITEM_TYPE>(sql);
        }
        public static Tuple<List<WO_ITEM_TYPE>, EQResult> getCategoryById(string id)
        {
            string sql = $@"SELECT TYP.ITEM_TYPE_ID,INITCAP(TYP.ITEM_TYPE_NAME)ITEM_TYPE_NAME
                        FROM RFL.ITEM_TYPE TYP WHERE TYP.ITEM_TYPE_ID='{id}'";
            return DatabaseOracleClient.SqlToListObjectBind<WO_ITEM_TYPE>(sql);
        }

        public static Tuple<List<WO_ITEM_CLASS>, EQResult> getClassByCategoryId(string categoryId)
        {
            string sql = $@"SELECT IM.ITEM_CLASS_ID,ICLS.ITEM_CLASS_NAME,IM.ITEM_TYPE_ID,COUNT(IM.ITEM_ID)ITEMS_COUNT
                        FROM RFL.ITEM_CLASS ICLS 
                        INNER JOIN RPGL.ITEMS IM ON ICLS.ITEM_CLASS_ID=IM.ITEM_CLASS_ID
                        WHERE IM.D_SALE_PRICE > 0 AND IM.ITEM_CLASS_ID NOT IN('RC9000000', 'RC9500050')
                        AND IM.INACTIVE = 'N' AND IM.ITEM_GROUP_ID = 'RFLGR001'
                        AND IM.ITEM_TYPE_ID = '{categoryId}'
                        GROUP BY IM.ITEM_CLASS_ID,ICLS.ITEM_CLASS_NAME,IM.ITEM_TYPE_ID";
            return DatabaseOracleClient.SqlToListObjectBind<WO_ITEM_CLASS>(sql);
        }

        public static Tuple<List<WO_ITEM_CLASS>, EQResult> getClassById(string classId)
        {
            string sql = $@"SELECT IM.ITEM_CLASS_ID,ICLS.ITEM_CLASS_NAME
                        FROM RFL.ITEM_CLASS ICLS  WHERE ICLS.ITEM_CLASS_ID={classId}'";
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
            return DatabaseOracleClient.SqlToListObjectBind<WO_ITEMS>(sql);
        }
        public static Tuple<List<WO_ITEMS>, EQResult> getProductById(string productId)
        {
            string sql = $@"SELECT IM.ITEM_ID,IM.ITEM_NAME
                FROM RPGL.ITEMS IM
                WHERE IM.D_SALE_PRICE > 0 AND IM.ITEM_CLASS_ID NOT IN('RC9000000', 'RC9500050')
                AND IM.INACTIVE = 'N' AND IM.ITEM_GROUP_ID = 'RFLGR001'
                AND IM.ITEM_ID='{productId}'";
            return DatabaseOracleClient.SqlToListObjectBind<WO_ITEMS>(sql);
        }
    }
}