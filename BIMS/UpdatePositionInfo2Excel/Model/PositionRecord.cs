using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatePositionInfo2Excel.Model
{
    [SqlParameter("position")]
    public class PositionRecord
    {
        [SqlParameter("id"), PrimaryKey, Required, AutoIncrement]
        public int Id { get; set; }
        [SqlParameter("prefecture"), Required, Unique, ExcelColumn("C")]
        public string Prefecture { get; set; }

        [SqlParameter("city"), Required, Unique, ExcelColumn("D")]
        public string City { get; set; }
        [SqlParameter("district"), Required, Unique, ExcelColumn("E")]
        public string District { get; set; }

        [SqlParameter("address_detail"), Required, Unique, ExcelColumn("F")]
        public string AddressDetail { get; set; }
        [SqlParameter("full_address"), Required, Unique, ExcelColumn("F")]
        public string FullAddress { get; set; }

        [SqlParameter("postoffice"), Required, Unique, ExcelColumn("H")]
        public string Postoffice { get; set; }

    }
}
