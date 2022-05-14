using Aio.Db.Client.Entrance;
using Aio.Model;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Services
{
    public class NFService
    {
        public static Tuple<List<NIF_DIST>, EQResult> getNIF_DIST(string distId)
        {
            string sql = $@"SELECT DIST_ID FROM NIF_DIST WHERE DIST_ID='{distId}'";
            return DatabaseOracleClient.SqlToListObjectBind<NIF_DIST>(sql);
        }


        public static Tuple<List<T_DTDM>, EQResult> getDistrict()
        {
            string sql = $@"SELECT D.DTDM_TEXT,D.DTDM_NAME FROM RFL.T_DTDM D WHERE D.GEO_CODE IS NOT NULL ORDER BY D.DTDM_NAME";
            return DatabaseOracleClient.SqlToListObjectBind<T_DTDM>(sql);
        }
        public static Tuple<List<T_DTNM>, EQResult> getThanaByDistrId(string distrId)
        {
            string sql = $@"SELECT DISTINCT TN.DTNM_TEXT,TN.DTNM_NAME FROM RFL.T_DTNM TN JOIN RFL.T_DITHUN T ON T.THANA_TEXT=TN.DTNM_TEXT WHERE T.DSTRT_TEXT='{distrId}' ORDER BY TN.DTNM_NAME";
            return DatabaseOracleClient.SqlToListObjectBind<T_DTNM>(sql);
        }
        public static Tuple<List<T_DITHUN>, EQResult> getUnionByThanaId(string thanaId)
        {
            string sql = $@"SELECT U.UNION_TEXT,U.UNION_NAME FROM RFL.T_DITHUN U WHERE U.THANA_TEXT='{thanaId}' ORDER BY U.UNION_NAME";
            return DatabaseOracleClient.SqlToListObjectBind<T_DITHUN>(sql);
        }

        public static EQResult ApplicationCreate(NIF_APPL obj, string distId)
        {
            List<string> sqlList = new List<string>();
            string sql = $@"INSERT INTO RPGL.NIF_APPL(APPL_NID,NID_IMG,APPL_IMG,FULL_NAME,BIRTH_DATE,MOBILE_NUMBER,EMAIL_ADDRESS,FATHER_NAME,MOTHER_NAME,PARENTS_MOBILE,SPOUSE_NAME,SPOUSE_MOBILE,HOUSE_ROAD,VILLAGE_NAME,UNION_NAME,POLICE_STATION,DISTRICT,HOUSE_ROAD2,VILLAGE_NAME2,UNION_NAME2,POLICE_STATION2,DISTRICT2,AS_PRESENT,ACCOUNT_NAME,ACCOUNT_NO,BANK_NAME,BRANCH_NAME,IS_ACTIVE,CREATE_DATE,CREATE_USER,DIST_TIN,DIST_BIN,DIST_TRADE) VALUES('{obj.APPL_NID.Trim().Replace("'", "")}','0','0','{obj.FULL_NAME.Trim().Replace("'", "")}',TO_DATE('{obj.BIRTH_DATE.ToString("dd/MMM/yyyy")}','dd/MON/yyyy'),'{obj.MOBILE_NUMBER.Trim().Replace("'", "")}','{obj.EMAIL_ADDRESS.Trim().Replace("'", "")}','{obj.FATHER_NAME.Trim().Replace("'", "")}','{obj.MOTHER_NAME.Trim().Replace("'", "")}','{obj.PARENTS_MOBILE.Trim().Replace("'", "")}','{obj.SPOUSE_NAME.Trim().Replace("'", "")}','{obj.SPOUSE_MOBILE.Trim().Replace("'", "")}','{obj.HOUSE_ROAD.Trim().Replace("'", "")}','{obj.VILLAGE_NAME.Trim().Replace("'", "")}','{obj.UNION_NAME.Trim().Replace("'", "")}','{obj.POLICE_STATION.Trim().Replace("'", "")}','{obj.DISTRICT.Trim().Replace("'", "")}','{obj.HOUSE_ROAD2.Trim().Replace("'", "")}','{obj.VILLAGE_NAME2.Trim().Replace("'", "")}','{obj.UNION_NAME2.Trim().Replace("'", "")}','{obj.POLICE_STATION2.Trim().Replace("'", "")}','{obj.DISTRICT2.Trim().Replace("'", "")}',{obj.AS_PRESENT},'{obj.ACCOUNT_NAME.Trim().Replace("'", "")}','{obj.ACCOUNT_NO.Trim().Replace("'", "")}','{obj.BANK_NAME.Trim().Replace("'", "")}','{obj.BRANCH_NAME.Trim().Replace("'", "")}',1,sysdate,'{distId}','{obj.DIST_TIN.Trim().Replace("'", "")}','{obj.DIST_BIN.Trim().Replace("'", "")}','{obj.DIST_TRADE.Trim().Replace("'", "")}')";
            sqlList.Add(sql);

            sql = NIF_DIST(obj.APPL_NID.Trim().Replace("'", ""), distId, obj.DIST_TIN.Trim().Replace("'", ""), obj.DIST_BIN.Trim().Replace("'", ""), obj.DIST_TRADE.Trim().Replace("'", ""));
            if (sql == "X")
            {
                return new EQResult() { ROWS = 0, MESSAGES = "Already Applied", SUCCESS = false };
            }
            sqlList.Add(sql);
            return DatabaseOracleClient.PostSqlList(sqlList);
        }

        private static string NIF_DIST(string dsto_nidn, string distId, string tin, string bin, string trade)
        {

            string sql_1 = @"SELECT T.DIST_ID,T.DIST_NAME,T.DIGR_TEXT,T.OPDATE,T.CLDATE,T.CANCELLED
                            FROM RFL.DISTRIBUTOR_MASTER T
                            WHERE T.CANCELLED='N' AND T.CLDATE IS NULL AND T.DIST_CAT_ID='DC001' AND T.DIST_TYPE_ID='T001'
                            AND T.SETTLED='N' AND T.DIST_ID='" + distId + "' AND T.DIST_ID NOT IN (SELECT T.DIST_ID FROM RPGL.NIF_DIST T)";
            Tuple<List<DISTRIBUTOR_MASTER>, EQResult> obj = DatabaseOracleClient.SqlToListObjectBind<DISTRIBUTOR_MASTER>(sql_1);
            if (obj.Item2.ROWS == 1)
            {
                return $@"INSERT INTO RPGL.NIF_DIST(APPL_NID,DIST_ID,DIST_NAME,DIGR_TEXT,OPDATE,CLDATE,CANCELLED,DIST_NID,DIST_TIN,DIST_BIN,DIST_TRADE,IS_ACTIVE,CREATE_DATE,CREATE_USER) VALUES('{dsto_nidn.Trim().Replace("'", "")}','{obj.Item1.First().DIST_ID}','{obj.Item1.First().DIST_NAME}','{obj.Item1.First().DIGR_TEXT}',TO_DATE('{obj.Item1.First().OPDATE.ToString("dd/MMM/yyyy")}','dd/MON/yyyy'),'','N','XX','{tin.Trim().Replace("'", "")}','{bin.Trim().Replace("'", "")}','{trade.Trim().Replace("'", "")}',0,sysdate,'" + distId + "')";
            }
            else
            {
                return "X";
            }
        }
        public static EQResult AddDistributor(string nid, string distId)
        {
            List<string> sqlList = new List<string>();
            string sql = NIF_DIST(nid, distId, "Z", "Z", "Z");
            if (sql == "X")
            {
                return new EQResult() { ROWS = 0, MESSAGES = "Already Applied", SUCCESS = false };
            }
            sqlList.Add(sql);
            return DatabaseOracleClient.PostSqlList(sqlList);
        }

        public static EQResult AddNominee(NIF_NOMI obj, string distId)
        {
            string sql_1 = $@"SELECT * FROM NIF_NOMI WHERE APPL_NID='{obj.APPL_NID}'";
            Tuple<List<NIF_NOMI>, EQResult> Obj = DatabaseOracleClient.SqlToListObjectBind<NIF_NOMI>(sql_1);
            //2 nominee added
            if (Obj.Item2.ROWS == 2 )
            {
                return new EQResult() { ROWS = 0, MESSAGES = "Already Applied", SUCCESS = false };
            }
            //1st or second added
            sql_1 = $@"SELECT * FROM NIF_NOMI WHERE APPL_NID='{obj.APPL_NID}' AND NOMI_ID='{obj.NOMI_ID}'";
            Obj = DatabaseOracleClient.SqlToListObjectBind<NIF_NOMI>(sql_1);
            if (Obj.Item2.ROWS == 1)
            {
                return new EQResult() { ROWS = 0, MESSAGES = "Already Applied", SUCCESS = false };
            }
            //nominee NID already added with another
            sql_1 = $@"SELECT * FROM NIF_NOMI WHERE NOMI_NID='{obj.NOMI_NID}'";
            Obj = DatabaseOracleClient.SqlToListObjectBind<NIF_NOMI>(sql_1);
            if (Obj.Item2.ROWS == 1)
            {
                return new EQResult() { ROWS = 0, MESSAGES = "Already Applied", SUCCESS = false };
            }

            List<string> sqlList = new List<string>();
            string sql = $@"INSERT INTO RPGL.NIF_NOMI(NOMI_ID,APPL_NID,NOMI_NID,NID_IMG,NOM_IMG,FULL_NAME,BIRTH_DATE,MOBILE_NUMBER,EMAIL_ADDRESS,FATHER_NAME,MOTHER_NAME,PARENTS_MOBILE,SPOUSE_NAME,SPOUSE_MOBILE,HOUSE_ROAD,VILLAGE_NAME,UNION_NAME,POLICE_STATION,DISTRICT,RELATION_APPL,CONTRIBUTION,CREATE_USER) VALUES({obj.NOMI_ID},'{obj.APPL_NID}','{obj.NOMI_NID.Trim().Replace("'", "")}','0','0','{obj.FULL_NAME.Trim().Replace("'", "")}',TO_DATE('{obj.BIRTH_DATE.ToString("dd/MMM/yyyy")}','dd/MON/yyyy'),'{obj.MOBILE_NUMBER.Trim().Replace("'", "")}','{obj.EMAIL_ADDRESS.Trim().Replace("'", "")}','{obj.FATHER_NAME.Trim().Replace("'", "")}','{obj.MOTHER_NAME.Trim().Replace("'", "")}','{obj.PARENTS_MOBILE.Trim().Replace("'", "")}','{obj.SPOUSE_NAME.Trim().Replace("'", "")}','{obj.SPOUSE_MOBILE.Trim().Replace("'", "")}','{obj.HOUSE_ROAD.Trim().Replace("'", "")}','{obj.VILLAGE_NAME.Trim().Replace("'", "")}','{obj.UNION_NAME.Trim().Replace("'", "")}','{obj.POLICE_STATION.Trim().Replace("'", "")}','{obj.DISTRICT.Trim().Replace("'", "")}','{obj.RELATION_APPL.Trim().Replace("'", "")}','{obj.CONTRIBUTION}','" + distId + "')";
            sqlList.Add(sql);
            return DatabaseOracleClient.PostSqlList(sqlList);
        }
    }
}