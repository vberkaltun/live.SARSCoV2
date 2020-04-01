﻿using Newtonsoft.Json;

namespace live.SARSCoV2.Dataset.Http
{
    class Historical
    {
        [JsonProperty(PropertyName = "country")]
        public string Domain { get; set; }

        [JsonProperty(PropertyName = "province")]
        public string Province { get; set; }

        [JsonProperty(PropertyName = "timeline")]
        public Timeline Timeline { get; set; }
    }
}