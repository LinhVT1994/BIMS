using DataUtilities.Attributes;
using DataUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DesignDataImporter.Model
{
    [SqlParameter("regions")]
    public class JapanRegion:Element
    {
        [SqlParameter("region_id"), PrimaryKey, Required, AutoIncrement]
        public int Id { get; set; }

        [SqlParameter("region_name"), Required]
        public string RegionName { get; set; }

        [SqlParameter("region_name_roman"), Required]
        public string RegionNameRoman { get; set; }

        [SqlParameter("region_level"), Required]
        public int RegionLevel { get; set; }

        [SqlParameter("region_group_code"), Required]
        public int RegionGroupCode { get; set; }

        [SqlParameter("zip_code"), Required]
        public string ZipCode { get; set; }

        [SqlParameter("region_parent_id"), Required]
        public string RegionParentId { get; set; }

    }
}
