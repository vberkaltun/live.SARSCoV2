using FluentScheduler;
using live.SARSCoV2.Module.HttpRequest;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2.Module.Scheduler
{
    partial class Scheduler<T> : Registry, IScheduler
    {
        #region Properties

        public static string ClassName => typeof(T).FullName;

        public static string Path { get; protected set; }
        public static int Interval { get; protected set; }

        #endregion

        #region Methods

        public Scheduler() { }
        public Scheduler(string path, int interval = SCHEDULED_JOB_INTERVAL)
        {
            Path = path;
            Interval = interval;

            // schedular
            Schedule<Task>().NonReentrant().ToRunNow().AndEvery(Interval).Seconds();
        }

        #endregion

        #region Subclasses

        public class Task : IJob
        {
            public virtual async void Execute() => await new HttpRequest<T>().GetAsync(Path);
        }

        #endregion
    }
}
