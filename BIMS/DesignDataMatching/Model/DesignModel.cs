using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataMatching.Model
{
    /*
     * CREATE TABLE DesignModel(
        id SERIAL,
        symbol varchar(20),
        business_suporter varchar(30),
	    construction_name varchar(200),
	    full_address varchar(500),
	    contractor varchar(500),
	    start_date varchar(100),
	    finish_date varchar(100),
	    implementation_area double precision,
	    amount_of_money double precision,
    	owner varchar(30),
	    partner varchar(500),
	    partner1 varchar(500),
	    partner2 varchar(500),
	    partner3 varchar(500),
	    partner4 varchar(500),
	    partner5 varchar(500),
	    purpose varchar(100),
	    scale varchar(100),
    	volume double precision,
	    structure varchar(100),
	   constraint design_model_Key primary key(id)
    );
     * */
     [SqlParameter("DesignModel")]
    public class DesignModel
    {
        [SqlParameter("id"), AutoIncrement, PrimaryKey]
        public int Id
        {
            get;
            set;
        }

        [SqlParameter("symbol"), ExcelColumn("A"),Required, Unique]
        public string Symbol
        {
            get;
            set;
        }
        [SqlParameter("business_suporter"), Required, ExcelColumn("C")]
        public string BusinessSuporter
        {
            get;
            set;
        }
      

        [SqlParameter("construction_name"), Required, ExcelColumn("E")]
        public string ConstructionName
        {
            get;
            set;
        }
        [SqlParameter("full_address"), Required, ExcelColumn("G")]
        public string FullAddress
        {
            get;
            set;
        }
        [SqlParameter("contractor"), Required, ExcelColumn("H")]
        public string Contractor
        {
            get;
            set;
        }
        [SqlParameter("start_date"), Required, ExcelColumn("I")]
        public string StartDate
        {
            get;
            set;
        }
        [SqlParameter("finish_date"), Required, ExcelColumn("K")]
        public string FinishDate
        {
            get;
            set;
        }
        [SqlParameter("implementation_area"), Required, ExcelColumn("L")]
        public double ImplementationArea
        {
            get;
            set;
        }
        [SqlParameter("amount_of_money"), Required, ExcelColumn("M")]
        public double AmountOfMoney
        {
            get;
            set;
        }
        [SqlParameter("owner"), Required, ExcelColumn("N")]
        public string Owner
        {
            get;
            set;
        }

        [SqlParameter("partner"), Required, ExcelColumn("O")]
        public string Partner
        {
            get;
            set;
        }

        [SqlParameter("partner1"), Required, ExcelColumn("P")]
        public string Partner1
        {
            get;
            set;
        }

        [SqlParameter("partner2"), Required, ExcelColumn("Q")]
        public string Partner2
        {
            get;
            set;
        }


        [SqlParameter("partner3"), Required, ExcelColumn("R")]
        public string Partner3
        {
            get;
            set;
        }
        [SqlParameter("partner4"), Required, ExcelColumn("S")]
        public string Partner4
        {
            get;
            set;
        }
        [SqlParameter("partner5"), Required, ExcelColumn("T")]
        public string Partner5
        {
            get;
            set;
        }
        [SqlParameter("purpose"), Required, ExcelColumn("AD")]
        public string Purpose
        {
            get;
            set;
        }
        [SqlParameter("scale"), Required, ExcelColumn("AE")]
        public string Scale
        {
            get;
            set;
        }
        [SqlParameter("volume"), Required, ExcelColumn("AF")]
        public double Volume
        {
            get;
            set;
        }

        [SqlParameter("structure"), Required, ExcelColumn("AG")]
        public string Structure
        {
            get;
            set;
        }
    }
}
