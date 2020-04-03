using DataUtilities.Attributes;
using DataUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UploadTestData.Model
{
    /**
    * @author  LinhVT
    * @version 1.0
    * @since   2018/11/7
    */
    [SqlParameter("construction_executing")]
    class ConstructionExecuting : Element
    {
        private int _Construction_Executing_Id;
        private double _Cement_Amount;
        private double _Archived_Strength;
        public ConstructionExecuting()
        {

        }
        public ConstructionExecuting(int id, double cementAmount, double archivedStrength)
        {
            Construction_Executing_Id = id;
            Cement_Amount = cementAmount;
            Archived_Strength = archivedStrength;
        }
        #region properties of this class. 
        [Required, PrimaryKey, AutoIncrement, SqlParameter("construction_executing_id")]
        public int Construction_Executing_Id
        {
            get
            {
                return _Construction_Executing_Id;
            }
            set
            {
                _Construction_Executing_Id = value;
            }
        }
        [Required, ExcelColumn("AP"), SqlParameter("cement_amount")]
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
        [Required, ExcelColumn("AN"), SqlParameter("archived_strength")]
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
        [Required, ExcelColumn("CF"), SqlParameter("cement_id")]
        public int CementId
        {
            get;
            set;
        }
        [Required,
         SqlParameter("testing_sample_id"),
         ExcelColumn("CE")]
        public int TestingSampleId
        {
            get;
            set;
        }
        #endregion
    }
}
