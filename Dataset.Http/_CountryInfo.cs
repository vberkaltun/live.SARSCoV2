using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class CountryInfo
    {
        [JsonProperty(PropertyName = "_id")]
        public long ID { get; }

        [JsonProperty(PropertyName = "country")]
        public string Domain { get; }

        [JsonProperty(PropertyName = "iso2")]
        public string ISO2 { get; }

        [JsonProperty(PropertyName = "iso3")]
        public string ISO3 { get; }

        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; }

        [JsonProperty(PropertyName = "long")]
        public double Longitude { get; }

        [JsonProperty(PropertyName = "flag")]
        public string Flag { get; }
    }
}
