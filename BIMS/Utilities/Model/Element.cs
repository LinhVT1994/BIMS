using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUtilities.Model
{
    public class Element : DynamicObject
    {
        // The inner dictionary.
        Dictionary<string, object> dictionary
            = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name.ToLower();
            return dictionary.TryGetValue(name, out result);
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            dictionary[binder.Name.ToLower()] = value;

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }

    }
}
