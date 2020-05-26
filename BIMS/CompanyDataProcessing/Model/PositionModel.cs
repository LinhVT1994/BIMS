using DataUtilities.Attributes;

namespace CompanyDataProcessing.Model
{
    /**
    * A <see cref="PositionModel"/> object what contains information relate to position of a contruction.
    * @author  LinhVT
    * @version 1.0
    * @since   2018/11/21
    * Edited day: 2018/12/15
    * Edit content: comments and namespace.
    */
    [SqlParameter("position")]
    public class PositionModel 
    {
        #region Variables Declaraction
        private int positionId;
        private string name;
        private string latitude;
        private string longitude;
        private int region_Id;
        #endregion

        #region Constructor
        public PositionModel()
        {

        }
        public PositionModel(int id, string name, string latitude = null, string longitude = null)
        {
            PositionId = id;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
        #endregion

        #region Properties

        [Required, AutoIncrement, PrimaryKey, SqlParameter("position_id"), ExcelTemporaryStorage("Q")]
        public int PositionId
        {
            get
            {
                return positionId;
            }
            set
            {
                positionId = value;
            }
        }
        public string FullAddress
        {
            get;
            set;
        }

        [Required, ExcelColumn("P"), SqlParameter("region_id")]
        public int RegionId
        {
            get
            {
                return region_Id;
            }
            set
            {
                region_Id = value;
            }
        }
        [Required, ExcelColumn("F"),SqlParameter("name")]
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

        [Required, ExcelColumn("N"), SqlParameter("latitude")]
        public string Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
            }
        }
        [Required,ExcelColumn("O"), SqlParameter("longitude")]
        public string Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
            }
        }
        #endregion
    }
}
