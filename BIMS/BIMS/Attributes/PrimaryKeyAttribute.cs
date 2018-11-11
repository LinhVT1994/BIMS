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
        public static PropertyInfo GetPrimaryKey(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> requiredPropaties = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(PrimaryKeyAttribute), false); // get the attributes of a property.
                if (attributes.Length > 0)
                {
                    return property;
                }
            }
            return null;
        }
        public static object GetPrimaryKeyValue(Object obj)
        {
            PropertyInfo key =  GetPrimaryKey(obj.GetType());
            return key.GetValue(obj);
        }
        public static bool IsPrimaryKey(Type type, string name)
        {
            PropertyInfo primaryKey = GetPrimaryKey(type);
            if (name.ToLower().Equals(primaryKey.Name.ToLower()))
            {
                return true;
            }
            return false;
        }
    }
}