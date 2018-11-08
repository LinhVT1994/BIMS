using BIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
    /**
    * @author  LinhVT
    * @version 1.0
    * @since   2018/11/6
    */
    [SqlParameter("position")]
    class Position : Element
    {
        private int    _Position_Id;
        private string _Name;
        private string _Latitude;
        private string _Longitude;
        public Position()
        {
        }
        public Position(int id, string name, string latitude = null, string longitude = null)
        {
            Position_Id = id;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
        #region properties
        [Required, AutoIncrement, PrimaryKey, SqlParameter("position_id")]
        public int Position_Id
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
        [Required, Unique, ExcelColumn("G"), SqlParameter("name")]
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

        [Required, SqlParameter("latitude")]
        public string Latitude
        {
            get
            {
                return _Latitude;
            }
            set
            {
                _Latitude = value;
            }
        }
        [Required, SqlParameter("longitude")]
        public string Longitude
        {
            get
            {
                return _Longitude;
            }
            set
            {
                _Longitude = value;
            }
        }
        #endregion
    }
}
