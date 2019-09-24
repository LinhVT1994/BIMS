using BIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
    [SqlParameter("purpose")]
    class PurposeModel : Element 
    {
        #region Variables
        private int purposeId;
        private string name;
        private string description;
        #endregion

        #region Properties
        [Required, AutoIncrement ,PrimaryKey, SqlParameter("purpose_id")]
        public int PurposeId
        {
            get
            {
                return purposeId;
            }
            set
            {
                purposeId = value;
            }
        }
        [Required, Unique,ExcelColumn("K"),SqlParameter("name")]
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
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        #endregion
    }
}
