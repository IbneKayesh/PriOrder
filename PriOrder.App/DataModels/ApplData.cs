using System;
using System.Configuration;

namespace PriOrder.App.DataModels
{
    public static class ApplData
    {
        public static bool CHACHE_ENABLED = Convert.ToBoolean(ConfigurationManager.AppSettings?["cacheEnabled"]);
        public static int CHACHE_TIME = Convert.ToInt32(ConfigurationManager.AppSettings?["cacheTimeMinute"]);
        public const int CAT_IMG_SIZE = 51200; //1024 * 50 KB
        public const int CLS_IMG_SIZE = 51200; //1024 * 50 KB
        public const int ITM_IMG_SIZE = 51200; //1024 * 50 KB

        public const int PRO_IMG_SIZE = 512000; //1024 * 500 KB

        public const int NF_IMG_SIZE = 5242880; //1024 * 10 MB

        public const string CART_COUNT_CACHE = "ChCartItemCount";
    }
}