namespace live.SARSCoV2.Dataset.Sql
{
    struct Country
    {
        public long Updated { get; set; }

        public string Domain { get; set; }
        public string DomainISO2 { get; set; }
        public string DomainISO3 { get; set; }
        public string Province { get; set; }

        public string Content { get; set; }
    }
}
