using System;
using System.Collections.Generic;
using FluentScheduler;
using live.SARSCoV2.Dataset.Http;
using live.SARSCoV2.Module.Scheduler;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2
{
    class Program
    {
        static void Main()
        {
            // set message visible
            Extension.SetVisibleMessage();
            // show main app informations
            Extension.PrintAppInfo();

            // start the scheduler
            JobManager.Initialize(new InheritScheduler<General>(@"https://corona.lmao.ninja/all"));
            JobManager.Initialize(new InheritScheduler<List<Country>>(@"https://corona.lmao.ninja/v2/jhucsse"));
            JobManager.Initialize(new InheritScheduler<List<Historical>>(@"https://corona.lmao.ninja/v2/historical"));

            while (true)
            {
                if (Extension.ReadChar() != EXIT_CODE)
                    continue;

                // stop the scheduler
                JobManager.StopAndBlock();
                break;
            }
        }
    }
}