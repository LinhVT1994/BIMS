using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BIMS.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // only use this attribute for properties.
    class ExcelColumnAttribute : Attribute
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
        public string Name
        {
            get
            {
                return _Name;
            }
        }
    }
}
