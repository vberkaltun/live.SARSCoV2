using System.Collections.Generic;

namespace live.SARSCoV2.Dataset.Json
{
    struct Timeline
    {
        public Dictionary<string, long> Cases { get; set; }
        public Dictionary<string, long> Deaths { get; set; }
        public Dictionary<string, long> Recovered { get; set; }
    }
}
