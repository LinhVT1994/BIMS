using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    class AdditionalConstruction
    {
        [PrimaryKey,
         AutoIncrement, Required,
         SqlParameter("construction_id"),
         ExcelTemporaryStorage("BH")]
        public int ConstructionId
        {
            get;
            set;
        }
    }
}
