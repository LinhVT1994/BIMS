﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataUtilities.Attributes
{
    public class AttributeUtilities
    {
        public static PropertyInfo[] GetProperties(object obj)
        {
            Type type = obj.GetType();
            return type.GetProperties();
        }
    }
}
