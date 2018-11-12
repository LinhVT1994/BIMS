using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using static BIMS.Attributes.AutoIncrementAttribute;
using static BIMS.Attributes.UniqueAttribute;
using static BIMS.Attributes.ExcelColumnAttribute;
using static BIMS.Attributes.DistinguishAttribute;
using static BIMS.Attributes.ForeignKeyAttribute;
using static BIMS.Attributes.PrimaryKeyAttribute;
using static BIMS.Attributes.PropertyInfoExtensions;
using BIMS.Attributes;
using System.Reflection;
using BIMS.Model;

namespace BIMS.Utilities
{
    class ExcelToSqlManipulationEdition
    {
        private string _Url = @"C:\Users\TUAN-LINH\Desktop\TestData.xlsx";
        // private static string url = @"C:\Users\VuLin\Desktop\TestData.xlsx";
        private Excel.Application _XlApplication = null;
        private Excel.Worksheet _XlworkSheet = null;
        Excel.Workbook _XlWorkBook = null;
        private int _StartRowInExcel = 5;
        private Excel.Range _XlRange;
        private int _NumbOfRows = 0;
        private int _NumbOfColumns = 0;
        private ExcelToSqlManipulationEdition(string url)
        {
            _Url = url;
            try
            {
                _XlApplication = new Excel.Application();
                _XlApplication.Visible = false;
                _XlApplication.DisplayAlerts = false;
                _XlWorkBook = _XlApplication.Workbooks.Open(url);
                _XlworkSheet = (Excel.Worksheet)_XlWorkBook.Sheets[1];
                _XlworkSheet.Unprotect();
                _XlRange = _XlworkSheet.UsedRange;
                _NumbOfRows = _XlRange.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell).Row;
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
        public static ExcelToSqlManipulationEdition CreateInstance(string url)
        {
            ExcelToSqlManipulationEdition newOne = null;
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            try
            {
                newOne = new ExcelToSqlManipulationEdition(url);
            }
            catch (ArgumentException)
            {
                newOne = null;
            }
            return newOne;

        }
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
                        if (!IsAutoIncrement(typeof(T), pName))
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
                                LoggingHelper.WriteDown(message);
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    else if(IsForeignKey(typeof(T), pName)) // is foreign key.
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
                        RequestToSql<T>(newObj);
                    }
                }
            }
            CloseExcelFile();
        }

        public bool ExecuteMultiRecords<T>()
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
                                string valueInCell = GetValueInCell(row, columnsInExcel[index]);
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
                // update for foreign key properties.
                SetRelationshipsForObjects<T>(listNewObjects,row);
                // insert all of elements in the list to the sql.

            }
            // close the excel app.
            CloseExcelFile();
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
                HandleForeignKeyPropertiesOfObject<T>(item,row);
            }
        }
        private void HandleForeignKeyPropertiesOfObject<T>(T obj,int row)
        {
            Type type = obj.GetType();
            foreach (var foreignkeyName in GetForeignKeyProperties(type))
            {
                List<string> refTables = GetDistinguishTables(type, foreignkeyName);
                if (refTables == null || refTables.Count <= 0)
                {
                    // dealing like a nomal foreign key.
                    HandleForForeignKey<T>(obj, foreignkeyName , row);
                }
                else
                {
                    PropertyInfo propertyInfo = obj.GetType().GetProperty(foreignkeyName);
                    List<string> refTablesRelateWith = GetDistinguishTables(type, foreignkeyName);
                    Dictionary<string, string> refConditions = GetDistinguishConditions(type, foreignkeyName);
                    List<SqlParameter> sqlParameters = CreateListSqlParamenter(refConditions, row);
                    Dictionary<string, string> a = GetExcelColumnReferences(type, foreignkeyName);
                    KeyValuePair<string, string> tableRelationInfo =   CreateRelationshipsInTableSqlFromObjects(propertyInfo.PropertyType, refTablesRelateWith.ElementAt(0));
                    GetDataFromSql(tableRelationInfo, sqlParameters);


                    string refId = GetRefId(type, foreignkeyName);
                    string refTable = GetRefTable(type, foreignkeyName);
                }
            }
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
            for (int i = 0; i < listObjects.Count -1; i++)
            {
                 string s =  GetRelationshipTwoObjects(listObjects.ElementAt(i), listObjects.ElementAt(i+1));
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
                primaryKey = SqlParameterAttribute.GetNameOfParameterInSql(type2,GetPrimaryKey(type2).Name);
            }
            else
            {
                table = (type2.GetCustomAttributes(typeof(SqlParameterAttribute), false)[0] as SqlParameterAttribute).PropertyName;
                foreignKey = SqlParameterAttribute.GetNameOfParameterInSql(type2, type1.Name);
                tableRef = (type1.GetCustomAttributes(typeof(SqlParameterAttribute), false)[0] as SqlParameterAttribute).PropertyName;
                primaryKey = SqlParameterAttribute.GetNameOfParameterInSql(type1, GetPrimaryKey(type1).Name);
            }
            if (table==null|| foreignKey == null || tableRef == null || primaryKey == null)
            {
                return null;
            }
            return string.Format("{0}.{1}={2}.{3}", table, foreignKey, tableRef, primaryKey);
        }
        public List<SqlParameter> CreateListSqlParamenter(Dictionary<string, string> refConditions, int row)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            foreach (var item in refConditions)
            {
                string value = GetValueInCell(row, item.Value);
                list.Add(new SqlParameter(item.Key, value));
            }
            return list;
        }
        private DataSet GetDataFromSql(KeyValuePair<string,string> connectTablesString, List<SqlParameter> sqlParameters, string getWhat = "*")
        {
            // select * from table1,table2,table3 where parames;
            StringBuilder sqlQuery = new StringBuilder();
            string sParam = null;

            if (sqlParameters.Count <= 0)
            {
                return null;
            }
            else
            {
                foreach (SqlParameter para in sqlParameters)
                {
                    sParam += para.ParameterName + "=@" + para.ParameterName + " and ";
                }
                sParam = sParam.Remove(sParam.Length - 5);
                sqlQuery.AppendFormat("select {0} from {1} where ({2}) and ({3})",getWhat, connectTablesString.Value, connectTablesString.Key, sParam);
                SqlDataAccess sqlDataAccess = new SqlDataAccess();
                var resultsOfSelecting = sqlDataAccess.ExecuteSelectMultiTables(sqlQuery.ToString(), sqlParameters.ToArray());
              
            }
            return null;

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
        private  bool CheckDefaultValue(object obj, PropertyInfo p)
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
        /// <summary>
        /// Handling for properties what attributes are requred.
        /// </summary>
        /// <param name="newObj">the object will be filled by value.</param>
        /// <param name="property">the object's property will be filled by value.</param>
        /// <param name="row">The row of the recoders in excel.</param>
        private void HandleForRequiredProperty(object newObj, string property,int row)
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
        private string HandleForUniqueKey(object newObj,string property,int row,out string rowName)
        {
            string key = null;
            Dictionary<string, string> columnMapping = ColumnNamesMapping(newObj.GetType());
            // read position in excel
            if (columnMapping.ContainsKey(property))
            {
                PropertyInfo propertyInfo = newObj.GetType().GetProperty(property);
                string columnName = null;
                string returnedValue = GetValueInCell(columnMapping,property, row,out columnName);
                key = returnedValue;
                if (string.IsNullOrWhiteSpace(returnedValue))
                {
                    string message = string.Format("Error at: Cell[{0},{1}] Handled: {2} Message: {3}", row, columnName, "Ignore", "Can't get value on this cell.");
                    LoggingHelper.WriteDown(message);
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
                        LoggingHelper.WriteDown(message);
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
                SqlDataAccess sqlDataAccess = new SqlDataAccess();
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
         private int RequestToSql<T>(T parseTo)
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
                    PropertyInfo propertyInfo = parseTo.GetType().GetProperty(property);
                    object result = propertyInfo.GetValue(parseTo);
                    if (result != null)
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
                            throw new Exception("Code hasnot implemented");
                        }
                    }
                }
               
            }
            return CreateInsertQuery(table, parameters);
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

            SqlDataAccess sqlDataAccess = new SqlDataAccess();
            return sqlDataAccess.ExecuteInsertOrUpdateQuery(sqlQuery.ToString(), sqlParams.ToArray());
        }
        private string GetValueInCell(Dictionary<string, string> columnMap, string property, int row, out string columnName)
        {
            if(columnMap.TryGetValue(property, out columnName))
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
