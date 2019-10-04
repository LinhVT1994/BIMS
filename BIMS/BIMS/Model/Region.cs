using BIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMS.Model
{
    class Region : Element
    {

        private string area;

        [Required,ExcelColumn("F")]
        public string Prefecture
        {
            get;
            set;
        }
        [Required,ExcelColumn("G")]
        public string Ward
        {
            get;
            set;
        }
        [Required,ExcelColumn("H")]
        public string Area
        {
            get
            {
                return area;
            }
            set
            {
                area = value;
            }
        }
        public void Adjust()
        {
            if (string.IsNullOrWhiteSpace(area))
            {
                return;
            }

            var isWardIncluded = area.Contains('区');
            var isGunIncluded = area.Contains('郡');
            var isTownIncluded = area.Contains('町');
            var isMuraIncluded = area.Contains('村');
            string path1 = "";
            string path2 = "";
            int index = 0;
            int len = area.Count();
            bool found = false;

            for (int i = 0; i < len; i++)
            {
                char c = area[i];
                if (c == '区' || c == '郡' || c == '町' || c == '村')
                {
                    index = i;
                    found = true;
                    break;
                }
            }
            if (found)
            {
                int subLen = index + 1;
                if (subLen < len)
                {
                    path1 = area.Substring(0, subLen);
                    path2 = area.Substring(subLen, len - subLen);
                }
                if (!string.IsNullOrWhiteSpace(path2))
                {
                    Ward = Ward + path1;
                    Area = path2;
                }
            }
        }

    }
}
