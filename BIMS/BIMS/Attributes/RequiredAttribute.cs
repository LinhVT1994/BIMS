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
        public static List<string> GetRequiredProperties(Type type)
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

        public static bool IsRequired(Type type,string name)
        {
            List<string> requiredProperties = GetRequiredProperties(type);
            return requiredProperties.Contains(name);
        }
    }
}
