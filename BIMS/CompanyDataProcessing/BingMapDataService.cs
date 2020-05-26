using CompanyDataProcessing.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDataProcessing
{
    public class BingMapDataService
    {
        private string bingMapsKey = "OWZi0pIKOrV2FXqT0xnd~cMagH83n9x6F9hPdRUv8iA~AidhQReCw3RefM1-LpOK05qmbf6Ayoim47JZwwYE-N7EyFcQiw2QxQZQd67FEgqs";
        private string jpRegionParameters = "o=json&c=ja";
        private string locationAPI = "http://dev.virtualearth.net/REST/v1/Locations/";
        public BingMapDataService()
        {

        }
        public RootObject SearchLocationByQuery(string query)
        {
            string requestUrl = locationAPI + "?query=" + query + "&" + jpRegionParameters + "&key=" + bingMapsKey;
            RootObject rs = GetJsonResponse(requestUrl);
            return rs;
        }
        public RootObject SearchLocationSync(double latitude, double longitude)
        {
            string requestUrl = locationAPI + latitude + "," + longitude + "?" + jpRegionParameters + "&key=" + bingMapsKey;
            RootObject rs = GetJsonResponse(requestUrl);
            return rs;
        }
        private string FormatAddressQuery(RegionModel region, int type = 0)
        {
            return region.FullAddress;
        }

        public RootObject SearchLocation(double latitude, double longitude)
        {
            string requestUrl = locationAPI + latitude + "," + longitude + "?" + jpRegionParameters + "&key=" + bingMapsKey;
            RootObject rs = GetJsonResponse(requestUrl);
            return rs;
        }
        public RootObject SearchLocation(string fulladdress)
        {
            if (string.IsNullOrWhiteSpace(fulladdress))
            {
                return null;
            }
            string addressParametersString = "q=" + fulladdress;
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("{0}?{1}&{2}&key={3}", locationAPI, addressParametersString, jpRegionParameters, bingMapsKey);

            string requestUrl = queryBuilder.ToString();
            return GetJsonResponse(requestUrl);
        }
        public RootObject SearchLocation(RegionModel region, int type = 0)
        {
            string countryRegion = "jp";
            string addressParametersString = FormatAddressQuery(region, type);
            if (type == 1)
            {
                addressParametersString = "q=" + addressParametersString;
            }

            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("{0}?{1}&{2}&key={3}", locationAPI, addressParametersString, jpRegionParameters, bingMapsKey);

            string requestUrl = queryBuilder.ToString();
            return GetJsonResponse(requestUrl);
        }
        private RootObject GetJsonResponse(string requestUrl)
        {
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var json = reader.ReadToEnd();

                    return JsonConvert.DeserializeObject<RootObject>(json);
                }
            }
        }
    }
}