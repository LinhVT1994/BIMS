using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using static BIMS.Attributes.AutoIncrementAttribute;
using static BIMS.Attributes.UniqueAttribute;
using static BIMS.Attributes.ExcelColumnAttribute;
using BIMS.Attributes;
using System.Reflection;

namespace BIMS.Utilities
{
    class ExcelReader
    {
        private static ExcelReader excelReader = null;

        private ExcelReader()
        {
        }
        public static ExcelReader GetInstance()
        {
            if (excelReader == null)
            {
                excelReader = new ExcelReader();
            }
            return excelReader;
        }
        /// <summary>
        /// read from a excel file.
        /// </summary>
        /// <returns></returns>
        public bool Read(string url)
        {
            Excel.Application xlApplication = new Excel.Application();
            try
            {
                Excel.Workbook xlWorkBook = xlApplication.Workbooks.Open(url);
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        public Dictionary<string, T> Read<T>(string url)
        {
            Dictionary<string, T> dicResult = new Dictionary<string, T>();
            Type type = typeof(T);
            // the properties what need to get value from a excel file.
            List<string> properties = RequiredAttribute.GetRequiredProperties(typeof(T));
            if (properties.Count == 0)
            {
                return null;
            }
            // a mapping the name of a property and a name of column in a excel file.
            Dictionary<string, string> columnMap = ColumnNamesMapping(typeof(T));
            if (columnMap.Count == 0)
            {
                return null;
            }
            // the list of properties what required to have to has a value is unique. 
            List<string> uniqueProperties = GetUniqueProperties(typeof(T));
            // the list of properties what will be increated automaticlly.
            List<string> autoIncreateProperties = GetAutoIncrementProperties(typeof(T));

            Excel.Application xlApplication = new Excel.Application();
            Excel.Workbook xlWorkBook = null;
            xlApplication.Visible = false;
            xlApplication.DisplayAlerts = false;
            xlWorkBook = xlApplication.Workbooks.Open(url);
            Excel.Worksheet xlworkSheet = null;
            xlworkSheet = (Excel.Worksheet)xlWorkBook.Sheets[1];
            xlworkSheet.Unprotect();
            int id = 1;
            int startIndex = 5;
            Excel.Range xlRange = xlworkSheet.UsedRange;
            int numbOfRows = xlRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row;
            int numbOfColumns = xlRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Column;

            for (int i = startIndex; i < numbOfRows; i++)
            {
                T x = (T)Activator.CreateInstance(typeof(T));
                string key = null;
                foreach (string property in properties) // properties are 
                {
                    PropertyInfo propertyInfo = x.GetType().GetProperty(property);
                    if (IsAutoIncrement(typeof(T), property))
                    {
                        propertyInfo.SetValue(x, dicResult.Count + 1);
                    }
                    else if (IsUnique(typeof(T), property))
                    {
                        // read position in excel
                        if (columnMap.ContainsKey(property))
                        {
                            string rowName = null;
                            if (columnMap.TryGetValue(property, out rowName))
                            {
                                string s = xlworkSheet.Cells[i, rowName];
                                propertyInfo.SetValue(x, s);
                                key = s;
                            }
                            else
                            {
                                throw new Exception("Cant get the name of row: " + property);
                            }
                        }
                        else
                        {
                            throw new Exception("Can't get data from the excel file: " + property);
                        }
                    }
                    else
                    {
                        if (columnMap.ContainsKey(property)) // if this property has value will get from in Excel file.
                        {
                            string rowName = null;
                            if (columnMap.TryGetValue(property, out rowName))
                            {
                                string s = xlworkSheet.Cells[i, rowName];
                                propertyInfo.SetValue(x, s);
                            }
                            else
                            {
                                propertyInfo.SetValue(x, null);
                            }
                        }
                        else
                        {
                            propertyInfo.SetValue(x, null);
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(key))
                {
                    new Exception("Key is null : " + key);
                }
                else
                {
                    dicResult.Add(key, x);
                }
            }
            return dicResult;
        }

    }
}