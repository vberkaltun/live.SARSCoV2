using System;

namespace live.SARSCoV2.Module.Scheduler
{
    interface IScheduler
    {
        Guid ID { get; }
        int Interval { get; }
        bool Autostart { get; }

        void Schedule();
        void Terminate();
    }
}