using DataUtilities.Attributes;
using DataUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("party")]
    public class PartyModel: Element
    {
        [PrimaryKey,AutoIncrement,Required,SqlParameter("party_id")]
        public int PartyId
        {
            get;
            set;
        }
        [Unique,ExcelColumn("B"), Required, SqlParameter("name")]
        public string PartyName
        {
            get;
            set;
        }
    }
}
