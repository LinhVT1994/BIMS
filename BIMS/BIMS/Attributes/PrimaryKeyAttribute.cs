using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute()
        {
        }
        public static List<string> GetPrimaryKeyProperties(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> requiredPropaties = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(PrimaryKeyAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    requiredPropaties.Add(property.Name); // add a attribute in the required properties.
                }
            }
            return requiredPropaties;
        }
        public static bool IsPrimaryKey(Type type, string name)
        {
            List<string> requiredProperties = GetPrimaryKeyProperties(type);
            return requiredProperties.Contains(name);
        }
    }
}