using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataUtilities.Attributes
{
    public class ForeignKeyAttribute : Attribute
    {
        private Dictionary<string, string> _MappingWithExcelColumn = null;
        private string _RefId = null;
        private string _RefTableName = null;
        public ForeignKeyAttribute(string refTable, string mappingOperator)
        {
            _MappingWithExcelColumn = new Dictionary<string, string>();
            string[] array = mappingOperator.Split(new string[] { "=>" }, StringSplitOptions.None);

            if (array == null || array.Length != 2)
            {
                throw new Exception("Is not a correct syntax." + mappingOperator);
            }
            string mappingKeys = array[0];
            foreach (var key in mappingKeys.Split(','))
            {
                var pair = Parse(key);
                if (!pair.Equals(default(KeyValuePair<string, string>)))
                {
                    _MappingWithExcelColumn.Add(pair.Key, pair.Value);
                }
            }
            _RefId = array[1];
            _RefTableName = refTable;

        }

        public static List<string> GetForeignKeyProperties(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> requiredPropaties = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ForeignKeyAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    requiredPropaties.Add(property.Name); // add a attribute in the required properties.
                }
            }
            return requiredPropaties;
        }
        public static List<PropertyInfo> GetForeignKey(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<PropertyInfo> requiredPropaties = new List<PropertyInfo>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ForeignKeyAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    requiredPropaties.Add(property); // add a attribute in the required properties.
                }
            }
            return requiredPropaties;
        }
        public static bool IsForeignKey(Type type, string name)
        {
            List<string> requiredProperties = GetForeignKeyProperties(type);
            return requiredProperties.Contains(name);
        }
        public Dictionary<string, string> MappingWithExcelColumn
        {
            get
            {
                return _MappingWithExcelColumn;
            }
        }
        public string RefId
        {
            get
            {
                return _RefId;
            }
        }
        public static Dictionary<string, string> GetExcelColumnReferences(Type type, string propertyName)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> requiredPropaties = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals(propertyName))
                {
                    Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ForeignKeyAttribute), false); // get the attributes of a property.
                    if (attributes.Length > 0)
                    {
                        return ((ForeignKeyAttribute)attributes[0]).MappingWithExcelColumn;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }
        public static string GetRefId(Type type, string propertyName)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> requiredPropaties = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals(propertyName))
                {
                    Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ForeignKeyAttribute), false); // get the attributes of a property.
                    if (attributes.Length > 0)
                    {
                        return ((ForeignKeyAttribute)attributes[0]).RefId;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            return null;
        }
        public static string GetRefTable(Type type, string propertyName)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> requiredPropaties = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals(propertyName))
                {
                    Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ForeignKeyAttribute), false); // get the attributes of a property.
                    if (attributes.Length > 0)
                    {
                        return ((ForeignKeyAttribute)attributes[0])._RefTableName;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            return null;
        }
        private KeyValuePair<string, string> Parse(string s)
        {
            s = s.TrimEnd(']');
            string[] array = s.Split('[');
            if (array.Length == 2)
            {
                return new KeyValuePair<string, string>(array[0], array[1]);
            }
            return default(KeyValuePair<string, string>);

        }
    }
}