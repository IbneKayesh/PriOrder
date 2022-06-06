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
            if (Obj.Item2.ROWS == 2)
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

        public static NIF_APPL_VIEW ViewAppl(string distId)
        {
            var obj = new NIF_APPL_VIEW();
            string sql = $@"select na.appl_nid,na.nid_img,na.appl_img,na.full_name,na.birth_date,na.mobile_number,
                        na.email_address,na.father_name,na.mother_name,na.parents_mobile,na.spouse_name,na.spouse_mobile,
                        na.house_road,na.village_name,un1.union_name union_name,ps1.dtnm_name police_station,ds1.dtdm_name district,
                        na.house_road2,na.village_name2,un2.union_name union_name2,ps2.dtnm_name police_station2,ds2.dtdm_name district2,
                        na.account_name,na.account_no,na.bank_name,na.branch_name,na.appl_date,
                        na.dist_tin,na.dist_bin,na.dist_trade
                        from nif_appl na
                        left join rfl.t_dithun un1 on na.union_name=un1.union_text
                        left join rfl.t_dtnm ps1 on na.police_station= ps1.dtnm_text
                        left join rfl.t_dtdm ds1 on na.district= ds1.dtdm_text
                        left join rfl.t_dithun un2 on na.union_name2=un2.union_text
                        left join rfl.t_dtnm ps2 on na.police_station2= ps2.dtnm_text
                        left join rfl.t_dtdm ds2 on na.district2= ds2.dtdm_text
                        where na.appl_nid=(select appl_nid from nif_dist where dist_id='{distId}')";
            var _appl = DatabaseOracleClient.SqlToListObjectBind<NIF_APPL>(sql);
            if (_appl.Item2.ROWS == 1)
            {
                obj.NIF_APPL = _appl.Item1.First();

                sql = $@"select na.nomi_id,na.appl_nid,na.nomi_nid,na.nid_img,na.nom_img,
                        na.full_name,na.birth_date,na.mobile_number,
                        na.email_address,na.father_name,na.mother_name,na.parents_mobile,na.spouse_name,na.spouse_mobile,
                        na.house_road,na.village_name,na.union_name,na.police_station,na.district,
                        na.relation_appl,na.contribution
                        from nif_nomi na where na.APPL_NID='{obj.NIF_APPL.APPL_NID}' order by na.nomi_id";
                var _nom = DatabaseOracleClient.SqlToListObjectBind<NIF_NOMI>(sql);
                if (_appl.Item2.ROWS == 1)
                {
                    obj.NIF_NOMI = _nom.Item1;
                }
                else
                {
                    obj.NIF_NOMI = new List<NIF_NOMI>();
                }
            }
            else
            {
                obj = new NIF_APPL_VIEW();
            }
            return obj;
        }


        public static EQResult UpdatePicture(NIF_IMGS obj, string distId)
        {

            List<string> sqlList = new List<string>();
            string sql = "";
            if (obj.ITEM_IMAGE_TYPE == 1)
            {
                sql = $@"UPDATE RPGL.NIF_APPL SET NID_IMG='Y' WHERE APPL_NID='{obj.APPL_NID}'";
            }
            else if (obj.ITEM_IMAGE_TYPE == 2)
            {
                sql = $@"UPDATE RPGL.NIF_APPL SET APPL_IMG='Y' WHERE APPL_NID='{obj.APPL_NID}'";
            }
            else if (obj.ITEM_IMAGE_TYPE == 3)
            {
                sql = $@"UPDATE RPGL.NIF_NOMI SET NID_IMG='Y' WHERE IS_ACTIVE=1 AND NOMI_ID=1 AND APPL_NID=(SELECT APPL_NID FROM RPGL.NIF_APPL WHERE APPL_NID='{obj.APPL_NID}')";
            }
            else if (obj.ITEM_IMAGE_TYPE == 4)
            {
                sql = $@"UPDATE RPGL.NIF_NOMI SET NOM_IMG='Y' WHERE IS_ACTIVE=1 AND NOMI_ID=1 AND APPL_NID=(SELECT APPL_NID FROM RPGL.NIF_APPL WHERE APPL_NID='{obj.APPL_NID}')";
            }
            else if (obj.ITEM_IMAGE_TYPE == 5)
            {
                sql = $@"UPDATE RPGL.NIF_NOMI SET NID_IMG='Y' WHERE IS_ACTIVE=1 AND NOMI_ID=2 AND APPL_NID=(SELECT APPL_NID FROM RPGL.NIF_APPL WHERE APPL_NID='{obj.APPL_NID}')";
            }
            else if (obj.ITEM_IMAGE_TYPE == 6)
            {
                sql = $@"UPDATE RPGL.NIF_NOMI SET NOM_IMG='Y' WHERE IS_ACTIVE=1 AND NOMI_ID=2 AND APPL_NID=(SELECT APPL_NID FROM RPGL.NIF_APPL WHERE APPL_NID='{obj.APPL_NID}')";
            }
            sqlList.Add(sql);

            return DatabaseOracleClient.PostSqlList(sqlList);
        }


        public static Tuple<List<NIF_NOMI>, EQResult> getNomineeNumber(string distId)
        {
            string sql = $@"select nomi_id
                            from nif_nomi where appl_nid 
                            in (
                            select appl_nid
                            from nif_dist where dist_id='{distId}')";
            return DatabaseOracleClient.SqlToListObjectBind<NIF_NOMI>(sql);
        }

        public static EQResultTable ViewAllAppl()
        {
            //string sql = $@"select na.appl_nid master_id,na.full_name applicant_name,na.birth_date dob,na.mobile_number mobile,na.email_address email,na.father_name,na.mother_name,na.parents_mobile,na.spouse_name,na.spouse_mobile,
            //            na.house_road present_road,na.village_name present_village,na.union_name present_union,na.police_station present_ps,na.district present_district,
            //            na.house_road2 permanent_road,na.village_name2 permanent_village,na.union_name2 permanent_union,na.police_station2 permanent_ps,na.district2 permanent_district,
            //            na.account_name,na.account_no,na.bank_name,na.branch_name,na.appl_date application_date,
            //            na.dist_tin tin_no,na.dist_bin bin_no,na.dist_trade trade_lic_no
            //            from nif_appl na order by na.appl_nid";
            string sql = @"select to_char(a.appl_nid) master_id,a.full_name owner_name,u1.union_name,t1.dtnm_name thana,d1.dtdm_name district,to_char(a.mobile_number)mobile,
                to_char(ad.dist_id) id,ad.dist_name,ag.digr_name group_name,adm.addr2 ||', '|| adm.addr3 address,
                adz.sales_zone_id zone_id,adz.sales_zone zone,at1.dtnm_text thana_id,at1.dtnm_name thana_name,to_char(adc.contacts) dist_contact
                from nif_appl a
                join rfl.t_dithun u1 on a.union_name=u1.union_text
                join rfl.t_dtnm t1 on a.police_station=t1.dtnm_text
                join rfl.t_dtdm d1 on a.district=d1.dtdm_text
                join nif_dist ad on a.appl_nid=ad.appl_nid
                join rfl.t_digr ag on ad.digr_text=ag.digr_text
                join rfl.distributor_master adm on ad.dist_id=adm.dist_id
                join rfl.sales_zone adz on adm.sales_zone_id=adz.sales_zone_id
                join rfl.t_dtnm at1 on adm.domo_dtnm=at1.oid
                join rfl.distributor_contacts adc on adm.dist_id=adc.dist_id and adc.seqn=101";
            return DatabaseOracleClient.GetDataTable(sql);
        }
    }
}