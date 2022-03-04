using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PriOrder.App.DataModels
{
    public static class ApplData
    {
        public  static int CHACHE_TIME = Convert.ToInt32(ConfigurationManager.AppSettings["cacheTimeMinute"]);
    }
}