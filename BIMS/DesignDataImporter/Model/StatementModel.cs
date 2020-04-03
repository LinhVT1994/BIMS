using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("statement")]
    public class StatementModel
    {
        [SqlParameter("statement_id"), PrimaryKey, AutoIncrement, Required]
        public int StatementId {get;set;}
        [SqlParameter("construction_detail_id"), Required ,ExcelColumn("CQ")]
        public int ConstructionDetailId { get; set; }
        [SqlParameter("liquefaction"),Required, ExcelColumn("CC")]
        public bool Liquefaction
        {
            get;
            set;
        }

        [SqlParameter("flmin"), Required, ExcelColumn("CD")]
        public double Fmin
        {
            get;
            set;
        }
        [SqlParameter("consolidation_settlement"), Required, ExcelColumn("CE")]
        public bool ConsolidationSettlement
        {
            get;
            set;
        }

    }
}
