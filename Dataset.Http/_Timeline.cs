﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace live.SARSCoV2.Dataset.Http
{
    struct Timeline
    {
        [JsonProperty(PropertyName = "cases")]
        public Dictionary<string, long> Cases { get; set; }

        [JsonProperty(PropertyName = "deaths")]
        public Dictionary<string, long> Deaths { get; set; }

        [JsonProperty(PropertyName = "recovered")]
        public Dictionary<string, long> Recovered { get; set; }
    }
}
