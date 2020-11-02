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
        [SqlParameter("statement_id"),ExcelTemporaryStorage("CI"), PrimaryKey, AutoIncrement, Required]
        public int StatementId {get;set;}
        [SqlParameter("construction_detail_id"), Required, ExcelColumn("CC")]
        public int ConstructionDetailId { get; set; }

        [SqlParameter("liquefaction"),Required]
        public bool Liquefaction
        {
            get;
            set;
        }

        [SqlParameter("flmin"), Required, ExcelColumn("BO")]
        public double Fmin
        {
            get;
            set;
        }
        [SqlParameter("consolidation_settlement"), Required]
        public bool ConsolidationSettlement
        {
            get;
            set;
        }
        private string liquefactionRaw;
        [Required, ExcelColumn("BN")]
        public string LiquefactionRaw
        {
            get
            {
                return liquefactionRaw;
            }
            set
            {
                liquefactionRaw = value;
                if (!string.IsNullOrWhiteSpace(liquefactionRaw) && liquefactionRaw.Contains("○"))
                {
                    Liquefaction = true;
                }
            }
        }
        private string consolidationSettlementRaw;
        [Required, ExcelColumn("BP")]
        public string ConsolidationSettlementRaw
        {
            get
            {
                return consolidationSettlementRaw;
            }
            set
            {
                consolidationSettlementRaw = value;
                if (!string.IsNullOrWhiteSpace(consolidationSettlementRaw) && consolidationSettlementRaw.Contains("○"))
                {
                    ConsolidationSettlement = true;
                }
            }
        }
    }
}
