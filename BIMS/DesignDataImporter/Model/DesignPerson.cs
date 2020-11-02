using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataImporter.Model
{
    [SqlParameter("design")]
    public class DesignPerson
    {
        [PrimaryKey,
         Required,
         AutoIncrement,
         SqlParameter("design_id")]
        public int DesignId
        {
            get;
            set;
        }

        [Required,
        SqlParameter("designer_id")]
        public int DesignerId
        {
            get;
            set;
        }
        private string designerName;
        [Required,
         ExcelColumn("CE")]
        public string DesignerName
        {
            get
            {
                return designerName;
            }
            set
            {
                designerName = value;
                Debug.WriteLine(designerName);
                if (!string.IsNullOrWhiteSpace(designerName))
                {
                    designerName = designerName.Trim();
                }
                switch (designerName)
                {
                    case "近藤":
                        DesignerId = 9;
                        break;
                    case "村上":
                        DesignerId = 6;
                        break;
                    case "ホアン":
                        DesignerId = 7;
                        break;
                    case "ユイ":
                        DesignerId = 5;
                        break;
                    case "タオ":
                        DesignerId = 10;
                        break;
                    case "マイアイン":
                        DesignerId = 11;
                        break;
                    case "ラン":
                        DesignerId = 12;
                        break;
                    case "金":
                        DesignerId = 13;
                        break;
                    case "フィ":
                        DesignerId = 8;
                        break;
                    case "尾川":
                        DesignerId = 15;
                        break;
                    case "山名":
                        DesignerId = 17;
                        break;
                    case "和田":
                        DesignerId = 18;
                        break;
                    case "宮北":
                        DesignerId = 16;
                        break;
                    case "ダット":
                        DesignerId = 14;
                        break;
                    case "サン":
                        DesignerId = 20;
                        break;
                    case "ウット":
                        DesignerId = 21;
                        break;
                    default:
                        break;
                }
            }
        }
        [Required,
         SqlParameter("construction_detail_id"),
         ExcelColumn("CC")]
        public int ConDesignId
        {
            get;
            set;
        }
        [Required,
         SqlParameter("is_main")]
        public bool IsMain
        {
            get;
            set;
        }
    }
}
