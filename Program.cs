﻿using System;
using System.Collections.Generic;
using System.Reflection;
using FluentScheduler;
using live.SARSCoV2.Dataset;
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

        static void printAppInfo()
        {
            Extension.PrintMessage(string.Format("{0} {1}",
                Assembly.GetExecutingAssembly().GetName().Name,
                Assembly.GetExecutingAssembly().GetName().Version), JobType.Informational);
            
            Extension.PrintMessage(string.Format("Exit code: {0}, Interval: {1}",
                EXIT_CODE, SCHEDULED_JOB_INTERVAL), JobType.Informational);
        }
    }
}