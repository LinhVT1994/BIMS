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
         SqlParameter("construction_id"), 
         ExcelTemporaryStorage("CP")]
        public int ConstructionId
        {
            get;
            set;
        }
        [Required, 
         Unique, 
         SqlParameter("construction_no"),  
         ExcelColumn("A")]
        public string ConstructionNo
        {
            get;
            set;
        }

        [Required, SqlParameter("started_day"), ExcelColumn("E")]
        public DateTime StartDate
        {
            get;
            set;
        }
        [Required, SqlParameter("finished_day"), ExcelColumn("F")]
        public DateTime FinishedDay
        {
            get;
            set;
        }
        [Required, SqlParameter("name"), ExcelColumn("C")]
        public string Name
        {
            get;
            set;
        }
        [Required, SqlParameter("status")]
        public int Status
        {
            get;
            set;
        }
        [Required, SqlParameter("position_id"), ExcelColumn("CO")]
        public int PositionId
        {
            get;
            set;
        }
    }
}
