using System;
using System.Collections.Generic;
using FluentScheduler;
using live.SARSCoV2.Dataset.Http;
using live.SARSCoV2.Scheduler;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2
{
    class Program
    {
        static void Main()
        {
            // show message
            Extension.SetVisibleMessage();

            // show main app informations
            printAppInfo();

            // start the scheduler
            JobManager.Initialize(new InheritScheduler<General>(@"https://corona.lmao.ninja/all"));
            JobManager.Initialize(new InheritScheduler<List<Country>>(@"https://corona.lmao.ninja/countries"));
            JobManager.Initialize(new InheritScheduler<List<Historical>>(@"https://corona.lmao.ninja/v2/jhucsse"));

            while (true)
            {
                if (Extension.ReadChar() != EXIT_CODE)
                    continue;

                // stop the scheduler
                JobManager.StopAndBlock();
                break;
            }
        }

        static void printAppInfo()
        {
            Extension.PrintMessage(string.Format("{0} {1}",
                APP_NAME, APP_VERSION, USER_NAME), JobType.Informational);

            Extension.PrintMessage(string.Format("Exit code: {0}, Interval: {1}, Null Value Handling: {2}",
                EXIT_CODE, SCHEDULED_JOB_INTERVAL, NULL_VALUE_HANDLING), JobType.Informational);
        }
    }
}