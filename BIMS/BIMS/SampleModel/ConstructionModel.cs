using BIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.SampleModel
{
    public class ConstructionModel
    {
        [Required, AutoIncrement, PrimaryKey, SqlParameter("id")]
        public int Id
        {
            get; set;
        }
        [Required, Unique, SqlParameter("name"), ExcelColumn("C")]
        public string Name
        {
            get; set;
        }
    }
}
