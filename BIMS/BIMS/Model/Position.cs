using BIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
    class Position
    {
        private int _Position_Id;
        private string _Name;
        private string _Latitute;
        private string _Longitute;
        public Position()
        {
        }
        public Position(int id, string name, string latitute = null, string longitute = null)
        {
            Potition_Id = id;
            Name = name;
            Latitute = latitute;
            Longitute = longitute;
        }

        #region properties
        [AutoIncrement]
        [Required]
        public int Potition_Id
        {
            get
            {
                return _Position_Id;
            }
            set
            {
                _Position_Id = value;
            }
        }
        [Required]
        [Unique]
        [ExcelColumn("G")]
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

        [Required]
        public string Latitute
        {
            get
            {
                return _Latitute;
            }
            set
            {
                _Latitute = value;
            }
        }

        [Required]
        public string Longitute
        {
            get
            {
                return _Longitute;
            }
            set
            {
                _Longitute = value;
            }
        }
        #endregion
    }
}
