using FluentScheduler;
using live.SARSCoV2.HttpRequest;

namespace live.SARSCoV2.Scheduler
{
    class InheritScheduler<T> : Scheduler<T>
    {
        public InheritScheduler(string path, int interval = Global.SCHEDULED_JOB_INTERVAL)
        {
            Path = path;
            Interval = interval;

            // print message
            Extension.PrintMessage(ClassName, Global.JobType.Scheduled);

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
                Extension.PrintMessage(ClassName, Global.JobType.Executed);

                // call base
                await new InheritHttpRequest<T>().GetAsync(Path);
            }
        }
    }
}
