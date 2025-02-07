﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataUtilities.Attributes
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class)]
    public class SqlParameterAttribute : Attribute
    {
        private string _PropertyName = null;
        public SqlParameterAttribute(string name)
        {
            name = name.ToLower();
            this._PropertyName = name;
        }

        public static string GetNameOfParameterInSql(Type type,string propertyName)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Trim().ToLower().Equals(propertyName.Trim().ToLower()))
                {
                    Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(SqlParameterAttribute), false); // get the attributes of a property.
                    if (attributes.Length > 0)
                    {
                        string value = ((SqlParameterAttribute)attributes[0]).PropertyName.ToString();
                        return value;
                    }
                }
            }
            return null;
        }
        public string PropertyName
        {
            get
            {
                return _PropertyName;
            }
        }

    }
}
