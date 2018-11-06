using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // only use this attribute for properties.
    class MappingForeignKeyAttribute : Attribute
    {

        private readonly string _Propertyname;
        private readonly string _ForeignKey;
        public MappingForeignKeyAttribute(string foreignKey , string propertyName)
        {
            _Propertyname = propertyName;
            _ForeignKey = foreignKey;
        }
        /// <summary>
        /// Get all of properties that is required.
        /// </summary>
        /// <param name="obj"></param>
        public static List<string> GetMappingForeignKeyAttribute(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> requiredPropaties = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(RequiredAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    requiredPropaties.Add(property.Name); // add a attribute in the required properties.
                }
            }
            return requiredPropaties;
        }
        
        public static bool IsRequired(Type type, string name)
        {
            List<string> requiredProperties = GetMappingForeignKeyAttribute(type);
            return requiredProperties.Contains(name);
        }
        public static KeyValuePair<string, string> GetMapping4ForeignKey(Type type, string propertyName)
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
                        string refProperty = (attributes[0] as MappingForeignKeyAttribute).PropertyName.ToString();
                        string foreignKey = (attributes[0] as MappingForeignKeyAttribute).ForeignKey.ToString();
                        KeyValuePair<string, string> pair = new KeyValuePair<string, string>(foreignKey, refProperty);
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
        public string PropertyName
        {
            get
            {
                return _Propertyname;
            }
        }
        public string ForeignKey
        {
            get
            {
                return _ForeignKey;
            }
        }
    }
}
