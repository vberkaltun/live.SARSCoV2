namespace live.SARSCoV2.Dataset.Sql
{
    struct CountryV2
    {
        public readonly CountryInfoV3 DomainInfo;
        public readonly StatisticsV2 Statistics;

        public readonly long Updated;
    }
}
