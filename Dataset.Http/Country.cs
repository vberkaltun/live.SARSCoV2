﻿using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class Country
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
}