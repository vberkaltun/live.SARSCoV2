using Newtonsoft.Json;

namespace NovelCovidAPI.Module.Dataset
{
    public class CountryV2
    {
        [JsonProperty(PropertyName = "country")]
        public string Domain { get; set; }

        [JsonProperty(PropertyName = "countryInfo")]
        public CountryInfo DomainInfo { get; set; }

        [JsonProperty(PropertyName = "cases")]
        public long Cases { get; set; }

        [JsonProperty(PropertyName = "todayCases")]
        public long TodayCases { get; set; }

        [JsonProperty(PropertyName = "deaths")]
        public long Deaths { get; set; }

        [JsonProperty(PropertyName = "todayDeaths")]
        public long TodayDeaths { get; set; }

        [JsonProperty(PropertyName = "recovered")]
        public long Recovered { get; set; }

        [JsonProperty(PropertyName = "active")]
        public long Active { get; set; }

        [JsonProperty(PropertyName = "critical")]
        public long Critical { get; set; }

        [JsonProperty(PropertyName = "casesPerOneMillion")]
        public long CasesPerOneMillion { get; set; }

        [JsonProperty(PropertyName = "deathsPerOneMillion")]
        public long DeathsPerOneMillion { get; set; }

        [JsonProperty(PropertyName = "updated")]
        public long Updated { get; set; }
    }
}
