using Newtonsoft.Json;
using System.Collections.Generic;

namespace live.SARSCoV2.Dataset.Http
{
    class Timeline
    {
        [JsonProperty(PropertyName = "cases")]
        public readonly Dictionary<string, long> Cases;

        [JsonProperty(PropertyName = "deaths")]
        public readonly Dictionary<string, long> Deaths;

        [JsonProperty(PropertyName = "recovered")]
        public readonly Dictionary<string, long> Recovered;
    }
}
