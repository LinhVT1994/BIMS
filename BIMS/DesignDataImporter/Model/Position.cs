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
        [SqlParameter("position_id"), PrimaryKey, Required, AutoIncrement, ExcelTemporaryStorage("BG")]
        public int Id { get; set; }

        [Required, ExcelColumn("A")]
        public string ConstructioNo { get; set; }

        [SqlParameter("name"), Required, ExcelColumn("U")]
        public string Name { get; set; }

        [SqlParameter("latitude"), Required, ExcelColumn("N")]
        public double Latitude { get; set; }

        [SqlParameter("longitude"), Required, ExcelColumn("O")]
        public double Longitude { get; set; }
        [Required, 
         ExcelColumn("L"), 
         SqlParameter("region_id"), 
         ForeignKey("regions", "zip_code[L]=>region_id")]
        public JapanRegion Region
        {
            get;
            set;
        }
    }
}
