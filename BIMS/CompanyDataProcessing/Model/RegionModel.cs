using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDataProcessing.Model
{
    [SqlParameter("regions")]
    public class RegionModel
    {
        [ExcelColumn("L"), Required]
        public string FullAddress { get; internal set; }

        [ExcelColumn("P"),AutoIncrement,Required,SqlParameter("region_id")]
        public int RegionId
        {
            get;
            set;
        }
        [Required, ExcelColumn("M"),SqlParameter("zip_code")]
        public string Zipcode
        {
            get;
            set;
        }
        [Required, ExcelColumn("N")] // V, W ,X,Y  ,Z
        public string Latitude
        {
            get;
            set;
        }
        [Required, ExcelColumn("O")] // V, W ,X,Y  ,Z
        public string Longitude
        {
            get;
            set;
        }
    }
}
