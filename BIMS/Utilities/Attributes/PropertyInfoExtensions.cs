using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataUtilities.Model;

namespace DataUtilities.Attributes
{
    public static class PropertyInfoExtensions
    {
        public static void SetValueByDataType(this PropertyInfo propertyInfo, object obj, object value)
        {

            if (value == null)
            {
                propertyInfo.SetValue(obj, null);
            }
            else if (propertyInfo.PropertyType.BaseType == typeof(Element)) // 
            {
                if (value.GetType().BaseType == typeof(Element))
                {
                    propertyInfo.SetValue(obj,value);
                }
                else
                {
                    var newOne = Activator.CreateInstance(propertyInfo.PropertyType);
                    PropertyInfo keyInfo = PrimaryKeyAttribute.GetPrimaryKey(newOne.GetType());
                    keyInfo.SetValueByDataType(newOne, value);
                    propertyInfo.SetValue(obj, newOne);
                }
            }
            else
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(obj, value.ToString());
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    string s = (value.GetType() == typeof(string)) ? (string)value : value.ToString();
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        propertyInfo.SetValue(obj, default(int));
                    }
                    else
                    {
                        int val;
                        if (int.TryParse(s, out val))
                        {
                            propertyInfo.SetValue(obj, val);
                        }
                    }
                }
                else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                {
                    DateTime dt;
                    if (DateTime.TryParse(value.ToString(), out dt))
                    {
                        propertyInfo.SetValue(obj, dt);
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, null);
                    }
                }
                else if (propertyInfo.PropertyType == typeof(bool))
                {
                    var temp = value.ToString();
                    if (temp.Contains("●") || temp.Contains("○"))
                    {
                        propertyInfo.SetValue(obj, true);
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, false);
                    }
                   
                }
                else if (propertyInfo.PropertyType == typeof(double))
                {
                    double db;
                    double.TryParse(value.ToString(), out db);
                    propertyInfo.SetValue(obj, db);
                }
                else
                {
                    propertyInfo.SetValue(obj, null);
                }
            }
        }
    }
}
