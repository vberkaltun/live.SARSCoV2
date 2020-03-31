using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class CountryInfo
    {
        [JsonProperty(PropertyName = "_id")]
        public readonly long ID;

        [JsonProperty(PropertyName = "country")]
        public readonly string Domain;

        [JsonProperty(PropertyName = "iso2")]
        public readonly string ISO2;

        [JsonProperty(PropertyName = "iso3")]
        public readonly string ISO3;

        [JsonProperty(PropertyName = "lat")]
        public readonly double Latitude;

        [JsonProperty(PropertyName = "long")]
        public readonly double Longitude;

        [JsonProperty(PropertyName = "flag")]
        public readonly string Flag;
    }
}
