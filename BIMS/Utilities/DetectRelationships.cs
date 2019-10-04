using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static DataUtilities.Attributes.AutoIncrementAttribute;
using static DataUtilities.Attributes.UniqueAttribute;
using static DataUtilities.Attributes.ExcelColumnAttribute;
using static DataUtilities.Attributes.ForeignKeyAttribute;
using static DataUtilities.Attributes.PrimaryKeyAttribute;

namespace DataUtilities
{
    public class DetectRelationships
    {
        private  Stack<PropertyInfo> stack = new Stack<PropertyInfo>();
        private  List<PropertyInfo> results = new List<PropertyInfo>();
        private void Execute(Type type, string condition)
        {
            List<string> foreignkeys = GetForeignKeyProperties(type);
            if (foreignkeys.Count <= 0 || type.Name.ToLower().Equals(condition.ToLower()))
            {
                return;
            }
            foreach (var item in GetForeignKey(type))
            {
                stack.Push(item);
                Execute(item.PropertyType, condition);

                if (item.Name.ToLower().Equals(condition.ToLower())) // tra ve list kq
                {
                    foreach (var v in stack)
                    {
                        results.Add(v);
                    }
                    break;
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        public static List<PropertyInfo>  GetRelationships(Type type, string condition)
        {
            DetectRelationships detec = new DetectRelationships();
            detec.Execute(type, condition);
            return detec.results;
        }

    }
}
