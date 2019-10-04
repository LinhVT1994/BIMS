using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataUtilities.Attributes;
using static DataUtilities.Attributes.AutoIncrementAttribute;
using static DataUtilities.Attributes.UniqueAttribute;
using static DataUtilities.Attributes.ExcelColumnAttribute;
using static DataUtilities.Attributes.ForeignKeyAttribute;
using System.Reflection;
using DataUtilities.Attributes;
using DataUtilities.Data;

namespace DataUtilities
{
    /**
    * Utility class contains methods what is usually used.
    * 
    *
    * @author  LinhVT
    * @version 1.0
    * @since   2018/11/6
    */
    public class Utility
    {
        private static Stopwatch watch = null;
        private static string message = null;
        public static void StartCountingTime(string message)
        {
            Utility.message = message;
            if (watch == null)
            {
                watch = System.Diagnostics.Stopwatch.StartNew();
            }
            if (!watch.IsRunning)
            {
                watch.Start();
            }
        }
        public static object ParseDataWith(Type type,DataSet dataSet)
        {
            object anonymous = (object)Activator.CreateInstance(type);
            List<PropertyInfo> propertiesInfo= RequiredAttribute.GetRequiredProperties(type);
            foreach (PropertyInfo propertyInfo in propertiesInfo)
            {
                string key = SqlParameterAttribute.GetNameOfParameterInSql(type, propertyInfo.Name);
                string value = dataSet.Value(key.ToLower());
                if (value!=null)
                {
                    propertyInfo.SetValueByDataType(anonymous, value);
                }
            }
            return anonymous;

        }
        public static object SetDefaultValue(object obj)
        {
            return null;
        }
        public static void StopCountingTime()
        {
            if (watch.IsRunning)
            {
                var elapsedMs = watch.ElapsedMilliseconds;
                Debug.WriteLine("   Message: " + Utility.message+", Time to run: " + elapsedMs);
                watch.Stop();
            }
        }
    }
}
