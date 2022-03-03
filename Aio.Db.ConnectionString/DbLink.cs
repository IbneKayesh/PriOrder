using System.Configuration;

namespace Aio.Db.ConnectionString
{
    public static class DbLink
    {
        public static string GET(string _c)
        {
            return ConfigurationManager.ConnectionStrings[_c].ConnectionString;
        }
    }
}
