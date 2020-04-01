using FluentScheduler;

namespace live.SARSCoV2.Module.Base
{
    class BaseMember : Registry, IBaseMember
    {
        public Logger Logger => new Logger();
    }
}
