using DataUtilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDataMatching.Model
{
    [SqlParameter("estimation_data")]
    public class EstimationData
    {
        [SqlParameter("Colid"), AutoIncrement, PrimaryKey]
        public int ColId
        {
            get;
            set;
        }
        [SqlParameter("ColNo"), ExcelColumn("A"), Required, Unique]
        public string ColNo
        {
            get;
            set;
        }
        [SqlParameter("ColF"), ExcelColumn("F"), Required]
        public string ColF
        {
            get;
            set;
        }
        [SqlParameter("ColG"), ExcelColumn("G"), Required]
        public string ColG
        {
            get;
            set;
        }
        [SqlParameter("ColH"), ExcelColumn("H"), Required]
        public string ColH
        {
            get;
            set;
        }
        [SqlParameter("ColI"), ExcelColumn("I"), Required]
        public string ColI
        {
            get;
            set;
        }
        [SqlParameter("ColJ"), ExcelColumn("J"), Required]
        public string ColJ
        {
            get;
            set;
        }
        [SqlParameter("ColK"), ExcelColumn("K"), Required]
        public string ColK
        {
            get;
            set;
        }
        [SqlParameter("ColL"), ExcelColumn("L"), Required]
        public string ColL
        {
            get;
            set;
        }
        [SqlParameter("ColM"), ExcelColumn("M"), Required]
        public string ColM
        {
            get;
            set;
        }
        [SqlParameter("ColN"), ExcelColumn("N"), Required]
        public string ColN
        {
            get;
            set;
        }
        [SqlParameter("ColO"), ExcelColumn("O"), Required]
        public string ColO
        {
            get;
            set;
        }
        [SqlParameter("ColP"), ExcelColumn("P"), Required]
        public string ColP
        {
            get;
            set;
        }
        [SqlParameter("ColQ"), ExcelColumn("Q"), Required]
        public string ColQ
        {
            get;
            set;
        }
        [SqlParameter("ColR"), ExcelColumn("R"), Required]
        public string ColR
        {
            get;
            set;
        }
        [SqlParameter("ColS"), ExcelColumn("S"), Required]
        public double ColS
        {
            get;
            set;
        }
        [SqlParameter("ColT"), ExcelColumn("T"), Required]
        public double ColT
        {
            get;
            set;
        }
        [SqlParameter("ColU"), ExcelColumn("U"), Required]
        public double ColU
        {
            get;
            set;
        }
        [SqlParameter("ColV"), ExcelColumn("V"), Required]
        public double ColV
        {
            get;
            set;
        }
        [SqlParameter("ColW"), ExcelColumn("W"), Required]
        public string ColW
        {
            get;
            set;
        }
        [SqlParameter("ColX"), ExcelColumn("X"), Required]
        public string ColX
        {
            get;
            set;
        }
        [SqlParameter("ColY"), ExcelColumn("Y"), Required]
        public string ColY
        {
            get;
            set;
        }
        [SqlParameter("ColZ"), ExcelColumn("Z"), Required]
        public string ColZ
        {
            get;
            set;
        }
        [SqlParameter("ColAA"), ExcelColumn("AA"), Required]
        public double ColAA
        {
            get;
            set;
        }
        [SqlParameter("ColAB"), ExcelColumn("AB"), Required]
        public double ColAB
        {
            get;
            set;
        }
        [SqlParameter("ColAC"), ExcelColumn("AC"), Required]
        public double ColAC
        {
            get;
            set;
        }
        [SqlParameter("ColAD"), ExcelColumn("AD"), Required]
        public double ColAD
        {
            get;
            set;
        }
        [SqlParameter("ColAE"), ExcelColumn("AE"), Required]
        public double ColAE
        {
            get;
            set;
        }
        [SqlParameter("ColAF"), ExcelColumn("AF"), Required]
        public double ColAF
        {
            get;
            set;
        }
        [SqlParameter("ColAG"), ExcelColumn("AG"), Required]
        public double ColAG
        {
            get;
            set;
        }
        [SqlParameter("ColAH"), ExcelColumn("AH"), Required]
        public double ColAH
        {
            get;
            set;
        }
        [SqlParameter("ColAI"), ExcelColumn("AI"), Required]
        public double ColAI
        {
            get;
            set;
        }
        [SqlParameter("ColAJ"), ExcelColumn("AJ"), Required]
        public double ColAJ
        {
            get;
            set;
        }
        [SqlParameter("ColAK"), ExcelColumn("AK"), Required]
        public double ColAK
        {
            get;
            set;
        }
        [SqlParameter("ColAL"), ExcelColumn("AL"), Required]
        public double ColAL
        {
            get;
            set;
        }
        [SqlParameter("ColAM"), ExcelColumn("AM"), Required]
        public double ColAM
        {
            get;
            set;
        }
        [SqlParameter("ColAN"), ExcelColumn("AN"), Required]
        public double ColAN
        {
            get;
            set;
        }
        [SqlParameter("ColAP"), ExcelColumn("AP"), Required]
        public double ColAP
        {
            get;
            set;
        }
        [SqlParameter("ColAQ"), ExcelColumn("AQ"), Required]
        public double ColAQ
        {
            get;
            set;
        }
        [SqlParameter("ColAR"), ExcelColumn("AR"), Required]
        public double ColAR
        {
            get;
            set;
        }
        [SqlParameter("ColAS"), ExcelColumn("AS"), Required]
        public double ColAS
        {
            get;
            set;
        }
        [SqlParameter("ColAT"), ExcelColumn("AT"), Required]
        public double ColAT
        {
            get;
            set;
        }
        [SqlParameter("ColAU"), ExcelColumn("AU"), Required]
        public double ColAU
        {
            get;
            set;
        }
        [SqlParameter("ColAV"), ExcelColumn("AV"), Required]
        public double ColAV
        {
            get;
            set;
        }
        [SqlParameter("ColAW"), ExcelColumn("AW"), Required]
        public double ColAW
        {
            get;
            set;
        }
        [SqlParameter("ColAX"), ExcelColumn("AX"), Required]
        public double ColAX
        {
            get;
            set;
        }
        [SqlParameter("ColAY"), ExcelColumn("AY"), Required]
        public double ColAY
        {
            get;
            set;
        }
        [SqlParameter("ColAZ"), ExcelColumn("AZ"), Required]
        public double ColAZ
        {
            get;
            set;
        }
        [SqlParameter("ColBA"), ExcelColumn("BA"), Required]
        public double ColBA
        {
            get;
            set;
        }
        [SqlParameter("ColBB"), ExcelColumn("BB"), Required]
        public double ColBB
        {
            get;
            set;
        }
        [SqlParameter("ColBC"), ExcelColumn("BC"), Required]
        public double ColBC
        {
            get;
            set;
        }
        [SqlParameter("ColBD"), ExcelColumn("BD"), Required]
        public double ColBD
        {
            get;
            set;
        }
        [SqlParameter("ColBE"), ExcelColumn("BE"), Required]
        public double ColBE
        {
            get;
            set;
        }
    }
}
