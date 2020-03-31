namespace live.SARSCoV2.Dataset.Sql
{
    struct ICountry
    {
      public  long ID { get; set; }
        long Updated { get; }
        string Country { get; }
        string Province { get; }
        string Content { get; }
    }
}
