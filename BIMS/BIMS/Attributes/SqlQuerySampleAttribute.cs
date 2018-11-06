using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    class SqlQuerySampleAttribute : Attribute
    {
        private string sample = null;
        public SqlQuerySampleAttribute(string sample, string para)
        {
            this.sample = sample;
        }

        public static string GetQuery(Type type,string propertyName)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Trim().Equals(propertyName.Trim().ToLower()))
                {
                    Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(SqlQuerySampleAttribute), false); // get the attributes of a property.
                    if (attributes.Length > 0)
                    {
                        string value = ((SqlQuerySampleAttribute)attributes[0]).Sample.ToString();
                        return value;
                    }
                }
            }
            return null;
        }
        public static void RunAQuery(Type type,string methodName)
        {
             string sqlQuery = GetQuery(type,methodName); 

        }
        public string Sample
        {
            get
            {
                return sample;
            }
        }

    }
}
