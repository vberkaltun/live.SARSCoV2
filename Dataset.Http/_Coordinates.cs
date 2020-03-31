using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class Coordinates
    {
        [JsonProperty(PropertyName = "latitude")]
        public readonly double Latitude;

        [JsonProperty(PropertyName = "longitude")]
        public readonly double Longitude;
    }
}
