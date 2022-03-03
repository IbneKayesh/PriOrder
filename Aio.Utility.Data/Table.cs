using Aio.Model;
using System;
using System.Data;

namespace Aio.Utility.Data
{
    public static class Table
    {

        public static Tuple<DataTable, string> Filter(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                if (dataTable.Columns[0].ColumnName == AppKeys.GetError)
                {
                    return new Tuple<DataTable, string>(new DataTable(), dataTable.Rows[0][0].ToString());
                }
            }
            else
            {
                return new Tuple<DataTable, string>(new DataTable(), AppKeys.NoRowsFound);
            }
            return new Tuple<DataTable, string>(dataTable, AppKeys.PostSuccess);
        }
    }
}
