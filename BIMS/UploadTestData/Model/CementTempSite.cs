using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadTestData.Model
{
    [SqlParameter("cement")]
    public class CementTempSite
    {
        private int _Cement_Id;
        private string _Symbol;
        private string _Name;
        public CementTempSite()
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
        [Required, Unique, ExcelColumn("AO"), SqlParameter("symbol")]
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
        [Required, SqlParameter("name")]
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
    }
}
