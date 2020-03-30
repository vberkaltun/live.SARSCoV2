namespace live.SARSCoV2.Dataset.Sql
{
    struct Country
    {
        public readonly CountryInfo DomainInfo;
        public readonly PerOneMillion PerOneMillion;
        public readonly Today Today;

        public readonly long Updated;
    }
}
