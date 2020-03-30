using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset
{
    struct Country
    {
        [JsonProperty(PropertyName = "country")]
        public readonly string Domain;

        [JsonProperty(PropertyName = "countryInfo")]
        public readonly CountryInfo DomainInfo;

        [JsonProperty(PropertyName = "cases")]
        public readonly long Cases;

        [JsonProperty(PropertyName = "todayCases")]
        public readonly long TodayCases;

        [JsonProperty(PropertyName = "deaths")]
        public readonly long Deaths;

        [JsonProperty(PropertyName = "todayDeaths")]
        public readonly long TodayDeaths;

        [JsonProperty(PropertyName = "recovered")]
        public readonly long Recovered;

        [JsonProperty(PropertyName = "active")]
        public readonly long Active;

        [JsonProperty(PropertyName = "critical")]
        public readonly long Critical;

        [JsonProperty(PropertyName = "casesPerOneMillion")]
        public readonly long CasesPerOneMillion;

        [JsonProperty(PropertyName = "deathsPerOneMillion")]
        public readonly long DeathsPerOneMillion;
        
        [JsonProperty(PropertyName = "updated")]
        public readonly long Updated;
    }

    struct CountryInfo
    {
        [JsonProperty(PropertyName = "_id")]
        public readonly long ID;

        [JsonProperty(PropertyName = "country")]
        public readonly string Domain;

        [JsonProperty(PropertyName = "iso2")]
        public readonly string ISO2;

        [JsonProperty(PropertyName = "iso3")]
        public readonly string ISO3;

        [JsonProperty(PropertyName = "lat")]
        public readonly double Latitude;

        [JsonProperty(PropertyName = "long")]
        public readonly double Longitude;

        [JsonProperty(PropertyName = "flag")]
        public readonly string Flag;
    }
}
