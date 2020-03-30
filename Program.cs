using System;
using System.Collections.Generic;
using FluentScheduler;
using live.SARSCoV2.Dataset;
using live.SARSCoV2.HttpRequest;
using live.SARSCoV2.Scheduler;

namespace live.SARSCoV2
{
    class Program
    {
        static void Main()
        {
            Extension.SetVisibleMessage();

            // start the scheduler
            JobManager.Initialize(new InheritScheduler<General>(@"https://corona.lmao.ninja/all"));
            JobManager.Initialize(new InheritScheduler<List<Country>>(@"https://corona.lmao.ninja/countries"));
            JobManager.Initialize(new InheritScheduler<List<HistoricalV1>>(@"https://corona.lmao.ninja/v2/historical"));
            JobManager.Initialize(new InheritScheduler<List<HistoricalV2>>(@"https://corona.lmao.ninja/v2/jhucsse"));

            while (true)
            {
                if (Extension.ReadChar() != Global.EXIT_CODE)
                    continue;

                // stop the scheduler
                JobManager.StopAndBlock();
                break;
            }
        }
    }
}