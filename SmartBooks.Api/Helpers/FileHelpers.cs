using ClosedXML.Excel;
using SmartBooks.Api.Helpers.Models;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SmartBooks.Api.Helpers
{
    public static class FileHelpers
    {
        public static byte[] ExportTabularDataToExcel<T>(
            List<T> list,
            List<ColumnModel> headers,
            string sheetName = "Sheet1")
            where T : class
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);
            int startRow = 1;
            int currentRow = startRow;
            int currentCol = 1;

            PropertyInfo[] properties = typeof(T).GetProperties();
            Dictionary<string, PropertyInfo> dict = 
                new Dictionary<string, PropertyInfo>(properties.Length);
            foreach (var prop in properties)
            {
                dict.Add(prop.Name, prop);
            }
            foreach (ColumnModel column in headers)
            {
                worksheet.Cell(currentRow, currentCol).Value = column.Title;
                if (!dict.ContainsKey(column.ColumnName)) continue;

                PropertyInfo propertyInfo = dict[column.ColumnName];

                foreach (T obj in list)
                {
                    currentRow++;
                    object propValue = propertyInfo.GetValue(obj);
                    worksheet.Cell(currentRow, currentCol).SetValue(propValue);
                }
                currentRow = startRow;
                currentCol++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

    }
}
