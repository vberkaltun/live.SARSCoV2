namespace live.SARSCoV2.Dataset.Sql
{
    struct CountryV1
    {
        public readonly CountryInfoV3 DomainInfo;
        public readonly StatisticsV2 Statistics;
        public readonly StatisticsV1 PerOneMillion;
        public readonly StatisticsV1 Today;

        public readonly long Updated;
    }
}
