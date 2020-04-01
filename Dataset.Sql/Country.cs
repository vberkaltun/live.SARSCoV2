namespace live.SARSCoV2.Dataset.Sql
{
    struct Country
    {
        public long Updated { get; }
        public string CountryISO2 { get; }
        public string CountryISO3 { get; }
        public string Province { get; }
        public string Content { get; }
    }
}
