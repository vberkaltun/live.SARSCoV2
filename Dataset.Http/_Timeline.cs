using Newtonsoft.Json;
using System.Collections.Generic;

namespace live.SARSCoV2.Dataset.Http
{
    class Timeline
    {
        [JsonProperty(PropertyName = "cases")]
        public Dictionary<string, long> Cases { get; }

        [JsonProperty(PropertyName = "deaths")]
        public Dictionary<string, long> Deaths { get; }

        [JsonProperty(PropertyName = "recovered")]
        public Dictionary<string, long> Recovered { get; }
    }
}
