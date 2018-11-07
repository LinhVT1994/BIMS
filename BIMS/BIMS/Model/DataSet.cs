using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
    /**
    * This DataSet class to support for parse data from a reader object in Postgresql.
    * 
    *
    * @author  LinhVT
    * @version 1.0
    * @since   2018/11/6
    */
    class DataSet
    {
        private Dictionary<string, string> dictionary = null;
        public DataSet()
        {
            dictionary = new Dictionary<string, string>();
        }
        public void SetParameter(string name, string value)
        {
            dictionary.Add(name, value);
        }
        public DataSet SetParameters(Dictionary<string,string> dic)
        {
            dictionary = dic;
            return this;
        }
        public int Length
        {
            get
            {
                return dictionary.Count;
            }
        }
        public string Value(string name)
        {
            return dictionary[name];
        }
        public string KeyAt(int index)
        {
            return dictionary.ElementAt(index).Key;
        }
        public string ValueAt(int index)
        {
            return dictionary.ElementAt(index).Value;
        }
    }
}
