using Aio.Db.Client.Entrance;
using Aio.Model;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
using System;
using System.Collections.Generic;

namespace PriOrder.App.Services
{
    public class LoginService
    {
        public static Tuple<List<USER_LOGIN_INFO>, EQResult> getDistInfo(string distId, string passWord)
        {
            string sql = $@"select usr.a dist_id,usr.b dist_name,usr.d dist_group,'0' dist_mobile,'0' dist_balance from v_user usr where usr.a='{distId}' and usr.e='{passWord}'";
            return DatabaseOracleClient.SqlToListObjectBind<USER_LOGIN_INFO>(sql);
        }

        public static Tuple<List<WO_APP_MENU>, EQResult> getMenusByUserId(string _userId)
        {
            string sql= @"SELECT M.MENU_ID MODULE_ID,M.MENU_NAME_EN MODULE_NAME_EN,M.MENU_NAME_BN MODULE_NAME_BN,M.MENU_ICON MODULE_ICON,M.VIEW_ORDER MODULE_ORDER,PA.MENU_ID PARENT_ID,PA.MENU_NAME_EN PARENT_NAME_EN,PA.MENU_NAME_BN PARENT_NAME_BN,PA.MENU_ICON PARENT_ICON,PA.VIEW_ORDER PARENT_ORDER,ME.MENU_ID,ME.MENU_NAME_EN,ME.MENU_NAME_BN,ME.MENU_ICON,ME.AREA_NAME,ME.CONTROLLER_NAME,ME.ACTION_NAME,ME.VIEW_ORDER MENU_ORDER
                FROM WO_APP_MENU M
                JOIN WO_APP_MENU PA ON PA.PARENT_ID=M.MENU_ID AND PA.IS_ACTIVE=1 AND PA.PARENT_ID!=0 AND PA.CONTROLLER_NAME='PARENT'
                JOIN WO_APP_MENU ME ON PA.MENU_ID=ME.PARENT_ID AND ME.IS_ACTIVE=1 
                WHERE M.IS_ACTIVE=1 AND M.PARENT_ID=0 AND M.CONTROLLER_NAME='MODULE'";

            //--JOIN ZTN_USER_MENU UP ON ME.MENU_ID=UP.CHILD_MENU_ID AND UP.IS_ACTIVE=1 AND UP.USER_ID='" + _userId + "'
            return DatabaseOracleClient.SqlToListObjectBind<WO_APP_MENU>(sql);
        }

    }
}