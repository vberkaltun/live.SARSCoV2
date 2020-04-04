namespace live.SARSCoV2.Dataset.Json
{
    class CountryV2
    {
        public CountryInfo DomainInfo { get; set; }
        public Coordinates Coordinates { get; set; }

        public Statistics Statistics { get; set; }
        public Statistics Today { get; set; }
        public Statistics PerOneMillion { get; set; }

        public string Updated { get; set; }
    }
}
