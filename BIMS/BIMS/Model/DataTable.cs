using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
    /**
    * This DataTable class to support for parse data from a reader object in Postgresql.
    * 
    *
    * @author  LinhVT
    * @version 1.0
    * @since   2018/11/6
    */
   class DataTable
    {
        private readonly List<DataSet> data = null;
        public DataTable()
        {
            data = new List<DataSet>();
        }
        /// <summary>
        /// Fill a DataTable object by a NpgsqlDataReader object.
        /// </summary>
        /// <param name="reader">NpgsqlDataReader object what wants to parse data.</param>
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
        public List<DataSet> GetAllRecords()
        {
            return data;
        }
        /// <summary>
        /// Get all of columns in a row.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <returns>A DataSet object contains the result returned.</returns>
        public DataSet GetElementAt(int row)
        {
             return data.ElementAt(row);
        }
        /// <summary>
        /// Amount of elements in the result returned.
        /// </summary>
        public int Count
        {
            get
            {
                return data.Count;
            }
        }
    }
}
