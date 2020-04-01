using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class General
    {
        [JsonProperty(PropertyName = "cases")]
        public long Cases { get; set; }

        [JsonProperty(PropertyName = "deaths")]
        public long Deaths { get; set; }

        [JsonProperty(PropertyName = "recovered")]
        public long Recovered { get; set; }

        [JsonProperty(PropertyName = "updated")]
        public long Updated { get; set; }

        [JsonProperty(PropertyName = "active")]
        public long Active { get; set; }

        [JsonProperty(PropertyName = "affectedCountries")]
        public long AffectedCountries { get; set; }
    }
}