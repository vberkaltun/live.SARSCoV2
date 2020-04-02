using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class States
    {
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

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
