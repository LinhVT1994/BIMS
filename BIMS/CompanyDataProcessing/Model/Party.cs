using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDataProcessing.Model
{
    [SqlParameter("party")]
    public class PartyModel
    {
        #region Variables Declaration
        private int partyId;
        private string name;
        private string phone;
        private string email;
        private int positionId;
        private string detailAddress;
        private bool isMyCompany = false;
        #endregion

        #region Constructor
        public PartyModel()
        {
        }
        #endregion

        #region Properties
        [PrimaryKey, AutoIncrement, Required, SqlParameter("party_id")]
        public int PartyId
        {
            get
            {
                return partyId;
            }
            set
            {
                partyId = value;
            }
        }
        [Required, SqlParameter("is_my_company")]
        public bool IsMyCompany
        {
            get
            {
                return isMyCompany;
            }
            set
            {
                isMyCompany = value;
            }
        }
        [Required, ExcelColumn("Q"), SqlParameter("position_id")]
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
        [Required, ExcelColumn("B"), SqlParameter("name")]
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
        [Required, ExcelColumn("G"), SqlParameter("phone")]
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
                if (!string.IsNullOrWhiteSpace(phone))
                {
                    phone = phone.Replace("-", "");
                }
                
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
        public string Address
        {
            get
            {
                return detailAddress;
            }
            set
            {
                detailAddress = value;
            }
        }
        #endregion
    }
}
