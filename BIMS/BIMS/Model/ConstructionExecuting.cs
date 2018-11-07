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
    class ConstructionExecuting
    {
        private int _Construction_Executing_Id;
        private double _Cement_Amount;
        private double _Archived_Strength;
        private Cement _Cement;
        private TestingSample _TestingSample;

        
        public ConstructionExecuting()
        {

        }
        public ConstructionExecuting(int id, double cementAmount, double archivedStrength, Cement cement, TestingSample testingSample)
        {
            Construction_Excuting_Id = id;
            Cement_Amount = cementAmount;
            Archived_Strength = archivedStrength;
            Cement = cement;
            TestingSample = testingSample;
        }
        #region properties of this class. 
        [PrimaryKey]
        [AutoIncrement]
        public int Construction_Excuting_Id
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
        #endregion
    }
}
