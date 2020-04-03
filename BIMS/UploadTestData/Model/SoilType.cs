using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadTestData.Model
{
    [SqlParameter("soil_type")]
    public class SoilType
    {
        [Required, PrimaryKey, AutoIncrement, SqlParameter("soil_type_id")]
        public int Id
        {
            get;
            set;
        }
        [Required,Unique,ExcelColumn("L"), SqlParameter("cement_id")]
        public string Symbol
        {
            get;
            set;
        }
        [Required, ExcelColumn("L"), SqlParameter("name")]
        public string Name
        {
            get;
            set;
        }
        
    }
}
