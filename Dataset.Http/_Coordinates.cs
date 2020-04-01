using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    struct Coordinates
    {
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }
    }
}
