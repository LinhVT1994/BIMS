using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateExcelFileForVNTakeuchi.Models
{
    /*
     select con.construction_no, 
    structure_type.name as structure_type,
    scale.name as scale,
    purpose.name as purpose,
    rooftop.name as rooftop,
    design_route.name as design_route,
    cond.movable_loading,
    cond.snowfall_amount,
    cond.total_floor_area,
    cond.total_construction_area,
    cond.platform,
    cond.crushed_stone,
    cond.ocr_min,
    cond.fem_analysic,
    cond.around_situation,
    cond.has_embankment_plan,
    cond.is_narrow_land_boundary,
    cond.is_more_2terms,
    cond.has_burial_property_below,
    cond.freezer_temperature,
    cond.is_govemment_construction,
    cond.is_over_200_think
    from construction as con,construction_detail as cond,structure_type,purpose,design_route,rooftop, scale
    where con.construction_id = cond.construction_id 
    and structure_type.structure_type_id = cond.structure_type_id
    and purpose.purpose_id = cond.purpose_id
    and rooftop.rooftop_id=cond.rooftop_id
    and design_route.design_route_id=cond.design_route_id
    and scale.scale_id = cond.scale_id
     
     */
    public class DesignDataModel
    {
        [SqlParameter("construction_no"), PrimaryKey, Required, ExcelColumn("A")]
        public string No
        {
            get;
            set;
        }

        [SqlParameter("structure_type"), Required]
        public string structure_type
        {
            get;
            set;
        }
        [SqlParameter("scale"), Required]
        public string scale
        {
            get;
            set;
        }
        [SqlParameter("purpose"), Required]
        public string purpose
        {
            get;
            set;
        }
        [SqlParameter("rooftop"), Required]
        public string rooftop
        {
            get;
            set;
        }
        [SqlParameter("design_route"), Required]
        public string design_route
        {
            get;
            set;
        }
        [SqlParameter("movable_loading"), Required]
        public string movable_loading
        {
            get;
            set;
        }
        [SqlParameter("snowfall_amount"), Required]
        public string snowfall_amount
        {
            get;
            set;
        }
        [SqlParameter("total_floor_area"), Required]
        public string total_floor_area
        {
            get;
            set;
        }
        [SqlParameter("total_construction_area"), Required]
        public string total_construction_area
        {
            get;
            set;
        }
        [SqlParameter("platform"), Required]
        public bool platform
        {
            get;
            set;
        }
        [SqlParameter("crushed_stone"), Required]
        public bool crushed_stone
        {
            get;
            set;
        }
        [SqlParameter("ocr_min"), Required]
        public double ocr_min
        {
            get;
            set;
        }
        [SqlParameter("around_situation"), Required]
        public string around_situation
        {
            get;
            set;
        }
        [SqlParameter("has_embankment_plan"), Required]
        public bool has_embankment_plan
        {
            get;
            set;
        }
        [SqlParameter("is_narrow_land_boundary"), Required]
        public bool is_narrow_land_boundary
        {
            get;
            set;
        }
        [SqlParameter("is_more_2terms"), Required]
        public bool is_more_2terms
        {
            get;
            set;
        }
        [SqlParameter("has_burial_property_below"), Required]
        public bool has_burial_property_below
        {
            get;
            set;
        }
        [SqlParameter("freezer_temperature"), Required]
        public double freezer_temperature
        {
            get;
            set;
        }
        [SqlParameter("is_govemment_construction"), Required]
        public bool is_govemment_construction
        {
            get;
            set;
        }
        [SqlParameter("is_over_200_think"), Required]
        public bool is_over_200_think
        {
            get;
            set;
        }
    }
}
