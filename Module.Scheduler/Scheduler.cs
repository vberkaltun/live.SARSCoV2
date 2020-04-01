using System;
using FluentScheduler;
using live.SARSCoV2.Module.Base;

namespace live.SARSCoV2.Module.Scheduler
{
    class Scheduler : BaseMember, IScheduler
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

        public Scheduler(Action executed, int interval, bool autostart = true)
        {
            // print message
            Logger.Initialize(ClassName);

            Interval = interval;
            Executed = executed;
            Autostart = autostart;

            // run auto
            if (Autostart) Schedule();
        }

        public void Schedule()
        {
            // print message
            Logger.Read(ClassName);

            // schedular
            Schedule(() => Executed?.Invoke()).WithName(ID.ToString()).NonReentrant().ToRunNow().AndEvery(Interval).Seconds();
        }
        public void Terminate()
        {
            // print message
            Logger.Error(ClassName);

            // remove task
            JobManager.RemoveJob(ID.ToString());
        }

        #endregion
    }
}
