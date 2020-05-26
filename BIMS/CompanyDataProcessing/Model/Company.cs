
using DataUtilities.Attributes;
using DataUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDataProcessing.Model
{
    /*
     *CREATE TABLE "company"(
	        company_id SERIAL,
	        name varchar(200),	 -- 会社名
	        position_id int,
            email varchar(200),	-- 住所名前
	        phone  varchar(12),         -- 携帯電話番号 081804228150アドレス
	        is_my_company boolean,
	        constraint pk_company primary key("company_id")
        );
     * 
     */
    [SqlParameter("company_table1")]
    public class Company: Element
    {
        [Required, PrimaryKey, AutoIncrement, SqlParameter("id")]
        public int Id
        {
            get;
            set;
        }

        [Required, ExcelColumn("B"), SqlParameter("name")] // V, W ,X,Y  ,Z
        public string Name
        {
            get;
            set;
        }

        [Required, ExcelColumn("C"), SqlParameter("prefecture")] // V, W ,X,Y  ,Z
        public string Prefecture
        {
            get;
            set;
        }
        [Required, ExcelColumn("G"), SqlParameter("phone")] // V, W ,X,Y  ,Z
        public string Phone
        {
            get;
            set;
        }

        [Required, ExcelColumn("D"), SqlParameter("city")] // V, W ,X,Y  ,Z
        public string City
        {
            get;
            set;
        }
        [Required, ExcelColumn("E"), SqlParameter("district")] // V, W ,X,Y  ,Z
        public string Distric
        {
            get;
            set;
        }
        [Required, ExcelColumn("F"), SqlParameter("detail")] // V, W ,X,Y  ,Z
        public string Detail
        {
            get;
            set;
        }

        [Required, ExcelColumn("L"), SqlParameter("address")] // V, W ,X,Y  ,Z
        public string Address
        {
            get;
            set;
        }
        [Required, ExcelColumn("N"), SqlParameter("latitide")] // V, W ,X,Y  ,Z
        public string Latitude
        {
            get;
            set;
        }
        [Required, ExcelColumn("O"), SqlParameter("longitude")] // V, W ,X,Y  ,Z
        public string Longitude
        {
            get;
            set;
        }
        [Required, ExcelColumn("M"), SqlParameter("zip_code")] // V, W ,X,Y  ,Z
        public string Zipcode
        {
            get;
            set;
        }

        [ ExcelColumn("P")] // V, W ,X,Y  ,Z
        public int RegionId
        {
            get;
            set;
        }
    }
    [SqlParameter("company_table2")]
    public class Company2 : Element
    {
        [Required, PrimaryKey, AutoIncrement, SqlParameter("id")]
        public int Id
        {
            get;
            set;
        }

        [Required, ExcelColumn("B"), Unique, SqlParameter("name")] // V, W ,X,Y  ,Z
        public string Name
        {
            get;
            set;
        }
        [Required, ExcelColumn("G"), SqlParameter("phone")] // V, W ,X,Y  ,Z
        public string Phone
        {
            get;
            set;
        }

        [Required, ExcelColumn("C"), SqlParameter("prefecture")] // V, W ,X,Y  ,Z
        public string Prefecture
        {
            get;
            set;
        }
        [Required, ExcelColumn("D"), SqlParameter("city")] // V, W ,X,Y  ,Z
        public string City
        {
            get;
            set;
        }
        [Required, ExcelColumn("E"), SqlParameter("district")] // V, W ,X,Y  ,Z
        public string Distric
        {
            get;
            set;
        }
        [Required, ExcelColumn("F"), SqlParameter("detail")] // V, W ,X,Y  ,Z
        public string Detail
        {
            get;
            set;
        }

        [Required, ExcelColumn("L"), SqlParameter("address")] // V, W ,X,Y  ,Z
        public string Address
        {
            get;
            set;
        }
        [Required, ExcelColumn("N"), SqlParameter("latitide")] // V, W ,X,Y  ,Z
        public string Latitude
        {
            get;
            set;
        }
        [Required, ExcelColumn("O"), SqlParameter("longitude")] // V, W ,X,Y  ,Z
        public string Longitude
        {
            get;
            set;
        }
        [Required, ExcelColumn("M"), SqlParameter("zip_code")] // V, W ,X,Y  ,Z
        public string Zipcode
        {
            get;
            set;
        }

        [ExcelColumn("P")] // V, W ,X,Y  ,Z
        public int RegionId
        {
            get;
            set;
        }
    }
}
