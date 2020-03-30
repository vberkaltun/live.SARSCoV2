using Newtonsoft.Json;
using System.Collections.Generic;

namespace live.SARSCoV2.Dataset
{
    struct HistoricalV1
    {
        [JsonProperty(PropertyName = "country")]
        public readonly string Domain;

        [JsonProperty(PropertyName = "province")]
        public readonly string Province;

        [JsonProperty(PropertyName = "timeline")]
        public readonly Timeline Timeline;
    }

    struct Timeline
    {
        [JsonProperty(PropertyName = "cases")]
        public readonly Dictionary<string, long> Cases;

        [JsonProperty(PropertyName = "deaths")]
        public readonly Dictionary<string, long> Deaths;
    }
}
