using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static BIMS.Attributes.AutoIncrementAttribute;
using static BIMS.Attributes.UniqueAttribute;
using static BIMS.Attributes.ExcelColumnAttribute;
using static BIMS.Attributes.ForeignKeyAttribute;
using static BIMS.Attributes.PrimaryKeyAttribute;

namespace BIMS.Utilities
{
    class DetectRelationships
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
