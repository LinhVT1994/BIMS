using DataUtilities.Attributes;
using DataUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("scale")]
    public class ScaleModel : Element
    {
        #region Variables
        private int scaleId;
        private string name;
        private string description;
        #endregion

        #region Properties
        [Required, AutoIncrement, PrimaryKey, SqlParameter("scale_id")]
        public int ScaleId
        {
            get
            {
                return scaleId;
            }
            set
            {
                scaleId = value;
            }
        }
        [Required, Unique, ExcelColumn("AG"), SqlParameter("name")]
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
