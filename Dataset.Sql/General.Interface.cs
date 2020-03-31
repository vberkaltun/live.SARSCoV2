namespace live.SARSCoV2.Dataset.Sql
{
    interface IGeneral
    {
        long ID { get; }
        string Updated { get; }
        string Content { get; }
    }
}
