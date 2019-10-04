using BIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
    /*
     CREATE TABLE "construction_log"(
	id SERIAL,      
	name varchar(200) not null,
	purpose  varchar(200),
	executed_date varchar(200),
	area  double precision,
	volume double precision,
	scale varchar(200),
	structure varchar(200),
	method  varchar(200),
	address varchar(500),
	proximity_sea boolean,
	proximity_bay boolean,
	constraint pk_test_logs primary key(id)
    );

         */
    [SqlParameter("construction_log")]

    class ExecutedConstruction
    {
        [Required, AutoIncrement, PrimaryKey, SqlParameter("id")]
        public int Id
        {
            get;set;
        }
        [Required, Unique,SqlParameter("name"), ExcelColumn("A")]
        public string Name
        {
            get; set;
        }
        [Required, SqlParameter("purpose"), ExcelColumn("O")]
        public string Purpose
        {
            get; set;
        }
        [Required, SqlParameter("executed_date"), ExcelColumn("D")]
        public string ExecutedDate
        {
            get; set;
        }
        [Required, SqlParameter("area"), ExcelColumn("F")]
        public double Area
        {
            get; set;
        }

        [Required, SqlParameter("volume"), ExcelColumn("G")]
        public double Volume
        {
            get; set;
        }
        [Required, SqlParameter("scale"), ExcelColumn("H")]
        public string Scale
        {
            get; set;
        }
        [Required, SqlParameter("structure"), ExcelColumn("I")]
        public string Structure
        {
            get; set;
        }
        [Required, SqlParameter("method"), ExcelColumn("J")]
        public string Method
        {
            get; set;
        }

        [Required, SqlParameter("address"), ExcelColumn("N")]
        public string Address
        {
            get; set;
        }
        [Required, SqlParameter("proximity_sea"), ExcelColumn("L")]
        public bool ProximitySea
        {
            get; set;
        }
        [Required, SqlParameter("proximity_bay"), ExcelColumn("M")]
        public bool ProximityBay
        {
            get; set;
        }
    }
}
