using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetPartyDataFromDesignData
{
    public class CompanyArr
    {
        [ExcelColumn("R"), Required]
        public string Company1 { get; set; }
        [ExcelColumn("S"), Required]
        public string EmOfCompany1 { get; set; }
        [ExcelColumn("T"), Required]
        public string PhoneOfCompany1 { get; set; }
        [ExcelColumn("U"), Required]
        public string AddressOfCompany1 { get; set; }
        [ExcelColumn("V"), Required]
        public string EmailOfEmp1 { get; set; }

        [ExcelColumn("W"), Required]
        public string Company2 { get; set; }
        [ExcelColumn("X"), Required]
        public string EmOfCompany2 { get; set; }
        [ExcelColumn("Y"), Required]
        public string PhoneOfCompany2 { get; set; }
        [ExcelColumn("Z"), Required]
        public string AddressOfCompany2 { get; set; }
        [ExcelColumn("AA"), Required]
        public string EmailOfEmp2 { get; set; }

        [ExcelColumn("AB"), Required]
        public string Company3 { get; set; }
        [ExcelColumn("AC"), Required]
        public string Company4 { get; set; }

        [ExcelColumn("AD"), Required]
        public string Company5 { get; set; }
        [ExcelColumn("AE"), Required]
        public string Company6 { get; set; }
    }
}
