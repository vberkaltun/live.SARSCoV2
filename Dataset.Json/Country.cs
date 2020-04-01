namespace live.SARSCoV2.Dataset.Json
{
    struct Country
    {
        public CountryInfo DomainInfo { get; set; }
        public Statistics Statistics { get; set; }

        public long Updated { get; set; }
    }
}
