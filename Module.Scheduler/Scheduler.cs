using System;
using FluentScheduler;
using live.SARSCoV2.Module.Base;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2.Module.Scheduler
{
    class Scheduler : Logger, IScheduler
    {
        #region Properties

        public static string ClassName => typeof(Scheduler).FullName;

        public Guid ID { get; private set; } = Guid.NewGuid();
        public int Interval { get; private set; }
        public bool Autostart { get; private set; }

        #endregion

        #region Delegate

        private event Action Executed;

        #endregion

        #region Methods

        public Scheduler(Action executed = null, int interval = SCHEDULED_JOB_INTERVAL, bool autostart = true)
        {
            // print message
            PrintMessage(ClassName, JobType.Initialize);

            Interval = interval;
            Executed = executed;
            Autostart = autostart;

            // run auto
            if (Autostart) Schedule();
        }

        public void Schedule()
        {
            // print message
            PrintMessage(ClassName, JobType.Read);

            // schedular
            Schedule(() => Executed?.Invoke()).WithName(ID.ToString()).NonReentrant().ToRunNow().AndEvery(Interval).Seconds();
        }
        public void Terminate()
        {
            // print message
            PrintMessage(ClassName, JobType.Error);

            // remove task
            JobManager.RemoveJob(ID.ToString());
        }

        #endregion
    }
}
