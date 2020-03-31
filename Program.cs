using System;
using System.Collections.Generic;
using FluentScheduler;
using live.SARSCoV2.Dataset.Http;
using live.SARSCoV2.Scheduler;
using live.SARSCoV2.SqlAdapter;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2
{
    class Program
    {
         static async System.Threading.Tasks.Task Main()
        {
            // set message visible
            Extension.SetVisibleMessage();
            // show main app informations
            Extension.PrintAppInfo();

            // start the scheduler
            JobManager.Initialize(new InheritScheduler<General>(@"https://corona.lmao.ninja/all"));
            JobManager.Initialize(new InheritScheduler<List<Country>>(@"https://corona.lmao.ninja/countries"));
            JobManager.Initialize(new InheritScheduler<List<Historical>>(@"https://corona.lmao.ninja/v2/jhucsse"));

            InheritSqlAdapter adapter = new InheritSqlAdapter();
            await adapter.ConnectAsync();

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