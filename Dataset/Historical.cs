using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset
{
    struct Historical
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

    struct Coordinates
    {
        [JsonProperty(PropertyName = "latitude")]
        public readonly double Latitude;

        [JsonProperty(PropertyName = "longitude")]
        public readonly double Longitude;
    }

    struct Statistics
    {
        [JsonProperty(PropertyName = "confirmed")]
        public readonly long Cases;

        [JsonProperty(PropertyName = "deaths")]
        public readonly long Deaths;

        [JsonProperty(PropertyName = "recovered")]
        public readonly long Recovered;
    }
}
