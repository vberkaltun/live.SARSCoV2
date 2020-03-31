using System.Collections.Generic;

namespace live.SARSCoV2.Dataset.Json
{
    class Timeline
    {
        public Dictionary<string, long> Cases { get; }
        public Dictionary<string, long> Deaths { get; }
        public Dictionary<string, long> Recovered { get; }
    }
}
