using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadTestData.Model
{
    [SqlParameter("cement")]
    public class CementTemp
    {
        private int _Cement_Id;
        private string _Symbol;
        private string _Name;
        public CementTemp()
        {
            Cement_Id = -1;
            Symbol = null;
            Name = null;
        }
        [Required, PrimaryKey, AutoIncrement, SqlParameter("cement_id")]
        public int Cement_Id
        {
            get
            {
                return _Cement_Id;
            }
            set
            {
                _Cement_Id = value;
            }
        }
        [Required, Unique, ExcelColumn("C"), SqlParameter("symbol")]
        public string Symbol
        {
            get
            {
                return _Symbol;
            }
            set
            {
                _Symbol = value;
            }
        }
        [Required, ExcelColumn("B"), SqlParameter("name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        [Required, ExcelColumn("D"), SqlParameter("maker")]
        public string Maker
        {
            get;
            set;
        }
        [Required, ExcelColumn("E"), SqlParameter("classification")]
        public string Classification
        {
            get;
            set;
        }

        [Required, ExcelColumn("G"), SqlParameter("strength")]
        public string Strength
        {
            get;
            set;
        }

        [Required, ExcelColumn("H"), SqlParameter("hexavalent_chronium")]
        public string HexavalentChronium
        {
            get;
            set;
        }
    }
}
