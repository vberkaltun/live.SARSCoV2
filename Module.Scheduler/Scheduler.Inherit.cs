using FluentScheduler;
using live.SARSCoV2.Module.HttpRequest;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2.Module.Scheduler
{
    class InheritScheduler<T> : Scheduler<T>
    {
        public InheritScheduler(string path, int interval = SCHEDULED_JOB_INTERVAL)
        {
            Path = path;
            Interval = interval;

            // print message
            Extension.PrintMessage(ClassName, JobType.Initialize);

            // schedular
            Schedule<InheritTask>().NonReentrant().ToRunNow().AndEvery(Interval).Seconds();
        }
    }

    partial class Scheduler<T> : Registry, IScheduler
    {
        public class InheritTask : Task
        {
            public override async void Execute()
            {
                // print message
                Extension.PrintMessage(ClassName, JobType.Read);

                // call base
                await new InheritHttpRequest<T>().GetAsync(Path);
            }
        }
    }
}
