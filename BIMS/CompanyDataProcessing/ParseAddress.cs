using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDataProcessing
{
    public class ParseAddress
    {
        public static string[] MatchRegions(string address)
        {
            string prefecture = "";
            string ward = "";
            string district = "";
            string moreDetail = "";

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }
            int level = 0;
            for (int i = 0; i < address.Count(); i++)
            {

                if (level == 0)
                {
                    if (address[i] == '県' ||
                      address[i] == '道' ||
                      address[i] == '都' ||
                      address[i] == '府')
                    {
                        level = 1;
                    }
                    prefecture += address[i];

                }
                else if (level == 1)
                {
                    var isCityIncluded = address.Contains('市');
                    var isWardIncluded = address.Contains('区');

                    var isGunIncluded = address.Contains('郡');
                    var isTownIncluded = address.Contains('町');
                    var isMuraIncluded = address.Contains('村');
                    char splitChar;
                    if (isCityIncluded || isWardIncluded)
                    {
                        if (isCityIncluded && isWardIncluded)
                        {
                            splitChar = '区';
                        }
                        else if (isCityIncluded)
                        {
                            splitChar = '市';
                        }
                        else
                        {
                            splitChar = '区';
                        }

                    }
                    else
                    {
                        if (isGunIncluded && isTownIncluded)
                        {
                            splitChar = '町';
                        }
                        else if (isGunIncluded && isMuraIncluded)
                        {
                            splitChar = '村';
                        }
                        else
                        {
                            splitChar = '郡';
                        }
                    }
                    if (address[i] == splitChar)
                    {
                        level = 2;
                    }
                    ward += address[i];
                }
                else if (level == 2)
                {
                    if (Char.IsNumber(address[i + 1]))
                    {
                        level = 3;
                    }
                    district += address[i];
                }
                else
                {
                    moreDetail += address[i];
                }
            }
            return new string[4]
            {
                prefecture,
                ward,
                district,
                moreDetail
            };

        }
    }
}
