using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateExcelFileForVNTakeuchi.Models
{
/*
select construction_no as "construction_no",con.started_day, con.finished_day,con.name as "name", zipcode,prefecture,wardOrCity,area, pos_full.name as detail_of_position, pos_full.latitude,pos_full.longitude
from construction con
inner join 
(select position_id, zipcode, prefecture,wardOrCity,area, pos.name, pos.latitude, pos.longitude from position pos
inner join(select area_tb.region_id,prefecture_tb.region_name as prefecture,ward_tb.region_name as wardOrCity,area_tb.region_name as area,area_tb.zip_code as zipcode
from regions as area_tb
	inner join regions as ward_tb
	On area_tb.region_parent_id = ward_tb.region_id
	inner join regions as prefecture_tb 
	On ward_tb.region_parent_id = prefecture_tb.region_id) as fullregion
On fullregion.region_id = pos.region_id) as pos_full
On pos_full.position_id = con.position_id;
*/
    class GeneralConstructionInfoModel
    {
        [SqlParameter("construction_id"), PrimaryKey, Required, AutoIncrement]
        public string Id
        {
            get;
            set;
        }
        [SqlParameter("construction_no"), Required, ExcelColumn("A")]
        public string No
        {
            get;
            set;
        }
        [Required, SqlParameter("name"), ExcelColumn("B")]
        public string Name
        {
            get;
            set;
        }
        [Required, SqlParameter("started_day"), ExcelColumn("C")]
        public string StartedDay
        {
            get;
            set;
        }
        [Required, SqlParameter("finished_day"), ExcelColumn("D")]
        public string FinishedDay
        {
            get;
            set;
        }
        [Required, SqlParameter("zipcode"), ExcelColumn("I")]
        public string Zipcode
        {
            get;
            set;
        }
        [Required, SqlParameter("prefecture"), ExcelColumn("E")]
        public string Prefecture
        {
            get;set;
        }
        [Required, SqlParameter("wardOrCity"), ExcelColumn("F")]
        public string WardOrCity
        {
            get; set;
        }
        [Required, SqlParameter("area"), ExcelColumn("G")]
        public string Area
        {
            get; set;
        }
        [Required, SqlParameter("detail_of_position"), ExcelColumn("H")]
        public string detail_of_position
        {
            get; set;
        }
        [Required, SqlParameter("latitude"), ExcelColumn("K")]
        public string latitude
        {
            get; set;
        }
        [Required, SqlParameter("longitude"), ExcelColumn("J")]
        public string longitude
        {
            get; set;
        }

    }
}
