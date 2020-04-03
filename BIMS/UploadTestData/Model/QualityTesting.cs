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
    [SqlParameter("quality_testing")]
    class QualityTesting : Element
    {
        private int    _QualityTestingId;
        private string _Name;
        private double _ArchivedStrength7Days;
        private double _ArchivedStrength28Day;
        private ConstructionExecuting _ConstructionExecuting;
        public QualityTesting()
        {

        }
        public QualityTesting(int id, string name, double archivedStrengthAfter7days, double archivedStrength28Days)
        {
            QualityTestingId = id;
            Name = name;
            ArchivedStrength7Days = archivedStrengthAfter7days;
            ArchivedStrength28Days = archivedStrength28Days;

        }
        [Required, AutoIncrement, PrimaryKey, SqlParameter("quality_testing_id")]
        public int QualityTestingId
        {
            get
            {
                return _QualityTestingId;
            }
            set
            {
                _QualityTestingId = value;
            }
        }
        [Required,ExcelColumn("AR,AW,BB,BG,BL,BQ"), SqlParameter("name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        [Required, ExcelColumn("AS,AX,BC,BH,BM,BR"), SqlParameter("archived_strength_7day")]
        public double ArchivedStrength7Days
        {
            get
            {
                return _ArchivedStrength7Days;
            }
            set
            {
                _ArchivedStrength7Days = value;
            }
        }
        [Required, ExcelColumn("AT,AY,BD,BI,BN,BS"), SqlParameter("archived_strength_28day")]
        public double ArchivedStrength28Days
        {
            get
            {
                return _ArchivedStrength28Day;
            }
            set
            {
                _ArchivedStrength28Day = value;
            }
        }
        [Required,
         SqlParameter("construction_executing_id"),
         Distinguish("[construction.construction_no(E)]"),
         ForeignKey("construction_executing", "*[*]=>construction_executing_id")]
        public ConstructionExecuting ConstructionExecuting
        {
            get
            {
                return _ConstructionExecuting;
            }
            set
            {
                _ConstructionExecuting = value;
            }
        }

    }
}
