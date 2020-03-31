using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class Statistics
    {
        [JsonProperty(PropertyName = "confirmed")]
        public readonly long Cases;

        [JsonProperty(PropertyName = "deaths")]
        public readonly long Deaths;

        [JsonProperty(PropertyName = "recovered")]
        public readonly long Recovered;
    }
}
