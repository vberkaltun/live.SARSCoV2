using FluentScheduler;

namespace live.SARSCoV2.Module.Base
{
    class Base : Registry, IBase
    {
        public Logger Logger { get; }
    }
}
