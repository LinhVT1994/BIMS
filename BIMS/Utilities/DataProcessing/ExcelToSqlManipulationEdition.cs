using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using static DataUtilities.Attributes.AutoIncrementAttribute;
using static DataUtilities.Attributes.UniqueAttribute;
using static DataUtilities.Attributes.ExcelColumnAttribute;
using static DataUtilities.Attributes.DistinguishAttribute;
using static DataUtilities.Attributes.ForeignKeyAttribute;
using static DataUtilities.Attributes.PrimaryKeyAttribute;
using static DataUtilities.Attributes.PropertyInfoExtensions;
using DataUtilities.Data;
using System.Diagnostics;
using DataUtilities.Model;

namespace DataUtilities.DataProcessing
{
    public class ExcelToSqlManipulationEdition
    {
        //private string _Url = @"C:\Users\TUAN-LINH\Desktop\TestData.xlsx";
        public static string Url = null;
        private Microsoft.Office.Interop.Excel.Application _XlApplication = null;
        private Excel.Worksheet _XlworkSheet = null;
        Excel.Workbook _XlWorkBook = null;
        private int _StartRowInExcel = 1;
        public int EndAtLine
        {
            get
            {
                return _NumbOfRows;
            }
            set
            {
                _NumbOfRows = value;
            }
        }
        private string connectionString = null;
        public int StartRowInExcel
        {
            get
            {
                return _StartRowInExcel;
            }
            set
            {
                _StartRowInExcel = value;
            }
        }
        private Excel.Range _XlRange;
        private int _NumbOfRows = 0;
        private int _NumbOfColumns = 0;
        private ExcelToSqlManipulationEdition(string url, string connectionString)
        {
            Url = url;
            this.connectionString = connectionString;
            try
            {
                _XlApplication = new Microsoft.Office.Interop.Excel.Application();
                _XlApplication.Visible = false;
                _XlApplication.DisplayAlerts = false;
                _XlWorkBook = _XlApplication.Workbooks.Open(Url);
                _XlworkSheet = (Microsoft.Office.Interop.Excel.Worksheet)_XlWorkBook.Sheets[1];
                _XlworkSheet.Unprotect();
                _XlRange = _XlworkSheet.UsedRange;
                if (_NumbOfRows == 0)
                {
                    _NumbOfRows = _XlRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row;
                }
                
                _NumbOfColumns = _XlRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Column;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="url">Link to a extend file.</param>
        /// <returns>Null if cant open the file.Otherwise, an instance object will be return.</returns>
        public static ExcelToSqlManipulationEdition CreateInstance(string excelUrl,string connectionString)
        {
            ExcelToSqlManipulationEdition newOne = null;
            if (string.IsNullOrEmpty(excelUrl))
            {
                return null;
            }
            else if (string.IsNullOrEmpty(connectionString))
            {
                return null;
            }
            try
            {
                newOne = new ExcelToSqlManipulationEdition(excelUrl, connectionString);
            }

            catch (ArgumentException)
            {
                newOne = null;
            }
            return newOne;

        }

        public Dictionary<int, T> ReadData<T>()
        {
            try
            {
                // get properties what need to get value from an excel file.
                List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
                if (propertyNames.Count == 0)
                {
                    throw new Exception("Don't have any required property.");
                }
                Dictionary<int, T> results = new Dictionary<int, T>();
                for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
                {
                    // create new object of the genneric object.
                    T newObj = (T)Activator.CreateInstance(typeof(T));
                    foreach (string pName in propertyNames)
                    {
                        PropertyInfo propertyInfo = newObj.GetType().GetProperty(pName);
                        HandleForRequiredProperty(newObj, pName, row);
                    }

                    results.Add(row, newObj);
                }
                return results;
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                CloseExcelFile();
            }
        }
        /*
         public void PreProcessingData()
         {
             try
             {
                 // get properties what need to get value from an excel file.
                 List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(Region));
                 if (propertyNames.Count == 0)
                 {
                     throw new Exception("Don't have any required property.");
                 }
                 for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
                 {
                     // create new object of the genneric object.
                     Region newObj = (Region)Activator.CreateInstance(typeof(Region));
                     foreach (string pName in propertyNames)
                     {
                         PropertyInfo propertyInfo = newObj.GetType().GetProperty(pName);
                         HandleForRequiredProperty(newObj, pName, row);
                     }
                     newObj.Adjust();
                     var res = FindResultOnDatabase(newObj);
                     if (res != null && res.Count >= 1)
                     {
                         object rs1 = res.GetElementAt(0).Value("region_name");
                         object rs2 = res.GetElementAt(0).Value("zip_code");
                         SetValueInCell(row, "I", rs1);
                         SetValueInCell(row, "J", rs2);
                     }

                 }
             }
             catch (Exception e)
             {

                 throw e;
             }
             finally
             {
                 CloseExcelFile();
             }

         }


         private DataTable FindResultOnDatabase(Region newObj)
         {
             string sqlQuery = @"";
             StringBuilder str = new StringBuilder();
             str.AppendFormat("select * from regions where region_parent_id in(select region_id from regions where regions.region_parent_id in (select DISTINCT region_id from regions WHERE region_level = 1 and region_name = '{0}') and(region_name = '{1}')) and region_name like '%{2}%'", newObj.Prefecture, newObj.Ward, newObj.Area);

             SqlDataAccess sqlDataAccess = new SqlDataAccess();
             var sqlParams = new List<SqlParameter>();
             if (newObj == null || string.IsNullOrWhiteSpace(newObj.Prefecture) ||
                 string.IsNullOrWhiteSpace(newObj.Ward) ||
                 string.IsNullOrWhiteSpace(newObj.Area))
             {
                 return null;
             }
             sqlParams.Add(new SqlParameter("level1", newObj.Prefecture));
             sqlParams.Add(new SqlParameter("level2", newObj.Ward));
             sqlParams.Add(new SqlParameter("level3", newObj.Area));

             return sqlDataAccess.ExecuteSelectMultiTables(str.ToString(), null);

         }
        */
        public void Execute<T>()
        {
            bool hasALeastOneUnique = true;
            // the properties what need to get value from a excel file.
            List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
            if (propertyNames.Count == 0)
            {
                throw new Exception("Don't have any required property.");
            }
            // the list of properties what required to have to has a value is unique. 
            List<string> uniqueProperties = GetUniqueProperties(typeof(T));

            if (GetUniqueProperties(typeof(T)).Count <= 0)
            {
                hasALeastOneUnique = false;
            }
            Dictionary<string, T> dicResult = new Dictionary<string, T>();
            for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
            {
                // create new object of the genneric object.
                T newObj = (T)Activator.CreateInstance(typeof(T));
                string key = null;
                foreach (string pName in propertyNames)
                {
                    PropertyInfo propertyInfo = newObj.GetType().GetProperty(pName);
                    if (IsPrimaryKey(typeof(T), pName)) // is primany key.
                    {
                        if (IsAutoIncrement(typeof(T), pName))
                        {
                            if (!hasALeastOneUnique)
                            {
                                key = (dicResult.Count + 1).ToString();
                            }
                            propertyInfo.SetValue(newObj, dicResult.Count + 1);
                        }
                    }
                    else if (IsUnique(typeof(T), pName))
                    {
                        try
                        {
                            string columnName = null;
                            key = HandleForUniqueKey(newObj, pName, row, out columnName);
                            if (string.IsNullOrWhiteSpace(key))
                            {
                                string message = string.Format("Error at: Cell[{0},{1}] Handled: {2} Message: {3}", row, columnName, "Ignore", "Can't get value on this cell.");
                                SetErrorInfoMarkForRow(row);
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    else if (IsForeignKey(typeof(T), pName)) // is foreign key.
                    {
                        //　get the table and property what referenced to.
                        if (!HandleForForeignKey<T>(newObj, pName, row))
                        {
                            break;
                        }
                    }
                    else
                    {
                        HandleForRequiredProperty(newObj, pName, row);
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
                        RequestInsertToSql<T>(newObj);
                    }
                }
            }
            CloseExcelFile();
        }
        public void GetRowData<T>(Action<T> action)
        {
            try
            {
                // get properties what need to get value from an excel file.
                List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
                if (propertyNames.Count == 0)
                {
                    throw new Exception("Don't have any required property.");
                }
                for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
                {
                    // create new object of the genneric object.
                    T newObj = (T)Activator.CreateInstance(typeof(T));
                    foreach (string pName in propertyNames)
                    {
                        PropertyInfo propertyInfo = newObj.GetType().GetProperty(pName);
                        HandleForRequiredProperty(newObj, pName, row);
                    }
                    action?.Invoke(newObj);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                CloseExcelFile();
            }
        }
        public List<T> ExecuteDataGetting<T>(Action<T, object[]> getDataMethod, Predicate<T> CheckData)
        {
            List<T> listData = new List<T>();
            try
            {
                // get properties what need to get value from an excel file.
                List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
                if (propertyNames.Count == 0)
                {
                    throw new Exception("Don't have any required property.");
                }
                for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
                {
                    // create new object of the genneric object.
                    T newObj = (T)Activator.CreateInstance(typeof(T));
                    object[] data = new object[_NumbOfColumns];

                    for (int i = 1; i < _NumbOfColumns; i++)
                    {
                        data[i-1] = _XlworkSheet.Cells[row, i]?.Value;
                    }
                    if (getDataMethod != null)
                    {
                        getDataMethod?.Invoke(newObj, data);
                    }
                  
                    if (CheckData!=null)
                    {
                        if (CheckData.Invoke(newObj))
                        {
                            listData.Add(newObj);
                        }
                        Debug.WriteLine(newObj.ToString());
                    }
                }
                return listData;
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                CloseExcelFile();
            }
        }
        public void ExecuteTraveling<T>(Predicate<T> CheckData, Func<T,T> adjustObjMethod ,Dictionary<string, string> excelColumMap)
        {
            if (adjustObjMethod == null || excelColumMap == null || excelColumMap.Count <= 0)
            {
                throw new ArgumentNullException(nameof(adjustObjMethod));
            }
            try
            {
                // get properties what need to get value from an excel file.
                List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
                if (propertyNames.Count == 0)
                {
                    throw new Exception("Don't have any required property.");
                }
                for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
                {
                    if (EndAtLine>0&& EndAtLine == row)
                    {
                        break;
                    }
                    // create new object of the genneric object.
                    T newObj = (T)Activator.CreateInstance(typeof(T));
                    foreach (string pName in propertyNames)
                    {
                        PropertyInfo propertyInfo = newObj.GetType().GetProperty(pName);
                        HandleForRequiredProperty(newObj, pName, row);
                    }
                
                    if (!CheckData(newObj))
                    {
                        continue;
                    }
                    newObj = adjustObjMethod.Invoke(newObj);
                    if (newObj == null)
                    {
                        continue;
                    }
                    Type type = newObj.GetType();
                    foreach (var item in excelColumMap)
                    {
                        try
                        {
                            var result = type.GetProperty(item.Key);
                            var obj = result.GetValue(newObj);
                            SetValueInCell(row, item.Value, obj);
                        }
                        catch (IndexOutOfRangeException e)
                        {

                            throw e;
                        }
                   
                    }

                }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.WriteLine("Error: " + e.Message);
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                CloseExcelFile();
            }
        }


        public bool ExecuteMultiRecords<T>(Predicate<T> CheckData)
        {
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
            int numbRecords = GetNumbOfColumnsToRead(typeof(T));
            for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
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
                        else if (IsForeignKey(typeof(T), property)) // handling for a ForeignKey.
                        {

                        }
                        else
                        {
                            string[] columnsInExcel = null;
                            bool success = mappingInExcel.TryGetValue(property, out columnsInExcel); // get 
                            if (success)
                            {
                                // get value in the excel file.
                                try
                                {
                                    string valueInCell = GetValueInCell(row, columnsInExcel[index]);
                                    propertyInfo.SetValueByDataType(newObj, valueInCell);
                                }
                                catch (Exception e)
                                {

                                    throw e;
                                }

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
                if (listNewObjects == null || listNewObjects.Count <= 0)
                {
                    continue;
                }
                // update for foreign key properties.
              

                // insert all of elements in the list to the sql.
                foreach (var obj in listNewObjects)
                {
                    // set value for 
                    try
                    {
                        bool? validatedResult = CheckData?.Invoke(obj);
                        if ((validatedResult==null?false:(bool)validatedResult))
                        {
                           
                            RequestInsertToSql<T>(obj);
                        }
                        
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }

                }

            }
            // close the excel app.
            CloseExcelFile();
            return true;
        }

        public void ExecuteComparing<T>(Predicate<T> CheckData, Func<T, string> queryStringCreator, Dictionary<string, string> excelColumMap, Func<string,string,bool> CheckDataBeforeUpdate = null)
        {
            if (queryStringCreator == null || excelColumMap == null || excelColumMap.Count <= 0)
            {
                throw new ArgumentNullException(nameof(queryStringCreator));
            }
            try
            {
                // get properties what need to get value from an excel file.
                List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
                if (propertyNames.Count == 0)
                {
                    throw new Exception("Don't have any required property.");
                }
                for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
                {
                    // create new object of the genneric object.
                    T newObj = (T)Activator.CreateInstance(typeof(T));
                    foreach (string pName in propertyNames)
                    {
                        PropertyInfo propertyInfo = newObj.GetType().GetProperty(pName);
                        HandleForRequiredProperty(newObj, pName, row);
                    }
                  
                    if (!CheckData(newObj))
                    {
                        continue;
                    }
                    string query = queryStringCreator?.Invoke(newObj);
                    if (query == null)
                    {
                        throw new Exception("Null exception: " + nameof(query));
                    }
                    var res = GetDataFromDB(query);
                   
                    if (res != null && res.Count >= 1)
                    {
                        var data = ParseToListItems<T>(res);
                        
                        foreach (var item in excelColumMap)
                        {
                            object rs1 = res.GetElementAt(0).Value(item.Key);
                            if (rs1 != null && !string.IsNullOrWhiteSpace(rs1.ToString()))
                            {
                                if (CheckDataBeforeUpdate == null)
                                {
                                    SetValueInCell(row, item.Value, rs1);
                                }
                                else
                                {
                                    if (CheckDataBeforeUpdate.Invoke(item.Key, rs1 == null?"" : rs1.ToString()))
                                    {
                                        SetValueInCell(row, item.Value, rs1);
                                    }
                                }
                           
                            }
                           
                        }
                    }

                }
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                CloseExcelFile();
            }
        }
        private IEnumerable<T> ParseToListItems<T>(DataTable datatable)
        {
            Type type = typeof(T);
            T newObj = default(T);
            List<T> result = new List<T>();
            int count = 0;
            foreach (DataSet record in datatable.GetAllRecords())
            {
                newObj = (T)Activator.CreateInstance(type);
                foreach (var property in GetRequiredProperties(type))
                {
                    string nameOfColumn = GetNameOfParameterInSql(type, property.Name);
                    if (string.IsNullOrWhiteSpace(nameOfColumn))
                    {
                        continue;
                    }
                    object value = record.Value(nameOfColumn);
                    property.SetValueByDataType(newObj, value);
                }
                result.Add(newObj);
            }
            return result;
        }
        public static List<PropertyInfo> GetRequiredProperties(Type type)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(RequiredAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    result.Add(property); // add a attribute in the required properties.
                }
            }
            return result;
        }
        public static string GetNameOfParameterInSql(Type type, string propertyName)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Trim().ToLower().Equals(propertyName.Trim().ToLower()))
                {
                    Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(SqlParameterAttribute), false); // get the attributes of a property.
                    if (attributes.Length > 0)
                    {
                        string value = ((SqlParameterAttribute)attributes[0]).PropertyName.ToString();
                        return value;
                    }
                }
            }
            return null;
        }
        public string GetQueryStringData<T>(T obj)
        {
            return null;
        }

        public DataTable GetDataFromDB(string query)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess(connectionString);
            return sqlDataAccess.ExecuteSelectMultiTables(query, null);
        }
        public void Upload<T>(Predicate<T> validate,
                              Func<T, T> preProcessingProceduce)
        {
            try
            {
                bool hasALeastOneUnique = true;
                // the properties what need to get value from a excel file.
                List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
                if (propertyNames.Count == 0)
                {
                    throw new Exception("Don't have any required property.");
                }
                // the list of properties what required to have to has a value is unique. 
                List<string> uniqueProperties = GetUniqueProperties(typeof(T));

                if (GetUniqueProperties(typeof(T)).Count <= 0)
                {
                    hasALeastOneUnique = false;
                }
                Dictionary<string, T> dicResult = new Dictionary<string, T>();
                for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
                {
                    // create new object of the genneric object.
                    T newObj = (T)Activator.CreateInstance(typeof(T));
                    string key = null;
                    foreach (string pName in propertyNames)
                    {
                        PropertyInfo propertyInfo = newObj.GetType().GetProperty(pName);
                        if (IsPrimaryKey(typeof(T), pName)) // is primany key.
                        {
                            if (IsAutoIncrement(typeof(T), pName))
                            {
                                if (!hasALeastOneUnique)
                                {
                                    key = (dicResult.Count + 1).ToString();
                                }
                                propertyInfo.SetValue(newObj, dicResult.Count + 1);
                            }
                        }
                        else if (IsUnique(typeof(T), pName))
                        {
                            try
                            {
                                string columnName = null;
                                key = HandleForUniqueKey(newObj, pName, row, out columnName);
                                if (string.IsNullOrWhiteSpace(key))
                                {
                                    string message = string.Format("Error at: Cell[{0},{1}] Handled: {2} Message: {3}", row, columnName, "Ignore", "Can't get value on this cell.");
                                    
                                    break;
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        else if (IsForeignKey(typeof(T), pName)) // is foreign key.
                        {
                            //　get the table and property what referenced to.
                            if (!HandleForForeignKey<T>(newObj, pName, row))
                            {
                                continue;
                            }
                        }
                        else
                        {
                            HandleForRequiredProperty(newObj, pName, row);
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
                            if (!validate.Invoke(newObj))
                            {
                                //SetErrorInfoMarkForRow(row);
                                continue;
                            }
                            newObj = preProcessingProceduce.Invoke(newObj);
                            RequestInsertToSql<T>(newObj);
                            var dataTable = GetLastElement<T>();
                            Type type = typeof(T);
                            var listSaveProperties = ExcelTemporaryStorageAttribute.GetExcelTemporaryStoragePropertiesName(type);
                            if (listSaveProperties != null && listSaveProperties.Count > 0)
                            {
                                foreach (var property in listSaveProperties)
                                {
                                    string sqlColumn = SqlParameterAttribute.GetNameOfParameterInSql(type, property);
                                    var data = dataTable.GetElementAt(0).Value(sqlColumn);
                                    string column = ExcelTemporaryStorageAttribute.GetExcelTemporaryStorageColumn(type, property);
                                    SetValueInCell(row, column, data);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                CloseExcelFile();
            }
        }
        public void UploadIfNotExisted<T>(Predicate<T> validate, 
                              Func<T,T> preProcessingProceduce = null)
        {
            bool hasALeastOneUnique = true;
            // the properties what need to get value from a excel file.
            List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
            if (propertyNames.Count == 0)
            {
                throw new Exception("Don't have any required property.");
            }
            // the list of properties what required to have to has a value is unique. 
            List<string> uniqueProperties = GetUniqueProperties(typeof(T));

            if (GetUniqueProperties(typeof(T)).Count <= 0)
            {
                hasALeastOneUnique = false;
            }
            Dictionary<string, T> dicResult = new Dictionary<string, T>();
            for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
            {
                // create new object of the genneric object.
                T newObj = (T)Activator.CreateInstance(typeof(T));
                string key = null;
                foreach (string pName in propertyNames)
                {
                    PropertyInfo propertyInfo = newObj.GetType().GetProperty(pName);
                    if (IsPrimaryKey(typeof(T), pName)) // is primany key.
                    {
                        if (IsAutoIncrement(typeof(T), pName))
                        {
                            if (!hasALeastOneUnique)
                            {
                                key = (dicResult.Count + 1).ToString();
                            }
                            propertyInfo.SetValue(newObj, dicResult.Count + 1);
                        }
                    }
                    else if (IsUnique(typeof(T), pName))
                    {
                        try
                        {
                            string columnName = null;
                            key = HandleForUniqueKey(newObj, pName, row, out columnName);
                            if (string.IsNullOrWhiteSpace(key))
                            {
                                string message = string.Format("Error at: Cell[{0},{1}] Handled: {2} Message: {3}", row, columnName, "Ignore", "Can't get value on this cell.");
                                SetErrorInfoMarkForRow(row);
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    else if (IsForeignKey(typeof(T), pName)) // is foreign key.
                    {
                        //　get the table and property what referenced to.
                        if (!HandleForForeignKey<T>(newObj, pName, row))
                        {
                            break;
                        }
                    }
                    else
                    {
                        HandleForRequiredProperty(newObj, pName, row);
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
                    bool? validated = validate?.Invoke(newObj);
                    if (validated == null || validated == false)
                    {
                        continue;
                    }
                    newObj = preProcessingProceduce.Invoke(newObj);

                    var result = CheckExistOnDB(newObj);
                    if (!result)
                    {
                        RequestInsertToSql<T>(newObj);
                    }
                   
                }
            }
            CloseExcelFile();
        }
        public void UploadExcelFromDB<T>()
        {
            bool hasALeastOneUnique = true;
            // the properties what need to get value from a excel file.
            List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
            if (propertyNames.Count == 0)
            {
                throw new Exception("Don't have any required property.");
            }
            // the list of properties what required to have to has a value is unique. 
            List<string> uniqueProperties = GetUniqueProperties(typeof(T));

            if (GetUniqueProperties(typeof(T)).Count <= 0)
            {
                hasALeastOneUnique = false;
            }
            Dictionary<string, T> dicResult = new Dictionary<string, T>();
            for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
            {
                // create new object of the genneric object.
                T newObj = (T)Activator.CreateInstance(typeof(T));
                string key = null;
                foreach (string pName in propertyNames)
                {
                    PropertyInfo propertyInfo = newObj.GetType().GetProperty(pName);
                    if (IsPrimaryKey(typeof(T), pName)) // is primany key.
                    {
                        if (IsAutoIncrement(typeof(T), pName))
                        {
                            if (!hasALeastOneUnique)
                            {
                                key = (dicResult.Count + 1).ToString();
                            }
                            propertyInfo.SetValue(newObj, dicResult.Count + 1);
                        }
                    }
                    else if (IsUnique(typeof(T), pName))
                    {
                        try
                        {
                            string columnName = null;
                            key = HandleForUniqueKey(newObj, pName, row, out columnName);
                            if (string.IsNullOrWhiteSpace(key))
                            {
                                string message = string.Format("Error at: Cell[{0},{1}] Handled: {2} Message: {3}", row, columnName, "Ignore", "Can't get value on this cell.");
                                SetErrorInfoMarkForRow(row);
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    else if (IsForeignKey(typeof(T), pName)) // is foreign key.
                    {
                        //　get the table and property what referenced to.
                        if (!HandleForForeignKey<T>(newObj, pName, row))
                        {
                            break;
                        }
                    }
                    else
                    {
                        HandleForRequiredProperty(newObj, pName, row);
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
                    var result = GetDataFromDB(newObj);
                    if (result != null)
                    {
                        Type type = typeof(T);
                        var listSaveProperties = ExcelTemporaryStorageAttribute.GetExcelTemporaryStoragePropertiesName(type);
                        if (listSaveProperties != null && listSaveProperties.Count > 0)
                        {
                            foreach (var property in listSaveProperties)
                            {
                                string sqlColumn = SqlParameterAttribute.GetNameOfParameterInSql(type, property);
                                var data = result.GetElementAt(0).Value(sqlColumn);
                                string column = ExcelTemporaryStorageAttribute.GetExcelTemporaryStorageColumn(type, property);
                                SetValueInCell(row, column, data);
                            }
                        }
                    }
                  

                }
            }
            CloseExcelFile();
        }
        public void Update<T>(Predicate<T> validate,
                         Func<T, T> preProcessingProceduce = null)
        {
            // the properties what need to get value from a excel file.
            List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
            if (propertyNames.Count == 0)
            {
                throw new Exception("Don't have any required property.");
            }
            // the list of properties what required to have to has a value is unique. 
            List<string> uniqueProperties = GetUniqueProperties(typeof(T));
            Dictionary<string, T> dicResult = new Dictionary<string, T>();
            for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
            {
                // create new object of the genneric object.
                T newObj = (T)Activator.CreateInstance(typeof(T));
                foreach (string pName in propertyNames)
                {
                    PropertyInfo propertyInfo = newObj.GetType().GetProperty(pName);
                   if (IsForeignKey(typeof(T), pName)) // is foreign key.
                    {
                        //　get the table and property what referenced to.
                        if (!HandleForForeignKey<T>(newObj, pName, row))
                        {
                            break;
                        }
                    }
                    else
                    {
                        HandleForRequiredProperty(newObj, pName, row);
                    }
                }
                // if the key has not setted any value then ignore it.
                bool? validated = validate?.Invoke(newObj);
                if (validated == null || validated == false)
                {
                    continue;
                }
                newObj = preProcessingProceduce.Invoke(newObj);

                var result = CheckExistOnDB(newObj);
                if (result)
                {
                    RequestUpdateToSql<T>(newObj);
                }
            }
            CloseExcelFile();
        }

        public void UpdateByPrimaryKey<T>(Predicate<T> validate)
        {
            // the properties what need to get value from a excel file.
            List<string> propertyNames = RequiredAttribute.GetRequiredPropertiesName(typeof(T));
            if (propertyNames.Count == 0)
            {
                throw new Exception("Don't have any required property.");
            }
            // the list of properties what required to have to has a value is unique. 
            List<string> uniqueProperties = GetUniqueProperties(typeof(T));
            Dictionary<string, T> dicResult = new Dictionary<string, T>();
            for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
            {
                // create new object of the genneric object.
                T newObj = (T)Activator.CreateInstance(typeof(T));
                foreach (string pName in propertyNames)
                {
                    PropertyInfo propertyInfo = newObj.GetType().GetProperty(pName);
                    if (IsForeignKey(typeof(T), pName)) // is foreign key.
                    {
                        //　get the table and property what referenced to.
                        if (!HandleForForeignKey<T>(newObj, pName, row))
                        {
                            break;
                        }
                    }
                    else
                    {
                        HandleForRequiredProperty(newObj, pName, row);
                    }
                }
                if (validate.Invoke(newObj))
                {
                    RequestUpdateToSql<T>(newObj);
                }
                
            }
            CloseExcelFile();
        }
        public DataTable GetLastElement<T>()
        {
            var typeObject = typeof(T);
            string tableName = (typeObject.GetCustomAttribute(typeof(SqlParameterAttribute), false) as SqlParameterAttribute).PropertyName;
            SqlDataAccess access = new SqlDataAccess(connectionString);
            var para = GetPrimaryKey(typeof(T));
            string id = SqlParameterAttribute.GetNameOfParameterInSql(typeObject, para.Name);

            string query = "select * from " + tableName + " order by " + id + " desc limit 1";
            return access.ExecuteSelectMultiTables(query,null);
        }
        public bool CheckExistOnDB<T>(T obj)
        {
            var typeObject = typeof(T);
            var properties = GetUniqueProperties(typeof(T));
            if (properties == null || properties.Count() <= 0)
            {
                return false;
            }

            string tableName = (typeObject.GetCustomAttribute(typeof(SqlParameterAttribute), false) as SqlParameterAttribute).PropertyName;
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("select * from {0} where ", tableName);
            foreach (var str in properties)
            {
                var propertyInfo = typeObject.GetProperty(str);
                string sqlCol = SqlParameterAttribute.GetNameOfParameterInSql(typeObject, str);
                var value = propertyInfo.GetValue(obj);
                if (propertyInfo.PropertyType == typeof(string))
                {
                    queryBuilder.AppendFormat("{0} = '{1}' and ", sqlCol, value);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    queryBuilder.AppendFormat("{0} = '{1}' and ", sqlCol, (int)value);
                }
                else
                {

                }
                
            }
            string query = queryBuilder.ToString();
            query = query.Remove(query.Length - 5, 5);
            SqlDataAccess access = new SqlDataAccess(connectionString);
            var data = access.ExecuteSelectMultiTables(query, null);
            if (data == null || data.Count <= 0)
            {
                return false;
            }
            return true;
        }
        public DataTable GetDataFromDB<T>(T obj)
        {
            var typeObject = typeof(T);
            var properties = GetUniqueProperties(typeof(T));
            if (properties == null || properties.Count() <= 0)
            {
                return null;
            }

            string tableName = (typeObject.GetCustomAttribute(typeof(SqlParameterAttribute), false) as SqlParameterAttribute).PropertyName;
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("select * from {0} where ", tableName);
            foreach (var str in properties)
            {
                var propertyInfo = typeObject.GetProperty(str);
                string sqlCol = SqlParameterAttribute.GetNameOfParameterInSql(typeObject, str);
                var value = propertyInfo.GetValue(obj);
                if (propertyInfo.PropertyType == typeof(string))
                {
                    queryBuilder.AppendFormat("{0} = '{1}' and ", sqlCol, value);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    queryBuilder.AppendFormat("{0} = '{1}' and ", sqlCol, (int)value);
                }

            }
            string query = queryBuilder.ToString();
            query = query.Remove(query.Length - 5, 5);
            SqlDataAccess access = new SqlDataAccess(connectionString);
            var data = access.ExecuteSelectMultiTables(query, null);
            if (data == null || data.Count <= 0)
            {
                return null;
            }
            return data;
        }
        public bool ExecuteMultiRecords<T>(Predicate<T> validate,Action<T> preProcessingProceduce = null)
        {
            try
            {
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
                int numbRecords = GetNumbOfColumnsToRead(typeof(T));
                for (int row = _StartRowInExcel; row < _NumbOfRows; row++)
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
                            else if (IsForeignKey(typeof(T), property)) // handling for a ForeignKey.
                            {

                            }
                            else
                            {
                                string[] columnsInExcel = null;
                                bool success = mappingInExcel.TryGetValue(property, out columnsInExcel); // get 
                                if (success)
                                {
                                    // get value in the excel file.
                                    try
                                    {
                                        string valueInCell = GetValueInCell(row, columnsInExcel[index]);
                                        propertyInfo.SetValueByDataType(newObj, valueInCell);
                                    }
                                    catch (Exception e)
                                    {

                                        throw e;
                                    }

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
                    if (listNewObjects == null || listNewObjects.Count <= 0)
                    {
                        continue;
                    }
                    // update for foreign key properties.
                    try
                    {
                        SetRelationshipsForObjects<T>(listNewObjects, row);
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }

                    // insert all of elements in the list to the sql.
                    foreach (var obj in listNewObjects)
                    {
                        // set value for 
                        try
                        {
                            bool? validated = validate?.Invoke(obj);
                            if (validated == null || validated == false)
                            {
                                continue;
                            }
                            if (preProcessingProceduce != null)
                            {
                                preProcessingProceduce?.Invoke(obj);
                            }

                            RequestInsertToSql<T>(obj);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                // close the excel app.
                CloseExcelFile();
            }
            return true;
          
        }
        /// <summary>
        /// Update for foreign key properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list of items want to update.</param>
        private void SetRelationshipsForObjects<T>(List<T> list, int row)
        {
            foreach (var item in list)
            {
                HandleForeignKeyPropertiesOfObject<T>(item, row);
            }
            T obj = list.ElementAt(0);
            Type type = obj.GetType();
            int id = -1;
            foreach (var foreignkeyName in GetForeignKeyProperties(type))
            {
                List<string> refTables = GetDistinguishTables(type, foreignkeyName);
                if (refTables != null && refTables.Count > 0)
                {
                    PropertyInfo propertyInfo = obj.GetType().GetProperty(foreignkeyName);
                    List<string> refTablesRelateWith = GetDistinguishTables(type, foreignkeyName);

                    List<SqlParameter> sqlParameters = CreateListSqlParamenter<T>(obj, foreignkeyName, row);
                    Dictionary<string, string> a = GetExcelColumnReferences(type, foreignkeyName);
                    string refTable = GetRefTable(type, foreignkeyName);
                    KeyValuePair<string, string> tableRelationInfo = CreateRelationshipsInTableSqlFromObjects(propertyInfo.PropertyType, refTablesRelateWith.ElementAt(0));
                    string selectOptions = CreateSelectOptions(propertyInfo.PropertyType, refTable, refTablesRelateWith.ElementAt(0));
                    DataTable recoders = GetDataFromSql(tableRelationInfo, sqlParameters, selectOptions);
                    id = DetectKeyId<T>(recoders, obj, foreignkeyName, row);
                    foreach (var item in list)
                    {
                        PropertyInfo property = item.GetType().GetProperty(foreignkeyName);
                        if (property != null)
                        {
                            property.SetValueByDataType(item, id);
                        }
                    }
                }
            }
        }
        private void HandleForeignKeyPropertiesOfObject<T>(T obj, int row)
        {
            Type type = obj.GetType();
            foreach (var foreignkeyName in GetForeignKeyProperties(type))
            {
                List<string> refTables = GetDistinguishTables(type, foreignkeyName);
                if (refTables == null || refTables.Count <= 0)
                {
                    // dealing like a nomal foreign key.
                    HandleForForeignKey<T>(obj, foreignkeyName, row);
                }
            }
        }

        public int DetectKeyId<T>(DataTable recoders, T obj, string foreignkeyName, int row)
        {
            Type type = obj.GetType();
            string refId = SqlParameterAttribute.GetNameOfParameterInSql(type, GetPrimaryKey(type).Name);
            string table = (type.GetCustomAttributes(typeof(SqlParameterAttribute), false)[0] as SqlParameterAttribute).PropertyName;
            PropertyInfo propertyInfo = type.GetProperty(foreignkeyName);
            string property = (propertyInfo.GetCustomAttributes(typeof(SqlParameterAttribute), false)[0] as SqlParameterAttribute).PropertyName;
            PropertyInfo primaryKey = PrimaryKeyAttribute.GetPrimaryKey(propertyInfo.PropertyType);
            string keys = (primaryKey.GetCustomAttributes(typeof(SqlParameterAttribute), false)[0] as SqlParameterAttribute).PropertyName;
            foreach (var item in recoders.GetAllRecords())
            {
                string id = item.Value(keys);
                if (IsUsedPrimaryKey(table, refId, property, int.Parse(id)))
                {

                }
                else
                {
                    return int.Parse(id);
                }
            }
            return 0;
        }

        public bool IsUsedPrimaryKey(string table, string refId, string property, object value)
        {
            List<SqlParameter> para = new List<SqlParameter>();
            para.Add(new SqlParameter(property, value));
            DataSet data = GetForeignKeyInSQL(refId, table, para);
            if (data == null || data.Length <= 0)
            {
                return false;
            }
            return true;
        }
        public string CreateSelectOptions<T>(T obj, string getOnTable, string toTable)
        {
            string selectProperties = null;
            Type type = null;
            if (obj is Type)
            {
                type = obj as Type;
            }
            else
            {
                type = obj.GetType();

            }
            List<Type> listObjects = new List<Type>();
            List<PropertyInfo> list = DetectRelationships.GetRelationships(type, toTable);
            foreach (var item in list)
            {
                listObjects.Add(item.PropertyType);
            }
            listObjects.Add(type);
            string tableName = null;
            foreach (Type typeObject in listObjects)
            {
                tableName = (typeObject.GetCustomAttribute(typeof(SqlParameterAttribute), false) as SqlParameterAttribute).PropertyName;
                if (tableName != null)
                {
                    if (getOnTable.ToLower().Equals(tableName.ToLower()))
                    {
                        foreach (string pInfo in RequiredAttribute.GetRequiredPropertiesName(typeObject))
                        {
                            string nameOfProperties = SqlParameterAttribute.GetNameOfParameterInSql(typeObject, pInfo);
                            //table.name as "table.name"
                            //  selectProperties += "," + string.Format("{0}.{1} as \"{2}.{3}\"", tableName, nameOfProperties, tableName, nameOfProperties); ;
                            selectProperties += "," + string.Format("{0}.{1}", tableName, nameOfProperties);
                        }
                    }

                }

            }
            selectProperties = selectProperties.TrimStart(',');
            return selectProperties;
        }
        public KeyValuePair<string, string> CreateRelationshipsInTableSqlFromObjects<T>(T obj, string toTable)
        {
            List<string> connectStringBetweenTables = new List<string>();
            List<Type> listObjects = new List<Type>();
            string nameofTables = null;
            Type type = null;
            if (obj is Type)
            {
                type = obj as Type;
            }
            else
            {
                type = obj.GetType();

            }
            List<PropertyInfo> list = DetectRelationships.GetRelationships(type, toTable);
            foreach (var item in list)
            {
                listObjects.Add(item.PropertyType);
            }
            listObjects.Add(type);
            for (int i = 0; i < listObjects.Count - 1; i++)
            {
                string s = GetRelationshipTwoObjects(listObjects.ElementAt(i), listObjects.ElementAt(i + 1));
                connectStringBetweenTables.Add(s);
            }
            bool first = true;
            foreach (var type1 in listObjects)
            {
                string tb = (type1.GetCustomAttributes(typeof(SqlParameterAttribute), false)[0] as SqlParameterAttribute).PropertyName;
                if (first)
                {
                    nameofTables = tb;
                    first = false;
                }
                else
                {
                    nameofTables = nameofTables + "," + tb;
                }
            }

            string query = null;
            first = true;
            foreach (string s in connectStringBetweenTables)
            {
                if (first)
                {
                    query = s;
                    first = false;
                }
                else
                {
                    query = query + " and " + s;
                }
            }
            return new KeyValuePair<string, string>(query, nameofTables);
        }
        public string GetRelationshipTwoObjects(Type type1, Type type2)
        {
            string table = null;
            string primaryKey = null;
            string tableRef = null;
            string foreignKey = null;
            if (GetForeignKeyProperties(type1).Contains(type2.Name))
            {
                table = (type1.GetCustomAttributes(typeof(SqlParameterAttribute), false)[0] as SqlParameterAttribute).PropertyName;
                foreignKey = SqlParameterAttribute.GetNameOfParameterInSql(type1, type2.Name);
                tableRef = (type2.GetCustomAttributes(typeof(SqlParameterAttribute), false)[0] as SqlParameterAttribute).PropertyName;
                primaryKey = SqlParameterAttribute.GetNameOfParameterInSql(type2, GetPrimaryKey(type2).Name);
            }
            else
            {
                table = (type2.GetCustomAttributes(typeof(SqlParameterAttribute), false)[0] as SqlParameterAttribute).PropertyName;
                foreignKey = SqlParameterAttribute.GetNameOfParameterInSql(type2, type1.Name);
                tableRef = (type1.GetCustomAttributes(typeof(SqlParameterAttribute), false)[0] as SqlParameterAttribute).PropertyName;
                primaryKey = SqlParameterAttribute.GetNameOfParameterInSql(type1, GetPrimaryKey(type1).Name);
            }
            if (table == null || foreignKey == null || tableRef == null || primaryKey == null)
            {
                return null;
            }
            return string.Format("{0}.{1}={2}.{3}", table, foreignKey, tableRef, primaryKey);
        }
        public List<SqlParameter> CreateListSqlParamenter<T>(T obj, string foreignkeyName, int row)
        {
            Type type = obj.GetType();
            Dictionary<string, string> refConditions = GetDistinguishConditions(type, foreignkeyName);
            List<SqlParameter> list = new List<SqlParameter>();
            foreach (var item in refConditions)
            {
                string value = GetValueInCell(row, item.Value);
                list.Add(new SqlParameter(item.Key, value));
            }
            PropertyInfo foreignKey = type.GetProperty(foreignkeyName);
            Type foreignKeyTableType = foreignKey.PropertyType;
            string tableName = (foreignKeyTableType.GetCustomAttributes(typeof(SqlParameterAttribute), false)[0] as SqlParameterAttribute).PropertyName;
            Dictionary<string, string> mappingForForeignKeyTable = ExcelColumnAttribute.ColumnNamesMapping(foreignKeyTableType);
            foreach (var item in mappingForForeignKeyTable)
            {
                if (!IsForeignKey(foreignKey.PropertyType, item.Key) && !IsPrimaryKey(foreignKeyTableType, item.Key))
                {
                    string value = GetValueInCell(row, item.Value);
                    PropertyInfo p = foreignKeyTableType.GetProperty(item.Key);
                    string nameParaInSql = SqlParameterAttribute.GetNameOfParameterInSql(foreignKeyTableType, item.Key);
                    list.Add(new SqlParameter(tableName + "." + nameParaInSql, SetTypeForAProperty(p, value)));
                }
            }
            return list;
        }
        private DataTable GetDataFromSql(KeyValuePair<string, string> connectTablesString, List<SqlParameter> sqlParameters, string getWhat = "*")
        {
            // select * from table1,table2,table3 where parames;
            StringBuilder sqlQuery = new StringBuilder();
            string sParam = null;
            DataTable resultsOfSelecting = null;
            if (sqlParameters.Count <= 0)
            {
                return null;
            }
            else
            {
                foreach (SqlParameter para in sqlParameters)
                {
                    if (para.Value != null)
                    {
                        sParam += para.ParameterName + "=@" + para.ParameterName + " and ";
                    }
                    else
                    {
                        sParam += para.ParameterName + " is null and ";
                    }

                }
                sParam = sParam.Remove(sParam.Length - 5);
                sqlQuery.AppendFormat("select {0} from {1} where ({2}) and ({3})", getWhat, connectTablesString.Value, connectTablesString.Key, sParam);


                SqlDataAccess sqlDataAccess = new SqlDataAccess(connectionString);
                resultsOfSelecting = sqlDataAccess.ExecuteSelectMultiTables(sqlQuery.ToString(), sqlParameters.ToArray());
            }
            return resultsOfSelecting;
        }
        /// <summary>
        /// remove elements what have not been set value yet.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list of items what want to pre-process.</param>
        /// <returns>A list of items what doesnt contain default elements.</returns>
        private List<T> PreProcess<T>(List<T> list)
        {
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
        /// Check an object is null or not.
        /// </summary>
        /// <param name="obj">The object want to check.</param>
        /// <returns></returns>
        private bool AllOfParameterIsNull(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            bool flag = false;
            foreach (PropertyInfo p in properties)
            {
                if (!CheckDefaultValue(obj, p))
                {
                    flag = true;
                }
            }
            return flag ? false : true;
        }
        /// <summary>
        /// Check a property is default.
        /// </summary>
        /// <param name="obj">Object contains property needs to check.</param>
        /// <param name="p">property want to check.</param>
        /// <returns>true if the value in property is default.</returns>
        private bool CheckDefaultValue(object obj, PropertyInfo p)
        {
            if (p.PropertyType == typeof(string))
            {
                var obj2 = p.GetValue(obj, null);
                return obj2 == null;
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
        private object SetTypeForAProperty(PropertyInfo p, string value)
        {
            if (p.PropertyType == typeof(string))
            {

                return value;
            }
            else if (p.PropertyType == typeof(int))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return default(int);
                }
                return int.Parse(value);
            }
            else if (p.PropertyType == typeof(double))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return default(double);
                }
                return double.Parse(value);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Handling for properties what attributes are requred.
        /// </summary>
        /// <param name="newObj">the object will be filled by value.</param>
        /// <param name="property">the object's property will be filled by value.</param>
        /// <param name="row">The row of the recoders in excel.</param>
        private void HandleForRequiredProperty(object newObj, string property, int row)
        {
            Dictionary<string, string> columnMapping = ColumnNamesMapping(newObj.GetType());
            PropertyInfo propertyInfo = newObj.GetType().GetProperty(property);
            if (columnMapping.ContainsKey(property)) // if this property has value will get from in Excel file.
            {
                string columnName = null;
                string returnedValue = GetValueInCell(columnMapping, property, row, out columnName);

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
        private string HandleForUniqueKey(object newObj, string property, int row, out string rowName)
        {
            string key = null;
            Dictionary<string, string> columnMapping = ColumnNamesMapping(newObj.GetType());
            // read position in excel
            if (columnMapping.ContainsKey(property))
            {
                PropertyInfo propertyInfo = newObj.GetType().GetProperty(property);
                string columnName = null;
                string returnedValue = GetValueInCell(columnMapping, property, row, out columnName);
                key = returnedValue;
                if (string.IsNullOrWhiteSpace(returnedValue))
                {
                    string message = string.Format("Error at: Cell[{0},{1}] Handled: {2} Message: {3}", row, columnName, "Ignore", "Can't get value on this cell.");
                    rowName = columnName;
                    return null;
                }
                else
                {
                    propertyInfo.SetValue(newObj, returnedValue);
                }
                rowName = columnName;
                return key;
            }
            else
            {
                rowName = null;
                CloseExcelFile();
                throw new Exception("The mapping attribute of this property is not correct. : " + property);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newObj"></param>
        /// <param name="property"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool HandleForForeignKey<T>(T newObj, string property, int row)
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
                        var valueInCell = GetValueInCell(row, pair.Value);
                        parameters.Add(new SqlParameter(pair.Key, valueInCell));
                    }
                }
                else
                {
                    string valueInCell = GetValueInCell(row, propertyInExcel);
                    if (string.IsNullOrWhiteSpace(valueInCell))
                    {
                        string message = string.Format("Error at: Cell[{0},{1}] Handled: {2} Message: {3}", row, propertyInExcel, "Ignore", "Can't get value on this cell.");
                        SetErrorInfoMarkForRow(row);
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
            if (dataSetResults == null || dataSetResults.Length <= 0)
            {
                string message = string.Format("Not exist in SQL");
                return false;
            }
            else
            {
                object anonymous = Utility.ParseDataWith(propertyInfo.PropertyType, dataSetResults);
                propertyInfo.SetValueByDataType(newObj, anonymous);
                return true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        private void SetErrorInfoMarkForRow(int row)
        {
            Excel.Range range = _XlworkSheet.Range[_XlworkSheet.Cells[row, "A"], _XlworkSheet.Cells[row, _NumbOfColumns]];
            range.Interior.Color = Excel.XlRgbColor.rgbRed;
        }

        /// <summary>
        /// Get value of a foreign key in sql.
        /// </summary>
        /// <param name="idRef">the property needs to get value.</param>
        /// <param name="tableRef">the table will needs to reference to get value.</param>
        /// <param name="sqlParams">the condisions to filt.</param>
        /// <returns></returns>
        public DataSet GetForeignKeyInSQL(string idRef, string tableRef, List<SqlParameter> sqlParams)
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
                sParam = sParam.Remove(sParam.Length - 1);
                sqlQuery.AppendFormat("select * from {1} where {2}", idRef, tableRef, sParam);
                SqlDataAccess sqlDataAccess = new SqlDataAccess(connectionString);
                DataTable resultsOfSelecting = sqlDataAccess.ExecuteSelectQuery(sqlQuery.ToString(), sqlParams.ToArray());
                if (resultsOfSelecting.Count <= 0)
                {
                    return null;
                }
                else
                {
                    DataSet data = resultsOfSelecting.GetElementAt(0);
                    string result = data.Value(idRef);
                    return data;
                }
            }
        }
        /// <summary>
        /// Insert a record to sql server.
        /// </summary>
        /// <typeparam name="T">the type of Element.</typeparam>
        /// <param name="parseTo">The object needs to insert to the sql server.</param>
        /// <returns>The numbers was effected in sql server.</returns>
        private int RequestInsertToSql<T>(T parseTo)
        {
            List<string> requiredProperties = RequiredAttribute.GetRequiredPropertiesName(parseTo.GetType());
            string table = typeof(T).GetAttributeValue((SqlParameterAttribute dna) => dna.PropertyName);
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (string property in requiredProperties)
            {
                if (IsAutoIncrement(typeof(T), property))
                {

                }
                else
                {
                    string paramName = SqlParameterAttribute.GetNameOfParameterInSql(parseTo.GetType(), property);
                    if (string.IsNullOrWhiteSpace(paramName))
                    {
                        continue;
                    }
                    PropertyInfo propertyInfo = parseTo.GetType().GetProperty(property);
                    object result = propertyInfo.GetValue(parseTo);
                    if (result != null)
                    {
                        object paramValue = propertyInfo.GetValue(parseTo);
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            parameters.Add(new SqlParameter(paramName, paramValue));
                        }
                        else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                        {
                            if (paramValue != null)
                            {
                                parameters.Add(new SqlParameter(paramName, paramValue));
                            }
                          
                        }
                        else if (propertyInfo.PropertyType == typeof(double))
                        {
                            parameters.Add(new SqlParameter(paramName, paramValue));
                        }
                        else if (propertyInfo.PropertyType == typeof(bool))
                        {
                            parameters.Add(new SqlParameter(paramName, paramValue));
                        }
                        else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                        {
                            if ((DateTime)result == default(DateTime))
                            {
                                parameters.Add(new SqlParameter(paramName, null));
                            }
                            else
                            {
                                DateTime dt;
                                DateTime.TryParse(result.ToString(), out dt);

                                parameters.Add(new SqlParameter(paramName, dt));
                            }
                            
                        }
                        else if (propertyInfo.PropertyType.BaseType == typeof(Element))
                        {
                            string refId = ForeignKeyAttribute.GetRefId(typeof(T), property);
                            object data = GetPrimaryKeyValue(paramValue);
                            if (data == null || ((int)data) <= 0)
                            {
                                return -1;
                            }
                            parameters.Add(new SqlParameter(paramName, data));
                        }
                        else
                        {
                            throw new Exception("Code hasnot implemented");
                        }
                    }
                }

            }
            return CreateInsertQuery(table, parameters);
        }
        private int RequestUpdateToSql<T>(T parseTo)
        {
            List<string> requiredProperties = RequiredAttribute.GetRequiredPropertiesName(parseTo.GetType());
            string table = typeof(T).GetAttributeValue((SqlParameterAttribute dna) => dna.PropertyName);
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (string property in requiredProperties)
            {
                if (IsAutoIncrement(typeof(T), property))
                {

                }
                else
                {
                    string paramName = SqlParameterAttribute.GetNameOfParameterInSql(parseTo.GetType(), property);
                    if (string.IsNullOrWhiteSpace(paramName))
                    {
                        continue;
                    }
                    PropertyInfo propertyInfo = parseTo.GetType().GetProperty(property);
                    object result = propertyInfo.GetValue(parseTo);
                    if (result != null)
                    {
                        object paramValue = propertyInfo.GetValue(parseTo);
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            parameters.Add(new SqlParameter(paramName, paramValue));
                        }
                        else if (propertyInfo.PropertyType == typeof(int)|| propertyInfo.PropertyType == typeof(int?))
                        {
                            if (paramValue == null)
                            {
                                continue;
                            }
                            parameters.Add(new SqlParameter(paramName, paramValue));
                        }
                        else if (propertyInfo.PropertyType == typeof(double))
                        {
                            parameters.Add(new SqlParameter(paramName, paramValue));
                        }
                        else if (propertyInfo.PropertyType == typeof(bool))
                        {
                            parameters.Add(new SqlParameter(paramName, paramValue));
                        }
                        else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                        {
                            if ((DateTime)result == default(DateTime))
                            {
                                parameters.Add(new SqlParameter(paramName, null));
                            }
                            else
                            {
                                DateTime dt;
                                DateTime.TryParse(result.ToString(), out dt);

                                parameters.Add(new SqlParameter(paramName, dt));
                            }

                        }
                        else if (propertyInfo.PropertyType.BaseType == typeof(Element))
                        {
                            string refId = ForeignKeyAttribute.GetRefId(typeof(T), property);
                            object data = GetPrimaryKeyValue(paramValue);
                            if (data == null || ((int)data) <= 0)
                            {
                                return -1;
                            }
                            parameters.Add(new SqlParameter(paramName, data));
                        }
                        else
                        {
                            throw new Exception("Code hasnot implemented");
                        }
                    }
                }

            }
            var primaryKeyP = GetPrimaryKey(parseTo.GetType());
            var key = GetNameOfParameterInSql(parseTo.GetType(), primaryKeyP.Name);
            var val = primaryKeyP.GetValue(parseTo);
            return CreateUpdateQuery(table, key, (int)val, parameters);
        }
        public string CreateSelectQuery()
        {
            StringBuilder sqlQuery = new StringBuilder();
            string tableCol = "regions";
            string parentRegionId = "region_parent_id";
            string regionId = "region_id";
            string regionLevel = "region_level";
            string regionName = "region_name";

            return null;

        }
        /// <summary>
        /// Insert data into a table.
        /// </summary>
        /// <param name="table">The name of table wants to insert into.</param>
        /// <param name="sqlParams">The list of parameters will insert.</param>
        /// <returns></returns>
        public int CreateInsertQuery(string table, List<SqlParameter> sqlParams)
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

            SqlDataAccess sqlDataAccess = new SqlDataAccess(connectionString);
            return sqlDataAccess.ExecuteInsertOrUpdateQuery(sqlQuery.ToString(), sqlParams.ToArray());
        }
        public int CreateUpdateQuery(string table, string key, int keyVal, List<SqlParameter> sqlParams)
        {
            if (sqlParams.Count <= 0)
            {
                return -1;
            }
            string sValues = null;
            foreach (SqlParameter para in sqlParams)
            {
                sValues += para.ParameterName + "=@" + para.ParameterName + ",";
            }
            sValues = sValues.Remove(sValues.Length - 1);
            string whereStatement = $"{key} = {keyVal}";
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.AppendFormat("update {0} set {1} where {2}", table, sValues, whereStatement);

            SqlDataAccess sqlDataAccess = new SqlDataAccess(connectionString);
            return sqlDataAccess.ExecuteInsertOrUpdateQuery(sqlQuery.ToString(), sqlParams.ToArray());
        }
        private string GetValueInCell(Dictionary<string, string> columnMap, string property, int row, out string columnName)
        {
            if (columnMap.TryGetValue(property, out columnName))
            {
                string s = null;
                try
                {
                    Excel.Range cell = _XlworkSheet.Cells[row, columnName];
                    if (cell.Value != null)
                    {
                        s = _XlworkSheet.Cells[row, columnName].Value.ToString();

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
        /// <summary>
        /// Get value in a cell in excel.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The value in the cell[row,column]</returns>
        private void SetValueInCell(int row, string columnName, object value)
        {
            try
            {
                Excel.Range cell = _XlworkSheet.Cells[row, columnName];
                _XlworkSheet.Cells[row, columnName].Value2 = value.ToString();
                _XlworkSheet.Cells[row, columnName].Interior.Color = Excel.XlRgbColor.rgbDarkGreen;


            }
            catch (Exception)
            {

            }
        }
        private string GetValueInCell(int row, string columnName)
        {
            try
            {
                Excel.Range cell = _XlworkSheet.Cells[row, columnName];
                if (cell.Value != null)
                {
                    return _XlworkSheet.Cells[row, columnName].Value.ToString();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Clode the file what is running.
        /// </summary>
        private void CloseExcelFile()
        {
            _XlWorkBook.Save();
            _XlWorkBook.Close();
            _XlApplication.Quit();
        }
    }
}
