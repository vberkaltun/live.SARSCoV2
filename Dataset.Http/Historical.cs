﻿using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    struct Historical
    {
        [JsonProperty(PropertyName = "country")]
        public readonly string Domain;

        [JsonProperty(PropertyName = "province")]
        public readonly string Province;

        [JsonProperty(PropertyName = "timeline")]
        public readonly Timeline Timeline;
    }
}