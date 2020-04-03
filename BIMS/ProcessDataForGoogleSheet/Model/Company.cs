using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDataForGoogleSheet.Model
{
    public class Company
    {
        [Required, ExcelColumn("A")]
        public string Name
        {
            get;
            set;
        }
        [Required, ExcelColumn("B")]
        public string Address
        {
            get;
            set;
        }
        [Required, ExcelColumn("C")]
        public string Phone
        {
            get;
            set;
        }
        [Required, ExcelColumn("D")]
        public string StructureSystem
        {
            get;
            set;
        }
        [Required, ExcelColumn("E")]
        public string OrderReceived
        {
            get;
            set;
        }
        [Required,  ExcelColumn("F")]
        public string Executed
        {
            get;
            set;
        }
        [Required, ExcelColumn("G")]
        public string RequestForQuotation
        {
            get;
            set;
        }
        [Required, ExcelColumn("AG")]
        public string Place1
        {
            get;
            set;
        }
        [Required, ExcelColumn("AH")]
        public string Place2
        {
            get;
            set;
        }
        [Required, ExcelColumn("AI")]
        public string Place3
        {
            get;
            set;
        }
        [Required, ExcelColumn("H,I,J,K,L,M")]
        public List<string> Employee
        {
            get;
            set;
        }
        public override string ToString()
        {

            return Name + " - " + Address;
        }
    }
}
