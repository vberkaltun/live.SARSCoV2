﻿namespace live.SARSCoV2.Dataset.Json
{
    class Country
    {
        public CountryInfo DomainInfo { get; set; }
        public Coordinates Coordinates { get; set; }
        public Statistics Statistics { get; set; }

        public long Updated { get; set; }
    }
}
