using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Aio.Db.OracleEF
{
    public static class TableEntity
    {
        #region Aio_EF
        /// <summary>
        ///  Bind to Object List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> BindObjectList<T>(DataTable dataTable) where T : new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in dataTable.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
        public static List<T> DataTableToListModel<T>(DataTable _dt) where T : new()
        {
            // define return list
            List<T> lst = new List<T>();
            if (_dt != null)
            {

                // go through each row
                foreach (DataRow r in _dt.Rows)
                {
                    // add to the list
                    lst.Add(CreateItemFromRow<T>(r));
                }
            }
            // return the list
            return lst;
        }
        private static T CreateItemFromRow<T>(DataRow _r) where T : new()
        {
            // create a new object
            T i = new T();
            // set the item
            SetItemFromRow(i, _r);
            // return 
            return i;
        }
        private static void SetItemFromRow<T>(T _i, DataRow _r) where T : new()
        {
            // go through each column
            foreach (DataColumn c in _r.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = _i.GetType().GetProperty(c.ColumnName);
                // if exists, set the value
                if (p != null && _r[c] != DBNull.Value)
                {
                    p.SetValue(_i, _r[c], null);
                }
            }
        }

        public static string ModelToInsertQuery<T>(T obj)
        {
            StringBuilder sbValues = new StringBuilder();
            StringBuilder sbColumns = new StringBuilder();
            string[] arrColumns;


            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);


            string qryTmp = "Insert into {0} ({1}) values({2})";
            string tableName = entityType.Name;

            foreach (PropertyDescriptor prop in properties)
            {


                dynamic val;

                if (prop.PropertyType.Name == "DateTime")
                {
                    var value = prop.GetValue(obj);
                    if (value != null)
                    {
                        var date = (DateTime?)value;
                        if (date == DateTime.MinValue)
                        {
                            date = null;
                        }
                        else
                        {
                            date = DateTime.ParseExact(String.Format("{0:MM/dd/yyyy HH:mm:ss tt}", date), "MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture); //String.Format("{0:MM/dd/yyyy}", date); 
                        }
                        sbValues.Append("TO_DATE('");
                        sbValues.Append(date);
                        sbValues.Append("','DD-MM-RR HH:MI:SS AM')");
                        sbValues.Append(",");

                        sbColumns.Append(prop.Name);
                        sbColumns.Append(",");


                    }
                }
                else if (prop.PropertyType.Name == "Int32")
                {
                    val = prop.GetValue(obj);
                    if (val != null)
                    {
                        if (val > 0)
                        {
                            //sbValues.Append("'");
                            sbValues.Append("q'[" + val + "]'");
                            //sbValues.Append("'");
                            sbValues.Append(",");
                            sbColumns.Append(prop.Name);
                            sbColumns.Append(",");
                        }
                    }
                }
                else
                {

                    val = prop.GetValue(obj);
                    if (val != null)
                    {

                        //sbValues.Append("'");
                        sbValues.Append("q'[" + val + "]'");
                        //sbValues.Append("'");
                        sbValues.Append(",");

                        sbColumns.Append(prop.Name);
                        sbColumns.Append(",");


                    }
                }
            }
            sbColumns.Remove(sbColumns.Length - 1, 1);
            sbValues.Remove(sbValues.Length - 1, 1);

            var rvStr = string.Format(qryTmp, tableName, sbColumns.ToString(), sbValues.ToString());
            return rvStr;
        }
        public static string ModelToInsertQuery<T>(T obj, string columns)
        {
            StringBuilder sbValues = new StringBuilder();
            StringBuilder sbColumns = new StringBuilder();
            string[] arrColumns;
            if (string.IsNullOrEmpty(columns))
            {
                arrColumns = new string[] { };
            }
            else
            {
                arrColumns = columns.Split(',');
            }

            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);


            string qryTmp = "Insert into {0} ({1}) values({2})";
            string tableName = entityType.Name;

            foreach (PropertyDescriptor prop in properties)
            {
                string columnName = "";
                if (arrColumns.Length > 0)
                {
                    columnName = arrColumns.FirstOrDefault(s => s.ToLower().Equals(prop.Name.ToLower()));
                }

                dynamic val;
                if (!string.IsNullOrEmpty(columnName))
                {
                    if (prop.PropertyType.Name == "DateTime")
                    {
                        var value = prop.GetValue(obj);
                        if (value != null)
                        {
                            var date = (DateTime?)value;
                            if (date == DateTime.MinValue)
                            {
                                date = null;
                            }
                            else
                            {
                                date = DateTime.ParseExact(String.Format("{0:MM/dd/yyyy HH:mm:ss tt}", date), "MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture); //String.Format("{0:MM/dd/yyyy}", date); 
                            }
                            sbValues.Append("TO_DATE('");
                            sbValues.Append(date);
                            sbValues.Append("','DD-MM-RR HH:MI:SS AM')");
                            sbValues.Append(",");

                            sbColumns.Append(prop.Name);
                            sbColumns.Append(",");
                        }
                    }
                    else
                    {

                        val = prop.GetValue(obj);
                        if (val != null)
                        {
                            //sbValues.Append("'");
                            sbValues.Append("q'[" + val + "]'");
                            //sbValues.Append("'");
                            sbValues.Append(",");
                            sbValues.Append(",");

                            sbColumns.Append(prop.Name);
                            sbColumns.Append(",");

                        }

                    }

                }



            }
            sbColumns.Remove(sbColumns.Length - 1, 1);
            sbValues.Remove(sbValues.Length - 1, 1);

            var rvStr = string.Format(qryTmp, tableName, sbColumns.ToString(), sbValues.ToString());
            return rvStr;

        }
        public static string ModelToUpdateQuery<T>(T obj, string columns, string condition)
        {
            var rvStr = "";
            StringBuilder sbValues = new StringBuilder();

            string[] arrColumns;
            if (string.IsNullOrEmpty(columns))
            {
                arrColumns = new string[] { };
            }
            else
            {
                arrColumns = columns.Split(',');
            }

            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);


            string qryTmp = "Update {0} {1} Where {2}";//0 Table,1 set value 2 condition
            string tableName = entityType.Name;
            sbValues.Append(" Set ");
            foreach (PropertyDescriptor prop in properties)
            {
                string columnName = "";
                if (arrColumns.Length > 0)
                {
                    columnName = arrColumns.FirstOrDefault(s => s.ToLower().Equals(prop.Name.ToLower()));
                }

                dynamic val;
                if (!string.IsNullOrEmpty(columnName))
                {
                    if (prop.PropertyType.Name == "DateTime")
                    {
                        var value = prop.GetValue(obj);
                        if (value != null)
                        {

                            var date = (DateTime?)value;
                            if (date == DateTime.MinValue)
                            {
                                date = null;
                            }
                            sbValues.Append(prop.Name);
                            sbValues.Append("=");
                            sbValues.Append("TO_DATE('");
                            sbValues.Append(date);
                            sbValues.Append("','DD-MM-RR HH:MI:SS AM')");
                            sbValues.Append(",");
                        }
                    }
                    else
                    {

                        val = prop.GetValue(obj);
                        if (val != null)
                        {
                            sbValues.Append(prop.Name);
                            sbValues.Append("=");
                            //sbValues.Append("'");
                            sbValues.Append("q'[" + val + "]'");
                            //sbValues.Append("'");
                            sbValues.Append(",");
                            sbValues.Append(",");
                        }

                    }
                }

            }

            sbValues.Remove(sbValues.Length - 1, 1);

            rvStr = string.Format(qryTmp, tableName, sbValues.ToString(), condition);
            //}
            return rvStr;

        }
        #endregion
        public static DataTable CreateDataTableFromListClass<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }
            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
