using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    struct General
    {
        [JsonProperty(PropertyName = "cases")]
        public readonly long Cases;
        
        [JsonProperty(PropertyName = "deaths")]
        public readonly long Deaths;
        
        [JsonProperty(PropertyName = "recovered")]
        public readonly long Recovered;
        
        [JsonProperty(PropertyName = "updated")]
        public readonly long Updated;
        
        [JsonProperty(PropertyName = "active")]
        public readonly long Active;
    }
}
