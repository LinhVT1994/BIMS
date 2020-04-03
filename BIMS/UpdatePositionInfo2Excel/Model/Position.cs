using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataUtilities.Attributes;
using DataUtilities.Model;

namespace UpdatePositionInfo2Excel.Model
{
    /* TempDatabase
     * CREATE TABLE position(
        id SERIAL,
        postoffice varchar(20),
        latitude double precision,
        longitude double precision,
        constraint pk_region primary key(id)
    );
     * */
    [SqlParameter("position")]
    public class Position : Element
    {
        [SqlParameter("id"),PrimaryKey, Required, AutoIncrement]
        public int Id { get; set; }
        
        [SqlParameter("postoffice"),Required, Unique,ExcelColumn("Y")]
        public string Postoffice { get; set; }

        [SqlParameter("latitude"), Required, ExcelColumn("Z")]
        public double Latitude { get; set; }

        [SqlParameter("longitude"), Required, ExcelColumn("AA")]
        public double Longitude { get; set; }
    }
}
