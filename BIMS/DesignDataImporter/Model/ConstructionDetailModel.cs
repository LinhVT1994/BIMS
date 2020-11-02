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
         ExcelTemporaryStorage("CC")
         SqlParameter("construction_detail_id")]
        public int ConstructionDetailId
        {
            get;
            set;
        }

        [Required,
         ExcelColumn("BZ")]
        public int ConstructionId
        {
            get;
            set;
        }
      
        [Required,
         ExcelColumn("L"),
         SqlParameter("movable_loading")]
        public double MovableLoading
        {
            get;
            set;
        }

        [Required,
         ExcelColumn("CB"),
         SqlParameter("rooftop_id")]
        public int? RooftopId { get; set; }

        [Required,
        ExcelColumn("CA"),
        SqlParameter("design_route_id"),]
        public int? DesignRouteId { get; set; }

        [Required,
        ExcelColumn("O"),
        SqlParameter("snowfall_amount")]
        public double SnowFallAmount
        {
            get;set;
        }
        [Required,
         ExcelColumn("P"),
         SqlParameter("total_floor_area")]
        public double TotalFloorArea
        {
            get; set;
        }

        [Required,
        ExcelColumn("Q"),
        SqlParameter("total_construction_area")]
        public double TotalConstructionArea
        {
            get; set;
        }

        [Required,
         ExcelColumn("AX"),
         SqlParameter("platform")]
        public bool Flatform
        {
            get; set;
        }
        [Required,
         ExcelColumn("AY"),
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
         ExcelColumn("BJ"),
         SqlParameter("ocr_comments")]
        public string OCRComment
        {
            get; set;
        }

        [Required, SqlParameter("started_day"), ExcelColumn("AF")]
        public DateTime? StartDate
        {
            get;
            set;
        }
        [Required, SqlParameter("finished_day"), ExcelColumn("AG")]
        public DateTime? FinishedDay
        {
            get;
            set;
        }
        [Required,
        ExcelColumn("BM"),
        SqlParameter("fem_analysic")]
        public bool FEMAnalysic
        {
            get; set;
        }
        [Required,
         ExcelColumn("BQ"),
        SqlParameter("around_situation")]
        public string AroundSituation
        {
            get;
            set;
        }

        [Required,
        ExcelColumn("BR"),
       SqlParameter("has_embankment_plan")]
        public bool HasEmbankmentPlan
        {
            get;
            set;
        }

        [Required,
         ExcelColumn("BS"),
         SqlParameter("is_narrow_land_boundary")]
        public bool IsNarrowLandBoundary
        {
            get;
            set;
        }
        [Required,
        ExcelColumn("BU"),
        SqlParameter("has_burial_property_below")]
        public bool HasBurialPropertyBelow
        {
            get;
            set;
        }
        [Required,
         ExcelColumn("BV"),
         SqlParameter("freezer_temperature")]
        public double FreezerTemperature
        {
            get;
            set;
        }
        [Required,
         ExcelColumn("BT"),
         SqlParameter("is_more_2terms")]
        public bool IsMore2Terms
        {
            get;
            set;
        }
         [Required,
         ExcelColumn("BX"),
         SqlParameter("is_over_200_think")]
        public bool IsOver200Thick
        {
            get;
            set;
        }
        [Required,
         ExcelColumn("BW"),
         SqlParameter("is_govemment_construction")]
        public bool IsGrovemmentConstruction
        {
            get;
            set;
        }
        [Required,
         ExcelColumn("BY"),
         SqlParameter("remark")]
        public string Remark
        {
            get;
            set;
        }
    }
}
