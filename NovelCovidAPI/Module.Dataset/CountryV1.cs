using Newtonsoft.Json;

namespace NovelCovidAPI.Module.Dataset
{
    public class CountryV1
    {
        [JsonProperty(PropertyName = "country")]
        public string Domain { get; set; }

        [JsonProperty(PropertyName = "province")]
        public string Province { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string Updated { get; set; }

        [JsonProperty(PropertyName = "stats")]
        public Statistics Statistics { get; set; }

        [JsonProperty(PropertyName = "coordinates")]
        public Coordinates Coordinates { get; set; }
    }
}
