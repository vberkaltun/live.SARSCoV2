using Newtonsoft.Json;

namespace NovelCovidAPI.Module.Dataset
{
    public struct Coordinates
    {
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }
    }
}
