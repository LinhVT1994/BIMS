using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataUtilities.Attributes;

namespace UpdatePositionInfo2Excel.Model
{
    public class JapanRegion
    {
        [SqlParameter("id"), PrimaryKey, Required, AutoIncrement]
        public int Id { get; set; }

        [SqlParameter("postoffice"), Required, Unique, ExcelColumn("Y")]
        public string Postoffice { get; set; }

        [SqlParameter("latitude"), Required, ExcelColumn("H")]
        public double Latitude { get; set; }

        [SqlParameter("longitude"), Required, ExcelColumn("I")]
        public double Longitude { get; set; }
    }
}
