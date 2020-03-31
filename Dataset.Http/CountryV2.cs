using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    struct CountryV2
    {
        [JsonProperty(PropertyName = "country")]
        public readonly string Domain;

        [JsonProperty(PropertyName = "province")]
        public readonly string Province;

        [JsonProperty(PropertyName = "updatedAt")]
        public readonly string Updated;

        [JsonProperty(PropertyName = "stats")]
        public readonly Statistics Statistics;

        [JsonProperty(PropertyName = "coordinates")]
        public readonly Coordinates Coordinates;
    }
}
