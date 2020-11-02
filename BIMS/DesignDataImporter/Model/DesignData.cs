using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("construction_detail")]
    public class DesignData
    {
        [SqlParameter("construction_detail_id")
         Required,
         PrimaryKey,
         ExcelColumn("CC")]
        public int ConstructionDetailId
        {
            get;
            set;
        }
        [Required,
        SqlParameter("rooftop_id")]
        public int? RooftopId { get; set; }

        [Required,
        SqlParameter("design_route_id"),]
        public int? DesignRouteId { get; set; }

        private int rooftopIdRaw;
        private int designRouteIdRaw;

        [Required,
        ExcelColumn("CB")]
        public int RooftopIdRaw
        {
            get
            {
                return rooftopIdRaw;
            }
            set
            {
                rooftopIdRaw = value;
                if (rooftopIdRaw != 0)
                {
                    RooftopId = rooftopIdRaw;
                }
            }
        }

        [Required,
        ExcelColumn("CA"),]
        public int DesignRouteIdRaw
        {
            get
            {
                return designRouteIdRaw;
            }
            set
            {
                designRouteIdRaw = value;
                if (designRouteIdRaw !=0 )
                {
                    DesignRouteId = designRouteIdRaw; 
                }
            }
        }
    }
}
