namespace live.SARSCoV2.Scheduler
{
    interface IScheduler
    {
        static string ClassName { get; }

        static string Path { get; }
        static string Interval { get; }
    }
}