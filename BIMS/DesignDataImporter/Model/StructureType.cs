using DataUtilities.Attributes;
using DataUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("structure_type")]
     public class StructureTypeModel : Element
    {
        #region Variables
        private int structipeTypeId;
        private string name;
        private string description;
        #endregion

        #region Properties
        [Required, PrimaryKey, AutoIncrement, SqlParameter("structure_type_id")]
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
        [Required, Unique, ExcelColumn("X"),SqlParameter("name")]
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
