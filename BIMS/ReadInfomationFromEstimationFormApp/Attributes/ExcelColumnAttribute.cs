using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ReadInfomationFromEstimationFormApp.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // only use this attribute for properties.
    public class ExcelColumnAttribute : Attribute
    {
        private readonly string _Name;
        public ExcelColumnAttribute(string columnName)
        {
            _Name = columnName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public static Dictionary<string,string> ColumnNamesMapping(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<string, string> requiredPropaties = new Dictionary<string, string>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ExcelColumnAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    string value = ((ExcelColumnAttribute)attributes[0]).Name.ToString();
                    requiredPropaties.Add(property.Name, value);
                }
            }
            return requiredPropaties;
        }
        public static Dictionary<string, string> ColumnNamesMappingByOrder(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<string, string> requiredPropaties = new Dictionary<string, string>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ExcelColumnAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    string value = ((ExcelColumnAttribute)attributes[0]).Name.ToString();
                    requiredPropaties.Add(property.Name, value);
                }
            }
            return requiredPropaties;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public static Dictionary<string, string> ColumnNamesMapping(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<string, string> requiredPropaties = new Dictionary<string, string>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ExcelColumnAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    string value = ((ExcelColumnAttribute)attributes[0]).Name.ToString();
                    requiredPropaties.Add(property.Name, value);
                }
            }
            return requiredPropaties;
        }
        public static int GetNumbOfColumnsToRead(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ExcelColumnAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    string value = ((ExcelColumnAttribute)attributes[0]).Name.ToString();
                    return value.Split(',').Length;
                }
            }
            return 0;
        }

        public static Dictionary<string, string[]> GetNameOfColumnsInExcel(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ExcelColumnAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    string value = ((ExcelColumnAttribute)attributes[0]).Name.ToString();
                    dic.Add(property.Name, value.Split(','));
                }
            }
            return dic;
        }

        public string Name
        {
            get
            {
                return _Name;
            }
        }
    }
}
