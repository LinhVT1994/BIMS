using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("construction")]
    public class ConDesign
    {
        [ PrimaryKey, 
          Required,
          ExcelColumn("BZ"),
          SqlParameter("construction_id"),
          ExcelTemporaryStorage("BZ")]
        public int ConstructionId
        {
            get;
            set;
        }
        [Required,
         SqlParameter("construction_detail_id"),
         ExcelColumn("CC")]
        public int ConDesignId
        {
            get;
            set;
        }
    }
}
