using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Attributes
{
     [AttributeUsage(AttributeTargets.Property)]
    class ForeignKeyAttribute : Attribute
    {
        private readonly string _ReferenceTable;
        private readonly string _ReferenceProperty;
        private readonly string _MappingProperty;
        public ForeignKeyAttribute(string refTable, string nameOfRefProperty, string mappingProperty)
        {
            _ReferenceTable = refTable;
            _ReferenceProperty = nameOfRefProperty;
            _MappingProperty = mappingProperty;
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
        public static bool IsForeignKey(Type type, string name)
        {
            List<string> requiredProperties = GetForeignKeyProperties(type);
            return requiredProperties.Contains(name);
        }
        public string MappingProperty
        {
            get
            {
                return _MappingProperty;
            }
        }
        public string ReferenceTable
        {
            get
            {
                return _ReferenceTable;
            }
        }
        public string ReferenceProperty
        {
            get
            {
                return _ReferenceProperty;
            }
        }
        public static KeyValuePair<string,string> GetReferences(Type type, string propertyName)
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
                        string refTableName = (attributes[0] as ForeignKeyAttribute).ReferenceTable.ToString();
                        string refProperty = (attributes[0] as ForeignKeyAttribute).ReferenceProperty.ToString();
                        KeyValuePair<string, string> pair = new KeyValuePair<string, string>(refTableName, refProperty);
                        return pair;
                    }
                    else
                    {
                        return default(KeyValuePair<string, string>);
                    }
                }
                
            }
            return default(KeyValuePair<string, string>);
        }

        public static string GetMappingProperty(Type type, string propertyName)
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
                        string mappingProperty = (attributes[0] as ForeignKeyAttribute).MappingProperty.ToString();
                        return mappingProperty;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            return null;
        }
    }
}
