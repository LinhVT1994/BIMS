using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("construction")]
    public class Constuction
    {
        [PrimaryKey, 
         AutoIncrement, Required,
         ExcelColumn("A"),
         SqlParameter("construction_id")]
        public int ConstructionId
        {
            get;
            set;
        }
        [Required, 
         Unique, 
         SqlParameter("construction_no"),  
         ExcelColumn("B")]
        public string ConstructionNo
        {
            get;
            set;
        }

        [Required, SqlParameter("started_day"), ExcelColumn("C")]
        public DateTime StartDate
        {
            get;
            set;
        }
        [Required, SqlParameter("finished_day"), ExcelColumn("D")]
        public DateTime FinishedDay
        {
            get;
            set;
        }
        [Required, SqlParameter("status"), ExcelColumn("E")]
        public int Status
        {
            get;
            set;
        }
        [Required, SqlParameter("name"), ExcelColumn("F")]
        public string Name
        {
            get;
            set;
        }
        [Required, SqlParameter("area"), ExcelColumn("L")]
        public double Area
        {
            get;
            set;
        }
        [Required, SqlParameter("price"), ExcelColumn("M")]
        public double Price
        {
            get;
            set;
        }
        [Required, SqlParameter("position_id"), ExcelColumn("G")]
        public int PositionId
        {
            get;
            set;
        }

        [Required,
        ExcelColumn("I"),
        SqlParameter("structure_type_id")]
        public int StructureTypeId { get; set; }

        [Required,
         ExcelColumn("J"),
         SqlParameter("scale_id")]
        public int ScaleId
        {
            get;
            set;
        }
        [Required,
         ExcelColumn("H"),
         SqlParameter("purpose_id")]
        public int PurposeId
        {
            get;
            set;
        }
    }
}
