using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class Coordinates
    {
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; }
    }
}
