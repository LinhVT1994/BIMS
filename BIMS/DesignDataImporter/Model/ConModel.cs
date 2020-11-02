using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("construction")]
    public class ConModel
    {
        [PrimaryKey,
         AutoIncrement, Required,
         ExcelColumn("A"),
         SqlParameter("construction_id"),
         ExcelTemporaryStorage("BZ")]
        public int ConstructionId
        {
            get;
            set;
        }
        [Required,
         Unique,
         SqlParameter("construction_no"),
         ExcelColumn("B")]
        public string ConstructionNo
        {
            get;
            set;
        }
    }
}
