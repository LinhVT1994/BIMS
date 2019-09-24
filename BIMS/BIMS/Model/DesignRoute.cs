using BIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
    [SqlParameter("design_route")]
    class DesignRoute : Element
    {
        #region Variables
        private int designRouteId;
        private string name;
        private string description;
        #endregion

        #region Properties
        [Required, PrimaryKey, SqlParameter("design_route_id")]
        public int DesignRouteId
        {
            get
            {
                return designRouteId;
            }
            set
            {
                designRouteId = value;
            }
        }
        [Required, Unique,ExcelColumn("N") ,SqlParameter("name")]
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
