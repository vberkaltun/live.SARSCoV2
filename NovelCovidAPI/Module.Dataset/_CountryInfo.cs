using Newtonsoft.Json;

namespace NovelCovidAPI.Module.Dataset
{
    public struct CountryInfo
    {
        [JsonProperty(PropertyName = "_id")]
        public long ID { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Domain { get; set; }

        [JsonProperty(PropertyName = "iso2")]
        public string ISO2 { get; set; }

        [JsonProperty(PropertyName = "iso3")]
        public string ISO3 { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "long")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "flag")]
        public string Flag { get; set; }
    }
}
