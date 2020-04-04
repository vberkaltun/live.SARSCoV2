using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class State
    {
        [JsonProperty(PropertyName = "state")]
        public string Province { get; set; }

        [JsonProperty(PropertyName = "cases")]
        public long Cases { get; set; }

        [JsonProperty(PropertyName = "todayCases")]
        public long TodayCases { get; set; }

        [JsonProperty(PropertyName = "deaths")]
        public long Deaths { get; set; }

        [JsonProperty(PropertyName = "todayDeaths")]
        public long TodayDeaths { get; set; }

        [JsonProperty(PropertyName = "active")]
        public long Active { get; set; }
    }
}
