using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMS.Attributes;

namespace BIMS.Model
{
    /** 
    * @author  LinhVT
    * @version 1.0
    * @since   2018/11/7
    */
    [SqlParameter("mixing_result")]
    class MixingResult : Element
    {
        private int    _MixingResultId;
        private double _Cement_Amount;
        private double _Archived_Strength;
        private double _Water_Content_Ratio;
        private double _Wet_Density;
        private Cement _Cement;
        private TestingSample _TestingSample;
        [Required,PrimaryKey, AutoIncrement, SqlParameter("mixing_result_id")]
        public int MixingResultId
        {
            get
            {
                return _MixingResultId;
            }
            set
            {
                _MixingResultId = value;
            }
        }
        [Required, ExcelColumn("U,Y,AC,AG"), SqlParameter("cement_amount")]
        public double Cement_Amount
        {
            get
            {
                return _Cement_Amount;
            }
            set
            {
                _Cement_Amount = value;
            }
        }
        [Required, ExcelColumn("V,Z,AD,AH"), SqlParameter("archived_strength")]
        public double Archived_Strength
        {
            get
            {
                return _Archived_Strength;
            }
            set
            {
                _Archived_Strength = value;
            }
        }
        [Required, ExcelColumn("W,AA,AE,AI"), SqlParameter("water_content_ratio")]
        public double Water_Content_Ratio
        {
            get
            {
                return _Water_Content_Ratio;
            }
            set
            {
                _Water_Content_Ratio = value;
            }
        }
        [Required, ExcelColumn("X,AB,AF,AJ"), SqlParameter("wet_density")]
        public double Wet_Density
        {
            get
            {
                return _Wet_Density;
            }
            set
            {
                _Wet_Density = value;
            }
        }
        [Required, SqlParameter("cement_id"), ForeignKey("cement", "symbol[I]=>cement_id")]
        public Cement Cement
        {
            get
            {
                return _Cement;
            }
            set
            {
                _Cement = value;
            }

        }
        //[Required, ExcelColumn("G"), SqlParameter("testing_sample_id")]
        [Required, 
         SqlParameter("testing_sample_id"), 
         Distinguish("[construction.construction_no(E)]"), 
         ForeignKey("testing_sample", "*[*]=>testing_sample_id")]
        public TestingSample TestingSample
        {
            get
            {
                return _TestingSample;
            }
            set
            {
                _TestingSample = value;
            }

        }
    }
}
