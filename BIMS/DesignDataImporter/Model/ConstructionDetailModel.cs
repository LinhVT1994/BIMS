using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("construction_detail")]
    public class ConstructionDetailModel
    {
        [Required, 
         AutoIncrement, 
         PrimaryKey,
         ExcelTemporaryStorage("CQ")
         SqlParameter("construction_detail_id")]
        public int ConstructionDetailId
        {
            get;
            set;
        }

        [Required,
         SqlParameter("construction_id"), 
         ExcelColumn("CP")]
        public int ConstructionId
        {
            get;
            set;
        }
        [Required,
         ExcelColumn("AE"),
         SqlParameter("structure_type_id"),
         ForeignKey("structure_type", "name[AE]=>structure_type_id")]
        public StructureTypeModel StructureType
        {
            get;
            set;
        }

        [Required,
         ExcelColumn("AG"),
         SqlParameter("scale_id"),
         ForeignKey("scale", "name[AG]=>scale_id")]
        public ScaleModel ScaleModel
        {
            get;
            set;
        }
        [Required,
         ExcelColumn("AI"),
         SqlParameter("purpose_id"),
         ForeignKey("purpose", "name[AI]=>purpose_id")]
        public PurposeModel PurposeModel
        {
            get;
            set;
        }

        [Required,
         ExcelColumn("AJ"),
         SqlParameter("movable_loading")]
        public double MovableLoading
        {
            get;
            set;
        }

        [Required,
         ExcelColumn("AK"),
         SqlParameter("rooftop_id"),
         ForeignKey("rooftop", "name[AK]=>rooftop_id")]
        public RooftopModel RooftopModel
        {
            get;
            set;
        }

        [Required,
        ExcelColumn("AL"),
         SqlParameter("design_route_id"),
        ForeignKey("design_route", "name[AL]=>design_route_id")]
        public DesignRouteModel DesignRoute
        {
            get;
            set;
        }

        [Required,
        ExcelColumn("AM"),
        SqlParameter("snowfall_amount")]
        public double SnowFallAmount
        {
            get;set;
        }
        [Required,
         ExcelColumn("AN"),
         SqlParameter("total_floor_area")]
        public double TotalFloorArea
        {
            get; set;
        }

        [Required,
        ExcelColumn("AO"),
        SqlParameter("total_construction_area")]
        public double TotalConstructionArea
        {
            get; set;
        }

        [Required,
         ExcelColumn("BM"),
         SqlParameter("platform")]
        public bool Flatform
        {
            get; set;
        }
        [Required,
         ExcelColumn("BN"),
         SqlParameter("crushed_stone")]
        public bool CrushedStone
        {
            get; set;
        }
        [Required,
         SqlParameter("ocr_min")]
        public double OCRMin
        {
            get; set;
        }
        [Required,
         ExcelColumn("BY"),
         SqlParameter("ocr_comments")]
        public string OCRComment
        {
            get; set;
        }

        [Required,
        ExcelColumn("CB"),
        SqlParameter("fem_analysic")]
        public bool FEMAnalysic
        {
            get; set;
        }
        [Required,
         ExcelColumn("CF"),
        SqlParameter("around_situation")]
        public string AroundSituation
        {
            get;
            set;
        }

        [Required,
        ExcelColumn("CG"),
       SqlParameter("has_embankment_plan")]
        public bool HasEmbankmentPlan
        {
            get;
            set;
        }

        [Required,
         ExcelColumn("CH"),
         SqlParameter("is_narrow_land_boundary")]
        public bool IsNarrowLandBoundary
        {
            get;
            set;
        }
        [Required,
        ExcelColumn("CJ"),
        SqlParameter("has_burial_property_below")]
        public bool HasBurialPropertyBelow
        {
            get;
            set;
        }
        [Required,
         ExcelColumn("CK"),
         SqlParameter("freezer_temperature")]
        public double FreezerTemperature
        {
            get;
            set;
        }
        [Required,
         ExcelColumn("CI"),
         SqlParameter("is_more_2terms")]
        public bool IsMore2Terms
        {
            get;
            set;
        }
         [Required,
         ExcelColumn("CM"),
         SqlParameter("is_over_200_think")]
        public bool IsOver200Thick
        {
            get;
            set;
        }
        [Required,
         ExcelColumn("CL"),
         SqlParameter("is_govemment_construction")]
        public bool IsGrovemmentConstruction
        {
            get;
            set;
        }
        [Required,
         ExcelColumn("CN"),
         SqlParameter("remark")]
        public string Remark
        {
            get;
            set;
        }
    }
}
