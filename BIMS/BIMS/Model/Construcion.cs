using BIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
    class Construcion
    {
        private int      _Construction_Id;
        private string   _Construction_No;
        private string   _Name;
        private Position _Position;

        public Construcion()
        {

        }
        [AutoIncrement, PrimaryKey, Required]
        public　int Construction_Id
        {
            get
            {
                return _Construction_Id;
            }
            set
            {
                _Construction_Id = value;
            }
        }
        [Required, Unique, ExcelColumn("E")]
        public string Construction_No
        {
            get
            {
                return _Construction_No;
            }
            set
            {
                _Construction_No = value;
            }
        }
        [Required, ExcelColumn("F")]
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
        [Required, ExcelColumn("G"), ForeignKey("position", "position_id","name")]
        public Position Position {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
            }
        }

    }
}
