using ReadInfomationFromEstimationFormApp.Attributes;
using ReadInfomationFromEstimationFormApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReadInfomationFromEstimationFormApp
{
    public class Utilities
    {
        public static int CountNullProperties(Models.EstimationForm data)
        {
            var type = typeof(EstimationForm);
            var properties = RequiredAttribute.GetRequiredPropertiesName(type);
            int count = 0;
            foreach (var p in properties)
            {
                var property = type.GetProperty(p);
                var val = property.GetValue(data);
                if (val == null)
                {
                    count = count + 1;
                }
            }
            return count;
        }
        public static bool WriteToTextFile(Models.EstimationForm data)
        {
            var requiredProperties = RequiredAttribute.GetRequiredPropertiesName(typeof(EstimationForm));
            var dicOfCol = ExcelColumnAttribute.ColumnNamesMapping(data).OrderBy(p=>p.Value.Count()).ThenBy(p =>p.Value);
     
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("result_data.txt", true))
            {
                string sResult = "";
                
                foreach (var item in dicOfCol)
                {
                    var excelColumn = item.Value;
                    PropertyInfo pInfo = typeof(EstimationForm).GetProperty(item.Key);
                    var rs = pInfo.GetValue(data);
                    sResult = sResult + rs + ";";
                  
                }
                file.WriteLine(sResult);
            }
            return true;
        }
        public static bool WriteBlankLine()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("result_data.txt", true))
            {
                file.WriteLine("BreakLine");
            }
            return true;
        }
    }
}
