using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataUtilities.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // only use this attribute for properties.
    public class UniqueAttribute : Attribute
    {
        public UniqueAttribute()
        {

        }
        public static List<string> GetUniqueProperties(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> requiredPropaties = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(UniqueAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    requiredPropaties.Add(property.Name); // add a attribute in the required properties.
                }
            }
            return requiredPropaties;
        }
        public static bool IsUnique(Type type, string name)
        {
            List<string> requiredProperties = GetUniqueProperties(type);
            return requiredProperties.Contains(name);
        }
    }
}
