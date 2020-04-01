using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    struct Statistics
    {
        [JsonProperty(PropertyName = "confirmed")]
        public long Cases { get; set; }

        [JsonProperty(PropertyName = "deaths")]
        public long Deaths { get; set; }

        [JsonProperty(PropertyName = "recovered")]
        public long Recovered { get; set; }
    }
}
