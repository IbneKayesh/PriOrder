using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PriOrder.App.Utility
{
    public class ExportExcelFileClosedXML
    {
        public static void GenerateExcel(DataTable dataTable, string path)
        {
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dataTable, "Sheet1");
            //wb.SaveAs(path);
        }
    }
}