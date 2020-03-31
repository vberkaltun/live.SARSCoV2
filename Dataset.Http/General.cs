using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    struct General
    {
        [JsonProperty(PropertyName = "cases")]
        public long Cases { get; }

        [JsonProperty(PropertyName = "deaths")]
        public long Deaths { get; }

        [JsonProperty(PropertyName = "recovered")]
        public long Recovered { get; }

        [JsonProperty(PropertyName = "updated")]
        public long Updated { get; }

        [JsonProperty(PropertyName = "active")]
        public long Active { get; }
    }
}
