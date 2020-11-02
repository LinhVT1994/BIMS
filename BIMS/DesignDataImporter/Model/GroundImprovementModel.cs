using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("ground_improvement")]
    public class GroundImprovementModel
    {
        [Required, 
            AutoIncrement, 
            PrimaryKey,
            ExcelTemporaryStorage("CH")
            SqlParameter("ground_improvement_id")]
        public int GroundImprovementId
        {
            get;
            set;
        }
        [Required, SqlParameter("construction_detail_id"), 
            ExcelColumn("CC")]
        public int ConstructionDetailId
        {
            get;
            set;
        }
        [Required,SqlParameter("improvement_area"), ExcelColumn("AR")]
        public double ImprovementArea
        {
            get;
            set;
        }
        [Required, SqlParameter("target_strength"), ExcelColumn("AS")]
        public double TagetStrength
        {
            get;
            set;
        }

        [Required,SqlParameter("first_soil_layer_thickness"), ExcelColumn("AT")]
        public double FirstSoilLayerThickness
        {
            get;
            set;
        }
        [Required, SqlParameter("second_soil_layer_thickness"), ExcelColumn("AV")]
        public double SecondSoilLayerThickness
        {
            get;
            set;
        }
    }
}
