using DataUtilities.Attributes;
using DataUtilities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("cooperating_on_design")]
    public class IdeaCooperatingOnDesign : Element
    {
        public IdeaCooperatingOnDesign()
        {
            RoleOfCooperatingId = 8;
        }
        [Required,AutoIncrement,PrimaryKey,SqlParameter("cooperating_on_design_id")]
        public int CooperatingOnDesignId
        {
            get;
            set;
        }
        [Required, SqlParameter("construction_detail_id"),ExcelColumn("CQ")]
        public int ConstructionDetailId
        {
            get;
            set;
        }
        [Required, SqlParameter("role_of_cooperating_id")]
        public int RoleOfCooperatingId
        {
            get;
            set;
        } = 8;
        [Required, SqlParameter("party_id"),ForeignKey("party", "name[AP]=>party_id")]
        public PartyModel Party
        {
            get;
            set;
        }
    }
    [SqlParameter("cooperating_on_design")]
    public class StructureCooperatingOnDesign : Element
    {
        [Required, AutoIncrement, PrimaryKey, SqlParameter("cooperating_on_design_id")]
        public int CooperatingOnDesignId
        {
            get;
            set;
        }
        [Required, SqlParameter("construction_detail_id"), ExcelColumn("CQ")]
        public int ConstructionDetailId
        {
            get;
            set;
        }
        [Required, SqlParameter("role_of_cooperating_id")]
        public int RoleOfCooperatingId
        {
            get;
            set;
        } = 9;
        [Required, SqlParameter("party_id"), ForeignKey("party", "name[AQ]=>party_id")]
        public PartyModel Party
        {
            get;
            set;
        }
    }
    [SqlParameter("cooperating_on_design")]
    public class RelativeCooperatingOnDesign : Element
    {
        [Required, AutoIncrement, PrimaryKey, SqlParameter("cooperating_on_design_id")]
        public int CooperatingOnDesignId
        {
            get;
            set;
        }
        [Required, SqlParameter("construction_detail_id"), ExcelColumn("CQ")]
        public int ConstructionDetailId
        {
            get;
            set;
        }
        [Required, SqlParameter("role_of_cooperating_id")]
        public int RoleOfCooperatingId
        {
            get;
            set;
        } = 10;
        [Required, SqlParameter("party_id"), ForeignKey("party", "name[AR]=>party_id")]
        public PartyModel Party
        {
            get;
            set;
        }
    }
    [SqlParameter("cooperating_on_design")]
    public class ConfirmationExCooperatingOnDesign : Element
    {
        [Required, AutoIncrement, PrimaryKey, SqlParameter("cooperating_on_design_id")]
        public int CooperatingOnDesignId
        {
            get;
            set;
        }
        [Required, SqlParameter("construction_detail_id"), ExcelColumn("CQ")]
        public int ConstructionDetailId
        {
            get;
            set;
        }
        [Required, SqlParameter("role_of_cooperating_id")]
        public int RoleOfCooperatingId
        {
            get;
            set;
        } = 11;
        [Required, SqlParameter("party_id"), ForeignKey("party", "name[AS]=>party_id")]
        public PartyModel Party
        {
            get;
            set;
        }
    }
    [SqlParameter("cooperating_on_design")]
    public class CheckCooperatingOnDesign : Element
    {
        [Required, AutoIncrement, PrimaryKey, SqlParameter("cooperating_on_design_id")]
        public int CooperatingOnDesignId
        {
            get;
            set;
        }
        [Required, SqlParameter("construction_detail_id"), ExcelColumn("CQ")]
        public int ConstructionDetailId
        {
            get;
            set;
        }
        [Required, SqlParameter("role_of_cooperating_id")]
        public int RoleOfCooperatingId
        {
            get;
            set;
        } = 12;
        [Required, SqlParameter("party_id"), ForeignKey("party", "name[AT]=>party_id")]
        public PartyModel Party
        {
            get;
            set;
        }
    }
}
