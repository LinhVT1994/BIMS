using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
    class DataTable
    {
        private readonly List<DataSet> data = null;
        public DataTable()
        {
            data = new List<DataSet>();
        }
        public void Fill(NpgsqlDataReader reader)
        {
            while (reader.Read())
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string name = reader.GetName(i);
                    string value = reader.GetValue(i).ToString();
                    item.Add(name, value);
                }
                data.Add(new DataSet().SetParameters(item));
            }
        }
        public DataSet GetElementAt(int row)
        {
             return data.ElementAt(row);
        }
        public int Count
        {
            get
            {
                return data.Count;
            }
        }
    }
}
