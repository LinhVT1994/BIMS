using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMS.Attributes;
namespace BIMS.Model
{
    /**
    * A Cement object what contains infomation relate to the cement.
    * 
    *
    * @author  LinhVT
    * @version 1.0
    * @since   2018/11/6
    */
    [SqlParameter("cement")]
    class Cement : Element
    {
        private int    _Cement_Id;
        private string _Symbol;
        private string _Name;
        public　Cement()
        {
            Cement_Id = -1;
            Symbol = null;
            Name = null;
        }
        [Required,AutoIncrement, SqlParameter("cement_id")]
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
        [Required, Unique, ExcelColumn("I"), SqlParameter("symbol")]
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
