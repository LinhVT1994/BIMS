using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("super_structure")]
    public class SuperStructureModel
    {
        [SqlParameter("super_structure_id"),PrimaryKey,Required,AutoIncrement]
        public int SuperStructureId
        {
            get;
            set;
        }

        [SqlParameter("construction_detail_id"),
         Required, 
         ExcelColumn("CC")]
        public int ConstructionDetailId
        {
            get;
            set;
        }
        [SqlParameter("amount_steel_in_grand_beam"), Required, ExcelColumn("AH")]
        public double AmountSteelInGrandBeam
        {
            get;
            set;
        }
        [SqlParameter("amount_steel_in_small_beam"), Required, ExcelColumn("AI")]
        public double AmountSteelInSmallBeam
        {
            get;
            set;
        }
        [SqlParameter("amount_steel_in_column"), Required, ExcelColumn("AJ")]
        public double AmountSteelInColumn
        {
            get;
            set;
        }
        public double SumOfAmountSteel
        {
            get
            {
                return AmountSteelInColumn + AmountSteelInGrandBeam + AmountSteelInSmallBeam;
            }
        }
    }
}
