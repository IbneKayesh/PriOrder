using Aio.Db.Client.Entrance;
using PriOrder.App.ModelsView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Services
{
    public class LoginService
    {
        public static Tuple<List<USER_LOGIN_INFO>, string> getDistInfo(string distId, string passWord)
        {
            string sql = $@"select usr.a dist_id,usr.b dist_name,usr.d dist_group,'0' dist_mobile,'0' dist_balance from v_user usr where usr.a='{distId}' and usr.e='{passWord}'";
            return DatabaseOracleClient.SqlToListObjectBind<USER_LOGIN_INFO>(sql);
        }

    }
}