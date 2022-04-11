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
    }
}