using DataUtilities.Attributes;
using DataUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadTestData.Model
{
    public class ConstructionModel : Element
    {
        private int _Construction_Id;
        private string _Construction_No;
        private string _Name;

        public ConstructionModel()
        {

        }
        [Required, 
         AutoIncrement,
         PrimaryKey, 
         SqlParameter("construction_id"),
         ExcelTemporaryStorage("CP")]
        public int Construction_Id
        {
            get
            {
                return _Construction_Id;
            }
            set
            {
                _Construction_Id = value;
            }
        }

        [Required, Unique, ExcelColumn("E"), SqlParameter("construction_no")]
        public string Construction_No
        {
            get
            {
                return _Construction_No;
            }
            set
            {
                _Construction_No = value;
            }
        }
        [Required, ExcelColumn("F"), SqlParameter("name")]
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
    }
}
