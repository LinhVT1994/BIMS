using DataUtilities.Attributes;
using DataUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("design_route")]
    public class DesignRouteModel : Element
    {
        #region Variables
        private int designRouteId;
        private string name;
        #endregion

        #region Properties
        [Required, AutoIncrement,PrimaryKey, SqlParameter("design_route_id")]
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
        [Required, Unique, ExcelColumn("AL") ,SqlParameter("name")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {

                name =value;
                
            }
        }
        #endregion
    }
}
