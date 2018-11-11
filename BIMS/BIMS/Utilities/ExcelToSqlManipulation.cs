using BIMS.Attributes;
using BIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using static BIMS.Attributes.AutoIncrementAttribute;
using static BIMS.Attributes.UniqueAttribute;
using static BIMS.Attributes.ExcelColumnAttribute;
using static BIMS.Attributes.DistinguishAttribute;
using static BIMS.Attributes.ForeignKeyAttribute;
using static BIMS.Attributes.PrimaryKeyAttribute;
using System.Diagnostics;
using System.Data.SqlClient;
using static BIMS.Attributes.PropertyInfoExtensions;
namespace BIMS.Utilities
{

    class ExcelToSqlManipulation
    {

       // private static string url = @"C:\Users\TUAN-LINH\Desktop\TestData.xlsx";
        private static string url = @"C:\Users\VuLin\Desktop\TestData.xlsx";
        public static DataSet GetForeignKeyInSQL(string idRef, string tableRef, List<SqlParameter> sqlParams)
        {
            // select * from ? where  abc = csss;
            StringBuilder sqlQuery = new StringBuilder();
            string sParam = null;

            if (sqlParams.Count <= 0)
            {
                return null;
            }
            else
            {
                foreach (SqlParameter para in sqlParams)
                {
                    sParam += para.ParameterName + "=@" + para.ParameterName + ",";
                }
                sParam = sParam.Remove(sParam.Length-1);
                sqlQuery.AppendFormat("select * from {1} where {2}", idRef, tableRef, sParam);
                SqlDataAccess sqlDataAccess = new SqlDataAccess();
                DataTable resultsOfSelecting = sqlDataAccess.ExecuteSelectQuery(sqlQuery.ToString(), sqlParams.ToArray());
                if (resultsOfSelecting.Count <= 0)
                {
                    return null;
                }
                else
                {
                    DataSet data =  resultsOfSelecting.GetElementAt(0);
                    string  result=  data.Value(idRef);
                    return data;
                }
            }
        }
        public static void Display<T>(List<PropertyInfo> list)
        {
            Debug.WriteLine(">" + typeof(T).Name);
            foreach (var item in list)
            {
                Debug.WriteLine(">>>"+item.Name);
            }
        }
        public static void CreateQuery<T>()
        {

        }
        public static bool Execute<T>()
        {
            //get the foreignkey what wants to search.
           // List<PropertyInfo>  list =   DetectRelationships.GetRelationships(typeof(T), "Construction");
           // CreateQuery<T>();
           // Display<T>(list);
           // return false;

            Excel.Application xlApplication = null;
            Excel.Worksheet xlworkSheet = null;
            Excel.Workbook xlWorkBook = null;
            Dictionary<string, T> dicResult = new Dictionary<string, T>();
            Type type = typeof(T);
            // the properties what need to get value from a excel file.
            List<string> properties = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
            if (properties.Count == 0)
            {
                throw new Exception("Dont have any required property.");
            }
            // a mapping the name of a property and a name of column in a excel file.
            Dictionary<string, string> columnMap = ColumnNamesMapping(typeof(T));
            if (columnMap.Count == 0)
            {
                return false;
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
            bool hasALeastOneUnique = true;
            if (UniqueAttribute.GetUniqueProperties(typeof(T)).Count <= 0)
            {
                hasALeastOneUnique = false;
            }
            for (int i = startIndex; i < numbOfRows; i++)
            {
                T newObj = (T)Activator.CreateInstance(typeof(T));
                string key = null;
                foreach (string property in properties) // properties are 
                {
                    PropertyInfo propertyInfo = newObj.GetType().GetProperty(property);
                    if (IsAutoIncrement(typeof(T), property))
                    {
                        if(!hasALeastOneUnique)
                        {
                            key = (dicResult.Count + 1).ToString();
                        }
                        propertyInfo.SetValue(newObj, dicResult.Count + 1);
                    }
                    else if (IsUnique(typeof(T), property))
                    {
                        // read position in excel
                        if (columnMap.ContainsKey(property))
                        {

                            string rowName = null;
                            string returnedValue = GetValueFromARow(xlworkSheet,columnMap, i, property, out rowName);
                            key = returnedValue;
                            if (string.IsNullOrWhiteSpace(returnedValue))
                            {
                                string message = string.Format("Error at: Cell[{0},{1}] Handled: {2} Message: {3}", i, rowName, "Ignore", "Can't get value on this cell.");
                                LoggingHelper.WriteDown(message);
                                break;
                            }
                            else
                            {
                                propertyInfo.SetValue(newObj, returnedValue);
                              
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

                        //　get the table and property what referenced to.
                        if (!HandleForeignKey<T>(newObj, property, xlworkSheet, i)) 
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (columnMap.ContainsKey(property)) // if this property has value will get from in Excel file.
                        {
                            string columnName = null;
                            string returnedValue = GetValueFromARow(xlworkSheet,columnMap, i, property, out columnName);
                            if (!string.IsNullOrWhiteSpace(returnedValue))
                            {
                                propertyInfo.SetValueByDataType(newObj, returnedValue);
                            }
                            else
                            {
                                propertyInfo.SetValueByDataType(newObj, null);
                            }
                        }
                        else
                        {
                            propertyInfo.SetValueByDataType(newObj, null);
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
                        dicResult.Add(key, newObj);
                        RequestToSql<T>(newObj);
                    }
                }
            }
            // close the excel app.
            xlWorkBook.Close();
            xlApplication.Quit();
            return true;
        }
        private static bool HandleForeignKey<T>(T newObj, string property , Excel.Worksheet xlworkSheet, int row)
        {
            PropertyInfo propertyInfo = newObj.GetType().GetProperty(property);
            Dictionary<string, string> excelColumnReferences = GetExcelColumnReferences(typeof(T), propertyInfo.Name);
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (var item in excelColumnReferences)
            {
                string propertyInSql = item.Key;
                string propertyInExcel = item.Value;
                if (propertyInSql.Equals("*") || propertyInExcel.Equals("*"))
                {
                    // 
                    Dictionary<string, string> dicConditions = GetDistinguishConditions(typeof(T), property);
                    foreach (var pair in dicConditions)
                    {
                        var valueInCell = GetValueInCell(xlworkSheet, row, pair.Value);
                        parameters.Add(new SqlParameter(pair.Key, valueInCell));
                    }
                }
                else
                {
                    string valueInCell = GetValueInCell(xlworkSheet, row, propertyInExcel);
                    if (string.IsNullOrWhiteSpace(valueInCell))
                    {
                        string message = string.Format("Error at: Cell[{0},{1}] Handled: {2} Message: {3}", row, propertyInExcel, "Ignore", "Can't get value on this cell.");
                        LoggingHelper.WriteDown(message);
                        break;
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(propertyInSql, valueInCell));
                    }
                }
            }
            string refId = GetRefId(typeof(T), propertyInfo.Name);
            string tableName = GetRefTable(typeof(T), propertyInfo.Name);
            DataSet dataSetResults = GetForeignKeyInSQL(refId, tableName, parameters);
            if (dataSetResults == null)
            {
                string message = string.Format("Not exist in SQL");
                LoggingHelper.WriteDown(message);
                return false;
            }
            else
            {
                object anonymous = Utility.ParseDataWith(propertyInfo.PropertyType, dataSetResults);
                propertyInfo.SetValueByDataType(newObj, anonymous);
                return true;
            }
        }
        private static object RequestToSql<T>(T parseTo)
        {
            List<string> requiredProperties = RequiredAttribute.GetRequiredPropertiesName(parseTo.GetType());
            string table = typeof(T).GetAttributeValue((SqlParameterAttribute dna) => dna.PropertyName);
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (string property in requiredProperties)
            {
               string paramName = SqlParameterAttribute.GetNameOfParameterInSql(parseTo.GetType(), property);
               PropertyInfo propertyInfo = parseTo.GetType().GetProperty(property);
               object result = propertyInfo.GetValue(parseTo);
               if (result!=null)
               {
                    object paramValue = propertyInfo.GetValue(parseTo);
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        parameters.Add(new SqlParameter(paramName, paramValue));
                    }
                    else if (propertyInfo.PropertyType == typeof(int))
                    {
                        parameters.Add(new SqlParameter(paramName, paramValue));
                    }
                    else if (propertyInfo.PropertyType == typeof(double))
                    {
                        parameters.Add(new SqlParameter(paramName, paramValue));
                    }
                    else if (propertyInfo.PropertyType.BaseType == typeof(Element))
                    {
                        string refId = ForeignKeyAttribute.GetRefId(typeof(T), property);
                        object data = GetPrimaryKeyValue(paramValue);
                        parameters.Add(new SqlParameter(paramName, data));
                    }
                    else 
                    {
                        throw new Exception("Code hasnot implement");
                    }
               }
            }
            CreateInsertQuery(table, parameters);
            return null;
        }
        public SqlParameter SetValueForParameter(SqlParameter parameter, PropertyInfo propertyInfo,Object obj,string paramName)
        {
            object result = propertyInfo.GetValue(obj);
            if (result != null)
            {
                string paramValue = propertyInfo.GetValue(obj).ToString();
                if (propertyInfo.PropertyType == typeof(string))
                {
                    return new SqlParameter(paramName, paramValue);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    return new SqlParameter(paramName, int.Parse(paramValue));
                }
                else if (propertyInfo.PropertyType == typeof(double))
                {
                    double db;
                    double.TryParse(paramValue, out db);
                    return new SqlParameter(paramName, db);
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        public static int CreateInsertQuery(string table, List<SqlParameter> sqlParams)
        {
            if (sqlParams.Count <= 0)
            {
                return -1;
            }
            string sPropertyNames = "(";
            foreach (SqlParameter para in sqlParams)
            {
                sPropertyNames += "" + para.ParameterName + ",";
            }
            sPropertyNames = sPropertyNames.Remove(sPropertyNames.Length - 1);
            sPropertyNames += ")";

            string sValues = null;
            foreach (SqlParameter para in sqlParams)
            {
                sValues += "@" + para.ParameterName + ",";
            }
            sValues = sValues.Remove(sValues.Length - 1);

            StringBuilder sqlQuery = new StringBuilder();
 
            sqlQuery.AppendFormat("insert into {0}{1} values({2})", table, sPropertyNames, sValues);

            SqlDataAccess sqlDataAccess = new SqlDataAccess();
            return sqlDataAccess.ExecuteInsertOrUpdateQuery(sqlQuery.ToString(), sqlParams.ToArray());
        }
        private static string GetValueFromARow(Excel.Worksheet xlworkSheet, Dictionary<string, string> columnMap, int row, string property, out string columnName)
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
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }
        public static string GetValueInCell(Excel.Worksheet xlworkSheet, int row , string columnName)
        {
            try
            {
                Excel.Range cell = xlworkSheet.Cells[row, columnName];
                if (cell.Value != null)
                {
                    return xlworkSheet.Cells[row, columnName].Value.ToString();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool ExecuteMultiRecords<T>()
        {
            Excel.Application xlApplication = null;
            Excel.Worksheet xlworkSheet = null;
            Excel.Workbook xlWorkBook = null;
            Dictionary<string, T> dicResult = new Dictionary<string, T>();
            // the list of properties what will be increated automaticlly.
            xlApplication = new Excel.Application();
            xlApplication.Visible = false;
            xlApplication.DisplayAlerts = false;
            xlWorkBook = xlApplication.Workbooks.Open(url);
            xlworkSheet = (Excel.Worksheet)xlWorkBook.Sheets[1];
            xlworkSheet.Unprotect();
            Excel.Range xlRange = xlworkSheet.UsedRange;

            int startIndex = 5;
            int numbOfRows = xlRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row;
            int numbOfColumns = xlRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Column;

            Type type = typeof(T);
            // the properties what need to get value from a excel file.
            List<string> properties = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
            if (properties.Count == 0)
            {
                throw new Exception("Dont have any required property.");
            }
            // a mapping the name of a property and a name of column in a excel file.
            Dictionary<string, string> columnMap = ColumnNamesMapping(typeof(T));
            if (columnMap.Count == 0)
            {
                return false;
            }
            int numbRecords =  GetNumbOfColumnsToRead(typeof(T));
            for (int i = startIndex; i < numbOfRows; i++)
            {
                Dictionary<string, string[]> mappingInExcel = ExcelColumnAttribute.GetNameOfColumnsInExcel(typeof(T));

                T newObj = default(T);
                List<T> listNewObjects = new List<T>();
                for (int index = 0; index < numbRecords; index++)
                {
                    newObj = (T)Activator.CreateInstance(typeof(T));
                    foreach (string property in properties) // properties are 
                    {
                        PropertyInfo propertyInfo = newObj.GetType().GetProperty(property);
                        if (IsPrimaryKey(typeof(T), property)) // handling for a ForeignKey.
                        {

                        }
                        else if(IsForeignKey(typeof(T), property)) // handling for a ForeignKey.
                        {
                        }
                        else
                        {
                            string[] columnsInExcel = null;
                            bool success = mappingInExcel.TryGetValue(property, out columnsInExcel); // get 
                            if (success)
                            {
                                // get value in the excel file.
                                string valueInCell = GetValueInCell(xlworkSheet, i, columnsInExcel[index]);
                                propertyInfo.SetValueByDataType(newObj, valueInCell);
                            }
                            else
                            {
                                throw new ArgumentException("The parameters in the ExcelColumnAttribute are not correct.");
                            }
                        }
                    }
                    listNewObjects.Add(newObj);
                }
                listNewObjects = PreProcess<T>(listNewObjects);
            }
            // close the excel app.
            xlWorkBook.Close();
            xlApplication.Quit();
            return true;
        }
        private static void SetRelationshipsForObjects<T>(List<T> list)
        {
            GetDistinguishTables(typeof(T),)
            foreach (var item in list)
            {

            }
        }
        // eliminate records what all of paramers is null.
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<T> PreProcess<T>(List<T> list) {
            List<T> correctItems = new List<T>(); 
            foreach (var item in list)
            {
                if (!AllOfParameterIsNull(item))
                {
                    correctItems.Add(item);
                }
            }
            return correctItems;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool AllOfParameterIsNull(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            bool flag = false;
            foreach (PropertyInfo p in properties)
            {
                if (!CheckDefaultValue(obj,p))
                {
                    flag = true;
                }
            }
            return flag ? false : true;
        }
        private static bool CheckDefaultValue(object obj, PropertyInfo p)
        {
            if (p.PropertyType == typeof(string))
            {
                var obj2 = p.GetValue(obj, null);
                return  obj2 == null;
            }
            else if (p.PropertyType == typeof(int))
            {
                var obj2 = p.GetValue(obj, null);
                return obj2.Equals(default(int));
            }
            else if (p.PropertyType == typeof(double))
            {
                var obj2 = p.GetValue(obj, null);
                return obj2.Equals(default(double));
            }
            else
            {
                var obj2 = p.GetValue(obj, null);
                return obj2 == null;
            }
        }
    }
}
