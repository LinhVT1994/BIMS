using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    class DistinguishAttribute : Attribute
    {
        public List<string> table = null;
        public Dictionary<string,string> conditions = null;
        //[construction.construction_no(E),testing_sample.name(X),testing_sample.Color(Y),testing_sample.description(M)]
        public DistinguishAttribute(string s)
        {
            s = s.TrimStart('[');
            s = s.TrimEnd(']');
            string[] array = s.Split(',');
            if (array.Length==0)
            {
                throw new AggregateException("Error format.");
            }
            table = new List<string>();
            conditions = new Dictionary<string,string>();

            foreach (var item in array)
            {
                table.Add(item.Split('.')[0]);
                string  item2 = item.Trim(')');
                var temp = item2.Split('(');
                conditions.Add(temp[0],temp[1]);
            }
        }
        public static List<string> GetDistinguishTables(Type type,string propertyName)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> table = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                if (propertyName.ToLower().Equals(property.Name.ToLower()))
                {
                    Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(DistinguishAttribute), false); // get the attributes of a property.
                    if (attributes.Length > 0)
                    {
                        return (attributes[0] as DistinguishAttribute).table;
                    }
                }
            }
            return null;
        }
        public static List<PropertyInfo> GetDistinguishProperties(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<PropertyInfo> result = new List<PropertyInfo>();
            List<string> table = new List<string>();
            foreach (PropertyInfo property in properties)
            {
               
                    Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(DistinguishAttribute), false); // get the attributes of a property.
                    if (attributes.Length > 0)
                    {
                         result.Add(property);
                    }
            }
            return result;
        }
        public static Dictionary<string, string> GetDistinguishConditions(Type type,string propertyName)
        {
            PropertyInfo[] properties = type.GetProperties();
            List<string> table = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                if (propertyName.ToLower().Equals(property.Name.ToLower()))
                {
                    Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(DistinguishAttribute), false); // get the attributes of a property.
                    if (attributes.Length > 0)
                    {
                        return (attributes[0] as DistinguishAttribute).conditions;
                    }
                }
            }
            return null;
        }

    }
}
