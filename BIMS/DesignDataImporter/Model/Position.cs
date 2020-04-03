using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataUtilities.Model;

namespace DesignDataImporter.Model
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
        [SqlParameter("position_id"), PrimaryKey, Required, AutoIncrement, ExcelTemporaryStorage("CO")]
        public int Id { get; set; }
        [SqlParameter("name"), Required, ExcelColumn("AB")]
        public string Name { get; set; }

        [SqlParameter("latitude"), Required, ExcelColumn("Z")]
        public double Latitude { get; set; }

        [SqlParameter("longitude"), Required, ExcelColumn("AA")]
        public double Longitude { get; set; }
        [Required, 
         ExcelColumn("G"), 
         SqlParameter("region_id"), 
         ForeignKey("regions", "regions.zip_code[Y]=>region_id")]
        public JapanRegion Region
        {
            get;
            set;
        }
    }
}
