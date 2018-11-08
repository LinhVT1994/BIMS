using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using static BIMS.Attributes.AutoIncrementAttribute;
using static BIMS.Attributes.UniqueAttribute;
using static BIMS.Attributes.ExcelColumnAttribute;
using static BIMS.Attributes.ForeignKeyAttribute;
using BIMS.Attributes;
using System.Reflection;
using System.Diagnostics;

namespace BIMS.Utilities
{
    /**
    * A ExcelReader object what contains methods to read data from an excel file;
    * 
    *
    * @author  LinhVT
    * @version 1.0
    * @since   2018/11/6
    */
    class ExcelDataAccess
    {
        private Excel.Application xlApplication = null;
        private Excel.Worksheet xlworkSheet = null;
        private Excel.Workbook xlWorkBook = null;
        private static ExcelDataAccess excelReader = null;

        private ExcelDataAccess()
        {
        }
        public static ExcelDataAccess GetInstance()
        {
            if (excelReader == null)
            {
                excelReader = new ExcelDataAccess();
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
            List<string> properties = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
            if (properties.Count == 0)
            {
                throw new Exception("Dont have any required property.");
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

            xlApplication = new Excel.Application();
            xlApplication.Visible = false;
            xlApplication.DisplayAlerts = false;

            xlWorkBook = xlApplication.Workbooks.Open(url);

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
                            string returnedValue = GetValueFromARow(columnMap, i, property, out rowName);
                            key = returnedValue;
                            if (string.IsNullOrWhiteSpace(returnedValue))
                            {
                                string message = string.Format("Error at: Cell[{0},{1}] Handled: {2} Message: {3}", i, rowName, "Ignore", "Can't get value on this cell.");
                                LoggingHelper.WriteDown(message);
                                break;
                            }
                            else
                            {
                                propertyInfo.SetValue(x, returnedValue);
                            }
                        }
                        else
                        {
                            xlWorkBook.Close();
                            xlApplication.Quit();
                            throw new Exception("Can't get data from the excel file: " + property);
                        }
                    }
                    else if (IsForeignKey(typeof(T), property))// get the id  from server sql.
                    {
                        Dictionary<string,string> excelColumnMapping=  GetExcelColumnReferences(typeof(T), property);
                        string table= GetRefTable(typeof(T), property);
                        string foreignKey = GetRefId(typeof(T), property);
                        Debug.WriteLine(string.Format("{0}  {1}  {2}", table, foreignKey, excelColumnMapping.ToString()));
                        //　get the table and property what referenced to.
                    }
                    else
                    {
                        if (columnMap.ContainsKey(property)) // if this property has value will get from in Excel file.
                        {
                            string columnName = null;
                            string returnedValue = GetValueFromARow(columnMap, i, property,out columnName);
                            if (!string.IsNullOrWhiteSpace(returnedValue))
                            {
                                propertyInfo.SetValue(x, returnedValue);
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
                // if the key has not setted any value then ignore it.
                if (string.IsNullOrWhiteSpace(key))
                {
                    //
                    continue;
                }
                else
                {
                    if (!dicResult.ContainsKey(key))
                    {
                        dicResult.Add(key, x);
                    }
                }
            }
            
            // close the excel app.
            xlWorkBook.Close();
            xlApplication.Quit();
            return dicResult;
        }
        private string GetValueFromARow(Dictionary<string, string> columnMap, int row,string property, out string columnName)
        {
            if (columnMap.TryGetValue(property, out columnName))
            {
                string s = null;
                try
                {
                    Excel.Range cell = xlworkSheet.Cells[row, columnName];
                    if (cell.Value != null)
                    {
                        s = xlworkSheet.Cells[row, columnName].Value.ToString();
                        return s;
                    }
                }
                catch(Exception)
                {
                    return null;
                }
            }
            return null;
        } 

    }
}