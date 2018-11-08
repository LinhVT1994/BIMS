using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // only use this attribute for properties.
    class RequiredAttribute : Attribute
    {
        public RequiredAttribute()
        {

        }
        /// <summary>
        /// Get all of properties that is required.
        /// </summary>
        /// <param name="obj"></param>
        public static List<string> GetRequiredPropertiesName(Type type)
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

        public static bool IsRequired(Type type,string name)
        {
            List<string> requiredProperties = GetRequiredPropertiesName(type);
            return requiredProperties.Contains(name);
        }
    }
}
