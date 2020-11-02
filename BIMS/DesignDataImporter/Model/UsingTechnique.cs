using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("using_technique")]
    public class UsingTechnique
    {
        [Required,
         PrimaryKey,
         AutoIncrement,
         SqlParameter("using_technique_id")]
        public int ConstructionId
        {
            get;
            set;
        }
        [Required,
        ExcelColumn("CC"),
        SqlParameter("construction_detail_id")]
        public int ConstructionDetailId
        {
            get;
            set;
        }
        [Required,
         SqlParameter("technique_id")]
        public int? TechniqueId
        {
            get;
            set;
        }
        public bool IsExecuted { get; set; }
        private string techniqueIdRaw;
        [Required,
         ExcelColumn("AQ")]
        public string TechniqueIdRaw
        {
            get
            {
                return techniqueIdRaw;
            }
            set
            {
                techniqueIdRaw = value;
                if (!string.IsNullOrWhiteSpace(techniqueIdRaw) && techniqueIdRaw.Contains("布"))
                {
                    IsExecuted = true;
                }
            }
        }
    }
}
