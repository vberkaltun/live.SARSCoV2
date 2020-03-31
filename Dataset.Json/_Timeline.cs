using System.Collections.Generic;

namespace live.SARSCoV2.Dataset.Json
{
    class Timeline
    {
        public readonly Dictionary<string, long> Cases;
        public readonly Dictionary<string, long> Deaths;
        public readonly Dictionary<string, long> Recovered;
    }
}
