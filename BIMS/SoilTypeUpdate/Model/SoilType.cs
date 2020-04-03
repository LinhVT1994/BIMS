using DataUtilities.Attributes;
using DataUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoilTypeUpdate.Model
{
    [SqlParameter("soil_type")]

    public class SoilType : Element
    {
        [AutoIncrement,Required, PrimaryKey,SqlParameter("soil_type_id")]
        public int SoilId
        {
            get;
            set;
        }
        [SqlParameter("name"),Unique,Required,ExcelColumn("B")]
        public string Name
        {
            get;
            set;
        }
        [SqlParameter("symbol"), Unique, Required, ExcelColumn("C")]
        public string Symbol
        {
            get;
            set;
        }
    }
}
