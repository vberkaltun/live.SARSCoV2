namespace live.SARSCoV2.Dataset.Json
{
    class CountryV1
    {
        public CountryInfo DomainInfo { get; set; }
        public Coordinates Coordinates { get; set; }
        public Statistics Statistics { get; set; }

        public string Updated { get; set; }
    }
}
