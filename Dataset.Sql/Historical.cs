namespace live.SARSCoV2.Dataset.Sql
{
    struct Historical
    {
        public string CountryISO2 { get; set; }
        public string CountryISO3 { get; set; }
        public string Province { get; set; }
        public string Content { get; set; }
    }
}