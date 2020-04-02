using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class Province
    {
        [JsonProperty(PropertyName = "country")]
        public string Domain { get; set; }

        [JsonProperty(PropertyName = "province")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string Updated { get; set; }

        [JsonProperty(PropertyName = "stats")]
        public Statistics Statistics { get; set; }

        [JsonProperty(PropertyName = "coordinates")]
        public Coordinates Coordinates { get; set; }
    }
}
