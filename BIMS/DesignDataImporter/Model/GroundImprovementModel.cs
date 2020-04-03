﻿using DataUtilities.Attributes;
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
        [Required, AutoIncrement, PrimaryKey, SqlParameter("ground_improvement_id")]
        public int GroundImprovementId
        {
            get;
            set;
        }
        [Required, SqlParameter("construction_detail_id"), ExcelColumn("CQ")]
        public int ConstructionDetailId
        {
            get;
            set;
        }
        [Required,SqlParameter("improvement_area"), ExcelColumn("BG")]
        public double ImprovementArea
        {
            get;
            set;
        }
        [Required, SqlParameter("target_strength"), ExcelColumn("BH")]
        public double TagetStrength
        {
            get;
            set;
        }

        [Required,SqlParameter("first_soil_layer_thickness"), ExcelColumn("BK")]
        public double FirstSoilLayerThickness
        {
            get;
            set;
        }
        [Required, SqlParameter("second_soil_layer_thickness"), ExcelColumn("BI")]
        public double SecondSoilLayerThickness
        {
            get;
            set;
        }
    }
}
