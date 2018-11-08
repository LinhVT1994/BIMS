using BIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Attributes
{
    static class PropertyInfoExtensions
    {
        public static void SetValueByDataType(this PropertyInfo propertyInfo, object obj,object value)
        {
            if (propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(obj, value.ToString());
            }
            else if (propertyInfo.PropertyType == typeof(int))
            {
                propertyInfo.SetValue(obj, int.Parse(value.ToString()));
            }
            else if (propertyInfo.PropertyType == typeof(double))
            {
                double db;
                double.TryParse(value.ToString(), out db);
                propertyInfo.SetValue(obj, db);
            }
            else
            {
               propertyInfo.SetValue(obj, value);

            }
        }


    }
}
