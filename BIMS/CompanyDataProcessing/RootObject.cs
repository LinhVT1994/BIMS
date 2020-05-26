using Newtonsoft.Json;
using System.Collections.Generic;

namespace CompanyDataProcessing
{
    public class Point
    {
        [JsonProperty(PropertyName = "type")]
        public string type { get; set; }
        [JsonProperty(PropertyName = "coordinates")]
        public List<double> coordinates { get; set; }
    }

    public class Address
    {
        [JsonProperty(PropertyName = "adminDistrict")]
        public string AdminDistrict { get; set; }
        [JsonProperty(PropertyName = "countryRegion")]
        public string CountryRegion { get; set; }
        [JsonProperty(PropertyName = "formattedAddress")]
        public string FormattedAddress { get; set; }
        [JsonProperty(PropertyName = "locality")]
        public string Locality { get; set; }
        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }
        [JsonProperty(PropertyName = "addressLine")]
        public string AddressLine { get; set; }
    }

    public class GeocodePoint
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "coordinates")]
        public List<double> Coordinates { get; set; }
        [JsonProperty(PropertyName = "calculationMethod")]
        public string CalculationMethod { get; set; }
        [JsonProperty(PropertyName = "usageTypes")]
        public List<string> UsageTypes { get; set; }
    }

    public class Resource
    {
        [JsonProperty(PropertyName = "bbox")]
        public List<double> Bbox { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "point")]
        public Point Point { get; set; }
        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }
        [JsonProperty(PropertyName = "confidence")]
        public string Confidence { get; set; }
        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }
        [JsonProperty(PropertyName = "geocodePoints")]
        public List<GeocodePoint> GeocodePoints { get; set; }
        [JsonProperty(PropertyName = "matchCodes")]
        public List<string> MatchCodes { get; set; }
    }

    public class ResourceSet
    {
        [JsonProperty(PropertyName = "estimatedTotal")]
        public int EstimatedTotal { get; set; }
        [JsonProperty(PropertyName = "resources")]
        public List<Resource> Resources { get; set; }
    }

    public class RootObject
    {
        [JsonProperty(PropertyName = "authenticationResultCode")]
        public string AuthenticationResultCode { get; set; }
        [JsonProperty(PropertyName = "BrandLogoUri")]
        public string BrandLogoUri { get; set; }
        [JsonProperty(PropertyName = "copyright")]
        public string Copyright { get; set; }
        [JsonProperty(PropertyName = "resourceSets")]
        public List<ResourceSet> ResourceSets { get; set; }
        [JsonProperty(PropertyName = "statusCode")]
        public int StatusCode { get; set; }
        [JsonProperty(PropertyName = "statusDescription")]
        public string StatusDescription { get; set; }
        [JsonProperty(PropertyName = "traceId")]
        public string TraceId { get; set; }
    }
}