using System;
using System.Collections.Generic;
using FluentScheduler;
using live.SARSCoV2.Dataset.Http;
using live.SARSCoV2.Module.Base;
using live.SARSCoV2.Module.HttpRequest;
using live.SARSCoV2.Module.Scheduler;
using static live.SARSCoV2.Global;

namespace live.SARSCoV2
{
    class Program : Logger
    {
        static void Main() => new SARSCoV2();
    }

    class SARSCoV2 : Logger
    {
        public SARSCoV2()
        {
            // set message visible
            SetVisibleMessage();
            // show main app informations
            PrintAppInfo();

            // start the scheduler
            JobManager.Initialize(new Scheduler(TaskGeneralAsync));
            JobManager.Initialize(new Scheduler(TaskCountry));
            JobManager.Initialize(new Scheduler(TaskHistorical));

            while (true)
            {
                if (ReadChar() != EXIT_CODE)
                    continue;

                // stop the scheduler
                JobManager.StopAndBlock();
                break;
            }
        }

        #region Methods

        public async void TaskGeneralAsync()
        {
            await new HttpRequest<General>().GetAsync(@"https://corona.lmao.ninja/all");
        }

        public async void TaskCountry()
        {
            await new HttpRequest<List<Country>>().GetAsync(@"https://corona.lmao.ninja/v2/jhucsse");
        }

        public async void TaskHistorical()
        {
            await new HttpRequest<List<Historical>>().GetAsync(@"https://corona.lmao.ninja/v2/historical");
        }

        public void PrintAppInfo()
        {
            PrintMessage(string.Format("{0} {1}",
                APP_NAME, APP_VERSION), JobType.Informational);

            PrintMessage(string.Format("Exit code: {0}, Interval: {1}, Null Value Handling: {2}",
                EXIT_CODE, SCHEDULED_JOB_INTERVAL, NULL_VALUE_HANDLING), JobType.Informational);
        }

        #endregion
    }
}