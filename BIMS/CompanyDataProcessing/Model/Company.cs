
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
    [SqlParameter("company")]
    public class Company: Element
    {
        [Required, PrimaryKey, AutoIncrement, SqlParameter("company_id")]
        public int Id
        {
            get;
            set;
        }
        [Required, ExcelColumn("Z"), Unique,SqlParameter("name")] // V, W ,X,Y  ,Z
        public string Name
        {
            get;
            set;
        }
    }
}
