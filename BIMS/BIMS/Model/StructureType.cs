using BIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
    [SqlParameter("structure_type")]
    class StructureTypeModel : Element
    {
        #region Variables
        private int structipeTypeId;
        private string name;
        private string description;
        #endregion

        #region Properties
        [Required, PrimaryKey, SqlParameter("structure_type_id")]
        public int StructureTypeId
        {
            get
            {
                return structipeTypeId;
            }
            set
            {
                structipeTypeId = value;
            }
        }
        [Required, Unique,ExcelColumn("I"),SqlParameter("name")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        #endregion
    }
}
