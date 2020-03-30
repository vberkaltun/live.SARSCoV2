using FluentScheduler;
using live.SARSCoV2.HttpRequest;

namespace live.SARSCoV2.Scheduler
{
    partial class Scheduler<T> : Registry, IScheduler
    {
        #region Properties

        public static string ClassName => typeof(T).FullName;

        public static string Path { get; set; }
        public static int Interval { get; set; }

        #endregion

        #region Methods

        public Scheduler() { }
        public Scheduler(string path, int interval = Global.SCHEDULED_JOB_INTERVAL)
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
            public virtual async void Execute()
            {
                await new HttpRequest<T>().GetAsync(Path);
            }
        }

        #endregion
    }
}
