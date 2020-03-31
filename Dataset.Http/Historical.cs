﻿using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    struct Historical
    {
        [JsonProperty(PropertyName = "country")]
        public string Domain { get; }

        [JsonProperty(PropertyName = "province")]
        public string Province { get; }

        [JsonProperty(PropertyName = "timeline")]
        public Timeline Timeline { get; }
    }
}