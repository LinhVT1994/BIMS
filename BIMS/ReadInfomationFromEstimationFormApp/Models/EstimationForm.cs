using ReadInfomationFromEstimationFormApp.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadInfomationFromEstimationFormApp.Models
{
    public class EstimationForm
    {
        #region 業務情報
        [Required, ExcelColumn("B"), ExcelColumnMapping("業務情報.物件番号"), Direction(Direction.Right)] //施工担当者
        public string ConstructionNo
        {
            get;
            set;
        }
       
        [Required, ExcelColumn("E"), ExcelColumnMapping("業務情報.工事名称"), Direction(Direction.Right)] //施工担当者
        public string ConstructionName
        {
            get;
            set;
        }
        [Required, ExcelColumn("F"), ExcelColumnMapping("業務情報.工事場所"), Direction(Direction.Right)] //施工担当者
        public string ConstructionAddress
        {
            get;
            set;
        }
        [Required, ExcelColumn("G"), ExcelColumnMapping("業務情報.施工担当者"), Direction(Direction.Right)] //施工担当者
        public string ConstructionStaff
        {
            get;
            set;
        }
        #endregion
        [Required, ExcelColumn("H"), ExcelColumnMapping("業務情報.予定工期", 0, 2)] //予定工期 スタート
        public string StartDateOnPlan
        {
            get;
            set;
        }
        [Required, ExcelColumn("I"), ExcelColumnMapping("業務情報.予定工期", 0, 5)] //予定工期 エンド
        public string EndDateOnPlan
        {
            get;
            set;
        }
        [Required, ExcelColumn("J"), ExcelColumnMapping("業務情報.実施工期", 0, 1)] //予定工期 スタート
        public string StartDate4Implementation
        {
            get;
            set;
        }
        [Required, ExcelColumn("K"), ExcelColumnMapping("業務情報.実施工期", 0, 4)] //実施工期 エンド
        public string EndDate4Implementation
        {
            get;
            set;
        }


        #region 施主情報
        [Required, ExcelColumn("L"), ExcelColumnMapping("施主情報.施主名"), Direction(Direction.Right)] //施主名
        public string ConstructionsOwnerName
        {
            get;
            set;
        }
        [Required, ExcelColumn("M"), ExcelColumnMapping("施主情報.施主住所"), Direction(Direction.Right)] // 施主住所
        public string ConstructionsOwnerAddress
        {
            get;
            set;
        }
        [Required, ExcelColumn("N"), ExcelColumnMapping("施主情報.建物完成予定日", 0, 1)] // 建物完成予定日
        public string FinishDateOnPlan
        {
            get;
            set;
        }
        #endregion

        #region 物件情報
        [Required, ExcelColumn("O"), ExcelColumnMapping("物件情報.建物用途"), Direction(Direction.Right)]//建物用途
        public string Purpose
        {
            get;
            set;
        }
        [Required, ExcelColumn("P"), ExcelColumnMapping("物件情報.規模・階数"), Direction(Direction.Right)]//規模・階数
        public string Scale
        {
            get;
            set;
        }
        [Required, ExcelColumn("Q"), ExcelColumnMapping("物件情報.構造種別"), Direction(Direction.Right)]//構造種別
        public string Structure
        {
            get;
            set;
        }
        [Required, ExcelColumn("R"), ExcelColumnMapping("物件情報.営業受注先", 0, 1)]//営業受注先
        public string SalesOrder
        {
            get;
            set;
        }
        [Required, ExcelColumn("S"), ExcelColumnMapping("物件情報.敷地面積"), Direction(Direction.Right)]//敷地面積
        public string SiteArea
        {
            get;
            set;
        }

        [Required, ExcelColumn("T"), ExcelColumnMapping("物件情報.建築面積"), Direction(Direction.Right)]//建築面積
        public string BuildingArea
        {
            get;
            set;
        }
        [Required, ExcelColumn("V"), ExcelColumnMapping("物件情報.施工面積"), Direction(Direction.Right)]//施工面積
        public string ExecutedArea
        {
            get;
            set;
        }
        #endregion

        #region 受注情報
        [Required, ExcelColumn("W"), ExcelColumnMapping("受注情報.受注先"), Direction(Direction.Right)]//受注先(元請)
        public string PrimeContractor
        {
            get;
            set;
        }
        [Required, ExcelColumn("X"), ExcelColumnMapping("受注情報.交渉相手"), Direction(Direction.Right)]//担当者 (交渉相手)
        public string PersonInChargeOfPrimeContractor
        {
            get;
            set;
        }
        [Required, ExcelColumn("Y"), ExcelColumnMapping("受注情報.契約年月日"), Direction(Direction.Right)]//契約年月日
        public string ContractDate
        {
            get;
            set;
        }
        [Required, ExcelColumn("Z"), ExcelColumnMapping("受注情報.連絡先"), Direction(Direction.Right)]//連絡先(電話)
        public string ContactPhone
        {
            get;
            set;
        }
        [Required, ExcelColumn("AA"), ExcelColumnMapping("受注情報.FAX"), Direction(Direction.Right)]//連絡先(FAX)
        public string ContactFax
        {
            get;
            set;
        }
        [Required, ExcelColumn("AB"), ExcelColumnMapping("受注情報.契約金額"), Direction(Direction.Right)]//契約金額
        public string PriceInContract
        {
            get;
            set;
        }
        [Required, ExcelColumn("AC"), ExcelColumnMapping("受注情報.見積提出金額"), Direction(Direction.Right)]//予算金額
        public string PriceOnEstimationFirst
        {
            get;
            set;
        }
        [Required, ExcelColumn("AD"), ExcelColumnMapping("受注情報.見積NET金額"), Direction(Direction.Right)]//見積NET金額
        public string PriceInNET
        {
            get;
            set;
        }
        [Required, ExcelColumn("AF"), ExcelColumnMapping("受注情報.値引後金額"), Direction(Direction.Right)]//値引後金額
        public string PriceAfterDiscount
        {
            get;
            set;
        }
        [Required, ExcelColumn("AG"), ExcelColumnMapping("受注情報.予算金額）"), Direction(Direction.Right)]//見積提出金額（税抜）
        public string PriceOnEstimation
        {
            get;
            set;
        }
        #endregion

        #region 施工情報
        [Required, ExcelColumn("AH"), ExcelColumnMapping("施工情報.一次改良?1次改良", 1, 0)]//施工面積 - 1次改良
        public string ExecutedAreaFirstLayer
        {
            get;
            set;
        }
        [Required, ExcelColumn("AI"), ExcelColumnMapping("施工情報.一次改良?1次改良", 2, 0)]//改良厚 - 1次改良
        public string HeightOfFirstLayer
        {
            get;
            set;
        }
        [Required, ExcelColumn("AJ"), ExcelColumnMapping("施工情報.一次改良?1次改良", 3, 0)]//施工量 - 1次改良
        public string ExecutedVolumnOfFirstLayer
        {
            get;
            set;
        }
        [Required, ExcelColumn("AK"), ExcelColumnMapping("施工情報.一次改良?1次改良", 4, 0)]//配合量 - 1次改良
        public string AmountOfCementOfFirstLayer
        {
            get;
            set;
        }
        [Required, ExcelColumn("AL"), ExcelColumnMapping("施工情報.一次改良?1次改良", 5, 0)]//使用材料 - 1次改良
        public string CementTypeOfFirstLayer
        {
            get;
            set;
        }

        [Required, ExcelColumn("AM"), ExcelColumnMapping("施工情報.二次改良?2次改良", 1, 0)]//施工面積 - 2次改良
        public string ExecutedAreaSecondLayer
        {
            get;
            set;
        }
        [Required, ExcelColumn("AN"), ExcelColumnMapping("施工情報.二次改良?2次改良", 2, 0)]//改良厚 - 2次改良
        public string HeightOfSecondLayer
        {
            get;
            set;
        }
        [Required, ExcelColumn("AO"), ExcelColumnMapping("施工情報.二次改良?2次改良", 3, 0)]//施工量 - 2次改良
        public string ExecutedVolumnOfSecondLayer
        {
            get;
            set;
        }
        [Required, ExcelColumn("AP"), ExcelColumnMapping("施工情報.二次改良?2次改良", 4, 0)]//配合量 - 2次改良
        public string AmountOfCementOfSecondLayer
        {
            get;
            set;
        }
        [Required, ExcelColumn("AQ"), ExcelColumnMapping("施工情報.二次改良?2次改良", 5, 0)]//使用材料 - 2次改良
        public string CementTypeOfSecondLayer
        {
            get;
            set;
        }
        #endregion

        #region 測定結果
        [Required, ExcelColumn("AR"), ExcelColumnMapping("測定結果.一次改良?1次改良", 1, 0)]
        public string BearingCapacityFirstLayer
        {
            get;
            set;
        }
        [Required, ExcelColumn("AS"), ExcelColumnMapping("測定結果.一次改良?1次改良", 2, -1)]
        public string CementAmountFirstLayer1
        {
            get;
            set;
        }
        [Required, ExcelColumn("AT"), ExcelColumnMapping("測定結果.一次改良?1次改良", 2, 0)]
        public string StrengthFirstLayer1
        {
            get;
            set;
        }
        [Required, ExcelColumn("AU"), ExcelColumnMapping("測定結果.一次改良?1次改良", 3, -1)]
        public string CementAmountFirstLayer2
        {
            get;
            set;
        }
        [Required, ExcelColumn("AV"), ExcelColumnMapping("測定結果.一次改良?1次改良", 3, 0)]
        public string StrengthFirstLayer2
        {
            get;
            set;
        }
        [Required, ExcelColumn("AW"), ExcelColumnMapping("測定結果.一次改良?1次改良", 4, -1)]
        public string CementAmountFirstLayer3
        {
            get;
            set;
        }
        [Required, ExcelColumn("AX"), ExcelColumnMapping("測定結果.一次改良?1次改良", 4, 0)]
        public string StrengthFirstLayer3
        {
            get;
            set;
        }
        [Required, ExcelColumn("AY"), ExcelColumnMapping("測定結果.二次改良?2次改良", 1, 0)]
        public string BearingCapacitySecondLayer
        {
            get;
            set;
        }
        [Required, ExcelColumn("AZ"), ExcelColumnMapping("測定結果.二次改良?2次改良", 2, -1)]
        public string CementAmountSecondLayer1
        {
            get;
            set;
        }
        [Required, ExcelColumn("BA"), ExcelColumnMapping("測定結果.二次改良?2次改良", 2, 0)]
        public string StrengthSecondLayer1
        {
            get;
            set;
        }
        [Required, ExcelColumn("BB"), ExcelColumnMapping("測定結果.二次改良?2次改良", 3, -1)]
        public string CementAmountSecondLayer2
        {
            get;
            set;
        }
        [Required, ExcelColumn("BC"), ExcelColumnMapping("測定結果.二次改良?2次改良", 3, 0)]
        public string StrengthSecondLayer2
        {
            get;
            set;
        }
        [Required, ExcelColumn("BD"), ExcelColumnMapping("測定結果.二次改良?2次改良", 4, -1)]
        public string CementAmountSecondLayer3
        {
            get;
            set;
        }
        [Required, ExcelColumn("BE"), ExcelColumnMapping("測定結果.二次改良?2次改良", 4, 0)]
        public string StrengthSecondLayer3
        {
            get;
            set;
        }
        #endregion 

    }
}
