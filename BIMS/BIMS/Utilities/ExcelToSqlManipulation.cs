using BIMS.Attributes;
using BIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Utilities
{
    class ExcelToSqlManipulation
    {
        private static string url = @"C:\Users\TUAN-LINH\Desktop\TestData.xlsx";
        public static bool Execute<T>()
        {
            ExcelDataAccess reader = ExcelDataAccess.GetInstance();
            Dictionary<string, T> excelData = reader.Read<T>(url);
            foreach (var row in excelData)
            {
                 T temp =  row.Value;
                List<string> requiredParams = null;
                PropertyInfo[] propertiesInfo = AttributeUtilities.GetProperties(temp);
                foreach (PropertyInfo property in propertiesInfo)
                {
                    // get all the requred parameters
                    requiredParams = RequiredAttribute.GetRequiredProperties(temp.GetType());
                    if (requiredParams != null)
                    {
                       
                    }
                    else
                    {
                        throw new Exception("Don't have any required parameters");
                    }
                }

            }
            return false;
        }
    }
}
