using BIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.DesignModel
{
    [SqlParameter("construction")]
    class ConstructionModel
    {
        [Required, AutoIncrement, PrimaryKey, SqlParameter("construction_id")]
        public int Construction_Id
        {
            get;
            set;
        }
        [Required, Unique, ExcelColumn("E"), SqlParameter("construction_no")]
        public string Construction_No
        {
            get;
            set;
        }
        [Required, ExcelColumn("F"), SqlParameter("name")]
        public string Name
        {
            get;
            set;
        }

    }
}
