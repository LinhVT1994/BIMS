using BIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
      /**
    * 
    * @author  LinhVT
    * @version 1.0
    * @since   2019/5/14
    * Edited day: 
    * Edit content: 
    */
    [SqlParameter("rooftop")]
    class RooftopModel : Element
    {
        #region Variables
        private int rooftopId;
        private string name;
        private string description;
        #endregion

        #region Properties

        [Required, PrimaryKey, SqlParameter("rooftop_id")]
        public int RooftopId
        {
            get
            {
                return rooftopId;
            }
            set
            {
                rooftopId = value;
            }
        }
        [Required, Unique, ExcelColumn("M"), SqlParameter("name")]
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
