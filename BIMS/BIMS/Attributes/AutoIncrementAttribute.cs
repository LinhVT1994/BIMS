using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BIMS.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // only use this attribute for properties.
    class AutoIncrementAttribute : Attribute
    {
        public AutoIncrementAttribute()
        {
        }
        public static List<string> GetAutoIncrementProperties(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> requiredPropaties = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(AutoIncrementAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    requiredPropaties.Add(property.Name); // add a attribute in the required properties.
                }
            }
            return requiredPropaties;
        }
        public static bool IsAutoIncrement(Type type, string name)
        {
            List<string> requiredProperties = GetAutoIncrementProperties(type);
            return requiredProperties.Contains(name);
        }
    }
}
