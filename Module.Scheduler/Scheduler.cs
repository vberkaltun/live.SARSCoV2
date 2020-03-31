using live.SARSCoV2.Module.Base;
using live.SARSCoV2.Module.HttpRequest;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2.Module.Scheduler
{
    class Scheduler<T> : Logger, IScheduler
    {
        #region Properties

        public string ClassName => typeof(T).FullName;

        public string Path { get; private set; }
        public int Interval { get; private set; }

        #endregion

        #region Methods

        public Scheduler(string path, int interval = SCHEDULED_JOB_INTERVAL)
        {
            // print message
            PrintMessage(ClassName, JobType.Initialize);

            Path = path;
            Interval = interval;
        }

        public virtual void ScheduleAsync()
        {
            // print message
            PrintMessage(ClassName, JobType.Read);

            // schedular
            Schedule(async () => await new HttpRequest<T>().GetAsync(Path)).NonReentrant().ToRunNow().AndEvery(Interval).Seconds();
        }

        #endregion
    }
}
