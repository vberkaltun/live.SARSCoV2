namespace live.SARSCoV2.Dataset.Sql
{
    interface IHistorical
    {
        long ID { get; }
        string Country { get; }
        string Province { get; }
        string Content { get; }
    }
}