using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class Statistics
    {
        [JsonProperty(PropertyName = "confirmed")]
        public long Cases { get; }

        [JsonProperty(PropertyName = "deaths")]
        public long Deaths { get; }

        [JsonProperty(PropertyName = "recovered")]
        public long Recovered { get; }
    }
}
