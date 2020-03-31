namespace live.SARSCoV2.Dataset.Json
{
    struct Country
    {
        public CountryInfo DomainInfo { get; }
        public Statistics Statistics { get; }

        public long Updated { get; }
    }
}
