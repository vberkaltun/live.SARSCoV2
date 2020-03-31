using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    struct Country
    {
        [JsonProperty(PropertyName = "country")]
        public string Domain { get; }

        [JsonProperty(PropertyName = "province")]
        public string Province { get; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string Updated { get; }

        [JsonProperty(PropertyName = "stats")]
        public Statistics Statistics { get; }

        [JsonProperty(PropertyName = "coordinates")]
        public Coordinates Coordinates { get; }
    }
}
