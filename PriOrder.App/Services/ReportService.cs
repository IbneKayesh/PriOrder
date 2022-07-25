using Aio.Db.Client.Entrance;
using Aio.Model;
using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Services
{
    public class ReportService
    {
        public static Tuple<List<T_UNDELIVERED>, EQResult> getUndelivered(string distId)
        {
            var procData = SpService.PRO_WO_GET_ALL("GET_UNDELIVERED", distId);
            EQResult rslt = new EQResult();
            rslt.SUCCESS = false;
            rslt.ROWS = 0;
            if (procData.Result.SUCCESS && procData.Result.ROWS == 1)
            {
                var objList = DatabaseOracleClient.DataTableToListObjectBind<T_UNDELIVERED>(procData.Set.Tables[0]);
                if (objList.Count > 0)
                {
                    rslt.SUCCESS = true;
                    rslt.ROWS = objList.Count;
                    return new Tuple<List<T_UNDELIVERED>, EQResult>(objList, rslt);
                }
            }
            return new Tuple<List<T_UNDELIVERED>, EQResult>(new List<T_UNDELIVERED>(), rslt);
        }

    }
}